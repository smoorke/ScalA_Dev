﻿Public Class ContextMenus
    'dummy class to prevent form being generated
End Class
Partial Public Class FrmMain

    Private Sub CloseToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles CloseToolStripMenuItem.Click
        'SendMessage(CType(sender.Owner, ContextMenuStrip).SourceControl.Tag.MainWindowHandle, &H100, Keys.F12, IntPtr.Zero)
        PostMessage(CType(sender.Tag, AstoniaProcess).MainWindowHandle, &H100, Keys.F12, IntPtr.Zero)

    End Sub

    Private Sub SelectToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles SelectToolStripMenuItem.Click
        'Dim name As String = CType(sender.Owner, ContextMenuStrip).SourceControl.Text
        Dim pp As AstoniaProcess = CType(sender.Tag, AstoniaProcess)
        If pp Is Nothing Then Exit Sub
        Dim name As String = pp.Name
        Debug.Print("SelectToolStrip: " & name)
        If Not cboAlt.Items.Contains(pp) Then
            PopDropDown()
        End If
        cboAlt.SelectedItem = pp
    End Sub

    Private Sub TopMostToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles TopMostToolStripMenuItem.Click
        'Dim pp As Process = CType(sender.Owner, ContextMenuStrip).SourceControl.Tag
        Dim pp As AstoniaProcess = CType(sender.Tag, AstoniaProcess)
        If pp Is Nothing Then Exit Sub
        Debug.Print("Topmost " & Not sender.Checked)
        If Not sender.Checked Then
            SetWindowPos(pp.MainWindowHandle, -1, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove)
        Else
            SetWindowPos(pp.MainWindowHandle, -2, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove)
        End If
    End Sub

    Private Sub CmsAlt_Opening(sender As ContextMenuStrip, e As System.ComponentModel.CancelEventArgs) Handles cmsAlt.Opening
        If My.Computer.Keyboard.ShiftKeyDown OrElse My.Computer.Keyboard.CtrlKeyDown Then
            cmsQuickLaunch.Show(sender.SourceControl, sender.SourceControl.PointToClient(MousePosition))
            e.Cancel = True
            Exit Sub
        End If
        Dim pp As AstoniaProcess = sender.SourceControl.Tag

        SelectToolStripMenuItem.Text = "Select " & pp.Name
        SelectToolStripMenuItem.Image = pp.GetIcon?.ToBitmap
        SelectToolStripMenuItem.Tag = pp

        TopMostToolStripMenuItem.Checked = pp.IsTopMost()
        TopMostToolStripMenuItem.Tag = pp

        Static closeAllToolStripMenuItem As ToolStripMenuItem = Nothing
        If sender.Items.Contains(closeAllToolStripMenuItem) Then
            sender.Items.Remove(closeAllToolStripMenuItem)
        End If

        sender.Items.RemoveAt(sender.Items.Count - 1)
        sender.Items.Add("Close " & pp.Name, My.Resources.F12, AddressOf CloseToolStripMenuItem_Click).Tag = pp

        If AstoniaProcess.Enumerate(True).Any(Function(p As AstoniaProcess) p.Name = "Someone") Then
            closeAllToolStripMenuItem = sender.Items.Add("Close All Idled", My.Resources.F12, AddressOf CloseAllIdle_Click)
        End If
    End Sub

    Private Sub CloseAllIdle_Click(sender As ToolStripMenuItem, e As EventArgs) 'Handles closeAllToolStripMenuItem.click

        For Each pp As AstoniaProcess In AstoniaProcess.Enumerate(True).Where(Function(p As AstoniaProcess) p.Name = "Someone")
            PostMessage(pp.MainWindowHandle, &H100, Keys.F12, IntPtr.Zero)
        Next

    End Sub


    Private ReadOnly iconCache As New Dictionary(Of String, Bitmap)

    Private Function GetIcon(ByVal PathName As String, isFolder As Boolean) As Bitmap

        If PathName Is Nothing Then Return Nothing

        If iconCache.ContainsKey(PathName) Then
            'Debug.Print($"iconCacheHit: {PathName}")
            Return iconCache(PathName)
        End If

        Debug.Print($"iconCahceMiss: {PathName}")

        Dim bm As Bitmap
        Dim fi As New SHFILEINFOW
        Dim ico As Icon

        If isFolder Then
            SHGetFileInfoW(PathName, 0, fi, System.Runtime.InteropServices.Marshal.SizeOf(fi), SHGFI_ICON Or SHGFI_SMALLICON)
            If fi.hIcon = IntPtr.Zero Then
                Debug.Print("hIcon empty: " & Runtime.InteropServices.Marshal.GetLastWin32Error)
                Return Nothing
            End If
            ico = Icon.FromHandle(fi.hIcon)
            bm = ico.ToBitmap
            DestroyIcon(fi.hIcon)
        Else 'not a folder
            Dim list As IntPtr = SHGetFileInfoW(PathName, 0, fi, System.Runtime.InteropServices.Marshal.SizeOf(fi), SHGFI_SYSICONINDEX Or SHGFI_SMALLICON)
            Dim hIcon As IntPtr = ImageList_GetIcon(list, fi.iIcon, 0)
            If hIcon = IntPtr.Zero Then
                Debug.Print("iconlist empty: " & Runtime.InteropServices.Marshal.GetLastWin32Error)
                Return Nothing
            End If
            ico = Icon.FromHandle(hIcon)
            bm = ico.ToBitmap
            DestroyIcon(hIcon)
            ImageList_Destroy(list)
        End If
        DestroyIcon(ico.Handle)
        ico.Dispose()

        Try
            iconCache.Add(PathName, bm)
        Catch
        End Try

        Return bm
    End Function
    Private ReadOnly nsSorter As IComparer(Of String) = New CustomStringSorter
    Private Function ParseDir(pth As String) As List(Of ToolStripItem)
        Dim menuItems As New List(Of ToolStripItem)
        Dim hasNoDirs As Boolean = True
        Dim hasNoFiles As Boolean = True
        'Const ICONTIMEOUT = 50
        Const TOTALTIMEOUT = 3000
        Dim timedout As Boolean = False

        Dim watch As Stopwatch = Stopwatch.StartNew()

        Dim Dirs As New List(Of ToolStripItem)
        Try
            For Each fullDirs As String In System.IO.Directory.EnumerateDirectories(pth)

                Dim attr As System.IO.FileAttributes = New System.IO.DirectoryInfo(fulldirs).Attributes
                If attr.HasFlag(System.IO.FileAttributes.Hidden) Then Continue For
                If attr.HasFlag(System.IO.FileAttributes.System) Then Continue For

                Dim smenu As New ToolStripMenuItem(System.IO.Path.GetFileName(fulldirs)) With {.Tag = fulldirs}

                smenu.DropDownItems.Add("(Dummy)").Enabled = False
                smenu.DoubleClickEnabled = True

                AddHandler smenu.MouseUp, AddressOf OpenProps
                AddHandler smenu.DoubleClick, AddressOf DblClickDir
                AddHandler smenu.DropDownOpening, AddressOf ParseSubDir
                AddHandler smenu.DropDown.Closing, AddressOf cmsQuickLaunchDropDown_Closing

                Dirs.Add(smenu)
                hasNoDirs = False
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
                                       .Where(Function(p) {".exe", ".jar", ".lnk", ".url", ".txt"}.Contains(System.IO.Path.GetExtension(p).ToLower))
            Debug.Print(System.IO.Path.GetFileName(fullLink))

            'don't add self to list
            If System.IO.Path.GetFileName(fullLink) = System.IO.Path.GetFileName(Environment.GetCommandLineArgs(0)) Then Continue For
            If fullLink = Environment.GetCommandLineArgs(0) Then Continue For

            Dim linkName As String
            If {".lnk", ".url"}.Contains(System.IO.Path.GetExtension(fullLink).ToLower) Then
                linkName = System.IO.Path.GetFileNameWithoutExtension(fullLink)
            Else
                linkName = System.IO.Path.GetFileName(fullLink)
            End If

            Dim item As New ToolStripMenuItem(linkName) With {.Tag = fullLink}
            AddHandler item.MouseUp, AddressOf OpenLnk

            Files.Add(item)
            hasNoFiles = False
            If watch.ElapsedMilliseconds > TOTALTIMEOUT Then
                timedout = True
                Exit For
            End If
        Next


        menuItems = Dirs.OrderBy(Function(d) d.Text, nsSorter).ThenBy(Function(d) d.Text.Length).Concat(
                   Files.OrderBy(Function(f) f.Text, nsSorter).ThenBy(Function(f) f.Text.Length)).ToList

        If timedout Then
            menuItems.Add(New ToolStripMenuItem("<TimedOut>") With {.Enabled = False})
        End If

        If My.Computer.Keyboard.CtrlKeyDown OrElse hasNoFiles Then
            If hasNoFiles AndAlso hasNoDirs Then menuItems.Add(New ToolStripMenuItem("(Emtpy)") With {.Enabled = False})
            If My.Computer.Keyboard.CtrlKeyDown OrElse hasNoDirs Then
                menuItems.Add(New ToolStripSeparator)
                Dim addShortcutMenu As New ToolStripMenuItem("Create Shortcut", My.Resources.Add) With {.Tag = pth}
                addShortcutMenu.DropDownItems.Add("(Dummy)").Enabled = False
                AddHandler addShortcutMenu.DropDownOpening, AddressOf AddShortcutMenu_DropDownOpening
                menuItems.Add(addShortcutMenu)
            End If
        End If

        '        If True Or watch.ElapsedMilliseconds >= ICONTIMEOUT Then
        cts?.Dispose()
        cts = New Threading.CancellationTokenSource
        cantok = cts.Token
        defferedIconLoading(Dirs, Files, cantok)
        '       End If

        Debug.Print($"parsing ""{pth}"" took {watch.ElapsedMilliseconds} ms")
        watch.Stop()
        Return menuItems
    End Function

    Private Sub cmsQuickLaunchDropDown_Closing(sender As Object, e As ToolStripDropDownClosingEventArgs)
        cts.Cancel()
    End Sub

    Private init As Boolean = True
    Private Sub defferedIconLoading(dirs As IEnumerable(Of ToolStripItem), fils As IEnumerable(Of ToolStripItem), ct As Threading.CancellationToken)
        Dim skipped As Boolean = False
        If init Then
            Debug.Print("init iconloader")
            Dim firstItem As ToolStripItem = If(dirs.FirstOrDefault, fils.FirstOrDefault)
            Me.BeginInvoke(updateImage, {firstItem, GetIcon(firstItem?.Tag.ToString, dirs.Any)}) ' needed or we get an exception in geticon later
            skipped = True
            init = False
        End If
        Try
            If dirs.Any Then
                Task.Run(Sub()
                             Parallel.ForEach(dirs.Skip(skipped).TakeWhile(Function(__) Not ct.IsCancellationRequested),
                                              Sub(item As ToolStripItem)
                                                  Me.BeginInvoke(updateImage, {item, GetIcon(item.Tag.ToString, True)})
                                              End Sub)
                         End Sub, ct)
            End If
            If fils.Any Then
                Task.Run(Sub()
                             Parallel.ForEach(fils.Skip(skipped AndAlso Not dirs.Any).TakeWhile(Function(__) Not ct.IsCancellationRequested),
                                              Sub(item As ToolStripItem)
                                                  Me.BeginInvoke(updateImage, {item, GetIcon(item.Tag.ToString, False)})
                                              End Sub)
                         End Sub, ct)
            End If
        Catch ex As System.Threading.Tasks.TaskCanceledException
            Debug.Print("defferedIconLoading Task canceled")
        Catch
            Debug.Print("defferedIconLoading general exception")
        End Try
    End Sub
    Private Sub ParseSubDir(sender As ToolStripMenuItem, e As EventArgs) 'handles dir.DropDownOpening
        sender.DropDownItems.Clear()
        sender.DropDownItems.AddRange(ParseDir(sender.Tag).ToArray)
    End Sub

    Private Sub AddShortcutMenu_DropDownOpening(sender As ToolStripMenuItem, e As EventArgs) 'Handles addShortcutMenu.DropDownOpening
        Debug.Print("addshortcut.sendertag:" & sender.Tag)
        sender.DropDownItems.Clear()
        For Each alt As AstoniaProcess In AstoniaProcess.Enumerate()
            Dim item As New ToolStripMenuItem(alt.Name, alt.GetIcon?.ToBitmap, AddressOf CreateShortCut) With {
                .Tag = {alt, sender.Tag} ' sender.tag is parent menu location
                }
            sender.DropDownItems.Add(item)
        Next
        If sender.DropDownItems.Count = 0 Then
            sender.DropDownItems.Add("(None)").Enabled = False
        End If
    End Sub

    Private Sub CreateShortCut(sender As ToolStripMenuItem, e As EventArgs)

        Dim alt As AstoniaProcess = CType(sender.Tag(0), AstoniaProcess)
        Dim ShortCutPath As String = sender.Tag(1)
        Dim ShortCutName As String = alt.Name
        Dim ShortCutLink As String = ShortCutPath & "\" & ShortCutName & ".lnk"

        If System.IO.File.Exists(ShortCutLink) AndAlso
           MessageBox.Show($"""{alt.Name}"" already exists. Overwrite?",
                           "Notice", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) _
           = DialogResult.Cancel Then Exit Sub

        'Dim QS As New ManagementObjectSearcher(“Select * from Win32_Process WHERE ProcessID=" & alt.Id.ToString)
        'Dim objCol As ManagementObjectCollection = QS.Get

        Dim mos As Management.ManagementObject = New Management.ManagementObjectSearcher($“Select * from Win32_Process WHERE ProcessID={alt.Id}").Get()(0)

        Dim arguments As String = mos("commandline")

        If arguments = "" Then
            If MessageBox.Show("Access denied!" & vbCrLf &
                           "Elevate ScalA to Administrator?" & vbCrLf &
                               "You will have to redo the shortcut creation.",
                               "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) _
               = DialogResult.Cancel Then Exit Sub
            My.Settings.Save()
            ElevateSelf()
            End 'program
            Exit Sub
        End If
        Debug.Print("cmdline:" & arguments)
        If arguments.StartsWith("""") Then
            'arguments = arguments.Substring(1) 'skipped with startindex
            arguments = arguments.Substring(arguments.IndexOf("""", 1) + 1)
        Else
            For Each exe As String In My.Settings.exe.Split({"|"c}, StringSplitOptions.RemoveEmptyEntries)
                If arguments.ToLower.StartsWith(exe.Trim) Then
                    arguments = arguments.Substring(exe.Trim.Length + 4)
                End If
            Next
        End If



        If Not arguments.ToLower.Contains("-w") Then
            If MessageBox.Show("Missing '-w' flag" & vbCrLf &
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
            oLink.WorkingDirectory = exepath.Substring(0, exepath.LastIndexOf("\"))
            oLink.WindowStyle = 1
            oLink.Save()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub CmsQuickLaunch_Opening(sender As ContextMenuStrip, e As System.ComponentModel.CancelEventArgs) Handles cmsQuickLaunch.Opening
        'If AltPP IsNot Nothing AndAlso AltPP?.Id <> 0 Then
        '    SendMessage(AltPP.MainWindowHandle, &H205, 0, 0) 'does not fix right click drag bug, does fix right click stuck after drag bug

        'End If
        pbZoom.Visible = False
        If My.Computer.Keyboard.ShiftKeyDown Then
            'show sysmenu
            Debug.Print("ShowSysMenu ")

            Me.ShowSysMenu(sender, Nothing)

            'Dim cmd As Integer = TrackPopupMenuEx(hSysMenu, &H102L, MousePosition.X, MousePosition.Y, Me.Handle, Nothing)
            'If cmd > 0 Then
            '    Debug.Print("SendMessage " & cmd)
            '    SendMessage(Me.Handle, WM_SYSCOMMAND, cmd, IntPtr.Zero)
            'End If

            e.Cancel = True
            Exit Sub
        End If


        'tmrTick.Interval = 1000
        sender.Items.Clear()

        If Not FileIO.FileSystem.DirectoryExists(My.Settings.links) Then My.Settings.links = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & "\ScalA"

        If Not FileIO.FileSystem.DirectoryExists(My.Settings.links) Then
            System.IO.Directory.CreateDirectory(My.Settings.links)
            System.IO.Directory.CreateDirectory(My.Settings.links & "\Example Folder")
            FileIO.FileSystem.WriteAllText(My.Settings.links & "\ReadMe.txt", My.Resources.ReadMe, False)
        End If


        sender.Items.AddRange(ParseDir(My.Settings.links).ToArray)

        If My.Computer.Keyboard.CtrlKeyDown Then
            sender.Items.Add(New ToolStripSeparator())
            sender.Items.Add("Select Folder", My.Resources.gear_wheel, AddressOf ChangeLinksDir)
        End If

        If AstoniaProcess.Enumerate(True).Any(Function(pp As AstoniaProcess) pp.Name = "Someone") Then
            If sender.SourceControl Is Nothing Then 'called from trayicon
                sender.Items.Insert(0, New ToolStripSeparator())
                sender.Items.Insert(0, New ToolStripMenuItem("Close All Idled", My.Resources.F12, AddressOf CloseAllIdle_Click))
                closeAllAtBottom = False
            Else
                sender.Items.Add(New ToolStripSeparator())
                sender.Items.Add("Close All Idled", My.Resources.F12, AddressOf CloseAllIdle_Click)
                closeAllAtBottom = True
            End If
        End If

        If sender.SourceControl Is btnStart AndAlso My.User.IsInRole(ApplicationServices.BuiltInRole.Administrator) Then
            'Dim image As Image = Nothing
            'Dim imageHandle = LoadImage(IntPtr.Zero, "#106", 1, 16, 16, 0)
            'If imageHandle <> IntPtr.Zero Then
            '    Using Ico = Icon.FromHandle(imageHandle)
            '        image = Ico.ToBitmap
            '        DestroyIcon(Ico.Handle)
            '    End Using
            'End If
            sender.Items.Add(New ToolStripSeparator())
            sender.Items.Add("UnElevate", btnStart.Image, AddressOf UnelevateSelf).ToolTipText = "Drop Admin Rights" & vbCrLf & "Use this If you can't use ctrl, alt and/or shift."
        End If

        If watchers.Count = 0 Then InitWatchers()

    End Sub
    Private closeAllAtBottom As Boolean = True




    Dim cts As New Threading.CancellationTokenSource
    Dim cantok As Threading.CancellationToken = cts.Token
    Private Sub CmsQuickLaunch_Closed(sender As ContextMenuStrip, e As ToolStripDropDownClosedEventArgs) Handles cmsQuickLaunch.Closed
        cts.Cancel() 'cancel deferred icon loading
        'sender.Items.Clear() 'this couses menu to stutter opening
        Debug.Print("cmsQuickLaunch closed reason:" & e.CloseReason.ToString)
        If AltPP IsNot Nothing AndAlso e.CloseReason <> ToolStripDropDownCloseReason.AppClicked AndAlso e.CloseReason <> ToolStripDropDownCloseReason.ItemClicked Then
            'these couse ghosting of menu and blanking of zoom when closed by reopening when reason = appclicked
            'SetWindowPos(AltPP.MainWindowHandle, -2, -1, -1, -1, -1, SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.IgnoreResize)
            If sender.SourceControl IsNot Nothing AndAlso AltPP.Id <> 0 Then
                AppActivate(AltPP.Id)
            End If
        End If
        'tmrTick.Interval = 50
        pbZoom.Visible = True
    End Sub

    Private Sub OpenProps(ByVal sender As ToolStripMenuItem, ByVal e As System.Windows.Forms.MouseEventArgs) 'Handles smenu.MouseUp, item.MouseUp
        Debug.Print($"click {sender.Tag}")
        If e.Button = MouseButtons.Right Then
            Dim sei As New SHELLEXECUTEINFO With {
               .cbSize = System.Runtime.InteropServices.Marshal.SizeOf(GetType(SHELLEXECUTEINFO)),
               .lpVerb = "properties",
               .lpFile = sender.Tag,
               .nShow = SW_SHOW,
               .fMask = SEE_MASK_INVOKEIDLIST
            }
            ShellExecuteEx(sei)
            'set properties to topmost
            Debug.Print(sender.Tag.ToString.Substring(sender.Tag.ToString.LastIndexOf("\") + 1))
            Dim WindowName As String = sender.Tag.ToString.ToLower.Substring(sender.Tag.ToString.LastIndexOf("\") + 1).Replace(".url", "").Replace(".lnk", "") & " Properties"
            Debug.Print($"WindowName:{WindowName}")
            Task.Run(Sub()
                         Dim watch As Stopwatch = Stopwatch.StartNew()

                         Dim hndl As IntPtr
                         While watch.ElapsedMilliseconds < 1500
                             Threading.Thread.Sleep(20)
                             hndl = FindWindow("#32770", WindowName)
                             Debug.Print($"findwindow {hndl}")
                             If hndl <> IntPtr.Zero Then Exit While
                         End While
                         SetWindowPos(hndl, -1, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove)
                         watch.Stop()
                     End Sub
                )
        End If
    End Sub

    Private Sub DblClickDir(ByVal sender As ToolStripMenuItem, ByVal e As EventArgs) 'Handles smenu.DoubleClick

        Dim pp As New Process With {.StartInfo = New ProcessStartInfo With {.FileName = sender.Tag}}

        Try
            pp.Start()
        Catch

        End Try
    End Sub

    Private Sub OpenLnk(ByVal sender As ToolStripItem, ByVal e As System.Windows.Forms.MouseEventArgs) 'handles item.MouseUp
        Debug.Print("openLnk: " & sender.Tag)
        If e.Button = MouseButtons.Right Then
            OpenProps(sender, e)
            Exit Sub
        End If

        Dim pp As Process

        Dim bat As String = "\noAdmin.bat"
        Dim tmpDir As String = FileIO.SpecialDirectories.Temp & "\ScalA"

        If Not FileIO.FileSystem.DirectoryExists(tmpDir) Then FileIO.FileSystem.CreateDirectory(tmpDir)
        If Not FileIO.FileSystem.FileExists(tmpDir & bat) Then FileIO.FileSystem.WriteAllText(tmpDir & bat, My.Resources.noadmin, False)

        pp = New Process With {.StartInfo = New ProcessStartInfo With {.FileName = tmpDir & bat,
                                                                       .Arguments = """" & sender.Tag & """",
                                                                       .WindowStyle = ProcessWindowStyle.Hidden,
                                                                       .CreateNoWindow = True}}
        Try
            pp.Start()
        Catch
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
            Using fb As New Ookii.Dialogs.WinForms.VistaFolderBrowserDialog
                fb.Description = "Select Folder Containing Your Shortcuts - ScalA"
                fb.UseDescriptionForTitle = True
                'fb.ShowNewFolderButton = False
                fb.RootFolder = Environment.SpecialFolder.Desktop
                fb.SelectedPath = My.Settings.links
                If fb.ShowDialog() = DialogResult.OK Then
                    ' Warning for Root folder with throw for dialog cancel
                    If fb.SelectedPath = System.IO.Path.GetPathRoot(fb.SelectedPath) AndAlso
                        MessageBox.Show("Warning: Selecting a root path is not recommended" & vbCrLf &
                                        $"Are you sure you want to use {fb.SelectedPath}?", "Warning",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.No Then Throw New Exception("dummy")
                    If My.Settings.links <> fb.SelectedPath Then
                        My.Settings.links = fb.SelectedPath
                        UpdateWatchers(My.Settings.links)
                    End If
                End If
            End Using
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

        Dim iniWatcher As System.IO.FileSystemWatcher = New System.IO.FileSystemWatcher(My.Settings.links) With {
            .NotifyFilter = System.IO.NotifyFilters.LastWrite,
            .Filter = "desktop.ini",
            .IncludeSubdirectories = True
        }

        AddHandler iniWatcher.Changed, AddressOf OnChanged
        iniWatcher.EnableRaisingEvents = True

        watchers.Add(iniWatcher)



        Dim lnkWatcher As System.IO.FileSystemWatcher = New System.IO.FileSystemWatcher(My.Settings.links) With {
            .NotifyFilter = System.IO.NotifyFilters.LastWrite Or
                            System.IO.NotifyFilters.FileName,
            .Filter = "*.lnk",
            .IncludeSubdirectories = True
        }

        AddHandler lnkWatcher.Renamed, AddressOf OnRenamed
        AddHandler lnkWatcher.Changed, AddressOf OnChanged
        lnkWatcher.EnableRaisingEvents = True

        watchers.Add(lnkWatcher)



        Dim urlWatcher As System.IO.FileSystemWatcher = New System.IO.FileSystemWatcher(My.Settings.links) With {
            .NotifyFilter = System.IO.NotifyFilters.LastWrite Or
                            System.IO.NotifyFilters.FileName,
            .Filter = "*.url",
            .IncludeSubdirectories = True
        }

        AddHandler urlWatcher.Renamed, AddressOf OnRenamed
        AddHandler urlWatcher.Changed, AddressOf OnChanged
        urlWatcher.EnableRaisingEvents = True

        watchers.Add(urlWatcher)



        Dim dirWatcher As System.IO.FileSystemWatcher = New System.IO.FileSystemWatcher(My.Settings.links) With {
            .NotifyFilter = System.IO.NotifyFilters.DirectoryName,
            .IncludeSubdirectories = True
        }

        AddHandler dirWatcher.Renamed, AddressOf OnRenamed
        dirWatcher.EnableRaisingEvents = True

        watchers.Add(dirWatcher)


    End Sub

    Private Sub OnRenamed(sender As System.IO.FileSystemWatcher, e As System.IO.RenamedEventArgs)
        Debug.Print($"Renamed:")
        Debug.Print($"    Old: {e.OldFullPath}")
        Debug.Print($"    New: {e.FullPath}")
        If iconCache.ContainsKey(e.OldFullPath) Then
            iconCache(e.FullPath) = iconCache(e.OldFullPath)
            iconCache.Remove(e.OldFullPath)
        End If
    End Sub

    Private Sub OnChanged(sender As System.IO.FileSystemWatcher, e As System.IO.FileSystemEventArgs)
        If e.ChangeType = System.IO.WatcherChangeTypes.Changed Then
            Debug.Print(sender.ToString)
            Debug.Print($"Changed: {e.FullPath}")
            If e.FullPath.ToLower.EndsWith("desktop.ini") Then
                iconCache.Remove(e.FullPath.Substring(0, e.FullPath.LastIndexOf("\")))
            End If
            If e.FullPath.ToLower.EndsWith(".lnk") Then
                iconCache.Remove(e.FullPath)
            End If
            If e.FullPath.ToLower.EndsWith(".url") Then
                iconCache.Remove(e.FullPath)
            End If
        End If
    End Sub

End Class
