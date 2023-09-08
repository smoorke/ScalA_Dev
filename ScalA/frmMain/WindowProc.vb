Public NotInheritable Class WindowProc
    'dummy class to prevent form generation
End Class

Partial NotInheritable Class FrmMain
    Dim SuppressWININICHANGECounter As Integer = 0
    Dim ThemeChanging As Boolean = False
    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case Hotkey.WM_HOTKEY
                Debug.Print($"Hotkey {m.WParam} pressed")
                Select Case m.WParam
                    Case 1 'ctrl-tab
                        'only preform switch when astonia or scala Is active
                        Dim activeID = GetActiveProcessID()
                        Debug.Print("aID " & activeID & " selfPID " & scalaPID)
                        If activeID = scalaPID OrElse Process.GetProcessById(activeID).HasClassNameIn(My.Settings.className) Then
                            'If Me.WindowState = FormWindowState.Minimized Then
                            '    Me.WindowState = FormWindowState.Normal
                            '    If wasMaximized Then
                            '        Dim tmp As Point = RestoreLoc
                            '        btnMax.PerformClick()
                            '        RestoreLoc = tmp
                            '    End If
                            'End If
                            If Me.WindowState = FormWindowState.Minimized Then
                                SendMessage(ScalaHandle, WM_SYSCOMMAND, SC_RESTORE, IntPtr.Zero)
                            End If
                            Me.Activate()
                            Me.BringToFront()
                            btnStart.PerformClick()
                        End If
                    Case 2 'ctrl-space
                        Cycle()
                    Case 3 'ctrl-shift-space
                        Cycle(True)
                    Case 4
                        CloseAllToolStripMenuItem_Click(New ToolStripMenuItem, New EventArgs)
                End Select
            Case WM_SYSCOMMAND
                Select Case m.WParam
                    Case SC_RESTORE
                        Debug.Print("SC_RESTORE " & m.LParam.ToString)
                        SetWindowLong(ScalaHandle, GWL_HWNDPARENT, AltPP.MainWindowHandle)
                        'Me.ShowInTaskbar = False
                        If AltPP?.IsMinimized Then
                            AltPP.Restore()
                            If WindowState <> FormWindowState.Minimized Then Exit Sub
                        End If
                        moveBusy = False
                        If WindowState = FormWindowState.Maximized Then
                            Debug.Print("Restore clicking btnMax")
                            btnMax.PerformClick()
                            Exit Sub
                        End If
                        Debug.Print("wasMax " & wasMaximized)
                        If wasMaximized Then
                            Me.WndProc(Message.Create(ScalaHandle, WM_SYSCOMMAND, SC_MAXIMIZE, IntPtr.Zero))
                            Exit Sub
                        End If
                        suppressWM_MOVEcwp = True
                        MyBase.DefWndProc(m)
                        Me.Invalidate()
                        suppressWM_MOVEcwp = False
                        Exit Sub
                    Case SC_MAXIMIZE
                        Debug.Print("SC_MAXIMIZE " & m.LParam.ToString)
                        If AltPP?.IsMinimized Then
                            AltPP.Restore()
                        End If
                        If Me.WindowState = FormWindowState.Minimized Then
                            Me.WindowState = FormWindowState.Normal
                            'Me.Location = RestoreLoc
                        End If
                        btnMax.PerformClick()
                        Debug.Print("wasMax " & wasMaximized)
                        m.Result = 0
                    Case SC_MINIMIZE
                        Debug.Print("SC_MINIMIZE")
                        btnMin.PerformClick()
                        m.Result = 0
                        Exit Sub
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
                    If Not suppressWM_MOVEcwp AndAlso Not wasMaximized Then
                        Debug.Print($"WM_MOVE {Me.WindowState}")
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
                UpdateThumb(If(chkDebug.Checked, 128, 255))
                AltPP?.ResetCache()
                Me.Invalidate()
                moveBusy = False
            Case WM_SIZE ' = &h0005
                Dim width As Integer = LOWORD(m.LParam)
                Dim height As Integer = HIWORD(m.LParam)
                Debug.Print($"WM_SIZE {m.WParam} {width}x{height}")
                If m.WParam = 1 Then ' minimized
                    FrmBehind.Opacity = 0
                Else
#If DEBUG Then
                    FrmBehind.Opacity = If(chkDebug.Checked, 1, 0.01)
#Else
                    FrmBehind.Opacity = 0.01
#End If
                End If
                If m.WParam = 2 Then 'maximized
                    ReZoom(New Drawing.Size(width, height))
                    FrmBehind.Size = New Size(width, height)
                    btnMax.Text = "🗗"
                    ttMain.SetToolTip(btnMax, "Restore")
                End If
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
                                         Debug.Print("WM_SIZE Except")
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
                Dim winpos As WINDOWPOS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                Debug.Print($"WM_WINDOWPOSCHANGING {winpos.x} {winpos.y} {winpos.cx} {winpos.cy} {winpos.flags} {Me.WindowState} {m.WParam} {AltPP?.HasExited} {AltPP?.IsRunning}")

                'If pnlButtons IsNot Nothing Then
                '    Dim posInfo As IntPtr = BeginDeferWindowPos(2)
                '    DeferWindowPos(posInfo, ScalaHandle, Nothing, winpos.x, winpos.y, winpos.cx, winpos.cy, SetWindowPosFlags.DoNotSendChangingEvent)
                '    DeferWindowPos(posInfo, pnlButtons.Handle, Nothing, winpos.cx - pnlButtons.Width, 0, -1, -1, SetWindowPosFlags.IgnoreResize)
                '    EndDeferWindowPos(posInfo)
                'End If
                'If posChangeBusy Then
                '    Debug.Print("WM_WINDOWPOSCHANGING busy")
                '    m.Result = 0
                '    Exit Sub
                'End If
            Case WM_SHOWWINDOW
                Debug.Print($"WM_SHOWWINDOW {m.WParam} {m.LParam}")
                If m.WParam = 0 AndAlso m.LParam = 1 Then 'minimize
                    Debug.Print($"AltPP?{{{AltPP?.Id}}}.isSDL{{{AltPP?.isSDL}}}")
                    If Not AltPP?.isSDL Then
                        Debug.Print("Not AltPP?.isSDL")
                        SetWindowLong(ScalaHandle, GWL_HWNDPARENT, restoreParent)
                    End If
                    FrmBehind.Hide()
                    FrmSizeBorder.Hide()
                    wasMaximized = (Me.WindowState = FormWindowState.Maximized)
                    Me.DefWndProc(m)
                    Me.WindowState = FormWindowState.Minimized
                    Exit Sub
                End If
                If m.WParam = 1 AndAlso m.LParam = 3 Then 'restore
                    Debug.Print($"wasMaximized {wasMaximized}")
                    FrmBehind.Show()
                    If Not FrmSizeBorder.Visible Then FrmSizeBorder.Show(Me)
                    Dim dummy = Task.Run(Sub()
                                             Threading.Thread.Sleep(100)
                                             If wasMaximized Then Me.Invoke(Sub() btnMax.PerformClick())
                                         End Sub)

                End If
            Case WM_WINDOWPOSCHANGED 'handle dragging of maximized window
                'If posChangeBusy Then
                '    Debug.Print("WM_WINDOWPOSCHANGED busy")
                '    m.Result = 0
                '    Exit Sub
                'End If
                Dim winpos As WINDOWPOS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                Debug.Print($"WM_WINDOWPOSCHANGED {winpos.x} {winpos.y} {winpos.cx} {winpos.cy} {winpos.flags} {Me.WindowState} {m.WParam} {AltPP?.HasExited} {AltPP?.IsRunning}")
                If Me.WindowState = FormWindowState.Minimized AndAlso AltPP?.HasExited AndAlso cboAlt.SelectedIndex <> 0 Then
                    Debug.Print("Sending restore")
                    Me.WndProc(Message.Create(ScalaHandle, WM_SYSCOMMAND, SC_RESTORE, Nothing))
                    Me.Show()
                    FrmBehind.Show()
                    If Not FrmSizeBorder.Visible Then FrmSizeBorder.Show(Me)
                End If
                If caption_Mousedown Then
                    FrmBehind.Bounds = New Rectangle(winpos.x, winpos.y, winpos.cx, winpos.cy)
                    'Debug.Print($"szb{FrmSizeBorder.Bounds} fbh{FrmBehind.Bounds}")
                End If
                If FrmSizeBorder IsNot Nothing AndAlso Me.WindowState = FormWindowState.Normal Then
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

                    AltPP?.CenterBehind(pbZoom, SetWindowPosFlags.ASyncWindowPosition)
                    pnlTitleBar.Width = winpos.cx - pnlButtons.Width - pnlSys.Width
                    Debug.Print($"winpos location {New Point(winpos.x, winpos.y)}")
                    Debug.Print($"winpos size {New Size(winpos.cx, winpos.cy)}")
                    'System.Runtime.InteropServices.Marshal.StructureToPtr(winpos, m.LParam, True)
                    FrmSizeBorder.Opacity = If(chkDebug.Checked, 1, 0.01)
                    FrmSizeBorder.Opacity = If(My.Settings.SizingBorder, FrmSizeBorder.Opacity, 0)
                    posChangeBusy = False
                    Exit Sub
                End If
            Case WM_WININICHANGE '&H1A
                If SuppressWININICHANGECounter > 0 Then
                    Debug.Print($"ReschangeCounter {SuppressWININICHANGECounter}")
                    SuppressWININICHANGECounter -= 1
                Else
                    If m.LParam = IntPtr.Zero AndAlso Me.WindowState = FormWindowState.Maximized Then
                        Debug.Print($"WM_WININICHANGE {m.LParam}")
                        'handle taskbar changing
                        Dim newWA = Screen.FromPoint(Me.Location + New Point(Me.Width / 2, Me.Height / 2)).WorkingArea
                        'only do adjustment when size change or moved from top/bottom to sides
                        If newWA.Height <> prevWA.Height OrElse newWA.Width <> prevWA.Width Then
                            Debug.Print($"Taskbar changed {prevWA}->{newWA}")
                            Me.WindowState = FormWindowState.Normal
                            btnMax.PerformClick() 'todo replace with gracefull resizing
                        End If
                    End If
                End If
            Case WM_DISPLAYCHANGE
                Debug.Print($"WM_DISPLAYCHANGE w {m.WParam} w {m.LParam} {LOWORD(m.LParam)} {HIWORD(m.LParam)}")
                AltPP?.ResetCache()
                If Me.WindowState = FormWindowState.Maximized Then SuppressWININICHANGECounter = 2
                Task.Run(Sub()
                             Threading.Thread.Sleep(5000)
                             SuppressWININICHANGECounter = 0
                         End Sub)
            Case WM_ENTERMENULOOP
                Debug.Print($"WM_ENTERMENULOOP {cmsQuickLaunch.Visible}")
                SysMenu.Visible = Not cmsQuickLaunch.Visible
            Case WM_EXITMENULOOP
                Debug.Print("WM_EXITMENULOOP")
                SysMenu.Visible = False
            Case WM_INITMENU
                Debug.Print($"WM_INITMENU {m.WParam} {SysMenu.Handle}")
                If FrmSettings.chkDoAlign.Checked Then
                    SysMenu.Disable(SC_SIZE)
                    SysMenu.Disable(SC_MOVE)
                    SysMenu.Disable(SC_RESTORE)
                    SysMenu.Disable(SC_MAXIMIZE)
                    SysMenu.Disable(SC_MINIMIZE)
                Else
                    If Me.WindowState = FormWindowState.Minimized Then
                        SysMenu.Disable(SC_MINIMIZE)
                    Else
                        SysMenu.Enable(SC_MINIMIZE)
                    End If
                    If Me.WindowState = FormWindowState.Normal Then
                        SysMenu.Disable(SC_RESTORE)
                        SysMenu.Enable(SC_SIZE)
                        SysMenu.Enable(SC_MOVE)
                        SysMenu.Enable(SC_MAXIMIZE)
                    Else
                        SysMenu.Enable(SC_RESTORE)
                        SysMenu.Disable(SC_SIZE)
                        SysMenu.Disable(SC_MOVE)
                    End If
                End If
            Case WM_DWMCOLORIZATIONCOLORCHANGED
                If Not ThemeChanging Then
                    ThemeChanging = True
                    If My.Settings.Theme = 1 Then
                        Dim darkmode As Boolean = WinUsingDarkTheme()
                        Debug.Print($"Theme Changing dark={darkmode}")

                        If darkmode <> My.Settings.DarkMode Then
                            ApplyTheme(darkmode)
                            Me.Invalidate()
                        End If
                    End If
                    Task.Run(Sub()
                                 Threading.Thread.Sleep(7500)
                                 ThemeChanging = False
                             End Sub)
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
            Case &H83 ' WM_NCCALCSIZE
                'If CType(m.WParam, Boolean) = True AndAlso pnlButtons IsNot Nothing Then
                '    Debug.Print($"WM_NCCALCSIZE")
                '    Dim NCCS As NCCALCSIZE_PARAMS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(NCCALCSIZE_PARAMS))

                '    Debug.Print($"{NCCS.rgrc(0)} {NCCS.rgrc(1)} {NCCS.rgrc(2)}")


                '    Dim origin As RECT = New RECT(NCCS.rgrc(1).Right - pnlButtons.Width, NCCS.rgrc(1).Top, NCCS.rgrc(1).Right, NCCS.rgrc(1).Top + pnlButtons.Height)
                '    Dim dest As RECT = New RECT(NCCS.rgrc(0).Right - pnlButtons.Width, NCCS.rgrc(0).Top, NCCS.rgrc(0).Right, NCCS.rgrc(0).Top + pnlButtons.Height)

                '    NCCS.rgrc(1) = origin
                '    NCCS.rgrc(2) = dest

                '    Debug.Print($"{NCCS.rgrc(0)} {NCCS.rgrc(1)} {NCCS.rgrc(2)}")

                '    m.Result = &H400 'WVR_VALIDRECTS

                '    System.Runtime.InteropServices.Marshal.StructureToPtr(NCCS, m.LParam, True)

                'End If
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

            Case &HC0D9 To &HC200 ' unknown

            Case Else
                Debug.Print($"Unhandeld WM_ 0x{m.Msg:X8} &H{m.Msg:X8}")
#End If
        End Select

        MyBase.WndProc(m)  ' allow form to process this message
    End Sub

    Dim prevLoc As Point
    Dim sizeMoveBusy As Boolean = False

End Class