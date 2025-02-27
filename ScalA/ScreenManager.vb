Public NotInheritable Class ScreenManager

    'TODO: replace more calls to Screen.ScalingPercent with ScreenManager.ScalingPercent


    ''' <summary>
    ''' Returns a list of screens to the left of the specified screen.
    ''' Caches the result to avoid repeated calculations.
    ''' </summary>
    ''' <returns></returns>
    Public Function ScreensAdjacentToLeft() As List(Of ScreenManager)
#If DEBUG Then
        Dim sw As Stopwatch = New Stopwatch
        Try
            sw.Start()
#End If
            If _leftScreens IsNot Nothing Then Return _leftScreens

            Dim scrList As New List(Of ScreenManager)

            Dim allScreens As Screen() = Screen.AllScreens

            If allScreens.Length > 1 Then
                Dim WindowBorderSize = 1 + (3 * Me.ScalingPercent() / 100)
                Dim WindowCaptionHeight = 1 + (26 * Me.ScalingPercent() / 100)
                Dim targetRect As New Rectangle(Me.Bounds.Left - WindowBorderSize, Me.Bounds.Top - WindowCaptionHeight, WindowBorderSize, Me.Bounds.Height)
                For Each s In allScreens
                    If s.Bounds.IntersectsWith(targetRect) Then
                        Dim tgtScreenManager As ScreenManager = ScreenManager.FromScreen(s)
                        If Not scrList.Contains(tgtScreenManager) Then
                            scrList.Add(tgtScreenManager)
                        End If
                    End If
                Next
            End If

            _leftScreens = scrList
            Return _leftScreens
#If DEBUG Then
        Finally
            sw.Stop()
            dBug.Print($"Enumerating leftscreens took {sw.ElapsedMilliseconds}ms")
        End Try
#End If
    End Function

    Private _NWScaling As Boolean? = Nothing
    ''' <summary>
    ''' Returns False when one or more screens to the left and/or top are in different scaling modes.
    ''' Caches the result to avoid repeated calculations.
    ''' </summary>
    ''' <returns></returns>
    ''' 
    ''' TODO: cache result
    Public Function SameScalingModesNW() As Boolean
#If DEBUG Then
        Dim sw As Stopwatch = New Stopwatch
        Try
            sw.Start()
#End If
            If _NWScaling IsNot Nothing Then Return _NWScaling

            Dim allScreens As Screen() = Screen.AllScreens
            If allScreens.Length > 1 Then
                Dim scrm As ScreenManager = ScreenManager.FromScreen(Me._screen)

                Dim currentScaling As Integer = scrm.ScalingPercent()

                Dim ClientWidth = AstoniaProcess.BiggestWindowWidth
                Dim ClientHeight = AstoniaProcess.BiggestWindowHeight

                Dim leftRect As New Rectangle(scrm.Bounds.Left - ClientWidth, scrm.Bounds.Top - ClientHeight, ClientWidth, scrm.Bounds.Height)
                Dim topRect As New Rectangle(scrm.Bounds.Left - ClientWidth, scrm.Bounds.Top - ClientHeight, scrm.Bounds.Width, ClientHeight)

                For Each s In allScreens
                    If (s.Bounds.IntersectsWith(leftRect) OrElse s.Bounds.IntersectsWith(topRect)) Then
                        Dim sM As ScreenManager = ScreenManager.FromScreen(s)
                        If sM.ScalingPercent() <> currentScaling Then
                            _NWScaling = False
                            Return False
                        End If
                    End If
                Next
            End If
            _NWScaling = True
            Return True
#If DEBUG Then
        Finally
            sw.Stop()
            dBug.Print($"Checking scaling modes took {sw.ElapsedMilliseconds}ms result:{_NWScaling}")
        End Try
        Return False
#End If
    End Function

    ''' <summary>
    ''' Exposes the bounds of the screen.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Bounds As Rectangle
        Get
            Return _screen.Bounds
        End Get
    End Property

    Public Shared Function CacheToScreens() As List(Of Screen)
        Return ScreenManagerCache.Keys.ToList
    End Function

    Private _screen As Screen = Nothing
    Private _Scaling As Integer? = Nothing
    Private _leftScreens As List(Of ScreenManager) = Nothing
    Private Shared ScreenManagerCache As Dictionary(Of Screen, ScreenManager) = New Dictionary(Of Screen, ScreenManager)

    Private Sub New(scr As Screen)
        _screen = scr
    End Sub

    ''' <summary>
    ''' Returns the primary screen as a ScreenManager instance
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property PrimaryScreen() As ScreenManager
        Get
            Return ScreenManager.FromScreen(Screen.PrimaryScreen)
        End Get
    End Property

    ''' <summary>
    ''' Retrieves a cached ScreenManager instance or creates a new one
    ''' </summary>
    ''' <param name="scr"></param>
    ''' <returns></returns>
    Public Shared Function FromScreen(scr As Screen) As ScreenManager
        Dim cachedManager As ScreenManager = Nothing
        If ScreenManagerCache.TryGetValue(scr, cachedManager) Then
            Return cachedManager
        Else
            dBug.Print($"ScreenManager FromScreen Cachemiss {scr}", 0)
            Dim newManager As New ScreenManager(scr)
            ScreenManagerCache.Add(scr, newManager)
            Return newManager
        End If
    End Function

    ''' <summary>
    ''' Returns the scaling percentage of the screen.
    ''' Note: this is a timeconsuming blocking operation hence we implemented several caches.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ScalingPercent() As Integer
        Get
            If Me._Scaling Is Nothing Then _Scaling = Me._ScalingPercent()

            Return _Scaling
        End Get
    End Property

    Private Const DWMWA_EXTENDED_FRAME_BOUNDS = 9
    Private Function _ScalingPercent() As Integer
        'TODO: investigate GetScaleFactorForMonitor (win8.1 and up only) and incorporate here if usefull

        If Me._screen Is Nothing Then Throw New NullReferenceException("_screen is nothing")

        Dim grab As New InactiveForm(Me._screen)
        Dim GrabHandler As EventHandler = Sub()
                                              grab.Location += New Point(1, 1) 'need to update the location so the frame changes
                                              Dim rcFrame As RECT
                                              DwmGetWindowAttribute(grab.Handle, DWMWA_EXTENDED_FRAME_BOUNDS, rcFrame, System.Runtime.InteropServices.Marshal.SizeOf(rcFrame))
                                              Dim rcWind As RECT
                                              GetWindowRect(grab.Handle, rcWind)
                                              grab.Tag = Int((rcFrame.right - rcFrame.left) / (rcWind.right - rcWind.left) * 100 / 25) * 25
                                              grab.Close()
                                              RemoveHandler grab.Shown, GrabHandler 'remove handler so GC can do its thing
                                              grab.Dispose()
                                          End Sub
        AddHandler grab.Shown, GrabHandler

        'BUG: due to this blocking operation the user cannot drag across monitor boundaries 'do not call in WM_poschange
        grab.ShowDialog()

        Return grab.Tag

    End Function

    ' Narrowing operator from ScreenManager to Screen
    Public Shared Narrowing Operator CType(sm As ScreenManager) As Screen
        Return sm._screen
    End Operator

    ' Widening operator from Screen to ScreenManager
    Public Shared Widening Operator CType(scr As Screen) As ScreenManager
        Return ScreenManager.FromScreen(scr)
    End Operator

    ' Override Equals method to compare ScreenManager instances based on screen bounds
    Public Overrides Function Equals(obj As Object) As Boolean
        If TypeOf obj Is ScreenManager Then
            Dim other As ScreenManager = CType(obj, ScreenManager)
            Return Me._screen.Bounds.Equals(other._screen.Bounds)
        End If
        Return False
    End Function

    ' Override GetHashCode method
    Public Overrides Function GetHashCode() As Integer
        Return _screen.Bounds.GetHashCode()
    End Function

    ' Implement = operator
    Public Shared Operator =(left As ScreenManager, right As ScreenManager) As Boolean
        If left Is Nothing AndAlso right Is Nothing Then Return True
        If left Is Nothing OrElse right Is Nothing Then Return False
        Return left.Equals(right)
    End Operator

    ' Implement <> operator
    Public Shared Operator <>(left As ScreenManager, right As ScreenManager) As Boolean
        Return Not (left = right)
    End Operator

    ' Implement = operator for ScreenManager and Screen
    Public Shared Operator =(left As ScreenManager, right As Screen) As Boolean
        If left Is Nothing AndAlso right Is Nothing Then Return True
        If left Is Nothing OrElse right Is Nothing Then Return False
        Return left.Equals(ScreenManager.FromScreen(right))
    End Operator

    ' Implement <> operator for ScreenManager and Screen
    Public Shared Operator <>(left As ScreenManager, right As Screen) As Boolean
        Return Not (left = right)
    End Operator

    ' Implement = operator for Screen and ScreenManager
    Public Shared Operator =(left As Screen, right As ScreenManager) As Boolean
        If left Is Nothing AndAlso right Is Nothing Then Return True
        If left Is Nothing OrElse right Is Nothing Then Return False
        Return ScreenManager.FromScreen(left).Equals(right)
    End Operator

    ' Implement <> operator for Screen and ScreenManager
    Public Shared Operator <>(left As Screen, right As ScreenManager) As Boolean
        Return Not (left = right)
    End Operator

    ''' <summary>
    ''' Clears cached screens. Needs to be run in WndProc WM_DISPLAYCHANGE message
    ''' </summary>
    Public Shared Sub resetCache()
        dBug.Print("ScreenManagerCache cleared")
        ScreenManagerCache.Clear()
    End Sub

    Public Overrides Function ToString() As String
        Return $"{_screen.DeviceName}, {_screen.Bounds}, Scaling={If(_Scaling.HasValue, _Scaling.Value.ToString(), "Not Calculated")}, IsPrimary={_screen.Primary}"
    End Function

    ' Expose the cache for testing purposes
    Public Shared ReadOnly Property Cache As Dictionary(Of Screen, ScreenManager)
        Get
            Return ScreenManagerCache
        End Get
    End Property
End Class

Partial NotInheritable Class FrmMain

    ' Function to find out if screens to the left and top are at different scaling modes
    ' TODO: replace with screenmanager calls
    ' BUG: false positive on screen to NW (and SE?)
    Private Sub CheckScreenScalingModes()


        Dim scm As ScreenManager = ScreenManager.FromScreen(Screen.FromControl(Me))
        Dim ssm As Boolean = scm.SameScalingModesNW

        Return

        '' OLD

        Dim currentScreen = Screen.FromHandle(Me.Handle)
        Dim currentScaling = currentScreen.ScalingPercent()

        Dim leftScreens As New List(Of Screen)
        Dim topScreens As New List(Of Screen)

        For Each scr As Screen In Screen.AllScreens

            If scr.Bounds.Right < currentScreen.Bounds.Left Then
                leftScreens.Add(scr)
            End If

            If scr.Bounds.Bottom < currentScreen.Bounds.Top Then
                topScreens.Add(scr)
            End If
        Next

        dBug.Print($"self {currentScreen?.Bounds} {currentScreen?.ScalingPercent}")

        For Each leftScreen In leftScreens
            dBug.Print($"left {leftScreen?.Bounds} {leftScreen?.ScalingPercent}")
            If leftScreen.ScalingPercent <> currentScaling Then
                dBug.Print($"Left screen has different scaling: {leftScreen.ScalingPercent}%")
                'pnlWarning.Show()
                Exit Sub
            End If
        Next

        For Each topScreen In topScreens
            dBug.Print($"top  {topScreen?.Bounds} {topScreen?.ScalingPercent}")
            If topScreen.ScalingPercent <> currentScaling Then
                dBug.Print($"Top screen has different scaling: {topScreen.ScalingPercent}%")
                'pnlWarning.Show()
                Exit Sub
            End If
        Next
        'pnlWarning.Hide()

    End Sub

End Class




Module ScreenExtensions

    Private Const DWMWA_EXTENDED_FRAME_BOUNDS = 9

    <System.Runtime.CompilerServices.Extension()>
    Public Function ScalingPercent(scrn As Screen) As Integer
        Return ScreenManager.FromScreen(scrn).ScalingPercent()
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function ScalingPercentTask(scrn As Screen) As Task(Of Integer)
        Dim grab As New InactiveForm(scrn)
        Dim tcs As New TaskCompletionSource(Of Integer)
        Dim GrabHandler As EventHandler = Sub()
                                              grab.Location += New Point(1, 1) 'need to update the location so the frame changes
                                              Dim rcFrame As RECT
                                              DwmGetWindowAttribute(grab.Handle, DWMWA_EXTENDED_FRAME_BOUNDS, rcFrame, System.Runtime.InteropServices.Marshal.SizeOf(rcFrame))
                                              Dim rcWind As RECT
                                              GetWindowRect(grab.Handle, rcWind)
                                              tcs.SetResult(Int((rcFrame.right - rcFrame.left) / (rcWind.right - rcWind.left) * 100 / 25) * 25)
                                              grab.Close()
                                              RemoveHandler grab.Shown, GrabHandler 'remove handler so GC can do its thing
                                              grab.Dispose()
                                          End Sub
        AddHandler grab.Shown, GrabHandler
        grab.Show()
        Return tcs.Task
    End Function

    Public NotInheritable Class InactiveForm : Inherits Form
        Protected Overloads Overrides ReadOnly Property ShowWithoutActivation() As Boolean
            Get
                Return True
            End Get
        End Property
        Public Sub New(scrn As Screen)
            Me.FormBorderStyle = FormBorderStyle.None
            Me.TransparencyKey = Color.White
            Me.BackColor = Me.TransparencyKey
            Me.ShowInTaskbar = False
            Me.StartPosition = FormStartPosition.Manual
            Me.Location = scrn.Bounds.Location
        End Sub
    End Class
End Module

#If DEBUG Then
Module ScreenManagerTests

    Sub RunTests(scrn As Screen)
        dBug.Print($"--Running ScreenManager Tests on {scrn}", 1)
        TestFromScreen(scrn)
        TestAllScreensToLeft(scrn)
        TestScalingPercent(scrn)
        TestNarrowingOperator(scrn)
        TestWideningOperator(scrn)
        TestListCachedScreens()
        TestCacheToScreens()
        TestResetCache()
    End Sub

    Sub TestFromScreen(scrn As Screen)
        Dim result As ScreenManager = ScreenManager.FromScreen(scrn)
        dBug.Print($"TestFromScreen: result is {If(result Is Nothing, "Nothing", result.ToString())}", 1)
    End Sub

    Sub TestAllScreensToLeft(scrn As Screen)
        Dim screenManager As ScreenManager = ScreenManager.FromScreen(scrn)
        Dim result As List(Of ScreenManager) = screenManager.ScreensAdjacentToLeft()
        dBug.Print($"TestAllScreensToLeft: {result.Count} Screens to the left: {String.Join(", ", result)}", 1)
    End Sub

    Sub TestScalingPercent(scrn As Screen)
        Dim screenManager As ScreenManager = ScreenManager.FromScreen(scrn)
        Dim result As Integer = screenManager.ScalingPercent()
        dBug.Print($"TestScalingPercent: result is {result}", 1)
    End Sub

    Sub TestNarrowingOperator(scrn As Screen)
        Dim screenManager As ScreenManager = ScreenManager.FromScreen(scrn)
        Dim result As Screen = CType(screenManager, Screen)
        dBug.Print($"TestNarrowingOperator: result is {If(result Is Nothing, "Nothing", result.ToString())}", 1)
    End Sub

    Sub TestWideningOperator(scrn As Screen)
        Dim result As ScreenManager = CType(scrn, ScreenManager)
        dBug.Print($"TestWideningOperator: result is {If(result Is Nothing, "Nothing", result.ToString())}", 1)
    End Sub
    Sub TestListCachedScreens()
        dBug.Print($"--Listing {ScreenManager.Cache.Count} Cached Screens--", 1)
        For Each kvp In ScreenManager.Cache
            dBug.Print($"Screen: {kvp.Key.DeviceName}, ScreenManager: {kvp.Value}", 1)
        Next
    End Sub
    Sub TestCacheToScreens()
        Dim result As List(Of Screen) = ScreenManager.CacheToScreens()
        dBug.Print($"TestCacheToScreens: result is {If(result Is Nothing, "Nothing", String.Join(", ", result))}", 1)
    End Sub
    Sub TestResetCache()
        ScreenManager.resetCache()
        Dim result As List(Of Screen) = ScreenManager.CacheToScreens()
        dBug.Print($"TestResetCache: result count is {result.Count}", 1)
    End Sub

End Module
#End If

