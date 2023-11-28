﻿Public Class Timers
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
        'Debug.Print($"ws {Me.WindowState}")
        If Not AltPP?.IsRunning() Then
            Debug.Print($"Not AltPP?.IsRunning() {Me.WindowState}")
            Me.Show()
            FrmBehind.Show()
            If Not FrmSizeBorder.Visible Then FrmSizeBorder.Show(Me)
            If Not My.Settings.CycleOnClose Then
                SetWindowLong(ScalaHandle, GWL_HWNDPARENT, restoreParent)
                AllowSetForegroundWindow(scalaPID)
                Me.Activate()
                BringToFront()
                cboAlt.SelectedIndex = 0
                tmrOverview.Enabled = True
                tmrTick.Enabled = False
                FlashWindow(ScalaHandle, True) 'show on taskbar
                FlashWindow(ScalaHandle, False) 'stop blink
                '                Dim timeout As UInteger
                '                Dim ret = SystemParametersInfo(SPI.GETFOREGROUNDLOCKTIMEOUT, 0, timeout, 0)
                '                timeout = Math.Max(100, timeout + 1)
                '                Debug.Print($"timeout:{timeout} {ret}")
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
                Exit Sub
            Else 'CycleOnClose
                Cycle()
            End If
        End If

        If Not UpdateTitle() Then Exit Sub

        If Me.WindowState = FormWindowState.Minimized Then Exit Sub

        Dim pci As New CURSORINFO With {.cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(CURSORINFO))}
        GetCursorInfo(pci)
        If pci.flags <> 0 Then ' cursor is visible
            If Not wasVisible AndAlso AltPP?.IsActive() Then
                Debug.Print("scrollthumb released")
                If storedY <> pci.ptScreenpos.y OrElse storedX <> pci.ptScreenpos.x Then
                    Debug.Print("scrollthumb moved")
                    Dim Xfactor As Double = pbZoom.Width / rcC.Width
                    Dim Yfactor As Double = pbZoom.Height / rcC.Height
                    Dim movedX As Integer = storedX + ((pci.ptScreenpos.x - storedX) * Xfactor)
                    Dim movedY As Integer = storedY + ((pci.ptScreenpos.y - storedY) * Yfactor)
                    If movedY >= Me.Bottom Then movedY = Me.Bottom - 2
                    Cursor.Position = New Point(movedX, movedY)

                    Dim bzB As Rectangle = Me.RectangleToScreen(pbZoom.Bounds)
                    Dim ipt As New Point(movedX.Map(bzB.Left, bzB.Right, 0, rcC.Width),
                                         movedY.Map(bzB.Top, bzB.Bottom, 0, rcC.Height))
                    SendMessage(AltPP.MainWindowHandle, WM_MOUSEMOVE, WM_MM_GetWParam, New LParamMap(ipt)) 'update client internal mousepos
                    Debug.Print($"ipt {ipt}")
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

            Dim ptZ As Point = Me.PointToScreen(pbZoom.Location)

            newX = MousePosition.X.Map(ptZ.X, ptZ.X + pbZoom.Width, ptZ.X, ptZ.X + pbZoom.Width - rcC.Width) - AltPP.ClientOffset.X - My.Settings.offset.X
            newY = MousePosition.Y.Map(ptZ.Y, ptZ.Y + pbZoom.Height, ptZ.Y, ptZ.Y + pbZoom.Height - rcC.Height) - AltPP.ClientOffset.Y - My.Settings.offset.Y

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
                                 If Not AltPP.IsActive() Then flags.SetFlag(SetWindowPosFlags.DoNotChangeOwnerZOrder)
                                 If AltPP.IsBelow(ScalaHandle) Then flags.SetFlag(SetWindowPosFlags.IgnoreZOrder)
                                 Dim pt As Point = MousePosition - New Point(newX + AltPP.ClientOffset.X, newY + AltPP.ClientOffset.Y)
                                 Dim wparam = WM_MM_GetWParam()
                                 Dim lparam = New LParamMap(pt)
                                 If prevWMMMpt <> MousePosition Then
                                     SendMessage(AltPP.MainWindowHandle, WM_MOUSEMOVE, wparam, lparam) 'update client internal mousepos
                                 End If
                                 SetWindowPos(AltPP.MainWindowHandle, ScalaHandle, newX, newY, -1, -1, flags)
                                 If prevWMMMpt <> MousePosition Then
                                     SendMessage(AltPP.MainWindowHandle, WM_MOUSEMOVE, wparam, lparam) 'update client internal mousepos
                                 End If
                                 prevWMMMpt = MousePosition
                             Catch ex As Exception
                                 Debug.Print(ex.Message)
                             Finally
                                 swpBusy = False
                             End Try
                         End Sub)
            End If
        End If
    End Sub
    Dim prevWMMMpt As New Point

    Shared ReadOnly startThumbsDict As New Concurrent.ConcurrentDictionary(Of Integer, IntPtr)
    Shared ReadOnly opaDict As New Concurrent.ConcurrentDictionary(Of Integer, Byte)
    Shared ReadOnly rectDic As New Concurrent.ConcurrentDictionary(Of Integer, Rectangle)
    Shared ReadOnly swDict As New Concurrent.ConcurrentDictionary(Of Integer, Stopwatch)


    Private TickCounter As Integer = 0

    Friend Shared AOBusy As Boolean = False
    Private AOshowEqLock As Boolean = False

    Friend Shared apSorter As AstoniaProcessSorter
    Private ovBusy As Boolean = False
    Private Async Sub TimerOverview_Tick(sender As Timer, e As EventArgs) Handles tmrOverview.Tick

        If Me.WindowState = FormWindowState.Minimized Then Exit Sub

        If ovBusy Then Exit Sub
        ovBusy = True

#If DEBUG Then
        chkDebug.Text = TickCounter
#End If

        Dim alts As List(Of AstoniaProcess) = AstoniaProcess.Enumerate(blackList, True).OrderBy(Function(ap) ap.Name, apSorter).ToList

        Dim visibleButtons As List(Of AButton) = UpdateButtonLayout(alts.Count)

        Dim botCount = alts.Where(Function(ap) botSortList.Contains(ap.Name)).Count()
        Dim topCount = alts.Count - botCount
        Dim skipCount = visibleButtons.Count - botCount

        'Dim apCounter = 0
        'Dim butCounter = 0
        Dim eqLockShown = False


        Await Task.Run(Sub() Parallel.ForEach(visibleButtons, New ParallelOptions With {.MaxDegreeOfParallelism = -1},
                         Sub(but As AButton, ls As ParallelLoopState, butCounter As Integer)
                             Try
                                 Dim apCounter = butCounter
                                 If apCounter >= topCount Then apCounter = butCounter - skipCount + topCount
                                 'Debug.Print($"pfe tick{TickCounter} but{butCounter} ap{apCounter} bot{botCount} top{topCount} skip{skipCount}")
                                 If apCounter < alts.Count AndAlso
                                    (butCounter < topCount OrElse butCounter >= skipCount) AndAlso
                                    Not alts(apCounter).HasExited Then
                                     'buttons with alts

                                     Dim ap As AstoniaProcess = alts(apCounter)
                                     Dim apID = ap.Id
                                     but.AP = ap
                                     but.BeginInvoke(Sub() but.Text = ap.Name)

                                     If ap.IsActive() Then
                                         but.Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Bold)
                                         but.BeginInvoke(Sub() but.Select())
                                     Else
                                         but.Font = New Font("Microsoft Sans Serif", 8.25)
                                     End If

                                     If but.pidCache <> ap.Id Then but.BackgroundImage = Nothing
                                     If but.BackgroundImage Is Nothing Then
                                         Using ico As Bitmap = ap.GetIcon?.ToBitmap
                                             If ico IsNot Nothing Then
                                                 'but.Invoke(updateButtonBackgroundImage, {but, New Bitmap(ico, New Size(16, 16))})
                                                 but.BackgroundImage = New Bitmap(ico, New Size(16, 16))
                                                 Debug.Print($"{ap.Name} icon updated")
                                             Else
                                                 'but.Invoke(updateButtonBackgroundImage, {but, Nothing})
                                                 but.BackgroundImage = Nothing
                                             End If
                                             but.pidCache = ap.Id
                                         End Using
                                     End If

                                     Dim sw = swDict.GetOrAdd(ap.Id, Stopwatch.StartNew)
                                     If but.Image Is Nothing OrElse sw.ElapsedMilliseconds > 42 Then
                                         but.Image = ap.GetHealthbar
                                         sw.Restart()
                                     End If

                                     but.ContextMenuStrip = cmsAlt

                                     If Me.Invoke(Function() cboAlt.SelectedIndex = 0) Then
                                         If Not startThumbsDict.ContainsKey(apID) Then

                                             Dim thumbid As IntPtr = IntPtr.Zero
                                             DwmRegisterThumbnail(ScalaHandle, ap.MainWindowHandle, thumbid)
                                             startThumbsDict(apID) = thumbid
                                             Debug.Print($"registered thumb {startThumbsDict(apID)} {ap.Name} {apID}")
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
                         End Sub))

        Dim thumbContainedMouse As Boolean = False
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
                        Debug.Print("scrollthumb released")
                        If storedY <> pci.ptScreenpos.y OrElse storedX <> pci.ptScreenpos.x Then
                            Debug.Print("scrollthumb moved")
                            Dim Xfactor As Double = but.ThumbRectangle.Width / ap.ClientRect.Width
                            Dim Yfactor As Double = but.ThumbRectangle.Height / ap.ClientRect.Height
                            Dim movedX As Integer = storedX + ((pci.ptScreenpos.x - storedX) * Xfactor)
                            Dim movedY As Integer = storedY + ((pci.ptScreenpos.y - storedY) * Yfactor)
                            Cursor.Position = New Point(movedX, movedY)

                            Dim bzB As Rectangle = but.RectangleToScreen(but.ThumbRectangle)
                            Dim ipt As New Point(movedX.Map(bzB.Left, bzB.Right, 0, rccB.Width),
                                                 movedY.Map(bzB.Top, bzB.Bottom, 0, rccB.Height))
                            SendMessage(AltPP.MainWindowHandle, WM_MOUSEMOVE, WM_MM_GetWParam, New LParamMap(ipt)) 'update client internal mousepos
                            Debug.Print($"ipt {ipt}")
                        End If
                    End If
                    storedX = pci.ptScreenpos.x
                    storedY = pci.ptScreenpos.y
                    wasVisible = True
                End If

                If Not AOBusy Then
                    AltPP = ap
                    If ap.IsMinimized Then
                        Debug.Print($"before {rcwB} {rccB}")
                        ap.Restore()
                        rcwB = ap.WindowRect
                        rccB = ap.ClientRect
                        Debug.Print($"after {rcwB} {rccB}")
                    End If

                    If pci.flags = 0 Then ' cursor is hidden do not move astonia. fixes scrollbar thumb.
                        wasVisible = False ' note there is a client bug where using thumb will intermittently cause it to jump down wildly
                    Else

                        If cmsQuickLaunch.Visible OrElse cmsAlt.Visible Then
                            SetWindowLong(ScalaHandle, GWL_HWNDPARENT, restoreParent)
                        Else
                            SetWindowLong(ScalaHandle, GWL_HWNDPARENT, ap.MainWindowHandle)
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
                        Dim newXB = MousePosition.X.Map(ptZB.X, ptZB.X + but.ThumbRectangle.Width, ptZB.X, ptZB.X + but.ThumbRECT.Width - but.ThumbRECT.X - rccB.Width) - AstClientOffsetB.Width - My.Settings.offset.X
                        Dim newYB = MousePosition.Y.Map(ptZB.Y, ptZB.Y + but.ThumbRectangle.Height, ptZB.Y, ptZB.Y + but.ThumbRECT.Height - but.ThumbRECT.Top - rccB.Height) - AstClientOffsetB.Height - My.Settings.offset.Y

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
                                                      Dim wparam = WM_MM_GetWParam()
                                                      Dim lparam = New LParamMap(pt)
                                                      If prevWMMMpt <> MousePosition Then
                                                          SendMessage(but.AP.MainWindowHandle, WM_MOUSEMOVE, wparam, lparam) 'update client internal mousepos
                                                      End If
                                                      SetWindowPos(but.AP.MainWindowHandle, ScalaHandle, newXB, newYB, -1, -1, flags)
                                                      If prevWMMMpt <> MousePosition Then
                                                          SendMessage(but.AP.MainWindowHandle, WM_MOUSEMOVE, wparam, lparam) 'update client internal mousepos
                                                      End If
                                                      prevWMMMpt = MousePosition
                                                  Catch ex As Exception
                                                      Debug.Print(ex.Message)
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
            Dim active = GetForegroundWindow()
            'Dim activePP = alts.FirstOrDefault(Function(ap) ap.MainWindowHandle = active)
            Dim activePP = alts.Find(Function(ap) ap.MainWindowHandle = active)

            If activePP IsNot Nothing AndAlso Not activePP.IsBelow(ScalaHandle) Then
                SetWindowLong(ScalaHandle, GWL_HWNDPARENT, active)
                SetWindowPos(active, ScalaHandle, -1, -1, -1, -1, SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate)
                SetWindowLong(ScalaHandle, GWL_HWNDPARENT, restoreParent)
            End If
        End If


        If eqLockShown AndAlso My.Settings.LockEq AndAlso My.Settings.gameOnOverview Then
            AOshowEqLock = True
        Else
            AOshowEqLock = False
        End If

        ' Dim purgeList As List(Of Integer) = startThumbsDict.Keys.Except(alts.Select(Function(ap) ap.Id)).ToList
        Dim purgelist As List(Of Integer) = startThumbsDict.Keys.ToList
        If cboAlt.SelectedIndex = 0 Then
            purgelist = purgelist.Except(alts.Select(Function(ap) ap.Id)).ToList
        Else
            purgelist = purgelist.Except({AltPP.Id}).ToList
        End If

        For Each ppid As Integer In purgelist 'tolist needed as we mutate the thumbsdict
            Debug.Print("unregister thumb " & startThumbsDict(ppid).ToString)
            DwmUnregisterThumbnail(startThumbsDict(ppid))
            startThumbsDict.TryRemove(ppid, Nothing)
            rectDic.TryRemove(ppid, Nothing)
            Dim sw As Stopwatch = Nothing
            If swDict.TryRemove(ppid, sw) Then
                sw.Stop()
                sw = Nothing
            End If
        Next

        TickCounter += 1
        If TickCounter >= visibleButtons.Count Then TickCounter = 0
        ovBusy = False
    End Sub
#Region "TmrOverview_Tick_Old"
#If 0 Then
    Private Sub TmrOverview_Tick_Old(sender As Timer, e As EventArgs)

        If Me.WindowState = FormWindowState.Minimized Then
            Exit Sub
        End If

#If DEBUG Then
        chkDebug.Text = TickCounter
#End If

        Dim alts As List(Of AstoniaProcess) = AstoniaProcess.Enumerate(blackList, True).OrderBy(Function(ap) ap.Name, apSorter).ToList

        pnlOverview.SuspendLayout()
        Dim visibleButtons As List(Of AButton) = UpdateButtonLayout(alts.Count)

        Dim botCount = alts.Where(Function(ap) botSortList.Contains(ap.Name)).Count()
        Dim topCount = alts.Count - botCount
        Dim skipCount = visibleButtons.Count - botCount

        Dim apCounter = 0
        Dim butCounter = 0
        Dim eqLockShown = False
        Dim thumbContainedMouse As Boolean = False

        For Each but As AButton In visibleButtons
            butCounter += 1
            'Debug.Print($"apCount < alts.Count AndAlso (i < topCount OrElse i > skipCount")
            'Debug.Print($"{apCount} < {alts.Count} AndAlso ({i} < {topCount} OrElse {i} > {skipCount}")
            If apCounter < alts.Count AndAlso (butCounter <= topCount OrElse butCounter > skipCount) Then 'buttons with alts

                Dim ap As AstoniaProcess = alts(apCounter)
                Dim apID As Integer = ap.Id
                but.AP = ap
                but.Text = ap.Name

                If ap.IsActive() Then
                    but.Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Bold)
                    but.Select()
                Else
                    but.Font = New Font("Microsoft Sans Serif", 8.25)
                End If
                'Debug.Print($"tick {TickCounter} apc {apCounter} {ap.Name}")
                If (TickCounter = apCounter OrElse but.BackgroundImage Is Nothing OrElse but.Image Is Nothing) Then
                    Dim localAPC = apCounter
                    Dim localTick = TickCounter
                    Task.Run(Sub()
                                 If (localTick = localAPC AndAlso ap.Id <> but.pidCache) OrElse but.BackgroundImage Is Nothing Then
                                     Using ico As Bitmap = ap.GetIcon?.ToBitmap
                                         If ico IsNot Nothing Then
                                             'but.Invoke(updateButtonBackgroundImage, {but, New Bitmap(ico, New Size(16, 16))})
                                             but.BackgroundImage = New Bitmap(ico, New Size(16, 16))
                                         Else
                                             'but.Invoke(updateButtonBackgroundImage, {but, Nothing})
                                             but.BackgroundImage = Nothing
                                         End If
                                         but.pidCache = ap.Id
                                     End Using
                                 End If

                                 Dim sw = swDict.GetOrAdd(ap.Id, Stopwatch.StartNew)
                                 If sw.ElapsedMilliseconds < 42 AndAlso but.Image IsNot Nothing Then
                                     Exit Sub
                                 End If

                                 If (localTick = localAPC) OrElse but.Image Is Nothing Then
                                     'Me.Invoke(updateButtonImage, {but, ap.GetHealthbar()})
                                     but.Image = ap.GetHealthbar
                                 End If
                                 sw.Restart()
                             End Sub)
                End If

                apCounter += 1

                but.ContextMenuStrip = cmsAlt

                If Not startThumbsDict.ContainsKey(apID) Then
                    Dim thumbid As IntPtr = IntPtr.Zero
                    DwmRegisterThumbnail(ScalaHandle, ap.MainWindowHandle, thumbid)
                    startThumbsDict(apID) = thumbid
                    Debug.Print($"registered thumb {startThumbsDict(apID)} {ap.Name} {apID}")
                End If

                rectDic(apID) = but.ThumbRECT




                'Dim ACO = New Size(pttB.X - rcwB.Left, pttB.Y - rcwB.Top)

                ' Debug.Print($"ACO {ap.Name}:{ACO}")

                Dim prp As New DWM_THUMBNAIL_PROPERTIES With {
                                   .dwFlags = DwmThumbnailFlags.DWM_TNP_OPACITY Or DwmThumbnailFlags.DWM_TNP_VISIBLE Or DwmThumbnailFlags.DWM_TNP_RECTDESTINATION Or DwmThumbnailFlags.DWM_TNP_SOURCECLIENTAREAONLY,
                                   .opacity = opaDict.GetValueOrDefault(apID, If(chkDebug.Checked, 128, 255)),
                                   .fVisible = True,
                                   .rcDestination = rectDic(apID),
                                   .fSourceClientAreaOnly = True}

                DwmUpdateThumbnailProperties(startThumbsDict(apID), prp)

                If My.Settings.gameOnOverview Then 'todo move this to seperate timer and make async

                    'InvalidateRect(ap.MainWindowHandle, IntPtr.Zero, False)
                    'SendMessage(ap.MainWindowHandle, WM_PAINT, IntPtr.Zero, IntPtr.Zero)
                    'RedrawWindow(ap.MainWindowHandle, Nothing, Nothing, RedrawWindowFlags.Invalidate Or RedrawWindowFlags.InternalPaint)


                    Dim pci As New CURSORINFO With {.cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(CURSORINFO))}
                    GetCursorInfo(pci)
                    If pci.flags <> 0 Then ' cursor is visible
                        If Not wasVisible AndAlso ap.IsActive() Then
                            Debug.Print("scrollthumb released")
                            If storedY <> pci.ptScreenpos.y Then
                                Debug.Print("scrollthumb moved")
                                Dim factor As Double = but.ThumbRectangle.Height / ap.ClientRect.Height
                                Dim movedY As Integer = storedY + ((pci.ptScreenpos.y - storedY) * factor)
                                Cursor.Position = New Point(pci.ptScreenpos.x, movedY)
                            End If
                        End If
                        storedY = pci.ptScreenpos.y
                        wasVisible = True
                    End If

                    If Not AOBusy AndAlso but.ThumbContains(MousePosition) Then
                        AltPP = ap
                        Dim rcwB As Rectangle = ap.WindowRect
                        Dim rccB As Rectangle = ap.ClientRect
                        If ap.IsMinimized Then
                            Debug.Print($"before {rcwB} {rccB}")
                            ap.Restore()
                            rcwB = ap.WindowRect
                            rccB = ap.ClientRect
                            Debug.Print($"after {rcwB} {rccB}")
                        End If

                        If pci.flags = 0 Then ' cursor is hidden
                            wasVisible = False
                            Exit For ' do not move astonia when cursor is hidden. fixes scrollbar thumb.
                            ' note there is a client bug where using thumb will intermittently cause it to jump down wildly
                        End If

                        thumbContainedMouse = True

                        If cmsQuickLaunch.Visible OrElse cmsAlt.Visible Then
                            SetWindowLong(ScalaHandle, GWL_HWNDPARENT, restoreParent)
                        Else
                            SetWindowLong(ScalaHandle, GWL_HWNDPARENT, ap.MainWindowHandle)
                        End If

                        'Dim rcwB As Rectangle
                        'Dim pttB As Point

                        'GetWindowRect(ap.MainWindowHandle, rcwB)
                        'ClientToScreen(ap.MainWindowHandle, pttB)

                        ap.SavePos(rcwB.Location, False)

                        eqLockShown = True
                        Dim excludGearLock As Integer = If(AltPP?.isSDL, 18, 0)
                        PnlEqLock.Location = but.ThumbRECT.Location + New Point((rccB.Width \ 2 - 262.Map(0, 400, 0, rccB.Width / 2)).Map(0, rccB.Width, 0, but.ThumbRECT.Width - but.ThumbRECT.Left), 0)
                        PnlEqLock.Size = New Size((524 - excludGearLock).Map(0, 800, 0, but.ThumbRECT.Width - but.ThumbRECT.Left),
                                                   45.Map(0, 600, 0, but.ThumbRECT.Height - but.ThumbRECT.Top))

                        Dim pttB As New Point
                        ClientToScreen(ap?.MainWindowHandle, pttB)
                        Dim AstClientOffsetB = New Size(pttB.X - rcwB.Left, pttB.Y - rcwB.Top)

                        Dim ptZB = Me.PointToScreen(but.ThumbRECT.Location)
                        Dim newXB = MousePosition.X.Map(ptZB.X, ptZB.X + but.ThumbRectangle.Width, ptZB.X, ptZB.X + but.ThumbRECT.Width - but.ThumbRECT.X - rccB.Width) - AstClientOffsetB.Width - My.Settings.offset.X
                        Dim newYB = MousePosition.Y.Map(ptZB.Y, ptZB.Y + but.ThumbRectangle.Height, ptZB.Y, ptZB.Y + but.ThumbRECT.Height - but.ThumbRECT.Top - rccB.Height) - AstClientOffsetB.Height - My.Settings.offset.Y

                        AOBusy = True
                        Task.Run(Sub()
                                     Try
                                         Dim ci As New CURSORINFO With {.cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(CURSORINFO))}
                                         GetCursorInfo(ci)
                                         If ci.flags = 0 Then Exit Sub
                                         AOBusy = True
                                         Dim flags = swpFlags
                                         If Not but.AP.IsActive() Then flags = flags Or SetWindowPosFlags.DoNotChangeOwnerZOrder
                                         'If but.Tag?.IsBelow(ScalaHandle) Then flags = flags Or SetWindowPosFlags.IgnoreZOrder
                                         Dim pt As Point = MousePosition - New Point(newXB + ap.ClientOffset.X, newYB + ap.ClientOffset.Y)
                                         If prevWMMMpt <> MousePosition Then
                                             SendMessage(but.AP.MainWindowHandle, WM_MOUSEMOVE, Nothing, (pt.Y << 16) + pt.X) 'update client internal mousepos
                                         End If
                                         SetWindowPos(but.AP.MainWindowHandle, ScalaHandle, newXB, newYB, -1, -1, flags)
                                         If prevWMMMpt <> MousePosition Then
                                             SendMessage(but.AP.MainWindowHandle, WM_MOUSEMOVE, Nothing, (pt.Y << 16) + pt.X) 'update client internal mousepos
                                         End If
                                         prevWMMMpt = MousePosition
                                     Catch ex As Exception
                                         Debug.Print(ex.Message)
                                     Finally
                                         AOBusy = False
                                     End Try
                                 End Sub)
                    End If 'but.ThumbContains(MousePosition)
                End If 'gameonoverview
            Else ' buttons w/o alts
                but.Text = String.Empty
                but.AP = Nothing 'New AstoniaProcess(Nothing)
                but.ContextMenuStrip = cmsQuickLaunch
                but.BackgroundImage = Nothing
                but.Image = Nothing
                but.pidCache = 0
            End If
        Next but



        If Not thumbContainedMouse AndAlso My.Settings.gameOnOverview Then
            eqLockShown = False
            Dim active = GetForegroundWindow()
            Dim activePP = alts.FirstOrDefault(Function(ap) ap.MainWindowHandle = active)
            If activePP IsNot Nothing AndAlso Not activePP.IsBelow(ScalaHandle) Then
                SetWindowLong(ScalaHandle, GWL_HWNDPARENT, active)
                SetWindowPos(active, ScalaHandle, -1, -1, -1, -1, SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate)
                SetWindowLong(ScalaHandle, GWL_HWNDPARENT, restoreParent)
            End If
        End If


        If eqLockShown AndAlso My.Settings.LockEq AndAlso My.Settings.gameOnOverview Then
            AOshowEqLock = True
        Else
            AOshowEqLock = False
        End If

        ' Dim purgeList As List(Of Integer) = startThumbsDict.Keys.Except(alts.Select(Function(ap) ap.Id)).ToList
        For Each ppid As Integer In startThumbsDict.Keys.Except(alts.Select(Function(ap) ap.Id)).ToList 'tolist needed as we mutate the thumbsdict
            Debug.Print("unregister thumb " & startThumbsDict(ppid).ToString)
            DwmUnregisterThumbnail(startThumbsDict(ppid))
            startThumbsDict.TryRemove(ppid, Nothing)
            rectDic.TryRemove(ppid, Nothing)
            Dim sw As Stopwatch = Nothing
            If swDict.TryRemove(ppid, sw) Then
                sw.Stop()
                sw = Nothing
            End If
        Next

        pnlOverview.ResumeLayout()
        TickCounter += 1
        If TickCounter >= visibleButtons.Count Then TickCounter = 0
    End Sub
#End If
#End Region
    Private activeID As Integer = 0
    Private hasCName As Boolean = False
    Private Async Sub TmrActive_Tick(sender As Timer, e As EventArgs) Handles tmrActive.Tick

        activeID = GetActiveProcessID() ' this returns 0 when switching tasks
        Try
            hasCName = Process.GetProcessById(activeID).HasClassNameIn(My.Settings.className)
        Catch
            hasCName = False
        End Try
        If activeID = scalaPID OrElse activeID = AltPP?.Id OrElse
                (My.Settings.gameOnOverview AndAlso pnlOverview.Visible AndAlso
                pnlOverview.Controls.OfType(Of AButton).Any(Function(ab) ab.Visible AndAlso ab.AP IsNot Nothing AndAlso ab.AP.Id = activeID)) Then ' is on overview
            setActive(True)
        ElseIf activeID <> 0 Then 'inactive
            setActive(False)
        End If

        If IPC.RequestActivation Then
            IPC.RequestActivation = 0
            Debug.Print("IPC.requestActivation")

            If AltPP?.IsMinimized Then
                AltPP.Restore()
            End If

            If Me.WindowState = FormWindowState.Minimized Then
                If AltPP IsNot Nothing Then
                    SetWindowLong(ScalaHandle, GWL_HWNDPARENT, AltPP.MainWindowHandle)
                End If
                SendMessage(FrmSizeBorder.Handle, WM_SYSCOMMAND, SC_RESTORE, IntPtr.Zero)
                SendMessage(FrmBehind.Handle, WM_SYSCOMMAND, SC_RESTORE, IntPtr.Zero)
                Me.WindowState = If(wasMaximized, FormWindowState.Maximized, FormWindowState.Normal)
            End If

            'ShowWindow(ScalaHandle, SW_SHOW)

            Me.TopMost = True
            Me.BringToFront()
            Await Task.Delay(100)
            Me.TopMost = My.Settings.topmost

            If Not pnlOverview.Visible Then
                AltPP?.CenterBehind(pbZoom)
                AltPP?.Activate()
                Debug.Print($"{moveBusy} {swpBusy}")
                moveBusy = False
            Else
                AppActivate(scalaPID)
            End If

        End If
        'Me.SuspendLayout()
        If Not (MouseButtons.HasFlag(MouseButtons.Right) OrElse MouseButtons.HasFlag(MouseButtons.Middle)) Then
            If cboAlt.SelectedIndex <> 0 OrElse My.Settings.gameOnOverview Then
                If My.Settings.LockEq AndAlso Not My.Computer.Keyboard.AltKeyDown AndAlso Not My.Computer.Keyboard.ShiftKeyDown Then 'AndAlso Not EQLockClick Then
                    If Not (MouseButtons.HasFlag(MouseButtons.Right) OrElse MouseButtons.HasFlag(MouseButtons.Middle)) Then
                        If Not PnlEqLock.Visible Then
                            If Not pnlOverview.Visible AndAlso Not My.Settings.gameOnOverview Then
                                PnlEqLock.Visible = True
                            Else
                                PnlEqLock.Visible = AOshowEqLock OrElse Not pnlOverview.Visible
                            End If
                        End If
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
                        ElseIf SysMenu.Contains(MousePosition) OrElse FrmSettings.SysMenu.Contains(MousePosition) Then
                            Cursor.Current = Cursors.Default
                        End If
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

        Dim dummy = Task.Run(Sub() CloseErrorDialog())

        Dim setbehind As IntPtr = AltPP?.MainWindowHandle

        'If setbehind = IntPtr.Zero Then
        '    setbehind = If(pnlOverview.Visible, ScalaHandle, AltPP?.MainWindowHandle)
        'End If

        If setbehind = IntPtr.Zero AndAlso pnlOverview.Visible Then setbehind = ScalaHandle

        SetWindowPos(FrmBehind.Handle, setbehind, -1, -1, -1, -1,
                     SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.DoNotActivate Or SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.ASyncWindowPosition)

    End Sub
End Class
