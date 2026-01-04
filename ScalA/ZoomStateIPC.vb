Imports System.IO.MemoryMappedFiles
Imports System.Runtime.InteropServices
Imports System.Threading

''' <summary>
''' Shares zoom state with SDL2 wrapper DLL for mouse coordinate transformation
''' Supports multiple ScalA instances controlling the same game
''' </summary>
Module ZoomStateIPC

    ''' <summary>
    ''' Maximum number of ScalA instances that can control the same game
    ''' </summary>
    Public Const MAX_SCALA_INSTANCES As Integer = 8

    ''' <summary>
    ''' Header size (64 bytes) - reserved space for future expansion
    ''' </summary>
    Public Const HEADER_SIZE As Integer = 64

    ''' <summary>
    ''' Entry size (64 bytes) - reserved space for future expansion
    ''' </summary>
    Public Const ENTRY_SIZE As Integer = 64

    ''' <summary>
    ''' Current structure version
    ''' </summary>
    Public Const STRUCT_VERSION As Integer = 1

    ' Header offsets
    Private Const HDR_VERSION As Integer = 0
    Private Const HDR_COUNT As Integer = 4

    ' Entry field offsets (relative to entry start)
    Private Const ENT_SCALAPID As Integer = 0
    Private Const ENT_VIEWPORTX As Integer = 4
    Private Const ENT_VIEWPORTY As Integer = 8
    Private Const ENT_VIEWPORTW As Integer = 12
    Private Const ENT_VIEWPORTH As Integer = 16
    Private Const ENT_CLIENTW As Integer = 20
    Private Const ENT_CLIENTH As Integer = 24
    Private Const ENT_ENABLED As Integer = 28

    Private _zoomStateMmf As MemoryMappedFile = Nothing
    Private _zoomStateMmva As MemoryMappedViewAccessor = Nothing
    Private _zoomStateMutex As Mutex = Nothing
    Private _currentGamePid As Integer = 0
    Private _mySlotIndex As Integer = -1

    ''' <summary>
    ''' Size of the shared memory: 64-byte header + 64-byte entries
    ''' </summary>
    Private ReadOnly SHARED_MEM_SIZE As Integer = HEADER_SIZE + (ENTRY_SIZE * MAX_SCALA_INSTANCES)

    ''' <summary>
    ''' Initialize zoom state sharing for a specific game process
    ''' </summary>
    ''' <param name="gamePid">The process ID of the game being managed</param>
    Public Sub InitZoomState(gamePid As Integer)
        If gamePid = _currentGamePid AndAlso _zoomStateMmf IsNot Nothing Then
            Return ' Already initialized for this game
        End If

        CleanupZoomState()

        Try
            Dim mapName As String = $"ScalA_ZoomState_{gamePid}"
            Dim mutexName As String = $"ScalA_ZoomState_Mutex_{gamePid}"

            _zoomStateMutex = New Mutex(False, mutexName)
            _zoomStateMmf = MemoryMappedFile.CreateOrOpen(mapName, SHARED_MEM_SIZE)
            _zoomStateMmva = _zoomStateMmf.CreateViewAccessor()
            _currentGamePid = gamePid

            ' Initialize header version if needed and find our slot
            _zoomStateMutex.WaitOne()
            Try
                ' Set version in header
                _zoomStateMmva.Write(HDR_VERSION, STRUCT_VERSION)
                _mySlotIndex = FindOrCreateSlot()
                dBug.Print($"ZoomStateIPC: Initialized for game PID {gamePid}, using slot {_mySlotIndex}")
            Finally
                _zoomStateMutex.ReleaseMutex()
            End Try

        Catch ex As Exception
            dBug.Print($"ZoomStateIPC: Failed to initialize - {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' Get the byte offset for an entry
    ''' </summary>
    Private Function EntryOffset(slotIndex As Integer) As Integer
        Return HEADER_SIZE + (slotIndex * ENTRY_SIZE)
    End Function

    ''' <summary>
    ''' Find existing slot for this ScalA instance or create a new one
    ''' </summary>
    Private Function FindOrCreateSlot() As Integer
        Dim myPid As Integer = scalaPID

        ' Read current count from header
        Dim count As Integer = _zoomStateMmva.ReadInt32(HDR_COUNT)
        If count < 0 OrElse count > MAX_SCALA_INSTANCES Then count = 0

        ' Look for existing slot with our PID or an empty/dead slot
        Dim emptySlot As Integer = -1
        For i As Integer = 0 To MAX_SCALA_INSTANCES - 1
            Dim entryPid As Integer = _zoomStateMmva.ReadInt32(EntryOffset(i) + ENT_SCALAPID)

            If entryPid = myPid Then
                Return i ' Found our existing slot
            End If

            If emptySlot = -1 AndAlso entryPid = 0 Then
                emptySlot = i ' Found an empty slot
            ElseIf emptySlot = -1 AndAlso entryPid <> 0 Then
                ' Check if this ScalA is still running
                Try
                    Process.GetProcessById(entryPid)
                Catch
                    ' Process not running, we can use this slot
                    emptySlot = i
                End Try
            End If
        Next

        If emptySlot = -1 Then
            emptySlot = 0 ' Fallback to first slot if all are taken
        End If

        ' Initialize our slot
        Dim offset As Integer = EntryOffset(emptySlot)
        _zoomStateMmva.Write(offset + ENT_SCALAPID, myPid)
        _zoomStateMmva.Write(offset + ENT_ENABLED, 0)

        ' Update count if needed
        If emptySlot >= count Then
            _zoomStateMmva.Write(HDR_COUNT, emptySlot + 1)
        End If

        Return emptySlot
    End Function

    ''' <summary>
    ''' Update the zoom state with current viewport and game dimensions
    ''' </summary>
    Public Sub UpdateZoomState(viewportScreenBounds As Rectangle, gameClientSize As Size, enabled As Boolean)
        If _zoomStateMmva Is Nothing OrElse _mySlotIndex < 0 Then Return

        Try
            Dim offset As Integer = EntryOffset(_mySlotIndex)

            ' Write entry fields (ScalaPID already set during init)
            _zoomStateMmva.Write(offset + ENT_VIEWPORTX, viewportScreenBounds.X)
            _zoomStateMmva.Write(offset + ENT_VIEWPORTY, viewportScreenBounds.Y)
            _zoomStateMmva.Write(offset + ENT_VIEWPORTW, viewportScreenBounds.Width)
            _zoomStateMmva.Write(offset + ENT_VIEWPORTH, viewportScreenBounds.Height)
            _zoomStateMmva.Write(offset + ENT_CLIENTW, gameClientSize.Width)
            _zoomStateMmva.Write(offset + ENT_CLIENTH, gameClientSize.Height)
            _zoomStateMmva.Write(offset + ENT_ENABLED, If(enabled, 1, 0))
        Catch ex As Exception
            dBug.Print($"ZoomStateIPC: Failed to update - {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' Enable or disable coordinate transformation
    ''' </summary>
    Public Sub SetZoomStateEnabled(enabled As Boolean)
        If _zoomStateMmva Is Nothing OrElse _mySlotIndex < 0 Then Return
        Try
            _zoomStateMmva.Write(EntryOffset(_mySlotIndex) + ENT_ENABLED, If(enabled, 1, 0))
        Catch ex As Exception
            dBug.Print($"ZoomStateIPC: Failed to set enabled - {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' Cleanup zoom state resources
    ''' </summary>
    Public Sub CleanupZoomState()
        Try
            ' Clear our slot before cleanup
            If _zoomStateMmva IsNot Nothing AndAlso _mySlotIndex >= 0 Then
                _zoomStateMutex?.WaitOne()
                Try
                    Dim offset As Integer = EntryOffset(_mySlotIndex)
                    ' Clear entry by setting ScalaPID to 0 (marks as unused)
                    _zoomStateMmva.Write(offset + ENT_SCALAPID, 0)
                    _zoomStateMmva.Write(offset + ENT_ENABLED, 0)
                Finally
                    _zoomStateMutex?.ReleaseMutex()
                End Try
            End If
        Catch
        End Try

        _zoomStateMmva?.Dispose()
        _zoomStateMmva = Nothing

        _zoomStateMmf?.Dispose()
        _zoomStateMmf = Nothing

        _zoomStateMutex?.Dispose()
        _zoomStateMutex = Nothing

        _currentGamePid = 0
        _mySlotIndex = -1
    End Sub

    ''' <summary>
    ''' Update zoom state from frmMain state - call this after zoom/move/resize events
    ''' </summary>
    Public Sub UpdateFromFrmMain(frm As FrmMain, enabled As Boolean)
        If frm Is Nothing OrElse frm.AltPP Is Nothing OrElse Not frm.AltPP.IsRunning Then
            SetZoomStateEnabled(False)
            Return
        End If

        ' Initialize if needed for this game
        If _currentGamePid <> frm.AltPP.Id Then
            InitZoomState(frm.AltPP.Id)
        End If

        ' Get viewport screen bounds and game client size
        Dim viewportBounds As Rectangle = frm.RectangleToScreen(frm.pbZoom.Bounds)
        Dim clientSize As Size = frm.AltPP.ClientRect.Size

        ' Update shared memory
        UpdateZoomState(viewportBounds, clientSize, enabled)
    End Sub

End Module
