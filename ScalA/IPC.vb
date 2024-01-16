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


    Dim _mmfIPCov As MemoryMappedFile = MemoryMappedFile.CreateOrOpen($"ScalA_IPC_{FrmMain.scalaPID}", Marshal.SizeOf(Of Integer))
    Dim _mmvaIPCov As MemoryMappedViewAccessor = _mmfIPCov.CreateViewAccessor()

    Public Sub AddToWhitelistOrRemoveFromBL(spId As Integer, apId As Integer)
        If spId = FrmMain.scalaPID Then
            _mmvaIPCov.Write(0, apId)
        Else
            Dim _mmfIPC As MemoryMappedFile = MemoryMappedFile.CreateOrOpen($"ScalA_IPC_{spId}", Marshal.SizeOf(Of Integer))
            Dim _mmvaIPC As MemoryMappedViewAccessor = _mmfIPC.CreateViewAccessor()
            _mmvaIPC.Write(0, apId)
        End If
    End Sub

    Public Function AddToWhitelistOrRemoveFromBL(Optional spId As Integer = 0) As Integer
        If spId = 0 Then spId = FrmMain.scalaPID
        If spId = FrmMain.scalaPID Then
            Return _mmvaIPCov.ReadInt32(0)
        Else
            Using _mmfIPC As MemoryMappedFile = MemoryMappedFile.CreateOrOpen($"ScalA_IPC_{spId}", Marshal.SizeOf(Of Integer)),
                  _mmvaIPC As MemoryMappedViewAccessor = _mmfIPC.CreateViewAccessor()
                Return _mmvaIPC.ReadInt32(0)
            End Using
        End If
    End Function


    <DllImport("user32.dll")>
    Private Function IsWindowVisible(ByVal hWnd As IntPtr) As Boolean : End Function

    Public Function EnumOtherScalAs() As IEnumerable(Of Process)
        Dim currentProcessId As Integer = Process.GetCurrentProcess().Id

        Return Process.GetProcesses().AsParallel().
                Where(Function(p)
                          'note: IsWindowVisble returns false for ScalAs attached to a client
                          Return p.Id <> currentProcessId AndAlso IsWindowVisible(p.MainWindowHandle) AndAlso
                                 IsScalA(p) AndAlso IsOnOverview(p)
                      End Function)
    End Function

    Private ReadOnly OrigScalAfname As String = Process.GetCurrentProcess.MainModule.FileVersionInfo.OriginalFilename

    ''' <summary>
    ''' Is process SclaA?
    ''' </summary>
    ''' <param name="p"></param>
    ''' <returns></returns>
    Public Function IsScalA(p As Process) As Boolean
        Try
            Debug.Print($"{p.Id} {p.MainModule.FileName} {p.MainModule.FileVersionInfo.OriginalFilename}")
            Return p.MainModule.FileVersionInfo.OriginalFilename = OrigScalAfname
        Catch ex As Exception
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Determines if target ScalA Is on overview
    ''' </summary>
    ''' <param name="p"></param>
    ''' <returns></returns>
    Public Function IsOnOverview(p As Process) As Boolean
        p.Refresh()
        Debug.Print($"IsOnOverview {p.MainWindowTitle}")
        If p.MainWindowTitle = "ScalA" Then Return True
        Return False
    End Function
End Module