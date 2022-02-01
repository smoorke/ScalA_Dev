Partial Public Class FrmMain



    Public AltPP As AstoniaProcess = New AstoniaProcess()
    'Private WndClass() As String = {"MAINWNDMOAC", "䅍义乗䵄䅏C"}
#Region " Alt Dropdown "
    Private Sub PopDropDown(sender As ComboBox)

        Dim current As AstoniaProcess = CType(sender.SelectedItem, AstoniaProcess)
        sender.BeginUpdate()
        updatingCombobox = True

        sender.Items.Clear()
        sender.Items.Add(New AstoniaProcess) 'Someone
        sender.Items.AddRange(AstoniaProcess.Enumerate.ToArray)

        If current IsNot Nothing AndAlso sender.Items.Contains(current) Then
            sender.SelectedItem = current
        Else
            sender.SelectedIndex = 0
        End If

        updatingCombobox = False
        sender.EndUpdate()
    End Sub

    Private Sub CboAlt_DropDown(sender As ComboBox, e As EventArgs) Handles cboAlt.DropDown
        pbZoom.Visible = False
        AButton.ActiveOverview = False
        PopDropDown(sender)
    End Sub
    Private Sub CmbResolution_DropDown(sender As ComboBox, e As EventArgs) Handles cmbResolution.DropDown
        pbZoom.Visible = False
        AButton.ActiveOverview = False
    End Sub

    Private Async Sub ComboBoxes_DropDownClosed(sender As ComboBox, e As EventArgs) Handles cboAlt.DropDownClosed, cmbResolution.DropDownClosed
        Await Task.Delay(200)
        If cboAlt.SelectedIndex > 0 Then
            pbZoom.Visible = True
        End If
        AButton.ActiveOverview = My.Settings.gameOnOverview
    End Sub

#End Region

    Dim altTopM As Integer = -2
    Private ReadOnly restoreParent As UInteger = GetWindowLong(Me.Handle, GWL_HWNDPARENT)
    Private prevItem As AstoniaProcess = New AstoniaProcess()
    Private updatingCombobox As Boolean = False
    Private Async Sub CboAlt_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboAlt.SelectedIndexChanged

        If updatingCombobox Then
            Exit Sub
        End If

        Debug.Print($"CboAlt_SelectedIndexChanged {sender.SelectedIndex}")

        'btnAlt1.Focus()

        Dim that As ComboBox = CType(sender, ComboBox)

        If AltPP.Id <> 0 AndAlso AltPP.Equals(CType(that.SelectedItem, AstoniaProcess)) Then
            AppActivate(AltPP.Id)
            Exit Sub
        End If

        If AltPP.Id = 0 AndAlso that.SelectedIndex = 0 Then
            Exit Sub
        End If

        counter = 0

        SetWindowLong(Me.Handle, GWL_HWNDPARENT, restoreParent)
        RestorePos(AltPP)

        AltPP = CType(that.SelectedItem, AstoniaProcess)
        UpdateTitle()
        If that.SelectedIndex = 0 Then
            pnlOverview.SuspendLayout()
            UpdateButtonLayout(AstoniaProcess.Enumerate.Count)
            For Each but As AButton In pnlOverview.Controls.OfType(Of AButton).TakeWhile(Function(b) b.Text <> "")
                but.Image = Nothing
                but.BackgroundImage = Nothing
                but.Text = String.Empty
            Next
            pnlOverview.ResumeLayout()
            pnlOverview.Show()
            tmrOverview.Enabled = True
            tmrTick.Enabled = False
            If prevItem.Id <> 0 Then startThumbsDict(prevItem.Id) = thumb
            sysTrayIcon.Icon = My.Resources.moa3
            prevItem = CType(that.SelectedItem, AstoniaProcess)
            Exit Sub
        Else
            pnlOverview.Hide()
            tmrOverview.Enabled = False
            Debug.Print("tmrStartup.stop")
        End If


        If Not AltPP?.IsRunning Then
            Dim idx As Integer = that.SelectedIndex
            that.Items.RemoveAt(idx)
            that.SelectedIndex = Math.Min(idx, that.Items.Count - 1)
            Exit Sub
        End If


        If Not AltPP?.Id = 0 Then


            GetWindowRect(AltPP.MainWindowHandle, rcW)
            GetClientRect(AltPP.MainWindowHandle, rcC)

            Debug.Print($"rcW:{rcW.ToString}")
            Debug.Print($"rcC:{rcC.ToString}")

            'check if target is running as windowed. if not ask to run it with -w
            If rcC.Width = 0 AndAlso rcC.Height = 0 Then
                'MessageBox.Show("Client is not running in windowed mode", "Error")
                Await reOpenAsWindowed(AltPP)
                'cboAlt.SelectedIndex = 0
                Exit Sub
            End If

            AppActivate(AltPP.Id)

            Dim ptt As Point

            Debug.Print("ClientToScreen")
            ClientToScreen(AltPP.MainWindowHandle, ptt)

            AstClientOffset = New Size(ptt.X - rcW.Left, ptt.Y - rcW.Top)

            'Debug.Print("updateTitle")
            'UpdateTitle()

            'newX = Me.Left + pbZoom.Left - AstClientOffset.Width - My.Settings.offset.X
            'newY = Me.Top + pbZoom.Top - AstClientOffset.Height - My.Settings.offset.Y

            Debug.Print("SetWindowLong")

            altTopM = If(AltPP.IsTopMost(), -1, -2)

            'SetWindowPos(AltPP.MainWindowHandle, -2, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove)
            SetWindowLong(Me.Handle, GWL_HWNDPARENT, AltPP.MainWindowHandle) ' have Client always be beneath ScalA (set Scala to be owner of client)
            '                                                                         note SetParent() doesn't work.

            Debug.Print("AltPPTopMost " & AltPP.IsTopMost.ToString)
            Debug.Print("SelfTopMost " & Process.GetCurrentProcess.IsTopMost.ToString)

            'Debug.Print("SetWindowPos")
            'SetWindowPos(AltPP.MainWindowHandle, Me.Handle, newX, newY, -1, -1, SetWindowPosFlags.IgnoreResize) ' + SetWindowPosFlags.DoNotActivate)

            Dim item As AstoniaProcess = CType(that.SelectedItem, AstoniaProcess)
            If Not startThumbsDict.ContainsKey(item.Id) Then
                Debug.Print("createThumb")
                CreateThumb()
            Else
                Debug.Print($"reassignThumb {item.Id} {startThumbsDict(item.Id)} {item.Name}")
                thumb = startThumbsDict(item.Id)
            End If

            For Each thumbid As IntPtr In startThumbsDict.Values
                If thumbid = thumb Then Continue For
                DwmUnregisterThumbnail(thumbid)
            Next
            startThumbsDict.Clear()


            Debug.Print("updateThumb")
            If rectDic.ContainsKey(item.Id) Then
                AnimateThumb(rectDic(item.Id), New Rectangle(pbZoom.Left, pbZoom.Top, pbZoom.Right, pbZoom.Bottom))
            Else
                UpdateThumb(255)
            End If
            rectDic.Clear()

            AppActivate(AltPP.Id)
            sysTrayIcon.Icon = AltPP?.GetIcon

        Else 'AltPP.Id = 0


        End If

        Debug.Print("tmrTick.Enabled")
        tmrTick.Enabled = True

        prevItem = that.SelectedItem

        If cboAlt.SelectedIndex > 0 Then
            pbZoom.Show()
        Else
            AButton.ActiveOverview = My.Settings.gameOnOverview
        End If

    End Sub

    Dim alreadylaunched As Boolean = False
    Private Async Function reOpenAsWindowed(altPP As AstoniaProcess) As Task

        If alreadylaunched Then
            Return
        End If


        Debug.Print($"runasWindowed: {altPP.Name} {altPP.Id}")

        Dim shortcutlink As String = FileIO.SpecialDirectories.Temp & "\ScalA\tmp.lnk"

        Dim mos As Management.ManagementObject = New Management.ManagementObjectSearcher($“Select * from Win32_Process WHERE ProcessID={altPP.Id}").Get()(0)

        Dim arguments As String = mos("commandline")

        Debug.Print($"arguments:""{arguments}""")
        Debug.Print($"exePath:""{mos("ExecutablePath")}""")

        If arguments = "" Then
            If MessageBox.Show("Access denied!" & vbCrLf &
                           "Elevate ScalA to Administrator?",
                               "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) _
               = DialogResult.Cancel Then Return
            My.Settings.Save()
            ElevateSelf()
            End 'program
            Return
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



        Dim exepath As String = ""
        Try
            exepath = mos("ExecutablePath")
        Catch

        End Try
        Dim oLink As Object
        Try

            oLink = CreateObject("WScript.Shell").CreateShortcut(shortcutlink)

            oLink.TargetPath = exepath
            oLink.Arguments = arguments.Trim() & " -w"
            oLink.WorkingDirectory = exepath.Substring(0, exepath.LastIndexOf("\"))
            oLink.WindowStyle = 1
            oLink.Save()
        Catch ex As Exception
            Return
        End Try

        Dim targetName As String = altPP.Name

        SendMessage(altPP.MainWindowHandle, &H100, Keys.F12, IntPtr.Zero)

        Dim pp As Process

        Dim bat As String = "\noAdmin.bat"
        Dim tmpDir As String = FileIO.SpecialDirectories.Temp & "\ScalA"

        If Not FileIO.FileSystem.DirectoryExists(tmpDir) Then FileIO.FileSystem.CreateDirectory(tmpDir)
        If Not FileIO.FileSystem.FileExists(tmpDir & bat) Then FileIO.FileSystem.WriteAllText(tmpDir & bat, My.Resources.AsInvoker, False)

        pp = New Process With {.StartInfo = New ProcessStartInfo With {.FileName = tmpDir & bat,
                                                                       .Arguments = """" & shortcutlink & """",
                                                                       .WindowStyle = ProcessWindowStyle.Hidden,
                                                                       .CreateNoWindow = True}}
        Try
            alreadylaunched = True
            pp.Start()
        Catch
        Finally
            pp.Dispose()
        End Try



        Me.Cursor = Cursors.WaitCursor
        Dim count As Integer = 0

        While True
            count += 1
            Await Task.Delay(50)
            Dim targetPPs As AstoniaProcess() = AstoniaProcess.Enumerate.Where(Function(ap) ap.Name = targetName).ToArray()
            If targetPPs.Length > 0 AndAlso targetPPs(0) IsNot Nothing AndAlso targetPPs(0).Id <> 0 Then
                PopDropDown(cboAlt)
                cboAlt.SelectedItem = targetPPs(0)
                Exit While
            End If
            If count >= 100 Then
                MessageBox.Show("Windowing failed")
                Exit While
            End If
        End While
        Me.Cursor = Cursors.Default

        If System.IO.File.Exists(shortcutlink) Then
            Debug.Print("Deleting shortcut")
            System.IO.File.Delete(shortcutlink)
        End If

        alreadylaunched = False
    End Function

    Dim AstClientOffset As New Size(0, 0)
    Public Sub RestorePos(altPP As AstoniaProcess)
        If altPP?.IsRunning() Then
            SetWindowPos(altPP.MainWindowHandle, altTopM, rcW.Left, rcW.Top, -1, -1, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate Or SetWindowPosFlags.ASyncWindowPosition)
        End If
        For Each id In restoreDic.Keys
            Dim altAP As AstoniaProcess = New AstoniaProcess(Process.GetProcessById(id))
            If altAP.IsRunning() Then
                SetWindowPos(altAP.MainWindowHandle, 0, restoreDic(id).X, restoreDic(id).Y, -1, -1, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate Or SetWindowPosFlags.ASyncWindowPosition)
            End If
        Next
        restoreDic.Clear()
    End Sub

    Private Sub UpdateTitle()
        Dim titleSuff As String = String.Empty
        Dim traytooltip As String = "ScalA"
        If AltPP?.IsRunning Then
            Try
                titleSuff = " - " & AltPP.MainWindowTitle
                traytooltip = AltPP.MainWindowTitle
            Catch e As Exception
            End Try
        End If
        Me.Text = "ScalA" & titleSuff
        sysTrayIcon.Text = traytooltip.Cap(63)
        With My.Application.Info.Version
            lblTitle.Text = "- ScalA v" & .Major & "." & .Minor & "." & .Build & titleSuff
        End With
    End Sub

    Public Shared zooms() As Size = GetResolutions()

    Private Sub FrmMain_Load(sender As Form, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = True

        If New Version(My.Settings.SettingsVersion) < My.Application.Info.Version Then
            My.Settings.Upgrade()
            My.Settings.SettingsVersion = My.Application.Info.Version.ToString
            My.Settings.Save()
            zooms = GetResolutions()
        End If

        ScalaHandle = Me.Handle

        Debug.Print("mangleSysMenu")
        MangleSysMenu()

        Debug.Print("topmost " & My.Settings.topmost)
        Me.TopMost = My.Settings.topmost
        Me.chkHideMessage.Checked = My.Settings.hideMessage


        'left big in designer to facilitate editing
        cornerNW.Size = New Size(2, 2)
        cornerNE.Size = New Size(2, 2)
        cornerSW.Size = New Size(2, 2)
        cornerSE.Size = New Size(2, 2)

        cmbResolution.Items.AddRange(zooms.Select(Function(ss) ss.Width & "x" & ss.Height).ToArray)
        cmbResolution.SelectedIndex = My.Settings.zoom

        Debug.Print("location " & My.Settings.location.ToString)
        Me.Location = My.Settings.location

        Dim args() As String = Environment.GetCommandLineArgs()

        cboAlt.BeginUpdate()
        cboAlt.Items.Add(New AstoniaProcess()) 'someone
        cboAlt.SelectedIndex = 0
        Dim APlist As List(Of AstoniaProcess) = AstoniaProcess.Enumerate.ToList
        For Each ap As AstoniaProcess In APlist
            cboAlt.Items.Add(ap)
            If args.Count > 1 AndAlso ap.Name = args(1) Then
                Debug.Print($"Selecting '{ap.Name}'")
                cboAlt.SelectedItem = ap
            End If
        Next
        cboAlt.EndUpdate()


        AddAButtons(APlist.Count)
        UpdateButtonLayout(APlist.Count)

        If cboAlt.SelectedIndex = 0 AndAlso args.Count = 1 Then
            Debug.Print("Selecting Default")
            cboAlt.SelectedIndex = If(cboAlt.Items.Count = 2, 1, 0)
        End If

        Debug.Print("updateTitle")
        UpdateTitle()

        Dim progdata As String = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & "\ScalA"
        If My.Settings.links = "" Then
            My.Settings.links = progdata
        End If
        If Not System.IO.Directory.Exists(progdata) Then
            System.IO.Directory.CreateDirectory(progdata)
            System.IO.Directory.CreateDirectory(progdata & "\Example Folder")
        End If
        'copy readme to progdata
        If Not FileIO.FileSystem.FileExists(progdata & "\ReadMe.txt") OrElse My.Resources.ReadMe.Length <> FileIO.FileSystem.GetFileInfo(progdata & "\ReadMe.txt").Length Then
            Debug.Print("Readme is diffrent or nonexistant")
            FileIO.FileSystem.WriteAllText(progdata & "\ReadMe.txt", My.Resources.ReadMe, False, System.Text.Encoding.ASCII)
        End If

#If DEBUG Then
        chkDebug.Visible = True
        Dim test As New ContextMenuStrip
        test.Items.Add(New ToolStripMenuItem("Parse Info", Nothing, AddressOf dBug.ParseInfo))
        test.Items.Add(New ToolStripMenuItem("Reset Hide", Nothing, AddressOf dBug.ResetHide))
        test.Items.Add(New ToolStripMenuItem("ResumeLayout", Nothing, AddressOf dBug.Resumelayout))
        test.Items.Add(New ToolStripMenuItem("Button Info", Nothing, AddressOf dBug.buttonInfo))
        chkDebug.ContextMenuStrip = test
#End If

        'set shield if runing as admin
        If My.User.IsInRole(ApplicationServices.BuiltInRole.Administrator) Then

            'Dim size = SystemInformation.SmallIconSize
            Dim image = LoadImage(IntPtr.Zero, "#106", 1, 16, 16, 0)
            If image <> IntPtr.Zero Then
                Using Ico = Icon.FromHandle(image)
                    btnStart.Image = Ico.ToBitmap
                    DestroyIcon(Ico.Handle)
                End Using
                btnStart.Text = ""
            End If
        End If

        If System.IO.File.Exists(FileIO.SpecialDirectories.Temp & "\ScalA\tmp.lnk") Then
            Debug.Print("Deleting shortcut")
            System.IO.File.Delete(FileIO.SpecialDirectories.Temp & "\ScalA\tmp.lnk")
        End If



    End Sub
    Private Shared Function GetResolutions() As Size()
        Dim reslist As New List(Of Size)
        For Each line As String In My.Settings.resolutions.Split(vbCrLf.ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            Dim parts() As String = line.ToUpper.Split("X")
            Debug.Print(parts(0) & " " & parts(1))
            reslist.Add(New Size(parts(0), parts(1)))
        Next

        Return reslist.ToArray
    End Function



#Region " Move Self "

#If 0 Then
    Private Async Sub MoveSelf(ByVal sender As Control, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnlTitleBar.MouseDown, lblTitle.MouseDown
        'todo: handle frmsettings.alignment 
        If e.Button = MouseButtons.Left Then
            Await Task.Delay(100) 'this to allow double clicks
            sender.Capture = False
            Const WM_NCLBUTTONDOWN As Integer = &HA1
            Const HTCAPTION As Integer = 2
            Dim msg As Message = Message.Create(Me.Handle, WM_NCLBUTTONDOWN, New IntPtr(HTCAPTION), IntPtr.Zero)
            Me.DefWndProc(msg)
        End If
    End Sub
#Else
    Private MovingForm As Boolean
    Private MoveForm_MousePosition As Point

    Public Sub MoveForm_MouseDown(sender As Control, e As MouseEventArgs) Handles pnlTitleBar.MouseDown, lblTitle.MouseDown
        'Me.TopMost = True
        If FrmSettings.chkDoAlign.Checked Then
            If Me.WindowState <> FormWindowState.Maximized AndAlso e.Button = MouseButtons.Left Then
                MovingForm = True
                MoveForm_MousePosition = e.Location
            End If
        Else
            If e.Button = MouseButtons.Left AndAlso e.Clicks = 1 Then
                sender.Capture = False
                tmrTick.Stop()
                tmrMove.Start()
                Dim msg As Message = Message.Create(Me.Handle, WM_NCLBUTTONDOWN, New IntPtr(HTCAPTION), IntPtr.Zero)
                Me.WndProc(msg)
                tmrMove.Stop()
                If cboAlt.SelectedIndex > 0 Then
                    AppActivate(AltPP.Id)
                    tmrTick.Start()
                End If
                Debug.Print("movetimer stopped")
            End If
        End If
    End Sub

    Public Sub MoveForm_MouseMove(sender As Control, e As MouseEventArgs) Handles _
    pnlTitleBar.MouseMove, lblTitle.MouseMove ' Add more handles here (Example: PictureBox1.MouseMove)
        If MovingForm Then
            If AltPP IsNot Nothing AndAlso Not FrmSettings.chkDoAlign.Checked Then
                Static moveable As Boolean = True
                If moveable Then
                    Task.Run(Sub()
                                 moveable = False
                                 AltPP?.CenterWindowPos(ScalaHandle,
                                                    Me.Left + pbZoom.Left + (pbZoom.Width / 2),
                                                    Me.Top + pbZoom.Top + (pbZoom.Height / 2),
                                                    SetWindowPosFlags.DoNotActivate)
                                 moveable = True
                             End Sub)
                End If
            End If
            Dim newoffset As Point = e.Location - MoveForm_MousePosition
            Me.Location += newoffset
            If FrmSettings.chkDoAlign.Checked Then
                FrmSettings.ScalaMoved += newoffset
            End If
        End If
    End Sub

    Public Sub MoveForm_MouseUp(sender As Control, e As MouseEventArgs) Handles pnlTitleBar.MouseUp, lblTitle.MouseUp
        ' only fires when settings.chkAlign is on
        If e.Button = MouseButtons.Left Then
            Debug.Print("Mouseup")
            MovingForm = False
            If AltPP?.IsRunning AndAlso Not FrmSettings.chkDoAlign.Checked Then
                AltPP?.CenterWindowPos(ScalaHandle,
                                                    Me.Left + pbZoom.Left + (pbZoom.Width / 2),
                                                    Me.Top + pbZoom.Top + (pbZoom.Height / 2),
                                                    SetWindowPosFlags.DoNotActivate)
            End If
        End If
    End Sub
    Private Sub FrmMain_ResizeEnd(sender As Form, e As EventArgs) Handles Me.ResizeEnd
        Debug.Print("ResizeEnd")
    End Sub
    Private Sub tmrMove_Tick(sender As Object, e As EventArgs) Handles tmrMove.Tick
        If AltPP?.IsRunning Then
            Static moveable As Boolean = True
            If moveable Then
                Task.Run(Sub()
                             moveable = False
                             AltPP?.CenterWindowPos(ScalaHandle,
                                                    Me.Left + pbZoom.Left + (pbZoom.Width / 2),
                                                    Me.Top + pbZoom.Top + (pbZoom.Height / 2),
                                                    SetWindowPosFlags.DoNotActivate)
                             moveable = True
                         End Sub)
            End If
        End If
    End Sub
#End If

#End Region


    Private Sub FrmMain_Closing(sender As Form, e As EventArgs) Handles Me.Closing
        RestorePos(AltPP)
        If Me.WindowState = FormWindowState.Normal Then
            My.Settings.location = Me.Location
        End If
        Hotkey.UnregHotkey(Me)
    End Sub


    Public Function GetActiveProcessID() As UInteger
        Dim hWnd As IntPtr = GetForegroundWindow()
        Dim ProcessID As UInteger = 0

        GetWindowThreadProcessId(hWnd, ProcessID)

        Return ProcessID
    End Function

    Dim rcW As Rectangle ' windowrect
    Dim rcC As Rectangle ' clientrect
    Public newX As Integer
    Public newY As Integer
    Private ScalaHandle As IntPtr = Me.Handle
    Private storedY As Integer = 0
    Private wasVisible As Boolean = True

    Private Sub tmrTick_Tick(sender As Timer, e As EventArgs) Handles tmrTick.Tick

        If Not AltPP?.IsRunning() Then
            Debug.Print("Not AltPP?.IsRunning()")
            If Not My.Settings.CycleOnClose Then
                tmrTick.Enabled = False
                cboAlt.SelectedIndex = 0
                tmrOverview.Enabled = True
                Exit Sub
            Else 'CycleOnClose
                Dim requestedIndex = cboAlt.SelectedIndex + 1
                Debug.Print($"CycleOnCLose: {cboAlt.SelectedIndex} {requestedIndex}")
                Debug.Print($"              {cboAlt.Items.Count} > {requestedIndex} = {cboAlt.Items.Count > requestedIndex}")
                If requestedIndex < cboAlt.Items.Count Then
                    cboAlt.SelectedIndex = requestedIndex
                Else 'add new items if available
                    'Dim comboboxitems As List(Of AstoniaProcess) = (From item As AstoniaProcess In cboAlt.Items Select value = item).ToList

                    Dim newItems = AstoniaProcess.Enumerate.Except(
                                                 (From item As AstoniaProcess In cboAlt.Items Select value = item),
                                                   New AstoniaProcessEqualityComparer)
                    Debug.Print($"newitems.count {newItems.Count}")
                    If newItems.Any() Then
                        Debug.Print("adding new items")
                        cboAlt.Items.AddRange(newItems.ToArray)
                        cboAlt.SelectedIndex = requestedIndex
                    Else
                        'PopDropDown()
                        cboAlt.SelectedIndex = If(cboAlt.Items.Count = 1, 0, 1)
                    End If
                End If
            End If
        End If

        UpdateTitle()

        If Me.WindowState = FormWindowState.Minimized Then
            Exit Sub
        End If

        Dim pci As New CURSORINFO With {.cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(CURSORINFO))}
        GetCursorInfo(pci)
        If pci.flags <> 0 Then ' cursor is visible
            If Not wasVisible AndAlso AltPP?.IsActive() Then
                Debug.Print("scrollthumb released")
                If storedY <> pci.ptScreenpos.y Then
                    Debug.Print("scrollthumb moved")
                    Dim factor As Double = pbZoom.Height / rcC.Height
                    Dim movedY As Integer = storedY + ((pci.ptScreenpos.y - storedY) * factor)
                    If movedY >= Me.Bottom Then movedY = Me.Bottom - 2
                    Cursor.Position = New Point(pci.ptScreenpos.x, movedY)
                End If
            End If
            storedY = pci.ptScreenpos.y

            wasVisible = True
        Else ' cursor is hidden
            wasVisible = False
            Exit Sub ' do not move astonia when cursor is hidden. fixes scrollbar thumb.
            ' note there is a client bug where using thumb will intermittently cause it to jump down wildly
        End If


        If pbZoom.Contains(MousePosition) Then 'todo: replace with mousehook?

            Dim ptZ As Point = pbZoom.Location

            ClientToScreen(Me.Handle, ptZ)
            newX = MousePosition.X.Map(ptZ.X, ptZ.X + pbZoom.Width, ptZ.X, ptZ.X + pbZoom.Width - rcC.Width) - AstClientOffset.Width - My.Settings.offset.X
            newY = MousePosition.Y.Map(ptZ.Y, ptZ.Y + pbZoom.Height, ptZ.Y, ptZ.Y + pbZoom.Height - rcC.Height) - AstClientOffset.Height - My.Settings.offset.Y

            Task.Run(Sub()
                         Try
                             Dim flags As SetWindowPosFlags = SetWindowPosFlags.IgnoreResize Or
                                                              SetWindowPosFlags.DoNotActivate Or
                                                              SetWindowPosFlags.ASyncWindowPosition
                             If QlCtxIsOpen Then flags = flags Or SetWindowPosFlags.IgnoreZOrder
                             SetWindowPos(AltPP?.MainWindowHandle, ScalaHandle, newX, newY, -1, -1, flags)
                         Catch ex As Exception
                         End Try
                     End Sub)

        End If
    End Sub

    Private Sub CmbResolution_MouseDown(sender As ComboBox, e As MouseEventArgs) Handles cmbResolution.MouseDown
        If e.Button = MouseButtons.Right Then FrmSettings.Show()
    End Sub

    Public Sub CmbResolution_SelectedIndexChanged(sender As ComboBox, e As EventArgs) Handles cmbResolution.SelectedIndexChanged
        Debug.Print("cboResolution_SelectedIndexChanged")
        My.Settings.zoom = sender.SelectedIndex
        If AltPP?.IsRunning Then
            newX = Me.Left + pbZoom.Left - AstClientOffset.Width - My.Settings.offset.X
            newY = Me.Top + pbZoom.Top - AstClientOffset.Height - My.Settings.offset.Y
            Debug.Print("SetWindowPos")
            SetWindowPos(AltPP.MainWindowHandle, Me.Handle, newX, newY, -1, -1, SetWindowPosFlags.IgnoreResize + SetWindowPosFlags.DoNotActivate)
        End If
        Debug.Print("reZoom")
        ReZoom(zooms(My.Settings.zoom))
    End Sub

    Private Sub ReZoom(newSize As Size)
        Me.SuspendLayout()
        If Me.WindowState <> FormWindowState.Maximized Then
            Me.Size = New Size(newSize.Width + 2, newSize.Height + 26)
            pbZoom.Left = 1
            pnlOverview.Left = 1
            pbZoom.Size = newSize
            pnlOverview.Size = newSize
            cmbResolution.Enabled = True
        Else 'FormWindowState.Maximized
            pbZoom.Left = 0
            pnlOverview.Left = 0
            pbZoom.Width = newSize.Width + 1
            pbZoom.Height = newSize.Height - 25
            pnlOverview.Size = pbZoom.Size
            cmbResolution.Enabled = False
        End If
        If cboAlt.SelectedIndex <> 0 Then
            Debug.Print("updateThumb")
            UpdateThumb(255)
        End If
        Me.ResumeLayout(True)
        pnlTitleBar.Width = pnlButtons.Left - pnlTitleBar.Left

        cornerNW.Location = New Point(0, 0)
        cornerNE.Location = New Point(Me.Width - 2, 0)
        cornerSW.Location = New Point(0, Me.Height - 2)
        cornerSE.Location = New Point(Me.Width - 2, Me.Height - 2)

        If Me.WindowState <> FormWindowState.Maximized AndAlso My.Settings.roundCorners Then

            cornerNW.Visible = True
            cornerNE.Visible = True
            cornerSW.Visible = True
            cornerSE.Visible = True

        Else 'maximized
            cornerNW.Visible = False
            cornerNE.Visible = False
            cornerSW.Visible = False
            cornerSE.Visible = False
        End If

    End Sub

#Region " SysMenu "

    Private hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
    Public Sub MangleSysMenu()
        Const GWL_STYLE As Integer = -16
        Debug.Print("SetWindowLong")
        SetWindowLong(Me.Handle, GWL_STYLE, GetWindowLong(Me.Handle, GWL_STYLE) Or WindowStyles.WS_SYSMENU Or WindowStyles.WS_MINIMIZEBOX) 'Enable SysMenu and MinimizeBox 

        hSysMenu = GetSystemMenu(Me.Handle, False)
        If hSysMenu Then
            ModifyMenuA(hSysMenu, SC_CLOSE, MF_BYCOMMAND, SC_CLOSE, "&Close") 'remove Alt-F4
            SetMenuItemBitmaps(hSysMenu, SC_CLOSE, MF_BYCOMMAND, 5, IntPtr.Zero) 're-add close icon
            SetMenuDefaultItem(hSysMenu, SC_CLOSE, MF_BYCOMMAND)
            'RemoveMenu(hSysMenu, SC_SIZE, MF_BYCOMMAND) 'remove size menuitem
            ModifyMenuA(hSysMenu, SC_SIZE, MF_BYCOMMAND Or 1, SC_SIZE, "&Size") 'disable size item
            InsertMenuA(hSysMenu, 0, MF_SEPARATOR Or MF_BYPOSITION, 0, String.Empty)
            InsertMenuA(hSysMenu, 0, MF_BYPOSITION, 1337, "Settings")
            SetMenuItemBitmaps(hSysMenu, 0, MF_BYPOSITION, CType(My.Resources.gear_wheel, Bitmap).GetHbitmap(Color.Red), Nothing)
        End If
    End Sub

    Public Sub ShowSysMenu(sender As Control, e As MouseEventArgs) Handles pnlTitleBar.MouseUp, lblTitle.MouseUp, btnMin.MouseUp, btnMax.MouseUp
        If e Is Nothing OrElse e.Button = MouseButtons.Right Then
            Debug.Print("ShowSysMenu hSysMenu=" & hSysMenu.ToString)
            pbZoom.Visible = False
            AButton.ActiveOverview = False

            Dim cmd As Integer = TrackPopupMenuEx(hSysMenu, TPM_RIGHTBUTTON Or TPM_RETURNCMD, MousePosition.X, MousePosition.Y, Me.Handle, Nothing)

            If cmd > 0 Then
                Debug.Print("SendMessage " & cmd)
                SendMessage(Me.Handle, WM_SYSCOMMAND, cmd, IntPtr.Zero)
            End If

            If cboAlt.SelectedIndex > 0 Then
                pbZoom.Visible = True
            End If
            AButton.ActiveOverview = My.Settings.gameOnOverview
        End If
    End Sub

#End Region

    Dim wasMaximized As Boolean = False 'todo find out what this does and see if it can be removed
    Dim minByMenu As Boolean = False 'todo find out what this does and see if it can be removed

    'Const WM_GETMINMAXINFO = &H24
    ''<StructLayout(LayoutKind.Sequential)>
    ''Private Structure MINMAXINFO
    ''    Dim ptReserved As Point
    ''    Dim ptMaxSize As Point
    ''    Dim ptMaxPosition As Point
    ''    Dim ptMinTrackSize As Point
    ''    Dim ptMaxTrackSize As Point
    ''End Structure

    Protected Overrides Sub WndProc(ByRef m As Message)
        'If m.Msg = &H2E0 Then
        '    Debug.Print("&H2E0")
        '    Dim b As Rectangle = Screen.FromControl(Me).WorkingArea ' //where mForm Is Application.OpenForms[0], this Is specific to my applications use case.
        '    b.X = 0 '; //The bounds will Try To base it On Monitor 1 For example, 1920x1080, To another 1920x1080 monitor On right, will Set X To 1080, making it show up On the monitor On right, outside its bounds.
        '    b.Y = 0 '; //same As b.X
        '    MaximizedBounds = b '; //Apply it To MaximizedBounds
        '    m.Result = New IntPtr(0) '; //Tell WndProc it did stuff

        'Else


        Select Case m.Msg
            Case Hotkey.WM_HOTKEY
                Select Case m.WParam
                    Case 1 'ctrl-tab
                        'only preform switch when astonia or scala Is active
                        Dim activeID = GetActiveProcessID()
                        Debug.Print("aID " & activeID & " selfPID " & scalaPID)
                        If activeID = scalaPID OrElse Process.GetProcessById(activeID).IsClassNameIn(My.Settings.className) Then
                            If Me.WindowState = FormWindowState.Minimized Then
                                Me.WindowState = FormWindowState.Normal
                            End If
                            Me.Activate()
                            Me.BringToFront()
                            btnStart.PerformClick()
                        End If
                    Case 2 'ctrl-space

                        Dim requestedIndex = cboAlt.SelectedIndex + 1
                        Debug.Print($"hotkey2: {cboAlt.SelectedIndex} {requestedIndex}")
                        Debug.Print($"         {cboAlt.Items.Count} > {requestedIndex} = {cboAlt.Items.Count > requestedIndex}")
                        If requestedIndex < cboAlt.Items.Count Then
                            cboAlt.SelectedIndex = requestedIndex
                        Else 'add new items if available
                            'Dim comboboxitems As List(Of AstoniaProcess) = (From item As AstoniaProcess In cboAlt.Items Select value = item).ToList

                            Dim newItems = AstoniaProcess.Enumerate.Except(
                                                 (From item As AstoniaProcess In cboAlt.Items Select value = item),
                                                   New AstoniaProcessEqualityComparer)
                            Debug.Print($"newitems.count {newItems.Count}")
                            If newItems.Any() Then
                                Debug.Print("adding new items")
                                cboAlt.Items.AddRange(newItems.ToArray)
                                cboAlt.SelectedIndex = requestedIndex
                            Else
                                'PopDropDown()
                                cboAlt.SelectedIndex = If(cboAlt.Items.Count = 1, 0, 1)
                            End If
                        End If
                    Case 3 'ctrl-shift-space
                        Dim requestedindex = cboAlt.SelectedIndex - 1
                        Debug.Print($"hotkey3: {cboAlt.SelectedIndex} {requestedindex}")
                        Debug.Print($"         {cboAlt.Items.Count} > {requestedindex} = {cboAlt.Items.Count > requestedindex}")
                        If requestedindex < 1 Then
                            PopDropDown(cboAlt)
                            requestedindex = cboAlt.Items.Count - 1
                        End If
                        cboAlt.SelectedIndex = requestedindex
                End Select
            Case WM_SYSCOMMAND
                Select Case m.WParam
                    Case SC_RESTORE
                        Debug.Print("SC_RESTORE")
                        SetWindowLong(Me.Handle, GWL_HWNDPARENT, AltPP.MainWindowHandle)
                        'Me.ShowInTaskbar = False
                        Debug.Print("wasMax " & wasMaximized)
                        If wasMaximized Then
                            btnMax.PerformClick()
                            Exit Sub
                        End If
                        If minByMenu Then
                            Me.Location = restoreLoc
                        End If
                        minByMenu = False
                    Case SC_MAXIMIZE
                        Debug.Print("SC_MAXIMIZE " & m.LParam.ToString)

                        btnMax.PerformClick()

                        Debug.Print("wasMax " & wasMaximized)

                        m.Result = 0
                    Case SC_MINIMIZE
                        Debug.Print("SC_MINIMIZE")
                        minByMenu = True
                        wasMaximized = (Me.WindowState = FormWindowState.Maximized)
                        Debug.Print("wasMax " & wasMaximized)
                        If Not wasMaximized Then
                            restoreLoc = Me.Location
                            Debug.Print("restoreLoc " & restoreLoc.ToString)
                        End If
                        SetWindowLong(Me.Handle, GWL_HWNDPARENT, restoreParent)
                        RestorePos(AltPP)
                    Case 1337
                        Debug.Print("Settings called by 1337")
                        FrmSettings.Show()
                End Select


        End Select




        MyBase.WndProc(m)  ' allow form to process this message
    End Sub

    ReadOnly startThumbsDict As New Dictionary(Of Integer, IntPtr)
    ReadOnly opaDict As New Dictionary(Of Integer, Byte)
    ReadOnly rectDic As New Dictionary(Of Integer, Rectangle)
    ReadOnly restoreDic As New Dictionary(Of Integer, Point)

    Const dimmed As Byte = 240
    Private counter As Integer = 0

    Dim AOBusy As Boolean = False
    Private Async Sub TmrOverview_Tick(sender As Timer, e As EventArgs) Handles tmrOverview.Tick

        'Debug.Print("tmrStartup.Tick")
#If DEBUG Then
        chkDebug.Text = counter
#End If

        Dim i As Integer = 0
        Dim alts As List(Of AstoniaProcess) = AstoniaProcess.Enumerate _
                    .Where(Function(ap) Not ({"", "Someone"}.Contains(ap.Name) AndAlso ap.MainWindowTitle.Count(Function(c As Char) c = "-"c) < 2)).ToList
        pnlOverview.SuspendLayout()
        UpdateButtonLayout(alts.Count)
        For Each but As AButton In pnlOverview.Controls.OfType(Of AButton).Where(Function(b) b.Visible) 'loop through visible buttons
            If i < alts.Count Then
                Dim name As String = alts(i).Name
                While name = "" OrElse name = "Someone" OrElse alts(i).MainWindowTitle.Count(Function(c As Char) c = "-"c) < 2
                    'don't list "Someone"s or clients in the process of logging in ("-"<2)
                    i += 1
                    If i >= alts.Count Then
                        Exit For
                    End If
                    name = alts(i).Name
                End While

                but.Text = name
                but.Tag = alts(i)
                If alts(i)?.IsActive() Then
                    but.Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Bold)
                    but.Select()
                Else
                    but.Font = New Font("Microsoft Sans Serif", 8.25)
                End If
                If counter = 0 Then
                    Using ico As Bitmap = alts(i).GetIcon?.ToBitmap
                        If ico IsNot Nothing Then
                            but.BackgroundImage = New Bitmap(ico, New Size(16, 16))
                        Else
                            but.BackgroundImage = Nothing
                        End If
                    End Using
                End If
                but.ContextMenuStrip = cmsAlt

                'activeNameList.Add(name)
                If Not startThumbsDict.ContainsKey(alts(i).Id) Then
                    startThumbsDict(alts(i).Id) = IntPtr.Zero
                    DwmRegisterThumbnail(Me.Handle, alts(i).MainWindowHandle, startThumbsDict(alts(i).Id))
                    Debug.Print("registered thumb " & startThumbsDict(alts(i).Id).ToString & " " & name)
                End If

                rectDic(alts(i).Id) = but.ThumbRECT
                Dim prp As New DWM_THUMBNAIL_PROPERTIES With {
                                   .dwFlags = DwmThumbnailFlags.DWM_TNP_OPACITY Or DwmThumbnailFlags.DWM_TNP_SOURCECLIENTAREAONLY Or DwmThumbnailFlags.DWM_TNP_VISIBLE Or DwmThumbnailFlags.DWM_TNP_RECTDESTINATION,
                                   .opacity = opaDict.GetValueOrDefault(alts(i).Id, &HFF),
                                   .fSourceClientAreaOnly = True,
                                   .fVisible = True,
                                   .rcDestination = rectDic(alts(i).Id)
                               }
                DwmUpdateThumbnailProperties(startThumbsDict(alts(i).Id), prp)

                'If Not MovingForm Then but.Image = alts(i).getHealthbar() 'couses window moving to stutter.
                'Task.Run(Sub() but.Image = but.Tag.getHealthbar()) 'smooth window move. causes excetpion in unamanged code on but.image.
                If counter = 0 OrElse but.Image Is Nothing Then
#Disable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed
                    Task.Run(Sub()
                                 Try
                                     Me.BeginInvoke(updateImage, {but, CType(but.Tag, AstoniaProcess).getHealthbar()})
                                 Catch
                                 End Try
                             End Sub)
#Enable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed
                End If
                If My.Settings.gameOnOverview Then 'todo move this to seperate timer and make async
                    Dim rccB As Rectangle
                    GetClientRect(but?.Tag.MainWindowHandle, rccB)

                    Dim pci As New CURSORINFO With {.cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(CURSORINFO))}
                    GetCursorInfo(pci)
                    If pci.flags <> 0 Then ' cursor is visible
                        If Not wasVisible AndAlso but.Tag?.IsActive() Then
                            Debug.Print("scrollthumb released")
                            If storedY <> pci.ptScreenpos.y Then
                                Debug.Print("scrollthumb moved")
                                Dim factor As Double = but.ThumbRectangle.Height / rccB.Height
                                Dim movedY As Integer = storedY + ((pci.ptScreenpos.y - storedY) * factor)
                                If movedY >= Me.Bottom Then movedY = Me.Bottom - 2
                                Cursor.Position = New Point(pci.ptScreenpos.x, movedY)
                            End If
                        End If
                        storedY = pci.ptScreenpos.y

                        wasVisible = True
                    Else ' cursor is hidden
                        wasVisible = False
                        Exit For ' do not move astonia when cursor is hidden. fixes scrollbar thumb.
                        ' note there is a client bug where using thumb will intermittently cause it to jump down wildly
                    End If

                    If Not AOBusy AndAlso but.ThumbContains(MousePosition) Then
                        SetWindowLong(Me.Handle, GWL_HWNDPARENT, CType(but.Tag, AstoniaProcess).MainWindowHandle)

                        Dim ptZB As Point = but.ThumbRECT.Location
                        Dim rcwB As Rectangle


                        GetWindowRect(but?.Tag.MainWindowHandle, rcwB)
                        If but.Tag?.id IsNot Nothing AndAlso Not restoreDic.ContainsKey(but.Tag?.Id) Then
                            restoreDic.Add(but.Tag?.id, rcwB.Location)
                        End If

                        Dim pttB As Point

                        ClientToScreen(but?.Tag.MainWindowHandle, pttB)

                        Dim AstClientOffsetB = New Size(pttB.X - rcwB.Left, pttB.Y - rcwB.Top)

                        ClientToScreen(Me.Handle, ptZB)
                        Dim newXB = MousePosition.X.Map(ptZB.X, ptZB.X + but.ThumbRectangle.Width, ptZB.X, ptZB.X + but.ThumbRECT.Width - but.ThumbRECT.X - rccB.Width) - AstClientOffsetB.Width - My.Settings.offset.X
                        Dim newYB = MousePosition.Y.Map(ptZB.Y, ptZB.Y + but.ThumbRectangle.Height, ptZB.Y, ptZB.Y + but.ThumbRECT.Height - but.ThumbRECT.Top - rccB.Height) - AstClientOffsetB.Height - My.Settings.offset.Y


                        AOBusy = True
                        Await Task.Run(Sub()
                                           Try
                                               Dim flags As SetWindowPosFlags = SetWindowPosFlags.IgnoreResize Or
                                                                                SetWindowPosFlags.DoNotActivate Or
                                                                                SetWindowPosFlags.ASyncWindowPosition
                                               If QlCtxIsOpen Then flags = flags Or SetWindowPosFlags.IgnoreZOrder
                                               SetWindowPos(but.Tag?.MainWindowHandle, ScalaHandle, newXB, newYB, -1, -1, flags)
                                           Catch ex As Exception
                                           End Try
                                       End Sub)
                        AOBusy = False
                    End If 'but.ThumbContains(MousePosition)
                End If 'gameonoverview


            Else ' i >= alts.Count
                but.Text = String.Empty
                but.Tag = Nothing 'New AstoniaProcess(Nothing)
                but.ContextMenuStrip = cmsQuickLaunch
                but.BackgroundImage = Nothing
                but.Image = Nothing
            End If
            i += 1
            'but.ResumeLayout()
        Next but

        ' Dim purgeList As List(Of Integer) = startThumbsDict.Keys.Except(alts.Select(Function(ap) ap.Id)).ToList
        For Each ppid As Integer In startThumbsDict.Keys.Except(alts.Select(Function(ap) ap.Id)).ToList 'purgeList
            Debug.Print("unregister thumb " & startThumbsDict(ppid).ToString)
            DwmUnregisterThumbnail(startThumbsDict(ppid))
            startThumbsDict.Remove(ppid)
            rectDic.Remove(ppid)
        Next

        pnlOverview.ResumeLayout(True)
        counter += 1
        If counter > 11 Then counter = 0
    End Sub
    Delegate Sub updateImageDelegate(obj As Object, bm As Bitmap)
    Private Shared ReadOnly updateImage As updateImageDelegate = New updateImageDelegate(AddressOf updateImageMethod)
    Private Shared Sub updateImageMethod(obj As Object, bm As Bitmap) 'used by button and toolstripmenuitem
        If obj Is Nothing Then Exit Sub
        If TypeOf obj Is Button Then
            CType(obj, Button).Image = bm
        ElseIf TypeOf obj Is ToolStripItem Then
            CType(obj, ToolStripItem).Image = bm
        Else
            obj.image = bm
        End If
    End Sub
    Private Function getNextPerfectSquare(num As Integer)

        Return 16
    End Function
    Private Sub AddAButtons(count As Integer)
        Dim sqNum As Integer = getNextPerfectSquare(count)
        pnlOverview.SuspendLayout()
        For i As Integer = 1 To sqNum
            Dim but As AButton = New AButton(i, 0, 0, 200, 150)

            AddHandler but.Click, AddressOf BtnAlt_Click
            AddHandler but.MouseDown, AddressOf BtnAlt_MouseDown
            pnlOverview.Controls.Add(but)
        Next i
        pnlOverview.ResumeLayout()
    End Sub
    Private Sub UpdateButtonLayout(count As Integer)
        pnlOverview.SuspendLayout()

        Dim numrows As Integer = 2
        Select Case count + If(My.Settings.hideMessage, 0, 1)
            Case 0 To 4
                numrows = 2
            Case 5 To 9
                numrows = 3
            Case Else
                numrows = 4
        End Select

        Dim newSZ As New Size(pbZoom.Size.Width / numrows, pbZoom.Size.Height / numrows)
        Dim widthTooMuch As Boolean = False
        Dim heightTooMuch As Boolean = False

        If newSZ.Width * numrows > pbZoom.Width Then widthTooMuch = True
        If newSZ.Height * numrows > pbZoom.Height Then heightTooMuch = True


        Dim i = If(My.Settings.hideMessage, 1, 2)
        For Each but As AButton In pnlOverview.Controls.OfType(Of AButton)
            If i <= numrows ^ 2 Then
                but.Size = newSZ
                If widthTooMuch AndAlso i Mod numrows = 0 Then but.Width -= 1 'last column
                If heightTooMuch AndAlso i > (numrows - 1) * numrows Then but.Height -= 1 'last row
                but.Visible = True
            Else
                but.Visible = False
            End If
            i += 1
        Next

        pnlMessage.Size = newSZ
        pbMessage.Size = newSZ
        chkHideMessage.Location = New Point(pnlMessage.Width - chkHideMessage.Width, pnlMessage.Height - chkHideMessage.Height)

        pnlOverview.ResumeLayout(True)
    End Sub

    Private Sub BtnQuit_MouseEnter(sender As Button, e As EventArgs) Handles btnQuit.MouseEnter
        sender.ForeColor = SystemColors.Control
        sender.BackColor = Color.Red
    End Sub

    Private Sub BtnQuit_MouseLeave(sender As Button, e As EventArgs) Handles btnQuit.MouseLeave
        sender.ForeColor = SystemColors.ControlText
        sender.BackColor = Color.Transparent
    End Sub

    Private Sub BtnQuit_Click(sender As Button, e As EventArgs) Handles btnQuit.Click
        Me.Close()
    End Sub

    Private Sub BtnMin_Click(sender As Button, e As EventArgs) Handles btnMin.Click
        Debug.Print("btnMin_Click")
        wasMaximized = (Me.WindowState = FormWindowState.Maximized)
        If Not wasMaximized Then
            restoreLoc = Me.Location
            Debug.Print("restoreLoc " & restoreLoc.ToString)
        End If
        SetWindowLong(Me.Handle, GWL_HWNDPARENT, restoreParent)
        Me.WindowState = FormWindowState.Minimized

        RestorePos(AltPP)

    End Sub
    Private Sub BtnAlt_Click(sender As AButton, e As EventArgs) ' Handles AButton.click
        If sender.Text = String.Empty Then
            'show cms
            cmsQuickLaunch.Show(sender, sender.PointToClient(MousePosition))
            Exit Sub
        End If
        If My.Settings.gameOnOverview Then
            AButton.ActiveOverview = True
            Exit Sub
        End If
        tmrOverview.Enabled = False
        Debug.Print("tmrStartup.stop")
        If Not cboAlt.Items.Contains(sender.Tag) Then
            PopDropDown(cboAlt)
        End If
        cboAlt.SelectedItem = sender.Tag
    End Sub
    Private Sub BtnAlt_MouseDown(sender As Button, e As MouseEventArgs) ' handles AButton.mousedown
        Debug.Print($"MouseDown {e.Button}")
        Select Case e.Button
            Case MouseButtons.XButton1, MouseButtons.XButton2
                sender.Select()
                Try
                    If sender.Tag IsNot Nothing AndAlso sender.Tag.id <> 0 Then AppActivate(sender.Tag.id)
                Catch ex As Exception

                End Try
        End Select
    End Sub
    Private Sub BtnAlt_MouseEnter(sender As AButton, e As EventArgs) ' Handles AButton.MouseEnter
        Try
            opaDict(CType(sender.Tag, AstoniaProcess).Id) = dimmed
        Catch
        End Try
    End Sub
    Private Sub BtnAlt_MouseLeave(sender As AButton, e As EventArgs) ' Handles AButton.MouseLeave
        Try
            opaDict(CType(sender.Tag, AstoniaProcess).Id) = &HFF
        Catch
        End Try
    End Sub

    Private Sub ChkHideMessage_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkHideMessage.CheckedChanged
        Debug.Print("chkHideMessage " & sender.Checked)
        If sender.Checked Then
            'btnAlt9.Visible = True
            pnlMessage.Visible = False
            My.Settings.hideMessage = True
        Else
            pnlMessage.Visible = True
            My.Settings.hideMessage = False
        End If
    End Sub

    Dim restoreLoc As Point
    Private Sub BtnMax_Click(sender As Button, e As EventArgs) Handles btnMax.Click
        Debug.Print("btnMax_Click")
        '🗖,🗗,⧠
        If Me.WindowState <> FormWindowState.Maximized Then
            'go maximized
            sender.Text = "🗗"
            wasMaximized = True
            For Each scrn As Screen In Screen.AllScreens
                If scrn.Bounds.Contains(MousePosition) Then
                    Debug.Print("screen workarea " & scrn.WorkingArea.ToString)
                    Debug.Print("screen bounds " & scrn.Bounds.ToString)
                    Me.MaximizedBounds = New Rectangle(scrn.WorkingArea.Left - scrn.Bounds.Left, scrn.WorkingArea.Top - scrn.Bounds.Top, scrn.WorkingArea.Width, scrn.WorkingArea.Height)
                    Debug.Print("new maxbound " & MaximizedBounds.ToString)
                    If Me.WindowState = FormWindowState.Normal Then
                        restoreLoc = Me.Location
                        Debug.Print("restoreLoc " & restoreLoc.ToString)
                    End If
                    Me.Location = scrn.WorkingArea.Location
                    Me.WindowState = FormWindowState.Maximized
                    Exit For
                End If
            Next
            ReZoom(New Size(Me.MaximizedBounds.Width, Me.MaximizedBounds.Height))
        Else 'go normal
            Me.WindowState = FormWindowState.Normal
            sender.Text = "⧠"
            Me.Location = restoreLoc
            wasMaximized = False
            ReZoom(zooms(cmbResolution.SelectedIndex))
            AltPP?.CenterWindowPos(ScalaHandle, Me.Left + pbZoom.Left + (pbZoom.Width / 2), Me.Top + pbZoom.Top + (pbZoom.Height / 2))
        End If
    End Sub

    Private Sub BtnStart_Click(sender As Button, e As EventArgs) Handles btnStart.Click
        cboAlt.SelectedIndex = 0
        RestorePos(AltPP)
        pnlOverview.Controls.OfType(Of AButton).First.Select()
    End Sub

    ReadOnly scalaPID As Integer = Process.GetCurrentProcess().Id
    Private Sub TmrActive_Tick(sender As Timer, e As EventArgs) Handles tmrActive.Tick

        Try
            Dim activeID As Integer = GetActiveProcessID()
            If activeID = scalaPID OrElse activeID = AltPP?.Id Then
                lblTitle.ForeColor = SystemColors.ControlText
                btnMax.ForeColor = SystemColors.ControlText
                btnMin.ForeColor = SystemColors.ControlText
                btnStart.ForeColor = SystemColors.ControlText
                cboAlt.ForeColor = SystemColors.ControlText
                cmbResolution.ForeColor = SystemColors.ControlText
                'btnQuit.ForeColor = SystemColors.ControlText
            Else
                lblTitle.ForeColor = Color.FromArgb(&HFF666666)
                btnMax.ForeColor = Color.FromArgb(&HFF666666)
                btnMin.ForeColor = Color.FromArgb(&HFF666666)
                btnStart.ForeColor = Color.FromArgb(&HFF666666)
                cboAlt.ForeColor = Color.FromArgb(&HFF666666)
                cmbResolution.ForeColor = Color.FromArgb(&HFF666666)
                'btnQuit.ForeColor = Color.FromArgb(&HFF666666)
            End If
            If (activeID = scalaPID OrElse Process.GetProcessById(activeID).IsClassNameIn(My.Settings.className)) Then
                If My.Settings.SwitchToOverview Then
                    Hotkey.RegisterHotkey(Me, 1, Hotkey.KeyModifier.NoRepeat Or My.Settings.StoCtrl Or My.Settings.StoShift Or My.Settings.StoAlt, My.Settings.StoKey)
                Else
                    Hotkey.UnregHotkey(Me, 1)
                End If
                If My.Settings.CycleAlt Then
                    Hotkey.RegisterHotkey(Me, 2, Hotkey.KeyModifier.NoRepeat Or My.Settings.CycleAltKeyFwd Or My.Settings.CycleShiftKeyFwd Or My.Settings.CycleCtrlKeyFwd, My.Settings.CycleKeyFwd)
                    Hotkey.RegisterHotkey(Me, 3, Hotkey.KeyModifier.NoRepeat Or My.Settings.CycleAltKeyBwd Or My.Settings.CycleShiftKeyBwd Or My.Settings.CycleCtrlKeyBwd, My.Settings.CycleKeyBwd)
                Else
                    Hotkey.UnregHotkey(Me, 2)
                    Hotkey.UnregHotkey(Me, 3)
                End If
            Else
                Hotkey.UnregHotkey(Me)
            End If
        Catch ex As Exception
            lblTitle.ForeColor = Color.FromArgb(&HFF666666)
            btnMax.ForeColor = Color.FromArgb(&HFF666666)
            btnMin.ForeColor = Color.FromArgb(&HFF666666)
            btnStart.ForeColor = Color.FromArgb(&HFF666666)
            cboAlt.ForeColor = Color.FromArgb(&HFF666666)
            cmbResolution.ForeColor = Color.FromArgb(&HFF666666)
            'btnQuit.ForeColor = Color.FromArgb(&HFF666666)
            Hotkey.UnregHotkey(Me)

        End Try
    End Sub

    Private Sub FrmMain_MouseDoubleClick(sender As Control, e As MouseEventArgs) Handles pnlTitleBar.DoubleClick, lblTitle.DoubleClick
        Debug.Print("title_DoubleClick")
        If e.Button = MouseButtons.Left Then btnMax.PerformClick()
    End Sub




    Public Sub ElevateSelf()

        Dim procStartInfo As New ProcessStartInfo With {
            .UseShellExecute = True,
            .FileName = Environment.GetCommandLineArgs()(0),
            .Arguments = """" & CType(Me.cboAlt.SelectedItem, AstoniaProcess).Name & """",
            .WindowStyle = ProcessWindowStyle.Normal,
            .Verb = "runas" 'add this to prompt for elevation
        }

        If Me.WindowState = FormWindowState.Normal Then
            My.Settings.location = Me.Location
        End If
        My.Settings.Save()
        Try
            Process.Start(procStartInfo).WaitForInputIdle()
        Catch e As System.ComponentModel.Win32Exception
            'operation cancelled
        Catch e As InvalidOperationException
            'wait for inputidle is needed
        Catch e As Exception
            Throw e
        End Try
        sysTrayIcon.Visible = False
        sysTrayIcon.Dispose()

    End Sub
    Public Sub UnelevateSelf()
        If Me.WindowState = FormWindowState.Normal Then
            My.Settings.location = Me.Location
        End If
        My.Settings.Save()

        tmrActive.Stop()
        tmrMove.Stop()
        tmrOverview.Stop()
        tmrTick.Stop()

        RestorePos(AltPP)

        ExecuteProcessUnElevated(Environment.GetCommandLineArgs()(0), "Someone", IO.Directory.GetCurrentDirectory())
        sysTrayIcon.Visible = False
        sysTrayIcon.Dispose()
        End 'program
    End Sub


#If DEBUG Then
    Private Sub ChkDebug_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkDebug.CheckedChanged
        Debug.Print(Screen.GetWorkingArea(sender).ToString)
        UpdateThumb(If(sender.Checked, 122, 255))
    End Sub
#End If


    Private Sub SysTrayIcon_MouseDoubleClick(sender As NotifyIcon, e As MouseEventArgs) Handles sysTrayIcon.MouseDoubleClick
        Debug.Print("sysTrayIcon_MouseDoubleClick")
        If e.Button = MouseButtons.Right Then Exit Sub
        If Me.WindowState = FormWindowState.Minimized Then
            Me.Location = restoreLoc
            SetWindowLong(Me.Handle, GWL_HWNDPARENT, AltPP.MainWindowHandle) 'hides scala from taskbar
            Me.WindowState = FormWindowState.Normal
            ReZoom(zooms(cmbResolution.SelectedIndex))
            btnMax.Text = "⧠"
        End If
        Me.Show()
        'Me.BringToFront() 'doesn't work
        If AltPP IsNot Nothing AndAlso AltPP.Id <> 0 Then
            SetWindowPos(AltPP.MainWindowHandle, -2, -1, -1, -1, -1, SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.IgnoreResize)
            AppActivate(AltPP.Id)
        Else
            Me.TopMost = True
            Me.TopMost = My.Settings.topmost
        End If
    End Sub

    Private Async Sub FrmMain_Click(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        Debug.Print($"me.mousedown {sender.name}")
        MyBase.WndProc(Message.Create(ScalaHandle, WM_CANCELMODE, 0, 0))
        cmsQuickLaunch.Close()
        Await Task.Delay(100)
        If cboAlt.SelectedIndex > 0 Then
            pbZoom.Show()
        Else
            AButton.ActiveOverview = My.Settings.gameOnOverview
        End If
    End Sub

    Private Sub cmsAlt_Closed(sender As Object, e As ToolStripDropDownClosedEventArgs) Handles cmsAlt.Closed
        AButton.ActiveOverview = My.Settings.gameOnOverview
    End Sub

    Private Sub cmsAlt_Opened(sender As Object, e As EventArgs) Handles cmsAlt.Opened
        AButton.ActiveOverview = False
    End Sub
End Class

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

        MessageBox.Show(exePath & vbCrLf & cmdLine)
    End Sub

    Friend Sub ResetHide(sender As Object, e As EventArgs)
        FrmMain.chkHideMessage.Checked = False
    End Sub

    Friend Sub Resumelayout(sender As Object, e As EventArgs)
        FrmMain.pnlOverview.ResumeLayout(True)
    End Sub

    Friend Sub buttonInfo(sender As Object, e As EventArgs)
        Dim i = 1
        For Each but As Button In FrmMain.pnlOverview.Controls.OfType(Of Button).Where(Function(b) b.Visible)
            Debug.Print($"Button {i} Size: {but.Size}")
            i += 1
        Next
    End Sub
End Module

#End If