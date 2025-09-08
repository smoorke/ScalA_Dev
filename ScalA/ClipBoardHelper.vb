Imports System.Runtime.InteropServices
Imports System.Threading

Module ClipBoardHelper

    Public clipBoardInfo As ClipboardFileInfo

    Public Sub dupeClipBoard()
        Dim hw As IntPtr = GetClipboardOwner()
        Dim pid As Integer
        GetWindowThreadProcessId(hw, pid)
        Debug.Print($"dupeClipBoard is b0rken {pid = FrmMain.scalaPID}")
        If pid = FrmMain.scalaPID Then
            HybridClipboardPersist()
        End If
    End Sub

    Public Declare Function OleGetClipboard Lib "ole32.dll" (ByRef ppDataObj As IntPtr) As Integer
    Public Declare Function OleFlushClipboard Lib "ole32.dll" () As Integer
    Public Declare Function ReleaseStgMedium Lib "ole32.dll" (ByRef pmedium As STGMEDIUM) As Integer

    ' COM IDataObject (ole32 version, not WinForms!)
    <ComImport, Guid("0000010e-0000-0000-C000-000000000046"),
 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IOleDataObject
        Sub GetData(ByRef formatetc As FORMATETC, ByRef medium As STGMEDIUM)
        Sub GetDataHere(ByRef formatetc As FORMATETC, ByRef medium As STGMEDIUM)
        Sub QueryGetData(ByRef formatetc As FORMATETC)
        Sub GetCanonicalFormatEtc(ByRef formatetcIn As FORMATETC, ByRef formatetcOut As FORMATETC)
        Sub SetData(ByRef formatetc As FORMATETC, ByRef medium As STGMEDIUM, fRelease As Boolean)
        Sub EnumFormatEtc(dwDirection As Integer, ByRef enumFormatEtc As Object)
        Sub DAdvise(ByRef formatetc As FORMATETC, advf As Integer, pAdvSink As Object, ByRef connection As Integer)
        Sub DUnadvise(dwConnection As Integer)
        Sub EnumDAdvise(ByRef enumAdvise As Object)
    End Interface
    <StructLayout(LayoutKind.Sequential)>
    Public Structure FORMATETC
        Public cfFormat As Short
        Public ptd As IntPtr
        Public dwAspect As Integer
        Public lindex As Integer
        Public tymed As Integer
    End Structure
    <StructLayout(LayoutKind.Sequential)>
    Public Structure STGMEDIUM
        Public tymed As Integer
        Public unionmember As IntPtr
        Public pUnkForRelease As IntPtr
    End Structure

    Public Sub HybridClipboardPersist()
        Dim pDataObj As IntPtr
        Try
            ' 1. Get the raw IDataObject pointer from Explorer
            pDataObj = RetryOleGetClipboard()

            If pDataObj = IntPtr.Zero Then Exit Sub

            ' 2. Wrap it in a managed IDataObject
            Dim srcDataObj As IOleDataObject = CType(Marshal.GetObjectForIUnknown(pDataObj), IOleDataObject)

            ' 3. Convert into a WinForms DataObject (our own managed copy)
            Dim newData As New DataObject()

            Dim shellFormatsToSkip As New HashSet(Of String) From {
    "AsyncFlag",
    "Shell IDList Array",
    "DataObjectAttributes",
    "DataObjectAttributesRequiringElevation",
    "FileGroupDescriptorW"
}
            Dim formatsToCopy As New HashSet(Of String) From {
    "FileDrop",
    "FileNameW",
    "FileName",
    "Preferred DropEffect",
    "Shell IDList Array"
}
            ' At this point you’d enumerate formats via EnumFormatEtc on IOleDataObject,
            ' then copy into newData. For simplicity we instead grab WinForms view of clipboard:
            Dim managedCopy As Windows.Forms.IDataObject = Clipboard.GetDataObject()
            If managedCopy IsNot Nothing Then
                For Each fmt As String In managedCopy.GetFormats(True)
                    Try
                        Dim data = managedCopy.GetData(fmt, True)
                        dBug.Print($"Duping {fmt} {data?.GetType}")
                        If data IsNot Nothing Then
                            If formatsToCopy.Contains(fmt) Then
                                newData.SetData(fmt, data)
                            End If
                        End If
                    Catch ex As Exception
                        Debug.Print($"Could not copy format {fmt}: {ex.Message}")
                    End Try
                Next
            End If

            ' 4. Set clipboard with our copy, making us the owner
            Clipboard.SetDataObject(newData, True)

            ' 5. Flush so Windows makes it permanent
            Dim ret = RetryOleFlushClipboard()
            Debug.Print($"OleFlushClipboard success={ret}")

        Catch ex As Exception
            Debug.Print("HybridClipboardPersist error: " & ex.Message)
        Finally
            If pDataObj <> IntPtr.Zero Then Marshal.Release(pDataObj)
        End Try
    End Sub

    Private Function RetryOleGetClipboard(Optional maxRetries As Integer = 5, Optional delayMs As Integer = 50) As IntPtr
        Dim pDataObj As IntPtr = IntPtr.Zero
        Dim attempt As Integer = 0

        Do While attempt <maxRetries
            Dim hr As Integer = OleGetClipboard(pDataObj)
            If hr = 0 AndAlso pDataObj <> IntPtr.Zero Then
                Return pDataObj ' success
            End If
            attempt += 1
            Thread.Sleep(delayMs)
        Loop

        Return IntPtr.Zero ' failed after retries
    End Function

    Private Function RetryOleFlushClipboard(Optional maxRetries As Integer = 5, Optional delayMs As Integer = 50) As Boolean
        Dim attempt As Integer = 0

        Do While attempt < maxRetries
            Dim hr As Integer = OleFlushClipboard()
            If hr = 0 Then Return True
            attempt += 1
            Thread.Sleep(delayMs)
        Loop

        Return False
    End Function

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
