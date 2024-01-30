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
        Catch ex As Exception
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Gets the actual MainWindoHandle.
    ''' Process.MainWindowHandle is null for attached ScalA
    ''' </summary>
    ''' <param name="p"></param>
    ''' <returns></returns>

    <System.Runtime.CompilerServices.Extension()>
    Public Function GetHandle(p As Process) As IntPtr
        Dim sharednum = _mmvaInstances.ReadInt32(0)

        ReDim Preserve _Instances(sharednum)
        _mmvaInstances.ReadArray(Of ScalAInfo)(4, _Instances, 0, sharednum)

        Return _Instances.FirstOrDefault(Function(si) si.pid = p.Id).handle

    End Function



    <StructLayout(LayoutKind.Sequential)> '64 bytes
    Public Structure ScalAInfo
        Public pid As Integer
        Public isOnOverview As Boolean
        Public handle As IntPtr

        Private padding0 As Integer

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

        Public Sub New(pid As Integer, overview As Boolean)
            Me.pid = pid
            Me.isOnOverview = overview
            Me.handle = FrmMain.ScalaHandle
        End Sub

    End Structure

    Private _mmfInstances As MemoryMappedFile = MemoryMappedFile.CreateOrOpen($"ScalA_IPCInstances", 4 + Marshal.SizeOf(GetType(ScalAInfo)) * (localNum + 1))
    Private _mmvaInstances As MemoryMappedViewAccessor = _mmfInstances.CreateViewAccessor()
    Private localnum As UInteger = 0
    Private _Instances() As ScalAInfo

    Public Sub AddOrUpdateInstance(id As Integer, Optional overview As Boolean = False)
        Dim sharednum = _mmvaInstances.ReadInt32(0)

        ReDim Preserve _Instances(sharednum)
        _mmvaInstances.ReadArray(Of ScalAInfo)(4, _Instances, 0, sharednum)

        Dim newInstance = New ScalAInfo(id, overview)

        Dim existingIndex As Integer = Array.FindIndex(_Instances, Function(inst) inst = newInstance)

        If existingIndex = -1 Then
            ' The instance does not exist, you can add it now
            ' todo: find an index that has an empty instance
            'ReDim Preserve _Instances(sharednum)
            _Instances(sharednum) = newInstance

            If sharednum <> localnum Then
                _mmfInstances = MemoryMappedFile.CreateOrOpen($"ScalA_IPCInstances", 4 + Marshal.SizeOf(GetType(ScalAInfo)) * (sharednum + 1))
                _mmvaInstances = _mmfInstances.CreateViewAccessor()
                localnum = sharednum
            End If

            ' Update the shared count 
            _mmvaInstances.Write(0, _Instances.Length)
        Else
            ' The instance already exists, update the existing instance in the array
            _Instances(existingIndex) = newInstance
        End If
        ' Write the array back to the memory-mapped file
        _mmvaInstances.Write(0, _Instances.Length)
        _mmvaInstances.WriteArray(4, _Instances, 0, _Instances.Length)
    End Sub
    Public Function getInstances() As IEnumerable(Of ScalAInfo)
        Dim sharednum = _mmvaInstances.ReadInt32(0)

        If sharednum <> localnum Then
            _mmfInstances = MemoryMappedFile.CreateOrOpen($"ScalA_IPCInstances", 4 + Marshal.SizeOf(GetType(ScalAInfo)) * sharednum)
            _mmvaInstances = _mmfInstances.CreateViewAccessor()
            localnum = sharednum
        End If

        ReDim _Instances(sharednum - 1)
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