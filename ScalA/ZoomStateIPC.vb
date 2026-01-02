Imports System.IO.MemoryMappedFiles
Imports System.Runtime.InteropServices

''' <summary>
''' Shares zoom state with SDL2 wrapper DLL for mouse coordinate transformation
''' </summary>
Module ZoomStateIPC

    ''' <summary>
    ''' Zoom state structure - must match SDL2Wrapper's ScalAZoomState (28 bytes)
    ''' </summary>
    <StructLayout(LayoutKind.Sequential, Pack:=1)>
    Public Structure ScalAZoomState
        Public ViewportX As Integer         ' pbZoom screen X position
        Public ViewportY As Integer         ' pbZoom screen Y position
        Public ViewportW As Integer         ' pbZoom width (what user sees)
        Public ViewportH As Integer         ' pbZoom height (what user sees)
        Public ClientW As Integer           ' Original game client width
        Public ClientH As Integer           ' Original game client height
        Public Enabled As Integer           ' 1 if transform active, 0 otherwise
    End Structure

    Private _zoomStateMmf As MemoryMappedFile = Nothing
    Private _zoomStateMmva As MemoryMappedViewAccessor = Nothing
    Private _currentGamePid As Integer = 0

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
            _zoomStateMmf = MemoryMappedFile.CreateOrOpen(mapName, Marshal.SizeOf(GetType(ScalAZoomState)))
            _zoomStateMmva = _zoomStateMmf.CreateViewAccessor()
            _currentGamePid = gamePid

            ' Initialize with disabled state
            Dim initialState As New ScalAZoomState With {
                .ViewportX = 0,
                .ViewportY = 0,
                .ViewportW = 0,
                .ViewportH = 0,
                .ClientW = 0,
                .ClientH = 0,
                .Enabled = 0
            }
            _zoomStateMmva.Write(0, initialState)

            dBug.Print($"ZoomStateIPC: Initialized for game PID {gamePid}")
        Catch ex As Exception
            dBug.Print($"ZoomStateIPC: Failed to initialize - {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' Update the zoom state with current viewport and game dimensions
    ''' </summary>
    Public Sub UpdateZoomState(viewportScreenBounds As Rectangle, gameClientSize As Size, enabled As Boolean)
        If _zoomStateMmva Is Nothing Then Return

        Try
            Dim state As New ScalAZoomState With {
                .ViewportX = viewportScreenBounds.X,
                .ViewportY = viewportScreenBounds.Y,
                .ViewportW = viewportScreenBounds.Width,
                .ViewportH = viewportScreenBounds.Height,
                .ClientW = gameClientSize.Width,
                .ClientH = gameClientSize.Height,
                .Enabled = If(enabled, 1, 0)
            }
            _zoomStateMmva.Write(0, state)
        Catch ex As Exception
            dBug.Print($"ZoomStateIPC: Failed to update - {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' Enable or disable coordinate transformation
    ''' </summary>
    Public Sub SetZoomStateEnabled(enabled As Boolean)
        If _zoomStateMmva Is Nothing Then Return
        Try
            ' Read current state, update enabled flag, write back
            Dim state As ScalAZoomState
            _zoomStateMmva.Read(0, state)
            state.Enabled = If(enabled, 1, 0)
            _zoomStateMmva.Write(0, state)
        Catch ex As Exception
            dBug.Print($"ZoomStateIPC: Failed to set enabled - {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' Cleanup zoom state resources
    ''' </summary>
    Public Sub CleanupZoomState()
        Try
            ' Disable before cleanup
            If _zoomStateMmva IsNot Nothing Then
                Dim state As ScalAZoomState
                _zoomStateMmva.Read(0, state)
                state.Enabled = 0
                _zoomStateMmva.Write(0, state)
            End If
        Catch
        End Try

        _zoomStateMmva?.Dispose()
        _zoomStateMmva = Nothing

        _zoomStateMmf?.Dispose()
        _zoomStateMmf = Nothing

        _currentGamePid = 0
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
