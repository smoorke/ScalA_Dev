Imports System.Collections.Concurrent

Public Class ContextMenus
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
            PopDropDown(cboAlt)
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
    Private ReadOnly iconCache As New ConcurrentDictionary(Of String, Bitmap)

    Private Function GetIcon(ByVal PathName As String) As Bitmap

        If PathName Is Nothing Then Return Nothing

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
                        Return Nothing
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
                        Return Nothing
                    End If
                    ico = Icon.FromHandle(hIcon)
                    bm = ico.ToBitmap
                    DestroyIcon(hIcon)
                End If
                DestroyIcon(ico.Handle)
                ico.Dispose()

                Return bm
            End Function)
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

                Dim attr As System.IO.FileAttributes = New System.IO.DirectoryInfo(fullDirs).Attributes
                If attr.HasFlag(System.IO.FileAttributes.Hidden) Then Continue For
                If attr.HasFlag(System.IO.FileAttributes.System) Then Continue For

                Dim smenu As New ToolStripMenuItem(System.IO.Path.GetFileName(fullDirs)) With {.Tag = fullDirs & "\"}

                smenu.DropDownItems.Add("(Dummy)").Enabled = False
                smenu.DoubleClickEnabled = True

                AddHandler smenu.MouseUp, AddressOf QL_Click
                AddHandler smenu.DoubleClick, AddressOf DblClickDir
                AddHandler smenu.DropDownOpening, AddressOf ParseSubDir
                'AddHandler smenu.DropDownOpened, AddressOf deferredIconLoading
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
            'Debug.Print(System.IO.Path.GetFileName(fullLink))

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
            AddHandler item.MouseDown, AddressOf QL_Click
            'AddHandler item.MouseEnter, AddressOf QL_MouseEnter
            'AddHandler item.MouseLeave, AddressOf QL_MouseLeave

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

        cts?.Dispose()
        cts = New Threading.CancellationTokenSource
        cantok = cts.Token
        deferredIconLoading(Dirs.Concat(Files), cantok)

        Debug.Print($"parsing ""{pth}"" took {watch.ElapsedMilliseconds} ms")
        watch.Stop()
        Return menuItems
    End Function

    'Private Sub QL_MouseLeave(sender As Object, e As EventArgs)
    '    cmsQuickLaunch.AutoClose = True
    'End Sub

    'Private Sub QL_MouseEnter(sender As Object, e As EventArgs)
    '    cmsQuickLaunch.AutoClose = False
    'End Sub

    Private Sub cmsQuickLaunchDropDown_Closing(sender As Object, e As ToolStripDropDownClosingEventArgs)
        cts?.Cancel()
    End Sub

    Private init As Boolean = True

    Private Sub deferredIconLoading(items As IEnumerable(Of ToolStripItem), ct As Threading.CancellationToken)
        Dim skipped As Boolean = False
        If init Then
            Debug.Print("init iconloader")
            Dim firstItem As ToolStripItem = items.FirstOrDefault
            Me.BeginInvoke(updateImage, {firstItem, GetIcon(firstItem?.Tag.ToString)}) ' needed or we get an exception in geticon later
            skipped = True
            init = False
        End If
        Try
            Task.Run(Sub()
                         Parallel.ForEach(items.Skip(skipped).TakeWhile(Function(__) Not ct.IsCancellationRequested),
                                          Sub(it As ToolStripItem)
                                              Me.BeginInvoke(updateImage, {it, GetIcon(it.Tag.ToString)})
                                          End Sub)
                     End Sub, ct)
        Catch ex As System.Threading.Tasks.TaskCanceledException
            Debug.Print("deferredIconLoading Task canceled")
        Catch
            Debug.Print("deferredIconLoading general exception")
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
        AButton.ActiveOverview = False
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
            sender.Items.Add(New ToolStripSeparator())
            sender.Items.Add("UnElevate", btnStart.Image, AddressOf UnelevateSelf).ToolTipText = "Drop Admin Rights" & vbCrLf & "Use this If you can't use ctrl, alt and/or shift."
        End If

        If watchers.Count = 0 Then InitWatchers()

    End Sub
    Private closeAllAtBottom As Boolean = True




    Dim cts As New Threading.CancellationTokenSource
    Dim cantok As Threading.CancellationToken = cts.Token
    Private Async Sub CmsQuickLaunch_Closed(sender As ContextMenuStrip, e As ToolStripDropDownClosedEventArgs) Handles cmsQuickLaunch.Closed
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
        Await Task.Delay(100)
        pbZoom.Visible = True
        AButton.ActiveOverview = True
    End Sub


    Dim QlCtxMenu As New ContextMenu({
        New MenuItem("Name"),
        New MenuItem("-"),
        New MenuItem("Delete"),
        New MenuItem("Rename"),
        New MenuItem("-"),
        New MenuItem("Properties")})
    'Dim kb As New Microsoft.VisualBasic.Devices.Keyboard
    Private Sub QL_Click(sender As ToolStripMenuItem, e As MouseEventArgs) 'handles Abutton.mousedown
        If e.Button = MouseButtons.Right Then
            sender.BackColor = Color.FromArgb(&HFFB5D7F3)
            sender.DropDown.Close()

            Dim path As String = sender.Tag
            Dim name As String = sender.Text

            ModifyMenuA(QlCtxMenu.Handle, 0, MF_BYPOSITION, 256, $"{name}")

            Dim hbm As IntPtr = CType(sender.Image, Bitmap).GetHbitmap(Color.Red)
            SetMenuItemBitmaps(QlCtxMenu.Handle, 0, MF_BYPOSITION, hbm, Nothing)
            SetMenuDefaultItem(QlCtxMenu.Handle, 0, MF_BYPOSITION)
            Dim value = TrackPopupMenuEx(QlCtxMenu.Handle, TPM_RECURSE Or TPM_RIGHTBUTTON Or TPM_RETURNCMD, MousePosition.X, MousePosition.Y, Me.Handle, Nothing)
            Debug.Print($"contextmenuvalue {value - 255}")
            Select Case value - 255
                Case 1 ' Open
                    OpenLnk(sender, e)
                Case 3 ' Delete
                    Dim folderContentsMessage As String = vbCrLf
                    If path.EndsWith("\") Then
                        Dim folderCount As Integer = System.IO.Directory.GetDirectories(path, "*.*", IO.SearchOption.AllDirectories).Count
                        Dim folS As String = If(folderCount = 1, "", "s")
                        Dim filesCount As Integer = System.IO.Directory.GetFiles(path, "*.*", IO.SearchOption.AllDirectories).Count
                        Dim filS As String = If(filesCount = 1, "", "s")
                        folderContentsMessage &= $"This folder contains {folderCount} folder{folS} and {filesCount} file{filS}."
                        If MessageBox.Show($"Are you sure you want to move ""{name}"" to the Recycle Bin?" & folderContentsMessage,
                                       "Confirm Delete", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                            My.Computer.FileSystem.DeleteDirectory(path, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.DoNothing)
                        End If
                    Else
                        My.Computer.FileSystem.DeleteFile(path, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.DoNothing)
                    End If
                Case 4 ' Rename
                    Dim title As String = $"Rename {name}"
                    Task.Run(Sub()
                                 Dim watch As Stopwatch = Stopwatch.StartNew()

                                 Dim hndl As IntPtr
                                 While watch.ElapsedMilliseconds < 2000
                                     Threading.Thread.Sleep(20)
                                     hndl = FindWindow(Nothing, title)
                                     Debug.Print($"findwindow {hndl}")
                                     If hndl <> IntPtr.Zero Then Exit While
                                 End While
                                 SetWindowPos(hndl, -1, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove)
                                 watch.Stop()
                             End Sub)
                    Dim newname = InputBox("Enter new name", title, name, MousePosition.X - 177, MousePosition.Y - 76)
                    If newname <> "" Then
                        'Dim newpath = path.Substring(0, path.TrimEnd("\").LastIndexOf("\")) & "\" & newname
                        Debug.Print($"oldpath: {path}")
                        'Debug.Print($"newpath: {newpath}")
                        Try
                            If path.EndsWith("\") Then
                                FileIO.FileSystem.RenameDirectory(path, newname)
                            Else
                                Dim targetname As String = newname
                                If {".lnk", ".url"}.Contains(System.IO.Path.GetExtension(path).ToLower) Then targetname &= System.IO.Path.GetExtension(path)
                                FileIO.FileSystem.RenameFile(path, targetname)
                            End If
                        Catch
                            MessageBox.Show(Me, $"Error renaming to {newname}!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                        Debug.Print($"renamed to {newname}")
                        'IO.File.Move(path,  newname)
                    End If
                Case 6 ' Properties
                    OpenProps(sender, e)
            End Select
            sender.BackColor = SystemColors.Control
            DeleteObject(hbm)
        ElseIf Not sender.Tag.EndsWith("\") Then 'do not process click on dirs
            Debug.Print("clicked not a dir")
            OpenLnk(sender, e)
            'cmsQuickLaunch.Close(ToolStripDropDownCloseReason.ItemClicked)
        End If
    End Sub

    Private Sub OpenProps(ByVal sender As Object, ByVal e As MouseEventArgs) 'Handles smenu.MouseUp, item.MouseUp
        Debug.Print($"OpenProps {sender.Tag} {sender.GetType.ToString}")
        Dim pth As String = sender.Tag.ToString.TrimEnd("\")
        If e.Button = MouseButtons.Right Then
            Dim sei As New SHELLEXECUTEINFO With {
               .cbSize = System.Runtime.InteropServices.Marshal.SizeOf(GetType(SHELLEXECUTEINFO)),
               .lpVerb = "properties",
               .lpFile = pth,
               .nShow = SW_SHOW,
               .fMask = SEE_MASK_INVOKEIDLIST
            }
            ShellExecuteEx(sei) 'open properties
            'set properties to topmost
            Task.Run(Sub()
                         Dim watch As Stopwatch = Stopwatch.StartNew()
                         Dim WindowName As String = pth.ToLower.Substring(pth.LastIndexOf("\") + 1).Replace(".url", "").Replace(".lnk", "") & " Properties"

                         Dim hndl As IntPtr
                         While watch.ElapsedMilliseconds < 2000
                             Threading.Thread.Sleep(20)
                             hndl = FindWindow("#32770", WindowName)
                             Debug.Print($"findwindow {hndl}")
                             If hndl <> IntPtr.Zero Then Exit While
                         End While
                         SetWindowPos(hndl, -1, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove)
                         watch.Stop()
                     End Sub)
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
        'If e.Button = MouseButtons.Right Then
        '    OpenProps(sender, e)
        '    Exit Sub
        'End If

        Dim pp As Process

        Dim bat As String = "\AsInvoker.bat"
        Dim tmpDir As String = FileIO.SpecialDirectories.Temp & "\ScalA"

        If Not FileIO.FileSystem.DirectoryExists(tmpDir) Then FileIO.FileSystem.CreateDirectory(tmpDir)
        If Not FileIO.FileSystem.FileExists(tmpDir & bat) Then FileIO.FileSystem.WriteAllText(tmpDir & bat, My.Resources.AsInvoker, False)

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

        AddHandler dirWatcher.Renamed, AddressOf OnRenamedDir
        dirWatcher.EnableRaisingEvents = True

        watchers.Add(dirWatcher)


    End Sub
    Private Sub OnRenamedDir(sender As System.IO.FileSystemWatcher, e As System.IO.RenamedEventArgs)
        Debug.Print($"Renamed Dir: {sender.NotifyFilter}")
        Debug.Print($"    Old: {e.OldFullPath}")
        Debug.Print($"    New: {e.FullPath}")

        Dim item As Bitmap = Nothing
        If iconCache.TryRemove(e.OldFullPath & "\", item) Then iconCache.TryAdd(e.FullPath & "\", item)

    End Sub
    Private Sub OnRenamed(sender As System.IO.FileSystemWatcher, e As System.IO.RenamedEventArgs)
        Debug.Print($"Renamed File: {sender.NotifyFilter}")
        Debug.Print($"    Old: {e.OldFullPath}")
        Debug.Print($"    New: {e.FullPath}")

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
            If {".lnk", ".url"}.Contains(System.IO.Path.GetExtension(e.FullPath).ToLower) Then
                iconCache.TryRemove(e.FullPath, Nothing)
            End If
        End If
    End Sub

End Class
