

Public Class ScaledScrollable

    Shared fontName As String = "Segoe UI"
    Shared fontCache As New Concurrent.ConcurrentDictionary(Of Single, Font)
    Public Shared ReadOnly Property GetFont(scale As Single) As Font
        Get
            Return fontCache.GetOrAdd(scale, Function(s) New Font(fontName, Math.Max(1, 12 * s), FontStyle.Regular, GraphicsUnit.Pixel))
        End Get
    End Property

    Private Shared _scaling As Single = 1.0!
    Public Shared Property Scaling As Single
        Get
            Return If(My.Settings.QLScale > 0, If(My.Settings.QLScale = 4, My.Settings.customQLScale, My.Settings.QLScale), _scaling)
        End Get
        Set(value As Single)
            _scaling = value
        End Set
    End Property
    Public Class ScrollButtonHook : Inherits NativeWindow

        Private _down As Boolean = False
        Private _but As Label = Nothing
        Private _parent As Control
        Private repeatTimer As New Timer()
        Private isFirstTick As Boolean = True

        Sub New(but As Label, up As Boolean)
            _but = but
            _parent = but.Parent
            _down = Not up
            AddHandler repeatTimer.Tick, AddressOf OnRepeatTick
        End Sub

        Private Sub OnRepeatTick(sender As Object, e As EventArgs)
            If isFirstTick Then
                isFirstTick = False
                repeatTimer.Interval = 50 ' Fast repeat interval after the initial hold delay
            End If

            If _but.Enabled AndAlso
                   (GetAsyncKeyState(Keys.LButton) And &H8000S) <> 0 OrElse
                   (GetAsyncKeyState(Keys.MButton) And &H8000S) <> 0 OrElse
                   (GetAsyncKeyState(Keys.RButton) And &H8000S) <> 0 OrElse
                   (GetAsyncKeyState(Keys.XButton1) And &H8000S) <> 0 OrElse
                   (GetAsyncKeyState(Keys.XButton2) And &H8000S) <> 0 Then
                ScrollDropDown(_parent, New MouseEventArgs(MouseButtons.None, 0, 0, 0, If(_down, -1, 1)))
            Else
                repeatTimer.Stop()
            End If
        End Sub

        Protected Overrides Sub WndProc(ByRef m As Message)
            Const WM_CAPTURECHANGED As Integer = &H217

            Select Case m.Msg
                Case WM_LBUTTONDOWN, WM_MBUTTONDOWN, WM_RBUTTONDOWN, WM_XBUTTONDOWN
                    isFirstTick = True
                    repeatTimer.Interval = 400 ' Initial delay (in ms) before auto-repeat kicks in
                    repeatTimer.Start()

                    ' Fire the action immediately on press down

                    ScrollDropDown(_parent, New MouseEventArgs(MouseButtons.None, 0, 0, 0, If(_down, -1, 1)))

                    Return ' Block default WinForms click action completely

                Case WM_LBUTTONUP, WM_MBUTTONUP, WM_RBUTTONUP, WM_XBUTTONUP, WM_CAPTURECHANGED
                    repeatTimer.Stop()
            End Select

            MyBase.WndProc(m)
        End Sub
    End Class
    Private Shared activeHooks As New List(Of ScrollButtonHook)()
    Public Class ContextMenuStrip : Inherits Windows.Forms.ContextMenuStrip

        Private StickyLabels_Initialized As Boolean = False

        Protected Overrides Sub WndProc(ByRef m As Message)
            MyBase.WndProc(m)
        End Sub

        Sub New(Container As System.ComponentModel.IContainer)
            MyBase.New(Container)
            Me.Font = GetFont(Scaling)
            Me.ImageScalingSize = New Size(16 * Scaling, 16 * Scaling)
        End Sub


        Protected Overrides Sub OnOpening(e As System.ComponentModel.CancelEventArgs)
            Dim dpi As UInteger = 96
            GetDpiForMonitor(MonitorFromPoint(Cursor.Position, 2), 0, dpi, dpi)
            Scaling = CSng(dpi) / 96.0!
            Me.Font = GetFont(Scaling)
            Me.ImageScalingSize = New Size(CInt(16 * Scaling), CInt(16 * Scaling))

            MyBase.OnOpening(e)

            For Each hook In activeHooks
                hook.ReleaseHandle()
            Next
            activeHooks.Clear()

            Dim lbls = Me.Controls.OfType(Of Label).ToArray
            Dim first = lbls?.FirstOrDefault
            For Each itm As Label In lbls
                itm.BackColor = If(My.Settings.DarkMode, Color.LightGray, SystemColors.Control)
                itm.Image = GetTriangle(itm Is first, Scaling)

                Dim hook As New ScrollButtonHook(itm, itm Is first)
                hook.AssignHandle(itm.Handle)
                activeHooks.Add(hook)
            Next
        End Sub

        Public Overrides Property Font As Font
            Get
                Return GetFont(Scaling)
            End Get
            Set(value As Font)
                fontName = value.FontFamily.Name
                fontCache.Clear()
            End Set
        End Property

        Protected Overrides Sub Dispose(disposing As Boolean)
            fadeSwDict.Clear()
            MyBase.Dispose(disposing)
        End Sub
        Public Sub ContextMenuStrip_MouseWheel(sender As ToolStripDropDown, e As MouseEventArgs) Handles Me.MouseWheel
            ScaledScrollable.ScrollDropDown(sender, e)
        End Sub
    End Class


    Private Shared ReadOnly miScrollInternal As Reflection.MethodInfo = GetType(ToolStripDropDownMenu).GetMethod("ScrollInternal",
                                                  Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance, Nothing,
                                                  New Type() {GetType(Integer)}, Nothing)

    Private Shared Sub ScrollDropDown(sender As ToolStripDropDown, e As MouseEventArgs)
        'this custom implementation is needed to handle a bug in base class scroll behaviour

        Dim firstItem As ToolStripItem = sender.GetNextItem(Nothing, ArrowDirection.Down)
        Dim lastItem As ToolStripItem = sender.GetNextItem(Nothing, ArrowDirection.Up)

        If firstItem Is Nothing OrElse lastItem Is Nothing Then Exit Sub

        Dim labels = sender.Controls.OfType(Of Label).ToArray()

        If labels.Length <> 2 AndAlso Not labels(0).Visible AndAlso Not labels(1).Visible Then Exit Sub

        Dim topButton As Label = labels(0)     ' Up button
        Dim bottomButton As Label = labels(1)  ' Down button


        If e.Delta > 0 Then
            If Not topButton.Enabled Then
                FadeBackColor(topButton, Color.Pink, If(My.Settings.DarkMode, Color.LightGray, SystemColors.Control), 400)
                Exit Sub
            End If
        Else
            If Not bottomButton.Enabled Then
                FadeBackColor(bottomButton, Color.Pink, If(My.Settings.DarkMode, Color.LightGray, SystemColors.Control), 400)
                Exit Sub
            End If
        End If

        ' Call internal scroll

        If miScrollInternal IsNot Nothing Then
            If e.Delta < 0 Then
                If bottomButton.Visible Then miScrollInternal.Invoke(sender, {firstItem.Height})
            Else
                If topButton.Visible Then miScrollInternal.Invoke(sender, {-firstItem.Height})
            End If
        End If

        'update scrollbuttons

        If e.Delta < 0 Then
            ' Scrolling Down: Check if the last item has scrolled up into/past the bottom viewable area
            If lastItem.Bounds.Bottom <= sender.ClientSize.Height - bottomButton.Height Then
                bottomButton.Enabled = False
            Else
                topButton.Enabled = True
                bottomButton.Enabled = True
            End If
        Else
            ' Scrolling Up: Check if the first item has scrolled down into/past the top viewable area
            If firstItem.Bounds.Top >= topButton.Height Then
                topButton.Enabled = False
            Else
                topButton.Enabled = True
                bottomButton.Enabled = True
            End If
        End If

        ' close dropdowns whose item is out of bounds

        Dim CRwoBut = New Rectangle(sender.ClientRectangle.Left, sender.ClientRectangle.Top + topButton.Height, sender.ClientRectangle.Width, sender.ClientRectangle.Height - topButton.Height - bottomButton.Height)
        For Each itm As ToolStripMenuItem In sender.Items.OfType(Of ToolStripMenuItem).Where(Function(dd) dd.HasDropDown AndAlso dd.DropDown.Visible)
            If Not CRwoBut.IntersectsWith(itm.Bounds) Then
                itm.DropDown.Close()
            End If
        Next

    End Sub

    Private Shared fadeSwDict As New Concurrent.ConcurrentDictionary(Of IntPtr, Stopwatch)
    Private Shared Sub FadeBackColor(ctrl As Control, fromColor As Color, toColor As Color, durationMs As Integer)

        If ctrl Is Nothing OrElse ctrl.IsDisposed OrElse Not ctrl.Visible Then Exit Sub

        If durationMs <= 0 Then
            ctrl.BackColor = toColor
            Exit Sub
        End If

        Dim handle As IntPtr = ctrl.Handle

        Dim fadeSw As Stopwatch = fadeSwDict.GetOrAdd(handle, Function(h) New Stopwatch())

        If fadeSw.ElapsedMilliseconds = 0 Then
            Debug.Print($"spawnning fadetask {handle}")
            fadeSw.Start()
            Task.Run(Sub()
                         Dim progress As Double = 0.0
                         Dim lastcolor = Color.Empty
                         Dim rDelta As Integer = CInt(toColor.R) - fromColor.R
                         Dim gDelta As Integer = CInt(toColor.G) - fromColor.G
                         Dim bDelta As Integer = CInt(toColor.B) - fromColor.B
                         Do

                             progress = Math.Min(fadeSw.ElapsedMilliseconds / durationMs, 1.0)

                             Dim r As Integer = fromColor.R + rDelta * progress
                             Dim g As Integer = fromColor.G + gDelta * progress
                             Dim b As Integer = fromColor.B + bDelta * progress

                             Dim newColor = Color.FromArgb(r, g, b)

                             If newColor <> lastcolor Then
                                 If ctrl.IsDisposed Then Exit Do
                                 ctrl.BeginInvoke(Sub() ctrl.BackColor = newColor)
                                 lastcolor = newColor
                             End If

                             DwmFlush()

                         Loop While fadeSw.ElapsedMilliseconds <= durationMs

                         ' Ensure exact final color
                         If Not ctrl.IsDisposed Then
                             ctrl.BeginInvoke(Sub() ctrl.BackColor = toColor)
                         End If

                         fadeSwDict.TryRemove(handle, Nothing)
                     End Sub)
        Else
            fadeSw.Restart()
        End If

    End Sub
    <Runtime.InteropServices.DllImport("dwmapi.dll")>
    Private Shared Function DwmFlush() As Integer : End Function
    Public Class ToolStripMenuItem : Inherits Windows.Forms.ToolStripMenuItem

        Sub New(text As String, image As Image)
            MyBase.New(text, image)
            'Me.DropDown?.Dispose()
            'Me.DropDown = New CustomContextMenu.ToolStripDropDownMenu With {.Renderer = frmMain.cmsQL.Renderer, .Font = GetFont(Scaling), .ImageScalingSize = New Size(16 * Scaling, 16 * Scaling)}
        End Sub

        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            If Me.HasDropDown AndAlso Me.DropDown.Visible Then
                Exit Sub 'fix dropdown shifting back to top on overflow
            End If
            MyBase.OnMouseEnter(e)
        End Sub

        Protected Overrides Function CreateDefaultDropDown() As ToolStripDropDown
            Return New ScaledScrollable.ToolStripDropDownMenu() With {
                .OwnerItem = Me,
                .Renderer = Nothing,
                .Font = GetFont(Scaling),
                .ImageScalingSize = New Size(16 * Scaling, 16 * Scaling)
            }
        End Function
        Protected Overrides Sub OnDropDownShow(e As EventArgs)
            MyBase.OnDropDownShow(e)

            If Me.Owner IsNot Nothing AndAlso Me.DropDown IsNot Nothing AndAlso Me.DropDown.Renderer Is Nothing Then
                Me.DropDown.Renderer = Me.Owner.Renderer
            End If
        End Sub

    End Class

    Public Class ToolStripDropDownMenu : Inherits Windows.Forms.ToolStripDropDownMenu

        Dim StickyLabels_initialized As Boolean = False

        Protected Overrides Sub OnOpened(e As EventArgs)
            MyBase.OnOpened(e)

            If Not StickyLabels_initialized Then

                Dim lbls = Me.Controls.OfType(Of Label).ToArray
                Dim first = lbls?.FirstOrDefault
                For Each itm As Label In lbls
                    itm.BackColor = If(My.Settings.DarkMode, Color.LightGray, SystemColors.Control)
                    itm.Image = GetTriangle(itm Is first, Scaling)

                    Dim hook As New ScrollButtonHook(itm, itm Is first)
                    hook.AssignHandle(itm.Handle)
                    activeHooks.Add(hook)
                Next

                StickyLabels_initialized = True
            End If

        End Sub

        Public Sub ToolStripDropDownMenu_MouseWheel(sender As ToolStripDropDown, e As MouseEventArgs) Handles Me.MouseWheel
            ScaledScrollable.ScrollDropDown(sender, e)
        End Sub
    End Class

    Private Shared Function OddInt(dbl As Single) As Integer
        Dim int As Integer = CInt(dbl)
        If int Mod 2 = 0 Then int -= 1
        If int <= 0 Then Return 1
        Return int
    End Function

    Private Shared TriangleDict As New Concurrent.ConcurrentDictionary(Of Tuple(Of Boolean, Single), Bitmap)
    Private Shared Function GetTriangle(up As Boolean, scaling As Single) As Image

        Return TriangleDict.GetOrAdd(New Tuple(Of Boolean, Single)(up, scaling),
                                     Function()
                                         Dim width = OddInt(9 * scaling)
                                         Dim extrapadding As Integer = Math.Max(1, Math.Floor(2 * (scaling - 1.0!)))
                                         Dim bmp As New Bitmap(width, (width + 1) \ 2 + extrapadding * 2)

                                         Debug.Print($"Triangle {scaling} size {bmp.Size} pad {extrapadding}")

                                         Dim mid As Integer = bmp.Width \ 2

                                         For i As Integer = extrapadding To bmp.Height - 1 - extrapadding

                                             Dim triRow As Integer = i - extrapadding

                                             Dim y As Integer

                                             If up Then
                                                 y = i
                                             Else
                                                 y = bmp.Height - 1 - i
                                             End If

                                             For x As Integer = mid - triRow To mid + triRow
                                                 If x >= 0 AndAlso x < bmp.Width Then
                                                     bmp.SetPixel(x, y, Color.Black)
                                                 End If
                                             Next

                                         Next

                                         Return bmp
                                     End Function)


    End Function


End Class

