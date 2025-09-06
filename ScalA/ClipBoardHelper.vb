Imports System.Runtime.InteropServices

Module ClipBoardHelper

    Public clipBoardInfo As ClipboardFileInfo

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
            Debug.Print($"""{dropEffectObj?.GetType()}""")
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
