Imports System.IO.MemoryMappedFiles
Imports System.Runtime.InteropServices
Imports System.Threading

''' <summary>
''' Shares zoom state with SDL2 wrapper DLL for mouse coordinate transformation
''' Supports multiple ScalA instances controlling the same game
''' Dynamically grows if more instances are needed
''' </summary>
Module ZoomStateIPC

    ''' <summary>
    ''' Initial capacity for ScalA instances - will grow if needed
    ''' </summary>
    Private Const INITIAL_CAPACITY As Integer = 32

    ''' <summary>
    ''' Header size (64 bytes) - reserved space for future expansion
    ''' </summary>
    Public Const HEADER_SIZE As Integer = 64

    ''' <summary>
    ''' Entry size (64 bytes) - reserved space for future expansion
    ''' </summary>
    Public Const ENTRY_SIZE As Integer = 64

    ''' <summary>
    ''' Current structure version - bump when layout changes
    ''' </summary>
    Public Const STRUCT_VERSION As Integer = 2

    ' Header offsets
    Private Const HDR_VERSION As Integer = 0
    Private Const HDR_COUNT As Integer = 4
    Private Const HDR_VERSIONMISMATCH As Integer = 8
    Private Const HDR_MAXINSTANCES As Integer = 12  ' Current allocated capacity

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
    Private _currentMaxInstances As Integer = 0

    ''' <summary>
    ''' Calculate shared memory size for given capacity
    ''' </summary>
    Private Function CalcSharedMemSize(capacity As Integer) As Integer
        Return HEADER_SIZE + (ENTRY_SIZE * capacity)
    End Function

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

            ' Try to open existing map first to check its size
            Dim existingMax As Integer = 0
            Try
                Using existingMmf = MemoryMappedFile.OpenExisting(mapName)
                    Using existingView = existingMmf.CreateViewAccessor()
                        existingMax = existingView.ReadInt32(HDR_MAXINSTANCES)
                    End Using
                End Using
            Catch
                ' Map doesn't exist yet, we'll create it
            End Try

            ' Use existing capacity or initial capacity
            _currentMaxInstances = If(existingMax > 0, existingMax, INITIAL_CAPACITY)
            Dim memSize As Integer = CalcSharedMemSize(_currentMaxInstances)

            _zoomStateMmf = MemoryMappedFile.CreateOrOpen(mapName, memSize)
            _zoomStateMmva = _zoomStateMmf.CreateViewAccessor()
            _currentGamePid = gamePid

            ' Initialize header and find our slot
            _zoomStateMutex.WaitOne()
            Try
                ' Set version and max instances in header
                _zoomStateMmva.Write(HDR_VERSION, STRUCT_VERSION)
                _zoomStateMmva.Write(HDR_MAXINSTANCES, _currentMaxInstances)
                _mySlotIndex = FindOrCreateSlot()
                dBug.Print($"ZoomStateIPC: Initialized for game PID {gamePid}, using slot {_mySlotIndex}/{_currentMaxInstances}")
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
    ''' Will trigger resize if all slots are in use
    ''' </summary>
    Private Function FindOrCreateSlot() As Integer
        Dim myPid As Integer = scalaPID
        Dim maxSlots As Integer = _currentMaxInstances
        If maxSlots <= 0 Then maxSlots = INITIAL_CAPACITY

        ' Read current count from header
        Dim count As Integer = _zoomStateMmva.ReadInt32(HDR_COUNT)
        If count < 0 OrElse count > maxSlots Then count = 0

        ' Look for existing slot with our PID or an empty/dead slot
        Dim emptySlot As Integer = -1
        For i As Integer = 0 To maxSlots - 1
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

        ' If no empty slot found, try to resize
        If emptySlot = -1 Then
            If TryResize() Then
                ' After resize, use the first new slot
                emptySlot = maxSlots
            Else
                ' Resize failed, overwrite first slot as fallback
                dBug.Print($"ZoomStateIPC: WARNING - All {maxSlots} slots in use, overwriting slot 0")
                emptySlot = 0
            End If
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
    ''' Resize the shared memory to double the current capacity
    ''' Must be called while holding the mutex
    ''' </summary>
    Private Function TryResize() As Boolean
        If _zoomStateMmva Is Nothing OrElse _currentMaxInstances <= 0 Then Return False

        Try
            Dim oldMax As Integer = _currentMaxInstances
            Dim newMax As Integer = oldMax * 2
            Dim mapName As String = $"ScalA_ZoomState_{_currentGamePid}"
            Dim oldSize As Integer = CalcSharedMemSize(oldMax)
            Dim newSize As Integer = CalcSharedMemSize(newMax)

            dBug.Print($"ZoomStateIPC: Resizing from {oldMax} to {newMax} instances")

            ' Read all existing data
            Dim oldData(oldSize - 1) As Byte
            _zoomStateMmva.ReadArray(0, oldData, 0, oldSize)

            ' Close current handles
            _zoomStateMmva.Dispose()
            _zoomStateMmf.Dispose()
            _zoomStateMmva = Nothing
            _zoomStateMmf = Nothing

            ' Create new larger map (this replaces the old one since we closed it)
            _zoomStateMmf = MemoryMappedFile.CreateOrOpen(mapName, newSize)
            _zoomStateMmva = _zoomStateMmf.CreateViewAccessor()

            ' Copy old data and update header
            _zoomStateMmva.WriteArray(0, oldData, 0, oldSize)
            _zoomStateMmva.Write(HDR_MAXINSTANCES, newMax)
            _currentMaxInstances = newMax

            dBug.Print($"ZoomStateIPC: Resize complete, now {newMax} instances")
            Return True

        Catch ex As Exception
            dBug.Print($"ZoomStateIPC: Resize failed - {ex.Message}")
            Return False
        End Try
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
        _currentMaxInstances = 0
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

    ''' <summary>
    ''' Check if the DLL flagged a version mismatch
    ''' </summary>
    ''' <returns>True if DLL set mismatch flag, False otherwise</returns>
    Public Function HasVersionMismatch() As Boolean
        If _zoomStateMmva Is Nothing Then Return False
        Try
            Return _zoomStateMmva.ReadInt32(HDR_VERSIONMISMATCH) <> 0
        Catch
            Return False
        End Try
    End Function

End Module
