Imports System.IO.MemoryMappedFiles
Imports System.Runtime.InteropServices
Imports System.Threading
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


    Dim _mmfIPCov As MemoryMappedFile = MemoryMappedFile.CreateOrOpen($"ScalA_IPC_{FrmMain.scalaPID}", Marshal.SizeOf(Of Integer) * 2)
    Dim _mmvaIPCov As MemoryMappedViewAccessor = _mmfIPCov.CreateViewAccessor()

    Public Sub AddToWhitelistOrRemoveFromBL(spId As Integer, apId As Integer)
        If spId = FrmMain.scalaPID Then
            _mmvaIPCov.Write(0, apId)
        Else
            Dim _mmfIPC As MemoryMappedFile = MemoryMappedFile.CreateOrOpen($"ScalA_IPC_{spId}", Marshal.SizeOf(Of Integer) * 2)
            Dim _mmvaIPC As MemoryMappedViewAccessor = _mmfIPC.CreateViewAccessor()
            _mmvaIPC.Write(0, apId)
        End If
    End Sub

    Public Function AddToWhitelistOrRemoveFromBL(Optional spId As Integer = 0) As Integer
        If spId = 0 Then spId = FrmMain.scalaPID
        If spId = FrmMain.scalaPID Then
            Return _mmvaIPCov.ReadInt32(0)
        Else
            Using _mmfIPC As MemoryMappedFile = MemoryMappedFile.CreateOrOpen($"ScalA_IPC_{spId}", Marshal.SizeOf(Of Integer) * 2),
                  _mmvaIPC As MemoryMappedViewAccessor = _mmfIPC.CreateViewAccessor()
                Return _mmvaIPC.ReadInt32(0)
            End Using
        End If
    End Function

    Public Sub SelectAlt(spID As Integer, apID As Integer)
        If spID = FrmMain.scalaPID Then
            _mmvaIPCov.Write(4, apID)
        Else
            Dim _mmfIPC As MemoryMappedFile = MemoryMappedFile.CreateOrOpen($"ScalA_IPC_{spID}", Marshal.SizeOf(Of Integer) * 2)
            Dim _mmvaIPC As MemoryMappedViewAccessor = _mmfIPC.CreateViewAccessor()
            _mmvaIPC.Write(4, apID)
        End If
    End Sub
    Public Function ReadSelectAlt(Optional spId As Integer = 0) As Integer
        If spId = 0 Then spId = FrmMain.scalaPID
        If spId = FrmMain.scalaPID Then
            Return _mmvaIPCov.ReadInt32(4)
        Else
            Using _mmfIPC As MemoryMappedFile = MemoryMappedFile.CreateOrOpen($"ScalA_IPC_{spId}", Marshal.SizeOf(Of Integer) * 2),
                  _mmvaIPC As MemoryMappedViewAccessor = _mmfIPC.CreateViewAccessor()
                Return _mmvaIPC.ReadInt32(4)
            End Using
        End If
    End Function

    Private ReadOnly OrigScalAfname As String = Process.GetCurrentProcess.MainModule.FileVersionInfo.OriginalFilename

    ''' <summary>
    ''' Is process ScalA?
    ''' </summary>
    ''' <param name="p"></param>
    ''' <returns></returns>
    <System.Runtime.CompilerServices.Extension()>
    Public Function IsScalA(p As Process) As Boolean
        Try
            Return p.MainModule.FileVersionInfo.OriginalFilename = OrigScalAfname
        Catch ex As System.ComponentModel.Win32Exception
            Dim processPath As String = ""

            Dim processHandle As IntPtr = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, False, p.Id)
            Try
                If Not processHandle = IntPtr.Zero Then
                    Dim buffer As New System.Text.StringBuilder(1024)
                    If QueryFullProcessImageName(processHandle, 0, buffer, buffer.Capacity) Then
                        processPath = buffer.ToString()
                    End If
                End If
            Finally
                CloseHandle(processHandle)
            End Try

            Return FileVersionInfo.GetVersionInfo(processPath).OriginalFilename = OrigScalAfname

        Catch ex As Exception
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Gets the actual ScalA MainWindoHandle.
    ''' Process.MainWindowHandle is null for attached ScalA
    ''' </summary>
    ''' <param name="p"></param>
    ''' <returns></returns>

    <System.Runtime.CompilerServices.Extension()>
    Public Function GetWindowHandle(p As Process) As IntPtr
        _mutex.WaitOne()
        Try
            Dim sharednum = _mmvaInstances.ReadInt32(0)

            Dim _Instances(sharednum - 1) As ScalAInfo
            _mmvaInstances.ReadArray(Of ScalAInfo)(4, _Instances, 0, sharednum)

            If p.IsScalA Then
                Return _Instances.FirstOrDefault(Function(si) si.pid = p.Id).handle
            Else
                Return p.MainWindowHandle
            End If
        Finally
            _mutex.ReleaseMutex()
        End Try
    End Function

    <StructLayout(LayoutKind.Sequential)> '64 bytes
    Public Structure ScalAInfo
        Public pid As Integer
        Public isOnOverview As Boolean
        Public handle As IntPtr
        Public AltPPid As Integer

        Private padding1 As Long
        Private padding2 As Long
        Private Padding3 As Long
        Private padding4 As Long
        Private padding5 As Long
        Private Padding6 As Long

        Public Shared Operator =(i1 As ScalAInfo, i2 As ScalAInfo) As Boolean
            Return i1.pid = i2.pid
        End Operator

        Public Shared Operator <>(i1 As ScalAInfo, i2 As ScalAInfo) As Boolean
            Return i1.pid <> i2.pid
        End Operator

        Public Sub New(pid As Integer, overview? As Boolean, APid? As Integer)
            Me.pid = pid
            If overview IsNot Nothing Then Me.isOnOverview = overview
            Me.handle = FrmMain.ScalaHandle
            If APid IsNot Nothing Then Me.AltPPid = APid
        End Sub

    End Structure

    Private _mmfInstances As MemoryMappedFile = MemoryMappedFile.CreateOrOpen($"ScalA_IPCInstances", 4 + Marshal.SizeOf(GetType(ScalAInfo)) * (localnum + 1))
    Private _mmvaInstances As MemoryMappedViewAccessor = _mmfInstances.CreateViewAccessor()
    Private localnum As UInteger = 0
    'Private _Instances() As ScalAInfo
    Private _mutex As New Mutex(False, "ScalA_IPCInstances_Mutex")

    Public Sub AddOrUpdateInstance(id As Integer, Optional overview? As Boolean = Nothing, Optional apID? As Integer = Nothing)
        _mutex.WaitOne()
        Try
            Dim sharednum = _mmvaInstances.ReadInt32(0)

            Dim _Instances(sharednum - 1) As ScalAInfo
            _mmvaInstances.ReadArray(Of ScalAInfo)(4, _Instances, 0, sharednum)

            Dim existingIndex As Integer = Array.FindIndex(_Instances, Function(inst) inst.pid = id)

            If existingIndex = -1 Then
                ' The instance does not exist, you can add it now
                ' todo: find an index that has an empty instance
                ReDim Preserve _Instances(sharednum)
                _Instances(sharednum) = New ScalAInfo(id, overview, apID)

                If sharednum <> localnum Then
                    _mmfInstances = MemoryMappedFile.CreateOrOpen($"ScalA_IPCInstances", 4 + Marshal.SizeOf(GetType(ScalAInfo)) * (sharednum + 1))
                    _mmvaInstances = _mmfInstances.CreateViewAccessor()
                    localnum = sharednum
                End If

                ' Update the shared count 
                _mmvaInstances.Write(0, _Instances.Length)
            Else
                ' The instance already exists, update the existing instance in the array
                _Instances(existingIndex) = New ScalAInfo(id,
                                                          If(overview IsNot Nothing, overview, _Instances(existingIndex).isOnOverview),
                                                          If(apID IsNot Nothing, apID, _Instances(existingIndex).AltPPid))
            End If
            ' Write the array back to the memory-mapped file
            _mmvaInstances.Write(0, _Instances.Length)
            _mmvaInstances.WriteArray(4, _Instances, 0, _Instances.Length)
        Finally
            _mutex.ReleaseMutex()
        End Try
    End Sub
    Public Function getInstances() As IEnumerable(Of ScalAInfo)
        _mutex.WaitOne()
        Try
            Dim sharednum = _mmvaInstances.ReadInt32(0)

            If sharednum <> localnum Then
                _mmfInstances = MemoryMappedFile.CreateOrOpen($"ScalA_IPCInstances", 4 + Marshal.SizeOf(GetType(ScalAInfo)) * sharednum)
                _mmvaInstances = _mmfInstances.CreateViewAccessor()
                localnum = sharednum
            End If

            Dim _Instances(sharednum - 1) As ScalAInfo
            _mmvaInstances.ReadArray(4, _Instances, 0, sharednum)

            Dim newInstances = _Instances.Select(Function(si As ScalAInfo)
                                                     Try
                                                         Using pp As Process = Process.GetProcessById(si.pid)
                                                             If Not pp.IsScalA() Then Return New ScalAInfo
                                                             Return si
                                                         End Using
                                                     Catch
                                                         Return New ScalAInfo
                                                     End Try
                                                 End Function).Where(Function(sin) sin.pid <> 0).ToArray

            _Instances = newInstances
            _mmvaInstances.Write(0, _Instances.Length)
            _mmvaInstances.WriteArray(4, _Instances, 0, _Instances.Length)
            Return _Instances
        Finally
            _mutex.ReleaseMutex()
        End Try
    End Function
    Public Function getOtherInstances() As IEnumerable(Of ScalAInfo)
        Return getInstances.Where(Function(si) si.pid <> Process.GetCurrentProcess.Id)
    End Function

    Public Function EnumOtherScalAs() As IEnumerable(Of Process)
        Return getOtherInstances.Select(Function(si)
                                            Try
                                                Return Process.GetProcessById(si.pid)
                                            Catch
                                                Return Nothing
                                            End Try
                                        End Function).Where(Function(p) p IsNot Nothing)
    End Function
    Public Function EnumOtherOverviews() As IEnumerable(Of Process)
        Return getOtherInstances.Select(Function(si)
                                            Try
                                                If si.isOnOverview Then
                                                    Return Process.GetProcessById(si.pid)
                                                Else
                                                    Return Nothing
                                                End If
                                            Catch
                                                Return Nothing
                                            End Try
                                        End Function).Where(Function(p) p IsNot Nothing)
    End Function
End Module