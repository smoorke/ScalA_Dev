Imports System.IO.MemoryMappedFiles
Module IPC
    Private ReadOnly _QlCtxOpen As MemoryMappedViewAccessor = MemoryMappedFile.CreateOrOpen("ScalA_IPC_QlCtxOpen", 1).CreateViewAccessor()
    Public Property IsQlCtxOpen As Boolean
        Get
            Return _QlCtxOpen.ReadBoolean(0)
        End Get
        Set(ByVal value As Boolean)
            _QlCtxOpen.Write(0, value)
            Debug.Print($"set QlCtxOpen to {value}")
        End Set
    End Property
End Module