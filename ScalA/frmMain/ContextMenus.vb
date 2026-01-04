Imports System.Collections.Concurrent
Imports System.Runtime.InteropServices
Imports System.Threading
Imports ScalA.QL

Public NotInheritable Class ContextMenus
    'dummy class to prevent form being generated
End Class
Partial Public NotInheritable Class FrmMain

#Region "cmsQuit"
    Private Sub CloseScalAToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles CloseScalAToolStripMenuItem.Click
        btnQuit.PerformClick()
    End Sub

    Private Sub CloseAstoniaToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles CloseAstoniaToolStripMenuItem.Click
        AltPP?.CloseOrKill()
    End Sub

    Private Async Sub CloseBothToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles CloseBothToolStripMenuItem.Click
        AltPP?.CloseOrKill()
        Await Task.Run(Sub()
                           While Not AltPP?.HasExited()
                               Threading.Thread.Sleep(50)
                           End While
                       End Sub)
        btnQuit.PerformClick()
    End Sub

    Private Async Sub CloseAllToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        tmrOverview.Stop()
        tmrTick.Stop()
        tmrActive.Stop()
        WindowState = FormWindowState.Minimized
        Dim procs = AstoniaProcess.EnumAll().ToList
        Parallel.ForEach(procs, Sub(ap) ap.CloseOrKill())

        Dim cts As New Threading.CancellationTokenSource()
        cts.CancelAfter(100 * procs.Count)
        Dim ct = cts.Token
        Await Task.Run(Sub()
                           While Not ct.IsCancellationRequested AndAlso
                                 procs.Any(Function(ap As AstoniaProcess) Not ap.HasExited)
                               CloseErrorDialog()
                               Threading.Thread.Sleep(34)
                               dBug.Print("busy error dialog closer")
                           End While
                       End Sub, ct)
        btnQuit.PerformClick()
    End Sub

    Private Sub CloseAllExceptToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles CloseAllExceptToolStripMenuItem.Click
        Parallel.ForEach(AstoniaProcess.ListProcesses(blackList, False).Where(Function(ap) ap.Id <> AltPP.Id), Sub(pp As AstoniaProcess)
                                                                                                                   pp.CloseOrKill()
                                                                                                               End Sub)
        AppActivate(scalaPID)
    End Sub

    Private Sub CloseAllButNameToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles CloseAllButNameToolStripMenuItem.Click
        Parallel.ForEach(AstoniaProcess.ListProcesses(blackList, False).Where(Function(ap) ap.Id <> sender.Tag.Id), Sub(pp As AstoniaProcess)
                                                                                                                        pp.CloseOrKill()
                                                                                                                    End Sub)
        AppActivate(scalaPID)
    End Sub

    Private Sub CloseAllOverviewToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles CloseAllOverviewToolStripMenuItem.Click
        Dim procs = pnlOverview.Controls().OfType(Of AButton).Select(Of AstoniaProcess)(Function(ab) ab.AP)
        Parallel.ForEach(procs, Sub(ap) ap?.CloseOrKill())
        AppActivate(scalaPID)
    End Sub

    Private Sub cmsQuit_Opening(sender As ContextMenuStrip, e As System.ComponentModel.CancelEventArgs) Handles cmsQuit.Opening
        sender.Opacity = 0
        If cboAlt.SelectedIndex = 0 Then
            CloseAstoniaToolStripMenuItem.Visible = False
            CloseBothToolStripMenuItem.Visible = False
            CloseAllOverviewToolStripMenuItem.Visible = pnlOverview.Controls.OfType(Of AButton).Any(Function(ab) ab.AP IsNot Nothing)
            CloseAllExceptToolStripMenuItem.Visible = False
        Else
            CloseAstoniaToolStripMenuItem.Text = $"Close {AltPP.UserName}"
            CloseBothToolStripMenuItem.Text = $"Close {AltPP.UserName} && ScalA"
            CloseAstoniaToolStripMenuItem.Visible = True
            CloseBothToolStripMenuItem.Visible = True
            CloseAllOverviewToolStripMenuItem.Visible = False
            CloseAllExceptToolStripMenuItem.Text = $"Close All but {AltPP.UserName}"
            Task.Run(Sub() Me.BeginInvoke(Sub() CloseAllExceptToolStripMenuItem.Visible = AstoniaProcess.ListProcesses(blackList, True).Any(Function(ap As AstoniaProcess) ap.Id <> AltPP.Id)))
        End If
        Task.Run(Sub()
                     Dim aps = AstoniaProcess.Enumerate(False).ToList
                     If aps.Count = 0 OrElse (aps.Count = 1 AndAlso DirectCast(Me.Invoke(Function() cboAlt.SelectedIndex > 0), Boolean)) Then
                         Me.BeginInvoke(Sub()
                                            CloseAllSeparator.Visible = False
                                            CloseAllToolStripMenuItem.Visible = False
                                        End Sub)
                     Else
                         Me.BeginInvoke(Sub()
                                            CloseAllSeparator.Visible = True
                                            CloseAllToolStripMenuItem.Visible = True
                                        End Sub)
                     End If
                 End Sub)

        Task.Run(Sub()
                     If AstoniaProcess.EnumSomeone.Any() Then
                         Me.BeginInvoke(Sub() CloseAllIdleToolStripMenuItem.Visible = True)
                     Else
                         Me.BeginInvoke(Sub() CloseAllIdleToolStripMenuItem.Visible = False)
                     End If
                 End Sub)

        ttMain.Hide(btnQuit)
        pbZoom.Visible = False
        AButton.ActiveOverview = False

    End Sub

    Private Sub cmsQuit_Closed(sender As Object, e As ToolStripDropDownClosedEventArgs) Handles cmsQuit.Closed
        Dim unused = RestoreClicking()
    End Sub
#End Region 'cmsQuit

    Private Sub CloseToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles CloseToolStripMenuItem.Click
        'PostMessage(CType(sender.Tag, AstoniaProcess).MainWindowHandle, &H100, Keys.F12, IntPtr.Zero)
        DirectCast(sender.Tag, AstoniaProcess)?.CloseOrKill()
    End Sub

    Private Sub SelectToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles SelectToolStripMenuItem.Click
        Dim pp As AstoniaProcess = DirectCast(sender.Tag, AstoniaProcess)
        If pp Is Nothing Then Exit Sub
        dBug.Print("SelectToolStrip: " & pp.UserName)
        SelectAlt(pp)
    End Sub

    Private Sub ReLaunchToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles ReLaunchToolStripMenuItem.Click
        Dim pp As AstoniaProcess = DirectCast(sender.Tag, AstoniaProcess)
        AltPP = pp
        AltPP.restart()
    End Sub


    Private Sub TopMostToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles TopMostToolStripMenuItem.Click
        Dim pp As AstoniaProcess = DirectCast(sender.Tag, AstoniaProcess)
        If pp Is Nothing Then Exit Sub
        dBug.Print("Topmost " & Not sender.Checked)
        If Not sender.Checked Then
            SetWindowPos(pp.MainWindowHandle, SWP_HWND.TOPMOST, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove)
        Else
            SetWindowPos(pp.MainWindowHandle, SWP_HWND.NOTOPMOST, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove)
        End If
    End Sub
    Private Shared closeAllIdleTSMI As ToolStripMenuItem = Nothing
    Private Sub CmsAlt_Opening(sender As ContextMenuStrip, e As System.ComponentModel.CancelEventArgs) Handles cmsAlt.Opening
        If My.Computer.Keyboard.ShiftKeyDown OrElse My.Computer.Keyboard.CtrlKeyDown Then
            cmsQuickLaunch.Show(sender.SourceControl, sender.SourceControl.PointToClient(MousePosition))
            e.Cancel = True
            Exit Sub
        End If

        If Application.OpenForms().OfType(Of FrmSettings).Any Then
            FrmSettings.BringToFront()
            FrmSettings.btnHelp.Select()
            e.Cancel = True
            Exit Sub
        End If
        'Detach(False)
        UntrapMouse(MouseButtons.Right)
        AppActivate(scalaPID) 'fix right click drag bug

        Dim pp As AstoniaProcess = DirectCast(sender.SourceControl, AButton).AP
        sender.Tag = pp

#If 0 Then
#If DEBUG Then
        Static DebugToolStripMenuItem As ToolStripMenuItem = Nothing
        If DebugToolStripMenuItem Is Nothing Then
            DebugToolStripMenuItem = New ToolStripMenuItem("Debug", Nothing, AddressOf dBug.DebugAlt)
            sender.Items.Insert(0, DebugToolStripMenuItem)
        End If
        DebugToolStripMenuItem.Tag = pp
#End If
#End If
        SelectToolStripMenuItem.Text = "Select " & pp?.UserName
        SelectToolStripMenuItem.Image = pp?.GetIcon?.ToBitmap
        SelectToolStripMenuItem.Tag = pp


        dBug.Print($"cmsAlt {pp?.Name} ""{pp?.loggedInAs}"" {pp?.UserName}")

        If pp?.Name = "Someone" Then
            ReLaunchToolStripMenuItem.Visible = True
            If String.IsNullOrWhiteSpace(pp?.loggedInAs) Then
                ReLaunchToolStripMenuItem.Text = $"ReLaunch {pp.UserName}"
            Else
                ReLaunchToolStripMenuItem.Text = $"ReLaunch {pp?.loggedInAs}"
            End If
        Else
            ReLaunchToolStripMenuItem.Visible = False
        End If
        ReLaunchToolStripMenuItem.Tag = pp

        TopMostToolStripMenuItem.Checked = pp?.TopMost()
        TopMostToolStripMenuItem.Tag = pp

        SortSubToolStripMenuItem.Tag = pp

        MoveToolStripMenuItem.Tag = pp

        ActiveOverviewToolStripMenuItem.Checked = My.Settings.gameOnOverview

        SidebarModeToolStripMenuItem.Checked = SidebarScalA IsNot Nothing

        If sender.Items.Contains(closeAllIdleTSMI) Then
            sender.Items.Remove(closeAllIdleTSMI)
        End If

        sender.Items.RemoveAt(sender.Items.Count - 1)
        sender.Items.Add("Close " & pp?.UserName, My.Resources.F12, AddressOf CloseToolStripMenuItem_Click).Tag = pp

        Dim other As String = If(pp?.Name = "Someone", "Other ", "")
        Dim somecount As Integer = AstoniaProcess.EnumSomeone.Count(Function(p) p.Name = "Someone")
        dBug.Print($"somecount {somecount}")
        If somecount > 0 AndAlso Not (other = "Other " AndAlso somecount = 1) Then
            closeAllIdleTSMI = sender.Items.Add($"Close All {other}Someone", My.Resources.moreF12, AddressOf CloseAllIdle_Click)
            closeAllIdleTSMI.Tag = pp
        End If

        CloseAllButNameToolStripMenuItem.Text = $"Close All but {pp?.UserName}"
        CloseAllButNameToolStripMenuItem.Tag = pp

    End Sub
    Private Sub MoveToolStripMenuItem_DropDownOpening(sender As ToolStripMenuItem, e As EventArgs) Handles MoveToolStripMenuItem.DropDownOpening
        Dim lst = EnumOtherOverviews.OrderBy(Function(p) p.ProcessName, NsSorter).ToList
        MoveToolStripMenuItem.DropDownItems.Clear()
        MoveToolStripMenuItem.DropDownItems.Add(NoOtherOverviewsToolStripMenuItem)
        NoOtherOverviewsToolStripMenuItem.Visible = lst.Count = 0

        MoveToolStripMenuItem.DropDownItems.Add(KeepToolStripMenuItem)
        MoveToolStripMenuItem.DropDownItems.Add(New ToolStripSeparator With {.Visible = lst.Count > 0})
        KeepToolStripMenuItem.Visible = lst.Count > 0
        KeepToolStripMenuItem.CheckState = If(My.Settings.KeepOnOverview, CheckState.Indeterminate, CheckState.Unchecked)

        dBug.Print(sender.Tag.name)
        Dim Rang = lst.Select(Function(p) New ToolStripMenuItem(p.ProcessName, Nothing, AddressOf MoveTo_Click) With {.Tag = New Tuple(Of AstoniaProcess, Process)(sender.Tag, p)}).ToArray
        MoveToolStripMenuItem.DropDownItems.AddRange(Rang)
    End Sub

    Private Sub KeepToolStripMenuItem_Mousedown(sender As ToolStripMenuItem, e As MouseEventArgs) Handles KeepToolStripMenuItem.MouseDown
        dBug.Print($"{sender.Owner}")
        CType(sender.Owner, ToolStripDropDownMenu).AutoClose = False 'this messes with topmost
        If e.Button = MouseButtons.Right Then
            If sender.CheckState <> CheckState.Indeterminate Then
                sender.CheckState = CheckState.Indeterminate
            Else
                sender.CheckState = CheckState.Unchecked
            End If
        End If
        If e.Button = MouseButtons.Left Then sender.Checked = Not sender.Checked

        My.Settings.KeepOnOverview = sender.CheckState = CheckState.Indeterminate

    End Sub
    Private Sub KeepToolStripMenuItem_Mouseleave(sender As ToolStripMenuItem, e As EventArgs) Handles KeepToolStripMenuItem.MouseLeave
        CType(sender.Owner, ToolStripDropDownMenu).AutoClose = True
    End Sub

    Private Sub MoveToolStripMenuItem_DropDownOpened(sender As ToolStripMenuItem, e As EventArgs) Handles MoveToolStripMenuItem.DropDownOpened
        Dim menuPos As Point = sender.DropDown.Bounds.Location
        For Each tsmi As ToolStripMenuItem In sender.DropDownItems.OfType(Of ToolStripMenuItem).Where(Function(mi) mi.Tag IsNot Nothing)
            Dim hndl As IntPtr = DirectCast(tsmi.Tag.item2, Process).GetWindowHandle 'mainwindowhandle reports null for attached ScalAs. replaced with a fields in IPC
            Dim rcOtherScala As RECT

            If IsIconic(hndl) Then
                Dim wp As New WINDOWPLACEMENT With {.length = Runtime.InteropServices.Marshal.SizeOf(GetType(WINDOWPLACEMENT))}
                GetWindowPlacement(hndl, wp)
                rcOtherScala = wp.normalPosition
            Else
                GetWindowRect(hndl, rcOtherScala)
            End If
            Dim targetPos As Point = New Point(rcOtherScala.left + (rcOtherScala.right - rcOtherScala.left) / 2, rcOtherScala.top + (rcOtherScala.bottom - rcOtherScala.top) / 2)
            Dim menuitPos As Point = menuPos + tsmi.Bounds.Location - New Point(tsmi.Bounds.Height / 2, tsmi.Bounds.Height / 2)
            tsmi.Image = DrawArrow(menuitPos, targetPos)
        Next
    End Sub
    Public Property SidebarScalA As Process = Nothing
    Private Sub SidebarModeToolStripMenuItem_MouseUp(sender As ToolStripMenuItem, e As MouseEventArgs) Handles SidebarModeToolStripMenuItem.MouseUp
        If sender.Checked AndAlso e.Button = MouseButtons.Right Then
            sender.Checked = False
            IPC.SelectAlt(SidebarScalA.Id, 0)
            SidebarScalA = Nothing
            If sender.HasDropDown Then
                For Each item As ToolStripMenuItem In sender.DropDownItems
                    item.Checked = False
                Next
            End If
        End If
    End Sub

    Private Sub SidebarModeToolStripMenuItem_DropDownOpening(sender As ToolStripMenuItem, e As EventArgs) Handles SidebarModeToolStripMenuItem.DropDownOpening
        Dim otherScalAs As List(Of Process) = IPC.EnumOtherScalAs.ToList
        sender.DropDownItems.Clear()
        If otherScalAs.Count = 0 Then
            sender.DropDownItems.Add(NoOtherScalAsToolStripMenuItem)
        Else
            Dim Rang = otherScalAs.Select(Function(p) New ToolStripMenuItem(p.ProcessName,
                                                                            Nothing,
                                                                            AddressOf SelectSidebarTarget) With {.Tag = p,
                                                                                                                 .Checked = (SidebarScalA IsNot Nothing AndAlso p.Id = SidebarScalA.Id)
                                                                                                                }).ToArray
            sender.DropDownItems.AddRange(Rang)
        End If
    End Sub

    Private Sub SelectSidebarTarget(sender As ToolStripMenuItem, e As EventArgs)
        SidebarScalA = sender.Tag
    End Sub

    Private Sub SidebarModeToolStripMenuItem_DropDownOpened(sender As ToolStripMenuItem, e As EventArgs) Handles SidebarModeToolStripMenuItem.DropDownOpened
        Dim menuPos As Point = sender.DropDown.Bounds.Location
        For Each tsmi As ToolStripMenuItem In sender.DropDownItems.OfType(Of ToolStripMenuItem).Where(Function(mi) mi.Tag IsNot Nothing)
            Dim hndl As IntPtr = DirectCast(tsmi.Tag, Process).GetWindowHandle 'mainwindowhandle reports null for attached ScalAs. replaced with a fields in IPC
            Dim rcOtherScala As RECT

            If IsIconic(hndl) Then
                Dim wp As New WINDOWPLACEMENT With {.length = Runtime.InteropServices.Marshal.SizeOf(GetType(WINDOWPLACEMENT))}
                GetWindowPlacement(hndl, wp)
                rcOtherScala = wp.normalPosition
            Else
                GetWindowRect(hndl, rcOtherScala)
            End If
            Dim targetPos As Point = New Point(rcOtherScala.left + (rcOtherScala.right - rcOtherScala.left) / 2, rcOtherScala.top + (rcOtherScala.bottom - rcOtherScala.top) / 2)
            Dim menuitPos As Point = menuPos + tsmi.Bounds.Location - New Point(tsmi.Bounds.Height / 2, tsmi.Bounds.Height / 2)
            tsmi.Image = DrawArrow(menuitPos, targetPos)
        Next

        'TODO: figure out why this won't work.
        'Dim tooltipHWnd = FindWindow("WindowsForms10.tooltips_class32.app.0.141b42a_r9_ad1", Nothing)
        'SetWindowPos(tooltipHWnd, SWP_HWND.TOP, -1, -1, -1, -1, SetWindowPosFlags.DoNotActivate) ' Or SetWindowPosFlags.IgnoreMove)

        BringTooltipToTop()

    End Sub

    Private Sub BringTooltipToTop()
        EnumWindows(Function(hWnd, lParam)
                        If IsWindowVisible(hWnd) Then
                            Dim className As New System.Text.StringBuilder(256)
                            GetClassName(hWnd, className, className.Capacity)
                            If className.ToString().Contains("tooltips_class32") Then
                                SetWindowPos(hWnd, SWP_HWND.TOP, -1, -1, -1, -1, SetWindowPosFlags.DoNotActivate Or SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.IgnoreResize)
                                Return False 'stop enumeration, note: IsWindowVisible is needed as enumeration includes non-visible tts
                            End If
                        End If
                        Return True 'continue enumeration
                    End Function, IntPtr.Zero)
    End Sub
    Private Function DrawArrow(startPoint As Point, endPoint As Point) As Image

        Dim bmp As New Bitmap(16, 16)

        Using g As Graphics = Graphics.FromImage(bmp)
            ' Calculate the angle between the two points
            Dim angle As Double = Math.Atan2(endPoint.Y - startPoint.Y, endPoint.X - startPoint.X) * 180 / Math.PI - 90

            g.TranslateTransform(8, 8) ' Translate to the center of the bitmap
            g.RotateTransform(angle) ' Rotate based on the angle

            g.DrawImage(My.Resources.gnome_maps16, New Point(-8, -8))

            g.ResetTransform() ' Reset the transformations
        End Using

        Return bmp
    End Function

    Private Async Sub MoveTo_Click(sender As ToolStripMenuItem, e As EventArgs)
        Dim ap As AstoniaProcess = sender.Tag.item1
        Dim sp As Process = sender.Tag.Item2
        dBug.Print($"moving {ap.Name} to {sp.Id} {sp.ProcessName}")
        IPC.AddToWhitelistOrRemoveFromBL(sp.Id, ap.Id)

        If KeepToolStripMenuItem.Checked Then Exit Sub

        Dim i = 0
        Do
            Await Task.Delay(50)
            i += 1
        Loop Until IPC.AddToWhitelistOrRemoveFromBL(sp.Id) = 0 OrElse i >= 20
        If IPC.AddToWhitelistOrRemoveFromBL(sp.Id) = 0 Then
            If My.Settings.Whitelist Then
                topSortList.RemoveAll(Function(it) it = ap.UserName)
                botSortList.RemoveAll(Function(it) it = ap.UserName)
            Else
                blackList.Add(ap.UserName)
            End If
            My.Settings.topSort = String.Join(vbCrLf, blackList.Concat(topSortList))
            My.Settings.botSort = String.Join(vbCrLf, blackList.Concat(botSortList))

            apSorter = New AstoniaProcessSorter(topSortList, botSortList)

            Await Task.Delay(100)
            Detach(True)
        End If
    End Sub
    Private Sub CmsAlt_Closed(sender As Object, e As ToolStripDropDownClosedEventArgs) Handles cmsAlt.Closed
        If TypeOf sender.SourceControl Is AButton Then
            Dim but As AButton = sender.sourcecontrol
            If Not but.Contains(MousePosition) Then
                but.BackColor = If(My.Settings.DarkMode, Color.DarkGray, Color.FromArgb(&HFFE1E1E1))
            End If
        End If
        AButton.ActiveOverview = My.Settings.gameOnOverview
        Me.TopMost = My.Settings.topmost
    End Sub
    Private TTinit As Boolean = False

    Private Sub CmsAlt_Opened(sender As ContextMenuStrip, e As EventArgs) Handles cmsAlt.Opened
        AButton.ActiveOverview = False
        Task.Run(Sub() Me.BeginInvoke(Sub() CloseAllButNameToolStripMenuItem.Visible = AstoniaProcess.ListProcesses(blackList, True).Any(Function(ap As AstoniaProcess) ap.Id <> sender.Tag.Id)))

#If DEBUG Then
        If TTinit Then
            TTinit = False
            'find tooltip window
            EnumWindows(Function(w As IntPtr, l As IntPtr)
                            Dim pid As Integer
                            Dim thread = GetWindowThreadProcessId(w, pid)
                            If pid <> scalaPID Then Return True
                            If Not GetWindowClass(w).ToLower.Contains("tooltips_class32") Then Return True
                            Debug.Print($"Found tooltip {thread} ""{GetWindowText(w)}"" sender.handle {sender.Handle}")
                            Debug.Print($"ga root {GetAncestor(w, GA_ROOT)} {GetAncestor(sender.Handle, GA_ROOT)}")
                            Debug.Print($"ga parent {GetAncestor(w, GA_PARENT)} {GetAncestor(sender.Handle, GA_PARENT)}")
                            Debug.Print($"ga rootowner {GetAncestor(w, GA_ROOTOWNER)} {GetAncestor(sender.Handle, GA_ROOTOWNER)}")

                            If GetAncestor(w, GA_PARENT) <> GetAncestor(sender.Handle, GA_PARENT) Then Return True

                            'trying to fix tooltip phasing in/out when it gets bumped due to not fitting working area

                            'fixed by using own tooltip

                            Return False
                        End Function, IntPtr.Zero)
        End If
#End If


    End Sub
    Private Sub SortSubToolStripMenuItem_MouseEnter(sender As ToolStripMenuItem, e As EventArgs) Handles SortSubToolStripMenuItem.MouseEnter
        If MouseButtons And MouseButtons.Right = MouseButtons.Right Then
            sender.Image = My.Resources.gear_wheel
        End If
    End Sub
    Private Sub SortSubToolStripMenuItem_MouseLeave(sender As ToolStripMenuItem, e As EventArgs) Handles SortSubToolStripMenuItem.MouseLeave
        sender.Image = Nothing
    End Sub
    Private Sub SortSubToolStripMenuItem_MouseDown(sender As ToolStripMenuItem, e As MouseEventArgs) Handles SortSubToolStripMenuItem.MouseDown
        If e.Button = MouseButtons.Right Then
            sender.Image = My.Resources.gear_wheel
        End If
    End Sub
    Private Sub SortSubToolStripMenuItem_MouseUp(sender As ToolStripMenuItem, e As MouseEventArgs) Handles SortSubToolStripMenuItem.MouseUp
        If e.Button = MouseButtons.Right Then
            FrmSettings.Tag = FrmSettings.tabSortAndBL
            FrmSettings.Show()
            FrmSettings.BringToFront()
            cmsAlt.Close()
        End If
    End Sub

    Private Sub SortSubToolStripMenuItem_DropDownOpening(sender As ToolStripMenuItem, e As EventArgs) Handles SortSubToolStripMenuItem.DropDownOpening

        For Each item In sender.DropDownItems.OfType(Of ToolStripMenuItem)
            item.CheckState = CheckState.Unchecked
        Next

        Dim AltName As String = DirectCast(sender.Tag, AstoniaProcess).UserName

        Dim topContains As Boolean = topSortList.Contains(AltName)
        Dim botContains As Boolean = botSortList.Contains(AltName)

        If topContains Then
            TopFirstToolStripMenuItem.CheckState = CheckState.Indeterminate
            TopLastToolStripMenuItem.CheckState = CheckState.Indeterminate
        End If

        If botContains Then
            BotFirstToolStripMenuItem.CheckState = CheckState.Indeterminate
            BotLastToolStripMenuItem.CheckState = CheckState.Indeterminate
        End If

        If topSortList.FirstOrDefault() = AltName Then
            TopFirstToolStripMenuItem.CheckState = CheckState.Checked
            TopLastToolStripMenuItem.CheckState = CheckState.Unchecked
        End If
        If topSortList.LastOrDefault() = AltName Then
            TopLastToolStripMenuItem.CheckState = CheckState.Checked
            TopFirstToolStripMenuItem.CheckState = CheckState.Unchecked
        End If

        NoneSortToolStripMenuItem.Checked = Not (topContains OrElse botContains)

        If botSortList.FirstOrDefault() = AltName Then
            BotFirstToolStripMenuItem.CheckState = CheckState.Checked
            BotLastToolStripMenuItem.CheckState = CheckState.Unchecked
        End If
        If botSortList.LastOrDefault() = AltName Then
            BotLastToolStripMenuItem.CheckState = CheckState.Checked
            BotFirstToolStripMenuItem.CheckState = CheckState.Unchecked
        End If

    End Sub

    Private Sub NoneSortToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles NoneSortToolStripMenuItem.Click,
            TopFirstToolStripMenuItem.Click, TopLastToolStripMenuItem.Click,
            BotFirstToolStripMenuItem.Click, BotLastToolStripMenuItem.Click
        Dim AltName As String = DirectCast(sender.OwnerItem.Tag, AstoniaProcess).UserName
        dBug.Print($"Apply sorting {AltName} {sender.Tag}")

        topSortList.Remove(AltName)
        botSortList.Remove(AltName)

        Select Case sender.Tag
            Case -2
                topSortList.Insert(0, AltName)
            Case -1
                topSortList.Add(AltName)
            Case 1
                botSortList.Insert(0, AltName)
            Case 2
                botSortList.Add(AltName)
        End Select

        My.Settings.topSort = String.Join(vbCrLf, blackList.Concat(topSortList))
        My.Settings.botSort = String.Join(vbCrLf, blackList.Concat(botSortList))

        apSorter = New AstoniaProcessSorter(topSortList, botSortList)

    End Sub
    Private Sub CloseAllIdle_Click(sender As ToolStripMenuItem, e As EventArgs) Handles CloseAllIdleToolStripMenuItem.Click

        For Each pp As AstoniaProcess In AstoniaProcess.EnumSomeone
            If sender.Tag?.id = pp.Id AndAlso sender.Tag?.name = "Someone" Then Continue For
            'PostMessage(pp.MainWindowHandle, &H100, Keys.F12, IntPtr.Zero)
            pp.CloseOrKill()
        Next

    End Sub

    ' Icon caching now in QLIconCache module

#Region "ParseDir Helpers"
    ''' <summary>
    ''' Attaches standard event handlers to a QuickLaunch menu item
    ''' </summary>
    Private Sub AttachQLMenuItemHandlers(item As ToolStripMenuItem, isFolder As Boolean)
        AddHandler item.MouseDown, AddressOf QL_MouseDown
        AddHandler item.Paint, AddressOf QLMenuItem_Paint

        If isFolder Then
            AddHandler item.DoubleClick, AddressOf DblClickDir
            AddHandler item.DropDownOpening, AddressOf ParseSubDir
            AddHandler item.DropDownOpened, AddressOf QL_DropDownOpened
            AddHandler item.DropDown.Closing, AddressOf CmsQuickLaunch_Closing
            AddHandler item.DropDown.DragEnter, AddressOf QL_DragEnter
            AddHandler item.DropDown.DragOver, AddressOf QL_DragOver
            item.DropDown.AllowDrop = True
        End If
    End Sub
#End Region

    Private Function ParseDir(pth As String, Optional itemsOnly As Boolean = False) As List(Of ToolStripItem)
        Dim menuItems As New List(Of ToolStripItem)
        Dim isEmpty As Boolean = True
        'Const ICONTIMEOUT = 50
        Const TOTALTIMEOUT = 2000
        Dim timedout As Boolean = False

        Dim watch As Stopwatch = Stopwatch.StartNew()

        Dim Dirs As New ConcurrentBag(Of ToolStripItem)
        Dim Files As New ConcurrentBag(Of ToolStripItem)
        cts = New Threading.CancellationTokenSource
        cantok = cts.Token

        ' Reduce parallelism for HDDs (drives with seek penalty)
        Dim driveLetter As Char = If(pth.Length >= 1, pth(0), "C"c)
        Dim isHDD As Boolean = DriveIncursSeekPenalty(driveLetter)
        Dim maxDegree As Integer = If(isHDD, Math.Max(1, usableCores \ 4), Math.Max(1, usableCores - 2))
        Dim opts As New ParallelOptions With {.CancellationToken = cantok, .MaxDegreeOfParallelism = maxDegree}
        Dim hiddencount As Integer = 0
        Try
            IO.Directory.EnumerateFileSystemEntries(pth).GetEnumerator()?.Dispose()
        Catch ex As Exception
            Dim path As String = IO.Path.GetDirectoryName(pth.TrimEnd("\"c))
            If Not path.EndsWith("\"c) Then path &= "\"c
            Dim msgparts As String() = ex.Message.Replace(path, "").Split("'"c)
            Dim message As String
            If msgparts.Length = 3 Then
                message = "'"c & msgparts(1) & "'"c & vbCrLf & msgparts(0) & msgparts(2).Trim()
            ElseIf msgparts.Length >= 3 Then
                Dim pathstart As Integer = ex.Message.IndexOf("'"c)
                Dim pathend As Integer = ex.Message.LastIndexOf("'"c)
                message = ex.Message.Substring(pathstart, pathend - pathstart + 1).Replace(path, "") & vbCrLf & ex.Message.Substring(0, pathstart) & ex.Message.Substring(pathend + 1).Trim
            Else
                message = ex.Message
            End If
            menuItems.Add(New ToolStripMenuItem("<Access Denied>", My.Resources.denied) With {.Enabled = False, .ToolTipText = message})
            Return menuItems
        End Try
        Try
            Parallel.ForEach(EnumerateData(pth, True, cantok).AsThrottled(usableCores), opts,
                             Sub(data As WIN32_FIND_DATAW)

                                 Dim fulldirs As String = IO.Path.Combine(pth, data.cFileName)

                                 Dim attr As System.IO.FileAttributes = data.dwFileAttributes
                                 Dim hidden As Boolean = False
                                 If attr.HasFlag(System.IO.FileAttributes.Hidden) OrElse attr.HasFlag(System.IO.FileAttributes.System) Then
                                     Threading.Interlocked.Increment(hiddencount)
                                     hidden = True
                                 End If

                                 Dim vis As Boolean = Not hidden OrElse ctrlshift_pressed OrElse My.Settings.QLShowHidden
                                 Dim dispname As String = System.IO.Path.GetFileName(fulldirs)

                                 Dim qli As New QLInfo With {.path = fulldirs & "\", .hidden = hidden, .name = dispname, .isFolder = True}
                                 Dim smenu As New ToolStripMenuItem(If(vis, dispname.Replace("&", "&&"), "*Hidden*"), FolderIcon) With {.Tag = qli, .Visible = vis, .DoubleClickEnabled = True}

                                 Me.BeginInvoke(Sub()
                                                    smenu.DropDownItems.Add("(Dummy)").Enabled = False
                                                    AttachQLMenuItemHandlers(smenu, isFolder:=True)
                                                End Sub)

                                 Dirs.Add(smenu)
                                 If vis Then
                                     isEmpty = False
                                 End If
                                 If watch.ElapsedMilliseconds > TOTALTIMEOUT Then
                                     timedout = True
                                 End If
                             End Sub)

            'Parallel.ForEach(IO.Directory.EnumerateFiles(pth).AsThrottled(usableCores).Where(Function(p) QLFilter.Contains(System.IO.Path.GetExtension(p).ToLower)), opts,
            'Sub(fullLink As String)
            Parallel.ForEach(EnumerateData(pth, False, cantok).AsThrottled(usableCores).Where(Function(p) QLFilter.Contains(System.IO.Path.GetExtension(p.cFileName).ToLower)), opts,
                             Sub(data As WIN32_FIND_DATAW)
                                 Dim fullLink As String = IO.Path.Combine(pth, data.cFileName)

                                 Dim attr As System.IO.FileAttributes = data.dwFileAttributes 'New System.IO.FileInfo(fullLink).Attributes
                                 Dim hidden As Boolean = False
                                 If attr.HasFlag(System.IO.FileAttributes.Hidden) OrElse attr.HasFlag(System.IO.FileAttributes.System) Then
                                     Threading.Interlocked.Increment(hiddencount)
                                     hidden = True
                                 End If


                                 'don't add self to list
                                 If System.IO.Path.GetFileName(fullLink) = System.IO.Path.GetFileName(Environment.GetCommandLineArgs(0)) Then Exit Sub 'Continue For
                                 If fullLink = Environment.GetCommandLineArgs(0) Then Exit Sub 'Continue For
                                 Dim target As String = String.Empty

                                 Dim vis As Boolean = Not hidden OrElse ctrlshift_pressed OrElse My.Settings.QLShowHidden

                                 Dim qli As New QLInfo With {.path = fullLink, .hidden = hidden}

                                 If fullLink.ToLower.EndsWith(".lnk") Then

                                     Dim lin As New ShellLinkInfo(fullLink)

                                     target = lin.TargetPath

                                     qli.target = target

                                     If My.Settings.QLResolveLnk Then

                                         qli.pointsToDir = lin.PointsToDir 'pointstodir can be false negative when access is denied

                                         If lin.PointsToDir OrElse (Not String.IsNullOrEmpty(target) AndAlso CallAsTaskWithTimeout(AddressOf IO.Directory.Exists, target, 200)) Then

                                             If Not qli.target.EndsWith("\"c) Then qli.target &= "\"

                                             'Dim hid As Boolean = Not hidden OrElse ctrlshift_pressed OrElse My.Settings.QLShowHidden
                                             Dim dispname As String = System.IO.Path.GetFileNameWithoutExtension(fullLink)
                                             qli.name = dispname
                                             qli.isFolder = True
                                             Dim smenu As New ToolStripMenuItem(If(vis, dispname.Replace("&", "&&"), "*Hidden*"), FolderIconWithOverlay) With {.Tag = qli, .Visible = vis, .DoubleClickEnabled = True}

                                             Me.BeginInvoke(Sub()
                                                                smenu.DropDownItems.Add("(Dummy)").Enabled = False
                                                                AttachQLMenuItemHandlers(smenu, isFolder:=True)
                                                            End Sub)

                                             addLinkWatcher(target, fullLink)

                                             Dirs.Add(smenu)
                                             If vis Then isEmpty = False

                                             Exit Sub 'Continue For
                                         End If
                                     End If
                                 End If
                                 Dim linkName As String
                                 If hideExt.Contains(System.IO.Path.GetExtension(fullLink).ToLower) Then
                                     linkName = System.IO.Path.GetFileNameWithoutExtension(fullLink)
                                 Else
                                     linkName = System.IO.Path.GetFileName(fullLink)
                                 End If

                                 qli.name = linkName
                                 qli.isFolder = False

                                 Dim item As New ToolStripMenuItem(If(vis, linkName.Replace("&", "&&"), "*Hidden*")) With {.Tag = qli, .Visible = vis}
                                 Me.BeginInvoke(Sub() AttachQLMenuItemHandlers(item, isFolder:=False))

                                 Files.Add(item)
                                 If vis Then
                                     isEmpty = False
                                 End If
                                 If watch.ElapsedMilliseconds > TOTALTIMEOUT Then
                                     timedout = True
                                 End If
                             End Sub)
        Catch ex As System.OperationCanceledException
            menuItems.Add(New ToolStripMenuItem("<Operation Canceled>", My.Resources.denied) With {.Enabled = False})
        End Try

        ' Apply custom sort order if available
        Dim sortOrder As List(Of String) = ReadSortOrder(pth)
        Dim sortedDirs = ApplySortOrderV2(Dirs.ToList(), sortOrder, Function(d) CType(d.Tag, QLInfo).path.TrimEnd("\"c), NsSorter)
        Dim sortedFiles = ApplySortOrderV2(Files.ToList(), sortOrder, Function(f) CType(f.Tag, QLInfo).path, NsSorter)
        Dim allItems = sortedDirs.Concat(sortedFiles).ToList()

        If itemsOnly Then
            DeferredIconLoading(Dirs, Files, cantok)
            Return allItems
        End If

        ' Handle overflow for large folders
        If allItems.Count > QL_INITIAL_ITEMS Then
            menuItems = allItems.Take(QL_INITIAL_ITEMS).ToList()
            Dim remainingItems = allItems.Skip(QL_INITIAL_ITEMS).ToList()
            Dim moreItem = CreateLoadMoreItem(remainingItems)
            menuItems.Add(moreItem)
        Else
            menuItems = allItems
        End If

        If timedout Then
            menuItems.Add(New ToolStripMenuItem("<TimedOut>") With {.Enabled = False})
        End If

        If isEmpty Then
            menuItems.Add(New ToolStripMenuItem("(Empty)") With {.Enabled = False, .ToolTipText = $"{If(hiddencount, $"Press Ctrl-Shift to reveal {hiddencount} Files/Dirs.{vbCrLf}", "")}Folder may still contain unwatched items.{vbCrLf}Go to Settings/QL and adjust Filter if you are missing files."})
            clipBoardInfo = GetClipboardFilesAndAction()
            If clipBoardInfo.Files?.Count > 0 AndAlso clipBoardInfo.Files.Any(Function(f) IO.File.Exists(f) OrElse IO.Directory.Exists(f)) Then


                PasteSep = New ToolStripSeparator

                menuItems.Add(PasteSep)

                pasteTSItem = New ToolStripMenuItem("Paste ", Nothing, AddressOf ClipAction)

                menuItems.Add(pasteTSItem)
                If clipBoardInfo.Action.HasFlag(DragDropEffects.Link) Then
                    pasteLinkTSItem = New ToolStripMenuItem("Paste Shortcut", Nothing, AddressOf ClipAction)
                    menuItems.Add(pasteLinkTSItem)
                End If

                pasteTSItem.Tag = New MenuTag With {.path = pth & "Empty", .action = "Paste"}
                pasteLinkTSItem.Tag = New MenuTag With {.path = pth & "Empty", .action = "PasteLink"}

                If clipBoardInfo.Files.Count = 1 Then
                    Dim fil As String = clipBoardInfo.Files(0)
                    If Not (IO.File.Exists(fil) OrElse IO.Directory.Exists(fil)) Then
                        Debug.Print("file/dir missing")
                        PasteSep.Visible = False
                        pasteTSItem.Visible = False
                        pasteLinkTSItem.Visible = False
                    Else
                        Dim nm As String = IO.Path.GetFileName(fil)
                        If String.IsNullOrEmpty(nm) Then nm = fil
                        If hideExt.Contains(IO.Path.GetExtension(nm)) Then
                            nm = IO.Path.GetFileNameWithoutExtension(nm)
                        End If
                        pasteTSItem.Text = $"{If(clipBoardInfo.Action.HasFlag(DragDropEffects.Move), "Move", "Paste")} ""{nm.CapWithEllipsis(16)}"""
                        pasteTSItem.ToolTipText = If(nm.Length > 16, nm, "")

                        If clipBoardInfo.Files(0).ToLower.EndsWith(".lnk") Then
                            pasteLinkTSItem.Visible = False
                        Else
                            pasteLinkTSItem.Text = "Paste Shortcut"
                        End If

                        Task.Run(Sub()
                                     Dim img = GetIconFromFile(fil, supressCacheMiss:=True)
                                     Me.Invoke(Sub()
                                                   pasteTSItem.Image = img
                                                   If clipBoardInfo.Files(0).ToLower.EndsWith(".lnk") Then
                                                       pasteTSItem.Image = img.addOverlay(My.Resources.shortcutOverlay)
                                                   Else
                                                       pasteLinkTSItem.Image = img.addOverlay(My.Resources.shortcutOverlay)
                                                   End If
                                               End Sub)
                                 End Sub)
                    End If
                Else
                    pasteTSItem.Text = $"{If(clipBoardInfo.Action.HasFlag(DragDropEffects.Move), "Move", "Paste")} Multiple ({clipBoardInfo.Files?.Count})"

                    Dim idx = 0
                    Dim sb As New Text.StringBuilder
                    For Each clippath As String In clipBoardInfo.Files
                        sb.AppendLine(IO.Path.GetFileName(clippath) & If(IO.Directory.Exists(clippath), "\", ""))
                        idx += 1
                        If idx >= 5 Then
                            sb.AppendLine($"<and {clipBoardInfo.Files.Count - idx} more>")
                            Exit For
                        End If
                    Next
                    pasteTSItem.ToolTipText = sb.ToString

                    pasteTSItem.Image = My.Resources.multiPaste

                    pasteLinkTSItem.Text = "Paste Shortcuts"
                    pasteLinkTSItem.Image = pasteTSItem.Image.addOverlay(My.Resources.shortcutOverlay)
                End If

            End If
        End If

        menuItems.Add(New ToolStripSeparator With {.Visible = isEmpty OrElse ctrlshift_pressed})
        Dim addShortcutMenu As New ToolStripMenuItem("New", My.Resources.Add) With {.Tag = pth, .Visible = isEmpty OrElse ctrlshift_pressed}
        addShortcutMenu.DropDownItems.Add("(Dummy)").Enabled = False
        AddHandler addShortcutMenu.DropDownOpening, AddressOf AddShortcutMenu_DropDownOpening
        AddHandler addShortcutMenu.DropDownOpened, AddressOf QL_DropDownOpened
        menuItems.Add(addShortcutMenu)

        'cts?.Dispose()
        'cts = New Threading.CancellationTokenSource
        'cantok = cts.Token
        DeferredIconLoading(Dirs, Files, cantok)

        dBug.Print($"parsing ""{pth}"" took {watch.ElapsedMilliseconds} ms")
        watch.Stop()
        Return menuItems
    End Function

    ''' <summary>
    ''' Creates a "Load More" menu item that displays remaining items when clicked.
    ''' </summary>
    Private Function CreateLoadMoreItem(remainingItems As List(Of ToolStripItem)) As ToolStripMenuItem
        Dim moreItem As New ToolStripMenuItem($"<{remainingItems.Count} more...>") With {
            .Tag = remainingItems,
            .ForeColor = COLOR_WINDOWS_BLUE
        }
        AddHandler moreItem.MouseDown, AddressOf LoadMoreItems_MouseDown
        Return moreItem
    End Function

    ''' <summary>
    ''' Handles click on "Load More" item - inserts remaining items into the menu.
    ''' </summary>
    Private Sub LoadMoreItems_MouseDown(sender As Object, e As MouseEventArgs)

        If Not e.Button.HasFlag(MouseButtons.Left) Then
            Exit Sub
        End If

        Dim moreItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim remainingItems As List(Of ToolStripItem) = CType(moreItem.Tag, List(Of ToolStripItem))
        Dim owner As ToolStripDropDown = moreItem.Owner

        If owner Is Nothing OrElse remainingItems Is Nothing Then Exit Sub

        ' Find position of the "more" item
        Dim insertIndex As Integer = owner.Items.IndexOf(moreItem)
        If insertIndex < 0 Then Exit Sub

        ' Remove the "more" item
        owner.Items.Remove(moreItem)

        ' Determine how many items to load this batch
        Dim itemsToAdd As List(Of ToolStripItem)
        Dim newRemainingItems As List(Of ToolStripItem) = Nothing

        If remainingItems.Count > QL_LOAD_MORE_BATCH Then
            itemsToAdd = remainingItems.Take(QL_LOAD_MORE_BATCH).ToList()
            newRemainingItems = remainingItems.Skip(QL_LOAD_MORE_BATCH).ToList()
        Else
            itemsToAdd = remainingItems
        End If

        ' Insert the new items at the position of the old "more" item
        For i As Integer = 0 To itemsToAdd.Count - 1
            owner.Items.Insert(insertIndex + i, itemsToAdd(i))
        Next

        ' If there are still more items, add a new "more" item
        If newRemainingItems IsNot Nothing AndAlso newRemainingItems.Count > 0 Then
            Dim newMoreItem = CreateLoadMoreItem(newRemainingItems)
            owner.Items.Insert(insertIndex + itemsToAdd.Count, newMoreItem)
        End If

        ' Start deferred icon loading for the newly added items
        Dim dirs As New ConcurrentBag(Of ToolStripItem)(itemsToAdd.Where(Function(it) TypeOf it.Tag Is QLInfo AndAlso CType(it.Tag, QLInfo).path.EndsWith("\")))
        Dim files As New ConcurrentBag(Of ToolStripItem)(itemsToAdd.Where(Function(it) TypeOf it.Tag Is QLInfo AndAlso Not CType(it.Tag, QLInfo).path.EndsWith("\")))
        DeferredIconLoading(dirs, files, cantok)

        moreItem.Dispose()
        dBug.Print($"Loaded {itemsToAdd.Count} more items, {If(newRemainingItems?.Count, 0)} remaining")
    End Sub

#If DEBUG Then
    Private Sub QL_DropDownClosed(sender As ToolStripDropDown, e As ToolStripDropDownClosedEventArgs)
        For Each it As ToolStripItem In sender.Items.Cast(Of ToolStripItem).ToArray
            If TypeOf it.Tag Is QLInfo Then
                'it.Dispose() 'this causes a disposed eception somewhere
            Else
                'it.Visible = True
            End If
        Next
    End Sub
#End If
    Public Sub DisposeMenuRecurse(items As ToolStripItemCollection)
        For Each item As ToolStripItem In items.Cast(Of ToolStripItem).ToArray
            If TypeOf item Is ToolStripMenuItem Then
                Dim tsmi As ToolStripMenuItem = item
                If tsmi.HasDropDownItems Then
                    DisposeMenuRecurse(tsmi.DropDown.Items)
                End If
            End If
            item.Dispose()
        Next
    End Sub

    ' EnumerateData now in QLDirectoryParser module


    Private Sub QL_DropDownOpened(sender As ToolStripMenuItem, e As EventArgs)
        Dim handle = sender.DropDown.Handle

        Dim owner = GetWindowLong(handle, GWL_HWNDPARENT)
        Debug.Print($"Dropdown taskbar debug ""{owner}""")
        If owner = 0 Then
            'there is an activation issue here, don't know which window needs to be active for this to not occur
            'everything i've tried is more glitchy than the current bug
            SetWindowLong(handle, GWL_HWNDPARENT, GetWindowLong(cmsQuickLaunch.Handle, GWL_HWNDPARENT))
        End If

        ' Handle overflow for large folders - constrain dropdown to screen bounds
        Dim dropDown = sender.DropDown
        Dim dropDownScreen = Screen.FromPoint(dropDown.Bounds.Location)
        Dim workingArea = dropDownScreen.WorkingArea

        ' Calculate maximum height based on screen working area with some padding
        Dim maxHeight As Integer = workingArea.Height - 20

        ' Set MaximumSize to enable scroll arrows when content exceeds screen height
        If dropDown.MaximumSize.Height <> maxHeight Then
            dropDown.MaximumSize = New Size(0, maxHeight)
        End If

        ' Ensure dropdown doesn't go off-screen vertically
        Dim dropDownBounds = dropDown.Bounds
        If dropDownBounds.Bottom > workingArea.Bottom Then
            Dim newY As Integer = Math.Max(workingArea.Top, workingArea.Bottom - dropDown.Height)
            dropDown.Top = newY
        End If
        If dropDownBounds.Top < workingArea.Top Then
            dropDown.Top = workingArea.Top
        End If

    End Sub

    Dim PasteSep As ToolStripSeparator = New ToolStripSeparator
    Dim pasteTSItem As ToolStripMenuItem = New ToolStripMenuItem("Paste ""Name""", Nothing, AddressOf ClipAction)
    Dim pasteLinkTSItem As ToolStripMenuItem = New ToolStripMenuItem("Paste Shortcut", Nothing, AddressOf ClipAction)

    'cannot rely on these, diff win version have diff icons for these
    'Public multiPasteBitmap As Bitmap = loadImageResBitmap(245)
    'Public shortcutOverlay As Bitmap = loadImageResBitmap(154)
    'Public warningOverlay As Bitmap = loadImageResBitmap(219) 'note this is incorrect in windows10

    Dim multipasteBitmapOverlay As Bitmap = My.Resources.multiPaste.addOverlay(My.Resources.shortcutOverlay)

    Dim QLCtxMenuOpenedOn As ToolStripMenuItem

    ' Drag & Drop sorting state moved to QLDragDropHandler.State

    ' Flag to force QL close (bypasses Ctrl-held check)
    Private qlForceClose As Boolean = False

    ' Paste operation tracking
    Private qlPasting As Boolean = False
    Private qlPasteTargetPath As String = Nothing
    Private qlPasteOwnerItem As ToolStripMenuItem = Nothing

    ' Overflow handling for large folders
    Private Const QL_INITIAL_ITEMS As Integer = 50
    Private Const QL_LOAD_MORE_BATCH As Integer = 50

    Private Sub QLMenuItem_MouseDown(sender As ToolStripMenuItem, e As MouseEventArgs)
        If e.Button = MouseButtons.Right AndAlso TypeOf sender.Tag Is QLInfo Then
            QLDragDropHandler.State.Start(sender, e.Location)
        End If
    End Sub

    Dim DragSep As New ToolStripSeparator

    Private Sub QLMenuItem_MouseMove(sender As ToolStripMenuItem, e As MouseEventArgs)
        Dim state = QLDragDropHandler.State
        If state.DraggedItem IsNot Nothing AndAlso e.Button = MouseButtons.Right Then
            ' Check if we've moved enough to start dragging
            If Not state.IsActive AndAlso IsDragThresholdExceeded(state.StartPoint, e.Location) Then
                state.IsActive = True
                dBug.Print($"QL Drag started: {state.DraggedItem.Text}")
            End If
        End If
    End Sub

    Private Sub QLMenuItem_MouseEnter(sender As ToolStripMenuItem, e As EventArgs)
        Dim state = QLDragDropHandler.State
        If state.IsActive AndAlso state.DraggedItem IsNot Nothing AndAlso sender IsNot state.DraggedItem Then
            If TypeOf sender.Tag Is QLInfo Then
                ' Clear previous highlight
                If state.DropTarget IsNot Nothing AndAlso state.DropTarget IsNot sender Then
                    state.DropTarget.BackColor = Color.Empty
                End If
                ' Highlight new drop target
                state.DropTarget = sender
                sender.BackColor = Color.FromArgb(100, COLOR_HIGHLIGHT_BLUE)
                dBug.Print($"QL Drop target: {sender.Text}")
            End If
        End If
    End Sub

    Private Sub QLMenuItem_MouseUp(sender As ToolStripMenuItem, e As MouseEventArgs)
        Dim state = QLDragDropHandler.State
        If state.IsActive AndAlso state.DraggedItem IsNot Nothing AndAlso state.DropTarget IsNot Nothing Then
            ' Perform the reorder - now handled by QL_DragDrop
        End If
        ' Clean up drag state
        state.Reset()
    End Sub

    Private Sub PerformQLReorder(dragItem As ToolStripMenuItem, dropTarget As ToolStripMenuItem)
        Dim state = QLDragDropHandler.State
        If state.FolderPath Is Nothing Then Exit Sub

        Dim dragQli As QLInfo = CType(dragItem.Tag, QLInfo)
        Dim dropQli As QLInfo = CType(dropTarget.Tag, QLInfo)

        Dim dragName As String = If(dragQli.path.EndsWith("\"), dragQli.name, IO.Path.GetFileName(dragQli.path))
        Dim dropName As String = If(dropQli.path.EndsWith("\"), dropQli.name, IO.Path.GetFileName(dropQli.path))

        dBug.Print($"QL Reorder: '{dragName}' to position of '{dropName}'")

        ' Delegate to QLDragDropHandler
        QLDragDropHandler.UpdateSortOrder(state.FolderPath, dragName, dropName, dragItem.GetCurrentParent()?.Items)
        dBug.Print($"QL Sort order saved")
    End Sub

    Private Sub QLMenuItem_Paint(sender As ToolStripMenuItem, e As PaintEventArgs)
        If QLCtxMenuOpenedOn Is sender Then
            QLCtxMenuOpenedOn.Select()
        End If
    End Sub

    Private ctrlshift_pressed As Boolean = False
    Sub Application_Idle(sender As Object, e As EventArgs)
        If cmsQuickLaunch.Visible AndAlso My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown AndAlso Not ctrlshift_pressed Then
            ctrlshift_pressed = True
            SetVisRecurse(cmsQuickLaunch.Items)
        End If
    End Sub

    Sub SetVisRecurse(col As ToolStripItemCollection)
        Task.Run(Sub()
                     Dim hasHidden As Boolean = False
                     Dim menu As List(Of ToolStripItem) = col.Cast(Of ToolStripItem).ToList
                     Parallel.ForEach(menu,
                            Sub(it)
                                Me.Invoke(Sub()
                                              If it.Text = "*Hidden*" AndAlso TypeOf it.Tag Is QLInfo Then
                                                  it.Text = CType(it.Tag, QLInfo).name.Replace("&", "&&")
                                                  hasHidden = True
                                              End If
                                              If it.Text <> "(Emtpy)" Then
                                                  If Not (it Is pasteLinkTSItem AndAlso clipBoardInfo.Files.Count = 1 AndAlso clipBoardInfo.Files(0).ToLower.EndsWith(".lnk")) Then
                                                      it.Visible = True
                                                  End If
                                              End If
                                          End Sub)
                                If TypeOf it Is ToolStripMenuItem Then
                                    Dim item As ToolStripMenuItem = it
                                    If item.HasDropDown AndAlso item.DropDown.Visible Then
                                        SetVisRecurse(item.DropDownItems)
                                    End If
                                End If
                            End Sub)
                     If hasHidden Then
                         Dim ei As ToolStripMenuItem = menu.FirstOrDefault(Function(it) it.Text = "(Empty)")
                         If ei IsNot Nothing AndAlso ei.Visible Then
                             Me.Invoke(Sub() ei.Visible = False)
                             Dim itemstohide As New List(Of ToolStripItem)
                             For Each itm As ToolStripItem In menu.SkipWhile(Function(it) TypeOf it IsNot ToolStripSeparator).Skip(1) 'hide items below 1st separator
                                 itemstohide.Add(itm)
                                 If TypeOf itm Is ToolStripSeparator AndAlso itemstohide.Count > 1 Then
                                     For Each it As ToolStripItem In itemstohide
                                         Me.Invoke(Sub() it.Visible = False)
                                     Next
                                     Exit For 'itm
                                 End If
                             Next 'itm
                         End If
                     End If
                 End Sub)
    End Sub

    Private Sub DeferredIconLoading(Dirs As IEnumerable(Of ToolStripItem), Files As IEnumerable(Of ToolStripItem), ct As Threading.CancellationToken)
        Task.Run(Sub()
                     Try
                         Dim opts As New ParallelOptions With {.CancellationToken = ct, .MaxDegreeOfParallelism = Math.Max(1, usableCores - 2)}
                         Dim items = Files.Concat(Dirs)
                         Parallel.ForEach(items, opts,
                                          Sub(it As ToolStripMenuItem)
                                              Dim qli As QLInfo = it.Tag
                                              Dim ico = GetIconFromCache(qli)
                                              Dim check As Boolean = clipBoardInfo.Files?.Contains(qli.path.TrimEnd("\"c))
                                              Me.Invoke(Sub()
                                                            it.Image = ico
                                                            it.Checked = check
                                                        End Sub)
                                          End Sub)

                         Parallel.ForEach(items, opts,
                                          Sub(it As ToolStripMenuItem)
                                              Dim qli As QLInfo = it.Tag
                                              If qli.path.EndsWith(".lnk") AndAlso Not String.IsNullOrEmpty(qli.target) AndAlso
                                                Not CallAsTaskWithTimeout(Function(p) IO.File.Exists(p) OrElse IO.Directory.Exists(p), qli.target, 500) Then
                                                  'qli.invalidTarget = True
                                                  'it.Tag = qli
                                                  Dim ico = it.Image.addOverlay(My.Resources.WarningOverlay, True)
                                                  Me.Invoke(Sub()
                                                                it.Image = ico
                                                                'it.Invalidate()
                                                            End Sub)
                                              End If
                                          End Sub)

                     Catch ex As System.OperationCanceledException
                         dBug.Print("deferrediconloading operationCanceled")
                     Catch
                         dBug.Print("deferredIconLoading general exception")
                     End Try
                 End Sub, ct)
    End Sub
    Public Sub CloseOtherDropDowns(items As ToolStripItemCollection, Optional keep As HashSet(Of ToolStripMenuItem) = Nothing)
        If keep Is Nothing Then keep = New HashSet(Of ToolStripMenuItem)
        For Each it As ToolStripMenuItem In items.OfType(Of ToolStripMenuItem)
            ' Recursively close all dropdowns of the current item
            If it.HasDropDownItems AndAlso it.DropDown.Visible Then
                CloseOtherDropDowns(it.DropDownItems, keep)
                If Not keep.Contains(it) Then
                    it.DropDown.Close()
                    dBug.Print($"Closing dropdown of {it.Text}")
                End If
            End If
        Next
    End Sub

    Private Sub ParseSubDir(sender As ToolStripMenuItem, e As EventArgs) ' Handles DummyToolStripMenuItem.DropDownOpening
        dBug.Print($"{sender.OwnerItem}")
        dBug.Print($"{sender}")

        Dim keep As New HashSet(Of ToolStripMenuItem)
        Dim curr = sender.OwnerItem
        While curr IsNot Nothing
            keep.Add(curr)
            curr = curr.OwnerItem
        End While

        CloseOtherDropDowns(cmsQuickLaunch.Items, keep)
        Dim qli As QLInfo = sender.Tag
        'Dim target = If(sender.Tag.length >= 4, sender.Tag(3), sender.Tag(0))
        Dim target = If(String.IsNullOrEmpty(qli.target), qli.path, qli.target)

        'sender.DropDownItems.Clear()
        'Dim olditems = New ToolStripItemCollection(sender.Owner, sender.DropDownItems.Cast(Of ToolStripItem).ToArray)
        DisposeMenuRecurse(sender.DropDownItems)

        If CallAsTaskWithTimeout(AddressOf IO.Directory.Exists, target, 750) Then
            sender.DropDownItems.AddRange(ParseDir(target).ToArray)
        Else
            Dim dirname As String = IO.Path.GetDirectoryName(target.TrimEnd("\"c))
            Debug.Print($"dir missing {dirname}")
            Dim name = target.Replace(dirname, "").Trim("\"c)
            sender.DropDownItems.Add(New ToolStripMenuItem("<Error>", My.Resources.Warning) With {.Enabled = False, .ToolTipText = $"'{name}'{vbCrLf}Target Directory Missing"})
        End If

    End Sub

    ' folderIcon now in QLIconCache module
    Private Sub AddShortcutMenu_DropDownOpening(sender As ToolStripMenuItem, e As EventArgs) 'Handles addShortcutMenu.DropDownOpening
        dBug.Print("addshortcut.sendertag:" & sender.Tag)
        sender.DropDownItems.Clear()
        Dim folderitem = New ToolStripMenuItem("Folder", FolderIcon) With {.Tag = sender.Tag}
        AddHandler folderitem.MouseDown, AddressOf Ql_NewFolder
        sender.DropDownItems.Add(folderitem)
        sender.DropDownItems.Add(New ToolStripSeparator())

        For Each alt As AstoniaProcess In AstoniaProcess.Enumerate(blackList).OrderBy(Function(ap) ap.UserName)
            Dim item As ToolStripMenuItem = New ToolStripMenuItem(alt.UserName, alt.GetIcon?.ToBitmap) With {
                                               .Tag = {alt, sender.Tag}}
            AddHandler item.MouseDown, AddressOf CreateShortCut
            sender.DropDownItems.Add(item) ' sender.tag is parent menu location
        Next
        If sender.DropDownItems.Count = 2 Then
            sender.DropDownItems.Add("(None)").Enabled = False
        ElseIf sender.DropDownItems.Count > 3 Then
            sender.DropDownItems.Add(New ToolStripSeparator())
            Dim addallItem As ToolStripMenuItem = New ToolStripMenuItem("Add All", My.Resources.Add) With {.Tag = sender.Tag}
            AddHandler addallItem.MouseDown, AddressOf AddAllShortcuts
            sender.DropDownItems.Add(addallItem)
        End If
        AddHandler sender.DropDown.Closing, Sub(sen As ToolStripDropDown, ev As ToolStripDropDownClosingEventArgs)
                                                If MouseButtons <> MouseButtons.Left AndAlso ev.CloseReason <> ToolStripDropDownCloseReason.CloseCalled Then
                                                    Debug.Print("cancel dropDownClose")
                                                    ev.Cancel = True
                                                End If
                                            End Sub
        Dim parentItem As ToolStripMenuItem = TryCast(sender.OwnerItem, ToolStripMenuItem)
        If parentItem IsNot Nothing Then
            AddHandler parentItem.DropDown.Closing, Sub(sen As ToolStripDropDown, ev As ToolStripDropDownClosingEventArgs)
                                                        ' Close this dropdown when the parent closes
                                                        sender.DropDown.Close(ToolStripDropDownCloseReason.CloseCalled)
                                                        Debug.Print("Closed child dropdown because parent closed.")
                                                    End Sub
        End If
    End Sub

    Private Sub CreateNewFolder(newpath As String)
        dBug.Print($"NewFolder:{newpath}")

        Dim rootFolder As String = newpath

        dBug.Print($"rootFolder: {rootFolder}")

        Dim newfolderPath = IO.Path.Combine(rootFolder, "New Folder")

        Dim i As Integer = 2
        While IO.Directory.Exists(newfolderPath)
            newfolderPath = IO.Path.Combine(rootFolder, $"New Folder ({i})")
            i += 1
        End While

        dBug.Print($"newfolderpath: {newfolderPath}")

        Try
            IO.Directory.CreateDirectory(newfolderPath)
        Catch ex As Exception
            dBug.Print($"CreateDirectory failed: {ex.Message}")
        End Try

        CloseOtherDropDowns(cmsQuickLaunch.Items, Nothing)
        cmsQuickLaunch.Close()

        If IO.Directory.Exists(newfolderPath) Then
            RenameMethod(newfolderPath, newfolderPath.Substring(newfolderPath.TrimEnd("\").LastIndexOf("\") + 1).TrimEnd("\"), isCreateFolder:=True)
        End If

    End Sub

    Private Sub Ql_NewFolder(sender As ToolStripMenuItem, e As MouseEventArgs)
        dBug.Print($"QlCtxNewFolder sender:{sender}")
        dBug.Print($"tag:    {sender?.Tag}")

        If e.Button <> MouseButtons.Left Then
            Exit Sub
        End If

        CloseOtherDropDowns(cmsQuickLaunch.Items, Nothing)
        cmsQuickLaunch.Close()

        CreateNewFolder(sender.Tag)

    End Sub
    Private Shared ReadOnly pipe As Char() = {"|"c}
    Private Sub CreateShortCut(sender As ToolStripMenuItem, e As MouseEventArgs)

        dBug.Print($"CreateShortCut")
        dBug.Print($"sender.text {sender.Text}")

        If e IsNot Nothing AndAlso e.Button <> MouseButtons.Left Then
            Exit Sub
        End If

        CloseOtherDropDowns(cmsQuickLaunch.Items, Nothing)
        cmsQuickLaunch.Close()

        Dim alt As AstoniaProcess = DirectCast(sender.Tag(0), AstoniaProcess)
        Dim ShortCutPath As String = sender.Tag(1)
        Dim ShortCutName As String = alt.UserName
        'If ShortCutName = "Someone" Then Exit Sub
        Dim ShortCutLink As String = ShortCutPath & "\" & ShortCutName & ".lnk"

        If System.IO.File.Exists(ShortCutLink) Then
            Select Case CustomMessageBox.Show(Me, $"""{alt.UserName}"" already exists. Overwrite?",
                           "Notice", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                Case DialogResult.Yes
                    'do nothing
                Case DialogResult.No
                    Exit Sub
                Case Else
                    dBug.Print($"Cancel {sender.Tag} ""{sender.OwnerItem}"" {sender.Text}")
                    If sender.OwnerItem?.Text <> QlCtxNewMenu.Text Then Throw New Exception("Abort")
            End Select
        End If
        'Dim QS As New ManagementObjectSearcher(“Select * from Win32_Process WHERE ProcessID=" & alt.Id.ToString)
        'Dim objCol As ManagementObjectCollection = QS.Get

        Dim mos As Management.ManagementObject = New Management.ManagementObjectSearcher($“Select * from Win32_Process WHERE ProcessID={alt.Id}").Get()(0)

        Dim arguments As String = mos("commandline")

        If arguments = "" Then
            If CustomMessageBox.Show(Me, "Access denied!" & vbCrLf &
                           "Elevate ScalA to Administrator?" & vbCrLf &
                               "You will have to redo the shortcut creation.",
                               "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) _
               = DialogResult.Cancel Then Exit Sub
            My.Settings.Save()
            RestartSelf(True)
            End 'program
            Exit Sub
        End If
        dBug.Print("cmdline:" & arguments)
        If arguments.StartsWith("""") Then
            'arguments = arguments.Substring(1) 'skipped with startindex
            arguments = arguments.Substring(arguments.IndexOf("""", 1) + 1)
        Else
            For Each exe As String In My.Settings.exe.Split(pipe, StringSplitOptions.RemoveEmptyEntries).Select(Function(s) s.Trim.ToLower & ".exe")
                If arguments.ToLower.Contains(exe) Then
                    arguments = arguments.Substring(arguments.ToLower.IndexOf(exe) + exe.Length)
                End If
            Next
        End If



        If Not arguments.ToLower.Contains("-w") Then
            If CustomMessageBox.Show(Me, "Missing '-w' flag" & vbCrLf &
                               "Shortcut will currently not start in windowed mode" & vbCrLf &
                               "Would you like to add the '-w' flag for this shortcut?" & vbCrLf &
                              $"{arguments.Trim}",
                        "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) _
                = DialogResult.Yes Then arguments &= " -w "
        End If

        arguments = Strings.Join(arguments.Split("-").Distinct.ToArray, "-") 'remove duplicates

        dBug.Print("cmdline mang:" & arguments)

        Dim exepath As String = ""
        Try
            exepath = mos("ExecutablePath")
        Catch ex As Exception
            dBug.Print($"Failed to get ExecutablePath: {ex.Message}")
        End Try

        Dim oLink As Object

        Try

            oLink = CreateObject("WScript.Shell").CreateShortcut(ShortCutLink)

            oLink.TargetPath = exepath
            oLink.Arguments = arguments.Trim()
            oLink.WorkingDirectory = alt.GetCurrentDirectory
            'oLink.WorkingDirectory = exepath.Substring(0, exepath.LastIndexOf("\"))
            oLink.WindowStyle = 1
            oLink.Save()
        Catch ex As Exception
            dBug.Print($"except: {ex.Message}")
            dBug.Print($"{ShortCutLink}")
        End Try

    End Sub

    Private Sub QlCtxAddAll(sender As Object, e As EventArgs)
        AddAllShortcuts(sender, Nothing)
    End Sub

    Private Sub AddAllShortcuts(sender As Object, e As MouseEventArgs)
        dBug.Print($"Add All ShortCuts")
        dBug.Print($"sender.tag {sender.tag}")

        If e IsNot Nothing AndAlso e.Button <> MouseButtons.Left Then
            Exit Sub
        End If

        Dim list As List(Of AstoniaProcess) = AstoniaProcess.Enumerate(blackList).OrderBy(Function(ap) ap.UserName).ToList

        CloseOtherDropDowns(cmsQuickLaunch.Items, Nothing)
        cmsQuickLaunch.Close()

        Dim path As String = sender.tag
        If Not path.EndsWith("\") Then
            path = path.Substring(0, path.LastIndexOf("\") + 1)
            dBug.Print($"sanitized: {path}")
        End If

        For Each ap As AstoniaProcess In list
            dBug.Print($"adding {ap.UserName}")
            Try
                CreateShortCut(New ToolStripMenuItem(ap.UserName) With {.Tag = {ap, path}}, Nothing)
            Catch ex As Exception
                If ex.Message = "Abort" Then
                    Exit Sub
                Else
                    Throw
                End If
            End Try
        Next

    End Sub
    Shared scaleFixForm As MenuScaleFixForm = Nothing
    Private NotInheritable Class MenuScaleFixForm : Inherits Form
        Protected Overloads Overrides ReadOnly Property ShowWithoutActivation() As Boolean
            Get
                Return True
            End Get
        End Property
        Public Sub New(scrn As Screen)
            Me.FormBorderStyle = FormBorderStyle.None
            Me.BackColor = Color.Lime
            If FrmMain.chkDebug.Checked Then
                Me.Opacity = 0.1
            Else
                Me.TransparencyKey = Me.BackColor
            End If
            Me.TopMost = True
            Me.ShowInTaskbar = False
            Me.StartPosition = FormStartPosition.Manual
            Me.Bounds = scrn.WorkingArea
        End Sub

        Protected Overrides ReadOnly Property CreateParams As CreateParams
            Get
                Dim param = MyBase.CreateParams
                param.ExStyle = param.ExStyle Or WindowStylesEx.WS_EX_TRANSPARENT
                Return param
            End Get
        End Property

    End Class
    Private Sub CmsQuickLaunch_Opening(sender As ContextMenuStrip, e As System.ComponentModel.CancelEventArgs) Handles cmsQuickLaunch.Opening

        UntrapMouse(MouseButtonStale)

        If Not (My.Computer.Keyboard.ShiftKeyDown AndAlso Not My.Computer.Keyboard.CtrlKeyDown) Then
            If scaleFixForm Is Nothing Then
                dBug.Print("spawning scalefixform")
                scaleFixForm = New MenuScaleFixForm(If(sender.SourceControl Is Nothing, Screen.PrimaryScreen, Screen.FromControl(Me)))
                scaleFixForm.Show()
                Task.Run(Sub() Me.BeginInvoke(Sub() cmsQuickLaunch.Show(MousePosition, If(sender.SourceControl Is Nothing, ToolStripDropDownDirection.AboveLeft, ToolStripDropDownDirection.Default))))
                e.Cancel = True
                Exit Sub
            Else
                scaleFixForm.Bounds = If(sender.SourceControl Is Nothing, Screen.PrimaryScreen, Screen.FromPoint(MousePosition)).WorkingArea
            End If
        End If

        CloseOtherDropDowns(cmsQuickLaunch.Items, New HashSet(Of ToolStripMenuItem))
        cmsQuickLaunch.Close()
        Try
            AppActivate(scalaPID) 'fix right click drag bug
        Catch ex As Exception
            dBug.Print($"Failed to activate ScalA on menu close: {ex.Message}")
        End Try
        'If Not My.Settings.MinMin OrElse Not AltPP?.isSDL Then Detach(False)
        ttMain.Hide(cboAlt)
        ttMain.Hide(btnStart)
        pbZoom.Visible = False
        AButton.ActiveOverview = False
        If My.Computer.Keyboard.ShiftKeyDown AndAlso Not My.Computer.Keyboard.CtrlKeyDown AndAlso sender.SourceControl Is Nothing Then
            dBug.Print($"ShowSysMenu {sender}")
            Dim pt As Point = sender.PointToClient(MousePosition)
            Me.ShowSysMenu(sender, New MouseEventArgs(MouseButtons.Right, 1, pt.X, pt.Y, 0))
            e.Cancel = True
            Exit Sub
        End If

        'tmrTick.Interval = 1000
        'sender.Items.Clear()
        DisposeMenuRecurse(sender.Items)

        If Not FileIO.FileSystem.DirectoryExists(My.Settings.links) Then My.Settings.links = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & "\ScalA"

        If Not FileIO.FileSystem.DirectoryExists(My.Settings.links) Then
            System.IO.Directory.CreateDirectory(My.Settings.links)
            System.IO.Directory.CreateDirectory(IO.Path.Combine(My.Settings.links, "Example Folder"))
        End If

        If My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown Then ctrlshift_pressed = True

        sender.Items.AddRange(ParseDir(IO.Path.GetFullPath(My.Settings.links)).ToArray)

        'If My.Computer.Keyboard.CtrlKeyDown Then
        '    sender.Items.Add(New ToolStripSeparator())
        '    sender.Items.Add("Select Folder", My.Resources.gear_wheel, AddressOf ChangeLinksDir)
        'End If

        'If sender.SourceControl Is btnStart AndAlso My.User.IsInRole(ApplicationServices.BuiltInRole.Administrator) Then
        '    sender.Items.Add(New ToolStripSeparator())
        '    sender.Items.Add("UnElevate", btnStart.Image, AddressOf UnelevateSelf).ToolTipText = $"Drop Admin Rights{vbCrLf}Use this If you can't use ctrl, alt and/or shift."
        'End If

        If fsWatchers.Count = 0 Then InitWatchers(My.Settings.links, fsWatchers)
        If shNotify = 0 Then RegisterShellNotify(Me.Handle)
        ' Dim dummy = FrmSettings.Visible 'needed or frmsettings reference in cmsQuickLaunch.Opened event may cause it to close

    End Sub

    'Private closeAllAtBottom As Boolean = True

    Private Sub cmsQuickLaunch_Opened(sender As ContextMenuStrip, e As EventArgs) Handles cmsQuickLaunch.Opened


        sender.AllowDrop = True

        'Task.Run(Sub()
        '             If AstoniaProcess.EnumSomeone.Any() Then
        '                 If sender.SourceControl Is Nothing Then 'called from trayicon
        '                     Me.Invoke(Sub()
        '                                   sender.Items.Insert(0, New ToolStripSeparator())
        '                                   sender.Items.Insert(0, New ToolStripMenuItem("Close All Someone", My.Resources.moreF12, AddressOf CloseAllIdle_Click))
        '                               End Sub)
        '                     'closeAllAtBottom = False
        '                 Else
        '                     Me.Invoke(Sub()
        '                                   sender.Items.Add(New ToolStripSeparator())
        '                                   sender.Items.Add("Close All Someone", My.Resources.moreF12, AddressOf CloseAllIdle_Click)
        '                               End Sub)
        '                     'closeAllAtBottom = True
        '                 End If
        '             End If
        '         End Sub)


        '   QL wrong when opened from tray due to change in visiblity hidden items.

        If sender.SourceControl Is Nothing Then 'opened from tray

            Dim hwnd As IntPtr = sender.Handle

            Dim rcM As RECT 'clientrect
            GetClientRect(hwnd, rcM)

            Dim loc As Point = MousePosition - New Point(rcM.right, rcM.bottom)

            Dim pswa = Screen.PrimaryScreen.WorkingArea

            loc = New Point(Math.Max(pswa.X, loc.X), Math.Max(pswa.Y, loc.Y))

            ' move QL to correct loc
            SetWindowPos(hwnd, SWP_HWND.TOPMOST, loc.X, loc.Y, -1, -1, SetWindowPosFlags.IgnoreZOrder Or SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate)
        End If

        ' Handle overflow for large folders - constrain menu to screen bounds
        Dim menuScreen = Screen.FromPoint(sender.Bounds.Location)
        Dim menuWorkingArea = menuScreen.WorkingArea
        Dim maxHeight As Integer = menuWorkingArea.Height - 20

        ' Set MaximumSize to enable scroll arrows when content exceeds screen height
        If sender.MaximumSize.Height <> maxHeight Then
            sender.MaximumSize = New Size(0, maxHeight)
        End If

        ' Ensure menu doesn't go off-screen vertically
        If sender.Bottom > menuWorkingArea.Bottom Then
            sender.Top = Math.Max(menuWorkingArea.Top, menuWorkingArea.Bottom - sender.Height)
        End If
        If sender.Top < menuWorkingArea.Top Then
            sender.Top = menuWorkingArea.Top
        End If

    End Sub

    Dim cts As New Threading.CancellationTokenSource
    Dim cantok As Threading.CancellationToken = cts.Token


    Private Sub CmsQuickLaunch_Closing(sender As Object, e As ToolStripDropDownClosingEventArgs) Handles cmsQuickLaunch.Closing ', item.DropDownClosing

        'For Each it As ToolStripMenuItem In CType(sender.items, ToolStripItemCollection).Cast(Of ToolStripItem).Where(Function(mi) TypeOf mi.Tag Is QLInfo)
        '    Dim qli As QLInfo = it.Tag
        '    If qli.invalidTarget Then
        '        'EvictIconCacheItem(qli.path)
        '    End If
        'Next

        ' Keep menu open during paste operations when progress dialog steals focus
        If qlPasting AndAlso e.CloseReason = ToolStripDropDownCloseReason.AppFocusChange Then
            Try
                Dim fgw = GetForegroundWindow()
                Dim pid As Integer
                GetWindowThreadProcessId(fgw, pid)
                dBug.Print($"pasting: menu close due to {GetWindowClass(fgw)} ""{GetWindowText(fgw)}"" pid={pid}")
                ' Cancel close if it's the explorer progress dialog (OperationStatusWindow)
                If pid = scalaPID AndAlso GetWindowClass(fgw) = "OperationStatusWindow" Then
                    e.Cancel = True
                    Return
                End If
            Catch ex As Exception
                dBug.Print($"Failed to check foreground window on menu close: {ex.Message}")
            End Try
        End If

        ' Allow close when force close flag is set
        If qlForceClose Then
            cts?.Cancel()
            Return
        End If

        If My.Computer.Keyboard.CtrlKeyDown AndAlso
                (e.CloseReason = ToolStripDropDownCloseReason.ItemClicked OrElse e.CloseReason = ToolStripDropDownCloseReason.AppFocusChange) Then
            e.Cancel = True
            Return
        End If
        cts?.Cancel()
    End Sub



    Private Sub CmsQuickLaunch_Closed(sender As ContextMenuStrip, e As ToolStripDropDownClosedEventArgs) Handles cmsQuickLaunch.Closed




        cts.Cancel() 'cancel deferred icon loading and setvis
        ctrlshift_pressed = False
        qlForceClose = False ' Reset force close flag
        ' Reset paste state when menu actually closes
        qlPasting = False
        qlPasteTargetPath = Nothing
        qlPasteOwnerItem = Nothing
        'sender.Items.Clear() 'this couses menu to stutter opening
        dBug.Print($"QL closed reason: {e.CloseReason} {caption_Mousedown}")

        'If cboAlt.SelectedIndex > 0 Then
        '    If (AltPP?.IsActive OrElse GetActiveProcessID() = scalaPID) AndAlso e.CloseReason <> ToolStripDropDownCloseReason.AppClicked Then
        '        AppActivate(scalaPID)
        '        If Me.WindowState = FormWindowState.Maximized Then 'Fixes astonia popping to front
        '            Attach(AltPP)
        '            AltPP?.Activate()
        '        End If
        '    End If
        'End If

        Task.Run(Sub()
                     Threading.Thread.Sleep(25)
                     If Not Me.Disposing Then
                         Try
                             Me.Invoke(Sub()
                                           If cmsQuickLaunch.Visible Then
                                               dBug.Print("Double QL?")
                                               Exit Sub 'this fixes closing on immediatly reopening QL, TODO: check if this affects scaling issue
                                           End If
                                           scaleFixForm?.Close()
                                           scaleFixForm = Nothing
                                       End Sub)
                         Catch ex As Exception
                             dBug.Print($"Failed to close scaleFixForm: {ex.Message}")
                         End Try
                     End If
                 End Sub)

        'Dim dummy = Task.Run(Sub()
        '                         Threading.Thread.Sleep(25)
        'If Not caption_Mousedown Then Attach(AltPP)
        '                     End Sub)

        Dim unused = RestoreClicking()

    End Sub

    Private Sub QlCtxOpen(sender As MenuItem, e As EventArgs)
        dBug.Print($"QlCtxOpen sender:{sender}")
        CloseOtherDropDowns(cmsQuickLaunch.Items, Nothing)
        cmsQuickLaunch.Close()
        OpenLnk(sender.Parent.Tag, New MouseEventArgs(MouseButtons.Left, 1, MousePosition.X, MousePosition.Y, 0))
    End Sub
    Private Sub QlCtxOpenAll(sender As MenuItem, e As EventArgs)
        dBug.Print($"QlCtxOpenAll sender:{sender}")
        Dim subitems As List(Of ToolStripMenuItem) = executableItems(DirectCast(sender.Parent.Tag, ToolStripMenuItem).DropDownItems.OfType(Of ToolStripMenuItem)).ToList

        CloseOtherDropDowns(cmsQuickLaunch.Items, Nothing)
        cmsQuickLaunch.Close()
        If subitems.Count >= 10 Then
            If Not CustomMessageBox.Show(Me, $"This will open {subitems.Count} items.{vbCrLf}Continue?",
                                        "Confirm Opening", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                dBug.Print($"Too many subitems {subitems.Count}")
                Exit Sub
            End If
        End If
        For Each ddi As ToolStripMenuItem In subitems
            OpenLnk(ddi, New MouseEventArgs(MouseButtons.Left, 1, MousePosition.X, MousePosition.Y, 0))
        Next
    End Sub

    Private Sub QlCtxRename(sender As MenuItem, e As EventArgs)
        Dim qli = CType(sender.Parent.Tag.Tag, QLInfo)
        Dim Path As String = qli.path
        Dim Name As String = qli.name

        dBug.Print($"QlCtxRename {Path} {Name}")
        RenameMethod(Path, Name)
    End Sub

    ' ReservedNames moved to QLFileOperations.ReservedNames

    Public renameOpen As Boolean
    Private Sub RenameMethod(Path As String, currentName As String, Optional isCreateFolder As Boolean = False)
        Dim title As String = If(isCreateFolder, "Create New folder", $"Rename {currentName}")
        Dim EditBox As Control = Nothing
        Task.Run(Sub()
                     Dim watch As Stopwatch = Stopwatch.StartNew()

                     Dim hndl As IntPtr
                     While watch.ElapsedMilliseconds < 2000
                         Threading.Thread.Sleep(20)
                         hndl = FindWindow(ScalaClass, title)
                         dBug.Print($"findwindow {hndl}")
                         If hndl <> IntPtr.Zero Then Exit While
                     End While

                     If hndl = IntPtr.Zero Then Exit Sub

                     If My.Settings.topmost Then SetWindowPos(hndl, SWP_HWND.TOPMOST, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove)

                     SendMessage(hndl, WM_SETICON, ICON_BIG, My.Resources.moa3.Handle)

                     Dim hEditBox As IntPtr

                     EnumChildWindows(hndl, Function(h, l)
                                                If GetWindowClass(h).Contains("EDIT") Then
                                                    hEditBox = h
                                                    Return False
                                                End If
                                                Return True
                                            End Function, IntPtr.Zero)

                     Debug.Print($"{GetWindowClass(hEditBox)}")

                     If GetWindowClass(hEditBox).Contains("EDIT") Then
                         EditBox = Control.FromHandle(hEditBox)
                         AddHandler EditBox.KeyDown, AddressOf EditBox_KeyDown
                         AddHandler EditBox.KeyPress, AddressOf EditBox_KeyPress

                         ' Select only filename without extension (exclude folders, .lnk, .url)
                         Dim isFolder As Boolean = Path.EndsWith("\")
                         Dim ext As String = System.IO.Path.GetExtension(Path).ToLower()
                         Dim isHiddenExt As Boolean = hideExt.Contains(ext)

                         If Not isFolder AndAlso Not isHiddenExt AndAlso currentName.Contains(".") Then
                             Dim lastDot As Integer = currentName.LastIndexOf(".")
                             If lastDot > 0 Then
                                 EditBoxHelper.SetSelection(hEditBox, 0, lastDot)
                             End If
                         End If
                     End If

                     Threading.Thread.Sleep(50)
                     Try
                         'AppActivate(scalaPID)
                     Catch ex As Exception
                         dBug.Print($"Failed to activate ScalA after rename dialog: {ex.Message}")
                     End Try
                     watch.Stop()
                 End Sub)

        Dim screenWA = Screen.FromPoint(MousePosition).WorkingArea
        Dim dialogLeft = Math.Min(Math.Max(screenWA.Left, MousePosition.X - 177), screenWA.Right - 370)
        Dim dialogTop = Math.Min(Math.Max(screenWA.Top, MousePosition.Y - 76), screenWA.Bottom - 152)
        scaleFixForm?.Close()
        scaleFixForm = Nothing

        renameOpen = True

        Dim toName As String = InputBox("Enter New Name", title, currentName, dialogLeft, dialogTop).TrimStart().TrimEnd(" .")

        RemoveHandler EditBox.KeyPress, AddressOf EditBox_KeyPress
        RemoveHandler EditBox.KeyDown, AddressOf EditBox_KeyDown

        renameOpen = False

        If QLFileOperations.IsReservedName(toName) Then
            CustomMessageBox.Show(Me, $"Error renaming ""{currentName}"" to ""{toName}""{vbCrLf}The specified device name is invalid.", If(isCreateFolder, "Create New folder", "Rename"), MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        dBug.Print($"Rename to {toName}")
        If toName <> "" AndAlso currentName <> toName Then
            ' Check if extension is being changed (exclude folders and hidden extensions)
            Dim isFolder As Boolean = Path.EndsWith("\")
            Dim oldExt As String = System.IO.Path.GetExtension(currentName).ToLower()
            Dim newExt As String = System.IO.Path.GetExtension(toName).ToLower()
            If Not isFolder AndAlso Not hideExt.Contains(System.IO.Path.GetExtension(Path).ToLower()) AndAlso oldExt <> newExt Then
                Dim confirmResult = CustomMessageBox.Show(Me,
                    $"If you change a file name extension, the file might become unusable.{vbCrLf}{vbCrLf}Are you sure you want to change it?",
                    "Rename", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                If confirmResult <> DialogResult.Yes Then
                    Exit Sub
                End If
            End If

            dBug.Print($"oldpath: {Path}")
            If hideExt.Contains(System.IO.Path.GetExtension(Path).ToLower) Then toName &= System.IO.Path.GetExtension(Path)
            'If Path.EndsWith("\") Then Path = System.IO.Path.GetDirectoryName(Path)
            If Path.EndsWith("\") Then Path = Path.TrimEnd("\")
            dBug.Print($"newpath: {System.IO.Path.GetDirectoryName(Path) & "\" & toName}")
            If Not MoveFileW(Path, System.IO.Path.GetDirectoryName(Path) & "\" & toName) Then
                Dim sb As New System.Text.StringBuilder(1024)
                FormatMessage(Format_Message.FORMAT_MESSAGE_FROM_SYSTEM Or Format_Message.FORMAT_MESSAGE_IGNORE_INSERTS, 0,
                              Err.LastDllError, 0, sb, sb.Capacity, Nothing)
                CustomMessageBox.Show(Me, $"Error renaming ""{currentName}"" to ""{toName}""{vbCrLf}{sb}", If(isCreateFolder, "Create New folder", "Rename"), MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                dBug.Print($"renamed to ""{toName}""")
            End If
        ElseIf toName = "" AndAlso isCreateFolder AndAlso IO.Directory.Exists(Path) Then
            IO.Directory.Delete(Path)
        End If
    End Sub

    Dim tt As ToolTip = Nothing

    Private Sub EditBox_KeyDown(sender As Object, e As KeyEventArgs)
        Dim eBox As TextBox = CType(sender, TextBox)

        If e.Control AndAlso e.KeyCode = Keys.Back Then
            EditBoxHelper.DeletePreviousWord(eBox.Handle)
            e.Handled = True
            e.SuppressKeyPress = True
        End If
    End Sub

    Dim ttCloseTask As Task = Nothing
    Dim ttCloseSw As Stopwatch = Nothing
    Private Sub EditBox_KeyPress(sender As Object, e As KeyPressEventArgs)
        Dim eBox As TextBox = CType(sender, TextBox)

        Debug.Print($"Edibox keydown {sender.GetType}")

        If tt Is Nothing Then tt = New Windows.Forms.ToolTip() With {
                                                    .IsBalloon = True,
                                                    .InitialDelay = 0,
                                                    .ShowAlways = True,
                                                    .ReshowDelay = 0}

        Dim htt As IntPtr = IntPtr.Zero
        EnumThreadWindows(ScalaThreadId, Function(hWnd, lParam)
                                             If Not IsWindowVisible(hWnd) Then Return True
                                             If GetWindowClass(hWnd).Contains("tooltips_class32") Then
                                                 htt = hWnd
                                                 Return False
                                             End If
                                             Return True
                                         End Function, IntPtr.Zero)


        If IO.Path.GetInvalidFileNameChars.Contains(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then

            e.Handled = True

            Beep()

            ttCloseSw = Stopwatch.StartNew()

            If htt = IntPtr.Zero Then ' tt not visible
                Debug.Print("showing tt")
                tt.Show($"A file name can't contain any of the following characters:{vbCrLf}{vbTab} \ / : * ? "" < > |", eBox, 5, -64)
            End If
            Dim eboxHandle = eBox.Handle
            If ttCloseTask Is Nothing Then
                ttCloseTask = Task.Run(Sub()
                                           Do
                                               Threading.Thread.Sleep(33)
                                               Debug.Print($"{GetAsyncKeyState(&H1)}")
                                               If WindowFromPoint(MousePosition) <> eboxHandle Then
                                                   If (GetAsyncKeyState(&H1) And &H8000) <> 0 Then Exit Do
                                                   If (GetAsyncKeyState(&H2) And &H8000) <> 0 Then Exit Do
                                                   If (GetAsyncKeyState(&H4) And &H8000) <> 0 Then Exit Do
                                                   If (GetAsyncKeyState(&H5) And &H8000) <> 0 Then Exit Do
                                                   If (GetAsyncKeyState(&H6) And &H8000) <> 0 Then Exit Do
                                               End If
                                           Loop While ttCloseSw.ElapsedMilliseconds <= 10000
                                           Me.Invoke(Sub()
                                                         If Not eBox?.IsDisposed Then tt.Hide(eBox)
                                                     End Sub)
                                           ttCloseTask = Nothing
                                           ttCloseSw.Reset()
                                       End Sub)
            End If
        Else
            If htt <> IntPtr.Zero Then tt.Hide(eBox)
        End If
    End Sub

    Private Sub QlCtxDelete(sender As MenuItem, e As EventArgs)
        'CloseOtherDropDowns(cmsQuickLaunch.Items, Nothing)
        'cmsQuickLaunch.Close()

        Dim tsmi As ToolStripMenuItem = sender.Parent.Tag

        Dim Path As String = CType(sender.Parent.Tag.Tag, QLInfo).path
        Dim name As String = sender.Parent.Tag.Text

        dBug.Print($"Delete {Path}")
        Dim shiftdown = My.Computer.Keyboard.ShiftKeyDown
        If Path.EndsWith("\") Then
            Dim stats = QLFileOperations.GetFolderStats(Path)
            Dim folS As String = If(stats.FolderCount = 1, "", "s")
            Dim filS As String = If(stats.FileCount = 1, "", "s")
            Dim folderContentsMessage As String = $"{vbCrLf}This folder contains {stats.FolderCount} folder{folS} and {stats.FileCount} file{filS}."
            If shiftdown OrElse CustomMessageBox.Show(Control.FromHandle(tsmi.DropDown.Handle), $"Are you sure you want to move ""{name}"" to the Recycle Bin?" & folderContentsMessage & $"{vbCrLf}Hold Shift to Permanently Delete.",
                                       "Confirm Delete", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                My.Computer.FileSystem.DeleteDirectory(Path, FileIO.UIOption.OnlyErrorDialogs,
                         If(shiftdown OrElse My.Computer.Keyboard.ShiftKeyDown, FileIO.RecycleOption.DeletePermanently, FileIO.RecycleOption.SendToRecycleBin),
                         FileIO.UICancelOption.DoNothing)
                If Not IO.Directory.Exists(Path) Then
                    If tsmi.HasDropDownItems Then
                        CloseOtherDropDowns(tsmi.GetCurrentParent().Items)
                    End If
                    tsmi.GetCurrentParent().Items.Remove(tsmi)
                    'Application.DoEvents()
                End If
            End If
        Else
            My.Computer.FileSystem.DeleteFile(Path, FileIO.UIOption.OnlyErrorDialogs,
                         If(shiftdown, FileIO.RecycleOption.DeletePermanently, FileIO.RecycleOption.SendToRecycleBin),
                         FileIO.UICancelOption.DoNothing)
            If Not IO.File.Exists(Path) Then
                tsmi.GetCurrentParent().Items.Remove(tsmi)
                'Application.DoEvents()
            End If
        End If
    End Sub

    Private Sub QlCtxProps(sender As MenuItem, e As EventArgs)
        CloseOtherDropDowns(cmsQuickLaunch.Items, Nothing)
        cmsQuickLaunch.Close()
        OpenProps(sender.Parent.Tag, New MouseEventArgs(MouseButtons.Right, 1, MousePosition.X, MousePosition.Y, 0))
    End Sub

    Private Sub QlCtxNewFolder(sender As MenuItem, e As EventArgs)
        dBug.Print($"QlCtxNewFolder sender:{sender}")
        dBug.Print($"tag:    {sender?.Tag}")

        Dim rootFolder As String = sender.Tag.Substring(0, sender.Tag.TrimEnd("\").LastIndexOf("\") + 1)
        'If rootFolder.EndsWith("\") Then
        '    rootFolder = rootFolder.Substring(0, rootFolder.TrimEnd("\").LastIndexOf("\") + 1)
        'Else
        '    rootFolder = rootFolder.Substring(0, rootFolder.LastIndexOf("\") + 1)
        'End If
        CloseOtherDropDowns(cmsQuickLaunch.Items, Nothing)
        cmsQuickLaunch.Close()
        CreateNewFolder(rootFolder)

    End Sub

    Private Sub QlCtxNewAlt(sender As MenuItem, e As EventArgs)
        dBug.Print($"newAlt: {sender.Text}")
        dBug.Print($"tag: {sender.Tag(1)}")
        CloseOtherDropDowns(cmsQuickLaunch.Items, Nothing)
        cmsQuickLaunch.Close()
        Dim rootFolder As String = sender.Tag(1)
        rootFolder = rootFolder.Substring(0, rootFolder.TrimEnd("\").LastIndexOf("\") + 1)
        Try
            CreateShortCut(New ToolStripMenuItem(sender.Text) With {.Tag = {sender.Tag(0), rootFolder}}, Nothing)
        Catch ex As Exception
            If ex.Message <> "Abort" Then Throw
        End Try
    End Sub

    Private ReadOnly folderHbm As IntPtr = FolderIcon.GetHbitmap(Color.Black)
    Private ReadOnly plusHbm As IntPtr = New Bitmap(My.Resources.Add, New Size(16, 16)).GetHbitmap(Color.Black)

    'Dim QlCtxIsOpen As Boolean = False 'to handle glitch in contextmenu when moving astonia window
    Dim QlCtxNewMenu As New MenuItem
    Dim QlCtxMenu As New ContextMenu

    Private Function executableItems(col As IEnumerable(Of ToolStripMenuItem)) As List(Of ToolStripMenuItem)
        Return col.Where(Function(itm) itm.Visible AndAlso TypeOf itm.Tag Is QLInfo AndAlso Not itm.HasDropDownItems).ToList
    End Function

    Public Shared DragCursor As Cursor
    Public Shared draggeditem As ToolStripMenuItem = Nothing

    Private Sub InvalidateRecurse(col As ToolStripItemCollection)
        col(0).Owner.Invalidate()
        For Each item In col.OfType(Of ToolStripMenuItem).Where(Function(it) it.HasDropDownItems)
            InvalidateRecurse(item.DropDownItems)
        Next
    End Sub

    Private Sub QL_DragEnter(sender As Object, e As DragEventArgs) Handles cmsQuickLaunch.DragEnter ', dropdown.dragenter
        'Debug.Print($"QL dragenter {sender}")

        CustomToolStripRenderer.insertItemAbove = Nothing
        CustomToolStripRenderer.insertItemBelow = Nothing

        InvalidateRecurse(cmsQuickLaunch.Items)

        'e.Effect = DragDropEffects.Move
        Cursor.Current = DragCursor
    End Sub

    Private Sub QL_DragOver(sender As Object, e As DragEventArgs) Handles cmsQuickLaunch.DragOver
        Dim draggedInfo As QLInfo = CType(draggeditem.Tag, QLInfo)
        Dim clientPt As Point
        Dim items As ToolStripItemCollection

        ' Get client point and items collection based on sender type
        If TypeOf sender Is ContextMenuStrip Then
            clientPt = DirectCast(sender, ContextMenuStrip).PointToClient(New Point(e.X, e.Y))
            items = DirectCast(sender, ContextMenuStrip).Items
        ElseIf TypeOf sender Is ToolStripDropDownMenu Then
            clientPt = DirectCast(sender, ToolStripDropDownMenu).PointToClient(New Point(e.X, e.Y))
            items = DirectCast(sender, ToolStripDropDownMenu).Items
        Else
            e.Effect = DragDropEffects.None
            Exit Sub
        End If

        If Not items.Contains(draggeditem) Then Exit Sub

        ' Use QLDragDropHandler to calculate insert position
        Dim pos = QLDragDropHandler.CalculateInsertPosition(items, clientPt, draggedInfo, draggeditem)

        If pos.IsValid Then
            e.Effect = DragDropEffects.Move
            CustomToolStripRenderer.insertItemAbove = pos.ItemAbove
            CustomToolStripRenderer.insertItemBelow = pos.ItemBelow
        Else
            e.Effect = DragDropEffects.None
            CustomToolStripRenderer.insertItemAbove = Nothing
            CustomToolStripRenderer.insertItemBelow = Nothing
        End If

        sender.Invalidate()
    End Sub

    Private Sub QL_QueryContinueDrag(ByVal sender As Object, ByVal e As QueryContinueDragEventArgs) 'Handles cmsQuickLaunch.QueryContinueDrag
        'Debug.Print($"QueryContinueDrag {sender} {sender.owner}")

        If WindowFromPoint(MousePosition) <> draggeditem.Owner.Handle Then
            CustomToolStripRenderer.insertItemAbove = Nothing
            CustomToolStripRenderer.insertItemBelow = Nothing
            draggeditem.Owner.Invalidate()
            Cursor.Current = Cursors.Default
        End If

    End Sub

    Private Sub QL_DragLeave(sender As Object, e As DragEventArgs) Handles cmsQuickLaunch.DragLeave
        Debug.Print($"QL dragleave {sender}") 'this doesn't fire. idk why
        'e.Effect = DragDropEffects.Move
        CustomToolStripRenderer.insertItemAbove = Nothing
        CustomToolStripRenderer.insertItemBelow = Nothing
        sender.Invalidate()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub QL_DragDrop(sender As Object, e As DragEventArgs) Handles cmsQuickLaunch.DragDrop
        Debug.Print($"dragdrop {sender} {e.Data}")
        Dim draggedInfo As QLInfo = CType(draggeditem.Tag, QLInfo)

        Dim clientPt As Point = sender.PointToClient(New Point(e.X, e.Y))
        Dim items As ToolStripItemCollection = sender.items

        ' Use QLDragDropHandler to calculate insert position
        Dim pos = QLDragDropHandler.CalculateInsertPosition(items, clientPt, draggedInfo, draggeditem)

        If Not pos.IsValid Then Exit Sub

        ' Determine folder path for sort order persistence
        Dim folderPath As String
        If TypeOf sender Is ContextMenuStrip Then
            folderPath = My.Settings.links
        Else
            folderPath = CType(sender.owneritem.Tag, QLInfo).path
        End If

        ' Use QLDragDropHandler to perform the reorder
        If QLDragDropHandler.PerformReorder(items, draggeditem, pos.Index, folderPath) Then
            Debug.Print($"writesortorder {folderPath}")
        End If
    End Sub

    Private Sub QL_Givefeedback(sender As Object, e As GiveFeedbackEventArgs)
        e.UseDefaultCursors = False
    End Sub
    Public Function CreateAlphaCursor(bmp As Bitmap, xHotSpot As Integer, yHotSpot As Integer) As Cursor
        Dim ptr As IntPtr = bmp.GetHicon
        Dim tmp As ICONINFO = New ICONINFO()
        GetIconInfo(ptr, tmp)
        tmp.xHotspot = xHotSpot
        tmp.yHotspot = yHotSpot
        tmp.fIcon = False
        ptr = CreateIconIndirect(tmp)

        DeleteObject(tmp.hbmColor)
        DeleteObject(tmp.hbmMask)

        Return New Cursor(ptr)
    End Function
    Public Function ExtractTextDarkPixels(ByVal src As Bitmap, ByVal textRect As Rectangle, ByVal bgColor As Color, targetAlpha As Single) As Bitmap

        ' Ensure format
        If src.PixelFormat <> Imaging.PixelFormat.Format32bppArgb Then
            src = src.Clone(New Rectangle(0, 0, src.Width, src.Height),
                        Imaging.PixelFormat.Format32bppArgb)
        End If

        textRect.Intersect(New Rectangle(0, 0, src.Width, src.Height))
        If textRect.IsEmpty Then Return New Bitmap(src.Width, src.Height)

        Dim mask As New Bitmap(src.Width, src.Height, Imaging.PixelFormat.Format32bppArgb)

        Dim srcData = src.LockBits(New Rectangle(0, 0, src.Width, src.Height),
                               Imaging.ImageLockMode.ReadOnly,
                               Imaging.PixelFormat.Format32bppArgb)

        Dim dstData = mask.LockBits(New Rectangle(0, 0, mask.Width, mask.Height),
                                Imaging.ImageLockMode.WriteOnly,
                                Imaging.PixelFormat.Format32bppArgb)

        ' Background luminance
        Dim bgLum As Integer = bgColor.R * 299 + bgColor.G * 587 + bgColor.B * 114
        'Dim whiteLum As Integer = 255 * 1000
        Dim effectiveBgLum As Integer = CInt(bgLum * targetAlpha * 0.9)

        For y As Integer = textRect.Top To textRect.Bottom - 1
            Dim srcRow As IntPtr = srcData.Scan0 + y * srcData.Stride
            Dim dstRow As IntPtr = dstData.Scan0 + y * dstData.Stride

            For x As Integer = textRect.Left To textRect.Right - 1
                Dim i As Integer = x * 4

                Dim b As Byte = Runtime.InteropServices.Marshal.ReadByte(srcRow, i)
                Dim g As Byte = Runtime.InteropServices.Marshal.ReadByte(srcRow, i + 1)
                Dim r As Byte = Runtime.InteropServices.Marshal.ReadByte(srcRow, i + 2)

                ' Luminance compare (no tolerance)
                Dim lum As Integer = r * 299 + g * 587 + b * 114

                If lum >= effectiveBgLum Then Continue For

                Runtime.InteropServices.Marshal.WriteByte(dstRow, i, b)
                Runtime.InteropServices.Marshal.WriteByte(dstRow, i + 1, g)
                Runtime.InteropServices.Marshal.WriteByte(dstRow, i + 2, r)
                Runtime.InteropServices.Marshal.WriteByte(dstRow, i + 3, 255)
            Next
        Next

        src.UnlockBits(srcData)
        mask.UnlockBits(dstData)

        Return mask
    End Function

    Function createcursor_v1(sender As ToolStripMenuItem, e As MouseEventArgs) As Cursor

        Dim ts As ToolStripDropDown = sender.Owner

        ' Create a bitmap of the whole dropdown
        Dim fullBmp As New Bitmap(ts.Width, ts.Height)
        ts.DrawToBitmap(fullBmp, New Rectangle(Point.Empty, ts.Size))

        ' Crop the item to remove border and arrow
        Dim iBounds = sender.Bounds
        Dim itemRect As Rectangle = New Rectangle(iBounds.X + 3, iBounds.Y + 1, iBounds.Width - 18, iBounds.Height - 2)
        Dim itemBmp As New Bitmap(itemRect.Width, itemRect.Height)
        Using g As Graphics = Graphics.FromImage(itemBmp)
            g.DrawImage(fullBmp, 0, 0, itemRect, GraphicsUnit.Pixel)
        End Using

        Dim bgColor = itemBmp.GetPixel(itemBmp.Width - 1, 0)


        ' Make a larger target bitmap (14 px wider)
        Dim extraWidth As Integer = 13
        Dim targetBmp As New Bitmap(itemBmp.Width + extraWidth, itemBmp.Height)
        Using g As Graphics = Graphics.FromImage(targetBmp)
            g.Clear(bgColor)
            ' Draw original itemBmp onto the larger bitmap
            g.DrawImageUnscaled(itemBmp, New Point(0, 0))
        End Using

        Dim textRect As Rectangle = New Rectangle(sender.ContentRectangle.X + 31, sender.ContentRectangle.Y - 1, sender.ContentRectangle.Width - 18, sender.ContentRectangle.Height)

        ' extract exact text pixels
        Dim textMask As Bitmap = ExtractTextDarkPixels(targetBmp, textRect, bgColor, 0.5)

        ' Apply transparency for drag cursor
        Dim cursorBmp As Bitmap = New Bitmap(targetBmp.AsTransparent(0.5), targetBmp.Size)

        'redraw icon fully opaque and draw text
        If sender.Image IsNot Nothing Then
            Using g As Graphics = Graphics.FromImage(cursorBmp)
                ' Draw icon at its normal location
                g.DrawImage(sender.Image, New Point(2, 2))
                g.DrawImageUnscaled(textMask, 0, 0)
            End Using
        End If

        ' Overlay the normal cursor
        Dim dragBmpWidth As Integer = cursorBmp.Width
        Dim dragBmpHeight As Integer = cursorBmp.Height
        Dim cursorWidth As Integer = Cursors.Default.Size.Width
        Dim cursorHeight As Integer = Cursors.Default.Size.Height

        Dim cursorX As Integer = e.X - 3
        Dim cursorY As Integer = e.Y - 1

        Dim requiredWidth As Integer = Math.Max(dragBmpWidth, cursorX + cursorWidth)
        Dim requiredHeight As Integer = Math.Max(dragBmpHeight, cursorY + cursorHeight)

        If requiredWidth > dragBmpWidth OrElse requiredHeight > dragBmpHeight Then
            Dim newBmp As New Bitmap(requiredWidth, requiredHeight)
            Using g As Graphics = Graphics.FromImage(newBmp)
                g.DrawImage(cursorBmp, 0, 0)
                Cursors.Default.Draw(g, New Rectangle(cursorX, cursorY, cursorWidth, cursorHeight))
            End Using
            cursorBmp.Dispose()
            cursorBmp = newBmp
        Else
            Using g As Graphics = Graphics.FromImage(cursorBmp)
                Cursors.Default.Draw(g, New Rectangle(cursorX, cursorY, cursorWidth, cursorHeight))
            End Using
        End If
        Return CreateAlphaCursor(cursorBmp, cursorX, cursorY)
    End Function

#Region "QL Context Menu Helpers"
    ''' <summary>
    ''' Creates a MenuItem for clipboard operations
    ''' </summary>
    Private Function CreateClipMenuItem(text As String, path As String, action As String) As MenuItem
        Return New MenuItem(text, AddressOf ClipAction) With {
            .Tag = New MenuTag With {.path = path, .action = action}
        }
    End Function

    ''' <summary>
    ''' Applies paste menu configuration from QLClipboardHandler
    ''' </summary>
    Private Sub ApplyPasteMenuConfig(pasteItem As MenuItem, pasteLinkItem As MenuItem,
                                     menu As ContextMenu, purgeList As List(Of IntPtr))
        Dim config = QLClipboardHandler.GetPasteMenuConfig(
            clipBoardInfo,
            AddressOf GetIconFromFile,
            My.Resources.shortcutOverlay,
            My.Resources.multiPaste)

        pasteItem.Enabled = config.PasteEnabled
        pasteLinkItem.Visible = config.PasteLinkVisible

        If config.PasteEnabled Then
            pasteItem.Text = config.PasteText
            pasteLinkItem.Text = config.PasteLinkText

            If Not String.IsNullOrEmpty(config.PasteTooltip) Then
                Dim tag As MenuTag = CType(pasteItem.Tag, MenuTag)
                tag.tooltip = config.PasteTooltip
                pasteItem.Tag = tag
            End If

            ' Apply icons
            Dim menuItems = menu.MenuItems.Cast(Of MenuItem).ToList()
            Dim cmdpos As Integer = menuItems.TakeWhile(Function(m) m.Handle <> pasteItem.Handle).Count(Function(it) it.Visible)

            If config.PasteIcon IsNot Nothing Then
                Dim pastehbm As IntPtr = config.PasteIcon.GetHbitmap(Color.Black)
                purgeList.Add(pastehbm)
                SetMenuItemBitmaps(menu.Handle, cmdpos, MF_BYPOSITION, pastehbm, Nothing)
            End If

            If config.PasteLinkVisible AndAlso config.PasteLinkIcon IsNot Nothing Then
                Dim pasteShortcutHbm As IntPtr = config.PasteLinkIcon.GetHbitmap(Color.Black)
                purgeList.Add(pasteShortcutHbm)
                SetMenuItemBitmaps(menu.Handle, cmdpos + 1, MF_BYPOSITION, pasteShortcutHbm, Nothing)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Builds the Astonia process submenu for creating shortcuts
    ''' </summary>
    Private Sub BuildAstoniaProcessSubmenu(QlCtxNewMenu As MenuItem, targetPath As String, purgeList As List(Of IntPtr))
        Dim aplist As List(Of AstoniaProcess) = AstoniaProcess.Enumerate(blackList).OrderBy(Function(ap) ap.UserName).ToList

        For Each ap As AstoniaProcess In aplist
            QlCtxNewMenu.MenuItems.Add(New MenuItem(ap.UserName, AddressOf QlCtxNewAlt) With {.Tag = {ap, targetPath}})
        Next

        If aplist.Count = 0 Then
            QlCtxNewMenu.MenuItems.Add(New MenuItem("(None)") With {.Enabled = False})
        ElseIf aplist.Count >= 2 Then
            QlCtxNewMenu.MenuItems.Add(New MenuItem("-"))
            Dim AddAllItem = New MenuItem("Add All", AddressOf QlCtxAddAll) With {.Tag = targetPath}
            QlCtxNewMenu.MenuItems.Add(AddAllItem)
            SetMenuItemBitmaps(QlCtxNewMenu.Handle, aplist.Count + 3, MF_BYPOSITION, plusHbm, Nothing)
        End If

        ' Apply icons to process items
        Dim staticCount As Integer = 2 ' Folder + separator
        Dim i As Integer = staticCount
        For Each item As MenuItem In QlCtxNewMenu.MenuItems.OfType(Of MenuItem).Skip(staticCount).Where(Function(m) m.Tag IsNot Nothing AndAlso TypeOf (m.Tag) IsNot String)
            Dim althbm As IntPtr = New Bitmap(DirectCast(item.Tag(0), AstoniaProcess).GetIcon?.ToBitmap, New Size(16, 16)).GetHbitmap(Color.Black)
            purgeList.Add(althbm)
            SetMenuItemBitmaps(QlCtxNewMenu.Handle, i, MF_BYPOSITION, althbm, Nothing)
            i += 1
        Next
    End Sub
#End Region

    Private Sub QL_MouseDown(sender As ToolStripMenuItem, e As MouseEventArgs) 'Handles cmsQuickLaunch.mousedown
        Dim qli As QLInfo = CType(sender.Tag, QLInfo)
        If e.Button = MouseButtons.Middle Then

            Dim wasChecked As Boolean = sender.Checked

            sender.Checked = False
            Application.DoEvents()

            ' Create cursor
            DragCursor = createcursor_v1(sender, e)
            Cursor.Current = DragCursor

            ' Start drag
            AddHandler sender.GiveFeedback, AddressOf QL_Givefeedback
            AddHandler sender.QueryContinueDrag, AddressOf QL_QueryContinueDrag
            AddHandler sender.Owner.DragDrop, AddressOf QL_DragDrop

            draggeditem = sender

            sender.Owner.Invalidate()
            sender.DropDown?.Close()
            Application.DoEvents()

            Dim res = sender.DoDragDrop(sender, DragDropEffects.Move Or DragDropEffects.Scroll)

            RemoveHandler sender.QueryContinueDrag, AddressOf QL_QueryContinueDrag
            RemoveHandler sender.GiveFeedback, AddressOf QL_Givefeedback
            RemoveHandler sender.Owner.DragDrop, AddressOf QL_DragDrop

            sender.Checked = wasChecked
            CustomToolStripRenderer.insertItemAbove = Nothing
            CustomToolStripRenderer.insertItemBelow = Nothing
            draggeditem = Nothing

            InvalidateRecurse(cmsQuickLaunch.Items)


            'DragCursor = CreateAlphaCursor(New Bitmap(sender.Image.AsTransparent(0.8), sender.Image.Size), sender.Image.Height \ 2, sender.Image.Height \ 2)

            'AddHandler sender.GiveFeedback, AddressOf QL_Givefeedback
            'Cursor.Current = DragCursor
            'sender.DoDragDrop(sender, DragDropEffects.Scroll Or DragDropEffects.Move)
            'RemoveHandler sender.GiveFeedback, AddressOf QL_Givefeedback
        End If
        If e.Button = MouseButtons.Right Then

            dBug.Print("QL_MouseDown")

            DestroyMenu(QlCtxMenu.Handle) ' manual destroy old since we recreate everytime, might not be needed if we dispose
            QlCtxMenu.Dispose()           ' but better to err on the side of caution and do it anyways.
            DestroyMenu(QlCtxNewMenu.Handle)
            QlCtxNewMenu.Dispose()

            Dim rcDrop As RECT
            Dim rcCurr As RECT
            Dim DropIsLeft As Boolean
            If sender.HasDropDown Then
                Dim paren = If(TryCast(sender.GetCurrentParent(), ToolStripDropDown)?.Handle, IntPtr.Zero)
                GetWindowRect(paren, rcCurr)
                GetWindowRect(sender.DropDown.Handle, rcDrop)
                DropIsLeft = rcDrop.left < rcCurr.left
            End If

            Dim newFolderItem As New MenuItem("Folder", AddressOf QlCtxNewFolder)
            QlCtxNewMenu = New MenuItem($"New", {
                                             newFolderItem,
                                             New MenuItem("-")})

            Dim path As String = qli.path

            Dim OpenItem = New MenuItem("Open", AddressOf QlCtxOpen) With {.DefaultItem = True}

            Dim cutItem = CreateClipMenuItem("Cut", path, "Cut")
            Dim copyItem = CreateClipMenuItem("Copy", path, "Copy")
            Dim pasteItem = CreateClipMenuItem("Paste", path, "Paste")
            Dim pasteLinkItem = CreateClipMenuItem("Paste Shortcut", path, "PasteLink")

            Application.DoEvents() ' allow submenu to fully render
            Dim executableSubItems = executableItems(sender.DropDownItems.OfType(Of ToolStripMenuItem))
            Dim openallTag As New MenuTag
            openallTag.tooltip = String.Join(vbCrLf, executableSubItems.Take(10).Select(Of String)(Function(it) CType(it.Tag, QLInfo).name))
            If executableSubItems.Count > 10 Then openallTag.tooltip &= $"{vbCrLf}<And {executableSubItems.Count - 10} More>"

#If Not DEBUG Then
            QlCtxMenu = New ContextMenu({
            OpenItem,
                New MenuItem($"Open All ({executableSubItems.Count}){vbTab}-->", AddressOf QlCtxOpenAll) With {
                            .Visible = (path.EndsWith("\") OrElse (My.Settings.QLResolveLnk AndAlso path.ToLower.EndsWith(".lnk"))) AndAlso
                                        executableSubItems.Count > 0,
                            .Tag = openallTag},
                New MenuItem("-"),
            cutItem, copyItem, pasteItem, pasteLinkItem,
                New MenuItem("-"),
                New MenuItem("Delete", AddressOf QlCtxDelete),
                New MenuItem("Rename", AddressOf QlCtxRename),
                New MenuItem("-"),
                New MenuItem("Properties", AddressOf QlCtxProps),
                New MenuItem("-"),
            QlCtxNewMenu})
#Else
            QlCtxMenu = New ContextMenu({
            OpenItem,
                New MenuItem(If(DropIsLeft, $"<--    Open All ({executableSubItems.Count})", $"Open All ({executableSubItems.Count}){vbTab}-->"), AddressOf QlCtxOpenAll) With {
                            .Visible = (path.EndsWith("\") OrElse (My.Settings.QLResolveLnk AndAlso path.ToLower.EndsWith(".lnk"))) AndAlso
                                        executableSubItems.Count > 0,
                            .Tag = openallTag},
                New MenuItem("-"),
            cutItem, copyItem, pasteItem, pasteLinkItem,
                New MenuItem("-"),
                New MenuItem("Delete", AddressOf QlCtxDelete),
                New MenuItem("Rename", AddressOf QlCtxRename),
                New MenuItem("-"),
                New MenuItem("Properties", AddressOf QlCtxProps),
                New MenuItem("-"),
                New MenuItem("Dump Info", AddressOf dBug.dumpItemInfo),
                New MenuItem("-"),
            QlCtxNewMenu})
#End If

            clipBoardInfo = GetClipboardFilesAndAction()
            Dim purgeList As New List(Of IntPtr)

            ' Configure paste menu items using helper
            ApplyPasteMenuConfig(pasteItem, pasteLinkItem, QlCtxMenu, purgeList)

            Dim name As String = qli.name

            QlCtxMenu.Tag = sender

            'this in conjunction with paintevent handles sender getting unselected when opening contextmenu
            'QLMenuItem_Paint
            sender.Select()
            QLCtxMenuOpenedOn = sender

            newFolderItem.Tag = path

            ' Build Astonia process submenu
            BuildAstoniaProcessSubmenu(QlCtxNewMenu, path, purgeList)

            ' Configure Open item with truncated name
            OpenItem.Text = name.Replace("&", "&&").CapWithEllipsis(25)
            If name.Length > 25 Then
                OpenItem.Tag = New MenuTag With {.tooltip = name}
            End If

            ' Apply menu item icons
            Dim hbm = IntPtr.Zero
            If sender.Image IsNot Nothing Then
                hbm = DirectCast(sender.Image, Bitmap).GetHbitmap(Color.Black)
                SetMenuItemBitmaps(QlCtxMenu.Handle, 0, MF_BYPOSITION, hbm, Nothing)
            End If

            SetMenuItemBitmaps(QlCtxNewMenu.Handle, 0, MF_BYPOSITION, folderHbm, Nothing)
            Dim visibleMenuCount = QlCtxMenu.MenuItems.Cast(Of MenuItem).Count(Function(it) it.Visible)
            SetMenuItemBitmaps(QlCtxMenu.Handle, visibleMenuCount - 1, MF_BYPOSITION, plusHbm, Nothing)

            dBug.Print($"purgeList.Count {purgeList.Count}")

            'MenuToolTip.InitializeTooltip(sender.Owner.Handle)
            CustomToolTip.InitializeTooltip()

            For Each item As MenuItem In QlCtxMenu.MenuItems
                ' Skip separators
                If item.Text = "-" Then Continue For

                ' Show tooltip when item is selected
                AddHandler item.Select, Sub(it As MenuItem, ev As EventArgs)

                                            Debug.Print("select menuitem")

                                            ' Find the popup menu window
                                            'Dim hwndMenu = FindWindow("#32768", Nothing) 'todo replace with enumwnd and a check for owner as scalahandle/scalaPid?
                                            Dim hwndMenu As IntPtr = IntPtr.Zero
                                            EnumThreadWindows(ScalaThreadId, Function(hWnd, lParam)
                                                                                 If Not IsWindowVisible(hWnd) Then Return True
                                                                                 If GetWindowClass(hWnd) = "#32768" Then
                                                                                     hwndMenu = hWnd
                                                                                     Return False ' stop enumeration
                                                                                 End If
                                                                                 Return True
                                                                             End Function, IntPtr.Zero)
                                            If hwndMenu = IntPtr.Zero Then
                                                dBug.Print("Menu window not found.")
                                                Exit Sub
                                            End If

                                            Dim text As String = "" 'tooltips with empty text are not shown
                                            If TypeOf it.Tag Is MenuTag Then
                                                text = CType(it.Tag, MenuTag).tooltip
                                            End If

                                            ' Find highlight rect
                                            Dim hMenu = QlCtxMenu.Handle
                                            Dim rc As RECT
                                            For i = 0 To GetMenuItemCount(hMenu) - 1
                                                Dim state = GetMenuState(hMenu, i, MF_BYPOSITION)
                                                If (state And MF_HILITE) <> 0 Then
                                                    GetMenuItemRect(IntPtr.Zero, hMenu, i, rc)
                                                    Exit For
                                                End If
                                            Next
                                            If it Is pasteItem Then
                                                CustomToolTip.ShowTooltipWithDelay(text, hwndMenu, rc, positionCallback:=Function() New Point(rc.left + 65, rc.top + If(clipBoardInfo.Files.Count = 1, 1, 35)))
                                            Else
                                                CustomToolTip.ShowTooltipWithDelay(text, hwndMenu, rc)
                                            End If

                                        End Sub

            Next


            If sender.HasDropDown Then

                tpmParam.rcExclude = rcDrop
                If Not DropIsLeft Then
                    tpmParam.rcExclude.right = Integer.MaxValue
                Else
                    tpmParam.rcExclude.left = -Integer.MaxValue
                End If
            Else
                tpmParam.rcExclude = New RECT
            End If

            TrackPopupMenuEx(QlCtxMenu.Handle, TPM_RECURSE, MousePosition.X, MousePosition.Y, ScalaHandle, tpmParam)

            CustomToolTip.HideTooltip()

            'sender.BackColor = Color.Transparent
            QLCtxMenuOpenedOn = Nothing

            'free up recources
            DeleteObject(hbm)
            For Each item As IntPtr In purgeList
                DeleteObject(item)
            Next


        ElseIf e.Button = MouseButtons.Left AndAlso Not qli.path.EndsWith("\") AndAlso Not (My.Settings.QLResolveLnk AndAlso qli.path.ToLower.EndsWith(".lnk") AndAlso qli.target.EndsWith("\"c)) Then 'do not process click on dirs as they are handled by doubleclick
            dBug.Print($"clicked Not a dir {My.Settings.QLResolveLnk} {CType(sender.Tag, QLInfo).target}")
            Task.Run(Sub() OpenLnk(sender, e))
            'cmsQuickLaunch.Close(ToolStripDropDownCloseReason.ItemClicked)
#If DEBUG Then
            If Not Debugger.IsAttached AndAlso chkDebug.Checked Then
                dBug.Print("Exception Thrown")
                Throw New Exception 'todo find out why this exception autocloses/gets hidden? when debugger not attached.
            End If

            ' not in OpenLnk
            ' not closerrordialog
            ' not WM_CANCELMODE
#End If
        End If
    End Sub
    Dim tpmParam As New TPMPARAMS With {.cbSize = Marshal.SizeOf(Of TPMPARAMS)}
    Private Sub ClipAction(sender As Object, e As EventArgs)
        Dim tgt As String = sender.tag.path
        Dim act As String = sender.tag.action

        Dim dropdown As Object

        If TypeOf sender Is MenuItem Then
            Dim tsmi As ToolStripMenuItem = CType(sender, MenuItem).Parent.Tag
            If tsmi.HasDropDownItems Then
                dropdown = TopMostToolStripMenuItem.DropDown
            Else
                dropdown = tsmi.GetCurrentParent()
            End If
        Else
            dropdown = Nothing
        End If


        If {"Paste", "PasteLink"}.Contains(act) Then
            ' Show confirmation when pasting folders
            'Dim folderCount As Integer = clipBoardInfo.Files.Where(Function(f) IO.Directory.Exists(f)).Count()
            'If folderCount > 0 Then
            '    Dim fileCount As Integer = clipBoardInfo.Files.Where(Function(f) IO.File.Exists(f)).Count()
            '    Dim msg As String
            '    If fileCount > 0 AndAlso folderCount > 0 Then
            '        msg = $"About to paste {fileCount} file(s) and {folderCount} folder(s).{vbCrLf}{vbCrLf}Are you sure you want to continue?"
            '    Else
            '        msg = $"About to paste {folderCount} folder(s).{vbCrLf}{vbCrLf}Are you sure you want to continue?"
            '    End If
            '    Dim confirmResult = CustomMessageBox.Show(Me, msg, "Confirm Paste", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
            '    If confirmResult <> DialogResult.OK Then
            '        Exit Sub
            '    End If
            'End If
            tgt = IO.Path.GetDirectoryName(tgt.TrimEnd("\"c))

            ' Set paste tracking state instead of closing menu
            qlPasting = True
            qlPasteTargetPath = tgt

            ' Find the owner menu item (folder) for later refresh
            If TypeOf sender Is ToolStripMenuItem Then
                Dim menuItem = CType(sender, ToolStripMenuItem)
                qlPasteOwnerItem = TryCast(menuItem.OwnerItem, ToolStripMenuItem)
            End If

            Task.Run(Sub()
                         Dim watch As Stopwatch = Stopwatch.StartNew()
                         Dim hndl As IntPtr = IntPtr.Zero

                         Dim found As Boolean = False
                         Dim wndProc As EnumWindowsProc = Function(hwnd, lParam)
                                                              If GetWindowClass(hwnd) = "OperationStatusWindow" Then
                                                                  Dim pid As Integer
                                                                  GetWindowThreadProcessId(hwnd, pid)
                                                                  If pid = scalaPID Then
                                                                      hndl = hwnd
                                                                      found = True
                                                                      Return False ' stop enumeration
                                                                  End If
                                                              End If
                                                              Return True ' continue enumeration
                                                          End Function

                         ' Wait for progress dialog to appear (up to 5 seconds)
                         While watch.ElapsedMilliseconds < 5000 AndAlso Not found
                             EnumWindows(wndProc, IntPtr.Zero)
                             If found Then Exit While
                             Threading.Thread.Sleep(50)
                         End While

                         If found Then
                             dBug.Print($"enumwindow {hndl} ""{GetWindowText(hndl)}""")
                             SetWindowPos(hndl, SWP_HWND.TOPMOST, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.DoNotActivate)
                             'SetWindowLong(hndl, GWL_HWNDPARENT, topWindHandle)
                             'If My.Settings.topmost Then SetWindowPos(hndl, SWP_HWND.TOPMOST, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.DoNotActivate)

                             ' Wait for progress dialog to close (paste complete)
                             While IsWindow(hndl)
                                 Threading.Thread.Sleep(100)
                             End While
                             dBug.Print("Paste progress dialog closed")
                         Else
                             ' No progress dialog appeared (fast paste), wait a bit for filesystem
                             Threading.Thread.Sleep(200)
                         End If

                         ' Refresh the menu after paste completes
                         If Not Me.Disposing AndAlso Not Me.IsDisposed Then
                             Try
                                 Dim newItems = ParseDir(tgt, True) ' Your function returning updated items
                                 Me.Invoke(Sub()
                                               Dim idx = 0
                                               Dim insertIndex = 0
                                               Dim origitems As ToolStripItemCollection = dropdown.items
                                               For Each item As ToolStripMenuItem In newItems.Cast(Of ToolStripItem).OfType(Of ToolStripMenuItem).Where(Function(it) TypeOf it.Tag Is QLInfo).ToList
                                                   Dim qli As QLInfo = item.Tag
                                                   If qli.path <> CType(origitems(idx).Tag, QLInfo).path Then
                                                       origitems.Insert(insertIndex, newItems(idx))
                                                       'newItems.RemoveAt(idx)
                                                   End If
                                                   idx += 1
                                                   insertIndex += 1
                                               Next
                                           End Sub)

                             Catch ex As Exception
                                 dBug.Print($"Failed to refresh QL after paste: {ex.Message}")
                             End Try
                         End If
                         watch.Stop()
                     End Sub)
        End If

        dBug.Print($"Clipaction {act} ""{tgt}""")
        InvokeExplorerVerb(tgt, act, ScalaHandle)
        If act.StartsWith("Paste") Then

        Else

            'SetFileDropListWithEffect(tgt.TrimEnd("\"c), act = "Cut") 'this is brokne. leads to silent crash

            pasteTSItem.Tag = New MenuTag With {.path = IO.Path.Combine(tgt, "Empty"), .action = "Paste"}
            pasteLinkTSItem.Tag = New MenuTag With {.path = IO.Path.Combine(tgt, "Empty"), .action = "PasteLink"}

            If Not (IO.File.Exists(tgt) OrElse IO.Directory.Exists(tgt)) Then
                Debug.Print("file/dir missing")
                PasteSep.Visible = False
                pasteTSItem.Visible = False
                pasteLinkTSItem.Visible = False
            Else
                Dim nm As String = IO.Path.GetFileName(tgt.TrimEnd("\"c))
                If String.IsNullOrEmpty(nm) Then nm = tgt
                If hideExt.Contains(IO.Path.GetExtension(nm)) Then
                    nm = IO.Path.GetFileNameWithoutExtension(nm)
                End If
                pasteTSItem.Text = $"{If(act = "Cut", "Move", "Paste")} ""{nm.CapWithEllipsis(16)}"""
                If nm.Length > 16 Then
                    pasteTSItem.ToolTipText = nm
                Else
                    pasteTSItem.ToolTipText = String.Empty
                End If

                If tgt.ToLower.EndsWith(".lnk") Then
                    pasteLinkTSItem.Visible = False
                Else
                    pasteLinkTSItem.Text = "Paste Shortcut"
                End If

                Task.Run(Sub()
                             Dim img = GetIconFromFile(tgt, False, True)
                             Me.Invoke(Sub()
                                           pasteTSItem.Image = img
                                           If tgt.ToLower.EndsWith(".lnk") Then
                                               pasteTSItem.Image = img.addOverlay(My.Resources.shortcutOverlay)
                                           Else
                                               pasteLinkTSItem.Image = img.addOverlay(My.Resources.shortcutOverlay)
                                           End If
                                       End Sub)
                         End Sub)
            End If
        End If
    End Sub

    Private Sub UpdateMenuChecks(menu As ToolStripItemCollection, itemSet As HashSet(Of String))
        For Each menuItm As ToolStripMenuItem In menu.OfType(Of ToolStripMenuItem).Where(Function(mi) TypeOf mi.Tag Is QLInfo)
            menuItm.Checked = itemSet.Contains(menuItm.Tag?.path.TrimEnd("\"c))
            menuItm.Invalidate() ')imageRect)
            If menuItm.DropDown?.Visible AndAlso menuItm.HasDropDownItems Then
                UpdateMenuChecks(menuItm.DropDownItems, itemSet)
            End If
        Next
    End Sub

    Dim restartCM As ContextMenu = New ContextMenu({New MenuItem("Restart w/o Closing", AddressOf restartWoClosing)})

    Private Sub restartWoClosing(sender As MenuItem, e As EventArgs)
        dBug.Print("Restart w/o Closing")
        dBug.Print($"parent: {sender.Parent}")
        dBug.Print($"tag: {sender.Parent.Tag}")
        'get owneritem?

        CType(sender.Parent.Tag, AstoniaProcess).restart(False)

    End Sub

    Dim restartBitmapInstalled As Boolean = False
    Private Sub restart_Mousedown(sender As ToolStripMenuItem, e As MouseEventArgs) Handles ReLaunchToolStripMenuItem.MouseDown
        If e.Button.HasFlag(MouseButtons.Right) Then

            If Not restartBitmapInstalled Then

                Using bmp As Bitmap = New Bitmap(My.Resources.Sync, New Size(16, 16))
                    Dim restartHbm As IntPtr = bmp.GetHbitmap(Color.Black)
                    SetMenuItemBitmaps(restartCM.Handle, 0, MF_BYPOSITION, restartHbm, Nothing)
                    ' The HBITMAP handle is created once and used for the lifetime of the app.
                    ' It is intentionally not released, as the OS will clean up GDI resources on process exit.
                    ' Releasing it here will make it not show up in menu.
                End Using

                restartBitmapInstalled = True

            End If

            sender.Select()
            sender.BackColor = Color.FromArgb(&HFFB5D7F3) 'this to fix a glitch where sender gets unselected
            restartCM.Tag = sender.Tag
            TrackPopupMenuEx(restartCM.Handle, TPM_RECURSE, MousePosition.X, MousePosition.Y, ScalaHandle, IntPtr.Zero)
            sender.BackColor = Color.Transparent

            cmsAlt.Close()

        End If
    End Sub

    Private Sub OpenProps(ByVal sender As ToolStripMenuItem, ByVal e As MouseEventArgs) 'Handles smenu.MouseUp, item.MouseUp
        dBug.Print($"OpenProps {sender.Tag} {sender.GetType}")
        If e.Button = MouseButtons.Right Then
            Dim pth As String = CType(sender.Tag, QLInfo).path.TrimEnd("\")
            Dim sei As New SHELLEXECUTEINFO With {
               .cbSize = System.Runtime.InteropServices.Marshal.SizeOf(GetType(SHELLEXECUTEINFO)),
               .lpVerb = "properties",
               .lpFile = pth,
               .nShow = SW_SHOW,
               .fMask = SEE_MASK_INVOKEIDLIST,
               .hwnd = ScalaHandle
            }
            CloseOtherDropDowns(cmsQuickLaunch.Items, Nothing)
            cmsQuickLaunch.Close()
            Task.Run(Sub()
                         ShellExecuteEx(sei) 'open properties
                         'set properties to topmost
                         Dim watch As Stopwatch = Stopwatch.StartNew()
                         'Dim WindowName As String = pth.ToLower.Substring(pth.LastIndexOf("\") + 1).Replace(".url", "").Replace(".lnk", "") ' & " Properties"
                         Dim itemName As String = IO.Path.GetFileName(pth)
                         If hideExt.Contains(IO.Path.GetExtension(itemName).ToLower) Then itemName = IO.Path.GetFileNameWithoutExtension(itemName)

                         Dim wn1 As String = itemName & " "
                         Dim wn2 As String = " " & itemName

                         Debug.Print($"wn: {itemName} {pth}")
                         'findwindow ignores case
                         Dim hndl As IntPtr = IntPtr.Zero
                         While watch.ElapsedMilliseconds < 10000 AndAlso hndl = IntPtr.Zero
                             Threading.Thread.Sleep(50)
                             '    'todo replace with enumwnd so we can be locale agnostic
                             '    hndl = FindWindow("#32770", WindowName)
                             '    dBug.Print($"findwindow {hndl}")
                             '    If hndl <> IntPtr.Zero Then Exit While

                             EnumWindows(Function(hw As IntPtr, lparam As IntPtr)

                                             If Not IsWindowVisible(hw) Then Return True
                                             If Not IsWindowEnabled(hw) Then Return True

                                             If GetWindowLong(hw, GWL_HWNDPARENT) = ScalaHandle Then Return True

                                             Dim pid As Integer
                                             GetWindowThreadProcessId(hw, pid)
                                             If pid <> scalaPID Then Return True

                                             If GetWindowClass(hw) <> "#32770" Then Return True

                                             dBug.Print($"fwT: {GetWindowClass(hw)} ""{GetWindowText(hw)}""")

                                             Dim title = GetWindowText(hw)
                                             If Not (title.StartsWith(wn1) OrElse title.EndsWith(wn2)) Then Return True

                                             Dim child = FindWindowEx(hw, IntPtr.Zero, "#32770", Nothing)
                                             Dim foundNoWn As Boolean = True
                                             EnumChildWindows(child, Function(c As IntPtr, l As IntPtr)
                                                                         Debug.Print($"c ""{GetWindowText(c)}"" {GetWindowClass(c)}")
                                                                         If GetWindowClass(c) = "Edit" Then
                                                                             Dim tex = GetWindowText(c)
                                                                             If tex = itemName Then
                                                                                 foundNoWn = False
                                                                                 Return False
                                                                             End If
                                                                         End If
                                                                         Return True
                                                                     End Function, IntPtr.Zero)
                                             If Not foundNoWn Then hndl = hw
                                             Return hndl = IntPtr.Zero

                                         End Function, IntPtr.Zero)

                         End While

                         SetWindowPos(hndl, If(My.Settings.topmost, SWP_HWND.TOPMOST, SWP_HWND.TOP), 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove)
                         SetWindowPos(ScalaHandle, hndl, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove)
                         SetWindowLong(hndl, GWL_HWNDPARENT, ScalaHandle)
                         If My.Settings.topmost Then SetWindowPos(hndl, SWP_HWND.TOPMOST, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove)
                         watch.Stop()
                     End Sub)
        End If
    End Sub

    Private Sub DblClickDir(ByVal sender As ToolStripMenuItem, ByVal e As EventArgs) 'Handles smenu.DoubleClick
        CloseOtherDropDowns(cmsQuickLaunch.Items, Nothing)
        cmsQuickLaunch.Close()

        Dim qli As QLInfo = sender.Tag
        If qli.path.ToLower.EndsWith(".lnk") Then
            OpenLnk(sender, New MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0))
            Exit Sub
        End If

        Dim pp As New Process With {.StartInfo = New ProcessStartInfo With {.FileName = CType(sender.Tag, QLInfo).path}}

        Try
            pp.Start()
        Catch ex As Exception
            Debug.Print(ex.Message)
            'Debugger.Break()
        End Try
    End Sub

    Dim explorerPath As String = Environment.ExpandEnvironmentVariables("%windir%\explorer.exe").Trim.ToLowerInvariant()
    Private waitCursorTimer As Stopwatch
    Private Sub OpenLnk(ByVal sender As ToolStripMenuItem, ByVal e As System.Windows.Forms.MouseEventArgs) 'handles item.MouseDown

        Dim qli = sender.Tag
        Dim pth As String = qli.path
        dBug.Print("openLnk: " & pth)
        If e Is Nothing Then Exit Sub
        If e.Button = MouseButtons.Right Then
            'OpenProps(sender, e)
            Exit Sub
        End If

        'Dim sli = New ShellLinkInfo(qli.path)
        'Dim result = sli.Resolve(1000UI << 16 And &H4UI) 'this doesn't yield desired results
        'dBug.Print($"resolvelink result {result}") 'result is 0 even for broken links
        'If Not String.IsNullOrEmpty(sli.TargetPath) AndAlso Not (IO.File.Exists(sli.LinkFile) AndAlso (IO.File.Exists(sli.TargetPath) OrElse IO.Directory.Exists(sli.TargetPath))) Then
        '    dBug.Print($"error lnk not found ""{sli.TargetPath}""")
        '    Exit Sub
        'End If
        Dim sli As New ShellLinkInfo(pth)
        'find broken explorer dialog and press ok
        If QLItemHandler.IsBrokenDirectoryLink(sli) Then
            EnumWindows(Function(hwnd As IntPtr, lParam As IntPtr)
                            If Not IsWindowVisible(hwnd) Then Return True
                            If GetWindowClass(hwnd) <> "#32770" Then Return True

                            Dim owner = GetWindowLong(hwnd, GWL_HWNDPARENT)
                            If owner = IntPtr.Zero Then Return True

                            Dim ownId As Integer
                            GetWindowThreadProcessId(owner, ownId)

                            If Not IsWindowEnabled(owner) AndAlso ProcessPath(ownId).ToLowerInvariant() = explorerPath AndAlso GetWindowClass(owner) = "CabinetWClass" Then
                                'find Ok button and press it
                                dBug.Print("childwinds")
                                Dim numbut As Integer = 0
                                Dim lastbut As IntPtr = IntPtr.Zero
                                EnumChildWindows(hwnd, Function(h As IntPtr, l As IntPtr)
                                                           dBug.Print($"{h} ""{GetWindowClass(h)}"" ""{GetWindowText(h)}""")
                                                           If GetWindowClass(h) = "Button" Then 'todo check windowtext for locale string IDOK
                                                               numbut += 1
                                                               lastbut = h
                                                           End If
                                                           Return True
                                                       End Function, IntPtr.Zero)
                                dBug.Print($"-- {numbut} --")
                                If numbut = 1 AndAlso lastbut <> IntPtr.Zero Then
                                    Dim tid = GetWindowThreadProcessId(hwnd, Nothing)
                                    AttachThreadInput(tid, GetCurrentThread(), True)
                                    SetForegroundWindow(hwnd)
                                    SetActiveWindow(hwnd)
                                    Threading.Thread.Sleep(50)
                                    SendMessage(lastbut, BM_CLICK, IntPtr.Zero, IntPtr.Zero)
                                    Threading.Thread.Sleep(50)
                                End If
                            End If

                            Return True
                        End Function, IntPtr.Zero)
        End If

        waitCursorTimer = Stopwatch.StartNew
        Me.Invoke(Sub()
                      Me.Cursor = Cursors.WaitCursor
                      setMenuCursor(cmsQuickLaunch, Cursors.WaitCursor)
                  End Sub)

        ' Launch using QLItemHandler
        Dim launchInfo = QLItemHandler.PrepareLaunch(qli)
        Dim launchResult = QLItemHandler.LaunchItem(launchInfo)

        If launchResult.Success Then
            MRU.Add(pth)
        Else
            dBug.Print($"pp.start {launchResult.ErrorMessage}")
            Me.Invoke(Sub()
                          CustomMessageBox.Show(Control.FromHandle(sender.Owner.Handle), $"Failed to launch{vbCrLf}{pth}{vbCrLf}{launchResult.ErrorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                      End Sub)
        End If

        ' Reset cursor after delay
        Task.Run(Sub()
                     Dim timeout As Integer = 123
                     Threading.Thread.Sleep(timeout)
                     If waitCursorTimer.ElapsedMilliseconds >= timeout Then
                         Me.BeginInvoke(Sub()
                                            Cursor = Cursors.Arrow
                                            setMenuCursor(cmsQuickLaunch, Cursors.Arrow)
                                        End Sub)
                     End If
                 End Sub)

        'Exit Sub
        'hoist error dialogs to front/owned.
        If swErrorHoist?.ElapsedMilliseconds < 9000 Then
            swErrorHoist = Stopwatch.StartNew
        Else
            Task.Run(Sub()
                         swErrorHoist = Stopwatch.StartNew()
                         Dim targetCmdPath As String = Environment.ExpandEnvironmentVariables("%windir%\system32\cmd.exe").Trim().ToLowerInvariant()

                         Do
                             Threading.Thread.Sleep(200)
                             Dim hwndList As New List(Of IntPtr)
                             Dim HideList As New List(Of IntPtr)
                             Dim centList As New List(Of IntPtr)
                             EnumWindows(Function(hwnd As IntPtr, lParam As IntPtr)

                                             If Not IsWindowVisible(hwnd) Then Return True
                                             If GetWindowClass(hwnd) <> "#32770" Then Return True

                                             Dim owner As IntPtr = GetWindowLong(hwnd, GWL_HWNDPARENT)
                                             If owner = IntPtr.Zero OrElse owner = ScalaHandle OrElse IsWindowEnabled(owner) Then Return True

                                             Dim ownId As UInteger
                                             GetWindowThreadProcessId(owner, ownId)

                                             If ProcessPath(ownId).ToLowerInvariant() = explorerPath AndAlso GetWindowClass(owner) = "CabinetWClass" Then
                                                 If GetWindowClass(FindWindowEx(owner, IntPtr.Zero, Nothing, Nothing)) = "ShellTabWindowClass" Then
                                                     dBug.Print($"Hidelist add: ""{GetWindowText(owner)}""")
                                                     HideList.Add(owner)
                                                     hwndList.Add(hwnd)
                                                     centList.Add(hwnd)
                                                 End If
                                                 Return True
                                             End If

                                             Dim parentpid As Integer = ownId
                                             Dim rootPid As Integer = ownId
                                             While parentpid <> -1 AndAlso parentpid <> 0 AndAlso swErrorHoist.ElapsedMilliseconds <= 10000
                                                 parentpid = GetParentPid(parentpid)
                                                 If parentpid <> -1 Then rootPid = parentpid
                                                 If rootPid = scalaPID Then Exit While
                                             End While

                                             If rootPid = scalaPID Then
                                                 If ProcessPath(ownId).ToLowerInvariant() = targetCmdPath Then
                                                     hwndList.Add(hwnd)
                                                 End If
                                             End If

                                             Return True
                                         End Function, IntPtr.Zero)

                             For Each hwn In hwndList
                                 SetWindowLong(hwn, GWL_HWNDPARENT, ScalaHandle)
                                 If My.Settings.topmost Then
                                     SetWindowPos(hwn, SWP_HWND.TOPMOST, -1, -1, -1, -1, SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.IgnoreResize)
                                 End If
                                 AllowSetForegroundWindow(ASFW_ANY)
                                 SetForegroundWindow(ScalaHandle)
                             Next
                             For Each hwn In HideList
                                 SetWindowLong(hwn, GWL_EXSTYLE, GetWindowLong(hwn, GWL_EXSTYLE) Or WindowStylesEx.WS_EX_TOOLWINDOW Or WindowStylesEx.WS_EX_LAYERED)
                                 SetLayeredWindowAttributes(hwn, 0, 0, LWA_ALPHA)
                             Next
                             For Each hwn In centList
                                 Dim rcW As New RECT
                                 GetClientRect(hwn, rcW)
                                 Dim rcSw As New RECT
                                 GetWindowRect(ScalaHandle, rcSw)
                                 Dim rcSc As New RECT
                                 GetClientRect(ScalaHandle, rcSc)
                                 SetWindowPos(hwn, SWP_HWND.TOP, rcSw.left + (rcSc.right - rcW.right) / 2, rcSw.top + (rcSc.bottom - rcW.bottom) / 2, 0, 0, SetWindowPosFlags.IgnoreResize)
                             Next

                         Loop While swErrorHoist.ElapsedMilliseconds <= 10000
                     End Sub)
        End If
    End Sub
    Dim swErrorHoist As Stopwatch
    Public Function GetParentPid(pid As Integer) As Integer
        Try
            Using proc As Process = Process.GetProcessById(pid)
                Dim pbi As New PROCESS_BASIC_INFORMATION()
                Dim retLen As Integer = 0
                Dim status As Integer = NtQueryInformationProcess(proc.Handle, 0, pbi, Marshal.SizeOf(pbi), retLen)
                If status = 0 Then
                    Return pbi.InheritedFromUniqueProcessId.ToInt32()
                End If
            End Using
        Catch ex As Exception
            Debug.Print($"NtQueryInformationProcess failed for {pid}: {ex.Message}")
        End Try
        Return -1
    End Function

    Private Sub setMenuCursor(menu As ToolStripDropDownMenu, Curs As Cursor)
        menu.Cursor = Curs
        For Each it As ToolStripMenuItem In menu.Items.OfType(Of ToolStripMenuItem).Where(Function(i) Not i.IsDisposed)
            If it.HasDropDown AndAlso it.DropDown.Visible AndAlso Not it.IsDisposed Then
                setMenuCursor(it.DropDown, Curs)
            End If
        Next
    End Sub

    Private Sub ChangeLinksDir()
        dBug.Print("changeLinksDir")
        Me.TopMost = False
        'Using fb As New FolderBrowserDialog

        Try
            Dim fp As New FolderPicker With {
                .Title = "Select Folder Containing Your Shortcuts - ScalA",
                .Multiselect = False,
                .InputPath = IO.Path.GetFullPath(My.Settings.links)}
            If fp.ShowDialog(Me) = True Then
                If fp.ResultPath = System.IO.Path.GetPathRoot(fp.ResultPath) AndAlso
                        CustomMessageBox.Show(Me, "Warning: Selecting a root path is not recommended" & vbCrLf &
                                        $"Are you sure you want to use {fp.ResultPath}?", "Warning",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.No Then Throw New Exception("dummy")
                If My.Settings.links <> fp.ResultPath Then
                    My.Settings.links = fp.ResultPath
                    UpdateWatchers(My.Settings.links)
                End If
            End If
        Catch
        Finally
            Me.TopMost = My.Settings.topmost
        End Try
    End Sub


    Private Sub cmsQuit_Opened(sender As ContextMenuStrip, e As EventArgs) Handles cmsQuit.Opened
        Dim hwnd As IntPtr = sender.Handle

        Dim rcC As RECT 'clientrect
        GetClientRect(hwnd, rcC)

        'Dim rcW As RECT
        'GetWindowRect(hwnd, rcW)

        Dim QBloc As Point = pnlButtons.PointToScreen(btnQuit.Location)

        Dim loc As Point = New Point(QBloc.X + btnQuit.Width - rcC.right, QBloc.Y + btnQuit.Height)

        ' move menu to correct loc
        SetWindowPos(hwnd, SWP_HWND.TOPMOST, loc.X, loc.Y, -1, -1, SetWindowPosFlags.IgnoreZOrder Or SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate)

        sender.Opacity = 1
    End Sub

    Private Sub ActiveOverviewToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles ActiveOverviewToolStripMenuItem.Click
        dBug.Print($"active overview alt {sender.Checked}")
        My.Settings.gameOnOverview = sender.Checked
        AButton.ActiveOverview = sender.Checked

        If Not sender.Checked Then
            AstoniaProcess.RestorePos(True)
            AltPP = Nothing
            Detach(True)
        End If
    End Sub

    Private Sub SidebarModeToolStripMenuItem_MouseEnter(sender As ToolStripMenuItem, e As EventArgs) Handles SidebarModeToolStripMenuItem.MouseEnter, ActiveOverviewToolStripMenuItem.MouseEnter
        Dim rc = New RECT(sender.Owner.RectangleToScreen(sender.Bounds))
        Debug.Print($"sidebarmh {rc}")
        CustomToolTip.ShowTooltipWithDelay(sender.ToolTipText, cmsAlt.Handle, rc, (sender.ToolTipText.Count(Function(c) c = vbLf) + 5) * 1000)
    End Sub
End Class

' QLSort module moved to QL\QLSort.vb
' QLInfo and MenuTag structures moved to QL\QLTypes.vb

Module ImageExtension
    ''' <summary>
    ''' Returns an image with desired opacity.
    ''' </summary>
    ''' <param name="img">source image</param>
    ''' <param name="opacity">desired opacity</param>
    ''' <returns>Nothing if an exception occurs or opacity is less than 0</returns>
    <Runtime.CompilerServices.Extension()>
    Public Function AsTransparent(ByVal img As Image, ByVal opacity As Single) As Image
        If opacity >= 1.0F Then Return img
        If opacity <= 0.0F Then Return Nothing
        Try
            Dim bmp = New Bitmap(img.Width, img.Height)
            Using gfx = Graphics.FromImage(bmp), attr As New Imaging.ImageAttributes()
                attr.SetColorMatrix(New Imaging.ColorMatrix With {.Matrix33 = opacity})
                gfx.DrawImage(img, New Rectangle(0, 0, bmp.Width, bmp.Height),
                              0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attr)
            End Using
            Return bmp
        Catch ex As Exception
            dBug.Print($"Exception in AsTransparent {ex.Message}")
            Return Nothing
        End Try
    End Function

    Private IconLock As New Object
    <Runtime.CompilerServices.Extension>
    Function addOverlay(bm As Image, over As Bitmap, Optional clone As Boolean = True) As Image
        If bm Is Nothing Then Return Nothing
        If over Is Nothing Then Return bm

        'Dim lockObj = iconLocks.GetOrAdd(bmp, Function(__) New Object())
        'SyncLock lockObj
        Dim bmp As Bitmap
        Try
            bmp = If(clone, bm.Clone, bm)
            Using g As Graphics = Graphics.FromImage(bmp)
                g.DrawImage(over, New Rectangle(New Point, bmp.Size))
            End Using
        Catch ex As Exception
            SyncLock IconLock
                bmp = If(clone, bm.Clone, bm)
                Using g As Graphics = Graphics.FromImage(bmp)
                    g.DrawImage(over, New Rectangle(New Point, bmp.Size))
                End Using
            End SyncLock
        End Try
        Return bmp
    End Function
End Module
Module EnumerationHelpers

    ''' <summary>
    ''' Wraps an IEnumerable in a throttled producer-consumer queue.
    ''' </summary>
    <Runtime.CompilerServices.Extension>
    Public Function AsThrottled(Of T)(source As IEnumerable(Of T), bufferSize As Integer) As IEnumerable(Of T)

        Dim bc As New BlockingCollection(Of T)(bufferSize)

        Task.Run(Sub()
                     Try
                         For Each item In source
                             bc.Add(item)
                         Next
                     Finally
                         bc.CompleteAdding()
                     End Try
                 End Sub)

        Return bc.GetConsumingEnumerable()
    End Function
End Module