Imports System.Runtime.InteropServices

Module ClipBoardHelper

    Public clipBoardInfo As ClipboardFileInfo

    Private _ListeningFormHand As IntPtr

    Public Sub registerClipListener()
        If AddClipboardFormatListener(ScalaHandle) Then _ListeningFormHand = ScalaHandle
    End Sub

    Public Sub unregisterClipListener()
        If RemoveClipboardFormatListener(_ListeningFormHand) Then _ListeningFormHand = Nothing
    End Sub



    Public Sub SetFileDropListWithEffect(ByVal file As String, ByVal isCut As Boolean)
        Try

            Dim data As New DataObject()
            Dim files As New Specialized.StringCollection From {file}

            data.SetFileDropList(files)

            data.SetData("Preferred DropEffect", New IO.MemoryStream(BitConverter.GetBytes(If(isCut, 2, 1))))

            Clipboard.SetDataObject(data, True)
            dBug.Print("Clipboard set: " & If(isCut, "Cut", "Copy"))
        Catch ex As Exception
            dBug.Print("Clipboard error: " & ex.Message)
        End Try

    End Sub

    Public Enum ClipboardAction
        None = 0
        Move = 2
        Cut = 2
        Copy = 5
    End Enum

    Public Structure ClipboardFileInfo
        Public Files As List(Of String)
        Public Action As ClipboardAction
    End Structure

    Public Function GetClipboardFilesAndAction() As ClipboardFileInfo
        Dim result As New ClipboardFileInfo With {
            .Files = New List(Of String),
            .Action = ClipboardAction.None
        }

        result.Files = Clipboard.GetFileDropList().Cast(Of String)().ToList

        Dim dataObj = Clipboard.GetDataObject()
        If dataObj IsNot Nothing Then
            'Dim dropEffectObj = dataObj.GetData(DataFormats.GetFormat(PreferredDropEffect).Name)
            Dim dropEffectObj = dataObj.GetData("Preferred DropEffect")
            'Debug.Print($"""{dropEffectObj?.GetType()}""")
            If TypeOf dropEffectObj Is IO.MemoryStream Then
                Dim mes = DirectCast(dropEffectObj, IO.MemoryStream)
                If mes.Length >= 4 Then
                    Dim bytes(3) As Byte
                    mes.Position = 0
                    mes.Read(bytes, 0, 4)
                    Dim effect = BitConverter.ToInt32(bytes, 0)
                    If effect = 2 Then
                        result.Action = ClipboardAction.Move
                    ElseIf effect = 5 Then
                        result.Action = ClipboardAction.Copy
                    End If
                End If
            End If
        End If

#If DEBUG Then
        Dim count As Integer = result.Files?.Count
        dBug.Print($"Clipboard Contains: {count} File{If(count = 1, "", "s")}, Action: {result.Action}")
        For Each it As String In result.Files
            dBug.Print($"""{it}""")
        Next
        dBug.Print("---")
#End If

        Return result

    End Function

    Public Sub InvokeExplorerVerb(filePath As String, verb As String, Optional hwnd As IntPtr = Nothing)
        Dim exec As New SHELLEXECUTEINFO()
        exec.cbSize = Marshal.SizeOf(exec)
        exec.fMask = SEE_MASK_INVOKEIDLIST
        exec.hwnd = hwnd
        exec.lpVerb = verb   ' canonical verb: "cut", "copy", "paste", "pastelink", "delete", "properties"
        exec.lpFile = filePath
        exec.nShow = SW_HIDE

        If Not ShellExecuteEx(exec) Then
            Debug.Print(New ComponentModel.Win32Exception(Marshal.GetLastWin32Error()).Message)
        End If
    End Sub


End Module
