Imports System.Runtime.InteropServices

Public NotInheritable Class WindowProc
    'dummy class to prevent form generation
End Class

Partial NotInheritable Class FrmMain
    Dim SuppressWININICHANGECounter As Integer = 0
    Dim ThemeChanging As Boolean = False

    Dim suppressRestoreBounds As Boolean = False

    Dim StructureToPtrSupported As Boolean = False


    Dim moveSW As Stopwatch = Stopwatch.StartNew

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case Hotkey.WM_HOTKEY
                dBug.Print($"Hotkey {m.WParam} pressed")
                Select Case m.WParam
                    Case 1 'ctrl-tab
                        'only perform switch when astonia or scala Is active
                        Dim activeID = GetActiveProcessID()
                        dBug.Print("aID " & activeID & " selfPID " & scalaPID)
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
                        dBug.Print("--Before--")
                        dBug.Print($"Scala {ScalaHandle} Top:{(GetWindowLong(ScalaHandle, GWL_EXSTYLE) And WindowStylesEx.WS_EX_TOPMOST) = WindowStylesEx.WS_EX_TOPMOST}")
                        dBug.Print($"Trget {activeProc.MainWindowTitle.Cap(10)} {activeHandle} Top:{(GetWindowLong(activeHandle, GWL_EXSTYLE) And WindowStylesEx.WS_EX_TOPMOST) = WindowStylesEx.WS_EX_TOPMOST}")
                        dBug.Print($"tgtOwner {GetWindowLong(activeHandle, GWL_HWNDPARENT)}")
                        If activeID = scalaPID OrElse activeProc.IsScalA() Then
                            dBug.Print("Cannot set self topmost with hotkey")
                            Exit Sub
                        End If
                        If Not activeProc.IsAstonia Then
                            Try
                                Dim wastopm As Boolean = (GetWindowLong(activeHandle, GWL_EXSTYLE) And WindowStylesEx.WS_EX_TOPMOST) = WindowStylesEx.WS_EX_TOPMOST

                                SetWindowPos(activeHandle, If(wastopm, SWP_HWND.NOTOPMOST, SWP_HWND.TOPMOST),
                                                  -1, -1, -1, -1,
                                                  SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate)

                                dBug.Print("--Mid 1--")
                                dBug.Print($"Scala {ScalaHandle} Top:{(GetWindowLong(ScalaHandle, GWL_EXSTYLE) And WindowStylesEx.WS_EX_TOPMOST) = WindowStylesEx.WS_EX_TOPMOST}")
                                dBug.Print($"Trget {activeProc.MainWindowTitle.Cap(10)} {activeHandle} Top:{(GetWindowLong(activeHandle, GWL_EXSTYLE) And WindowStylesEx.WS_EX_TOPMOST) = WindowStylesEx.WS_EX_TOPMOST}")
                                dBug.Print($"tgtOwner {GetWindowLong(activeHandle, GWL_HWNDPARENT)}")

                                If My.Settings.topmost Then
                                    Dim ownerHndl As IntPtr = GetWindowLong(activeHandle, GWL_HWNDPARENT)
                                    If ownerHndl = ScalaHandle AndAlso wastopm Then
                                        SetWindowLong(activeHandle, GWL_HWNDPARENT, 0)
                                    Else
                                        'If System.Diagnostics.Debugger.IsAttached() Then
                                        '    'Debugger.Break() 'WARNING: do not set GWL_HWNDPARENT to debugger. it will hang
                                        'End If
                                        SetWindowLong(activeHandle, GWL_HWNDPARENT, ScalaHandle)
                                        Try
                                            'todo: restore taskbar visibility?
                                            'AppActivate(activeID) 'doesn't work
                                        Catch ex As Exception
                                            dBug.Print(ex.Message)
                                        End Try
                                    End If
                                End If

                            Catch
                            Finally
                                Me.TopMost = My.Settings.topmost
                            End Try
                        End If
                        dBug.Print("---After---")
                        dBug.Print($"Scala {ScalaHandle} Top:{(GetWindowLong(ScalaHandle, GWL_EXSTYLE) And WindowStylesEx.WS_EX_TOPMOST) = WindowStylesEx.WS_EX_TOPMOST}")
                        dBug.Print($"Trget {activeHandle} Top:{(GetWindowLong(activeHandle, GWL_EXSTYLE) And WindowStylesEx.WS_EX_TOPMOST) = WindowStylesEx.WS_EX_TOPMOST}")
                        dBug.Print($"tgtOwner {GetWindowLong(activeHandle, GWL_HWNDPARENT)}")
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
                        dBug.Print("SC_RESTORE " & m.LParam.ToString)
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
                            dBug.Print("Restore clicking btnMax")
                            btnMax.PerformClick()
                            Exit Sub
                        End If
                        dBug.Print("wasMax " & wasMaximized)
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
                        dBug.Print("SC_MAXIMIZE " & m.LParam.ToString)
                        If AltPP?.IsMinimized Then
                            AltPP.Restore()
                        End If
                        If Me.WindowState = FormWindowState.Minimized Then
                            Me.WindowState = FormWindowState.Normal
                            'Me.Location = RestoreLoc
                        End If
                        btnMax.PerformClick()
                        dBug.Print("wasMax " & wasMaximized)
                        m.Result = 0
                    Case SC_MINIMIZE
                        dBug.Print("SC_MINIMIZE")
                        btnMin.PerformClick()
                        m.Result = 0
                        Exit Sub
                    Case SC_SIZE
                        SendMessage(FrmSizeBorder.Handle, WM_SYSCOMMAND, SC_SIZE, IntPtr.Zero)
                        m.Result = 0
                        Exit Sub
                    Case MC_SETTINGS
                        dBug.Print("Settings called by sysMenu")
                        FrmSettings.Show()
                        FrmSettings.WindowState = FormWindowState.Normal
                        FrmSettings.Activate()
                        Exit Sub
                End Select
            Case WM_QUERYOPEN
                dBug.Print("WM_QUERYOPEN")
                Attach(AltPP)
            Case WM_ERASEBKGND

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
                        If moveSW.ElapsedMilliseconds > 10 Then
                            'dBug.print($"WM_MOVE {Me.WindowState}")
                            moveBusy = True
                            moveSW.Restart()
                            Task.Run(Sub()
                                         'Exit Sub
                                         AltPP?.CenterWindowPos(ScalaHandle,
                                                        Me.Left + pbZoom.Left + (pbZoom.Width / 2),
                                                        Me.Top + pbZoom.Top + (pbZoom.Height / 2),
                                                        SetWindowPosFlags.ASyncWindowPosition Or SetWindowPosFlags.DoNotActivate,
                                                        True)
                                     End Sub)
                        Else
                            Task.Run(Sub()
                                         Threading.Thread.Sleep(10)
                                         If moveSW.ElapsedMilliseconds > 10 Then
                                             AltPP?.CenterWindowPos(ScalaHandle,
                                                        Me.Left + pbZoom.Left + (pbZoom.Width / 2),
                                                        Me.Top + pbZoom.Top + (pbZoom.Height / 2),
                                                        SetWindowPosFlags.ASyncWindowPosition Or SetWindowPosFlags.DoNotActivate,
                                                        True)
                                         End If
                                     End Sub)
                        End If
                    End If
                End If
            Case WM_ENTERSIZEMOVE
                dBug.Print($"WM_ENTERSIZEMOVE")
            Case WM_EXITSIZEMOVE
                dBug.Print($"WM_EXITSIZEMOVE")
                If Not (My.Settings.gameOnOverview AndAlso cboAlt.SelectedIndex = 0) Then UpdateThumb(If(chkDebug.Checked, 128, 255))
                If My.Settings.SizingBorder Then
                    If Me.WindowState = FormWindowState.Maximized Then
                        FrmSizeBorder.Hide()
                    Else
                        If Not FrmSizeBorder.Visible AndAlso My.Settings.SizingBorder Then FrmSizeBorder.Show(If(FrmSizeBorder.Owner Is Nothing, Me, Nothing))
                    End If
                End If
                AltPP?.ResetCache()
                Me.Invalidate()
                moveBusy = False
            Case WM_SIZE ' = &h0005
                Dim sz As Size = New LParamMap(m.LParam)
                dBug.Print($"WM_SIZE {m.WParam} {sz}")
                If suppressRestoreBounds AndAlso sz.Width <= RestoreBounds.Size.Width AndAlso sz.Height <= RestoreBounds.Size.Height Then
                    m.Result = 0
                    dBug.Print("WM_SIZE blocked")
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
                                         dBug.Print("WM_SIZE Except")
                                     Finally
                                         sizeMoveBusy = False
                                         moveBusy = False
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
                Dim winpos As WINDOWPOS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                If StructureToPtrSupported Then 'Marshal.StructureToPtr requires 4.5.1 or higher
                    If caption_Mousedown AndAlso captionMoveTrigger AndAlso New Point(winpos.x, winpos.y) = Me.RestoreBounds.Location Then
                        winpos.flags = winpos.flags Or SetWindowPosFlags.IgnoreMove
                        System.Runtime.InteropServices.Marshal.StructureToPtr(winpos, m.LParam, True)
                        dBug.Print($"Moveglitch fixed")
                        captionMoveTrigger = False
                    End If
                    If suppressRestoreBounds AndAlso New Rectangle(winpos.x, winpos.y, winpos.cx, winpos.cy) = Me.RestoreBounds Then
                        winpos.flags = winpos.flags Or SetWindowPosFlags.IgnoreMove
                        System.Runtime.InteropServices.Marshal.StructureToPtr(winpos, m.LParam, True)
                        dBug.Print($"Restoreglitch tweaked")
                        'suppressRestoreBounds = False
                    End If
                End If
               'Debug.Print("WinposChanging:")
                'Debug.Print($"{winpos.flags}")
               ' If Not Me.Disposing Then Debug.Print($"{winpos.hwndInsertAfter} {ScalaHandle} {FrmSizeBorder?.Handle} {frmOverlay?.Handle}")
            Case WM_SHOWWINDOW
                dBug.Print($"WM_SHOWWINDOW {m.WParam} {m.LParam}")
                If m.WParam = SW_HIDE AndAlso m.LParam = SW_PARENTCLOSING Then 'minimize
                    dBug.Print($"AltPP?{{{AltPP?.Id}}}.isSDL{{{AltPP?.isSDL}}}")
                    If Not AltPP?.isSDL Then
                        dBug.Print("Not AltPP?.isSDL")
                        Detach(True)
                    End If
                    FrmBehind.Hide()
                    FrmSizeBorder.Hide()
                    frmOverlay.Hide()
                    wasMaximized = (Me.WindowState = FormWindowState.Maximized)
                    Me.DefWndProc(m)
                    Me.WindowState = FormWindowState.Minimized
                    Exit Sub
                End If
                If m.WParam = SW_NORMAL AndAlso m.LParam = SW_PARENTOPENING Then 'restore
                    dBug.Print($"wasMaximized {wasMaximized}")
                    If Not FrmSizeBorder.Visible AndAlso My.Settings.SizingBorder Then FrmSizeBorder.Show(If(FrmSizeBorder.Owner Is Nothing, Me, Nothing))
                    If wasMaximized Then
                        SetWindowPos(ScalaHandle, SWP_HWND.TOP, Bounds.X, Bounds.Y, MaximizedBounds.Width, MaximizedBounds.Height, SetWindowPosFlags.ShowWindow)
                        'Me.WindowState = FormWindowState.Maximized
                    End If
                    AltPP?.CenterBehind(pbZoom, 0, True, True) 'fix thumb breaking
                    FrmBehind.Show()
                    If Not frmOverlay.Visible Then frmOverlay.Show(If(frmOverlay.Owner Is Nothing, Me, Nothing))
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
                    dBug.Print("Sending restore")
                    Me.WndProc(Message.Create(ScalaHandle, WM_SYSCOMMAND, SC_RESTORE, Nothing))
                    Me.Show()
                    FrmBehind.Show()
                    If Not FrmSizeBorder.Visible AndAlso My.Settings.SizingBorder Then FrmSizeBorder.Show()
                End If

                If pbZoom IsNot Nothing Then
                    'frmOverlay.Bounds = New Rectangle(winpos.x, winpos.y + 21, winpos.cx, winpos.cy - 21)
                    'SetWindowPos(frmOverlay.Handle, 0, winpos.x, winpos.y + 21, winpos.cx, winpos.cy - 21, SetWindowPosFlags.DoNotActivate Or SetWindowPosFlags.DoNotChangeOwnerZOrder Or SetWindowPosFlags.DoNotReposition)
                    frmOverlay.Bounds = New Rectangle(winpos.x, winpos.y + 21, winpos.cx, winpos.cy - 21)
                End If

                FrmBehind.Bounds = New Rectangle(winpos.x, winpos.y + 21, winpos.cx, winpos.cy - 21)

                If FrmSizeBorder IsNot Nothing AndAlso Me.WindowState = FormWindowState.Normal Then
                    FrmSizeBorder.Bounds = New Rectangle(winpos.x, winpos.y, winpos.cx, winpos.cy)
                End If

                If wasMaximized AndAlso caption_Mousedown Then
                    'Dim winpos As WINDOWPOS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                    'winpos.flags = SetWindowPosFlags.IgnoreMove
                    dBug.Print("WM_WINDOWPOSCHANGED from maximized And mousebutton down")
                    dBug.Print($"hwndInsertAfter {winpos.hwndInsertAfter}")
                    dBug.Print($"flags {winpos.flags}")
                    dBug.Print($"pos {winpos.x} {winpos.y} size {winpos.cx} {winpos.cy}")
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
                    dBug.Print($"winpos location {New Point(winpos.x, winpos.y)}")
                    dBug.Print($"winpos size {New Size(winpos.cx, winpos.cy)}")
                    'System.Runtime.InteropServices.Marshal.StructureToPtr(winpos, m.LParam, True)
                    FrmSizeBorder.Opacity = If(chkDebug.Checked, 1, 0.01)
                    FrmSizeBorder.Opacity = If(My.Settings.SizingBorder, FrmSizeBorder.Opacity, 0)
                    'posChangeBusy = False
                    Exit Sub
                End If
                Me.Invalidate() 'fix transparency on caption buttons when restore from min

            Case WM_STYLECHANGING '&h7C
                Dim ss As StyleStruct = Marshal.PtrToStructure(m.LParam, GetType(StyleStruct))
                dBug.Print($"WM_STYLECHANGING")
                If m.WParam = -20 Then 'ExStyle
                    dBug.Print($"From: {CType(ss.styleOld, WindowStylesEx)}")
                    dBug.Print($"  To: {CType(ss.styleNew, WindowStylesEx)}")
                ElseIf m.WParam = -16 Then 'Style
                    dBug.Print($"From: {CType(ss.styleOld, WindowStyles)}")
                    dBug.Print($"  To: {CType(ss.styleNew, WindowStyles)}")
                End If
                'Marshal.StructureToPtr(ss, m.LParam, True)
            Case WM_SETTINGCHANGE '&H1A 
                If m.WParam = 159 Then Exit Sub 'fix hang on Monitor Scaling change?
                'Detach(False) 'Fix hang on changing taskbar autohide/size
                dBug.Print($"WM_SETTINGCHANGE {m.LParam} {m.WParam}")
                Dim settingnname = Runtime.InteropServices.Marshal.PtrToStringAuto(m.LParam)
                dBug.Print($"sn=""{settingnname}""")
                If settingnname = "VisualEffects" Then
                    AnimsEnabled = getAnimationsEnabled()
                    If AnimsEnabled = False Then
                        CType(cmsQuickLaunch.Renderer, CustomToolStripRenderer)?.animTimer?.Stop()
                    Else
                        CType(cmsQuickLaunch.Renderer, CustomToolStripRenderer)?.animTimer?.Start()
                    End If
                    dBug.Print($"Animations {AnimsEnabled}")
                    'Attach(AltPP)
                End If
                If m.LParam = IntPtr.Zero AndAlso Me.WindowState = FormWindowState.Maximized Then
                    'handle taskbar changing
                    Dim newWA = Screen.FromPoint(Me.Location + New Point(Me.Width / 2, Me.Height / 2)).WorkingArea
                    'only do adjustment when size change or moved from top/bottom to sides
                    If newWA.Height <> prevWA.Height OrElse newWA.Width <> prevWA.Width Then
                        dBug.Print($"Taskbar changed {prevWA}->{newWA}")
                        Me.WindowState = FormWindowState.Normal
                        btnMax.PerformClick() 'todo replace with gracefull resizing
                    End If
                End If
                dBug.Print($"sRC:{suppressResChange} sMB:{Me.sizeMoveBusy} swpB:{swpBusy} mB:{moveBusy}")
                If m.WParam = 47 Then
                    moveBusy = False
                    If cboAlt.SelectedIndex > 0 Then
                        CmbResolution_SelectedIndexChanged(cmbResolution, EventArgs.Empty)
                    End If
                End If

                If My.Settings.Theme = 1 Then
                    Dim darkmode As Boolean = WinUsingDarkTheme()
                    dBug.Print($"Theme Changing dark={darkmode}")


                    ApplyTheme(darkmode)
                    Me.Invalidate()

                End If
                'clear QL iconcache
                iconCache.Clear()

            Case WM_CLIPBOARDUPDATE
                dBug.Print("WM_CLIPBOARDUPDATE")
                clipBoardInfo = GetClipboardFilesAndAction()
                UpdateMenuChecks(cmsQuickLaunch.Items, New HashSet(Of String)(clipBoardInfo.Files))

            Case WM_DISPLAYCHANGE
                dBug.Print($"WM_DISPLAYCHANGE w {m.WParam} w {m.LParam}")
                'cboAlt.SelectedIndex = 0
                Detach(False)
                ScreenManager.resetCache()



                'CheckScreenScalingModes()
                AstoniaProcess.BiggestWindowHeight = 0
                AstoniaProcess.BiggestWindowWidth = 0
            Case WM_SHNOTIFY
                dBug.Print("Assoc change")
                'clear non-wathed icons from cache
                EvictNonWatchedIcons()
            Case WM_ENTERMENULOOP
                dBug.Print($"WM_ENTERMENULOOP {cmsQuickLaunch.Visible}")
                SysMenu.Visible = Not cmsQuickLaunch.Visible
            Case WM_EXITMENULOOP
                dBug.Print("WM_EXITMENULOOP")
                SysMenu.Visible = False
            Case WM_INITMENU
                dBug.Print($"WM_INITMENU {m.WParam} {SysMenu.Handle}")
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

            Case WM_KEYDOWN
                dBug.Print($"WM_KEYDOWN w:{m.WParam} l:{m.LParam}")
                dBug.Print($"ScanCode {New LParamMap(m.LParam).scan}")
                If cboAlt.SelectedIndex > 0 Then
                    Attach(AltPP, True)
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
                dBug.Print($"WM_KEYUP w:{m.WParam} l:{m.LParam}")
                dBug.Print($"ScanCode {New LParamMap(m.LParam).scan}")
                If cboAlt.SelectedIndex > 0 Then
                    Attach(AltPP, True)
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
                dBug.Print($"WM_CHAR w:{m.WParam} l:{m.LParam}")
                dBug.Print($"ScanCode {New LParamMap(m.LParam).scan}")
                If cboAlt.SelectedIndex > 0 Then
                    Attach(AltPP, True)
                    If Not AltPP.isSDL Then
                        SendMessage(AltPP.MainWindowHandle, WM_CHAR, m.WParam, IntPtr.Zero)
                    End If
                End If
            Case WM_SYSKEYDOWN
                dBug.Print($"WM_SYSKEY {m.WParam} {m.LParam}")
                If cboAlt.SelectedIndex > 0 Then
                    Attach(AltPP, True)
                End If
            Case WM_NCACTIVATE
                dBug.Print("WM_NCACTIVATE")
                setActive(True)
                Me.Refresh()
            Case WM_ACTIVATEAPP
                Debug.Print($"WM_ACTIVATEAPP {m.WParam} {m.LParam} satid:{ScalaThreadId}")
                Dim tid = m.LParam
                Dim hThread As IntPtr = OpenThread(THREAD_QUERY_LIMITED_INFORMATION, False, tid)
                Try

                    Dim pid As Integer = GetProcessIdOfThread(hThread)

                    If pid = scalaPID Then
                        Debug.Print($"pid is ScalA")
                        EnumWindows(Function(h, l)
                                        'If Not IsWindowVisible(h) Then Return True
                                        If tid = GetWindowThreadProcessId(h, Nothing) Then
                                            Debug.Print($"window found: ""{GetWindowClass(h)}"" ""{GetWindowText(h)}""")
                                            Return False
                                        End If
                                        Return True
                                    End Function, IntPtr.Zero)
                    End If

                    Dim proc As Process = Process.GetProcessById(pid)
                    Debug.Print($"proc.ProcessName {pid} {proc.ProcessName}")
                Catch ex As Exception
                    Debug.Print($"WM_ACTIVATEAPP {ex.Message}")
                Finally
                    CloseHandle(hThread)
                End Try
#If DEBUG Then

            'Case &H6 ' WM_ACTIVATE
            'Case &H7 ' WM_SETFOCUS
            'Case &H8 ' WM_KILLFOCUS
            Case &HF ' WM_PAINT

            Case &HC ' WM_SETTEXT
            Case &HD ' WM_GETTEXT 
            Case &HE ' WM_GETTEXTLENGTH





            Case &H20 '	WM_SETCURSOR
            'Case &H21 ' WM_MOUSEACTIVATE
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

            Case &HC059 To &HC2BC ' unknown VS/Debug messages?

            Case Else
                Debug.Print($"Unhandeld {CType(m.Msg, WM_)} {m}")
#End If
        End Select

        MyBase.WndProc(m)  ' allow form to process this message

    End Sub

#If DEBUG Then
    Public Enum WM_ 'getnerated by chadGPT, needs sanity check
        WM_NULL = &H0
        WM_CREATE = &H1
        WM_DESTROY = &H2
        WM_MOVE = &H3
        WM_SIZE = &H5
        WM_ACTIVATE = &H6
        WM_SETFOCUS = &H7
        WM_KILLFOCUS = &H8
        WM_ENABLE = &HA
        WM_SETREDRAW = &HB
        WM_SETTEXT = &HC
        WM_GETTEXT = &HD
        WM_GETTEXTLENGTH = &HE
        WM_PAINT = &HF
        WM_CLOSE = &H10
        WM_QUERYENDSESSION = &H11
        WM_QUIT = &H12
        WM_QUERYOPEN = &H13
        WM_ERASEBKGND = &H14
        WM_SYSCOLORCHANGE = &H15
        WM_ENDSESSION = &H16
        WM_SHOWWINDOW = &H18
        WM_WININICHANGE = &H1A
        WM_SETTINGCHANGE = WM_WININICHANGE
        WM_DEVMODECHANGE = &H1B
        WM_ACTIVATEAPP = &H1C
        WM_FONTCHANGE = &H1D
        WM_TIMECHANGE = &H1E
        WM_CANCELMODE = &H1F
        WM_SETCURSOR = &H20
        WM_MOUSEACTIVATE = &H21
        WM_CHILDACTIVATE = &H22
        WM_QUEUESYNC = &H23
        WM_GETMINMAXINFO = &H24
        WM_PAINTICON = &H26
        WM_ICONERASEBKGND = &H27
        WM_NEXTDLGCTL = &H28
        WM_SPOOLERSTATUS = &H2A
        WM_DRAWITEM = &H2B
        WM_MEASUREITEM = &H2C
        WM_DELETEITEM = &H2D
        WM_VKEYTOITEM = &H2E
        WM_CHARTOITEM = &H2F
        WM_SETFONT = &H30
        WM_GETFONT = &H31
        WM_SETHOTKEY = &H32
        WM_GETHOTKEY = &H33
        WM_QUERYDRAGICON = &H37
        WM_COMPAREITEM = &H39
        WM_GETOBJECT = &H3D
        WM_COMPACTING = &H41
        WM_COMMNOTIFY = &H44
        WM_WINDOWPOSCHANGING = &H46
        WM_WINDOWPOSCHANGED = &H47
        WM_POWER = &H48
        WM_COPYDATA = &H4A
        WM_CANCELJOURNAL = &H4B
        WM_NOTIFY = &H4E
        WM_INPUTLANGCHANGEREQUEST = &H50
        WM_INPUTLANGCHANGE = &H51
        WM_TCARD = &H52
        WM_HELP = &H53
        WM_USERCHANGED = &H54
        WM_NOTIFYFORMAT = &H55
        WM_CONTEXTMENU = &H7B
        WM_STYLECHANGING = &H7C
        WM_STYLECHANGED = &H7D
        WM_DISPLAYCHANGE = &H7E
        WM_GETICON = &H7F
        WM_SETICON = &H80
        WM_NCCREATE = &H81
        WM_NCDESTROY = &H82
        WM_NCCALCSIZE = &H83
        WM_NCHITTEST = &H84
        WM_NCPAINT = &H85
        WM_NCACTIVATE = &H86
        WM_GETDLGCODE = &H87
        WM_SYNCPAINT = &H88
        WM_NCMOUSEMOVE = &HA0
        WM_NCLBUTTONDOWN = &HA1
        WM_NCLBUTTONUP = &HA2
        WM_NCLBUTTONDBLCLK = &HA3
        WM_NCRBUTTONDOWN = &HA4
        WM_NCRBUTTONUP = &HA5
        WM_NCRBUTTONDBLCLK = &HA6
        WM_NCMBUTTONDOWN = &HA7
        WM_NCMBUTTONUP = &HA8
        WM_NCMBUTTONDBLCLK = &HA9
        WM_KEYDOWN = &H100
        WM_KEYUP = &H101
        WM_CHAR = &H102
        WM_DEADCHAR = &H103
        WM_SYSKEYDOWN = &H104
        WM_SYSKEYUP = &H105
        WM_SYSCHAR = &H106
        WM_SYSDEADCHAR = &H107
        WM_UNICHAR = &H109
        WM_IME_STARTCOMPOSITION = &H10D
        WM_IME_ENDCOMPOSITION = &H10E
        WM_IME_COMPOSITION = &H10F
        WM_IME_KEYLAST = &H10F
        WM_INITDIALOG = &H110
        WM_COMMAND = &H111
        WM_SYSCOMMAND = &H112
        WM_TIMER = &H113
        WM_HSCROLL = &H114
        WM_VSCROLL = &H115
        WM_INITMENU = &H116
        WM_INITMENUPOPUP = &H117
        WM_MENUSELECT = &H11F
        WM_MENUCHAR = &H120
        WM_ENTERIDLE = &H121
        WM_MENURBUTTONUP = &H122
        WM_MENUDRAG = &H123
        WM_MENUGETOBJECT = &H124
        WM_UNINITMENUPOPUP = &H125
        WM_MENUCOMMAND = &H126
        WM_CHANGEUISTATE = &H127
        WM_UPDATEUISTATE = &H128
        WM_QUERYUISTATE = &H129
        WM_CTLCOLORMSGBOX = &H132
        WM_CTLCOLOREDIT = &H133
        WM_CTLCOLORLISTBOX = &H134
        WM_CTLCOLORBTN = &H135
        WM_CTLCOLORDLG = &H136
        WM_CTLCOLORSCROLLBAR = &H137
        WM_CTLCOLORSTATIC = &H138
        WM_MOUSEMOVE = &H200
        WM_LBUTTONDOWN = &H201
        WM_LBUTTONUP = &H202
        WM_LBUTTONDBLCLK = &H203
        WM_RBUTTONDOWN = &H204
        WM_RBUTTONUP = &H205
        WM_RBUTTONDBLCLK = &H206
        WM_MBUTTONDOWN = &H207
        WM_MBUTTONUP = &H208
        WM_MBUTTONDBLCLK = &H209
        WM_MOUSEWHEEL = &H20A
        WM_MOUSEHWHEEL = &H20E
        WM_PARENTNOTIFY = &H210
        WM_ENTERMENULOOP = &H211
        WM_EXITMENULOOP = &H212
        WM_NEXTMENU = &H213
        WM_SIZING = &H214
        WM_CAPTURECHANGED = &H215
        WM_MOVING = &H216
        WM_DEVICECHANGE = &H219
        WM_MDICREATE = &H220
        WM_MDIDESTROY = &H221
        WM_MDIACTIVATE = &H222
        WM_MDIRESTORE = &H223
        WM_MDINEXT = &H224
        WM_MDIMAXIMIZE = &H225
        WM_MDITILE = &H226
        WM_MDICASCADE = &H227
        WM_MDIICONARRANGE = &H228
        WM_MDIGETACTIVE = &H229
        WM_MDISETMENU = &H230
        WM_ENTERSIZEMOVE = &H231
        WM_EXITSIZEMOVE = &H232
        WM_DROPFILES = &H233
        WM_MDIREFRESHMENU = &H234
    End Enum

#End If


    Dim prevLoc As Point
    Dim sizeMoveBusy As Boolean = False


End Class