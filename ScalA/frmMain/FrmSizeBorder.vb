Public Class FrmSizeBorder
    Private Sub FrmSizeBorder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.None
        Me.DoubleBuffered = False
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        Dim bru As Brush = Brushes.Black
#If DEBUG Then
        bru = Brushes.Red
#End If

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
            cp.ExStyle = cp.ExStyle Or &H80 'WS_EX_TOOLWINDOW 'hide from alt-tab
            Return cp
        End Get
    End Property

    Private Const HTLEFT As Integer = 10, HTRIGHT As Integer = 11, HTTOP As Integer = 12, HTTOPLEFT As Integer = 13, HTTOPRIGHT As Integer = 14, HTBOTTOM As Integer = 15, HTBOTTOMLEFT As Integer = 16, HTBOTTOMRIGHT As Integer = 17
    Public suppressWM_SIZING As Boolean = True
    Private prevRC As New Rectangle
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)

        Select Case m.Msg
            Case WM_SIZING
                If Not suppressWM_SIZING Then
                    FrmMain.moveBusy = True
                    Dim rc As RECT = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(RECT))
                    Dim sr As New Rectangle(rc.Left + BorderSize, rc.Top + BorderSize, rc.Right - rc.Left - BorderSize * 2, rc.Bottom - rc.Top - BorderSize * 2)
                    If sr <> prevRC Then
                        SetWindowPos(FrmMain.ScalaHandle, IntPtr.Zero, sr.X, sr.Y, sr.Width, sr.Height,
                                 SetWindowPosFlags.ASyncWindowPosition Or SetWindowPosFlags.IgnoreZOrder)
                        FrmMain.ReZoom(New Size(sr.Width - 2, sr.Height - FrmMain.pnlTitleBar.Height - 1))
                        'If prevRC.Location = sr.Location AndAlso prevRC <> sr Then
                        '    FrmMain.AltPP?.CenterBehind(FrmMain.pbZoom)
                        'End If
                        prevRC = sr
                        End If
                    End If
            Case WM_EXITSIZEMOVE
                FrmMain.AltPP?.CenterBehind(FrmMain.pbZoom)
                FrmMain.moveBusy = False
        End Select


        MyBase.WndProc(m)

        If m.Msg = WM_NCHITTEST Then
            Dim mp = Me.PointToClient(Cursor.Position)
            Debug.Print($"WM_NCHITTEST")
            If TopLeftRC.Contains(mp) Then
                Debug.Print($"topleft true")
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
    End Sub

    Const BorderSize As Integer = 8

    Function TopRC() As Rectangle
        Return New Rectangle(0, 0, Me.ClientSize.Width, BorderSize)
    End Function

    Function LeftRC() As Rectangle
        Return New Rectangle(0, 0, BorderSize, Me.ClientSize.Height)
    End Function

    Function BotRC() As Rectangle
        Return New Rectangle(0, Me.ClientSize.Height - BorderSize, Me.ClientSize.Width, BorderSize)
    End Function

    Function RightRC() As Rectangle
        Return New Rectangle(Me.ClientSize.Width - BorderSize, 0, BorderSize, Me.ClientSize.Height)
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