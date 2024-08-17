Imports System.Collections.Concurrent

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
                               Debug.Print("busy error dialog closer")
                           End While
                       End Sub, ct)
        btnQuit.PerformClick()
    End Sub

    Private Sub CloseAllOverviewToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles CloseAllOverviewToolStripMenuItem.Click
        Dim procs = pnlOverview.Controls().OfType(Of AButton).Select(Of AstoniaProcess)(Function(ab) ab.AP)
        Parallel.ForEach(procs, Sub(ap) ap?.CloseOrKill())
    End Sub

    Private Sub cmsQuit_Opening(sender As ContextMenuStrip, e As System.ComponentModel.CancelEventArgs) Handles cmsQuit.Opening
        If cboAlt.SelectedIndex = 0 Then
            CloseAstoniaToolStripMenuItem.Visible = False
            CloseBothToolStripMenuItem.Visible = False
            CloseAllOverviewToolStripMenuItem.Visible = pnlOverview.Controls.OfType(Of AButton).Any(Function(ab) ab.AP IsNot Nothing)
        Else
            CloseAstoniaToolStripMenuItem.Text = $"Close {AltPP.Name}"
            CloseBothToolStripMenuItem.Text = $"Close {AltPP.Name} && ScalA"
            CloseAstoniaToolStripMenuItem.Visible = True
            CloseBothToolStripMenuItem.Visible = True
            CloseAllOverviewToolStripMenuItem.Visible = False
        End If
        Dim aps = AstoniaProcess.Enumerate(False).ToList
        If aps.Count = 0 OrElse (aps.Count = 1 AndAlso cboAlt.SelectedIndex > 0) Then
            CloseAllSeparator.Visible = False
            CloseAllToolStripMenuItem.Visible = False
        Else
            CloseAllSeparator.Visible = True
            CloseAllToolStripMenuItem.Visible = True
        End If

        If AstoniaProcess.EnumSomeone.Any() Then
            CloseAllIdleToolStripMenuItem.Visible = True
        Else
            CloseAllIdleToolStripMenuItem.Visible = False
        End If

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
        Debug.Print("SelectToolStrip: " & pp.Name)
        SelectAlt(pp)
    End Sub

    Private Sub TopMostToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles TopMostToolStripMenuItem.Click
        Dim pp As AstoniaProcess = DirectCast(sender.Tag, AstoniaProcess)
        If pp Is Nothing Then Exit Sub
        Debug.Print("Topmost " & Not sender.Checked)
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
        Detach(False)
        UntrapMouse(MouseButtons.Right)
        AppActivate(scalaPID) 'fix right click drag bug

        Dim pp As AstoniaProcess = DirectCast(sender.SourceControl, AButton).AP

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
        SelectToolStripMenuItem.Text = "Select " & pp.Name
        SelectToolStripMenuItem.Image = pp.GetIcon?.ToBitmap
        SelectToolStripMenuItem.Tag = pp

        TopMostToolStripMenuItem.Checked = pp.TopMost()
        TopMostToolStripMenuItem.Tag = pp

        SortSubToolStripMenuItem.Tag = pp

        MoveToolStripMenuItem.Tag = pp

        SidebarModeToolStripMenuItem.Checked = SidebarScalA IsNot Nothing

        If sender.Items.Contains(closeAllIdleTSMI) Then
            sender.Items.Remove(closeAllIdleTSMI)
        End If

        sender.Items.RemoveAt(sender.Items.Count - 1)
        sender.Items.Add("Close " & pp.Name, My.Resources.F12, AddressOf CloseToolStripMenuItem_Click).Tag = pp

        Dim other As String = If(pp.Name = "Someone", "Other ", "")
        Dim somecount As Integer = AstoniaProcess.EnumSomeone.Count(Function(p) p.Name = "Someone")
        Debug.Print($"somecount {somecount}")
        If somecount > 0 AndAlso Not (other = "Other " AndAlso somecount = 1) Then
            closeAllIdleTSMI = sender.Items.Add($"Close All {other}Someone", My.Resources.moreF12, AddressOf CloseAllIdle_Click)
            closeAllIdleTSMI.Tag = pp
        End If
    End Sub
    Private Sub MoveToolStripMenuItem_DropDownOpening(sender As ToolStripMenuItem, e As EventArgs) Handles MoveToolStripMenuItem.DropDownOpening
        Dim lst = EnumOtherOverviews.OrderBy(Function(p) p.ProcessName, nsSorter).ToList
        MoveToolStripMenuItem.DropDownItems.Clear()
        MoveToolStripMenuItem.DropDownItems.Add(NoOtherOverviewsToolStripMenuItem)
        NoOtherOverviewsToolStripMenuItem.Visible = lst.Count = 0

        MoveToolStripMenuItem.DropDownItems.Add(KeepToolStripMenuItem)
        MoveToolStripMenuItem.DropDownItems.Add(New ToolStripSeparator With {.Visible = lst.Count > 0})
        KeepToolStripMenuItem.Visible = lst.Count > 0
        KeepToolStripMenuItem.CheckState = If(My.Settings.KeepOnOverview, CheckState.Indeterminate, CheckState.Unchecked)

        Debug.Print(sender.Tag.name)
        Dim Rang = lst.Select(Function(p) New ToolStripMenuItem(p.ProcessName, Nothing, AddressOf MoveTo_Click) With {.Tag = New Tuple(Of AstoniaProcess, Process)(sender.Tag, p)}).ToArray
        MoveToolStripMenuItem.DropDownItems.AddRange(Rang)
    End Sub

    Private Sub KeepToolStripMenuItem_Mousedown(sender As ToolStripMenuItem, e As MouseEventArgs) Handles KeepToolStripMenuItem.MouseDown
        Debug.Print($"{sender.Owner}")
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
    Public SidebarScalA As Process = Nothing
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
        Debug.Print($"moving {ap.Name} to {sp.Id} {sp.ProcessName}")
        IPC.AddToWhitelistOrRemoveFromBL(sp.Id, ap.Id)

        If KeepToolStripMenuItem.Checked Then Exit Sub

        Dim i = 0
        Do
            Await Task.Delay(50)
            i += 1
        Loop Until IPC.AddToWhitelistOrRemoveFromBL(sp.Id) = 0 OrElse i >= 20
        If IPC.AddToWhitelistOrRemoveFromBL(sp.Id) = 0 Then
            If My.Settings.Whitelist Then
                topSortList.RemoveAll(Function(it) it = ap.Name)
                botSortList.RemoveAll(Function(it) it = ap.Name)
            Else
                blackList.Add(ap.Name)
            End If
            My.Settings.topSort = String.Join(vbCrLf, blackList.Concat(topSortList))
            My.Settings.botSort = String.Join(vbCrLf, blackList.Concat(botSortList))

            apSorter = New AstoniaProcessSorter(topSortList, botSortList)

            Await Task.Delay(100)
            Detach(True)
        End If
    End Sub
    Private Sub CmsAlt_Closed(sender As Object, e As ToolStripDropDownClosedEventArgs) Handles cmsAlt.Closed
        AButton.ActiveOverview = My.Settings.gameOnOverview
        Me.TopMost = My.Settings.topmost
    End Sub

    Private Sub CmsAlt_Opened(sender As Object, e As EventArgs) Handles cmsAlt.Opened
        AButton.ActiveOverview = False
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

        Dim AltName As String = DirectCast(sender.Tag, AstoniaProcess).Name

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
        Dim AltName As String = DirectCast(sender.OwnerItem.Tag, AstoniaProcess).Name
        Debug.Print($"Apply sorting {AltName} {sender.Tag}")

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

    Friend ReadOnly iconCache As New ConcurrentDictionary(Of String, Bitmap)

    Private Function GetIcon(ByVal PathName As String, transp As Boolean) As Bitmap

        If PathName Is Nothing Then Return Nothing

        Try
            Return iconCache.GetOrAdd(PathName,
                   Function()

                       Debug.Print($"iconCahceMiss: {PathName}")

                       Dim bm As Bitmap
                       Dim fi As New SHFILEINFOW
                       Dim ico As Icon

                       If PathName.EndsWith("\") Then
                           SHGetFileInfoW(PathName, 0, fi, System.Runtime.InteropServices.Marshal.SizeOf(fi), SHGFI_ICON Or SHGFI_SMALLICON)
                           If fi.hIcon = IntPtr.Zero Then
                               Debug.Print("hIcon empty: " & Runtime.InteropServices.Marshal.GetLastWin32Error)
                               Throw New Exception
                           End If
                           ico = Icon.FromHandle(fi.hIcon)
                           bm = ico.ToBitmap
                           DestroyIcon(fi.hIcon)
                       Else 'not a folder
                           Dim list As IntPtr = SHGetFileInfoW(PathName, 0, fi, System.Runtime.InteropServices.Marshal.SizeOf(fi), SHGFI_SYSICONINDEX Or SHGFI_SMALLICON)
                           Dim hIcon As IntPtr = ImageList_GetIcon(list, fi.iIcon, 0)
                           ImageList_Destroy(list)
                           If hIcon = IntPtr.Zero Then
                               Debug.Print("iconlist empty: " & Runtime.InteropServices.Marshal.GetLastWin32Error)
                               Throw New Exception
                           End If
                           ico = Icon.FromHandle(hIcon)
                           bm = ico.ToBitmap
                           DestroyIcon(hIcon)
                       End If
                       DestroyIcon(ico.Handle)
                       ico.Dispose()

                       Return bm
                   End Function).AsTransparent(If(transp, If(My.Settings.DarkMode, 0.4, 0.5), 1))
        Catch
            Debug.Print("GetIcon Exception")
            Return Nothing
        End Try
    End Function

    Private Shared ReadOnly nsSorter As IComparer(Of String) = New NaturalStringSorter
    Private Shared ReadOnly extensions As String() = {".exe", ".jar", ".lnk", ".url", ".txt"}
    Private Shared ReadOnly hideExt As String() = {".lnk", ".url"}
    Private Function ParseDir(pth As String) As List(Of ToolStripItem)
        Dim menuItems As New List(Of ToolStripItem)
        Dim isEmpty As Boolean = True
        'Const ICONTIMEOUT = 50
        Const TOTALTIMEOUT = 2000
        Dim timedout As Boolean = False

        Dim watch As Stopwatch = Stopwatch.StartNew()

        Dim Dirs As New List(Of ToolStripItem)
        Try
            For Each fullDirs As String In System.IO.Directory.EnumerateDirectories(pth)

                Dim attr As System.IO.FileAttributes = New System.IO.DirectoryInfo(fullDirs).Attributes
                Dim hidden As Boolean = False
                If attr.HasFlag(System.IO.FileAttributes.Hidden) OrElse attr.HasFlag(System.IO.FileAttributes.System) Then hidden = True

                Dim smenu As New ToolStripMenuItem(System.IO.Path.GetFileName(fullDirs)) With {.Tag = {fullDirs & "\", hidden}, .Visible = Not hidden OrElse ctrlshift_pressed OrElse My.Settings.QLShowHidden}

                smenu.DropDownItems.Add("(Dummy)").Enabled = False
                smenu.DoubleClickEnabled = True

                AddHandler smenu.MouseDown, AddressOf QL_MouseDown
                AddHandler smenu.DoubleClick, AddressOf DblClickDir
                AddHandler smenu.DropDownOpening, AddressOf ParseSubDir
                'AddHandler smenu.DropDownOpened, AddressOf deferredIconLoading
                AddHandler smenu.DropDown.Closing, AddressOf CmsQuickLaunchDropDown_Closing

                Dirs.Add(smenu)
                isEmpty = False
                If watch.ElapsedMilliseconds > TOTALTIMEOUT Then
                    timedout = True
                    Exit For
                End If
            Next

        Catch
            menuItems.Add(New ToolStripMenuItem("<Access Denied>") With {.Enabled = False})
            Return menuItems
        End Try

        Dim Files As New List(Of ToolStripItem)
        For Each fullLink As String In System.IO.Directory.EnumerateFiles(pth) _
                                       .Where(Function(p) extensions.Contains(System.IO.Path.GetExtension(p).ToLower))
            'Debug.Print(System.IO.Path.GetFileName(fullLink))

            Dim attr As System.IO.FileAttributes = New System.IO.DirectoryInfo(fullLink).Attributes
            Dim hidden As Boolean = False
            If attr.HasFlag(System.IO.FileAttributes.Hidden) OrElse attr.HasFlag(System.IO.FileAttributes.System) Then hidden = True

            'don't add self to list
            If System.IO.Path.GetFileName(fullLink) = System.IO.Path.GetFileName(Environment.GetCommandLineArgs(0)) Then Continue For
            If fullLink = Environment.GetCommandLineArgs(0) Then Continue For

            Dim linkName As String
            If hideExt.Contains(System.IO.Path.GetExtension(fullLink).ToLower) Then
                linkName = System.IO.Path.GetFileNameWithoutExtension(fullLink)
            Else
                linkName = System.IO.Path.GetFileName(fullLink)
            End If

            Dim item As New ToolStripMenuItem(linkName) With {.Tag = {fullLink, hidden}, .Visible = Not hidden OrElse ctrlshift_pressed OrElse My.Settings.QLShowHidden}
            AddHandler item.MouseDown, AddressOf QL_MouseDown
            'AddHandler item.MouseEnter, AddressOf QL_MouseEnter
            'AddHandler item.MouseLeave, AddressOf QL_MouseLeave

            Files.Add(item)
            isEmpty = False
            If watch.ElapsedMilliseconds > TOTALTIMEOUT Then
                timedout = True
                Exit For
            End If
        Next

        menuItems = Dirs.OrderBy(Function(d) d.Text, nsSorter).Concat(
                   Files.OrderBy(Function(f) f.Tag(0), nsSorter)).ToList

        If timedout Then
            menuItems.Add(New ToolStripMenuItem("<TimedOut>") With {.Enabled = False})
        End If

        If isEmpty Then menuItems.Add(New ToolStripMenuItem("(Empty)") With {.Enabled = False})

        menuItems.Add(New ToolStripSeparator With {.Visible = isEmpty OrElse ctrlshift_pressed})
        Dim addShortcutMenu As New ToolStripMenuItem("New", My.Resources.Add) With {.Tag = pth, .Visible = isEmpty OrElse ctrlshift_pressed}
        addShortcutMenu.DropDownItems.Add("(Dummy)").Enabled = False
        AddHandler addShortcutMenu.DropDownOpening, AddressOf AddShortcutMenu_DropDownOpening
        menuItems.Add(addShortcutMenu)

        cts?.Dispose()
        cts = New Threading.CancellationTokenSource
        cantok = cts.Token
        DeferredIconLoading(Dirs.Concat(Files), cantok)

        Debug.Print($"parsing ""{pth}"" took {watch.ElapsedMilliseconds} ms")
        watch.Stop()
        Return menuItems
    End Function
    Private ctrlshift_pressed As Boolean = False
    Sub Application_Idle(sender As Object, e As EventArgs)
        If cmsQuickLaunch.Visible AndAlso My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown AndAlso Not ctrlshift_pressed Then
            ctrlshift_pressed = True
            SetVisRecurse(cmsQuickLaunch.Items)
        End If
    End Sub

    Sub SetVisRecurse(col As ToolStripItemCollection)
        Task.Run(Sub() Parallel.ForEach(col.Cast(Of ToolStripItem),
            Sub(it)
                Me.BeginInvoke(Sub() it.Visible = True)
                If TypeOf it Is ToolStripMenuItem AndAlso DirectCast(it, ToolStripMenuItem).HasDropDownItems Then
                    SetVisRecurse(DirectCast(it, ToolStripMenuItem).DropDownItems)
                End If
            End Sub))
    End Sub

    Private Sub CmsQuickLaunchDropDown_Closing(sender As Object, e As ToolStripDropDownClosingEventArgs) Handles cmsQuickLaunch.Closing
        If My.Computer.Keyboard.CtrlKeyDown AndAlso
                (e.CloseReason = ToolStripDropDownCloseReason.ItemClicked OrElse e.CloseReason = ToolStripDropDownCloseReason.AppFocusChange) Then
            e.Cancel = True
            Return
        End If
        cts?.Cancel()
    End Sub
    Private Sub DeferredIconLoading(items As IEnumerable(Of ToolStripItem), ct As Threading.CancellationToken)
        Try
            Task.Run(Sub() Parallel.ForEach(items.TakeWhile(Function(__) Not ct.IsCancellationRequested),
                                          Sub(it As ToolStripItem)
                                              Dim ico = GetIcon(it.Tag(0), it.Tag(1))
                                              Me.BeginInvoke(Sub() it.Image = ico)
                                          End Sub), ct)
        Catch ex As System.Threading.Tasks.TaskCanceledException
            Debug.Print("deferredIconLoading Task canceled")
        Catch
            Debug.Print("deferredIconLoading general exception")
        End Try
    End Sub
    Private Sub ParseSubDir(sender As ToolStripMenuItem, e As EventArgs) ' Handles DummyToolStripMenuItem.DropDownOpening
        'Debug.Print($"ParseSubDir QlCtxIsOpen:{QlCtxIsOpen}")
        'If QlCtxIsOpen Then Exit Sub
        sender.DropDownItems.Clear()
        sender.DropDownItems.AddRange(ParseDir(sender.Tag(0)).ToArray)
    End Sub
    Private foldericon = GetIcon(FileIO.SpecialDirectories.Temp, False)
    Private Sub AddShortcutMenu_DropDownOpening(sender As ToolStripMenuItem, e As EventArgs) 'Handles addShortcutMenu.DropDownOpening
        Debug.Print("addshortcut.sendertag:" & sender.Tag)
        sender.DropDownItems.Clear()

        sender.DropDownItems.Add(New ToolStripMenuItem("Folder", foldericon, AddressOf Ql_NewFolder) With {.Tag = sender.Tag})
        sender.DropDownItems.Add(New ToolStripSeparator())

        For Each alt As AstoniaProcess In AstoniaProcess.Enumerate(blackList).OrderBy(Function(ap) ap.Name)
            sender.DropDownItems.Add(New ToolStripMenuItem(alt.Name, alt.GetIcon?.ToBitmap, AddressOf CreateShortCut) With {
                                               .Tag = {alt, sender.Tag}}) ' sender.tag is parent menu location
        Next
        If sender.DropDownItems.Count = 2 Then
            sender.DropDownItems.Add("(None)").Enabled = False
        ElseIf sender.DropDownItems.Count > 3 Then
            sender.DropDownItems.Add(New ToolStripSeparator())
            sender.DropDownItems.Add(New ToolStripMenuItem("Add All", My.Resources.Add, AddressOf AddAllShortcuts) With {.Tag = sender.Tag})
        End If

    End Sub

    Private Sub CreateNewFolder(newpath As String)
        Debug.Print($"NewFolder:{newpath}")

        Dim rootFolder As String = newpath

        Debug.Print($"rootFolder: {rootFolder}")

        Dim newfolderPath = IO.Path.Combine(rootFolder, "New Folder")

        Dim i As Integer = 2
        While IO.Directory.Exists(newfolderPath)
            newfolderPath = IO.Path.Combine(rootFolder, $"New Folder ({i})")
            i += 1
        End While

        Debug.Print($"newfolderpath: {newfolderPath}")

        Try
            IO.Directory.CreateDirectory(newfolderPath)
        Catch ex As Exception

        End Try

        cmsQuickLaunch.Close()

        If IO.Directory.Exists(newfolderPath) Then
            RenameMethod(newfolderPath, newfolderPath.Substring(newfolderPath.TrimEnd("\").LastIndexOf("\") + 1).TrimEnd("\"))
        End If

    End Sub

    Private Sub Ql_NewFolder(sender As ToolStripMenuItem, e As EventArgs)
        Debug.Print($"QlCtxNewFolder sender:{sender}")
        Debug.Print($"tag:    {sender?.Tag}")

        CreateNewFolder(sender.Tag)

    End Sub
    Private Shared ReadOnly pipe As Char() = {"|"c}
    Private Sub CreateShortCut(sender As ToolStripMenuItem, e As EventArgs)

        Debug.Print($"CreateShortCut")
        Debug.Print($"sender.text {sender.Text}")

        Dim alt As AstoniaProcess = DirectCast(sender.Tag(0), AstoniaProcess)
        Dim ShortCutPath As String = sender.Tag(1)
        Dim ShortCutName As String = sender.Text
        If ShortCutName = "Someone" Then Exit Sub
        Dim ShortCutLink As String = ShortCutPath & "\" & ShortCutName & ".lnk"

        If System.IO.File.Exists(ShortCutLink) Then
            Select Case CustomMessageBox.Show(Me, $"""{alt.Name}"" already exists. Overwrite?",
                           "Notice", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                Case DialogResult.Yes
                    'do nothing
                Case DialogResult.No
                    Exit Sub
                Case Else
                    Debug.Print($"Cancel {sender.Tag} ""{sender.OwnerItem}"" {sender.Text}")
                    If sender.OwnerItem?.Text <> "New" Then Throw New Exception("Abort")
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
        Debug.Print("cmdline:" & arguments)
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

        Debug.Print("cmdline mang:" & arguments)

        Dim exepath As String = ""
        Try
            exepath = mos("ExecutablePath")
        Catch

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
            Debug.Print($"except: {ex.Message}")
            Debug.Print($"{ShortCutLink}")
        End Try

        sender.DropDown.Close()

    End Sub

    Private Sub AddAllShortcuts(sender As Object, e As EventArgs)
        Debug.Print($"Add All ShortCuts")
        Debug.Print($"sender.tag {sender.tag}")

        Dim list As List(Of AstoniaProcess) = AstoniaProcess.Enumerate(blackList).OrderBy(Function(ap) ap.Name).ToList

        cmsQuickLaunch.Close()

        Dim path As String = sender.tag
        If Not path.EndsWith("\") Then
            path = path.Substring(0, path.LastIndexOf("\") + 1)
            Debug.Print($"sanitized: {path}")
        End If

        For Each ap As AstoniaProcess In list
            Debug.Print($"adding {ap.Name}")
            Try
                CreateShortCut(New ToolStripMenuItem(ap.Name) With {.Tag = {ap, path}}, Nothing)
            Catch ex As Exception
                If ex.Message = "Abort" Then
                    Exit Sub
                Else
                    Throw
                End If
            End Try
        Next

    End Sub
    Private Sub CmsQuickLaunch_Opening(sender As ContextMenuStrip, e As System.ComponentModel.CancelEventArgs) Handles cmsQuickLaunch.Opening
        UntrapMouse(MouseButtons.Right)
        Try
            AppActivate(scalaPID) 'fix right click drag bug
        Catch
        End Try
        If Not My.Settings.MinMin OrElse Not AltPP?.isSDL Then Detach(False)
        ttMain.Hide(cboAlt)
        ttMain.Hide(btnStart)
        pbZoom.Visible = False
        AButton.ActiveOverview = False
        If My.Computer.Keyboard.ShiftKeyDown AndAlso Not My.Computer.Keyboard.CtrlKeyDown Then
            Debug.Print("ShowSysMenu ")
            Dim pt As Point = sender.PointToClient(MousePosition)
            Me.ShowSysMenu(sender, New MouseEventArgs(MouseButtons.Right, 1, pt.X, pt.Y, 0))
            e.Cancel = True
            Exit Sub
        End If


        'tmrTick.Interval = 1000
        sender.Items.Clear()

        If Not FileIO.FileSystem.DirectoryExists(My.Settings.links) Then My.Settings.links = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & "\ScalA"

        If Not FileIO.FileSystem.DirectoryExists(My.Settings.links) Then
            System.IO.Directory.CreateDirectory(My.Settings.links)
            System.IO.Directory.CreateDirectory(My.Settings.links & "\Example Folder")
        End If

        If My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown Then ctrlshift_pressed = True

        sender.Items.AddRange(ParseDir(IO.Path.GetFullPath(My.Settings.links)).ToArray)

        'If My.Computer.Keyboard.CtrlKeyDown Then
        '    sender.Items.Add(New ToolStripSeparator())
        '    sender.Items.Add("Select Folder", My.Resources.gear_wheel, AddressOf ChangeLinksDir)
        'End If

        If AstoniaProcess.EnumSomeone.Any() Then
            If sender.SourceControl Is Nothing Then 'called from trayicon
                sender.Items.Insert(0, New ToolStripSeparator())
                sender.Items.Insert(0, New ToolStripMenuItem("Close All Someone", My.Resources.moreF12, AddressOf CloseAllIdle_Click))
                closeAllAtBottom = False
            Else
                sender.Items.Add(New ToolStripSeparator())
                sender.Items.Add("Close All Someone", My.Resources.moreF12, AddressOf CloseAllIdle_Click)
                closeAllAtBottom = True
            End If
        End If

        If sender.SourceControl Is btnStart AndAlso My.User.IsInRole(ApplicationServices.BuiltInRole.Administrator) Then
            sender.Items.Add(New ToolStripSeparator())
            sender.Items.Add("UnElevate", btnStart.Image, AddressOf UnelevateSelf).ToolTipText = $"Drop Admin Rights{vbCrLf}Use this If you can't use ctrl, alt and/or shift."
        End If

        If watchers.Count = 0 Then InitWatchers()

    End Sub

    Private closeAllAtBottom As Boolean = True




    Dim cts As New Threading.CancellationTokenSource
    Dim cantok As Threading.CancellationToken = cts.Token
    Private Sub CmsQuickLaunch_Closed(sender As ContextMenuStrip, e As ToolStripDropDownClosedEventArgs) Handles cmsQuickLaunch.Closed
        cts.Cancel() 'cancel deferred icon loading and setvis
        ctrlshift_pressed = False
        'sender.Items.Clear() 'this couses menu to stutter opening
        Debug.Print("cmsQuickLaunch closed reason:" & e.CloseReason.ToString)
        'If AltPP IsNot Nothing AndAlso
        '    e.CloseReason <> ToolStripDropDownCloseReason.AppClicked AndAlso
        '    e.CloseReason <> ToolStripDropDownCloseReason.ItemClicked Then
        '    'these couse ghosting of menu and blanking of zoom when closed by reopening when reason = appclicked
        '    'SetWindowPos(AltPP.MainWindowHandle, -2, -1, -1, -1, -1, SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.IgnoreResize)
        '    If sender.SourceControl IsNot Nothing AndAlso AltPP.Id <> 0 AndAlso Not renameOpen Then
        '        Try
        '            SetWindowLong(ScalaHandle, GWL_HWNDPARENT, AltPP.MainWindowHandle)
        '            AppActivate(AltPP.Id)
        '        Catch
        '        End Try
        '    End If
        'End If
        If cboAlt.SelectedIndex > 0 Then
            If (AltPP?.IsActive OrElse GetActiveProcessID() = scalaPID) AndAlso e.CloseReason <> ToolStripDropDownCloseReason.AppClicked Then
                AppActivate(scalaPID) 'Fixes astona popping to front
                Attach(AltPP)
                AltPP?.Activate()
            End If
        End If
        Dim dummy = Task.Run(Sub()
                                 Threading.Thread.Sleep(50)
                                 Attach(AltPP)
                             End Sub)
        Dim unused = RestoreClicking()
    End Sub

    Private Sub QlCtxOpen(sender As MenuItem, e As EventArgs)
        Debug.Print($"QlCtxOpen sender:{sender}")
        cmsQuickLaunch.Close()
        OpenLnk(sender.Parent.Tag, New MouseEventArgs(MouseButtons.Left, 1, MousePosition.X, MousePosition.Y, 0))
    End Sub
    Private Sub QlCtxOpenAll(sender As MenuItem, e As EventArgs)
        Debug.Print($"QlCtxOpenAll sender:{sender}")
        Dim subitems As List(Of ToolStripMenuItem) = DirectCast(sender.Tag, ToolStripMenuItem).
                DropDownItems.OfType(Of ToolStripMenuItem).
                Where(Function(it) extensions.Contains(IO.Path.GetExtension(it.Tag(0))) AndAlso it.Visible).ToList()

        cmsQuickLaunch.Close()
        If subitems.Count >= 10 Then
            If Not CustomMessageBox.Show(Me, $"This will open {subitems.Count} items.{vbCrLf}Continue?",
                                        "Confirm Opening", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Debug.Print("Too many subitems")
                Exit Sub
            End If
        End If
        For Each ddi As ToolStripMenuItem In subitems
            Debug.Print($"{ddi.Tag(0)}")
            OpenLnk(ddi, New MouseEventArgs(MouseButtons.Left, 1, MousePosition.X, MousePosition.Y, 0))
        Next
    End Sub

    Private Sub QlCtxRename(sender As MenuItem, e As EventArgs)
        Dim Path As String = sender.Parent.Tag.Tag(0)
        Dim Name As String = sender.Parent.Tag.text

        Debug.Print($"QlCtxRename {Path} {Name}")

        RenameMethod(Path, Name)
    End Sub
    Private renameOpen As Boolean
    Private Sub RenameMethod(Path As String, currentName As String)
        Dim title As String = $"Rename {currentName}"
        Dim scalaClass = GetWindowClass(ScalaHandle)
        Task.Run(Sub()
                     Dim watch As Stopwatch = Stopwatch.StartNew()

                     Dim hndl As IntPtr
                     While watch.ElapsedMilliseconds < 2000
                         Threading.Thread.Sleep(20)
                         hndl = FindWindow(scalaClass, title)
                         Debug.Print($"findwindow {hndl}")
                         If hndl <> IntPtr.Zero Then Exit While
                     End While
                     SetWindowPos(hndl, SWP_HWND.TOPMOST, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove)
                     Threading.Thread.Sleep(50)
                     Try
                         AppActivate(scalaPID)
                     Catch
                     End Try
                     watch.Stop()
                 End Sub)
        renameOpen = True
        Dim screenWA = Screen.FromPoint(MousePosition).WorkingArea
        Dim dialogLeft = Math.Min(Math.Max(screenWA.Left, MousePosition.X - 177), screenWA.Right - 370)
        Dim dialogTop = Math.Min(Math.Max(screenWA.Top, MousePosition.Y - 76), screenWA.Bottom - 152)
        Dim toName As String = InputBox("Enter New Name", title, currentName, dialogLeft, dialogTop).TrimEnd
        renameOpen = False
        Debug.Print($"Rename to {toName}")
        If toName <> "" AndAlso currentName <> toName Then
            Debug.Print($"oldpath: {Path}")
            If hideExt.Contains(System.IO.Path.GetExtension(Path).ToLower) Then toName &= System.IO.Path.GetExtension(Path)
            'If Path.EndsWith("\") Then Path = System.IO.Path.GetDirectoryName(Path)
            If Path.EndsWith("\") Then Path = Path.TrimEnd("\")
            Debug.Print($"newpath: {System.IO.Path.GetDirectoryName(Path) & "\" & toName}")
            If Not MoveFileW(Path, System.IO.Path.GetDirectoryName(Path) & "\" & toName) Then
                Dim sb As New System.Text.StringBuilder(1024)
                FormatMessage(Format_Message.FORMAT_MESSAGE_FROM_SYSTEM Or Format_Message.FORMAT_MESSAGE_IGNORE_INSERTS, 0,
                              Err.LastDllError, 0, sb, sb.Capacity, Nothing)
                CustomMessageBox.Show(Me, $"Error renaming ""{currentName}"" to ""{toName}""{vbCrLf}{sb}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                Debug.Print($"renamed to ""{toName}""")
            End If
        End If
    End Sub
    Private Sub QlCtxDelete(sender As MenuItem, e As EventArgs)
        cmsQuickLaunch.Close()

        Dim Path As String = sender.Parent.Tag.tag(0)
        Dim name As String = sender.Parent.Tag.Text

        Debug.Print($"Delete {Path}")
        Dim shiftdown = My.Computer.Keyboard.ShiftKeyDown
        Dim folderContentsMessage As String = vbCrLf
        If Path.EndsWith("\") Then
            Dim folderCount As Integer = System.IO.Directory.GetDirectories(Path, "*.*", IO.SearchOption.AllDirectories).Count
            Dim folS As String = If(folderCount = 1, "", "s")
            Dim filesCount As Integer = System.IO.Directory.GetFiles(Path, "*.*", IO.SearchOption.AllDirectories).Where(Function(f) IO.Path.GetFileName(f.ToLower) <> "desktop.ini").Count
            Dim filS As String = If(filesCount = 1, "", "s")
            folderContentsMessage &= $"This folder contains {folderCount} folder{folS} and {filesCount} file{filS}."
            If shiftdown OrElse CustomMessageBox.Show(Me, $"Are you sure you want to move ""{name}"" to the Recycle Bin?" & folderContentsMessage & $"{vbCrLf}Hold Shift to Permanently Delete.",
                                       "Confirm Delete", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                My.Computer.FileSystem.DeleteDirectory(Path, FileIO.UIOption.OnlyErrorDialogs,
                         If(shiftdown OrElse My.Computer.Keyboard.ShiftKeyDown, FileIO.RecycleOption.DeletePermanently, FileIO.RecycleOption.SendToRecycleBin),
                         FileIO.UICancelOption.DoNothing)
            End If
        Else
            My.Computer.FileSystem.DeleteFile(Path, FileIO.UIOption.AllDialogs,
                         If(shiftdown, FileIO.RecycleOption.DeletePermanently, FileIO.RecycleOption.SendToRecycleBin),
                         FileIO.UICancelOption.DoNothing)
        End If
    End Sub

    Private Sub QlCtxProps(sender As MenuItem, e As EventArgs)
        OpenProps(sender.Parent.Tag, New MouseEventArgs(MouseButtons.Right, 1, MousePosition.X, MousePosition.Y, 0))
    End Sub

    Private Sub QlCtxNewFolder(sender As MenuItem, e As EventArgs)
        Debug.Print($"QlCtxNewFolder sender:{sender}")
        Debug.Print($"tag:    {sender?.Tag}")

        Dim rootFolder As String = sender.Tag.Substring(0, sender.Tag.TrimEnd("\").LastIndexOf("\") + 1)
        'If rootFolder.EndsWith("\") Then
        '    rootFolder = rootFolder.Substring(0, rootFolder.TrimEnd("\").LastIndexOf("\") + 1)
        'Else
        '    rootFolder = rootFolder.Substring(0, rootFolder.LastIndexOf("\") + 1)
        'End If

        CreateNewFolder(rootFolder)

    End Sub

    Private Sub QlCtxNewAlt(sender As MenuItem, e As EventArgs)
        Debug.Print($"newAlt: {sender.Text}")
        Debug.Print($"tag: {sender.Tag(1)}")
        Dim rootFolder As String = sender.Tag(1)
        rootFolder = rootFolder.Substring(0, rootFolder.TrimEnd("\").LastIndexOf("\") + 1)
        Try
            CreateShortCut(New ToolStripMenuItem(sender.Text) With {.Tag = {sender.Tag(0), rootFolder}}, Nothing)
        Catch ex As Exception
            If ex.Message <> "Abort" Then Throw
        End Try
        cmsQuickLaunch.Close()
    End Sub

    Private ReadOnly folderHbm As IntPtr = foldericon?.GetHbitmap(Color.Red)
    Private ReadOnly plusHbm As IntPtr = New Bitmap(My.Resources.Add, New Size(16, 16)).GetHbitmap(Color.Red)

    'Dim QlCtxIsOpen As Boolean = False 'to handle glitch in contextmenu when moving astonia window
    Dim QlCtxNewMenu As New MenuItem
    Dim QlCtxMenu As New ContextMenu
    Private Sub QL_MouseDown(sender As ToolStripMenuItem, e As MouseEventArgs) 'Handles cmsQuickLaunch.mousedown
        If e.Button = MouseButtons.Right Then

            Debug.Print("QL_MouseDown")

            DestroyMenu(QlCtxMenu.Handle) ' manual destroy old since we recreate everytime, might not be needed if we dispose
            QlCtxMenu.Dispose()           ' but better to err on the side of caution and do it anyways.
            DestroyMenu(QlCtxNewMenu.Handle)
            QlCtxNewMenu.Dispose()

            'QlCtxIsOpen = True

            sender.Select()
            sender.BackColor = Color.FromArgb(&HFFB5D7F3) 'this to fix a glitch where sender gets unselected

            Dim newFolderItem As New MenuItem("Folder", AddressOf QlCtxNewFolder)
            QlCtxNewMenu = New MenuItem("New", {
                                             newFolderItem,
                                             New MenuItem("-")})

            Dim path As String = sender.Tag(0)

            QlCtxMenu = New ContextMenu({
                New MenuItem("Open", AddressOf QlCtxOpen) With {.DefaultItem = True},
                New MenuItem($"Open All{vbTab}-->", AddressOf QlCtxOpenAll) With {
                                .Visible = path.EndsWith("\") AndAlso
                                sender.DropDownItems.OfType(Of ToolStripMenuItem) _
                                      .Any(Function(it) it.Visible AndAlso it.Tag IsNot Nothing AndAlso extensions.Contains(IO.Path.GetExtension(it.Tag(0)))),
                                .Tag = sender
                                            },
                New MenuItem("-"),
                New MenuItem("Delete", AddressOf QlCtxDelete),
                New MenuItem("Rename", AddressOf QlCtxRename),
                New MenuItem("-"),
                New MenuItem("Properties", AddressOf QlCtxProps),
                New MenuItem("-"),
                QlCtxNewMenu})

            Dim name As String = sender.Text

            QlCtxMenu.Tag = sender
            newFolderItem.Tag = path

            Dim QlCtxNewMenuStaticItemsCount As Integer = QlCtxNewMenu.MenuItems.Count

            'dynamically add menuitems
            Dim aplist As List(Of AstoniaProcess) = AstoniaProcess.Enumerate(blackList).OrderBy(Function(ap) ap.Name).ToList
            For Each ap As AstoniaProcess In aplist
                QlCtxNewMenu.MenuItems.Add(New MenuItem(ap.Name, AddressOf QlCtxNewAlt) With {.Tag = {ap, path}})
            Next
            If aplist.Count = 0 Then
                QlCtxNewMenu.MenuItems.Add(New MenuItem("(None)") With {.Enabled = False})
            ElseIf aplist.Count >= 2 Then
                QlCtxNewMenu.MenuItems.Add(New MenuItem("-"))
                Dim AddAllItem = New MenuItem("Add All", AddressOf AddAllShortcuts) With {.Tag = path}
                QlCtxNewMenu.MenuItems.Add(AddAllItem)
                SetMenuItemBitmaps(QlCtxNewMenu.Handle, aplist.Count + 3, MF_BYPOSITION, plusHbm, Nothing)
            End If

            ModifyMenuA(QlCtxMenu.Handle, 0, MF_BYPOSITION, GetMenuItemID(QlCtxMenu.Handle, 0), $"{name}")
            Dim hbm = IntPtr.Zero
            If sender.Image IsNot Nothing Then
                hbm = DirectCast(sender.Image, Bitmap).GetHbitmap(Color.Red)
                SetMenuItemBitmaps(QlCtxMenu.Handle, 0, MF_BYPOSITION, hbm, Nothing)
            End If

            SetMenuItemBitmaps(QlCtxNewMenu.Handle, 0, MF_BYPOSITION, folderHbm, Nothing)
            SetMenuItemBitmaps(QlCtxMenu.Handle, 7, MF_BYPOSITION, plusHbm, Nothing)

            Dim purgeList As New List(Of IntPtr)

            Dim i = QlCtxNewMenuStaticItemsCount
            For Each item As MenuItem In QlCtxNewMenu.MenuItems.OfType(Of MenuItem).Skip(i).Where(Function(m) m.Tag IsNot Nothing AndAlso TypeOf (m.Tag) IsNot String)
                Dim althbm As IntPtr = New Bitmap(DirectCast(item.Tag(0), AstoniaProcess).GetIcon?.ToBitmap, New Size(16, 16)).GetHbitmap(Color.Red)
                purgeList.Add(althbm)
                SetMenuItemBitmaps(QlCtxNewMenu.Handle, i, MF_BYPOSITION, althbm, Nothing)
                i += 1
            Next
            Debug.Print($"purgeList.Count {purgeList.Count}")

            TrackPopupMenuEx(QlCtxMenu.Handle, TPM_RECURSE Or TPM_RIGHTBUTTON, MousePosition.X, MousePosition.Y, ScalaHandle, Nothing)

            sender.BackColor = Color.Transparent

            'free up recources
            DeleteObject(hbm)
            For Each item As IntPtr In purgeList
                DeleteObject(item)
            Next


        ElseIf Not sender.Tag(0).EndsWith("\") Then 'do not process click on dirs as they are handled by doubleclick
            Debug.Print("clicked Not a dir")
            Task.Run(Sub() OpenLnk(sender, e))
            'cmsQuickLaunch.Close(ToolStripDropDownCloseReason.ItemClicked)
        End If
    End Sub

    Private Sub OpenProps(ByVal sender As ToolStripMenuItem, ByVal e As MouseEventArgs) 'Handles smenu.MouseUp, item.MouseUp
        Debug.Print($"OpenProps {sender.Tag(0)} {sender.GetType}")
        Dim pth As String = sender.Tag(0).ToString.TrimEnd("\")
        If e.Button = MouseButtons.Right Then
            Dim sei As New SHELLEXECUTEINFO With {
               .cbSize = System.Runtime.InteropServices.Marshal.SizeOf(GetType(SHELLEXECUTEINFO)),
               .lpVerb = "properties",
               .lpFile = pth,
               .nShow = SW_SHOW,
               .fMask = SEE_MASK_INVOKEIDLIST
            }
            cmsQuickLaunch.Close()
            Task.Run(action:=Async Sub()
                                 ShellExecuteEx(sei) 'open properties
                                 'set properties to topmost
                                 Dim watch As Stopwatch = Stopwatch.StartNew()
                                 Dim WindowName As String = pth.ToLower.Substring(pth.LastIndexOf("\") + 1).Replace(".url", "").Replace(".lnk", "") & " Properties"
                                 'findwindow ignores case
                                 Dim hndl As IntPtr
                                 While watch.ElapsedMilliseconds < 10000
                                     Await Task.Delay(20)
                                     hndl = FindWindow("#32770", WindowName)
                                     Debug.Print($"findwindow {hndl}")
                                     If hndl <> IntPtr.Zero Then Exit While
                                 End While
                                 SetWindowPos(hndl, SWP_HWND.TOPMOST, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove)
                                 watch.Stop()
                             End Sub)
        End If
    End Sub

    Private Sub DblClickDir(ByVal sender As ToolStripMenuItem, ByVal e As EventArgs) 'Handles smenu.DoubleClick

        Dim pp As New Process With {.StartInfo = New ProcessStartInfo With {.FileName = sender.Tag(0)}}

        Try
            pp.Start()
        Catch

        End Try
    End Sub

    Private Sub OpenLnk(ByVal sender As ToolStripItem, ByVal e As System.Windows.Forms.MouseEventArgs) 'handles item.MouseDown
        Debug.Print("openLnk: " & sender.Tag(0))
        If e Is Nothing Then Exit Sub
        If e.Button = MouseButtons.Right Then
            'OpenProps(sender, e)
            Exit Sub
        End If

        Dim bat As String = "\AsInvoker.bat"
        Dim tmpDir As String = FileIO.SpecialDirectories.Temp & "\ScalA"

        If Not FileIO.FileSystem.DirectoryExists(tmpDir) Then FileIO.FileSystem.CreateDirectory(tmpDir)
        If Not FileIO.FileSystem.FileExists(tmpDir & bat) OrElse
           Not FileIO.FileSystem.GetFileInfo(tmpDir & bat).Length = My.Resources.AsInvoker.Length Then
            FileIO.FileSystem.WriteAllText(tmpDir & bat, My.Resources.AsInvoker, False, System.Text.Encoding.ASCII)
        End If

        Dim pp As Process = New Process With {.StartInfo = New ProcessStartInfo With {.FileName = tmpDir & bat,
                                                                       .Arguments = """" & sender.Tag(0) & """",
                                                                       .WorkingDirectory = System.IO.Path.GetDirectoryName(sender.Tag(0)),
                                                                       .WindowStyle = ProcessWindowStyle.Hidden,
                                                                       .CreateNoWindow = True}}

        Try
            pp.Start()
        Catch ex As Exception
            Debug.Print($"pp.start {ex.Message}")
        Finally
            pp.Dispose()
        End Try

        'btnStart.PerformClick()

    End Sub

    Private Sub ChangeLinksDir()
        Debug.Print("changeLinksDir")
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

    ReadOnly watchers As New List(Of System.IO.FileSystemWatcher)
    Public Sub UpdateWatchers(newPath As String)
        Debug.Print("updateWatchers")
        For Each w As System.IO.FileSystemWatcher In watchers
            w.Path = newPath
        Next
    End Sub
    Private Sub InitWatchers()
        Debug.Print("initWatchers")
        For Each w As System.IO.FileSystemWatcher In watchers
            w.Dispose()
        Next
        watchers.Clear()

        Dim iniWatcher As New System.IO.FileSystemWatcher(My.Settings.links) With {
            .NotifyFilter = System.IO.NotifyFilters.LastWrite,
            .Filter = "desktop.ini",
            .IncludeSubdirectories = True
        }

        AddHandler iniWatcher.Changed, AddressOf OnChanged
        iniWatcher.EnableRaisingEvents = True

        watchers.Add(iniWatcher)



        Dim lnkWatcher As New System.IO.FileSystemWatcher(My.Settings.links) With {
            .NotifyFilter = System.IO.NotifyFilters.LastWrite Or
                            System.IO.NotifyFilters.FileName,
            .Filter = "*.lnk",
            .IncludeSubdirectories = True
        }

        AddHandler lnkWatcher.Renamed, AddressOf OnRenamed
        AddHandler lnkWatcher.Changed, AddressOf OnChanged
        lnkWatcher.EnableRaisingEvents = True

        watchers.Add(lnkWatcher)



        Dim urlWatcher As New System.IO.FileSystemWatcher(My.Settings.links) With {
            .NotifyFilter = System.IO.NotifyFilters.LastWrite Or
                            System.IO.NotifyFilters.FileName,
            .Filter = "*.url",
            .IncludeSubdirectories = True
        }

        AddHandler urlWatcher.Renamed, AddressOf OnRenamed
        AddHandler urlWatcher.Changed, AddressOf OnChanged
        urlWatcher.EnableRaisingEvents = True

        watchers.Add(urlWatcher)



        Dim dirWatcher As New System.IO.FileSystemWatcher(My.Settings.links) With {
            .NotifyFilter = System.IO.NotifyFilters.DirectoryName,
            .IncludeSubdirectories = True
        }

        AddHandler dirWatcher.Renamed, AddressOf OnRenamedDir
        AddHandler dirWatcher.Deleted, AddressOf OnDeleteDir
        dirWatcher.EnableRaisingEvents = True

        watchers.Add(dirWatcher)


    End Sub
    Private Sub OnDeleteDir(sender As System.IO.FileSystemWatcher, e As System.IO.FileSystemEventArgs)
        Debug.Print($"Delete Dir: {sender.NotifyFilter}")
        Debug.Print($"      Type: {e.ChangeType}")
        Debug.Print($"      Path: {e.FullPath}")

        If e.ChangeType = IO.WatcherChangeTypes.Deleted Then
            For Each key In iconCache.Keys.Where(Function(k) k.StartsWith(e.FullPath & "\"))
                iconCache.TryRemove(key, Nothing)
            Next
        End If
    End Sub
    Private Sub OnRenamedDir(sender As System.IO.FileSystemWatcher, e As System.IO.RenamedEventArgs)
        Debug.Print($"Renamed Dir: {sender.NotifyFilter}")
        Debug.Print($"        Old: {e.OldFullPath}")
        Debug.Print($"        New: {e.FullPath}")

        For Each key In iconCache.Keys.Where(Function(k) k.StartsWith(e.OldFullPath & "\"))
            Dim item As Bitmap = Nothing
            If iconCache.TryRemove(key, item) Then iconCache.TryAdd(key.Replace(e.OldFullPath & "\", e.FullPath & "\"), item)
        Next
    End Sub
    Private Sub OnRenamed(sender As System.IO.FileSystemWatcher, e As System.IO.RenamedEventArgs)
        Debug.Print($"Renamed File: {sender.NotifyFilter}")
        Debug.Print($"         Old: {e.OldFullPath}")
        Debug.Print($"         New: {e.FullPath}")

        Dim item As Bitmap = Nothing
        If iconCache.TryRemove(e.OldFullPath, item) Then iconCache.TryAdd(e.FullPath, item)

    End Sub
    Private Sub OnChanged(sender As System.IO.FileSystemWatcher, e As System.IO.FileSystemEventArgs)
        If e.ChangeType = System.IO.WatcherChangeTypes.Changed Then
            Debug.Print(sender.ToString)
            Debug.Print($"Changed: {e.FullPath}")
            If e.FullPath.ToLower.EndsWith("desktop.ini") Then
                iconCache.TryRemove(e.FullPath.Substring(0, e.FullPath.LastIndexOf("\") + 1), Nothing)
            End If
            If hideExt.Contains(System.IO.Path.GetExtension(e.FullPath).ToLower) Then
                iconCache.TryRemove(e.FullPath, Nothing)
            End If
        End If
    End Sub

End Class
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
            Debug.Print($"Exception in AsTransparent {ex.Message}")
            Return Nothing
        End Try
    End Function
End Module