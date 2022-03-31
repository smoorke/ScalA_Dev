Imports System.IO.MemoryMappedFiles
Module IPC
    Private ReadOnly _mmfBoolean As MemoryMappedFile = MemoryMappedFile.CreateOrOpen("ScalA_IPC_Boolean", 2 * 1)
    Private ReadOnly _mmvaBoolean As MemoryMappedViewAccessor = _mmfBoolean.CreateViewAccessor()
    Public Property IsQlCtxOpen As Boolean
        Get
            'Debug.Print(_mmvaBoolean.ReadBoolean(0))
            Return _mmvaBoolean.ReadBoolean(0)
        End Get
        Set(ByVal value As Boolean)
            _mmvaBoolean.Write(0, value)
            Debug.Print($"set QlCtxOpen to {value}")
        End Set
    End Property

    Private ReadOnly _MeStr As String = Application.ExecutablePath.Replace(":", "").Replace("\", "").Replace(".", "")
    Private ReadOnly _mmfSI As MemoryMappedFile = MemoryMappedFile.CreateOrOpen($"ScalA_IPCSI_{_MeStr}", 1 * 4 + 1)
    Private ReadOnly _mmvaSI As MemoryMappedViewAccessor = _mmfSI.CreateViewAccessor()
    Public Property AlreadyOpenPID As Integer
        Get
            Return _mmvaSI.ReadInt32(0)
        End Get
        Set(value As Integer)
            _mmvaSI.Write(0, value)
        End Set
    End Property
    Public Property RequestActivation() As Boolean
        Get
            Return _mmvaSI.ReadBoolean(4)
        End Get
        Set(ByVal value As Boolean)
            _mmvaSI.Write(4, value)
        End Set
    End Property

End Module