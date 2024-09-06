Public NotInheritable Class FrmBehind

    Private Sub FrmBehind_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        Debug.Print($"behind down {e.Button}")
        Try
            If FrmMain.AltPP IsNot Nothing Then
                AppActivate(FrmMain.AltPP.Id)
            Else
                FrmMain.BringToFront()
                AppActivate(FrmMain.scalaPID)
            End If
        Catch ex As Exception

        End Try

        Dim msg As Integer = WM_XBUTTONDOWN
        Select Case e.Button
            Case MouseButtons.Left
                msg = WM_LBUTTONDOWN
            Case MouseButtons.Right
                msg = WM_RBUTTONDOWN
            Case MouseButtons.Middle
                msg = WM_MBUTTONDOWN
        End Select

        If FrmMain.AltPP IsNot Nothing Then SendMessage(FrmMain.AltPP.MainWindowHandle, msg, 0, IntPtr.Zero)

    End Sub

    Private Sub FrmBehind_Click(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        Debug.Print($"behind up {e.Button}")
        Try
            If FrmMain.AltPP IsNot Nothing Then
                AppActivate(FrmMain.AltPP.Id)
            Else
                FrmMain.BringToFront()
                AppActivate(FrmMain.scalaPID)
            End If
        Catch ex As Exception
            FrmMain.BringToFront()
        End Try

        Dim msg As Integer = WM_XBUTTONUP
        Select Case e.Button
            Case MouseButtons.Left
                msg = WM_LBUTTONUP
            Case MouseButtons.Right
                msg = WM_RBUTTONUP
            Case MouseButtons.Middle
                msg = WM_MBUTTONUP
        End Select

        If FrmMain.AltPP IsNot Nothing Then SendMessage(FrmMain.AltPP.MainWindowHandle, msg, 0, IntPtr.Zero)

    End Sub

    'Private Sub FrmBehind_Load(sender As Object, e As EventArgs)
    '    Me.FormBorderStyle = FormBorderStyle.None
    '    Me.DoubleBuffered = False
    '    Me.SetStyle(ControlStyles.ResizeRedraw, False)
    'End Sub

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or WindowStylesEx.WS_EX_TOOLWINDOW 'hide from alt-tab
            Return cp
        End Get
    End Property


    Public Overloads Property Bounds() As Rectangle
        Get
            Dim rc As Rectangle = MyBase.Bounds
            Return rc
        End Get
        Set(ByVal v As Rectangle)
            SetWindowPos(Me.Handle, IntPtr.Zero, v.X, v.Y, v.Width, v.Height,
                            SetWindowPosFlags.IgnoreZOrder Or SetWindowPosFlags.DoNotActivate Or SetWindowPosFlags.DoNotSendChangingEvent)
        End Set
    End Property

End Class