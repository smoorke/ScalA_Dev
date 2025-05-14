﻿
Public Class Timers
    'dummy to prevent form generation
End Class

Partial NotInheritable Class FrmMain


    'Dim rcW As Rectangle ' windowrect
    Dim rcC As Rectangle ' clientrect
    Public newX As Integer
    Public newY As Integer
    Public Shared ScalaHandle As IntPtr
    Private storedX As Integer = 0
    Private storedY As Integer = 0
    Private wasVisible As Boolean = True
    Private Shared swpBusy As Boolean = False
    Const swpFlags As SetWindowPosFlags = SetWindowPosFlags.IgnoreResize Or
                     SetWindowPosFlags.DoNotActivate Or
                     SetWindowPosFlags.ASyncWindowPosition
    Private Sub TmrTick_Tick(sender As Timer, e As EventArgs) Handles tmrTick.Tick
        'dBug.print($"ws {Me.WindowState}")
        If Not AltPP?.IsRunning() Then
            dBug.Print($"Not AltPP?.IsRunning() {Me.WindowState}")
            Me.Show()
            FrmBehind.Show()
            If Not FrmSizeBorder.Visible AndAlso My.Settings.SizingBorder Then FrmSizeBorder.Show()
            If Not My.Settings.CycleOnClose Then
                BringToFront()
                cboAlt.SelectedIndex = 0
                tmrOverview.Enabled = True
                tmrTick.Enabled = False
                Detach(True)
                'FlashWindow(ScalaHandle, True) 'show on taskbar
                'FlashWindow(ScalaHandle, False) 'stop blink
                '                Dim timeout As UInteger
                '                Dim ret = SystemParametersInfo(SPI.GETFOREGROUNDLOCKTIMEOUT, 0, timeout, 0)
                '                timeout = Math.Max(100, timeout + 1)
                '                dBug.print($"timeout:{timeout} {ret}")
                '                Try
                '                    Task.Run(action:=Async Sub()
                '                                         Await Task.Delay(timeout / 2)
                '                                         AllowSetForegroundWindow(scalaPID)
                '                                         AppActivate(Process.GetProcessesByName("Explorer").First().Id)
                '                                         Await Task.Delay(timeout / 2)
                '                                         AllowSetForegroundWindow(scalaPID)
                '                                         AppActivate(scalaPID)
                '                                         Me.Invoke(Sub() Me.Activate())
                '                                     End Sub)
                '                Catch ex As Exception
                '#If DEBUG Then
                '                    MessageBox.Show(ex.Message)
                '#End If
                '                End Try
                Try
                    AppActivate(scalaPID)
                Catch ex As Exception

                End Try
                Exit Sub
            Else 'CycleOnClose
                Cycle()
            End If
        End If

        If Not UpdateTitle() Then Exit Sub

        If Me.WindowState = FormWindowState.Minimized Then
            altSelectedOrOverview = Nothing
            Exit Sub
        End If

        altSelectedOrOverview = {AltPP}.ToList

        Dim pci As New CURSORINFO With {.cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(CURSORINFO))}
        GetCursorInfo(pci)
        If pci.flags <> 0 Then ' cursor is visible
            If Not wasVisible AndAlso AltPP?.IsActive() Then
                dBug.Print("scrollthumb released")
                If storedY <> pci.ptScreenpos.y OrElse storedX <> pci.ptScreenpos.x Then
                    dBug.Print("scrollthumb moved")
                    Dim Xfactor As Double = pbZoom.Width / rcC.Width
                    Dim Yfactor As Double = pbZoom.Height / rcC.Height
                    Dim movedX As Integer = storedX + ((pci.ptScreenpos.x - storedX) * Xfactor)
                    Dim movedY As Integer = storedY + ((pci.ptScreenpos.y - storedY) * Yfactor)
                    If movedY >= Me.Bottom Then movedY = Me.Bottom - 2
                    Cursor.Position = New Point(movedX, movedY)

                    Dim bzB As Rectangle = Me.RectangleToScreen(pbZoom.Bounds)
                    Dim ipt As New Point(movedX.Map(bzB.Left, bzB.Right, 0, rcC.Width),
                                         movedY.Map(bzB.Top, bzB.Bottom, 0, rcC.Height))
                    SendMessage(AltPP.MainWindowHandle, WM_MOUSEMOVE, WM_MOUSEMOVE_CreateWParam, New LParamMap(ipt)) 'update client internal mousepos
                    dBug.Print($"ipt {ipt}")
                End If
            End If
            storedX = pci.ptScreenpos.x
            storedY = pci.ptScreenpos.y
            wasVisible = True
        End If

        If pbZoom.Contains(MousePosition) Then

            If pci.flags = 0 Then ' cursor is hidden
                wasVisible = False
                Exit Sub ' do not move astonia when cursor is hidden. fixes scrollbar thumb.
                ' note there is a client bug where using thumb will intermittently cause it to jump down wildly
            End If

            If My.Settings.HoverActivate Then
                Dim id = GetActiveProcessID()
                If id <> 0 AndAlso id = scalaPID OrElse AltPP?.Id <> id Then
                    If Not (SysMenu.Visible OrElse cboAlt.DroppedDown OrElse cmbResolution.DroppedDown OrElse
                                                FrmSettings.Visible OrElse UpdateDialog.Contains(MousePosition) OrElse
                                                renameOpen OrElse CustomMessageBox.visible) Then
#If DEBUG Then
                        If Not chkDebug.ContextMenuStrip.Visible Then
#End If
                            'ap.Activate() doesn't work if not debugging
                            If Not AltPP?.IsActive AndAlso WindowFromPoint(MousePosition) = AltPP?.MainWindowHandle Then
                                dBug.Print($"Activating {AltPP?.Name}")
                                SendMouseInput(MouseEventF.XDown Or MouseEventF.XUp, 2)
                            End If
#If DEBUG Then
                        End If
#End If
                    End If
                End If
            End If


            Dim ptZ As Point = Me.PointToScreen(pbZoom.Location)

            If AltPP Is Nothing Then Exit Sub

            newX = MousePosition.X.Map(ptZ.X, ptZ.X + pbZoom.Width, ptZ.X, ptZ.X + pbZoom.Width - rcC.Width) - If(AltPP?.ClientOffset.X, 0) '- My.Settings.offset.X
            newY = MousePosition.Y.Map(ptZ.Y, ptZ.Y + pbZoom.Height, ptZ.Y, ptZ.Y + pbZoom.Height - rcC.Height) - If(AltPP?.ClientOffset.Y, 0) '- My.Settings.offset.Y

            If Not swpBusy AndAlso Not moveBusy AndAlso Not Resizing Then
                swpBusy = True
                Task.Run(Sub()
                             Try
                                 If AltPP Is Nothing OrElse Not AltPP.IsRunning Then Exit Sub
                                 Dim ci As New CURSORINFO With {.cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(CURSORINFO))}
                                 GetCursorInfo(ci)
                                 If ci.flags = 0 Then Exit Sub
                                 swpBusy = True
                                 Dim flags = swpFlags
                                 If Not AltPP?.IsActive() Then flags.SetFlag(SetWindowPosFlags.DoNotChangeOwnerZOrder)
                                 If AltPP?.IsBelow(ScalaHandle) Then flags.SetFlag(SetWindowPosFlags.IgnoreZOrder)
                                 Dim pt As Point = MousePosition - New Point(newX + If(AltPP?.ClientOffset.X, 0), newY + If(AltPP?.ClientOffset.Y, 0))
                                 Dim wparam = WM_MOUSEMOVE_CreateWParam()
                                 Dim lparam = New LParamMap(pt)
                                 If prevWMMMpt <> MousePosition Then
                                     SendMessage(If(AltPP?.MainWindowHandle, IntPtr.Zero), WM_MOUSEMOVE, wparam, lparam) 'update client internal mousepos
                                 End If
                                 SetWindowPos(If(AltPP?.MainWindowHandle, IntPtr.Zero), ScalaHandle, newX, newY, -1, -1, flags)
                                 If prevWMMMpt <> MousePosition Then
                                     SendMessage(If(AltPP?.MainWindowHandle, IntPtr.Zero), WM_MOUSEMOVE, wparam, lparam) 'update client internal mousepos
                                 End If
                                 prevWMMMpt = MousePosition
                             Catch ex As Exception
                                 dBug.Print(ex.Message)
                             Finally
                                 swpBusy = False
                             End Try
                         End Sub)
            End If
        End If

#If DEBUG Then
        TickCounter += 1
        If TickTimer.ElapsedMilliseconds >= 1000 Then
            HeartBeat = Not HeartBeat
            chkDebug.Text = $"{If(HeartBeat, "♡", "")}{TickCounter}"
            TickCounter = 0
            TickTimer.Restart()
        End If
#End If


    End Sub
    Dim prevWMMMpt As New Point

    Public Shared ReadOnly startThumbsDict As New Concurrent.ConcurrentDictionary(Of Integer, IntPtr)
    Shared ReadOnly opaDict As New Concurrent.ConcurrentDictionary(Of Integer, Byte)
    Shared ReadOnly rectDic As New Concurrent.ConcurrentDictionary(Of Integer, Rectangle)
    Shared ReadOnly swDict As New Concurrent.ConcurrentDictionary(Of Integer, Stopwatch)

#If DEBUG Then
    Private TickCounter As Integer = 0
    Private TickTimer As Stopwatch = Stopwatch.StartNew
    Private HeartBeat As Boolean = False
#End If

    Friend Shared AOBusy As Boolean = False
    Private AOshowEqLock As Boolean = False

    Friend Shared apSorter As AstoniaProcessSorter
    Private ovBusy As Boolean = False
    Private Shared lockObject As New Object()
    Private Async Sub TimerOverview_Tick(sender As Timer, e As EventArgs) Handles tmrOverview.Tick

        If Me.WindowState = FormWindowState.Minimized Then
            altSelectedOrOverview = Nothing
            Exit Sub
        End If

        If ovBusy Then Exit Sub
        ovBusy = True
        'SyncLock lockObject

#If DEBUG Then
        If TickTimer.ElapsedMilliseconds >= 1000 Then
            HeartBeat = Not HeartBeat
            TickTimer.Restart()
        End If
        chkDebug.Text = $"{If(HeartBeat, "♡", "")}{TickCounter}"
#End If

        Dim alts As List(Of AstoniaProcess) = AstoniaProcess.Enumerate(blackList, True).OrderBy(Function(ap) ap.Name, apSorter).ToList

        Dim visibleButtons As List(Of AButton) = UpdateButtonLayout(alts.Count)

        Dim botCount = alts.Where(Function(ap) botSortList.Contains(ap.Name)).Count()
        Dim topCount = alts.Count - botCount
        Dim skipCount = visibleButtons.Count - botCount

        'Dim apCounter = 0
        'Dim butCounter = 0
        Dim eqLockShown = False

        Await Task.Run(
            Sub()
                Parallel.ForEach(visibleButtons, New ParallelOptions With {.MaxDegreeOfParallelism = -1},
                    Sub(but As AButton, ls As ParallelLoopState, butCounter As Integer)
                        Try
                            Dim apCounter = butCounter
                            If apCounter >= topCount Then apCounter = butCounter - skipCount + topCount
                            'dBug.print($"pfe tick{TickCounter} but{butCounter} ap{apCounter} bot{botCount} top{topCount} skip{skipCount}")
                            If apCounter < alts.Count AndAlso
                                    (butCounter < topCount OrElse butCounter >= skipCount) AndAlso
                                    Not alts(apCounter).HasExited Then
                                'buttons with alts

                                Dim ap As AstoniaProcess = alts(apCounter)
                                Dim apID = ap.Id
                                but.AP = ap
                                but.BeginInvoke(Sub() but.Text = ap.Name)

                                If ap.IsActive() Then
                                    but.Font = AButton.BoldFont
                                    but.BeginInvoke(Sub() but.Select())
                                Else
                                    but.Font = AButton.NormalFont
                                End If

                                If Not ap.isSDL Then
                                    Dim exStyle As UInteger = GetWindowLong(ap.MainWindowHandle, GWL_EXSTYLE)
                                    If (exStyle And WindowStylesEx.WS_EX_COMPOSITED) <> WindowStylesEx.WS_EX_COMPOSITED Then
                                        SetWindowLong(ap.MainWindowHandle, GWL_EXSTYLE, exStyle Or WindowStylesEx.WS_EX_COMPOSITED)
                                        Debug.Print($"set {ap.Name} as composited")
                                    End If
                                End If

                                If but.pidCache <> ap.Id Then but.BackgroundImage = Nothing
                                If but.BackgroundImage Is Nothing Then
                                    Using ico As Bitmap = ap.GetIcon?.ToBitmap
                                        Dim img As Image = Nothing
                                        If ico IsNot Nothing Then
                                            img = New Bitmap(ico, New Size(16, 16))
                                            dBug.Print($"{ap.Name} icon updated")
                                        End If
                                        Me.BeginInvoke(Sub() but.BackgroundImage = img)
                                    End Using
                                End If
                                but.pidCache = ap.Id

                                Dim sw = swDict.GetOrAdd(ap.Id, Stopwatch.StartNew)
                                If but.Image Is Nothing OrElse sw.ElapsedMilliseconds > 66 Then
                                    Task.Run(Sub()
                                                 Dim img As Image = ap.GetHealthbar
                                                 Me.BeginInvoke(Sub() but.Image = img)
                                             End Sub)
                                    sw.Restart()
                                End If

                                but.ContextMenuStrip = cmsAlt
                                'Me.Invoke(Function() cboAlt.SelectedIndex = 0) 'do not use index as it changes when hovering dropdown items
                                If pnlOverview.Visible Then
                                    If Not startThumbsDict.ContainsKey(apID) Then

                                        Dim thumbid As IntPtr = IntPtr.Zero
                                        DwmRegisterThumbnail(ScalaHandle, ap.MainWindowHandle, thumbid)
                                        startThumbsDict(apID) = thumbid
                                        dBug.Print($"registered thumb {startThumbsDict(apID)} {ap.Name} {apID}")
                                    End If

                                    rectDic(apID) = but.ThumbRECT
                                    Dim prp As New DWM_THUMBNAIL_PROPERTIES With {
                                           .dwFlags = DwmThumbnailFlags.DWM_TNP_OPACITY Or DwmThumbnailFlags.DWM_TNP_VISIBLE Or DwmThumbnailFlags.DWM_TNP_RECTDESTINATION Or DwmThumbnailFlags.DWM_TNP_SOURCECLIENTAREAONLY,
                                           .opacity = opaDict.GetValueOrDefault(apID, If(chkDebug.Checked, 128, 255)),
                                           .fVisible = True,
                                           .rcDestination = rectDic(apID),
                                           .fSourceClientAreaOnly = True}


                                    DwmUpdateThumbnailProperties(startThumbsDict(apID), prp)
                                End If
                            Else 'buttons w/o alts
                                but.BeginInvoke(Sub() but.Text = "")
                                but.AP = Nothing
                                but.ContextMenuStrip = cmsQuickLaunch
                                but.BackgroundImage = Nothing
                                but.Image = Nothing
                                but.pidCache = 0
                            End If
                        Catch
                        End Try
                    End Sub)
            End Sub)

        Dim thumbContainedMouse As Boolean = False

        Try
            If My.Settings.gameOnOverview AndAlso Not caption_Mousedown Then
                Dim but As AButton = visibleButtons.Find(Function(ab) ab.pidCache > 0 AndAlso ab.ThumbContains(MousePosition))
                If but IsNot Nothing Then
                    thumbContainedMouse = True
                    Dim ap = but.AP
                    Dim rcwB As Rectangle = ap.WindowRect
                    Dim rccB As Rectangle = ap.ClientRect
                    Dim pci As New CURSORINFO With {.cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(CURSORINFO))}
                    GetCursorInfo(pci)
                    If pci.flags <> 0 Then ' cursor is visible
                        If Not wasVisible AndAlso ap.IsActive() Then
                            dBug.print("scrollthumb released")
                            If storedY <> pci.ptScreenpos.y OrElse storedX <> pci.ptScreenpos.x Then
                                dBug.print("scrollthumb moved")
                                Dim Xfactor As Double = but.ThumbRectangle.Width / ap.ClientRect.Width
                                Dim Yfactor As Double = but.ThumbRectangle.Height / ap.ClientRect.Height
                                Dim movedX As Integer = storedX + ((pci.ptScreenpos.x - storedX) * Xfactor)
                                Dim movedY As Integer = storedY + ((pci.ptScreenpos.y - storedY) * Yfactor)
                                Cursor.Position = New Point(movedX, movedY)

                                Dim bzB As Rectangle = but.RectangleToScreen(but.ThumbRectangle)
                                Dim ipt As New Point(movedX.Map(bzB.Left, bzB.Right, 0, rccB.Width),
                                                     movedY.Map(bzB.Top, bzB.Bottom, 0, rccB.Height))
                                SendMessage(AltPP.MainWindowHandle, WM_MOUSEMOVE, WM_MOUSEMOVE_CreateWParam(), New LParamMap(ipt)) 'update client internal mousepos
                                dBug.print($"ipt {ipt}")
                            End If
                        End If
                        storedX = pci.ptScreenpos.x
                        storedY = pci.ptScreenpos.y
                        wasVisible = True
                    End If

                    If Not AOBusy Then
                        AltPP = ap
                        If ap.IsMinimized Then
                            dBug.print($"before {rcwB} {rccB}")
                            ap.Restore()
                            rcwB = ap.WindowRect
                            rccB = ap.ClientRect
                            dBug.print($"after {rcwB} {rccB}")
                        End If

                        If pci.flags = 0 Then ' cursor is hidden do not move astonia. fixes scrollbar thumb.
                            wasVisible = False ' note there is a client bug where using thumb will intermittently cause it to jump down wildly
                        Else

                            If cmsQuickLaunch.Visible OrElse cmsAlt.Visible Then
                                'Detach(False)
                            Else
                                'Attach(ap)
                                If My.Settings.HoverActivate Then
                                    Dim id = GetActiveProcessID()
                                    If id = scalaPID OrElse isAstoniaOrScalA(id) Then
                                        If Not (SysMenu.Visible OrElse cboAlt.DroppedDown OrElse cmbResolution.DroppedDown OrElse
                                                FrmSettings.Visible OrElse UpdateDialog.Contains(MousePosition) OrElse
                                                renameOpen OrElse CustomMessageBox.visible) Then
#If DEBUG Then
                                            If Not chkDebug.ContextMenuStrip.Visible Then
#End If
                                                'ap.Activate() doesn't work if not debugging
                                                If Not ap.IsActive AndAlso WindowFromPoint(MousePosition) = ap.MainWindowHandle Then
                                                    dBug.Print($"Activating {ap.Name}")
                                                    SendMouseInput(MouseEventF.XDown Or MouseEventF.XUp, 2)
                                                End If
#If DEBUG Then
                                            End If
#End If
                                        End If
                                    End If
                                End If
                            End If

                            ap.SavePos(rcwB.Location, False)

                            eqLockShown = True
                            Dim excludGearLock As Integer = If(AltPP?.isSDL, 18, 0)
                            Dim lockHeight = 45
                            If rccB.Height >= 2000 Then
                                lockHeight += 120
                            ElseIf rccB.Height >= 1500 Then
                                lockHeight += 80
                            ElseIf rccB.Height >= 1000 Then
                                lockHeight += 40
                            End If
                            PnlEqLock.Location = but.ThumbRECT.Location + New Point((rccB.Width \ 2 - 262.Map(0, 400, 0, rccB.Width / 2)).Map(0, rccB.Width, 0, but.ThumbRECT.Width - but.ThumbRECT.Left), 0)
                            PnlEqLock.Size = New Size((524 - excludGearLock).Map(0, 800, 0, but.ThumbRECT.Width - but.ThumbRECT.Left),
                                  lockHeight.Map(0, rccB.Height, 0, but.ThumbRECT.Height - but.ThumbRECT.Top))

                            Dim pttB As New Point
                            ClientToScreen(ap?.MainWindowHandle, pttB)
                            Dim AstClientOffsetB = New Size(pttB.X - rcwB.Left, pttB.Y - rcwB.Top)

                            Dim ptZB = Me.PointToScreen(but.ThumbRECT.Location)
                            Dim newXB = MousePosition.X.Map(ptZB.X, ptZB.X + but.ThumbRectangle.Width, ptZB.X, ptZB.X + but.ThumbRECT.Width - but.ThumbRECT.X - rccB.Width) - AstClientOffsetB.Width '- My.Settings.offset.X
                            Dim newYB = MousePosition.Y.Map(ptZB.Y, ptZB.Y + but.ThumbRectangle.Height, ptZB.Y, ptZB.Y + but.ThumbRECT.Height - but.ThumbRECT.Top - rccB.Height) - AstClientOffsetB.Height '- My.Settings.offset.Y

                            AOBusy = True
                            Dim unused = Task.Run(Sub()
                                                      Try
                                                          Dim ci As New CURSORINFO With {.cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(CURSORINFO))}
                                                          GetCursorInfo(ci)
                                                          If ci.flags = 0 Then Exit Sub
                                                          AOBusy = True
                                                          Dim flags = swpFlags
                                                          If Not but.AP.IsActive() Then flags.SetFlag(SetWindowPosFlags.DoNotChangeOwnerZOrder)
                                                          'If but.Tag?.IsBelow(ScalaHandle) Then flags = flags Or SetWindowPosFlags.IgnoreZOrder
                                                          Dim pt As Point = MousePosition - New Point(newXB + ap.ClientOffset.X, newYB + ap.ClientOffset.Y)
                                                          Dim wparam = WM_MOUSEMOVE_CreateWParam()
                                                          Dim lparam = New LParamMap(pt)
                                                          If prevWMMMpt <> MousePosition Then
                                                              SendMessage(but.AP.MainWindowHandle, WM_MOUSEMOVE, wparam, lparam) 'update client internal mousepos
                                                          End If
                                                          SetWindowPos(but.AP.MainWindowHandle, ScalaHandle, newXB, newYB, -1, -1, flags)
                                                          Attach(but.AP)
                                                          If prevWMMMpt <> MousePosition Then
                                                              SendMessage(but.AP.MainWindowHandle, WM_MOUSEMOVE, wparam, lparam) 'update client internal mousepos
                                                          End If
                                                          prevWMMMpt = MousePosition
                                                      Catch ex As Exception
                                                          dBug.Print(ex.Message)
                                                      Finally
                                                          AOBusy = False
                                                      End Try
                                                  End Sub)
                        End If
                    End If
                End If
            End If

            If Not thumbContainedMouse AndAlso My.Settings.gameOnOverview Then
                eqLockShown = False
                AltPP = Nothing
                Dim active = GetForegroundWindow()
                'Dim activePP = alts.FirstOrDefault(Function(ap) ap.MainWindowHandle = active)
                Dim activePP = alts.Find(Function(ap) ap.MainWindowHandle = active)

                If activePP IsNot Nothing AndAlso Not activePP.IsBelow(ScalaHandle) Then
                    Attach(activePP)
                    SetWindowPos(active, ScalaHandle, -1, -1, -1, -1, SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate)
                    SetWindowPos(FrmBehind.Handle, active, -1, -1, -1, -1, SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate)

                    'Detach(False)
                End If
            End If
        Catch ex As Exception
            Detach(False)
        End Try


        If eqLockShown AndAlso My.Settings.LockEq AndAlso My.Settings.gameOnOverview AndAlso alts.Any Then
            AOshowEqLock = True
        Else
            AOshowEqLock = False
        End If

        ' Dim purgeList As List(Of Integer) = startThumbsDict.Keys.Except(alts.Select(Function(ap) ap.Id)).ToList
        Dim purgelist As List(Of Integer) = startThumbsDict.Keys.ToList
        If pnlOverview.Visible Then
            'Dim altsIDs = alts.ConvertAll(Function(ap) ap.Id).ToHashSet
            'purgelist.RemoveAll(Function(x) altsIDs.Contains(x))
            Dim altsIDs = alts.ConvertAll(Function(ap) ap.Id)
            purgelist = purgelist.Except(altsIDs).ToList
        Else
            If AltPP IsNot Nothing Then purgelist.Remove(If(AltPP?.Id, 0))
        End If

        For Each ppid As Integer In purgelist 'tolist needed as we mutate the thumbsdict
            dBug.Print("unregister thumb " & startThumbsDict(ppid).ToString)
            DwmUnregisterThumbnail(startThumbsDict(ppid))
            startThumbsDict.TryRemove(ppid, Nothing)
            rectDic.TryRemove(ppid, Nothing)
            Dim sw As Stopwatch = Nothing
            If swDict.TryRemove(ppid, sw) Then
                sw.Stop()
                sw = Nothing
            End If
        Next

        altSelectedOrOverview = alts
#If DEBUG Then
        TickCounter += 1
        If TickCounter >= visibleButtons.Count Then TickCounter = 0
#End If
        'End SyncLock
        ovBusy = False
    End Sub

    ''' <summary>
    ''' Checks if process with id is Astonia or ScalA
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Private Function isAstoniaOrScalA(id As Integer)
        If id = 0 Then Return False
        Using proc As Process = Process.GetProcessById(id)
            Try
                If (proc.IsAstonia AndAlso Not proc.HasExited) OrElse proc.IsScalA Then Return True
            Catch
                CType(proc, AstoniaProcess).IsRunning() 'elevate self
            End Try
        End Using
        Return False
    End Function


    Private activeID As Integer = 0
    Private activeIsAstonia As Boolean = False
    Private swAutoClose As Stopwatch = Stopwatch.StartNew
    Private AutoCloseCounter As Integer = 0
    Public PrevMouseAlt As AstoniaProcess
    Private Async Sub TmrActive_Tick(sender As Timer, e As EventArgs) Handles tmrActive.Tick

        If MouseButtonStale <> MouseButtons AndAlso MouseButtons <> MouseButtons.None Then
            MouseButtonStale = MouseButtons
        End If

        'If MouseButtons.HasFlag(MouseButtons.Left) AndAlso PnlEqLock.Contains(MousePosition) AndAlso Not (cmsQuickLaunch.Visible OrElse cmsAlt.Visible) Then
        '    Dim wp = WindowFromPoint(MousePosition)
        '    If wp = AltPP?.MainWindowHandle OrElse wp = ScalaHandle Then
        '        Try
        '            AppActivate(scalaPID)
        '            PnlEqLock.Capture = True
        '            AppActivate(AltPP.Id)
        '        Catch ex As Exception

        '        End Try
        '    End If
        'End If





        activeID = GetActiveProcessID() ' this returns 0 when switching tasks
        Try
            activeIsAstonia = Process.GetProcessById(activeID).IsAstonia
        Catch
            activeIsAstonia = False
        End Try
        If activeID = scalaPID OrElse activeID = AltPP?.Id OrElse
                (My.Settings.gameOnOverview AndAlso pnlOverview.Visible AndAlso
                pnlOverview.Controls.OfType(Of AButton).Any(Function(ab) ab.Visible AndAlso ab.AP IsNot Nothing AndAlso ab.AP.Id = activeID)) Then ' is on overview
            setActive(True)
            If Not MouseButtons.HasFlag(MouseButtons.Middle) AndAlso Not MouseButtons.HasFlag(MouseButtons.Right) Then AltPP?.ThreadInput(False)
        ElseIf activeID <> 0 Then 'inactive
            setActive(False)
            AltPP?.ThreadInput(True) 'fix bringtofront bug
        End If

        If (MouseButtons.HasFlag(MouseButtons.Right) OrElse MouseButtons.HasFlag(MouseButtons.Middle)) AndAlso
           AltPP?.IsActive() AndAlso
           WindowFromPoint(MousePosition) = AltPP?.MainWindowHandle Then
            PrevMouseAlt = AltPP
            'Debug.Print($"{PrevMouseAlt?.Name}")
            If Not (My.Computer.Keyboard.AltKeyDown OrElse My.Computer.Keyboard.ShiftKeyDown OrElse My.Computer.Keyboard.CtrlKeyDown) Then
                AltPP?.ThreadInput(True)
            End If
        Else
            If PrevMouseAlt IsNot Nothing AndAlso MouseButtons = MouseButtons.None Then
                If WindowFromPoint(MousePosition) <> PrevMouseAlt?.MainWindowHandle Then
                    UntrapMouse(MouseButtonStale)
                End If
                MouseButtonStale = MouseButtons.None
                PrevMouseAlt = Nothing
            End If
        End If

        If AltPP IsNot Nothing AndAlso AltPP.isSDL() AndAlso AltPP.IsActive AndAlso MouseButtons.HasFlag(MouseButtons.Right) Then
            Dim mp = MousePosition
            Dim rect = Me.RectangleToScreen(New Rectangle(0, 0, Me.Width, pnlTitleBar.Height))
            If rect.Contains(mp) Then
                Caption_MouseMove(Nothing, New MouseEventArgs(MouseButtons.Right, 1, mp.X, mp.Y, 0))
            End If
        End If

        If activeIsAstonia AndAlso Not My.Computer.Keyboard.CtrlKeyDown Then
            CloseOtherDropDowns(cmsQuickLaunch.Items, Nothing)
            cmsQuickLaunch.Close()
        End If

        If SidebarScalA IsNot Nothing AndAlso SidebarScalA.HasExitedSafe Then
            SidebarScalA = Nothing
        End If

        Dim addID As Integer = IPC.AddToWhitelistOrRemoveFromBL()
        If addID <> 0 Then
            Dim addAP = CType(Process.GetProcessById(addID), AstoniaProcess)
            Dim nam As String = addAP.Name

            If FrmSettings.Visible Then
                FrmSettings.tbcSettings.SelectedTab = FrmSettings.tabSortAndBL
                If FrmSettings.txtTopSort.Lines.Contains(nam) AndAlso FrmSettings.txtBotSort.Lines.Contains(nam) Then
                    FrmSettings.txtTopSort.Lines = FrmSettings.txtTopSort.Lines.Except({nam}).ToArray
                    FrmSettings.txtBotSort.Lines = FrmSettings.txtBotSort.Lines.Except({nam}).ToArray
                End If
                If FrmSettings.chkWhitelist.Checked Then
                    If Not FrmSettings.txtTopSort.Lines.Contains(nam) Then
                        Dim lins = FrmSettings.txtTopSort.Lines.ToList
                        lins.Add(nam)
                        FrmSettings.txtTopSort.Lines = lins.ToArray
                    End If
                    FrmSettings.txtTopSort.Select(FrmSettings.txtTopSort.Text.IndexOf(nam), nam.Length)
                    FrmSettings.txtTopSort.Focus()
                    FrmSettings.Focus()
                End If
            End If

            If blackList.Contains(nam) Then
                blackList.RemoveAll(Function(it) it = nam)
            End If
            If My.Settings.Whitelist AndAlso Not topSortList.Concat(botSortList).Contains(nam) Then
                topSortList.Add(nam)
            End If

            My.Settings.topSort = String.Join(vbCrLf, blackList.Concat(topSortList))
            My.Settings.botSort = String.Join(vbCrLf, blackList.Concat(botSortList))

            apSorter = New AstoniaProcessSorter(topSortList, botSortList)

            IPC.AddToWhitelistOrRemoveFromBL(scalaPID, 0)
            IPC.RequestActivation = True
        End If

        'Dim selInfo = IPC.ReadSelectAlt()
        'If selInfo.Item1 <> 0 Then
        '    Dim selAP As AstoniaProcess = New AstoniaProcess(Process.GetProcessById(selInfo.Item1))
        '    PopDropDown(cboAlt)
        '    AltPP.RestoreSinglePos(selInfo.Item2)
        '    selAP.CenterBehind(pbZoom, 0, True, True)
        '    cboAlt.SelectedItem = selAP
        '    IPC.SelectAlt(scalaPID, 0)
        '    IPC.RequestActivation = True
        'End If

        If IPC.RequestActivation Then
            IPC.RequestActivation = False
            dBug.print("IPC.requestActivation")

            If AltPP?.IsMinimized Then
                AltPP.Restore()
            End If

            If Me.WindowState = FormWindowState.Minimized Then
                Attach(AltPP)
                SendMessage(FrmSizeBorder.Handle, WM_SYSCOMMAND, SC_RESTORE, IntPtr.Zero)
                SendMessage(FrmBehind.Handle, WM_SYSCOMMAND, SC_RESTORE, IntPtr.Zero)
                Me.WindowState = If(wasMaximized, FormWindowState.Maximized, FormWindowState.Normal)
            End If

            'ShowWindow(ScalaHandle, SW_SHOW)
            Me.Show()
            If My.Settings.SizingBorder AndAlso Not FrmSizeBorder.Visible Then
                frmOverlay.Show(Me)
                FrmSizeBorder.Show(frmOverlay)
            End If

            Me.TopMost = True
            Me.BringToFront()
            Await Task.Delay(100)
            Me.TopMost = My.Settings.topmost

            If Not FrmSettings.Visible Then
                If Not pnlOverview.Visible Then
                    AltPP?.CenterBehind(pbZoom)
                    Attach(AltPP, True)
                    dBug.print($"{moveBusy} {swpBusy}")
                    moveBusy = False
                Else
                    AppActivate(scalaPID)
                End If
            End If
        End If
        'Me.SuspendLayout()
        If Not (MouseButtons.HasFlag(MouseButtons.Right) OrElse MouseButtons.HasFlag(MouseButtons.Middle)) Then
            If cboAlt.SelectedIndex <> 0 OrElse My.Settings.gameOnOverview Then
                If My.Settings.LockEq AndAlso Not My.Computer.Keyboard.AltKeyDown AndAlso Not My.Computer.Keyboard.ShiftKeyDown Then

                    PnlEqLock.Visible = AOshowEqLock OrElse (Not pnlOverview.Visible)

                    If PnlEqLock.Visible AndAlso
                   Not (cmsQuickLaunch.Visible OrElse cmsAlt.Visible) AndAlso
                   Not (FrmSettings.cmsGenerate.Visible OrElse FrmSettings.cmsQLFolder.Visible) AndAlso
                   Not FrmSettings.Contains(MousePosition) AndAlso
                   PnlEqLock.Contains(MousePosition) AndAlso
                   Not cboAlt.DropDownContains(MousePosition) AndAlso
                   Not cmbResolution.DropDownContains(MousePosition) AndAlso
                   Not SysMenu.Contains(MousePosition) AndAlso
                   Not FrmSettings.SysMenu.Contains(MousePosition) Then
                        Cursor.Current = Cursors.No
                    ElseIf SysMenu?.Contains(MousePosition) OrElse FrmSettings.SysMenu?.Contains(MousePosition) Then
                        Cursor.Current = Cursors.Default
                    End If
                    ChkEqLock.CheckState = CheckState.Checked
                    ChkEqLock.Text = "🔒"
                Else
                    PnlEqLock.Visible = False
                    If My.Settings.LockEq Then
                        ChkEqLock.CheckState = CheckState.Indeterminate
                        ChkEqLock.Text = "🔓"
                    End If
                End If
            Else
                PnlEqLock.Visible = False
            End If
        Else
            PnlEqLock.Visible = False
        End If
        'Me.ResumeLayout()
        ''locked 🔒
        ''unlocked 🔓

        If Not pnlOverview.Visible AndAlso AltPP?.loggedInAs <> "Someone" AndAlso AltPP?.Name = "Someone" AndAlso Not AltPP?.hideRestart Then
            Dim sb As Rectangle = Me.Bounds
            'frmOverlay.Bounds = New Rectangle(sb.X, sb.Y + 21, sb.Width, sb.Height - 21)
            'SetWindowPos(frmOverlay.Handle, 0, sb.X, sb.Y + 21, sb.Width, sb.Height - 21, SetWindowPosFlags.DoNotActivate Or SetWindowPosFlags.DoNotChangeOwnerZOrder Or SetWindowPosFlags.DoNotSendChangingEvent)
            frmOverlay.pbRestart.Show()
        Else
            frmOverlay.pbRestart.Hide()
        End If

        'this does not belong in this hot path
        If My.Settings.AutoCloseIdle AndAlso swAutoClose.ElapsedMilliseconds > 1000 Then
            AutoCloseCounter += 1
            If AutoCloseCounter > 5 Then AutoCloseCounter = 0
            If AutoCloseCounter = 0 AndAlso cboAlt.SelectedIndex <> 0 Then
                'todo populate loggedins when an alt is selected

                'ERROR: this is not thread safe
                'AstoniaProcess.loggedIns = New Concurrent.ConcurrentDictionary(Of Integer, AstoniaProcess)(AstoniaProcess.Enumerate({}, True).Where(Function(ap) ap.Name <> "Someone").Select(Function(ap) New KeyValuePair(Of Integer, AstoniaProcess)(ap.Id, ap)))

                Dim dum = AstoniaProcess.Enumerate({}, True).Where(Function(p) p.Name <> "Someone") 'p.name populates loggedins

                'Dim dum As AstoniaProcess 'to ensure optimizer doesn't remove the following for each
                'For Each ap In AstoniaProcess.Enumerate({}, True).Where(Function(p) p.Name <> "Someone") ' this is handled in p.name: AstoniaProcess.loggedIns.TryAdd(ap.Id, ap)
                '    dum = ap
                'Next
            End If

            Dim listingsomeone = IPC.getInstances.Any(Function(si) si.showingSomeones)

            '  ls  ns | c
            '  0   0  | 1
            '  0   1  | 1
            '  1   0  | 1
            '  1   1  | 0

            If Not (listingsomeone AndAlso My.Settings.OnlyAutoCloseOnNoSomeone) Then
                Dim dumm = Task.Run(Sub()
                                        Parallel.ForEach(AstoniaProcess.loggedIns.Values.Where(Function(p) p.Name = "Someone").ToArray,
                                            Sub(it As AstoniaProcess)
                                                If it.hasLoggedIn Then
                                                    dBug.print($"AutoClosing {it.loggedInAs}")
                                                    it.CloseOrKill()
                                                    AstoniaProcess.loggedIns.TryRemove(it.Id, Nothing)
                                                End If
                                            End Sub)
                                    End Sub)
                'For Each ap In AstoniaProcess.loggedIns.Where(Function(p) p.Name = "Someone").ToArray
                '    'If ap.Name = "Bool" OrElse ap.Name = "Someone" Then dBug.print($"Autoclose {ap.Name} ""{ap.hasLoggedIn}""")
                '    If ap.hasLoggedIn Then
                '        ap.CloseOrKill()
                '        dBug.print($"autoclose {ap.loggedInAs}")
                '        AstoniaProcess.loggedIns = AstoniaProcess.loggedIns.FindAll(Function(pp) pp.Id <> ap.Id)
                '    End If
                'Next
            End If
            swAutoClose.Restart()
        End If

        Dim dummy = Task.Run(Sub() CloseErrorDialog())

        Dim behindHandle = FrmBehind.Handle

        Dim setbehind As IntPtr? = AltPP?.MainWindowHandle

        ' Dim setbehind? = FindLowestZOrderHwnd(altSelectedOrOverview, ScalaHandle)

        If (setbehind Is Nothing OrElse setbehind = IntPtr.Zero) Then setbehind = ScalaHandle

        SetWindowPos(behindHandle, setbehind, -1, -1, -1, -1,
                                        SetWindowPosFlags.IgnoreMove Or
                                        SetWindowPosFlags.DoNotActivate Or
                                        SetWindowPosFlags.IgnoreResize) ' Or SetWindowPosFlags.ASyncWindowPosition

    End Sub

    Private behindTaskDone As Boolean = True
    Private altSelectedOrOverview As List(Of AstoniaProcess)



    ' why did i write this? intended use? why is this in timers?
    Private Function FindLowestZOrderHwnd(ByVal tP As List(Of AstoniaProcess), ByVal startHwnd As IntPtr) As IntPtr?

        'todo replace with enumwindows


        If tP Is Nothing Then Return Nothing
        If tP.Count <= 1 Then Return tP.FirstOrDefault()?.MainWindowHandle

        Dim lowestHwnd As IntPtr? = Nothing
        Dim currentHwnd As IntPtr = IntPtr.Zero

        ' Build a HashSet of window handles (hwnds) for the target processes
        Dim hwnds As New HashSet(Of IntPtr)(tP.Select(Function(p) p.MainWindowHandle).Where(Function(h) h <> IntPtr.Zero))

        ' Traverse windows in Z-order (starting from the top)
        currentHwnd = GetTopWindow(IntPtr.Zero)
        ' Traverse windows in Z-order (starting from the specified hwnd)
        While currentHwnd <> IntPtr.Zero
            ' Only consider visible windows
            If IsWindowVisible(currentHwnd) Then
                ' Check if the current window handle is one of the hwnds we care about
                If hwnds.Contains(currentHwnd) Then
                    ' Keep track of the last (i.e., bottom-most) matching window
                    lowestHwnd = currentHwnd
                End If
            End If

            ' Get the next window in Z-order
            currentHwnd = GetWindow(currentHwnd, GW_HWNDNEXT)
        End While


        dBug.print($"{If(lowestHwnd, "none")}")

        Return lowestHwnd

    End Function
End Class
