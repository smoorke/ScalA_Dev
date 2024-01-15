Imports System.IO.MemoryMappedFiles
Imports System.Runtime.InteropServices
Module IPC
    'Private ReadOnly _mmfBoolean As MemoryMappedFile = MemoryMappedFile.CreateOrOpen("ScalA_IPC_Boolean", 1)
    'Private ReadOnly _mmvaBoolean As MemoryMappedViewAccessor = _mmfBoolean.CreateViewAccessor()
    'Public Property QlCtxIsOpen As Boolean
    '    Get
    '        'Debug.Print(_mmvaBoolean.ReadBoolean(0))
    '        Return _mmvaBoolean.ReadBoolean(0)
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _mmvaBoolean.Write(0, value)
    '        Debug.Print($"set QlCtxOpen to {value}")
    '    End Set
    'End Property

    Private ReadOnly _MeStr As String = Application.ExecutablePath.Replace(":", "").Replace("\", "").Replace(".", "")
    Private ReadOnly _mmfSI As MemoryMappedFile = MemoryMappedFile.CreateOrOpen($"ScalA_IPCSI_{_MeStr}", 1 + 1)
    Private ReadOnly _mmvaSI As MemoryMappedViewAccessor = _mmfSI.CreateViewAccessor()
    Public Property AlreadyOpen As Boolean
        Get
            Return _mmvaSI.ReadBoolean(0)
        End Get
        Set(value As Boolean)
            _mmvaSI.Write(0, value)
        End Set
    End Property
    Public Property RequestActivation As Boolean
        Get
            Return _mmvaSI.ReadBoolean(1)
        End Get
        Set(ByVal value As Boolean)
            _mmvaSI.Write(1, value)
        End Set
    End Property

    <DllImport("user32.dll")>
    Private Function IsWindowVisible(ByVal hWnd As IntPtr) As Boolean : End Function

    Public Function EnumOtherScalAs() As IEnumerable(Of Process)
        Dim currentProcessId As Integer = Process.GetCurrentProcess().Id

        Return Process.GetProcesses().AsParallel().
                Where(Function(p)
                          Return p.Id <> currentProcessId AndAlso IsWindowVisible(p.MainWindowHandle) AndAlso IsScalA(p)
                      End Function)
    End Function

    Private OrigScalAfname As String = FileVersionInfo.GetVersionInfo(Environment.GetCommandLineArgs()(0)).OriginalFilename
    Public Function IsScalA(p As Process) As Boolean
        Try
            Return FileVersionInfo.GetVersionInfo(p.MainModule.FileName).OriginalFilename = OrigScalAfname
        Catch ex As Exception
            Return False
        End Try
    End Function
End Module