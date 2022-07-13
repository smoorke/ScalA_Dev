Public Class WindowProc
    'dummy class to prevent form generation
End Class

Partial Class FrmMain
    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case Hotkey.WM_HOTKEY
                Select Case m.WParam
                    Case 1 'ctrl-tab
                        'only preform switch when astonia or scala Is active
                        Dim activeID = GetActiveProcessID()
                        Debug.Print("aID " & activeID & " selfPID " & scalaPID)
                        If activeID = scalaPID OrElse Process.GetProcessById(activeID).IsClassNameIn(My.Settings.className) Then
                            If Me.WindowState = FormWindowState.Minimized Then
                                Me.WindowState = FormWindowState.Normal
                                If wasMaximized Then
                                    Dim tmp As Point = RestoreLoc
                                    btnMax.PerformClick()
                                    RestoreLoc = tmp
                                End If
                            End If
                            Me.Activate()
                            Me.BringToFront()
                            btnStart.PerformClick()
                        End If
                    Case 2 'ctrl-space
                        Cycle()
                    Case 3 'ctrl-shift-space
                        Cycle(True)
                End Select
            Case WM_SYSCOMMAND
                Select Case m.WParam
                    Case SC_RESTORE
                        Debug.Print("SC_RESTORE " & m.LParam.ToString)
                        SetWindowLong(Me.Handle, GWL_HWNDPARENT, AltPP.MainWindowHandle)
                        'Me.ShowInTaskbar = False
                        moveBusy = False
                        If WindowState = FormWindowState.Maximized Then
                            btnMax.PerformClick()
                            Exit Sub
                        End If
                        Debug.Print("wasMax " & wasMaximized)
                        If wasMaximized Then
                            'PostMessage(ScalaHandle, WM_SYSCOMMAND, SC_MAXIMIZE, IntPtr.Zero)
                            Me.WndProc(Message.Create(ScalaHandle, WM_SYSCOMMAND, SC_MAXIMIZE, IntPtr.Zero))
                            Exit Sub
                        End If
                        SysMenu.Enable(SC_MOVE)
                        suppressWM_MOVEcwp = True
                        MyBase.DefWndProc(m)
                        suppressWM_MOVEcwp = False
                        Exit Sub
                    Case SC_MAXIMIZE
                        Debug.Print("SC_MAXIMIZE " & m.LParam.ToString)
                        If Me.WindowState = FormWindowState.Minimized Then
                            Me.WindowState = FormWindowState.Normal
                            Me.Location = RestoreLoc
                        End If
                        btnMax.PerformClick()
                        SysMenu.Disable(SC_MOVE)
                        Debug.Print("wasMax " & wasMaximized)
                        m.Result = 0
                    Case SC_MINIMIZE
                        Debug.Print("SC_MINIMIZE")
                        wasMaximized = (Me.WindowState = FormWindowState.Maximized)
                        Debug.Print("wasMax " & wasMaximized)
                        If Not wasMaximized Then
                            RestoreLoc = Me.Location
                            Debug.Print("restoreLoc " & RestoreLoc.ToString)
                        End If
                        SetWindowLong(Me.Handle, GWL_HWNDPARENT, restoreParent)
                        AstoniaProcess.RestorePos(True)
                        SysMenu.Disable(SC_MOVE)
                    Case SC_SIZE
                        SendMessage(FrmSizeBorder.Handle, WM_SYSCOMMAND, SC_SIZE, IntPtr.Zero)
                        m.Result = 0
                        Exit Sub
                    Case &H8000 + 1337
                        Debug.Print("Settings called by 1337")
                        FrmSettings.Show()
                        FrmSettings.WindowState = FormWindowState.Normal
                End Select
            Case WM_MOVE
                'Debug.Print($"WM_MOVE {Me.WindowState}")
                Me.Cursor = Cursors.Default
                'frmCaptureClickBehind.Bounds = Me.RectangleToScreen(pbZoom.Bounds)
                If AltPP?.IsRunning AndAlso Not FrmSettings.chkDoAlign.Checked AndAlso Me.WindowState <> FormWindowState.Minimized Then
#If DEBUG Then
                    pbZoom.Visible = True
#End If
                    If Not suppressWM_MOVEcwp Then
                        'Debug.Print($"moveBusy true")
                        moveBusy = True
                        Task.Run(Sub()
                                     'Exit Sub
                                     AltPP?.CenterWindowPos(ScalaHandle,
                                                        Me.Left + pbZoom.Left + (pbZoom.Width / 2),
                                                        Me.Top + pbZoom.Top + (pbZoom.Height / 2),
                                                        SetWindowPosFlags.DoNotActivate Or SetWindowPosFlags.ASyncWindowPosition)

                                 End Sub)
                    End If
                End If
            Case WM_EXITSIZEMOVE
                Debug.Print($"WM_EXITSIZEMOVE")
                moveBusy = False
            Case WM_SIZE ' = &h0005
                Dim width As Integer = LOWORD(m.LParam)
                Dim height As Integer = HIWORD(m.LParam)
                Debug.Print($"WM_SIZE {m.WParam} {width}x{height}")
                'frmCaptureClickBehind.Size = New Size(width - 2, height - pnlTitleBar.Height)
                If m.WParam = 2 Then 'maximized
                    ReZoom(New Drawing.Size(width, height))
                End If
                FrmBehind.Bounds = New Rectangle(Me.Left, Me.Top, width, height)
                FrmSizeBorder.Bounds = New Rectangle(Me.Left, Me.Top, width, height)
                If Me.Location = prevLoc Then
                    If Not sizeMoveBusy Then
                        sizeMoveBusy = True
                        moveBusy = True
                        Task.Run(Sub()
                                     Try
                                         AltPP?.CenterWindowPos(ScalaHandle,
                                                        Me.Left + pbZoom.Left + (pbZoom.Width / 2),
                                                        Me.Top + pbZoom.Top + (pbZoom.Height / 2),
                                                        SetWindowPosFlags.DoNotActivate Or SetWindowPosFlags.DoNotChangeOwnerZOrder)
                                     Catch
                                     Finally
                                         sizeMoveBusy = False
                                         ' moveBusy = False
                                     End Try
                                 End Sub)
                    End If
                End If
                If Me.cmbResolution.SelectedIndex > 0 Then Me.moveBusy = False
                prevLoc = Me.Location
            Case WM_WINDOWPOSCHANGING
                'If posChangeBusy Then
                '    Debug.Print("WM_WINDOWPOSCHANGING busy")
                '    m.Result = 0
                '    Exit Sub
                'End If
            Case WM_WINDOWPOSCHANGED 'handle dragging of maximized window
                'If posChangeBusy Then
                '    Debug.Print("WM_WINDOWPOSCHANGED busy")
                '    m.Result = 0
                '    Exit Sub
                'End If
                Dim winpos As WINDOWPOS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                If winpos.x <> 0 AndAlso winpos.y <> 0 AndAlso frmBehind IsNot Nothing Then
                    'Dim wr As Rectangle
                    'GetWindowRect(FrmCaptureClickBehind.Handle, wr)
                    'Dim ptt As Point
                    'ClientToScreen(FrmCaptureClickBehind.Handle, ptt)
                    'Dim bordersize = ptt.X - wr.Left
                    'Dim captionsize = ptt.Y - wr.Top
                    'FrmCaptureClickBehind.Location = New Point(winpos.x - bordersize, winpos.y - 3)
                    'FrmCaptureClickBehind.ClientSize = New Size(winpos.cx, winpos.cy - captionsize)
                    frmBehind.Bounds = New Rectangle(winpos.x, winpos.y, winpos.cx, winpos.cy)
                    FrmSizeBorder.Bounds = New Rectangle(winpos.x, winpos.y, winpos.cx, winpos.cy)
                End If
                If wasMaximized AndAlso caption_Mousedown AndAlso Not posChangeBusy Then
                    'Dim winpos As WINDOWPOS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                    'winpos.flags = SetWindowPosFlags.IgnoreMove
                    Debug.Print("WM_WINDOWPOSCHANGED from maximized and mousebutton down")
                    Debug.Print($"hwndInsertAfter {winpos.hwndInsertAfter}")
                    Debug.Print($"flags {winpos.flags}")
                    Debug.Print($"pos {winpos.x} {winpos.y} size {winpos.cx} {winpos.cy}")
                    btnMax.Text = "⧠"
                    ttMain.SetToolTip(btnMax, "Maximize")
                    cmbResolution.Enabled = True
                    wasMaximized = False
                    posChangeBusy = True
                    AOshowEqLock = False
                    Me.Location = New Point(winpos.x, winpos.y)

                    Me.WindowState = FormWindowState.Normal
                    ReZoom(New Drawing.Size(winpos.cx - 2, winpos.cy - pnlTitleBar.Height - 1))
                    cmbResolution.SelectedIndex = My.Settings.zoom

                    AltPP?.CenterBehind(pbZoom)
                    pnlTitleBar.Width = winpos.cx - pnlButtons.Width - pnlSys.Width
                    Debug.Print($"winpos location {New Point(winpos.x, winpos.y)}")
                    Debug.Print($"winpos size {New Size(winpos.cx, winpos.cy)}")
                    'handle sysmenu max/restore worng
                    SysMenu.Disable(SC_RESTORE)
                    SysMenu.Enable(SC_MAXIMIZE)
                    'System.Runtime.InteropServices.Marshal.StructureToPtr(winpos, m.LParam, True)
                    FrmSizeBorder.Opacity = If(chkDebug.Checked, 1, 0.01)
                    posChangeBusy = False
                    Exit Sub
                End If
            Case WM_WININICHANGE '&H1A
                If m.LParam = IntPtr.Zero AndAlso Me.WindowState = FormWindowState.Maximized Then
                    Debug.Print($"WM_WININICHANGE {m.LParam}")
                    'handle taskbar changing
                    Dim newWA = Screen.FromPoint(Me.Location + New Point(Me.Width / 2, Me.Height / 2)).WorkingArea
                    'only do adjustment when size change or moved from top/bottom to sides
                    If newWA.Height <> prevWA.Height OrElse newWA.Width <> prevWA.Width Then
                        Debug.Print($"Taskbar changed {prevWA}->{newWA}")
                        Me.WindowState = FormWindowState.Normal
                        btnMax.PerformClick()
                    End If
                End If
            Case WM_ENTERMENULOOP
                Debug.Print($"WM_ENTERMENULOOP {cmsQuickLaunch.Visible}")
                SysMenu.Visible = Not cmsQuickLaunch.Visible
            Case WM_EXITMENULOOP
                Debug.Print("WM_EXITMENULOOP")
                SysMenu.Visible = False
            Case WM_INITMENU
                Debug.Print($"WM_INITMENU {m.WParam} {SysMenu.Handle}")
                If Me.WindowState = FormWindowState.Normal Then
                    SysMenu.Enable(SC_SIZE)
                    SysMenu.Enable(SC_MOVE)
                Else
                    SysMenu.Disable(SC_SIZE)
                    SysMenu.Disable(SC_MOVE)
                End If
#If DEBUG Then

            Case &H6 ' WM_AACTIVATE
            Case &H7 ' WM_SETFOCUS
            Case &H8 ' WM_KILLFOCUS
            Case &HF ' WM_PAINT

            Case &HC ' WM_SETTEXT
            Case &HD ' WM_GETTEXT 
            Case &HE ' WM_GETTEXTLENGTH



            Case &H1C ' WM_ACTIVATEAPP 

            Case &H20 '	WM_SETCURSOR
            Case &H21 ' WM_MOUSEACTIVATE
            Case &H24 ' WM_GETMINMAXINFO

            Case &H7F ' WM_GETICON 
            Case &H84 ' WM_NCHITTEST
            Case &H86 ' WM_NCACTIVATE

            Case &HA1 ' WM_NCLBUTTONDOWN

            Case &H104 ' WM_SYSKEYDOWN
            Case &H105 ' WM_SYSKEYUP

            Case &H121 ' WM_ENTERIDLE

            Case &H200 ' WM_MOUSEMOVE
                Debug.Print($"WM_MOUSEMOVE {Nothing} {Nothing}")

            Case &H210 ' WM_PARENTNOTIFY 
            Case &H215 ' WM_CAPTURECHANGED
            Case &H216 ' WM_MOVEING

            Case &H281 ' WM_IME_SETCONTEXT
            Case &H282 ' WM_IME_NOTIFY 

            Case &H2A1 ' WM_MOUSEHOVER 
            Case &H2A3 ' WM_MOUSELEAVE

            Case &HC0EA To &HC1CF ' unknown

            Case Else
                Debug.Print($"Unhandeld WM_ 0x{m.Msg:X8} &H{m.Msg:X8}")
#End If
        End Select

        MyBase.WndProc(m)  ' allow form to process this message
    End Sub

    Dim prevLoc As Point
    Dim sizeMoveBusy As Boolean = False

End Class