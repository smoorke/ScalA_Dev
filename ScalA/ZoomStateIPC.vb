Imports System.IO.MemoryMappedFiles
Imports System.Runtime.InteropServices

''' <summary>
''' Shares zoom state with SDL2 wrapper DLL for mouse coordinate transformation
''' </summary>
Module ZoomStateIPC

    ''' <summary>
    ''' Zoom state structure - must match SDL2Wrapper's ScalAZoomState
    ''' </summary>
    <StructLayout(LayoutKind.Sequential, Pack:=1)>
    Public Structure ScalAZoomState
        Public ScalaPid As Integer          ' ScalA process ID
        Public GameWindowHandle As Integer  ' Game window handle being managed
        Public ViewportX As Integer         ' pbZoom screen X position
        Public ViewportY As Integer         ' pbZoom screen Y position
        Public ViewportWidth As Integer     ' pbZoom width (what user sees)
        Public ViewportHeight As Integer    ' pbZoom height (what user sees)
        Public GameClientWidth As Integer   ' Original game client width (rcC.Width)
        Public GameClientHeight As Integer  ' Original game client height (rcC.Height)
        Public Enabled As Integer           ' 1 if transform should be applied, 0 otherwise
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
                .ScalaPid = scalaPID,
                .GameWindowHandle = 0,
                .ViewportX = 0,
                .ViewportY = 0,
                .ViewportWidth = 0,
                .ViewportHeight = 0,
                .GameClientWidth = 0,
                .GameClientHeight = 0,
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
    ''' <param name="viewportScreenBounds">Screen bounds of pbZoom (Me.RectangleToScreen(pbZoom.Bounds))</param>
    ''' <param name="gameClientSize">Size of game client area (rcC.Size)</param>
    ''' <param name="gameWindowHandle">Handle of the game window</param>
    ''' <param name="enabled">Whether coordinate transformation should be active</param>
    Public Sub UpdateZoomState(viewportScreenBounds As Rectangle, gameClientSize As Size, gameWindowHandle As IntPtr, enabled As Boolean)
        If _zoomStateMmva Is Nothing Then Return

        Try
            Dim state As New ScalAZoomState With {
                .ScalaPid = scalaPID,
                .GameWindowHandle = gameWindowHandle.ToInt32(),
                .ViewportX = viewportScreenBounds.X,
                .ViewportY = viewportScreenBounds.Y,
                .ViewportWidth = viewportScreenBounds.Width,
                .ViewportHeight = viewportScreenBounds.Height,
                .GameClientWidth = gameClientSize.Width,
                .GameClientHeight = gameClientSize.Height,
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
    ''' <param name="frm">The FrmMain instance</param>
    ''' <param name="enabled">Whether coordinate transformation should be active</param>
    Public Sub UpdateFromFrmMain(frm As FrmMain, enabled As Boolean)
        If frm Is Nothing OrElse frm.AltPP Is Nothing OrElse Not frm.AltPP.IsRunning Then
            SetZoomStateEnabled(False)
            Return
        End If

        ' Initialize if needed for this game
        If _currentGamePid <> frm.AltPP.Id Then
            InitZoomState(frm.AltPP.Id)
        End If

        ' Get viewport screen bounds
        Dim viewportScreenBounds As Rectangle = frm.RectangleToScreen(frm.pbZoom.Bounds)

        ' Get game client size
        Dim gameClientSize As Size = frm.AltPP.ClientRect.Size

        ' Update shared memory
        UpdateZoomState(viewportScreenBounds, gameClientSize, frm.AltPP.MainWindowHandle, enabled)
    End Sub

End Module
