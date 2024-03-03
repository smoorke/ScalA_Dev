Imports System.Net.Http
Imports System.Web

Partial Public NotInheritable Class FrmMain

    Private _altPP As AstoniaProcess

    Public Property AltPP As AstoniaProcess
        Get
            Return _altPP
        End Get
        Set(value As AstoniaProcess)
            If (_altPP Is Nothing AndAlso value IsNot Nothing) OrElse
               (_altPP IsNot Nothing AndAlso value Is Nothing) OrElse
               (_altPP IsNot Nothing AndAlso value IsNot Nothing AndAlso _altPP.Id <> value.Id) Then
                IPC.AddOrUpdateInstance(scalaPID, cboAlt.SelectedIndex = 0, value?.Id)
                'IPC.getInstances().FirstOrDefault(Function(si) si.pid = scalaPID)
                _altPP = value
            End If
        End Set
    End Property
    'Private WndClass() As String = {"MAINWNDMOAC", "䅍义乗䵄䅏C"}
#Region " Alt Dropdown "
    Friend Sub PopDropDown(sender As ComboBox)

        Dim current As AstoniaProcess = DirectCast(sender.SelectedItem, AstoniaProcess)
        sender.BeginUpdate()
        updatingCombobox = True

        sender.Items.Clear()
        sender.Items.Add(New AstoniaProcess) 'Someone

        sender.Items.AddRange(AstoniaProcess.Enumerate(blackList).OrderBy(Function(ap) ap.Name, apSorter).ToArray)

        If current IsNot Nothing AndAlso sender.Items.Contains(current) Then
            sender.SelectedItem = current
        Else
            sender.SelectedIndex = 0
        End If

        updatingCombobox = False
        sender.EndUpdate()
        sender.DropDownHeight = sender.ItemHeight * sender.Items.Count + 2
    End Sub

    Private Sub CboAlt_DropDown(sender As ComboBox, e As EventArgs) Handles cboAlt.DropDown
        pbZoom.Visible = False
        AButton.ActiveOverview = False
        PopDropDown(sender)
    End Sub
    Private Sub CmbResolution_DropDown(sender As ComboBox, e As EventArgs) Handles cmbResolution.DropDown
        moveBusy = False
        pbZoom.Visible = False
        AButton.ActiveOverview = False
    End Sub

    Private Sub ComboBoxes_DropDownClosed(sender As ComboBox, e As EventArgs) Handles cboAlt.DropDownClosed, cmbResolution.DropDownClosed
        moveBusy = False
        Dim unused = RestoreClicking()
    End Sub

#End Region
    Private Async Function RestoreClicking() As Task(Of Boolean)
        Await Task.Delay(150)
        If cboAlt.DroppedDown OrElse cmbResolution.DroppedDown OrElse cmsQuickLaunch.Visible OrElse cmsAlt.Visible OrElse cmsQuit.Visible OrElse SysMenu.Visible Then Return False
        If Not pnlOverview.Visible Then
            pbZoom.Visible = True
        Else
            AButton.ActiveOverview = My.Settings.gameOnOverview
        End If
        Return True
    End Function

    Private prevItem As New AstoniaProcess()
    Private updatingCombobox As Boolean = False
    Private Async Sub CboAlt_SelectedIndexChanged(sender As ComboBox, e As EventArgs) Handles cboAlt.SelectedIndexChanged

        If updatingCombobox Then Exit Sub

        'IPC.AddOrUpdateInstance(scalaPID, sender.SelectedIndex = 0)

        Debug.Print($"CboAlt_SelectedIndexChanged {sender.SelectedIndex}")

        'btnAlt1.Focus()

        'If AltPP IsNot Nothing AndAlso AltPP.Id <> 0 AndAlso AltPP.Equals(CType(that.SelectedItem, AstoniaProcess)) Then
        '    AltPP.Activate()
        '    Exit Sub
        'End If

        'If AltPP.Id = 0 AndAlso that.SelectedIndex = 0 Then
        '    Exit Sub
        'End If

        TickCounter = 0

        Detach(False)
        AstoniaProcess.RestorePos()
        AltPP = sender.SelectedItem
        'IPC.AddOrUpdateInstance(scalaPID, sender.SelectedIndex = 0, AltPP.Id)
        UpdateTitle()

        If sender.SelectedIndex = 0 Then
            If Not My.Settings.gameOnOverview Then
                Try
                    AppActivate(scalaPID)
                Catch
                End Try
            End If
            'pnlOverview.SuspendLayout()
            pnlOverview.Show()
            Dim visbut = UpdateButtonLayout(AstoniaProcess.Enumerate(blackList, True, True).Count)
            For Each but As AButton In visbut.Where(Function(b) b.Text <> "")
                but.Image = Nothing
                but.BackgroundImage = Nothing
                but.Text = String.Empty
            Next
            'pnlOverview.ResumeLayout()
            tmrOverview.Enabled = True
            tmrTick.Enabled = False
            If prevItem.Id <> 0 Then DwmUnregisterThumbnail(thumb)
            sysTrayIcon.Icon = My.Resources.moa3
            prevItem = DirectCast(sender.SelectedItem, AstoniaProcess)
            PnlEqLock.Visible = False
            AOshowEqLock = False
            Me.TopMost = My.Settings.topmost
            If Not startup AndAlso My.Settings.MaxNormOverview AndAlso Me.WindowState <> FormWindowState.Maximized Then
                btnMax.PerformClick()
            End If
            Exit Sub
        Else
            pnlOverview.Hide()
            tmrOverview.Enabled = False
            PnlEqLock.Visible = True
        End If

        If Not AltPP?.IsRunning Then
            Dim idx As Integer = sender.SelectedIndex
            sender.Items.RemoveAt(idx)
            sender.SelectedIndex = Math.Min(idx, sender.Items.Count - 1)
            Exit Sub
        End If


        If Not AltPP?.Id = 0 Then
            If AltPP.IsMinimized Then AltPP.Restore()

            'AltPP.ResetCache()

            Dim rcW As Rectangle = AltPP.WindowRect
            rcC = AltPP.ClientRect
            'GetWindowRect(AltPP.MainWindowHandle, rcW)
            'GetClientRect(AltPP.MainWindowHandle, rcC)

            Dim ptC As Point
            ClientToScreen(AltPP.MainWindowHandle, ptC)

            Debug.Print($"rcW:{rcW}")
            Debug.Print($"rcC:{rcC}")
            Debug.Print($"ptC:{ptC}")

            'check if target is running as windowed. if not ask to run it with -w
            If rcC.Width = 0 AndAlso rcC.Height = 0 OrElse
               rcC.X = ptC.X AndAlso rcC.Y = ptC.Y Then
                'MessageBox.Show("Client is not running in windowed mode", "Error")
                Debug.Print("Astonia Not Windowed")
                Await AltPP.ReOpenAsWindowed()
                'cboAlt.SelectedIndex = 0
                Exit Sub
            End If
            AltPP.SavePos(rcW.Location)

            'AltPP.CenterBehind(pbZoom, SetWindowPosFlags.ASyncWindowPosition)
            AltPP.CenterBehind(pbZoom, SetWindowPosFlags.ASyncWindowPosition, False, True)

            Debug.Print("tmrTick.Enabled")
            tmrTick.Enabled = True

            Debug.Print("AltPPTopMost " & AltPP.TopMost.ToString)
            Debug.Print("SelfTopMost " & Process.GetCurrentProcess.IsTopMost.ToString)

            Dim item As AstoniaProcess = DirectCast(sender.SelectedItem, AstoniaProcess)
            If startThumbsDict.ContainsKey(item.Id) Then
                Debug.Print($"reassignThumb {item.Id} {startThumbsDict(item.Id)} {item.Name}")
                thumb = startThumbsDict(item.Id)
            End If

            For Each thumbid As IntPtr In startThumbsDict.Values
                If thumbid = thumb Then Continue For
                DwmUnregisterThumbnail(thumbid)
            Next
            startThumbsDict.Clear()
            If My.Settings.MaxNormOverview AndAlso WindowState = FormWindowState.Maximized Then
                btnMax.PerformClick()
            End If
            Debug.Print($"updateThumb pbzoom {pbZoom.Size}")
            AltPP.CenterBehind(pbZoom, SetWindowPosFlags.DoNotActivate, True, True)
            If Not My.Settings.MaxNormOverview AndAlso AnimsEnabled AndAlso rectDic.ContainsKey(item.Id) Then
                AnimateThumb(rectDic(item.Id), New Rectangle(pbZoom.Left, pbZoom.Top, pbZoom.Right, pbZoom.Bottom))
            Else
                prevMode = 0
                UpdateThumb(If(chkDebug.Checked, 128, 255))
            End If
            rectDic.Clear()
            sysTrayIcon.Icon = AltPP?.GetIcon

            Dim ScalAWinScaling = Me.WindowsScaling()

            Debug.Print($"{ScalAWinScaling}% ScalA windows scaling")
            Debug.Print($"{AltPP.WindowsScaling}% altPP windows scaling")

            Dim failcounter = 0
            If AltPP.WindowsScaling <> ScalAWinScaling Then 'scala is scaled diffrent than Alt
                Const timeout As Integer = 500
                Dim sw As Stopwatch = Stopwatch.StartNew()
                Do 'looped delay until alt is scaled same
                    AltPP.CenterBehind(pbZoom, Nothing, True)
                    Dim rc As RECT
                    GetWindowRect(AltPP.MainWindowHandle, rc)
                    SetWindowPos(AltPP.MainWindowHandle, ScalaHandle, rc.left + 1, rc.top + 1, -1, -1, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.FrameChanged)
                    Debug.Print($"Scaling Delay {sw.ElapsedMilliseconds}ms {ScalAWinScaling}% vs {AltPP.WindowsScaling}")
                    Await Task.Delay(16)
                    If sw.ElapsedMilliseconds > timeout Then
                        sw.Stop()
                        Debug.Print($"Scaling Delay Timeout! {failcounter}")
                        AstoniaProcess.RestorePos(True)
                        Await Task.Delay(16)
                        sw = Stopwatch.StartNew()
                        failcounter += 1
                    End If
                Loop Until ScalAWinScaling = AltPP.WindowsScaling OrElse failcounter >= 3
                AltPP.ResetCache()
                UpdateThumb(If(chkDebug.Checked, 128, 255))
            End If

            Attach(AltPP)

            If My.Settings.topmost Then
                AltPP.TopMost = True
            End If

            AltPP?.Activate()

            moveBusy = False
        Else 'AltPP.Id = 0


        End If

        DoEqLock(pbZoom.Size)
        prevItem = sender.SelectedItem

        If sender.SelectedIndex > 0 Then
            pbZoom.Visible = True
        Else
            AButton.ActiveOverview = My.Settings.gameOnOverview
        End If

    End Sub
    Private Const DWMWA_EXTENDED_FRAME_BOUNDS As Integer = 9
    Public Function WindowsScaling() As Integer
        Dim rcFrame As RECT
        DwmGetWindowAttribute(ScalaHandle, DWMWA_EXTENDED_FRAME_BOUNDS, rcFrame, System.Runtime.InteropServices.Marshal.SizeOf(rcFrame))
        Dim rcWind As RECT
        GetWindowRect(ScalaHandle, rcWind)
        Return Int((rcFrame.right - rcFrame.left) / (rcWind.right - rcWind.left) * 100 / 25) * 25
    End Function

    Public Function GetScaling(hWnd As IntPtr) As Integer
        Dim rcFrame As RECT
        DwmGetWindowAttribute(hWnd, DWMWA_EXTENDED_FRAME_BOUNDS, rcFrame, System.Runtime.InteropServices.Marshal.SizeOf(rcFrame))
        Dim rcWind As RECT
        GetWindowRect(hWnd, rcWind)
        Dim rcClient As RECT
        GetClientRect(hWnd, rcClient)
        Dim hasBorder As Boolean = rcWind.top <> rcClient.top
        Return Int((rcFrame.right - rcFrame.left) / (rcWind.right - rcWind.left) * 100 / 25) * 25 + If(hasBorder, 0, 25)
    End Function

    Public Resizing As Boolean

    Private Function UpdateTitle() As Boolean
        Dim titleSuff As String = String.Empty
        Dim traytooltip As String = "ScalA"
        If AltPP?.IsRunning Then
            Try
                Dim title As String = AltPP.MainWindowTitle
                If String.IsNullOrEmpty(title) Then Return False
                titleSuff = " - " & title
                traytooltip = title
            Catch e As Exception
                Return False
            End Try
        End If
        Me.Text = "ScalA" & titleSuff
        sysTrayIcon.Text = traytooltip.Cap(63)
        With My.Application.Info.Version
            Dim build = .Build + If(.Revision, 1, 0)
            Dim rev As String = If(.Revision, $"b{ .Revision}", "")
            lblTitle.Text = $"- ScalA v{ .Major}.{ .Minor}.{build}{rev}{titleSuff}"
        End With
        Return True
    End Function

    Public Shared zooms() As Size = GetResolutions()

    Private Sub FrmMain_Load(sender As Form, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = True

        If Environment.OSVersion.Version.Major < 6 Then
            MessageBox.Show("ScalA requires Windows Vista or later", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End
        End If

        If My.Settings.SingleInstance AndAlso IPC.AlreadyOpen Then
            IPC.RequestActivation = True
            End
        End If
        IPC.AlreadyOpen = True

        Try
            Dim vers As String = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription
            If vers.ToLower.StartsWith(".net framework ") AndAlso New Version(vers.Split(" "c)(2)) >= New Version("4.5.1") Then
                StructureToPtrSupported = True
            End If
        Catch ex As Exception

        End Try

        sysTrayIcon.Visible = True

        If New Version(My.Settings.SettingsVersion) < My.Application.Info.Version Then
            My.Settings.Upgrade()
            My.Settings.SettingsVersion = My.Application.Info.Version.ToString
            If Not My.Settings.className.Contains("SDL_app") Then
                My.Settings.className &= " | SDL_app"
            End If
            My.Settings.Save()
            zooms = GetResolutions()
            topSortList = My.Settings.topSort.Split(vbCrLf.ToCharArray, StringSplitOptions.RemoveEmptyEntries).ToList
            botSortList = My.Settings.botSort.Split(vbCrLf.ToCharArray, StringSplitOptions.RemoveEmptyEntries).ToList
            blackList = topSortList.Intersect(botSortList).ToList
        End If
        topSortList = topSortList.Except(blackList).ToList
        botSortList = botSortList.Except(blackList).ToList

        apSorter = New AstoniaProcessSorter(topSortList, botSortList)

#If DEBUG Then
        Debug.Print("Top:")
        topSortList.ForEach(Sub(el) Debug.Print(el))
        Debug.Print("Bot:")
        botSortList.ForEach(Sub(el) Debug.Print(el))
        Debug.Print("blacklist:")
        blackList.ForEach(Sub(el) Debug.Print(el))
#End If



        Debug.Print("mangleSysMenu")
        InitSysMenu()
        ScalaHandle = Me.Handle

        Debug.Print("topmost " & My.Settings.topmost)
        Me.TopMost = My.Settings.topmost
        Me.chkHideMessage.Checked = My.Settings.hideMessage


        'left big in designer to facilitate editing
        cornerNW.Size = New Size(2, 2)
        cornerNE.Size = New Size(2, 2)
        cornerSW.Size = New Size(2, 2)
        cornerSE.Size = New Size(2, 2)

        cmbResolution.Items.Add($"{My.Settings.resol.Width}x{My.Settings.resol.Height}")
        cmbResolution.Items.AddRange(zooms.Select(Function(ss) ss.Width & "x" & ss.Height).ToArray)
        cmbResolution.SelectedIndex = My.Settings.zoom


        Debug.Print("location " & My.Settings.location.ToString)
        suppressWM_MOVEcwp = True
        Me.Location = My.Settings.location
        suppressWM_MOVEcwp = False

        Dim args() As String = Environment.GetCommandLineArgs()

        cboAlt.BeginUpdate()
        cboAlt.Items.Add(New AstoniaProcess()) 'someone
        cboAlt.SelectedIndex = 0
        Dim APlist As List(Of AstoniaProcess) = AstoniaProcess.Enumerate(blackList).ToList
        For Each ap As AstoniaProcess In APlist
            cboAlt.Items.Add(ap)
            If args.Count > 1 AndAlso ap.Name = args(1) Then
                Debug.Print($"Selecting '{ap.Name}'")
                cboAlt.SelectedItem = ap
            End If
        Next
        cboAlt.EndUpdate()

        If cmbResolution.SelectedIndex = 0 Then
            ReZoom(My.Settings.resol)
        End If

        AddAButtons()
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

#If DEBUG Then
        chkDebug.Visible = True
        Dim test As New ContextMenuStrip
        test.Items.Add(New ToolStripMenuItem("Parse Info", Nothing, AddressOf dBug.ParseInfo))
        test.Items.Add(New ToolStripMenuItem("Reset Hide", Nothing, AddressOf dBug.ResetHide))
        test.Items.Add(New ToolStripMenuItem("ResumeLayout", Nothing, AddressOf dBug.Resumelayout))
        test.Items.Add(New ToolStripMenuItem("Button Info", Nothing, AddressOf dBug.ButtonInfo))
        test.Items.Add(New ToolStripMenuItem("isBelow", Nothing, AddressOf dBug.IsBelow))
        Static dynamicitem1 As New ToolStripMenuItem($"movebusy {moveBusy}")
        test.Items.Add(dynamicitem1)
        test.Items.Add(New ToolStripMenuItem("Update", Nothing, AddressOf dBug.ToggleUpdate))
        test.Items.Add(New ToolStripMenuItem("Scaling", Nothing, AddressOf dBug.ScreenScaling))
        test.Items.Add(New ToolStripMenuItem("Shared Mem", Nothing, AddressOf dBug.SharedMem))
        Static dynamicitem2 As New ToolStripMenuItem($"Aborder", Nothing, AddressOf dBug.toggeleborder)
        test.Items.Add(dynamicitem2)
        test.Items.Add(New ToolStripMenuItem("ThumbSize", Nothing, AddressOf dBug.querySize))
        test.Items.Add(New ToolStripMenuItem("FudgeThumb", Nothing, AddressOf dBug.fudgeThumb))
        test.Items.Add(New ToolStripMenuItem("NudgeTaskbar", Nothing, AddressOf dBug.NudgeTaskbar))
        test.Items.Add(New ToolStripMenuItem("thumbStuff", Nothing, AddressOf dBug.thumbStuff))
        test.Items.Add(New ToolStripMenuItem("list others", Nothing, AddressOf dBug.listothers))


        chkDebug.ContextMenuStrip = test
        AddHandler test.Opening, Sub()
                                     Debug.Print("test Opening")
                                     dynamicitem1.Text = $"movebusy {moveBusy}"
                                     dynamicitem2.Text = $"Aborder {AltPP?.hasBorder}"
                                     UntrapMouse(MouseButtons.Right)
                                     AppActivate(scalaPID)
                                 End Sub
        AddHandler chkDebug.MouseUp, Sub(sen, ev) UntrapMouse(ev.Button)
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
                'btnStart.Text = ""
            End If
        End If

        If System.IO.File.Exists(FileIO.SpecialDirectories.Temp & "\ScalA\tmp.lnk") Then
            Debug.Print("Deleting shortcut")
            System.IO.File.Delete(FileIO.SpecialDirectories.Temp & "\ScalA\tmp.lnk")
        End If

        FrmBehind.Show()
        FrmSizeBorder.Show(Me)
        ScalaHandle = Me.Handle
        suppressWM_MOVEcwp = True

        If cmbResolution.SelectedIndex = 0 Then DoEqLock(My.Settings.resol)

        cmsAlt.Renderer = New ToolStripProfessionalRenderer(New CustomColorTable)
        cmsQuickLaunch.Renderer = cmsAlt.Renderer
        cmsQuit.Renderer = cmsAlt.Renderer

        If My.Settings.Theme = 0 Then 'undefined, system, light, dark
            If My.Settings.DarkMode Then
                My.Settings.Theme = 3
            Else
                My.Settings.Theme = 1
            End If
        End If

        Dim darkmode As Boolean = WinUsingDarkTheme()

        If My.Settings.Theme = 3 Then
            darkmode = True
        End If
        If My.Settings.Theme = 2 Then
            darkmode = False
        End If
        ApplyTheme(darkmode)

        Debug.Print($"Anims {AnimsEnabled}")

        tmrOverview.Interval = If(My.Settings.gameOnOverview, 33, 66)

        AddHandler Application.Idle, AddressOf Application_Idle

    End Sub

    Dim AnimsEnabled As Boolean = getAnimationsEnabled()
    Dim startup As Boolean = True

    Private Sub FrmMain_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Debug.Print("FrmMain_Shown")
        suppressWM_MOVEcwp = False
        If Not Screen.AllScreens.Any(Function(s) s.WorkingArea.Contains(Me.Location)) Then
            Debug.Print("location out of bounds")
            Dim msWA As Rectangle = Screen.PrimaryScreen.WorkingArea
            Me.Location = New Point(Math.Max(msWA.Left, (msWA.Width - Me.Width) / 2), Math.Max(msWA.Top, (msWA.Height - Me.Height) / 2))
        End If
        startup = False
        If My.Settings.StartMaximized Then
            btnMax.PerformClick()
        End If
        If cboAlt.SelectedIndex > 0 Then
            tmrTick.Start()
            moveBusy = False
        End If
        FrmBehind.Bounds = Me.Bounds
        If My.Settings.CheckForUpdate Then
            UpdateCheck()
        End If
        If cboAlt.SelectedIndex = 0 AndAlso My.Settings.MaxNormOverview AndAlso Me.WindowState = FormWindowState.Normal Then
            btnMax.PerformClick()
        End If
        FrmSizeBorder.Opacity = If(My.Settings.SizingBorder AndAlso Me.WindowState = FormWindowState.Normal, 0.01, 0)

    End Sub
    Friend Shared updateToVersion As String = "Error"
    Friend Shared ReadOnly client As HttpClient = New HttpClient() With {.Timeout = TimeSpan.FromMilliseconds(5000)}
    Friend Shared Async Sub UpdateCheck()
        Try
            Using clnt As HttpClient = New HttpClient()
                Using response As HttpResponseMessage = Await clnt.GetAsync("https://github.com/smoorke/ScalA/releases/download/ScalA/version")
                    response.EnsureSuccessStatusCode()
                    Dim responseBody As String = Await response.Content.ReadAsStringAsync()

                    If New Version(responseBody) > My.Application.Info.Version Then
                        FrmMain.pnlUpdate.Visible = True
                    Else
                        FrmMain.pnlUpdate.Visible = False
                    End If
                    updateToVersion = responseBody
                End Using
            End Using
        Catch ex As Exception
            FrmMain.pnlUpdate.Visible = False
            updateToVersion = "Error"
        End Try
    End Sub
    Friend Shared Async Function UpdateDownload() As Task
        Try
            Using response As HttpResponseMessage = Await client.GetAsync("https://github.com/smoorke/ScalA/releases/download/ScalA/ScalA.exe")
                response.EnsureSuccessStatusCode()
                Dim responseBody As Byte() = Await response.Content.ReadAsByteArrayAsync()

                If Not FileIO.FileSystem.DirectoryExists(FileIO.SpecialDirectories.Temp & "\ScalA\") Then
                    FileIO.FileSystem.CreateDirectory(FileIO.SpecialDirectories.Temp & "\ScalA\")
                End If

                FileIO.FileSystem.WriteAllBytes(FileIO.SpecialDirectories.Temp & "\ScalA\ScalA.exe", responseBody, False)

            End Using
        Catch e As Exception
            CustomMessageBox.Show("Error" & vbCrLf & e.Message)
        End Try
    End Function
    Friend Shared Async Function LogDownload() As Task
        Try
            Using response As HttpResponseMessage = Await client.GetAsync("https://github.com/smoorke/ScalA/releases/download/ScalA/ChangeLog.txt")
                response.EnsureSuccessStatusCode()
                Dim responseBody As Byte() = Await response.Content.ReadAsByteArrayAsync()

                If Not FileIO.FileSystem.DirectoryExists(FileIO.SpecialDirectories.Temp & "\ScalA\") Then
                    FileIO.FileSystem.CreateDirectory(FileIO.SpecialDirectories.Temp & "\ScalA\")
                End If

                FileIO.FileSystem.WriteAllBytes(FileIO.SpecialDirectories.Temp & "\ScalA\ChangeLog.txt", responseBody, False)

            End Using
        Catch e As Exception
            CustomMessageBox.Show("Error" & vbCrLf & e.Message)
        End Try
    End Function
    Friend Async Sub pbUpdateAvailable_Click(sender As PictureBox, e As MouseEventArgs) Handles pbUpdateAvailable.MouseDown

        If e.Button <> MouseButtons.Left Then Exit Sub

        Await LogDownload()

        If UpdateDialog.ShowDialog(Me) <> DialogResult.OK Then
            Exit Sub
        End If

        Dim MePath As String = Environment.GetCommandLineArgs(0)
        Dim sb As New Text.StringBuilder(260)
        Dim len = sb.Capacity
        Try
            Dim drive = Strings.Left(MePath, 2)
            If WNetGetConnection(drive, sb, len) = 0 Then MePath = MePath.Replace(drive, sb.ToString)
        Catch
            Debug.Print($"WNetGetConnection Exception")
        End Try
        SaveLocation()
        My.Settings.Save()
        tmrOverview.Stop()
        tmrTick.Stop()
        Await UpdateDownload()
        AstoniaProcess.RestorePos()
        Try
            If Not FileIO.FileSystem.DirectoryExists(FileIO.SpecialDirectories.Temp & "\ScalA\") Then
                FileIO.FileSystem.CreateDirectory(FileIO.SpecialDirectories.Temp & "\ScalA\")
            End If
            FileIO.FileSystem.WriteAllBytes(FileIO.SpecialDirectories.Temp & "\ScalA\ScalA_Updater.exe", My.Resources.ScalA_Updater, False)
            Dim si As New ProcessStartInfo With {
                                       .FileName = FileIO.SpecialDirectories.Temp & "\ScalA\ScalA_Updater.exe",
                                       .Arguments = $"""{MePath}"""
                          }
            If Not IsDirectoryWritable(IO.Path.GetDirectoryName(MePath)) Then si.Verb = "runas"
            If MePath <> Environment.GetCommandLineArgs(0) Then si.Arguments &= $" ""{IO.Directory.GetCurrentDirectory().TrimEnd("\")}"" ""{Environment.GetCommandLineArgs(0)}"""

            Process.Start(si)
            sysTrayIcon.Visible = False
            End
        Catch
        End Try
        If cboAlt.SelectedIndex = 0 Then
            tmrOverview.Start()
        Else
            tmrTick.Start()
        End If
    End Sub

    Public Sub ApplyTheme(darkmode As Boolean)
        My.Settings.DarkMode = darkmode
        If darkmode Then
            pnlOverview.BackColor = Color.Gray
            pnlButtons.BackColor = Color.FromArgb(60, 63, 65)
            pnlSys.BackColor = Color.FromArgb(60, 63, 65)
            Me.BackColor = Color.FromArgb(60, 63, 65)
            btnStart.BackColor = Color.FromArgb(60, 63, 65)
            pnlTitleBar.BackColor = Color.FromArgb(60, 63, 65)
            lblTitle.ForeColor = Colors.LightText
            ChkEqLock.ForeColor = Color.Gray
#If DEBUG Then
            chkDebug.ForeColor = Colors.LightText
#End If
        Else
            pnlOverview.BackColor = Color.FromKnownColor(KnownColor.Control)
            pnlButtons.BackColor = Color.FromKnownColor(KnownColor.Control)
            pnlSys.BackColor = Color.FromKnownColor(KnownColor.Control)
            Me.BackColor = Color.FromKnownColor(KnownColor.Control)
            btnStart.BackColor = Color.FromKnownColor(KnownColor.Control)
            pnlTitleBar.BackColor = Color.FromKnownColor(KnownColor.Control)
            lblTitle.ForeColor = Color.Black
            ChkEqLock.ForeColor = Color.Black
#If DEBUG Then
            chkDebug.ForeColor = Color.Black
#End If
        End If

        Dim bBc As Color = If(darkmode, Color.DarkGray, Color.FromArgb(&HFFE1E1E1))
        Task.Run(Sub() Parallel.ForEach(pnlOverview.Controls.OfType(Of AButton), Sub(but) but.BackColor = bBc))
        PnlEqLock.BackColor = bBc

        Dim bFaBc As Color = If(darkmode, Color.FromArgb(60, 63, 65), Color.FromKnownColor(KnownColor.Control))
        For Each but As Button In pnlButtons.Controls.OfType(Of Button)
            but.FlatAppearance.BorderColor = bFaBc
        Next

        cmbResolution.DarkTheme = darkmode
        cboAlt.DarkTheme = darkmode
        btnStart.DarkTheme = darkmode
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

    Private MovingForm As Boolean
    Private MoveForm_MousePosition As Point
    Private caption_Mousedown As Boolean = False
    Private captionMoveTrigger As Boolean = False

    Public Sub MoveForm_MouseDown(sender As Control, e As MouseEventArgs) Handles pnlTitleBar.MouseDown, lblTitle.MouseDown
        'Me.TopMost = True
        'setActive(True)
        'Me.Invalidate(True)

        If FrmSettings.chkDoAlign.Checked Then
            If Me.WindowState <> FormWindowState.Maximized AndAlso e.Button = MouseButtons.Left Then
                MovingForm = True
                MoveForm_MousePosition = e.Location
            End If
        Else
            If e.Button = MouseButtons.Left AndAlso e.Clicks = 1 Then
                sender.Capture = False
                tmrTick.Stop()
                caption_Mousedown = True
                If Me.WindowState = FormWindowState.Maximized Then
                    captionMoveTrigger = True
                    wasMaximized = True
                End If
                Dim msg As Message = Message.Create(ScalaHandle, WM_NCLBUTTONDOWN, New IntPtr(HTCAPTION), IntPtr.Zero)
                Debug.Print("WM_NCLBUTTONDOWN")
                Me.WndProc(msg)
                caption_Mousedown = False
                captionMoveTrigger = False
                If Not pnlOverview.Visible Then
                    AltPP.Activate()
                    tmrTick.Start()
                End If
                Debug.Print("movetimer stopped")
                'FrmSizeBorder.Bounds = Me.Bounds
            End If
        End If
    End Sub

    Public Sub MoveForm_MouseMove(sender As Control, e As MouseEventArgs) Handles _
    pnlTitleBar.MouseMove, lblTitle.MouseMove ' Add more handles here (Example: PictureBox1.MouseMove)
        If MovingForm Then
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
                AltPP?.CenterBehind(pbZoom, SetWindowPosFlags.DoNotActivate)
            End If
        End If
    End Sub

#End Region
    Public Sub SaveLocation()
        If Me.WindowState = FormWindowState.Normal Then
            My.Settings.location = Me.Location
        Else
            My.Settings.location = Me.RestoreBounds.Location
        End If
    End Sub

    Private Sub FrmMain_Closing(sender As Form, e As EventArgs) Handles Me.Closing
        AstoniaProcess.RestorePos()
        SaveLocation()
        tmrActive.Stop()
        Hotkey.UnregHotkey(Me)
    End Sub


    Public Function GetActiveProcessID() As UInteger
        Dim hWnd As IntPtr = GetForegroundWindow()
        Dim ProcessID As UInteger = 0

        GetWindowThreadProcessId(hWnd, ProcessID)

        Return ProcessID
    End Function


    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.Style = cp.Style Or WindowStyles.WS_SYSMENU Or WindowStyles.WS_MINIMIZEBOX
            'cp.ExStyle = cp.ExStyle Or WindowStylesEx.WS_EX_COMPOSITED
            'cp.ClassStyle = cp.ClassStyle Or CS_DROPSHADOW
            Return cp
        End Get
    End Property

    Private Sub CmbResolution_MouseDown(sender As ComboBox, e As MouseEventArgs) Handles cmbResolution.MouseDown
        If e.Button = MouseButtons.Right Then
            FrmSettings.Tag = FrmSettings.tabResolutions
            FrmSettings.Show()
        End If
    End Sub

    Public suppressResChange As Boolean = False
    Public Sub CmbResolution_SelectedIndexChanged(sender As ComboBox, e As EventArgs) Handles cmbResolution.SelectedIndexChanged
        moveBusy = False
        If sender.SelectedIndex = 0 Then Exit Sub
        Debug.Print($"cboResolution_SelectedIndexChanged {sender.SelectedIndex}")

        My.Settings.zoom = sender.SelectedIndex
        My.Settings.resol = zooms(sender.SelectedIndex - 1)

        If WindowState = FormWindowState.Maximized Then
            btnMax.PerformClick()
            wasMaximized = False
            Exit Sub
        End If

        sender.Items(0) = $"{My.Settings.resol.Width}x{My.Settings.resol.Height}"
        DoEqLock(My.Settings.resol)

        If suppressResChange Then Exit Sub
        ReZoom(My.Settings.resol)
        AltPP?.CenterBehind(pbZoom)

    End Sub

    Public Sub ReZoom(newSize As Size)
        Debug.Print($"reZoom {newSize}")
        'Me.SuspendLayout()
        suppressResChange = True
        If Me.WindowState <> FormWindowState.Maximized Then
            Me.Size = New Size(newSize.Width + 2, newSize.Height + pnlTitleBar.Height + 1)
            pbZoom.Left = 1
            pnlOverview.Left = 1
            pbZoom.Size = newSize
            pnlOverview.Size = newSize
            cmbResolution.Enabled = True

            'suppressResChange = True
            cmbResolution.SelectedIndex = My.Settings.zoom

        Else 'FormWindowState.Maximized
            pbZoom.Left = 0
            pnlOverview.Left = 0
            pbZoom.Width = newSize.Width
            pbZoom.Height = newSize.Height - pnlTitleBar.Height
            pnlOverview.Size = pbZoom.Size

            'suppressResChange = True
            cmbResolution.SelectedIndex = 0

        End If

        'If Me.WindowsScaling = 175 Then
        '    pbZoom.Top = pnlTitleBar.Height - 1
        '    pbZoom.Height = newSize.Height + 1
        '    If Me.WindowState = FormWindowState.Maximized Then
        '        pbZoom.Height -= pnlTitleBar.Height
        '    End If
        'End If
        cmbResolution.Items(0) = $"{pbZoom.Size.Width}x{pbZoom.Size.Height}"
        suppressResChange = False
        'Me.ResumeLayout(True)

        If cboAlt.SelectedIndex <> 0 Then
            Debug.Print("updateThumb")
            UpdateThumb(If(chkDebug.Checked, 128, 255))
        End If
        pnlTitleBar.Width = newSize.Width - pnlButtons.Width - pnlSys.Width
        Debug.Print($"rezoom pnlTitleBar.Width {pnlTitleBar.Width}")

        cornerNW.Location = New Point(0, 0)
        cornerNE.Location = New Point(Me.Width - 2, 0)
        cornerSW.Location = New Point(0, Me.Height - 2)
        cornerSE.Location = New Point(Me.Width - 2, Me.Height - 2)

        DoEqLock(newSize)

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

    Private Sub DoEqLock(newSize As Size)
        If rcC.Width = 0 Then
            PnlEqLock.Location = New Point(138.Map(800, 0, newSize.Width, 0), 25)
            PnlEqLock.Size = New Size(524.Map(0, 800, 0, newSize.Width),
                                       45.Map(0, 600, 0, newSize.Height))
        Else
#If True Then
            'Dim zoom As Size = My.Settings.resol
            Dim zoom As Size = newSize
            If cmbResolution.SelectedIndex > 0 Then
                zoom = zooms(cmbResolution.SelectedIndex - 1)
            End If
            Debug.Print($"DoEqLock zoom {zoom}")
            PnlEqLock.Location = New Point(CType(rcC.Width / 2 - 262.Map(0, 800, 0, rcC.Width), Integer).Map(rcC.Width, 0, zoom.Width, 0), 25)
            Dim excludGearLock As Integer = If(AltPP?.isSDL, 18, 0)
            Dim lockHeight = 45
            If rcC.Height >= 2000 Then
                lockHeight += 120
            ElseIf rcC.Height >= 1500 Then
                lockHeight += 80
            ElseIf rcC.Height >= 1000 Then
                lockHeight += 40
            End If
            PnlEqLock.Size = New Size((524 - excludGearLock).Map(0, 800, 0, rcC.Width).Map(0, rcC.Width, 0, zoom.Width),
                                       lockHeight.Map(0, rcC.Height, 0, zoom.Height))
#Else
            PnlEqLock.Location = New Point(CType(rcC.Width / 2 - 262, Integer).Map(rcC.Width, 0, zooms(cmbResolution.SelectedIndex).Width, 0), 25)
            PnlEqLock.Size = New Size(524.Map(rcC.Width, 0, zooms(cmbResolution.SelectedIndex).Width, 0),
                                       45.Map(0, rcC.Height, 0, zooms(cmbResolution.SelectedIndex).Height))
#End If
        End If
    End Sub

    ''' <summary>
    ''' Fix mousebutton stuck after drag bug
    ''' Note: needs to be run before acivating self
    ''' </summary>
    Private Sub UntrapMouse(button As MouseButtons)
        Dim activePID = GetActiveProcessID()
        'Debug.Print($"active {activePID} is AltPP.id {activePID = AltPP?.Id}")
        If activePID <> AltPP?.Id Then Exit Sub 'only when dragged from client
        Try
            'If My.Settings.gameOnOverview OrElse (Not pnlOverview.Visible AndAlso Not pbZoom.Contains(MousePosition)) Then
            Debug.Print($"untrap mouse {button}")
            If button = MouseButtons.Right Then PostMessage(AltPP?.MainWindowHandle, WM_RBUTTONUP, 0, 0)
            If button = MouseButtons.Middle Then PostMessage(AltPP?.MainWindowHandle, WM_MBUTTONUP, 0, 0)
            'End If
        Catch
        End Try
    End Sub
    ''' <summary>
    ''' wasMaximized is used to determine what state to restore to
    ''' </summary>
    Dim wasMaximized As Boolean = False

    Public moveBusy As Boolean = False
    Dim suppressWM_MOVEcwp As Boolean = False

    Private Sub Cycle(Optional up As Boolean = False)
        cboAlt.DroppedDown = False
        tmrTick.Enabled = False
        'PostMessage(AltPP.MainWindowHandle, WM_RBUTTONUP, 0, 0)'couses look to be sent when cycle hotkey contains ctrl
        'PostMessage(AltPP.MainWindowHandle, WM_MBUTTONUP, 0, 0)'causes alt to attack when hotkey contains ctrl
        PopDropDown(cboAlt)
        AstoniaProcess.RestorePos(True)
        If Me.WindowState = FormWindowState.Minimized Then
            SendMessage(ScalaHandle, WM_SYSCOMMAND, SC_RESTORE, IntPtr.Zero)
        End If
        Dim requestedindex = cboAlt.SelectedIndex + If(up, -1, 1)
        If requestedindex < 1 Then
            requestedindex = cboAlt.Items.Count - 1
        End If
        If requestedindex >= cboAlt.Items.Count Then
            requestedindex = 1
        End If
        If requestedindex >= cboAlt.Items.Count Then
            cboAlt.SelectedIndex = 0
            tmrOverview.Enabled = True
            tmrTick.Enabled = False
            pbZoom.Hide()
            pnlOverview.Show()
            sysTrayIcon.Icon = My.Resources.moa3
            Detach(True)
            Exit Sub
        End If
        Me.Activate()
        Me.BringToFront()
        cboAlt.SelectedIndex = requestedindex
        If requestedindex > 0 Then tmrTick.Start()
        Try
            If AltPP IsNot Nothing Then
                AppActivate(AltPP.Id)
            Else
                AppActivate(scalaPID)
            End If
        Catch ex As Exception

        End Try
    End Sub


    'Delegate Sub updateButtonImageDelegate(but As AButton, bm As Bitmap)
    'Private Shared ReadOnly updateButtonImage As New updateButtonImageDelegate(AddressOf UpdateButtonImageMethod)
    'Private Shared Sub UpdateButtonImageMethod(but As AButton, bm As Bitmap)
    '    If but Is Nothing Then Exit Sub
    '    but.Image = bm
    'End Sub
    'Delegate Sub updateButtonBackgroundImageDelegate(but As AButton, bm As Bitmap)
    'Private Shared ReadOnly updateButtonBackgroundImage As New updateButtonImageDelegate(AddressOf UpdateButtonBackgroundImageMethod)
    'Private Shared Sub UpdateButtonBackgroundImageMethod(but As AButton, bm As Bitmap)
    '    If but Is Nothing Then Exit Sub
    '    but.BackgroundImage = bm
    'End Sub
    Private Function GetNextPerfectSquare(num As Integer) As Integer
        Dim nextN As Integer = Math.Floor(Math.Sqrt(num)) + 1
        If nextN > 6 Then nextN = 6
        Return nextN * nextN
    End Function
    Private Sub AddAButtons()
        pnlOverview.SuspendLayout()
        For i As Integer = 1 To 42
            Dim but As New AButton(i, 0, 0, 200, 150)

            AddHandler but.Click, AddressOf BtnAlt_Click
            AddHandler but.MouseDown, AddressOf BtnAlt_MouseDown
            AddHandler but.MouseEnter, AddressOf BtnAlt_MouseEnter
            AddHandler but.MouseLeave, AddressOf BtnAlt_MouseLeave
            AddHandler but.MouseUp, AddressOf Various_MouseUp
            pnlOverview.Controls.Add(but)
        Next i
        pnlOverview.ResumeLayout()
    End Sub

    Private Function UpdateButtonLayout2(count As Integer) As List(Of AButton)
        count += If(My.Settings.hideMessage, 0, 1)

        Dim targetAspectRatio As Double = 800 / 600

        ' Calculate the number of columns and rows based on the target aspect ratio and available space
        Dim cols As Integer = Math.Max(2, Math.Ceiling(Math.Sqrt(count * pnlOverview.Width / pnlOverview.Height / targetAspectRatio)))
        Dim rows As Integer = Math.Ceiling(count / cols)

        ' Adjust the number of columns or rows to ensure at least 'count' total items
        While cols * rows < count
            If pnlOverview.Width / (cols + 1) >= pnlOverview.Height / (rows + 1) Then
                cols += 1
            Else
                rows += 1
            End If
        End While

        ' Calculate the size of each button based on the adjusted layout
        Dim buttonWidth As Integer = pnlOverview.Width \ cols
        Dim buttonHeight As Integer = pnlOverview.Height \ rows

        ' Ensure that the calculated button size is not too small
        buttonWidth = Math.Max(1, buttonWidth)
        buttonHeight = Math.Max(1, buttonHeight)

        Dim totalButtons = cols * rows
        Dim i = If(My.Settings.hideMessage, 1, 2)

        Dim visButtons As New List(Of AButton)

        For Each but As AButton In pnlOverview.Controls.OfType(Of AButton).ToList
            If i <= totalButtons Then
                but.Size = New Size(buttonWidth, buttonHeight)
                but.Visible = True
                visButtons.Add(but)
            Else
                but.Visible = False
                but.Text = ""
                If but.AP IsNot Nothing Then
                    DwmUnregisterThumbnail(startThumbsDict.GetValueOrDefault(but.AP.Id, IntPtr.Zero))
                    startThumbsDict.TryRemove(but.AP.Id, Nothing)
                End If
                but.AP = Nothing
                but.pidCache = 0
            End If
            i += 1
        Next

        pnlMessage.Size = New Size(buttonWidth, buttonHeight)
        pbMessage.Size = pnlMessage.Size
        chkHideMessage.Location = New Point(pnlMessage.Width - chkHideMessage.Width, pnlMessage.Height - chkHideMessage.Height)

        Return visButtons
    End Function



    Private Function UpdateButtonLayout(count As Integer) As List(Of AButton)

        'Return UpdateButtonLayout2(count)

        'pnlOverview.SuspendLayout()
        Dim numCols As Integer

        Select Case count + If(My.Settings.hideMessage, 0, 1)
            Case 0 To 4
                numCols = 2
            Case 5 To 9
                numCols = 3
            Case 10 To 16
                numCols = 4
            Case 17 To 25
                numCols = 5
            Case Else
                numCols = 6
        End Select
        Dim numRows As Integer = numCols

        If Me.WindowState = FormWindowState.Maximized OrElse My.Settings.ApplyAlterNormal Then

            Dim numAlts = count + If(My.Settings.hideMessage, 0, 1)

            numCols = 2 + My.Settings.ExtraMaxColRow
            numRows = 2 - If(My.Settings.OneLessRowCol, 1, 0)

            While numAlts > numCols * numRows
                numCols += 1
                numRows += 1
                If numCols >= 7 Then Exit While
            End While

            If pbZoom.Width < pbZoom.Height Then
                Dim swapper As Integer = numCols
                numCols = numRows
                numRows = swapper
            End If
        End If

        Dim newSZ As New Size(pbZoom.Size.Width \ numCols, pbZoom.Size.Height \ numRows)
        Dim widthTooMuch As Boolean = False
        Dim heightTooMuch As Boolean = False

        If newSZ.Width * numCols > pbZoom.Width Then widthTooMuch = True
        If newSZ.Height * numRows > pbZoom.Height Then heightTooMuch = True

        Dim totalButtons = numCols * numRows

        Dim i = If(My.Settings.hideMessage, 1, 2)

        Dim visButtons As New List(Of AButton)

        For Each but As AButton In pnlOverview.Controls.OfType(Of AButton).ToList

            If i <= totalButtons Then
                but.Size = newSZ
                If widthTooMuch AndAlso i Mod numCols = 0 Then but.Width -= 1 'last column
                If heightTooMuch AndAlso i > (numRows - 1) * numRows Then but.Height -= 1 'last row
                but.Visible = True
                visButtons.Add(but)
            Else
                but.Visible = False
                but.Text = ""
                If but.AP IsNot Nothing Then
                    DwmUnregisterThumbnail(startThumbsDict.GetValueOrDefault(but.AP.Id, IntPtr.Zero))
                    startThumbsDict.TryRemove(but.AP.Id, Nothing)
                End If
                but.AP = Nothing
                but.pidCache = 0
            End If
            i += 1
        Next

        pnlMessage.Size = newSZ
        pbMessage.Size = newSZ
        chkHideMessage.Location = New Point(pnlMessage.Width - chkHideMessage.Width, pnlMessage.Height - chkHideMessage.Height)

        'pnlOverview.ResumeLayout(True)

        Return visButtons
    End Function
    Private Sub Buttons_BackColorChanged(sender As Button, e As EventArgs) Handles btnQuit.BackColorChanged, btnMax.BackColorChanged, btnMin.BackColorChanged
        Dim target As Control = sender
        While TypeOf target IsNot Form AndAlso target.BackColor.ToArgb = Color.Transparent.ToArgb
            target = target.Parent
        End While
        sender.FlatAppearance.BorderColor = target.BackColor
    End Sub
    Private Sub BtnQuit_MouseEnter(sender As Button, e As EventArgs) Handles btnQuit.MouseEnter
        cornerNE.BackColor = Color.Red
        sender.BackColor = Color.Red
    End Sub
    Private Sub BtnQuit_MouseLeave(sender As Button, e As EventArgs) Handles btnQuit.MouseLeave
        cornerNE.BackColor = Color.Transparent
        sender.BackColor = Color.Transparent
    End Sub
    Private Sub BtnQuit_MouseDown(sender As Button, e As MouseEventArgs) Handles btnQuit.MouseDown
        cornerNE.BackColor = Color.FromArgb(255, 102, 102)
        sender.BackColor = Color.FromArgb(255, 102, 102)
    End Sub

    Private Sub BtnQuit_Click(sender As Button, e As EventArgs) Handles btnQuit.Click
        Me.Close()
    End Sub
    Private Sub Various_MouseUp(sender As Control, e As MouseEventArgs) Handles Me.MouseUp, btnQuit.MouseUp, pnlSys.MouseUp, pnlButtons.MouseUp, btnStart.MouseUp, cboAlt.MouseUp, cmbResolution.MouseUp, ChkEqLock.MouseUp
        UntrapMouse(e.Button) 'fix mousebutton stuck
    End Sub

    Private Sub BtnMin_Click(sender As Button, e As EventArgs) Handles btnMin.Click
        Debug.Print("btnMin_Click")
        'suppressWM_MOVEcwp = True
        wasMaximized = (Me.WindowState = FormWindowState.Maximized)
        If Not wasMaximized Then
            restoreLoc = Me.Location
            Debug.Print("restoreLoc " & restoreLoc.ToString)
        End If
        AppActivate(scalaPID)

        If My.Settings.MinMin AndAlso pnlOverview.Visible AndAlso My.Settings.gameOnOverview Then
            Detach(True)
            MinAllActiveOverview()
        ElseIf My.Settings.MinMin AndAlso cboAlt.SelectedIndex <> 0 AndAlso AltPP?.isSDL Then
            AltPP.Hide()
        Else
            Debug.Print("swl parent")
            AstoniaProcess.RestorePos(True)
            Detach(True)
        End If
        Me.WindowState = FormWindowState.Minimized
        Debug.Print($"WS {Me.WindowState}")
        'suppressWM_MOVEcwp = False
    End Sub

    Private Sub MinAllActiveOverview()
        For Each but As AButton In pnlOverview.Controls.OfType(Of AButton).Where(Function(b) b.AP IsNot Nothing)
            If Not but.AP.HasExited Then
                If but.AP.isSDL Then
                    but.AP.Hide()
                Else
                    but.AP.RestoreSinglePos()
                End If
            End If
        Next
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
        If Not cboAlt.Items.Contains(sender.AP) Then
            PopDropDown(cboAlt)
        End If
        cboAlt.SelectedItem = sender.AP
    End Sub
    Private Sub BtnAlt_MouseDown(sender As AButton, e As MouseEventArgs) ' handles AButton.mousedown
        Debug.Print($"MouseDown {e.Button}")
        If sender.AP Is Nothing Then Exit Sub
        Select Case e.Button
            Case MouseButtons.XButton1, MouseButtons.XButton2
                sender.Select()
                sender.AP.Activate()
            Case MouseButtons.Left
                If e.Clicks = 2 Then
                    If Not cboAlt.Items.Contains(sender.AP) Then
                        PopDropDown(cboAlt)
                    End If
                    cboAlt.SelectedItem = sender.AP
                End If
        End Select
    End Sub
    Private Sub BtnAlt_MouseEnter(sender As AButton, e As EventArgs) ' Handles AButton.MouseEnter
        If My.Settings.gameOnOverview Then Exit Sub
        If sender.AP Is Nothing Then Exit Sub
        opaDict(sender.AP.Id) = 240
    End Sub
    Private Sub BtnAlt_MouseLeave(sender As AButton, e As EventArgs) ' Handles AButton.MouseLeave
        If sender.AP Is Nothing Then Exit Sub
        opaDict(sender.AP.Id) = If(chkDebug.Checked, 128, 255)
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

    'Private _restoreLoc As Point
    '''' <summary>
    '''' Used to set Scala to the right position when restoring from maximized state
    '''' </summary>
    'Private Property RestoreLoc As Point
    '    Get
    '        Return _restoreLoc
    '    End Get
    '    Set(ByVal value As Point)
    '        _restoreLoc = value
    '        Debug.Print($"Set restoreloc to {value}")
    '    End Set
    'End Property

    Dim prevWA As Rectangle
    Dim restoreLoc As Point = Me.Location
    Private Sub BtnMax_Click(sender As Button, e As EventArgs) Handles btnMax.Click
        Debug.Print("btnMax_Click")
        suppressWM_MOVEcwp = True
        '🗖,🗗,⧠
        If Me.WindowState = FormWindowState.Normal Then
            My.Settings.zoom = cmbResolution.SelectedIndex
            'go maximized
            If Me.Location <> New Point(-32000, -32000) Then
                restoreLoc = Me.Location
            End If
            Dim scrn As Screen = Screen.FromPoint(restoreLoc + New Point(Me.Width / 2, Me.Height / 2))
            Debug.Print("screen workarea " & scrn.WorkingArea.ToString)
            Debug.Print("screen bounds " & scrn.Bounds.ToString)
            prevWA = scrn.WorkingArea

            Dim leftBorder As Integer = scrn.WorkingArea.Width * My.Settings.MaxBorderLeft / 1000
            Dim topBorder As Integer = scrn.WorkingArea.Height * My.Settings.MaxBorderTop / 1000
            Dim rightborder As Integer = scrn.WorkingArea.Width * My.Settings.MaxBorderRight / 1000
            Dim botBorder As Integer = scrn.WorkingArea.Height * My.Settings.MaxBorderBot / 1000

            'find out where taskbar is and add 1 pixel at that location
            'dirty hack to enable dragging when maximized
            If leftBorder + rightborder + topBorder + botBorder = 0 Then
                If scrn.WorkingArea.Left <> scrn.Bounds.Left Then leftBorder = 1
                If scrn.WorkingArea.Top <> scrn.Bounds.Top Then topBorder = 1
                If scrn.WorkingArea.Right <> scrn.Bounds.Right Then rightborder = 1
                If scrn.WorkingArea.Bottom <> scrn.Bounds.Bottom Then botBorder = 1
            End If
            'if taskbar set to auto hide find where it is hiding
            If leftBorder + rightborder + topBorder + botBorder = 0 Then
                'Dim monifo As New MonitorInfo With {.cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(MonitorInfo))}
                'GetMonitorInfo(MonitorFromPoint(scrn.Bounds.Location, MONITOR.DEFAULTTONEAREST), monifo)
                Dim pabd As New APPBARDATA With {
                        .cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(APPBARDATA)),
                        .rc = scrn.Bounds.ToRECT}
                For edge = 0 To 3
                    pabd.uEdge = edge
                    If SHAppBarMessage(ABM.GETAUTOHIDEBAREX, pabd) <> IntPtr.Zero Then
                        Select Case edge
                            Case 0
                                leftBorder = 1
                                Debug.Print("Hidden taskbar left")
                            Case 1
                                topBorder = 1
                                Debug.Print("Hidden taskbar top")
                            Case 2
                                rightborder = 1
                                Debug.Print("Hidden taskbar right")
                            Case 3
                                botBorder = 1
                                Debug.Print("Hidden taskbar bottom")
                        End Select
                        Exit For
                    End If
                Next
            End If
            'if no taskbar present add to bottom
            If leftBorder + rightborder + topBorder + botBorder = 0 Then
                Debug.Print("no taskbar present")
                botBorder = 1
            End If

            Debug.Print($"leftborder {leftBorder}")
            Debug.Print($"topborder {topBorder}")
            Debug.Print($"rightborder {rightborder}")
            Debug.Print($"botborder {botBorder}")

            Me.MaximizedBounds = New Rectangle(scrn.WorkingArea.Left - scrn.Bounds.Left + leftBorder,
                                           scrn.WorkingArea.Top - scrn.Bounds.Top + topBorder,
                                           scrn.WorkingArea.Width - leftBorder - rightborder,
                                           scrn.WorkingArea.Height - topBorder - botBorder)
            Debug.Print("new maxbound " & MaximizedBounds.ToString)
            If Me.WindowState = FormWindowState.Normal AndAlso Me.Location <> New Point(-32000, -32000) Then
                restoreLoc = Me.Location
                Debug.Print("restoreLoc " & restoreLoc.ToString)
            End If
            'ReZoom()
            If Me.Location = New Point(-32000, -32000) Then Me.Location = restoreLoc
            Me.WindowState = FormWindowState.Maximized
            sender.Text = "🗗"
            Me.Invalidate()
            ttMain.SetToolTip(sender, "Restore")
            wasMaximized = True
            FrmSizeBorder.Opacity = 0
        ElseIf Me.WindowState = FormWindowState.Maximized Then 'go normal
            Debug.Print($"restorebounds {RestoreBounds.Location}")
            Debug.Print($"maximizbounds {MaximizedBounds.Location}")
            Debug.Print($"restoreloc    {restoreLoc}")
            Debug.Print($"My.Settings.l {My.Settings.location}")
            Me.Location = restoreLoc
            Me.WindowState = FormWindowState.Normal
            sender.Text = "⧠"
            ttMain.SetToolTip(sender, "Maximize")
            'wasMaximized = False
            ReZoom(My.Settings.resol)
            'wasMaximized = True
            'Me.Location = restoreLoc
            AOshowEqLock = False
            FrmSizeBorder.Opacity = If(chkDebug.Checked, 1, 0.01)
            FrmSizeBorder.Opacity = If(My.Settings.SizingBorder, FrmSizeBorder.Opacity, 0)
        End If
        If cboAlt.SelectedIndex > 0 Then
            Attach(AltPP)
            AltPP?.CenterBehind(pbZoom)
        End If
        moveBusy = False
        suppressWM_MOVEcwp = False
        FrmSizeBorder.Bounds = Me.Bounds
    End Sub

    Private Sub BtnStart_Click(sender As Button, e As EventArgs) Handles btnStart.Click
        cboAlt.DroppedDown = False
        tmrTick.Stop()
        Dim prevAlt As AstoniaProcess = AltPP
        Debug.Print($"prevAlt?.Name {prevAlt?.Name}")
        AstoniaProcess.RestorePos(True)
        cboAlt.SelectedIndex = 0
        If prevAlt?.Id <> 0 Then
            pnlOverview.Controls.OfType(Of AButton).FirstOrDefault(Function(ab As AButton) ab.AP IsNot Nothing AndAlso ab.AP.Id = prevAlt.Id)?.Select()
        Else
            pnlOverview.Controls.OfType(Of AButton).First().Select()
        End If
    End Sub

    Public Shared ReadOnly scalaPID As Integer = Process.GetCurrentProcess().Id
    Public Shared topSortList As List(Of String) = My.Settings.topSort.Split(vbCrLf.ToCharArray, StringSplitOptions.RemoveEmptyEntries).ToList
    Public Shared botSortList As List(Of String) = My.Settings.botSort.Split(vbCrLf.ToCharArray, StringSplitOptions.RemoveEmptyEntries).ToList
    Public Shared blackList As List(Of String) = topSortList.Intersect(botSortList).ToList
    'Private EQLockClick As Boolean = False

    Public ReadOnly restoreParent As UInteger = GetWindowLong(Me.Handle, GWL_HWNDPARENT)
    Private prevHWNDParent As IntPtr = restoreParent
    Public Function Attach(ap As AstoniaProcess) As Long
        If ap Is Nothing OrElse prevHWNDParent = ap.MainWindowHandle Then Return 0
        Debug.Print($"Attach to: {ap.Name} {ap.Id}")
        prevHWNDParent = ap.MainWindowHandle
        If Not pnlOverview.Visible Then
            Dim rcAst As RECT
            GetWindowRect(ap.MainWindowHandle, rcAst)
            If rcAst.top <= Me.Location.Y Then
                SetWindowPos(ap.MainWindowHandle, ScalaHandle, Me.Location.X, Me.Location.Y + 2, -1, -1, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate)
            End If
        End If
        Return SetWindowLong(ScalaHandle, GWL_HWNDPARENT, ap.MainWindowHandle)
    End Function
#If DEBUG Then
    Private prevDetach As String
#End If
    Public Function Detach(show As Boolean) As Long
#If DEBUG Then
        If prevDetach <> AltPP?.Name Then
            Debug.Print($"Detach from: {AltPP?.Name} show:{show}")
            prevDetach = AltPP?.Name
        End If
#End If
        Try
            Return SetWindowLong(ScalaHandle, GWL_HWNDPARENT, restoreParent)
        Finally
            prevHWNDParent = restoreParent
            If show Then
                Task.Run(Sub()
                             Threading.Thread.Sleep(100)
                             AllowSetForegroundWindow(scalaPID)
                             Invoke(Sub() Activate())
                             Threading.Thread.Sleep(100)
                             FlashWindow(ScalaHandle, True) 'show on taskbar
                             Try
                                 AppActivate(scalaPID)
                             Catch
                             End Try
                             FlashWindow(ScalaHandle, False) 'stop blink
                         End Sub)
            End If
        End Try
    End Function


    Private Sub setActive(active As Boolean)
        Dim fcol As Color = Color.FromArgb(&HFF666666)
        If active Then fcol = If(My.Settings.DarkMode, Colors.LightText, SystemColors.ControlText)
        lblTitle.ForeColor = fcol
        btnMax.ForeColor = fcol
        btnMin.ForeColor = fcol
        btnStart.ForeColor = fcol
        cboAlt.ForeColor = fcol
        cmbResolution.ForeColor = fcol
        For Each but As Button In pnlButtons.Controls
            If but.Contains(MousePosition) Then
                If but Is btnQuit Then
                    but.ForeColor = Color.White
                Else
                    but.ForeColor = If(My.Settings.DarkMode, Color.White, SystemColors.ControlText)
                End If
            Else
                but.ForeColor = fcol
            End If
        Next
        cboAlt.ForeColor = fcol
        cmbResolution.ForeColor = fcol
    End Sub
    Private Sub CloseErrorDialog()
        Try
            Dim errorHwnd = FindWindow("#32770", "error")
            If errorHwnd Then
                If FindWindowEx(errorHwnd, Nothing, "Static", "Copy new->moac failed: 32") <> IntPtr.Zero OrElse
                   FindWindowEx(errorHwnd, Nothing, "Static", "Copy new->moac failed: 5") <> IntPtr.Zero Then
                    Dim butHandle = FindWindowEx(errorHwnd, Nothing, "Button", "OK")
                    SendMessage(butHandle, &HF5, IntPtr.Zero, IntPtr.Zero)
                    Debug.Print("Error dialog closed")
                End If
            End If
        Catch
            Debug.Print("CloseErrorDialog Exception")
        End Try
    End Sub

    Private Sub Title_MouseDoubleClick(sender As Control, e As MouseEventArgs) Handles pnlTitleBar.DoubleClick, lblTitle.DoubleClick
        Debug.Print("title_DoubleClick")
        If e.Button = MouseButtons.Left Then btnMax.PerformClick()
        'FrmSizeBorder.Bounds = Me.Bounds
    End Sub




    Public Sub RestartSelf(Optional asAdmin As Boolean = True)

        Dim procStartInfo As New ProcessStartInfo With {
            .UseShellExecute = True,
            .FileName = Environment.GetCommandLineArgs()(0),
            .Arguments = """" & CType(Me.cboAlt.SelectedItem, AstoniaProcess)?.Name & """",
            .WindowStyle = ProcessWindowStyle.Normal,
            .Verb = If(asAdmin, "runas", "") 'add this to prompt for elevation
        }

        SaveLocation()
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
        SaveLocation()
        My.Settings.Save()

        tmrActive.Stop()
        tmrOverview.Stop()
        tmrTick.Stop()

        AstoniaProcess.RestorePos()

        ExecuteProcessUnElevated(Environment.GetCommandLineArgs()(0), "Someone", IO.Directory.GetCurrentDirectory())
        sysTrayIcon.Visible = False
        sysTrayIcon.Dispose()
        End 'program
    End Sub


#If DEBUG Then
    Private Sub ChkDebug_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkDebug.CheckedChanged
        Debug.Print(Screen.GetWorkingArea(sender).ToString)
        If Not pnlOverview.Visible Then
            UpdateThumb(If(sender.Checked, 122, 255))
        Else
            For Each but As AButton In pnlOverview.Controls.OfType(Of AButton)
                but.Invalidate()
            Next
        End If
        If WindowState <> FormWindowState.Maximized Then
            FrmSizeBorder.Opacity = If(sender.Checked, 1, 0.01)
        End If
        FrmBehind.BackColor = If(sender.Checked, Color.Cyan, Color.Black)
        FrmBehind.Opacity = If(sender.Checked, 1, 0.01)
        FrmSizeBorder.Opacity = If(My.Settings.SizingBorder, FrmSizeBorder.Opacity, 0)
    End Sub
#End If


    Private Sub SysTrayIcon_MouseDoubleClick(sender As NotifyIcon, e As MouseEventArgs) Handles sysTrayIcon.MouseDoubleClick
        Debug.Print("sysTrayIcon_MouseDoubleClick")
        If e.Button = MouseButtons.Right Then Exit Sub
        'If Me.WindowState = FormWindowState.Minimized Then
        '    Me.Location = RestoreLoc
        '    SetWindowLong(ScalaHandle, GWL_HWNDPARENT, AltPP.MainWindowHandle) 'hides scala from taskbar
        '    suppressWM_MOVEcwp = True
        '    Me.WindowState = If(wasMaximized, FormWindowState.Maximized, FormWindowState.Normal)
        '    suppressWM_MOVEcwp = False
        '    ReZoom(zooms(cmbResolution.SelectedIndex)) 'handled in WM_SIZE
        '    AltPP?.CenterBehind(pbZoom)
        '    btnMax.Text = If(wasMaximized, "🗗", "⧠")
        '    ttMain.SetToolTip(btnMax, If(wasMaximized, "Restore", "Maximize"))
        '    If wasMaximized Then btnMax.PerformClick()
        'End If
        If AltPP?.IsMinimized Then AltPP?.Restore()
        If Me.WindowState = FormWindowState.Minimized Then
            Me.WndProc(Message.Create(ScalaHandle, WM_SYSCOMMAND, SC_RESTORE, Nothing))
        End If
        Me.Show()
        'Me.BringToFront() 'doesn't work
        If AltPP IsNot Nothing AndAlso AltPP.Id <> 0 AndAlso AltPP.IsRunning Then
            SetWindowPos(AltPP.MainWindowHandle, SWP_HWND.NOTOPMOST, -1, -1, -1, -1, SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.IgnoreResize)
            AltPP.Activate()
        Else
            Me.TopMost = True
            Me.TopMost = My.Settings.topmost
        End If
        'moveBusy = False
    End Sub

    Private Async Sub FrmMain_Click(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        Debug.Print($"me.mousedown {sender.name} {e.Button}")
        MyBase.WndProc(Message.Create(ScalaHandle, WM_CANCELMODE, 0, 0))
        cmsQuickLaunch.Close()
        Await Task.Delay(200)
        Debug.Print($"Me.MouseDown awaited")
        If Not pnlOverview.Visible Then
            pbZoom.Visible = True
            If e.Button <> MouseButtons.None Then
                AltPP?.Activate()
            End If
        Else
            AButton.ActiveOverview = My.Settings.gameOnOverview
        End If
    End Sub

    Private Function WM_MM_GetWParam() As Integer
        Dim wp As Integer
        wp = wp Or If(MouseButtons.HasFlag(MouseButtons.Left), MK_LBUTTON, 0)
        wp = wp Or If(MouseButtons.HasFlag(MouseButtons.Right), MK_RBUTTON, 0)
        wp = wp Or If(MouseButtons.HasFlag(MouseButtons.Middle), MK_MBUTTON, 0)
        wp = wp Or If(MouseButtons.HasFlag(MouseButtons.XButton1), MK_XBUTTON1, 0)
        wp = wp Or If(MouseButtons.HasFlag(MouseButtons.XButton2), MK_XBUTTON2, 0)
        wp = wp Or If(My.Computer.Keyboard.CtrlKeyDown, MK_CONTROL, 0)
        wp = wp Or If(My.Computer.Keyboard.ShiftKeyDown, MK_SHIFT, 0)
        Return wp
    End Function

    Private Async Sub JiggerMouse()
        prevWMMMpt = New Point
        Cursor.Position += New Point(-1, -1)
        Await Task.Delay(32)
        prevWMMMpt = New Point
        Cursor.Position += New Point(1, 1)
    End Sub
    Private Sub PnlEqLock_MouseDown(sender As Panel, e As MouseEventArgs) Handles PnlEqLock.MouseDown
        Debug.Print($"pnlEqLock.MouseDown {e.Button}")

        Dim wparam As Integer = WM_MM_GetWParam()

        Dim rc As Rectangle
        GetClientRect(AltPP.MainWindowHandle, rc)

        Dim mp As Point = sender.PointToClient(MousePosition)

        Dim sx As Integer = rcC.Width / 2 - 262.Map(0, 800, 0, rcC.Width)

        Dim excludGearLock As Integer = If(AltPP?.isSDL, 18, 0)
        Dim dx As Integer = (524 - excludGearLock).Map(0, 800, 0, rcC.Width)

        Dim mx As Integer = mp.X.Map(0, sender.Bounds.Width, sx, dx)

        Dim lockHeight = 45
        If rc.Height >= 2000 Then
            lockHeight += 120
        ElseIf rc.Height >= 1500 Then
            lockHeight += 80
        ElseIf rc.Height >= 1000 Then
            lockHeight += 40
        End If
        Dim my As Integer = mp.Y.Map(0, sender.Bounds.Height, 0, lockHeight)

        Debug.Print($"mx:{mx} my:{my}")


        If e.Button = MouseButtons.Middle OrElse e.Button = MouseButtons.Right Then
            PnlEqLock.Visible = False
            sender.Capture = False
            AltPP?.Activate()
            SendMessage(AltPP.MainWindowHandle, If(e.Button = MouseButtons.Right, WM_RBUTTONDOWN, WM_MBUTTONDOWN), wparam, New LParamMap(mx, my))
        End If


    End Sub
    Private Async Sub PnlEqLock_MouseUp(sender As Panel, e As MouseEventArgs) Handles PnlEqLock.MouseUp
        Debug.Print($"pnlEqLock.MouseUp {e.Button} lock vis {PnlEqLock.Visible}")
        If (e.Button = MouseButtons.Right OrElse e.Button = MouseButtons.Middle) AndAlso PnlEqLock.Contains(MousePosition) Then
            '    'EQLockClick = True
            PnlEqLock.Visible = False
            sender.Capture = False
            If e.Button = MouseButtons.Right Then
                SendMouseInput(MouseEventF.RightUp)
            Else
                '        SendMouseInput(MouseEventF.MiddleDown)
                '        Await Task.Delay(50)
                SendMouseInput(MouseEventF.MiddleUp)
            End If
            '    Await Task.Delay(25)
            '    'EQLockClick = False
        End If
        Await Task.Run(Sub() AltPP.Activate())
    End Sub

    Private Sub PnlEqLock_Mousemove(sender As Panel, e As MouseEventArgs) Handles PnlEqLock.MouseMove
        If e.Button = MouseButtons.Right OrElse e.Button = MouseButtons.Middle Then
            sender.Visible = False
        End If
    End Sub

    Private Sub ChkEqLock_CheckedChanged(sender As CheckBox, e As EventArgs) Handles ChkEqLock.CheckedChanged
        'locked 🔒
        'unlocked 🔓
        sender.Text = If(sender.CheckState = CheckState.Unchecked, "🔓", "🔒")

    End Sub
    Private prevMPX As Integer
    Private Sub Caption_MouseMove(sender As Object, e As MouseEventArgs) Handles pnlSys.MouseMove, btnStart.MouseMove, cboAlt.MouseMove, cmbResolution.MouseMove,
                                                                                 pnlTitleBar.MouseMove, lblTitle.MouseMove,
                                                                                 pnlUpdate.MouseMove, pbUpdateAvailable.MouseMove, ChkEqLock.MouseMove,
                                                                                 pnlSys.MouseMove, btnMin.MouseMove, btnMax.MouseMove, btnQuit.MouseMove
        If cboAlt.SelectedIndex = 0 Then Exit Sub
        If prevMPX = MousePosition.X Then Exit Sub
        prevMPX = MousePosition.X

        'TODO: move follwing code to tmrTick and test sizeborder drag
        Dim ptZ As Point = Me.PointToScreen(pbZoom.Location)

        ' Debug.Print("CaptionMouseMove")

        newX = MousePosition.X.Map(ptZ.X, ptZ.X + pbZoom.Width, ptZ.X, ptZ.X + pbZoom.Width - rcC.Width) - AltPP.ClientOffset.X - My.Settings.offset.X
        newY = Me.Location.Y

        Dim flags = swpFlags
        If Not AltPP.IsActive() Then flags.SetFlag(SetWindowPosFlags.DoNotChangeOwnerZOrder)
        If AltPP.IsBelow(ScalaHandle) Then flags.SetFlag(SetWindowPosFlags.IgnoreZOrder)

        SetWindowPos(AltPP.MainWindowHandle, ScalaHandle, newX, newY, -1, -1, flags)

    End Sub

End Class