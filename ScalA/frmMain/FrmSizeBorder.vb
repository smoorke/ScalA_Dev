Public NotInheritable Class FrmSizeBorder
    Private Sub FrmSizeBorder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.None
        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        Me.MinimumSize = New Size(400 + BorderSize * 2 + 2, 300 + BorderSize * 2 + FrmMain.pnlTitleBar.Height + 1)
    End Sub

#If DEBUG Then
    Private ReadOnly bru As Brush = Brushes.Red
#Else
    Private ReadOnly bru As Brush = Brushes.Black
#End If
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        e.Graphics.FillRectangle(bru, TopRC)
        e.Graphics.FillRectangle(bru, LeftRC)
        e.Graphics.FillRectangle(bru, RightRC)
        e.Graphics.FillRectangle(bru, BotRC)

        Dim xb As Integer = BorderSize + If(My.Settings.roundCorners, 2, 0)

        e.Graphics.FillRectangle(bru, New Rectangle(0, 0, xb, xb))
        e.Graphics.FillRectangle(bru, New Rectangle(Me.ClientSize.Width - xb, 0, xb, xb))
        e.Graphics.FillRectangle(bru, New Rectangle(0, Me.ClientSize.Height - xb, xb, xb))
        e.Graphics.FillRectangle(bru, New Rectangle(Me.ClientSize.Width - xb, Me.ClientSize.Height - xb, xb, xb))
    End Sub

    Private Sub FrmBehind_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        suppressWM_SIZING = False
    End Sub

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or WindowStylesEx.WS_EX_TOOLWINDOW 'hide from alt-tab
            'cp.ExStyle = cp.ExStyle Or WindowStylesEx.WS_EX_LAYERED
            Return cp
        End Get
    End Property

    Private Const HTLEFT As Integer = 10, HTRIGHT As Integer = 11, HTTOP As Integer = 12, HTTOPLEFT As Integer = 13, HTTOPRIGHT As Integer = 14, HTBOTTOM As Integer = 15, HTBOTTOMLEFT As Integer = 16, HTBOTTOMRIGHT As Integer = 17
    Public suppressWM_SIZING As Boolean = True
    Private prevSR As New Rectangle
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)

        Select Case m.Msg
            Case WM_SIZING
                If Not suppressWM_SIZING Then
                    Dim rc As RECT = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(RECT))
                    Dim sr As New Rectangle(rc.Left + BorderSize, rc.Top + BorderSize, rc.Right - rc.Left - BorderSize * 2, rc.Bottom - rc.Top - BorderSize * 2)
                    If sr <> prevSR Then
                        FrmMain.Bounds = sr
                        Dim zoomSZ As New Size(sr.Width - 2, sr.Height - FrmMain.pnlTitleBar.Height - 1)
                        My.Settings.resol = zoomSZ
                        My.Settings.zoom = 0
                        FrmMain.ReZoom(zoomSZ)
                        prevSR = sr
                    End If
                End If
        End Select

        MyBase.WndProc(m)

        If m.Msg = WM_NCHITTEST Then
            Dim mp = Me.PointToClient(Cursor.Position)
            If TopLeftRC.Contains(mp) Then
                m.Result = CType(HTTOPLEFT, IntPtr)
            ElseIf TopRightRC.Contains(mp) Then
                m.Result = CType(HTTOPRIGHT, IntPtr)
            ElseIf BotLeftRC.Contains(mp) Then
                m.Result = CType(HTBOTTOMLEFT, IntPtr)
            ElseIf BotRightRC.Contains(mp) Then
                m.Result = CType(HTBOTTOMRIGHT, IntPtr)
            ElseIf TopRC.Contains(mp) Then
                m.Result = CType(HTTOP, IntPtr)
            ElseIf LeftRC.Contains(mp) Then
                m.Result = CType(HTLEFT, IntPtr)
            ElseIf RightRC.Contains(mp) Then
                m.Result = CType(HTRIGHT, IntPtr)
            ElseIf BotRC.Contains(mp) Then
                m.Result = CType(HTBOTTOM, IntPtr)
            End If
        End If
        If m.Msg = WM_ENTERSIZEMOVE Then
            Debug.Print("border WM_ENTERSIZEMOVE")
            FrmMain.Resizing = True
        End If
        If m.Msg = WM_EXITSIZEMOVE Then
            Debug.Print("Border WM_EXITSIZEMOVE")
            FrmMain.ResumeLayout()
            FrmMain.Invalidate()
            FrmMain.Resizing = False
        End If
    End Sub

    Const BorderSize As Integer = 6

    Function TopRC() As Rectangle
        Return New Rectangle(0, 0, Me.ClientSize.Width, BorderSize + 1)
    End Function

    Function LeftRC() As Rectangle
        Return New Rectangle(0, 0, BorderSize + 1, Me.ClientSize.Height)
    End Function

    Function BotRC() As Rectangle
        Return New Rectangle(0, Me.ClientSize.Height - BorderSize - 1, Me.ClientSize.Width, BorderSize + 1)
    End Function

    Function RightRC() As Rectangle
        Return New Rectangle(Me.ClientSize.Width - BorderSize - 1, 0, BorderSize + 1, Me.ClientSize.Height)
    End Function

    Function TopLeftRC() As Rectangle
        Return New Rectangle(0, 0, BorderSize * 2, BorderSize * 2)
    End Function

    Function TopRightRC() As Rectangle
        Return New Rectangle(Me.ClientSize.Width - BorderSize * 2, 0, BorderSize * 2, BorderSize * 2)
    End Function

    Function BotLeftRC() As Rectangle
        Return New Rectangle(0, Me.ClientSize.Height - BorderSize * 2, BorderSize * 2, BorderSize * 2)
    End Function

    Function BotRightRC() As Rectangle
        Return New Rectangle(Me.ClientSize.Width - BorderSize * 2, Me.ClientSize.Height - BorderSize * 2, BorderSize * 2, BorderSize * 2)
    End Function


    Public Overloads Property Bounds() As Rectangle
        Get
            Dim rc As Rectangle = MyBase.Bounds
            Return rc
        End Get
        Set(ByVal v As Rectangle)
            SetWindowPos(Me.Handle, IntPtr.Zero, v.X - BorderSize, v.Y - BorderSize, v.Width + BorderSize * 2, v.Height + BorderSize * 2,
                            SetWindowPosFlags.IgnoreZOrder Or SetWindowPosFlags.DoNotActivate Or SetWindowPosFlags.DoNotSendChangingEvent)
        End Set
    End Property

End Class