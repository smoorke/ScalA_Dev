Public NotInheritable Class WindowProc
    'dummy class to prevent form generation
End Class

Partial NotInheritable Class FrmMain
    Dim SuppressWININICHANGECounter As Integer = 0
    Dim ThemeChanging As Boolean = False

    Dim suppressRestoreBounds As Boolean = False

    Dim StructureToPtrSupported As Boolean = False
    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case Hotkey.WM_HOTKEY
                dBug.print($"Hotkey {m.WParam} pressed")
                Select Case m.WParam
                    Case 1 'ctrl-tab
                        'only perform switch when astonia or scala Is active
                        Dim activeID = GetActiveProcessID()
                        dBug.print("aID " & activeID & " selfPID " & scalaPID)
                        If activeID = scalaPID OrElse Process.GetProcessById(activeID).IsAstonia Then
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
                    Case 5 'ctrl-win-T
                        'only when active window isn't scala nor Astonia
                        Dim activeID As Integer = GetActiveProcessID()
                        Dim activeHandle = GetForegroundWindow()
                        Dim activeProc = Process.GetProcessById(activeID)
                        dBug.print("--Before--")
                        dBug.print($"Scala {ScalaHandle} Top:{(GetWindowLong(ScalaHandle, GWL_EXSTYLE) And WindowStylesEx.WS_EX_TOPMOST) = WindowStylesEx.WS_EX_TOPMOST}")
                        dBug.print($"Trget {activeHandle} Top:{(GetWindowLong(activeHandle, GWL_EXSTYLE) And WindowStylesEx.WS_EX_TOPMOST) = WindowStylesEx.WS_EX_TOPMOST}")
                        dBug.print($"tgtOwner {GetWindowLong(activeHandle, GWL_HWNDPARENT)}")
                        If activeID = scalaPID OrElse activeProc.IsScalA() Then
                            dBug.print("Cannot set self topmost with hotkey")
                            Exit Sub
                        End If
                        If Not activeProc.IsAstonia Then
                            Try
                                Dim wastopm As Boolean = (GetWindowLong(activeHandle, GWL_EXSTYLE) And WindowStylesEx.WS_EX_TOPMOST) = WindowStylesEx.WS_EX_TOPMOST

                                SetWindowPos(activeHandle, If(wastopm, SWP_HWND.NOTOPMOST, SWP_HWND.TOPMOST),
                                                  -1, -1, -1, -1,
                                                  SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate)

                                dBug.print("--Mid 1--")
                                dBug.print($"Scala {ScalaHandle} Top:{(GetWindowLong(ScalaHandle, GWL_EXSTYLE) And WindowStylesEx.WS_EX_TOPMOST) = WindowStylesEx.WS_EX_TOPMOST}")
                                dBug.print($"Trget {activeProc.MainWindowTitle.Cap(10)} {activeHandle} Top:{(GetWindowLong(activeHandle, GWL_EXSTYLE) And WindowStylesEx.WS_EX_TOPMOST) = WindowStylesEx.WS_EX_TOPMOST}")
                                dBug.print($"tgtOwner {GetWindowLong(activeHandle, GWL_HWNDPARENT)}")

                                If My.Settings.topmost Then
                                    Dim ownerHndl As IntPtr = GetWindowLong(activeHandle, GWL_HWNDPARENT)
                                    If ownerHndl = ScalaHandle AndAlso wastopm Then
                                        SetWindowLong(activeHandle, GWL_HWNDPARENT, 0)
                                    Else
                                        If System.Diagnostics.Debugger.IsAttached() Then
                                            'Debugger.Break() 'WARNING: do not set GWL_HWNDPARENT to debugger. it will hang
                                        End If
                                        SetWindowLong(activeHandle, GWL_HWNDPARENT, ScalaHandle)
                                        Try
                                            'todo: restore taskbar visibility?
                                            'AppActivate(activeID) 'doesn't work
                                        Catch ex As Exception
                                            dBug.print(ex.Message)
                                        End Try
                                    End If
                                End If

                            Catch
                            Finally
                                Me.TopMost = My.Settings.topmost
                            End Try
                        End If
                        dBug.print("---After---")
                        dBug.print($"Scala {ScalaHandle} Top:{(GetWindowLong(ScalaHandle, GWL_EXSTYLE) And WindowStylesEx.WS_EX_TOPMOST) = WindowStylesEx.WS_EX_TOPMOST}")
                        dBug.print($"Trget {activeHandle} Top:{(GetWindowLong(activeHandle, GWL_EXSTYLE) And WindowStylesEx.WS_EX_TOPMOST) = WindowStylesEx.WS_EX_TOPMOST}")
                        dBug.print($"tgtOwner {GetWindowLong(activeHandle, GWL_HWNDPARENT)}")
                    Case 6 'AlterOverviewPlus
                        My.Settings.ExtraMaxColRow += 1
                    Case 7 'AlteroverviewMin
                        My.Settings.ExtraMaxColRow = Math.Max(0, My.Settings.ExtraMaxColRow - 1)
                    Case 8 'AlteroverviwStar
                        My.Settings.OneLessRowCol = Not My.Settings.OneLessRowCol

                End Select
            Case WM_SYSCOMMAND
                Select Case m.WParam
                    Case SC_RESTORE
                        dBug.print("SC_RESTORE " & m.LParam.ToString)
                        Attach(AltPP)

                        SendMessage(FrmSizeBorder.Handle, WM_SYSCOMMAND, SC_RESTORE, IntPtr.Zero)
                        SendMessage(FrmBehind.Handle, WM_SYSCOMMAND, SC_RESTORE, IntPtr.Zero)

                        If My.Settings.MinMin AndAlso My.Settings.gameOnOverview AndAlso pnlOverview.Visible Then
                            Parallel.ForEach(pnlOverview.Controls.OfType(Of AButton).TakeWhile(Function(b) b.Visible).Where(Function(b) b.AP IsNot Nothing),
                                             Sub(ab As AButton)
                                                 If ab.AP.isSDL Then ab.AP.Restore()
                                             End Sub)
                            Me.BringToFront()
                        End If

                        If AltPP?.IsMinimized Then
                            AltPP.Restore()
                            If WindowState <> FormWindowState.Minimized Then Exit Sub
                        End If
                        moveBusy = False
                        If WindowState = FormWindowState.Maximized Then
                            dBug.print("Restore clicking btnMax")
                            btnMax.PerformClick()
                            Exit Sub
                        End If
                        dBug.print("wasMax " & wasMaximized)
                        If wasMaximized Then
                            'suppressRestoreBounds = True
                            SetWindowPos(ScalaHandle, SWP_HWND.TOP, MaximizedBounds.X, MaximizedBounds.Y, MaximizedBounds.Width, MaximizedBounds.Height,
                                        SetWindowPosFlags.ShowWindow)
                            Me.WndProc(Message.Create(ScalaHandle, WM_SYSCOMMAND, SC_MAXIMIZE, IntPtr.Zero))
                            suppressRestoreBounds = False
                            Exit Sub
                        End If
                        suppressWM_MOVEcwp = True
                        MyBase.DefWndProc(m)
                        Me.Invalidate()
                        suppressWM_MOVEcwp = False
                        Exit Sub
                    Case SC_MAXIMIZE
                        dBug.print("SC_MAXIMIZE " & m.LParam.ToString)
                        If AltPP?.IsMinimized Then
                            AltPP.Restore()
                        End If
                        If Me.WindowState = FormWindowState.Minimized Then
                            Me.WindowState = FormWindowState.Normal
                            'Me.Location = RestoreLoc
                        End If
                        btnMax.PerformClick()
                        dBug.print("wasMax " & wasMaximized)
                        m.Result = 0
                    Case SC_MINIMIZE
                        dBug.print("SC_MINIMIZE")
                        btnMin.PerformClick()
                        m.Result = 0
                        Exit Sub
                    Case SC_SIZE
                        SendMessage(FrmSizeBorder.Handle, WM_SYSCOMMAND, SC_SIZE, IntPtr.Zero)
                        m.Result = 0
                        Exit Sub
                    Case MC_SETTINGS
                        dBug.print("Settings called by sysMenu")
                        FrmSettings.Show()
                        FrmSettings.WindowState = FormWindowState.Normal
                        Exit Sub
                End Select
            Case WM_MOVE
                'dBug.print($"WM_MOVE {Me.WindowState}")
                'FrmBehind.Location = New LParamMap(m.LParam)
                Me.Cursor = Cursors.Default
                'frmCaptureClickBehind.Bounds = Me.RectangleToScreen(pbZoom.Bounds)
                If AltPP?.IsRunning AndAlso Me.WindowState <> FormWindowState.Minimized Then
#If DEBUG Then
                    pbZoom.Visible = True
#End If
                    If Not suppressWM_MOVEcwp AndAlso Not wasMaximized AndAlso cboAlt.SelectedIndex > 0 Then
                        'dBug.print($"WM_MOVE {Me.WindowState}")
                        moveBusy = True
                        Task.Run(Sub()
                                     'Exit Sub
                                     AltPP?.CenterWindowPos(ScalaHandle,
                                                        Me.Left + pbZoom.Left + (pbZoom.Width / 2),
                                                        Me.Top + pbZoom.Top + (pbZoom.Height / 2),
                                                        SetWindowPosFlags.ASyncWindowPosition Or SetWindowPosFlags.DoNotActivate,
                                                        True)
                                 End Sub)
                    End If
                End If
            Case WM_EXITSIZEMOVE
                dBug.print($"WM_EXITSIZEMOVE")
                If Not (My.Settings.gameOnOverview AndAlso cboAlt.SelectedIndex = 0) Then UpdateThumb(If(chkDebug.Checked, 128, 255))
                If My.Settings.SizingBorder Then
                    If Me.WindowState = FormWindowState.Maximized Then
                        FrmSizeBorder.Hide()
                    Else
                        If Not FrmSizeBorder.Visible Then FrmSizeBorder.Show(Me)
                    End If
                End If
                AltPP?.ResetCache()
                Me.Invalidate()
                moveBusy = False
            Case WM_SIZE ' = &h0005
                Dim sz As Size = New LParamMap(m.LParam)
                dBug.print($"WM_SIZE {m.WParam} {sz}")
                If suppressRestoreBounds AndAlso sz.Width <= RestoreBounds.Size.Width AndAlso sz.Height <= RestoreBounds.Size.Height Then
                    m.Result = 0
                    dBug.print("WM_SIZE blocked")
                    Exit Sub
                End If
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
                    ReZoom(sz)
                    btnMax.Text = "🗗"
                    ttMain.SetToolTip(btnMax, "Restore")
                End If
                If Me.Location = prevLoc AndAlso cboAlt.SelectedIndex > 0 Then
                    If Not sizeMoveBusy Then
                        sizeMoveBusy = True
                        moveBusy = True
                        Task.Run(Sub()
                                     Try
                                         AltPP?.CenterWindowPos(ScalaHandle,
                                                        Me.Left + pbZoom.Left + (pbZoom.Width / 2),
                                                        Me.Top + pbZoom.Top + (pbZoom.Height / 2),
                                                        SetWindowPosFlags.DoNotChangeOwnerZOrder Or SetWindowPosFlags.ASyncWindowPosition Or SetWindowPosFlags.DoNotActivate)
                                     Catch
                                         dBug.print("WM_SIZE Except")
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
                'dBug.print($"vers: {System.Environment.Version}")
                'New Version(System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription.Split(" "c)(2))}
                'dBug.print(New Version(System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription).ToString)
                If StructureToPtrSupported Then 'Marshal.StructureToPtr requires 4.5.1 or higher
                    Dim winpos As WINDOWPOS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                    If caption_Mousedown AndAlso captionMoveTrigger AndAlso New Point(winpos.x, winpos.y) = Me.RestoreBounds.Location Then
                        winpos.flags = winpos.flags Or SetWindowPosFlags.IgnoreMove
                        System.Runtime.InteropServices.Marshal.StructureToPtr(winpos, m.LParam, True)
                        dBug.print($"Moveglitch fixed")
                        captionMoveTrigger = False
                    End If
                    If suppressRestoreBounds AndAlso New Rectangle(winpos.x, winpos.y, winpos.cx, winpos.cy) = Me.RestoreBounds Then
                        winpos.flags = winpos.flags Or SetWindowPosFlags.IgnoreMove
                        System.Runtime.InteropServices.Marshal.StructureToPtr(winpos, m.LParam, True)
                        dBug.print($"Restoreglitch tweaked")
                        'suppressRestoreBounds = False
                    End If
                End If
            Case WM_SHOWWINDOW
                dBug.print($"WM_SHOWWINDOW {m.WParam} {m.LParam}")
                If m.WParam = SW_HIDE AndAlso m.LParam = SW_PARENTCLOSING Then 'minimize
                    dBug.print($"AltPP?{{{AltPP?.Id}}}.isSDL{{{AltPP?.isSDL}}}")
                    If Not AltPP?.isSDL Then
                        dBug.print("Not AltPP?.isSDL")
                        Detach(True)
                    End If
                    FrmBehind.Hide()
                    FrmSizeBorder.Hide()
                    wasMaximized = (Me.WindowState = FormWindowState.Maximized)
                    Me.DefWndProc(m)
                    Me.WindowState = FormWindowState.Minimized
                    Exit Sub
                End If
                If m.WParam = SW_NORMAL AndAlso m.LParam = SW_PARENTOPENING Then 'restore
                    dBug.print($"wasMaximized {wasMaximized}")
                    If Not FrmSizeBorder.Visible Then FrmSizeBorder.Show(Me)
                    If wasMaximized Then
                        SetWindowPos(ScalaHandle, SWP_HWND.TOP, Bounds.X, Bounds.Y, MaximizedBounds.Width, MaximizedBounds.Height, SetWindowPosFlags.ShowWindow)
                        'Me.WindowState = FormWindowState.Maximized
                    End If
                    AltPP?.CenterBehind(pbZoom, 0, True, True) 'fix thumb breaking
                    FrmBehind.Show()
                End If
            Case WM_WINDOWPOSCHANGED 'handle dragging of maximized window
                'If posChangeBusy Then
                '    dBug.print("WM_WINDOWPOSCHANGED busy")
                '    m.Result = 0
                '    Exit Sub
                'End If
                Dim winpos As WINDOWPOS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                'dBug.print($"WM_WINDOWPOSCHANGED {winpos.x} {winpos.y} {winpos.cx} {winpos.cy} {winpos.flags} {Me.WindowState} {m.WParam} {AltPP?.HasExited} {AltPP?.IsRunning}")
                If Me.WindowState = FormWindowState.Minimized AndAlso AltPP?.HasExited AndAlso cboAlt.SelectedIndex <> 0 Then
                    dBug.print("Sending restore")
                    Me.WndProc(Message.Create(ScalaHandle, WM_SYSCOMMAND, SC_RESTORE, Nothing))
                    Me.Show()
                    FrmBehind.Show()
                    If Not FrmSizeBorder.Visible Then FrmSizeBorder.Show(Me)
                End If

                If pbZoom IsNot Nothing Then
                    frmOverlay.Bounds = New Rectangle(winpos.x, winpos.y + 21, winpos.cx, winpos.cy - 21)
                End If

                FrmBehind.Bounds = New Rectangle(winpos.x, winpos.y + 21, winpos.cx, winpos.cy - 21)

                If FrmSizeBorder IsNot Nothing AndAlso Me.WindowState = FormWindowState.Normal Then
                    FrmSizeBorder.Bounds = New Rectangle(winpos.x, winpos.y, winpos.cx, winpos.cy)
                End If
                If wasMaximized AndAlso caption_Mousedown Then
                    'Dim winpos As WINDOWPOS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                    'winpos.flags = SetWindowPosFlags.IgnoreMove
                    dBug.print("WM_WINDOWPOSCHANGED from maximized And mousebutton down")
                    dBug.print($"hwndInsertAfter {winpos.hwndInsertAfter}")
                    dBug.print($"flags {winpos.flags}")
                    dBug.print($"pos {winpos.x} {winpos.y} size {winpos.cx} {winpos.cy}")
                    btnMax.Text = "⧠"
                    ttMain.SetToolTip(btnMax, "Maximize")
                    cmbResolution.Enabled = True
                    wasMaximized = False

                    AOshowEqLock = False

                    Me.WindowState = FormWindowState.Normal
                    ReZoom(New Drawing.Size(winpos.cx - 2, winpos.cy - pnlTitleBar.Height - 1))
                    cmbResolution.SelectedIndex = My.Settings.zoom

                    If cboAlt.SelectedIndex > 0 Then AltPP?.CenterBehind(pbZoom, SetWindowPosFlags.ASyncWindowPosition Or SetWindowPosFlags.DoNotActivate)
                    pnlTitleBar.Width = winpos.cx - pnlButtons.Width - pnlSys.Width
                    dBug.print($"winpos location {New Point(winpos.x, winpos.y)}")
                dBug.print($"winpos size {New Size(winpos.cx, winpos.cy)}")
                    'System.Runtime.InteropServices.Marshal.StructureToPtr(winpos, m.LParam, True)
                    FrmSizeBorder.Opacity = If(chkDebug.Checked, 1, 0.01)
                    FrmSizeBorder.Opacity = If(My.Settings.SizingBorder, FrmSizeBorder.Opacity, 0)
                    'posChangeBusy = False
                    Exit Sub
                End If
            Case WM_WININICHANGE '&H1A
                dBug.print($"WM_WININICHANGE {m.LParam} {m.WParam}")
                Dim settingnname = Runtime.InteropServices.Marshal.PtrToStringAuto(m.LParam)
                If settingnname = "VisualEffects" Then
                    AnimsEnabled = getAnimationsEnabled()
                    dBug.print($"Animations {AnimsEnabled}")
                End If
                If SuppressWININICHANGECounter > 0 Then
                    dBug.print($"ReschangeCounter {SuppressWININICHANGECounter}")
                    SuppressWININICHANGECounter -= 1
                Else
                    If m.LParam = IntPtr.Zero AndAlso Me.WindowState = FormWindowState.Maximized Then
                        'handle taskbar changing
                        Dim newWA = Screen.FromPoint(Me.Location + New Point(Me.Width / 2, Me.Height / 2)).WorkingArea
                        'only do adjustment when size change or moved from top/bottom to sides
                        If newWA.Height <> prevWA.Height OrElse newWA.Width <> prevWA.Width Then
                            dBug.print($"Taskbar changed {prevWA}->{newWA}")
                            Me.WindowState = FormWindowState.Normal
                            btnMax.PerformClick() 'todo replace with gracefull resizing
                        End If
                    End If
                End If
            Case WM_DISPLAYCHANGE
                dBug.print($"WM_DISPLAYCHANGE w {m.WParam} w {m.LParam}")

            Case WM_ENTERMENULOOP
                dBug.print($"WM_ENTERMENULOOP {cmsQuickLaunch.Visible}")
                SysMenu.Visible = Not cmsQuickLaunch.Visible
            Case WM_EXITMENULOOP
                dBug.print("WM_EXITMENULOOP")
                SysMenu.Visible = False
            Case WM_INITMENU
                dBug.print($"WM_INITMENU {m.WParam} {SysMenu.Handle}")
                'If FrmSettings.chkDoAlign.Checked Then
                '    SysMenu.Disable(SC_SIZE)
                '    SysMenu.Disable(SC_MOVE)
                '    SysMenu.Disable(SC_RESTORE)
                '    SysMenu.Disable(SC_MAXIMIZE)
                '    SysMenu.Disable(SC_MINIMIZE)
                'Else
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
                'End If
            Case WM_DWMCOLORIZATIONCOLORCHANGED
                If Not ThemeChanging Then
                    ThemeChanging = True
                    If My.Settings.Theme = 1 Then
                        Dim darkmode As Boolean = WinUsingDarkTheme()
                        dBug.print($"Theme Changing dark={darkmode}")

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
            Case WM_KEYDOWN
                dBug.print($"WM_KEYDOWN w:{m.WParam} l:{m.LParam}")
                dBug.print($"ScanCode {New LParamMap(m.LParam).scan}")
                If cboAlt.SelectedIndex > 0 Then
                    AltPP.Activate()
                    Dim key As Keys = m.WParam
                    If Not AltPP.isSDL Then
                        SendMessage(AltPP.MainWindowHandle, WM_KEYDOWN, key, IntPtr.Zero)
                    Else
                        Dim scan As Byte = New LParamMap(m.LParam)
                        If key = Keys.Escape OrElse (key >= Keys.F1 AndAlso key <= Keys.F12) OrElse
                           key = Keys.Back Then
                            SendScanKey(scan)
                        ElseIf key = Keys.Delete OrElse key = Keys.PageUp OrElse key = Keys.PageDown OrElse
                               key = Keys.End OrElse key = Keys.Home Then
                            SendScanKeyEx(scan)
                        Else
                            SendKey(key)
                        End If
                    End If
                End If
            Case WM_KEYUP
                dBug.print($"WM_KEYUP w:{m.WParam} l:{m.LParam}")
                dBug.print($"ScanCode {New LParamMap(m.LParam).scan}")
                If cboAlt.SelectedIndex > 0 Then
                    AltPP.Activate()
                    Dim scan As Byte = New LParamMap(m.LParam)
                    If scan = 28 Then
                        If Not AltPP.isSDL Then
                            SendKeys.Send("{ENTER}")
                        Else
                            SendScanKey(scan)
                        End If
                    End If
                End If
            Case WM_CHAR
                dBug.print($"WM_CHAR w:{m.WParam} l:{m.LParam}")
                dBug.print($"ScanCode {New LParamMap(m.LParam).scan}")
                If cboAlt.SelectedIndex > 0 Then
                    AltPP.Activate()
                    If Not AltPP.isSDL Then
                        SendMessage(AltPP.MainWindowHandle, WM_CHAR, m.WParam, IntPtr.Zero)
                    End If
                End If
            Case WM_SYSKEYDOWN
                dBug.print($"WM_SYSKEY {m.WParam} {m.LParam}")
                If cboAlt.SelectedIndex > 0 Then
                    AltPP.Activate()
                End If
            Case WM_NCACTIVATE
                dBug.print("WM_NCACTIVATE")
                setActive(True)
                Me.Refresh()
#If DEBUG Then

            Case &H6 ' WM_ACTIVATE
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
                '    dBug.print($"WM_NCCALCSIZE")
                '    Dim NCCS As NCCALCSIZE_PARAMS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(NCCALCSIZE_PARAMS))

                '    dBug.print($"{NCCS.rgrc(0)} {NCCS.rgrc(1)} {NCCS.rgrc(2)}")


                '    Dim origin As RECT = New RECT(NCCS.rgrc(1).Right - pnlButtons.Width, NCCS.rgrc(1).Top, NCCS.rgrc(1).Right, NCCS.rgrc(1).Top + pnlButtons.Height)
                '    Dim dest As RECT = New RECT(NCCS.rgrc(0).Right - pnlButtons.Width, NCCS.rgrc(0).Top, NCCS.rgrc(0).Right, NCCS.rgrc(0).Top + pnlButtons.Height)

                '    NCCS.rgrc(1) = origin
                '    NCCS.rgrc(2) = dest

                '    dBug.print($"{NCCS.rgrc(0)} {NCCS.rgrc(1)} {NCCS.rgrc(2)}")

                '    m.Result = &H400 'WVR_VALIDRECTS

                '    System.Runtime.InteropServices.Marshal.StructureToPtr(NCCS, m.LParam, True)

                'End If
            Case &H84 ' WM_NCHITTEST

            Case &HA1 ' WM_NCLBUTTONDOWN

           ' Case &H104 ' WM_SYSKEYDOWN
            'Case &H105 ' WM_SYSKEYUP

            Case &H121 ' WM_ENTERIDLE

            Case &H200 ' WM_MOUSEMOVE
                'dBug.print($"WM_MOUSEMOVE {Nothing} {Nothing}")

            Case &H210 ' WM_PARENTNOTIFY 
            Case &H215 ' WM_CAPTURECHANGED
            Case &H216 ' WM_MOVEING

            Case &H281 ' WM_IME_SETCONTEXT
            Case &H282 ' WM_IME_NOTIFY 

            Case &H2A1 ' WM_MOUSEHOVER 
            Case &H2A3 ' WM_MOUSELEAVE

            Case &H319 ' WM_APPCOMMAND

            Case &HC059 To &HC2BC ' unknown

            Case Else
                Debug.Print($"Unhandeld WM_ 0x{m.Msg:X8} &H{m.Msg:X8} w{m.WParam} {m.LParam}")
#End If
        End Select
        MyBase.WndProc(m)  ' allow form to process this message
    End Sub

    Dim prevLoc As Point
    Dim sizeMoveBusy As Boolean = False
End Class