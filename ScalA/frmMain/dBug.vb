Imports System.IO.MemoryMappedFiles
Imports System.Runtime.InteropServices
#If DEBUG Then
Module dBug
    Friend Sub ParseInfo(sender As Object, e As EventArgs)
        If FrmMain.AltPP Is Nothing Then Exit Sub
        Dim QS As New Management.ManagementObjectSearcher(“Select * from Win32_Process WHERE ProcessID=" & FrmMain.AltPP.Id)
        Dim objCol As Management.ManagementObjectCollection = QS.Get

        Dim cmdLine As String = objCol(0)("commandline")

        If cmdLine?.StartsWith("""") Then
            cmdLine = cmdLine.Substring(1)
            cmdLine = cmdLine.Substring(cmdLine.IndexOf("""") + 1)
        ElseIf cmdLine?.StartsWith("new.exe") Then
            cmdLine = cmdLine.Substring(7)
        End If

        Dim exePath As String = objCol(0)("ExecutablePath")

        CustomMessageBox.Show(exePath & vbCrLf & cmdLine)
    End Sub

    Friend Sub ResetHide(sender As Object, e As EventArgs)
        FrmMain.chkHideMessage.Checked = False
    End Sub

    Friend Sub Resumelayout(sender As Object, e As EventArgs)
        FrmMain.pnlOverview.ResumeLayout(True)
    End Sub

    Friend Sub ButtonInfo(sender As Object, e As EventArgs)
        Dim i = 1
        For Each but As AButton In FrmMain.pnlOverview.Controls.OfType(Of AButton).Where(Function(b) b.Visible)
            Debug.Print($"Button {i} Size: {but.Size} thuRect: {but.ThumbRECT}")
            i += 1
        Next
        Debug.Print($"viscount: {FrmMain.pnlOverview.Controls.OfType(Of Button).Where(Function(b) b.Visible).Count}")
    End Sub

    Friend Sub IsBelow(sender As Object, e As EventArgs)
        Debug.Print($"isBelow {FrmMain.AltPP?.IsBelow(FrmMain.Handle)}")
    End Sub

    Friend Sub ToggleUpdate(sender As Object, e As EventArgs)
        FrmMain.pnlUpdate.Visible = Not FrmMain.pnlUpdate.Visible
    End Sub

    Friend Async Sub ScreenScaling(sender As Object, e As EventArgs)

        Dim sw As Stopwatch = Stopwatch.StartNew
        For Each scrn As Screen In Screen.AllScreens
            Debug.Print($"{scrn.DeviceName} {scrn.Bounds} {scrn.ScalingPercent}%")
        Next
        Debug.Print(sw.ElapsedMilliseconds)
        Debug.Print("-PFE-")
        sw.Restart()
        Parallel.ForEach(Screen.AllScreens, Sub(scrn As Screen)
                                                Debug.Print($"{scrn.DeviceName} {scrn.Bounds} {scrn.ScalingPercent}%")
                                            End Sub)
        Debug.Print(sw.ElapsedMilliseconds)
        Debug.Print("-TSK-")
        sw.Restart()
        For Each scrn As Screen In Screen.AllScreens
            Debug.Print($"{scrn.DeviceName} {scrn.Bounds} {Await scrn.ScalingPercentTask}%")
        Next
        Debug.Print(sw.ElapsedMilliseconds)
        sw.Stop()
    End Sub
    'struct sharedmem {
    '    unsigned int pid; 0
    '    Char hp, shield,end, mana; 4 5 6 7
    '    Char * base; 8
    '    int key, isprite, offX, offY; 16 20 24 28
    '    int flags, fsprite; 32 36
    '    Char swapped; 37
    '} __attribute__((packed));
    Friend Sub SharedMem(sender As Object, e As EventArgs)
        Dim map As MemoryMappedFile = MemoryMappedFile.CreateOrOpen($"MOAC{FrmMain.AltPP?.Id}", 37)
        Dim va As MemoryMappedViewAccessor = map.CreateViewAccessor()
        Debug.Print($"shmem {va.ReadInt32(0)} id {FrmMain.AltPP?.Id}")
        Debug.Print($"hp {va.ReadByte(4)} shield {va.ReadByte(5)} end {va.ReadByte(6)} mana {va.ReadByte(7)}")
    End Sub

    Friend Sub DebugAlt(sender As ToolStripMenuItem, e As EventArgs)
        Dim alt As AstoniaProcess = sender.Tag
        Debug.Print($"DebugAlt {alt.Name}")
        Debug.Print($"client rc {alt.ClientRect}")
        Debug.Print($"window rc {alt.WindowRect}")
    End Sub

    Friend Sub toggeleborder(sender As Object, e As EventArgs)
        'Static restoreWS As WindowStyles = GetWindowLong(FrmMain.AltPP.MainWindowHandle, GWL_STYLE) 'todo replace with magic number? legacy/vs sdl?
        'Debug.Print($"restorews {restoreWS}")
        Dim restoreWS As WindowStyles
        If FrmMain.AltPP.isSDL Then
            restoreWS = WindowStyles.WS_MINIMIZEBOX Or
                        WindowStyles.WS_SYSMENU Or
                        WindowStyles.WS_CAPTION Or
                        WindowStyles.WS_CLIPCHILDREN Or
                        WindowStyles.WS_CLIPSIBLINGS Or WindowStyles.WS_VISIBLE
        Else
            restoreWS = WindowStyles.WS_CAPTION Or WindowStyles.WS_CLIPSIBLINGS Or WindowStyles.WS_VISIBLE Or WindowStyles.WS_POPUP
        End If
        If FrmMain.AltPP.hasBorder() Then
            SetWindowLong(FrmMain.AltPP.MainWindowHandle, GWL_STYLE, WindowStyles.WS_POPUP Or WindowStyles.WS_VISIBLE)
            SetWindowPos(FrmMain.AltPP.MainWindowHandle, SWP_HWND.TOP, -1, -1, -1, -1,
                         SetWindowPosFlags.IgnoreZOrder Or
                         SetWindowPosFlags.IgnoreMove Or
                         SetWindowPosFlags.IgnoreResize Or
                         SetWindowPosFlags.FrameChanged)
        Else
            SetWindowLong(FrmMain.AltPP.MainWindowHandle, GWL_STYLE, restoreWS)
            SetWindowPos(FrmMain.AltPP.MainWindowHandle, SWP_HWND.TOP, -1, -1, -1, -1,
                         SetWindowPosFlags.IgnoreZOrder Or
                         SetWindowPosFlags.IgnoreMove Or
                         SetWindowPosFlags.IgnoreResize Or
                         SetWindowPosFlags.FrameChanged)
        End If
    End Sub

    Friend Sub querySize(sender As Object, e As EventArgs)
        Dim sz As Size
        Dim ret = DwmQueryThumbnailSourceSize(FrmMain.thumb, sz)
        Debug.Print($"{ret} {sz}")
    End Sub

    Friend Sub fudgeThumb(sender As Object, e As EventArgs)

        'Debug.Print($"create {FrmMain.CreateThumb()}")

        'FrmMain.AnimateThumb(New Rectangle(FrmMain.PnlEqLock.Left, FrmMain.PnlEqLock.Top, FrmMain.PnlEqLock.Right, FrmMain.PnlEqLock.Bottom), New Rectangle(FrmMain.pbZoom.Left, FrmMain.pbZoom.Top, FrmMain.pbZoom.Right, FrmMain.pbZoom.Bottom), 2000)
        'Await Task.Delay(50)
        'FrmMain.AltPP.ResetCache()
        'AppActivate(FrmMain.AltPP.Id)

        'DwmUnregisterThumbnail(FrmMain.thumb)
        'DwmRegisterThumbnail(FrmMain.Handle, FrmMain.AltPP.MainWindowHandle, FrmMain.thumb)

        'Dim tp As New DWM_THUMBNAIL_PROPERTIES With {
        '    .dwFlags = DwmThumbnailFlags.DWM_TNP_SOURCECLIENTAREAONLY Or DwmThumbnailFlags.DWM_TNP_RECTDESTINATION Or DwmThumbnailFlags.DWM_TNP_VISIBLE,
        '    .fSourceClientAreaOnly = True,
        '    .rcDestination = New Rectangle(FrmMain.pbZoom.Left, FrmMain.pbZoom.Top, FrmMain.pbZoom.Right, FrmMain.pbZoom.Bottom),
        '    .fVisible = True}

        ''tp.dwFlags = tp.dwFlags Or DwmThumbnailFlags.DWM_TNP_RECTSOURCE
        ''tp.rcSource = New Rectangle(0, 0, 0, 0)
        ''tp.rcSource = Nothing

        'Dim ret = DwmUpdateThumbnailProperties(FrmMain.thumb, tp)
        'Dim rcc As New RECT
        'GetClientRect(FrmMain.AltPP.MainWindowHandle, rcc)
        'Debug.Print($"rcc:{rcc}")
        'Dim rcFrame As RECT
        'DwmGetWindowAttribute(FrmMain.AltPP.MainWindowHandle, 9, rcFrame, System.Runtime.InteropServices.Marshal.SizeOf(rcFrame))
        'Debug.Print($"rcf:{rcFrame.ToRectangle}")

        Dim rcc As RECT
        GetClientRect(FrmMain.AltPP.MainWindowHandle, rcc)
        Dim rcw As RECT
        GetWindowRect(FrmMain.AltPP.MainWindowHandle, rcc)
        Const DWMWA_EXTENDED_FRAME_BOUNDS As Integer = 9
        Dim rcf As RECT
        DwmGetWindowAttribute(FrmMain.AltPP.MainWindowHandle, DWMWA_EXTENDED_FRAME_BOUNDS, rcf, System.Runtime.InteropServices.Marshal.SizeOf(rcf))
        Debug.Print($"rcc {rcc.ToRectangle} rcw {rcw.ToRectangle} rcf {rcf.ToRectangle}")

    End Sub

    Friend Sub NudgeTaskbar(sender As Object, e As EventArgs)
        SetWindowLong(FrmMain.ScalaHandle, GWL_HWNDPARENT, 0)
        'Debug.Print("1:" & FlashWindow(FrmMain.ScalaHandle, True))
        'Debug.Print("2:" & FlashWindow(FrmMain.ScalaHandle, False))
        'AppActivate(FrmMain.scalaPID)
        FrmMain.Activate()
    End Sub

    Friend Sub thumbStuff(sender As Object, e As EventArgs)
        Dim tb As Integer
        DwmRegisterThumbnail(FrmMain.ScalaHandle, FrmMain.AltPP.MainWindowHandle, tb)

        Dim tp As New DWM_THUMBNAIL_PROPERTIES With {.fSourceClientAreaOnly = True, .dwFlags = DwmThumbnailFlags.DWM_TNP_SOURCECLIENTAREAONLY}

        DwmUpdateThumbnailProperties(tb, tp)

        Dim sz As Size
        DwmQueryThumbnailSourceSize(tb, sz)
        Debug.Print($"size {sz}")
        DwmUnregisterThumbnail(tb)
    End Sub

    Friend Sub listothers(sender As Object, e As EventArgs)
        'Task.Run(Sub()
        '             Dim sw = Stopwatch.StartNew
        '             Dim list = EnumOtherScalAs().ToList
        '             Debug.Print($"- {list.Count} other ScalA{If(list.Count = 1, "", "s")} -")
        '             For Each pp As Process In list
        '                 Debug.Print($"{pp.Id}")
        '             Next
        '             Debug.Print($"- {sw.ElapsedMilliseconds}ms -")
        '             sw.Stop()
        '         End Sub)

        Debug.Print($"size: {Marshal.SizeOf(GetType(ScalAInfo))}")
        Debug.Print($"{New ScalAInfo}")

        For Each sai As IPC.ScalAInfo In IPC.getInstances()
            Dim pp = Process.GetProcessById(sai.pid)
            Debug.Print($"{pp.ProcessName} {pp.Path} {sai.isOnOverview} {sai.handle} {sai.AltPPid}")
        Next

    End Sub

    Friend Sub hookKey(sender As Object, e As EventArgs)
        If keybHook.HookHandle = IntPtr.Zero Then
            Debug.Print("hooking")
            keybHook.Hook()
        Else
            Debug.Print("unhook")
            keybHook.Unhook()
        End If
    End Sub

    Friend Sub ipcSize(sender As Object, e As EventArgs)
        Debug.Print($"IPC Size: {Marshal.SizeOf(GetType(IPC.ScalAInfo))}")
    End Sub

    Friend Sub dumpApCache(sender As Object, e As EventArgs)
        Debug.Print($"AP procCache {AstoniaProcess.ProcCache.Count}")
        Debug.Print($"AP Loggedins {AstoniaProcess.loggedIns.Count}")

        AstoniaProcess.loggedIns.Values.ToList.ForEach(Sub(ap)
                                                           Debug.Print($"{ap.Name} {ap.hasLoggedIn}")
                                                       End Sub)
    End Sub

    Friend Sub regFudge(sender As Object, e As EventArgs)
        If FrmMain.AltPP IsNot Nothing Then FrmMain.AltPP.RegHighDpiAware = False
    End Sub

    Friend Async Sub RestartClient(sender As Object, e As EventArgs)

        Dim targetname As String = FrmMain.AltPP.loggedInAs
        Debug.Print($"restarting {targetname}")



        FrmMain.AltPP.restart()
        FrmMain.Cursor = Cursors.WaitCursor
        Dim count As Integer = 0

        While True
            count += 1
            Await Task.Delay(50)
            Dim targetPPs As AstoniaProcess() = AstoniaProcess.Enumerate(FrmMain.blackList).Where(Function(ap) ap.Name = targetname).ToArray()
            If targetPPs.Length > 0 AndAlso targetPPs(0) IsNot Nothing AndAlso targetPPs(0).Id <> 0 Then
                FrmMain.PopDropDown(FrmMain.cboAlt)
                FrmMain.cboAlt.SelectedItem = targetPPs(0)
                Exit While
            End If
            If count >= 100 Then
                CustomMessageBox.Show(FrmMain, "Windowing failed")
                Exit While
            End If
        End While
        FrmMain.Cursor = Cursors.Default
    End Sub

    Friend Sub ShowRelButton(sender As Object, e As EventArgs)
        frmOverlay.pbRestart.Show()
    End Sub
End Module

#End If