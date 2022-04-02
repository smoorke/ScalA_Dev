﻿Partial Public Class FrmMain



    Public AltPP As New AstoniaProcess()
    'Private WndClass() As String = {"MAINWNDMOAC", "䅍义乗䵄䅏C"}
#Region " Alt Dropdown "
    Friend Sub PopDropDown(sender As ComboBox)
        AstoniaProcess.ResetEnumCache()

        Dim current As AstoniaProcess = CType(sender.SelectedItem, AstoniaProcess)
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
    Private prevItem As New AstoniaProcess()
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

        'If AltPP.Id = 0 AndAlso that.SelectedIndex = 0 Then
        '    Exit Sub
        'End If

        TickCounter = 0

        SetWindowLong(Me.Handle, GWL_HWNDPARENT, restoreParent)
        RestorePos(AltPP)

        AltPP = CType(that.SelectedItem, AstoniaProcess)
        UpdateTitle()
        If that.SelectedIndex = 0 Then
            pnlOverview.SuspendLayout()
            pnlOverview.Show()
            Dim visbut = UpdateButtonLayout(AstoniaProcess.Enumerate(blackList).Count)
            For Each but As AButton In visbut.Where(Function(b) b.Text <> "")
                but.Image = Nothing
                but.BackgroundImage = Nothing
                but.Text = String.Empty
            Next
            pnlOverview.ResumeLayout()
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

            Debug.Print($"rcW:{rcW}")
            Debug.Print($"rcC:{rcC}")

            'check if target is running as windowed. if not ask to run it with -w
            If rcC.Width = 0 AndAlso rcC.Height = 0 Then
                'MessageBox.Show("Client is not running in windowed mode", "Error")
                Await AltPP.ReOpenAsWindowed()
                'cboAlt.SelectedIndex = 0
                Exit Sub
            End If
            Try
                AppActivate(AltPP.Id)
            Catch
            End Try
            Dim ptt As Point

            Debug.Print("ClientToScreen")
            ClientToScreen(AltPP.MainWindowHandle, ptt)

            AstClientOffset = New Size(ptt.X - rcW.Left, ptt.Y - rcW.Top)

            'Debug.Print("updateTitle")
            'UpdateTitle()

            'newX = Me.Left + pbZoom.Left - AstClientOffset.Width - My.Settings.offset.X
            'newY = Me.Top + pbZoom.Top - AstClientOffset.Height - My.Settings.offset.Y

            Debug.Print("SetWindowLong")

            altTopM = If(AltPP.IsTopMost(), SWP_HWND.TOPMOST, SWP_HWND.NOTOPMOST)

            'SetWindowPos(AltPP.MainWindowHandle, -2, 0, 0, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreMove)
            SetWindowLong(Me.Handle, GWL_HWNDPARENT, AltPP.MainWindowHandle) ' have Client always be beneath ScalA (set Scala to be owned by client)
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

            'Dim unused = Task.Run(Sub() AltPP?.CenterWindowPos(ScalaHandle, Me.Left + pbZoom.Left + (pbZoom.Width / 2), Me.Top + pbZoom.Top + (pbZoom.Height / 2)))
            Debug.Print("updateThumb")
            If rectDic.ContainsKey(item.Id) Then
                AnimateThumb(rectDic(item.Id), New Rectangle(pbZoom.Left, pbZoom.Top, pbZoom.Right, pbZoom.Bottom))
            Else
                UpdateThumb(If(chkDebug.Checked, 128, 255))
            End If
            rectDic.Clear()
            Try
                AppActivate(AltPP.Id)
            Catch
            End Try
            sysTrayIcon.Icon = AltPP?.GetIcon
            'AltPP?.CenterWindowPos(ScalaHandle, Me.Left + pbZoom.Left + (pbZoom.Width / 2), Me.Top + pbZoom.Top + (pbZoom.Height / 2))
        Else 'AltPP.Id = 0


        End If

        Debug.Print("tmrTick.Enabled")
        tmrTick.Enabled = True

        prevItem = that.SelectedItem

        If that.SelectedIndex > 0 Then
            pbZoom.Show()
        Else
            AButton.ActiveOverview = My.Settings.gameOnOverview
        End If

    End Sub

    Dim AstClientOffset As New Size(0, 0)
    Public Sub RestorePos(altPP As AstoniaProcess)
        If altPP?.IsRunning() Then
            SetWindowPos(altPP.MainWindowHandle, altTopM, rcW.Left, rcW.Top, -1, -1, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate Or SetWindowPosFlags.ASyncWindowPosition)
        End If
        For Each id In restoreDic.Keys
            Try
                Dim altAP As New AstoniaProcess(Process.GetProcessById(id))
                If altAP.IsRunning() Then
                    SetWindowPos(altAP.MainWindowHandle, SWP_HWND.TOP, restoreDic(id).X, restoreDic(id).Y, -1, -1, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate Or SetWindowPosFlags.ASyncWindowPosition)
                End If
            Catch
            End Try
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

        If My.Settings.SingleInstance AndAlso IPC.AlreadyOpen Then
            IPC.RequestActivation = True
            End
        End If
        IPC.AlreadyOpen = True

        sysTrayIcon.Visible = True

        If New Version(My.Settings.SettingsVersion) < My.Application.Info.Version Then
            My.Settings.Upgrade()
            My.Settings.SettingsVersion = My.Application.Info.Version.ToString
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
        Dim APlist As List(Of AstoniaProcess) = AstoniaProcess.Enumerate(blackList).ToList
        For Each ap As AstoniaProcess In APlist
            cboAlt.Items.Add(ap)
            If args.Count > 1 AndAlso ap.Name = args(1) Then
                Debug.Print($"Selecting '{ap.Name}'")
                cboAlt.SelectedItem = ap
            End If
        Next
        cboAlt.EndUpdate()


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
        test.Items.Add(New ToolStripMenuItem("Button Info", Nothing, AddressOf dBug.ButtonInfo))
        chkDebug.ContextMenuStrip = test
        AddHandler test.Opening, Sub()
                                     Debug.Print("test Opening")
                                     AppActivate(scalaPID)
                                     UntrapRMouse()
                                 End Sub
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
    Private Sub TmrMove_Tick(sender As Object, e As EventArgs) Handles tmrMove.Tick
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
#End Region


    Private Sub FrmMain_Closing(sender As Form, e As EventArgs) Handles Me.Closing
        RestorePos(AltPP)
        If Me.WindowState = FormWindowState.Normal Then
            My.Settings.location = Me.Location
        End If
        tmrActive.Stop()
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

    Private Sub TmrTick_Tick(sender As Timer, e As EventArgs) Handles tmrTick.Tick



        If Not AltPP?.IsRunning() Then
            Debug.Print("Not AltPP?.IsRunning()")
            If Not My.Settings.CycleOnClose Then
                tmrTick.Enabled = False
                cboAlt.SelectedIndex = 0
                tmrOverview.Enabled = True
                AppActivate(scalaPID)
                Exit Sub
            Else 'CycleOnClose
                Cycle()
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
        End If

        If pbZoom.Contains(MousePosition) Then

            If pci.flags = 0 Then ' cursor is hidden
                wasVisible = False
                Exit Sub ' do not move astonia when cursor is hidden. fixes scrollbar thumb.
                ' note there is a client bug where using thumb will intermittently cause it to jump down wildly
            End If

            Dim ptZ As Point = Me.PointToScreen(pbZoom.Location)

            newX = MousePosition.X.Map(ptZ.X, ptZ.X + pbZoom.Width, ptZ.X, ptZ.X + pbZoom.Width - rcC.Width) - AstClientOffset.Width - My.Settings.offset.X
            newY = MousePosition.Y.Map(ptZ.Y, ptZ.Y + pbZoom.Height, ptZ.Y, ptZ.Y + pbZoom.Height - rcC.Height) - AstClientOffset.Height - My.Settings.offset.Y

            Task.Run(Sub()
                         Try
                             Dim flags As SetWindowPosFlags = SetWindowPosFlags.IgnoreResize Or
                                                              SetWindowPosFlags.DoNotActivate Or
                                                              SetWindowPosFlags.ASyncWindowPosition
                             If IsQlCtxOpen() Then flags = flags Or SetWindowPosFlags.IgnoreZOrder
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
            UpdateThumb(If(chkDebug.Checked, 128, 255))
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
            UntrapRMouse() ' fix rbuttn stuck bug
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
                        Cycle()
                    Case 3 'ctrl-shift-space
                        Cycle(True)
                End Select
            Case WM_SYSCOMMAND
                Select Case m.WParam
                    Case SC_RESTORE
                        Debug.Print("SC_RESTORE")
                        SetWindowLong(Me.Handle, GWL_HWNDPARENT, AltPP.MainWindowHandle)
                        'Me.ShowInTaskbar = False
                        Debug.Print("wasMax " & wasMaximized)
                        If wasMaximized Then
                            SendMessage(ScalaHandle, WM_SYSCOMMAND, SC_MAXIMIZE, IntPtr.Zero)
                            'btnMax.PerformClick
                            Exit Sub
                        End If
                        If minByMenu Then
                            Me.Location = RestoreLoc
                        End If
                        minByMenu = False
                    Case SC_MAXIMIZE
                        Debug.Print("SC_MAXIMIZE " & m.LParam.ToString)
                        If Me.WindowState = FormWindowState.Minimized Then
                            Me.WindowState = FormWindowState.Normal
                            Me.Location = RestoreLoc
                        End If
                        btnMax.PerformClick()

                        Debug.Print("wasMax " & wasMaximized)

                        m.Result = 0
                    Case SC_MINIMIZE
                        Debug.Print("SC_MINIMIZE")
                        minByMenu = True
                        wasMaximized = (Me.WindowState = FormWindowState.Maximized)
                        Debug.Print("wasMax " & wasMaximized)
                        If Not wasMaximized Then
                            RestoreLoc = Me.Location
                            Debug.Print("restoreLoc " & RestoreLoc.ToString)
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

    Private Sub Cycle(Optional up As Boolean = False)

        PopDropDown(cboAlt)
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
            'AppActivate(scalaPID)
            Exit Sub
        End If
        cboAlt.SelectedIndex = requestedindex
    End Sub



    ReadOnly startThumbsDict As New Dictionary(Of Integer, IntPtr)
    ReadOnly opaDict As New Dictionary(Of Integer, Byte)
    ReadOnly rectDic As New Dictionary(Of Integer, Rectangle)
    ReadOnly restoreDic As New Dictionary(Of Integer, Point)

    Const dimmed As Byte = 240
    Private TickCounter As Integer = 0

    Friend Shared AOBusy As Boolean = False

    Friend Shared apSorter As AstoniaProcessSorter

    Private Sub TmrOverview_Tick(sender As Timer, e As EventArgs) Handles tmrOverview.Tick

        If Me.WindowState = FormWindowState.Minimized Then
            Exit Sub
        End If

        'Debug.Print("tmrStartup.Tick")
#If DEBUG Then
        chkDebug.Text = TickCounter
#End If

        Dim alts As List(Of AstoniaProcess) = AstoniaProcess.Enumerate(blackList).OrderBy(Function(ap) ap.Name, apSorter).ToList

        pnlOverview.SuspendLayout()
        Dim visibleButtons As List(Of AButton) = UpdateButtonLayout(alts.Count)

        Dim botCount = alts.Where(Function(ap) botSortList.Contains(ap.Name)).Count()
        Dim topCount = alts.Count - botCount
        Dim skipCount = visibleButtons.Count - botCount

        Dim apCounter = 0
        Dim butCounter = 0

        For Each but As AButton In visibleButtons
            butCounter += 1
            'Debug.Print($"apCount < alts.Count AndAlso (i < topCount OrElse i > skipCount")
            'Debug.Print($"{apCount} < {alts.Count} AndAlso ({i} < {topCount} OrElse {i} > {skipCount}")
            If apCounter < alts.Count AndAlso (butCounter <= topCount OrElse butCounter > skipCount) Then 'buttons with alts

                Dim ap As AstoniaProcess = alts(apCounter)
                Dim apID As Integer = ap?.Id
                but.Tag = ap
                but.Text = ap.Name
                If ap?.IsActive() Then
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
                                 If localTick = localAPC OrElse but.BackgroundImage Is Nothing Then
                                     Using ico As Bitmap = ap.GetIcon?.ToBitmap
                                         If ico IsNot Nothing Then
                                             but.BeginInvoke(updateButtonBackgroundImage, {but, New Bitmap(ico, New Size(16, 16))})
                                         Else
                                             but.BeginInvoke(updateButtonBackgroundImage, {but, Nothing})
                                         End If
                                     End Using
                                 End If
                                 If localTick = localAPC OrElse but.Image Is Nothing Then
                                     Me.BeginInvoke(updateButtonImage, {but, ap.GetHealthbar()})
                                 End If
                             End Sub)
                End If

                apCounter += 1

                but.ContextMenuStrip = cmsAlt

                If Not startThumbsDict.ContainsKey(apID) Then
                    Dim thumbid As IntPtr = IntPtr.Zero
                    DwmRegisterThumbnail(Me.Handle, ap.MainWindowHandle, thumbid)
                    startThumbsDict(apID) = thumbid
                    Debug.Print($"registered thumb {startThumbsDict(apID)} {ap.Name} {apID}")
                End If

                rectDic(apID) = but.ThumbRECT
                Dim prp As New DWM_THUMBNAIL_PROPERTIES With {
                                   .dwFlags = DwmThumbnailFlags.DWM_TNP_OPACITY Or DwmThumbnailFlags.DWM_TNP_SOURCECLIENTAREAONLY Or DwmThumbnailFlags.DWM_TNP_VISIBLE Or DwmThumbnailFlags.DWM_TNP_RECTDESTINATION,
                                   .opacity = opaDict.GetValueOrDefault(apID, &HFF),
                                   .fSourceClientAreaOnly = True,
                                   .fVisible = True,
                                   .rcDestination = rectDic(apID)
                               }
                DwmUpdateThumbnailProperties(startThumbsDict(apID), prp)

                If My.Settings.gameOnOverview Then 'todo move this to seperate timer and make async

                    Dim rccB As Rectangle
                    GetClientRect(ap?.MainWindowHandle, rccB)

                    Dim pci As New CURSORINFO With {.cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(CURSORINFO))}
                    GetCursorInfo(pci)
                    If pci.flags <> 0 Then ' cursor is visible
                        If Not wasVisible AndAlso ap?.IsActive() Then
                            Debug.Print("scrollthumb released")
                            If storedY <> pci.ptScreenpos.y Then
                                Debug.Print("scrollthumb moved")
                                Dim factor As Double = but.ThumbRectangle.Height / rccB.Height
                                Dim movedY As Integer = storedY + ((pci.ptScreenpos.y - storedY) * factor)
                                Cursor.Position = New Point(pci.ptScreenpos.x, movedY)
                            End If
                        End If
                        storedY = pci.ptScreenpos.y
                        wasVisible = True
                    End If

                    If Not AOBusy AndAlso but.ThumbContains(MousePosition) Then
                        If pci.flags = 0 Then ' cursor is hidden
                            wasVisible = False
                            Exit For ' do not move astonia when cursor is hidden. fixes scrollbar thumb.
                            ' note there is a client bug where using thumb will intermittently cause it to jump down wildly
                        End If

                        SetWindowLong(Me.Handle, GWL_HWNDPARENT, ap?.MainWindowHandle)

                        Dim rcwB As Rectangle
                        Dim pttB As Point

                        GetWindowRect(ap.MainWindowHandle, rcwB)
                        ClientToScreen(ap.MainWindowHandle, pttB)

                        If Not restoreDic.ContainsKey(apID) Then
                            restoreDic.Add(apID, rcwB.Location)
                        End If

                        Dim AstClientOffsetB = New Size(pttB.X - rcwB.Left, pttB.Y - rcwB.Top)

                        Dim ptZB = Me.PointToScreen(but.ThumbRECT.Location)
                        Dim newXB = MousePosition.X.Map(ptZB.X, ptZB.X + but.ThumbRectangle.Width, ptZB.X, ptZB.X + but.ThumbRECT.Width - but.ThumbRECT.X - rccB.Width) - AstClientOffsetB.Width - My.Settings.offset.X
                        Dim newYB = MousePosition.Y.Map(ptZB.Y, ptZB.Y + but.ThumbRectangle.Height, ptZB.Y, ptZB.Y + but.ThumbRECT.Height - but.ThumbRECT.Top - rccB.Height) - AstClientOffsetB.Height - My.Settings.offset.Y


                        Task.Run(Sub()
                                     AOBusy = True
                                     Try
                                         Dim flags As SetWindowPosFlags = SetWindowPosFlags.IgnoreResize Or
                                                                      SetWindowPosFlags.DoNotActivate Or
                                                                      SetWindowPosFlags.ASyncWindowPosition
                                         If IsQlCtxOpen() Then flags = flags Or SetWindowPosFlags.IgnoreZOrder
                                         SetWindowPos(but.Tag?.MainWindowHandle, ScalaHandle, newXB, newYB, -1, -1, flags)
                                     Catch ex As Exception
                                     Finally
                                         AOBusy = False
                                     End Try
                                 End Sub)
                    End If 'but.ThumbContains(MousePosition)
                End If 'gameonoverview
            Else ' buttons w/o alts
                but.Text = String.Empty
                but.Tag = Nothing 'New AstoniaProcess(Nothing)
                but.ContextMenuStrip = cmsQuickLaunch
                but.BackgroundImage = Nothing
                but.Image = Nothing
            End If
        Next but

        ' Dim purgeList As List(Of Integer) = startThumbsDict.Keys.Except(alts.Select(Function(ap) ap.Id)).ToList
        For Each ppid As Integer In startThumbsDict.Keys.Except(alts.Select(Function(ap) ap.Id)).ToList 'tolist needed as we mutate the thumbsdict
            Debug.Print("unregister thumb " & startThumbsDict(ppid).ToString)
            DwmUnregisterThumbnail(startThumbsDict(ppid))
            startThumbsDict.Remove(ppid)
            rectDic.Remove(ppid)
        Next

        pnlOverview.ResumeLayout()
        TickCounter += 1
        If TickCounter >= visibleButtons.Count Then TickCounter = 0
    End Sub
    Delegate Sub updateButtonImageDelegate(but As AButton, bm As Bitmap)
    Private Shared ReadOnly updateButtonImage As New updateButtonImageDelegate(AddressOf UpdateButtonImageMethod)
    Private Shared Sub UpdateButtonImageMethod(but As AButton, bm As Bitmap)
        If but Is Nothing Then Exit Sub
        but.Image = bm
    End Sub
    Delegate Sub updateButtonBackgroundImageDelegate(but As AButton, bm As Bitmap)
    Private Shared ReadOnly updateButtonBackgroundImage As New updateButtonImageDelegate(AddressOf UpdateButtonBackgroundImageMethod)
    Private Shared Sub UpdateButtonBackgroundImageMethod(but As AButton, bm As Bitmap)
        If but Is Nothing Then Exit Sub
        but.BackgroundImage = bm
    End Sub
    Private Function GetNextPerfectSquare(num As Integer)
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
            pnlOverview.Controls.Add(but)
        Next i
        pnlOverview.ResumeLayout()
    End Sub
    Private Function UpdateButtonLayout(count As Integer) As List(Of AButton)
        pnlOverview.SuspendLayout()
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

        If Me.WindowState = FormWindowState.Maximized Then
            Select Case count + If(My.Settings.hideMessage, 0, 1)
                Case 0 To 6
                    numCols = 3
                    numRows = 2
                Case 7 To 12
                    numCols = 4
                    numRows = 3
                Case 13 To 20
                    numCols = 5
                    numRows = 4
                Case 21 To 30
                    numCols = 6
                    numRows = 5
                Case Else
                    numCols = 7
                    numRows = 6
            End Select
            If pbZoom.Width < pbZoom.Height Then
                Dim swapper As Integer = numCols
                numCols = numRows
                numRows = swapper
            End If
        End If

        Dim newSZ As New Size(pbZoom.Size.Width / numCols, pbZoom.Size.Height / numRows)
        Dim widthTooMuch As Boolean = False
        'Dim heightTooMuch As Boolean = False

        If newSZ.Width * numCols > pbZoom.Width Then widthTooMuch = True
        'If newSZ.Height * numRows > pbZoom.Height Then heightTooMuch = True

        Dim visButtons As New List(Of AButton)

        Dim i = If(My.Settings.hideMessage, 1, 2)
        For Each but As AButton In pnlOverview.Controls.OfType(Of AButton)

            If i <= numCols * numRows Then
                but.Size = newSZ
                If widthTooMuch AndAlso i Mod numCols = 0 Then but.Width -= If((pbZoom.Size.Width / numCols) Mod 1 < 0.5, 1, 2) 'last column
                'If heightTooMuch AndAlso i > (numRows - 1) * numRows Then but.Height -= 2 'last row
                but.Visible = True
                visButtons.Add(but)
            Else
                but.Visible = False
                but.Text = ""
                DwmUnregisterThumbnail(startThumbsDict.GetValueOrDefault(but.Tag?.id, IntPtr.Zero))
                startThumbsDict.Remove(but.Tag?.id)
                but.Tag = Nothing
            End If
            i += 1
        Next

        pnlMessage.Size = newSZ
        pbMessage.Size = newSZ
        chkHideMessage.Location = New Point(pnlMessage.Width - chkHideMessage.Width, pnlMessage.Height - chkHideMessage.Height)

        pnlOverview.ResumeLayout()

        Return visButtons
    End Function

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
    Private Sub BtnQuit_MouseUp(sender As Object, e As MouseEventArgs) Handles btnQuit.MouseUp
        UntrapRMouse()
    End Sub

    Private Sub BtnMin_Click(sender As Button, e As EventArgs) Handles btnMin.Click
        Debug.Print("btnMin_Click")
        wasMaximized = (Me.WindowState = FormWindowState.Maximized)
        If Not wasMaximized Then
            RestoreLoc = Me.Location
            Debug.Print("restoreLoc " & RestoreLoc.ToString)
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
        If My.Settings.gameOnOverview Then Exit Sub
        If sender.Tag Is Nothing Then Exit Sub
        opaDict(CType(sender.Tag, AstoniaProcess)?.Id) = dimmed
    End Sub
    Private Sub BtnAlt_MouseLeave(sender As AButton, e As EventArgs) ' Handles AButton.MouseLeave
        If sender.Tag Is Nothing Then Exit Sub
        opaDict(CType(sender.Tag, AstoniaProcess)?.Id) = &HFF
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

    Private _restoreLoc As Point
    Private Property RestoreLoc As Point
        Get
            Return _restoreLoc
        End Get
        Set(ByVal value As Point)
            _restoreLoc = value
            Debug.Print($"Set restoreloc to {value}")
        End Set
    End Property
    Private Sub BtnMax_Click(sender As Button, e As EventArgs) Handles btnMax.Click
        Debug.Print("btnMax_Click")
        '🗖,🗗,⧠
        If Me.WindowState <> FormWindowState.Maximized Then
            'go maximized
            For Each scrn As Screen In Screen.AllScreens
                If scrn.Bounds.Contains(Me.Location + New Point(Me.Width / 2, Me.Height / 2)) Then
                    Debug.Print("screen workarea " & scrn.WorkingArea.ToString)
                    Debug.Print("screen bounds " & scrn.Bounds.ToString)
                    Me.MaximizedBounds = New Rectangle(scrn.WorkingArea.Left - scrn.Bounds.Left, scrn.WorkingArea.Top - scrn.Bounds.Top, scrn.WorkingArea.Width, scrn.WorkingArea.Height)
                    Debug.Print("new maxbound " & MaximizedBounds.ToString)
                    If Me.WindowState = FormWindowState.Normal Then
                        RestoreLoc = Me.Location
                        Debug.Print("restoreLoc " & RestoreLoc.ToString)
                    End If
                    Me.Location = scrn.WorkingArea.Location
                    Me.WindowState = FormWindowState.Maximized
                    ReZoom(New Size(Me.MaximizedBounds.Width, Me.MaximizedBounds.Height))
                    sender.Text = "🗗"
                    wasMaximized = True
                    Exit For
                End If
            Next
        Else 'go normal
            Me.WindowState = FormWindowState.Normal
            sender.Text = "⧠"
            Me.Location = RestoreLoc
            wasMaximized = False
            ReZoom(zooms(cmbResolution.SelectedIndex))
        End If
        AltPP?.CenterWindowPos(ScalaHandle, Me.Left + pbZoom.Left + (pbZoom.Width / 2), Me.Top + pbZoom.Top + (pbZoom.Height / 2))
    End Sub

    Private Sub BtnStart_Click(sender As Button, e As EventArgs) Handles btnStart.Click
        Dim prevAlt As AstoniaProcess = AltPP
        Debug.Print($"prevAlt?.Name {prevAlt?.Name}")
        cboAlt.SelectedIndex = 0
        RestorePos(AltPP)
        If prevAlt.Id <> 0 Then
            pnlOverview.Controls.OfType(Of AButton).FirstOrDefault(Function(ab As AButton) ab.Tag?.id = prevAlt?.Id)?.Select()
        Else
            pnlOverview.Controls.OfType(Of AButton).First().Select()
        End If
    End Sub

    Private ReadOnly scalaPID As Integer = Process.GetCurrentProcess().Id
    Public Shared topSortList As List(Of String) = My.Settings.topSort.Split(CType(vbCrLf, Char()), StringSplitOptions.RemoveEmptyEntries).ToList
    Public Shared botSortList As List(Of String) = My.Settings.botSort.Split(CType(vbCrLf, Char()), StringSplitOptions.RemoveEmptyEntries).ToList
    Public Shared blackList As List(Of String) = topSortList.Intersect(botSortList).ToList


    Private Sub TmrActive_Tick(sender As Timer, e As EventArgs) Handles tmrActive.Tick

        Try
            Dim activeID As Integer = GetActiveProcessID()
            If activeID = scalaPID OrElse activeID = AltPP?.Id OrElse
                (My.Settings.gameOnOverview AndAlso New AstoniaProcess(Process.GetProcessById(activeID)).HasClassNameIn(My.Settings.className)) Then
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

        If IPC.RequestActivation Then
            IPC.RequestActivation = 0
            Debug.Print("IPC.requestActivation")

            If Me.WindowState = FormWindowState.Minimized Then
                If Not wasMaximized Then
                    SendMessage(ScalaHandle, WM_SYSCOMMAND, SC_RESTORE, IntPtr.Zero)
                Else
                    SendMessage(ScalaHandle, WM_SYSCOMMAND, SC_MAXIMIZE, IntPtr.Zero)
                End If
                'Me.Invalidate(True)
            End If
            If cboAlt.SelectedIndex > 0 Then
                AppActivate(AltPP.Id)
            Else
                AppActivate(scalaPID)
            End If
            Me.TopMost = False
            Me.TopMost = True
            Me.TopMost = My.Settings.topmost
        End If
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
            Me.Location = RestoreLoc
            SetWindowLong(Me.Handle, GWL_HWNDPARENT, AltPP.MainWindowHandle) 'hides scala from taskbar
            Me.WindowState = FormWindowState.Normal
            ReZoom(zooms(cmbResolution.SelectedIndex))
            btnMax.Text = "⧠"
        End If
        Me.Show()
        'Me.BringToFront() 'doesn't work
        If AltPP IsNot Nothing AndAlso AltPP.Id <> 0 AndAlso AltPP.IsRunning Then
            SetWindowPos(AltPP.MainWindowHandle, SWP_HWND.NOTOPMOST, -1, -1, -1, -1, SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.IgnoreResize)
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
            AppActivate(AltPP.Id)
        Else
            AButton.ActiveOverview = My.Settings.gameOnOverview
        End If
    End Sub

    Private Sub CmsAlt_Closed(sender As Object, e As ToolStripDropDownClosedEventArgs) Handles cmsAlt.Closed
        AButton.ActiveOverview = My.Settings.gameOnOverview
    End Sub

    Private Sub CmsAlt_Opened(sender As Object, e As EventArgs) Handles cmsAlt.Opened
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

    Friend Sub ButtonInfo(sender As Object, e As EventArgs)
        Dim i = 1
        For Each but As AButton In FrmMain.pnlOverview.Controls.OfType(Of AButton).Where(Function(b) b.Visible)
            Debug.Print($"Button {i} Size: {but.Size} thuRect: {but.ThumbRECT}")
            i += 1
        Next
        Debug.Print($"viscount: {FrmMain.pnlOverview.Controls.OfType(Of Button).Where(Function(b) b.Visible).Count}")
    End Sub
End Module

#End If