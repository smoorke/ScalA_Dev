Imports System.ComponentModel
Imports System.Net.Http
Imports System.Text

Public NotInheritable Class FrmSettings
    Public SysMenu As New SysMenu(Me)
    Dim startup As Boolean = True
    Private Sub FrmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'storeZoom = My.Settings.zoom

        If FrmMain.WindowState <> FormWindowState.Minimized Then
            Me.Location = New Point(
                FrmMain.Location.X + (FrmMain.Width - Me.Width) \ 2,
                FrmMain.Location.Y + (FrmMain.Height - Me.Height) \ 2)
        End If

        FrmMain.tmrHotkeys.Stop()

        If Me.Tag IsNot Nothing AndAlso TypeOf Me.Tag Is TabPage Then
            tbcSettings.SelectedTab = Me.Tag
            My.Settings.remeberSettingsTab = tbcSettings.SelectedIndex
            Me.Tag = Nothing
        Else
            tbcSettings.SelectedIndex = My.Settings.remeberSettingsTab
        End If

        chkTopMost.Checked = My.Settings.topmost
        chkRoundCorners.Checked = My.Settings.roundCorners
        chkOverViewIsGame.Checked = My.Settings.gameOnOverview

        'chkAspect.Checked = My.Settings.lockAspect
        'cmbAnchor.SelectedIndex = My.Settings.anchor

        'numXoffset.Value = My.Settings.offset.X
        'numYoffset.Value = My.Settings.offset.Y

        txtResolutions.Text = My.Settings.resolutions

        txtQuickLaunchPath.Text = My.Settings.links
        txtQuickLaunchPath.SelectionStart = txtQuickLaunchPath.TextLength

        txtExe.Text = My.Settings.exe
        txtClass.Text = My.Settings.className

        btnOpenFolderDialog.Image = GetStockIconImage(SIID.folder)
        If btnOpenFolderDialog.Image IsNot Nothing Then btnOpenFolderDialog.Text = ""

        chkSwitchToOverview.Checked = My.Settings.SwitchToOverview
        chkCycleAlts.Checked = My.Settings.CycleAlt

        chkStoAlt.Checked = My.Settings.StoAlt = Hotkey.KeyModifier.Alt
        dBug.print($"My.Settings.StoAlt {My.Settings.StoAlt}")
        chkStoCtrl.Checked = My.Settings.StoCtrl = Hotkey.KeyModifier.Control
        dBug.print($"My.Settings.StoCtrl {My.Settings.StoCtrl}")
        chkStoShift.Checked = My.Settings.StoShift = Hotkey.KeyModifier.Shift
        dBug.print($"My.Settings.StoShift {My.Settings.StoShift}")
        chkStoWin.Checked = My.Settings.StoWin = Hotkey.KeyModifier.Winkey
        dBug.print($"My.Settings.StoWin {My.Settings.StoWin}")

        chkCycleDownAlt.Checked = My.Settings.CycleAltKeyFwd = Hotkey.KeyModifier.Alt
        chkCycleDownCtrl.Checked = My.Settings.CycleCtrlKeyFwd = Hotkey.KeyModifier.Control
        chkCycleDownShift.Checked = My.Settings.CycleShiftKeyFwd = Hotkey.KeyModifier.Shift
        chkCycleDownShift.Checked = My.Settings.CycleWinKeyFwd = Hotkey.KeyModifier.Winkey

        chkCycleUpAlt.Checked = My.Settings.CycleAltKeyBwd = Hotkey.KeyModifier.Alt
        chkCycleUpCtrl.Checked = My.Settings.CycleCtrlKeyBwd = Hotkey.KeyModifier.Control
        chkCycleUpShift.Checked = My.Settings.CycleShiftKeyBwd = Hotkey.KeyModifier.Shift
        chkCycleUpWin.Checked = My.Settings.CycleWinKeyBwd = Hotkey.KeyModifier.Winkey

        txtStoKey.Text = keyNames(My.Settings.StoKey)

        txtCycleKeyUp.Text = keyNames(My.Settings.CycleKeyBwd)
        txtCycleKeyDown.Text = keyNames(My.Settings.CycleKeyFwd)

        txtCloseAll.Text = keyNames(My.Settings.CloseAllKey)

        txtTogTop.Text = keyNames(My.Settings.ToggleTopKey)

        txtAlterOverviewMinKey.Text = keyNames(My.Settings.AlterOverviewMinKey)
        txtAlterOverviewPlusKey.Text = keyNames(My.Settings.AlterOverviewPlusKey)
        txtAlterOverviewStarKey.Text = keyNames(My.Settings.AlterOverviewStarKey)

        StoKey = My.Settings.StoKey
        CycleKeyDown = My.Settings.CycleKeyFwd
        CycleKeyUp = My.Settings.CycleKeyBwd
        CloseAllKey = My.Settings.CloseAllKey
        TogTopKey = My.Settings.ToggleTopKey

        AlterOvervieKeyMin = My.Settings.AlterOverviewMinKey
        AlterOvervieKeyPlus = My.Settings.AlterOverviewPlusKey
        AlterOvervieKeyStar = My.Settings.AlterOverviewStarKey

        chkCycleOnClose.Checked = My.Settings.CycleOnClose

        chkCAALt.Checked = My.Settings.CloseAllAlt
        chkCACtrl.Checked = My.Settings.CloseAllCtrl
        chkCAShift.Checked = My.Settings.CloseAllShift
        chkCAWin.Checked = My.Settings.CloseAllWin

        chkCloseAll.Checked = My.Settings.CloseAll

        chkTogTopAlt.Checked = My.Settings.TogTopAlt
        chkTogTopCtrl.Checked = My.Settings.TogTopCtrl
        chkTogTopShift.Checked = My.Settings.TogTopShift
        chkTogTopWin.Checked = My.Settings.TogTopWin

        chkToggleTopMost.Checked = My.Settings.ToggleTop

        chkAlterOverviewMinAlt.Checked = My.Settings.AlterOverviewMinAlt
        chkAlterOverviewMinCtrl.Checked = My.Settings.AlterOverviewMinCtrl
        chkAlterOverviewMinShift.Checked = My.Settings.AlterOverviewMinShift
        chkAlterOverviewMinWin.Checked = My.Settings.AlterOverviewMinWin

        chkAlterOverviewPlusALt.Checked = My.Settings.AlterOverviewPlusAlt
        chkAlterOverviewPlusCtrl.Checked = My.Settings.AlterOverviewPlusCtrl
        chkAlterOverviewPlusShift.Checked = My.Settings.AlterOverviewPlusShift
        chkAlterOverviewPlusWin.Checked = My.Settings.AlterOverviewPlusWin

        chkAlterOverviewStarAlt.Checked = My.Settings.AlterOverviewStarAlt
        chkAlterOverviewStarCtrl.Checked = My.Settings.AlterOverviewStarCtrl
        chkAlterOverviewStarShift.Checked = My.Settings.AlterOverviewStarShift
        chkAlterOverviewStarWin.Checked = My.Settings.AlterOverviewStarWin

        chkAlterOverview.Checked = My.Settings.AlterOverview

        txtTopSort.Text = My.Settings.topSort
        txtBotSort.Text = My.Settings.botSort

        chkWhitelist.Checked = My.Settings.Whitelist

        ChkSingleInstance.Checked = My.Settings.SingleInstance

        NumBorderTop.Value = My.Settings.MaxBorderTop
        NumBorderBot.Value = My.Settings.MaxBorderBot
        NumBorderLeft.Value = My.Settings.MaxBorderLeft
        NumBorderRight.Value = My.Settings.MaxBorderRight

        ChkLessRowCol.Checked = My.Settings.OneLessRowCol

        NumExtraMax.Value = My.Settings.ExtraMaxColRow

        cmbTheme.SelectedIndex = My.Settings.Theme - 1

        cboScalingMode.SelectedIndex = My.Settings.ScalingMode

        chkCheckForUpdate.Checked = My.Settings.CheckForUpdate

        chkStartupMax.Checked = My.Settings.StartMaximized

        ChkSizeBorder.Checked = My.Settings.SizingBorder

        ChkMinMin.Checked = My.Settings.MinMin

        ChkQLShowHidden.Checked = My.Settings.QLShowHidden

        grpOverviewShortcut.Enabled = chkSwitchToOverview.Checked
        grpCycleShortcut.Enabled = chkCycleAlts.Checked
        grpCloseAllShortcut.Enabled = chkCloseAll.Checked
        grpToggleTopMost.Enabled = chkToggleTopMost.Checked
        grpAlterOverview.Enabled = chkAlterOverview.Checked

        chkHoverActivate.Checked = My.Settings.HoverActivate
        'chkHoverActivate.Enabled = My.Settings.gameOnOverview

        chkShowEnd.Checked = My.Settings.ShowEnd

        chkApplyAlterNormal.Checked = My.Settings.ApplyAlterNormal

        chkMinMaxOnSwitch.Checked = My.Settings.MaxNormOverview

        chkBlockWin.Checked = My.Settings.DisableWinKey
        chkNoAltTab.Checked = My.Settings.NoAltTab
        chkOnlyEsc.Checked = My.Settings.OnlyEsc

        chkAllowShiftEsc.Checked = My.Settings.AllowCtrlShiftEsc
        chkAllowShiftEsc.Enabled = My.Settings.OnlyEsc

        chkAutoCloseSomeone.Checked = My.Settings.AutoCloseIdle
        chkAutoCloseOnlyOnNoSome.Enabled = My.Settings.AutoCloseIdle
        chkAutoCloseOnlyOnNoSome.Checked = My.Settings.OnlyAutoCloseOnNoSomeone

        pb100PWarning.Visible = FrmMain.WindowsScaling <> 100

        validate_hotkey(Nothing, Nothing)

        Hotkey.UnregHotkey(Me)

        txtResolutions.SelectionStart = txtResolutions.TextLength

        If My.User.IsInRole(ApplicationServices.BuiltInRole.Administrator) Then
            Dim imge = LoadImage(IntPtr.Zero, "#106", 1, 16, 16, 0)
            If imge <> IntPtr.Zero Then
                Using Ico = Icon.FromHandle(imge)
                    pbUnElevate.Image = Ico.ToBitmap
                    DestroyIcon(Ico.Handle)
                End Using
            End If
            pnlElevation.Visible = True
        End If

    End Sub
    Private Sub FrmSettings_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Owner = FrmMain
        startup = False
    End Sub
    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case WM_ENTERMENULOOP
                SysMenu.Visible = True
            Case WM_EXITMENULOOP
                SysMenu.Visible = False
            Case WM_ACTIVATE
                dBug.Print("frmsettings Activate")
                FrmMain.Attach(FrmMain.AltPP)
        End Select
        If Me.Owner Is Nothing Then 'this to address ghost form when closing settings.
            Select Case m.Msg
                Case WM_ENTERMENULOOP
                    dBug.print("GhostSettings EnterMenuLoop")
                    FrmMain.AltPP?.Activate()
                    Me.Close()
                    Exit Sub
                Case WM_KEYDOWN
                    dBug.print($"GhostSettings WM_KEYDOWN {m.WParam} {m.LParam}")
                    dBug.print($"ScanCode {New LParamMap(m.LParam).scan}")
                    If FrmMain.cboAlt.SelectedIndex > 0 Then
                        FrmMain.AltPP.Activate()
                        Dim key As Keys = m.WParam
                        If Not FrmMain.AltPP.isSDL Then
                            SendMessage(FrmMain.AltPP.MainWindowHandle, WM_KEYDOWN, key, IntPtr.Zero)
                        Else
                            Dim scan As Byte = New LParamMap(m.LParam)
                            If key = Keys.Escape OrElse (key >= Keys.F1 AndAlso key <= Keys.F12) OrElse
                               key = Keys.Back Then
                                SendScanKey(scan)
                            ElseIf key = Keys.Delete OrElse key = Keys.PageUp OrElse key = Keys.PageDown OrElse
                                   key = Keys.End OrElse key = Keys.Home Then
                                SendScanKeyEx(scan)
                            Else
                                SendKey(key)
                            End If
                        End If
                    End If
                    'Me.Close()
                    Exit Sub
                Case WM_KEYUP
                    dBug.Print($"GhostSettings WM_KEYUP {m.WParam} {m.LParam}")
                    dBug.Print($"ScanCode {New LParamMap(m.LParam).scan}")
                    If FrmMain.cboAlt.SelectedIndex > 0 Then
                        FrmMain.AltPP.Activate()
                        Dim scan As Byte = New LParamMap(m.LParam)
                        If scan = 28 Then
                            If Not FrmMain.AltPP.isSDL Then
                                SendKeys.Send("{ENTER}")
                            Else
                                SendScanKey(scan)
                            End If
                        End If
                    End If
                    'Me.Close()
                    Exit Sub
                Case WM_CHAR
                    dBug.Print($"GhostSettings WM_CHAR {m.WParam} {m.LParam}")
                    dBug.Print($"ScanCode {New LParamMap(m.LParam).scan}")
                    If FrmMain.cboAlt.SelectedIndex > 0 Then
                        FrmMain.AltPP.Activate()
                        If Not FrmMain.AltPP.isSDL Then
                            SendMessage(FrmMain.AltPP.MainWindowHandle, WM_CHAR, m.WParam, IntPtr.Zero)
                        End If
                    End If
                    'Me.Close()
                Case WM_SYSKEYDOWN
                    dBug.Print($"GhostSettings WM_SYSKEY {m.WParam} {m.LParam}")
                    If FrmMain.cboAlt.SelectedIndex > 0 Then
                        FrmMain.AltPP.Activate()
                    End If
                    'Me.Close()
                    Exit Sub
            End Select

        End If
        MyBase.WndProc(m)
    End Sub

    Private restoreWhitelist As Boolean = My.Settings.Whitelist

    'https://docs.microsoft.com/en-us/windows/win32/api/shellapi/ne-shellapi-shstockiconid
    Enum SIID As UInteger
        folder = 3
        folderopen = 4
        shield = 77
    End Enum

    Private Shared Function GetStockIconImage(type As SIID) As Image
        Dim info As New SHSTOCKICONINFO With {.cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(SHSTOCKICONINFO))}

        SHGetStockIconInfo(type, &H101, info)

        Using ico As Icon = Icon.FromHandle(info.hIcon).Clone()
            DestroyIcon(info.hIcon)
            Dim bm As Bitmap = ico.ToBitmap
            DestroyIcon(ico.Handle)
            Return bm
        End Using
    End Function



    'ReadOnly storeZoom As Integer = My.Settings.zoom
    'Private resettingAlign As Boolean = False
    'Private Async Sub ChkDoAlign_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkDoAlign.CheckedChanged
    '    If resettingAlign Then
    '        resettingAlign = False
    '        Exit Sub
    '    End If
    '    If sender.Checked AndAlso FrmMain.cboAlt.SelectedIndex = 0 Then
    '        CustomMessageBox.Show(Me, "To perform alignment an alt needs to be selected.", "ScalA Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        resettingAlign = True
    '        sender.Checked = False
    '        Exit Sub
    '    End If
    '    If FrmMain.WindowState = FormWindowState.Maximized Then
    '        FrmMain.btnMax.PerformClick()
    '    End If
    '    FrmMain.tmrTick.Enabled = Not sender.Checked
    '    Await Task.Delay(200) ' wait for tmrtick to stop running async code
    '    'If FrmMain.cmbResolution.SelectedIndex <> 0 Then
    '    '    FrmMain.cmbResolution.SelectedIndex = If(sender.Checked, 0, storeZoom)
    '    'Else ' SelectedIndex = 0
    '    '    Call FrmMain.CmbResolution_SelectedIndexChanged(FrmMain.cmbResolution, Nothing)
    '    'End If

    '    grpAlign.Enabled = sender.Checked
    '    If sender.Checked Then
    '        FrmMain.suppressResChange = False

    '        dBug.print(rcAstOffsetBase.ToString)

    '        Dim rcClient As Rectangle = FrmMain.AltPP?.ClientRect
    '        'GetClientRect(FrmMain.AltPP.MainWindowHandle, rcClient)
    '        FrmMain.ReZoom(New Drawing.Size(rcClient.Width, rcClient.Height))
    '        FrmMain.cmbResolution.SelectedIndex = 0
    '        FrmMain.cmbResolution.Items(0) = "Aligning"
    '        FrmMain.suppressResChange = False

    '        Dim ptz As Point = FrmMain.pbZoom.PointToScreen(New Point)
    '        'FrmMain.AltPP.CenterBehind(FrmMain.pbZoom)
    '        SetWindowPos(FrmMain.AltPP.MainWindowHandle, FrmMain.ScalaHandle,
    '                            ptz.X - FrmMain.AltPP.ClientOffset.X,
    '                            ptz.Y - FrmMain.AltPP.ClientOffset.Y,
    '                            -1, -1,
    '                            SetWindowPosFlags.IgnoreResize)

    '        GetWindowRect(FrmMain.AltPP.MainWindowHandle, rcAstOffsetBase)
    '        manualNumUpdate = False
    '        numXoffset.Value = 0
    '        numYoffset.Value = 0
    '        manualNumUpdate = True
    '    Else
    '        FrmMain.cmbResolution.Items(0) = $"{My.Settings.resol.Width}x{My.Settings.resol.Height}"
    '        FrmMain.cmbResolution.SelectedIndex = My.Settings.zoom
    '    End If
    '    FrmMain.UpdateThumb(If(sender.Checked, 122, 255))
    '    FrmMain.cmbResolution.Enabled = Not sender.Checked
    '    tmrAlign.Enabled = sender.Checked
    '    chkDoAlign.Enabled = Not sender.Checked
    '    FrmMain.btnMin.Enabled = Not sender.Checked
    '    FrmMain.btnMax.Enabled = Not sender.Checked
    'End Sub

    Private Function ParseResolutions() As Boolean
        Const width = 0
        Const height = 1
        Const fail = False
        Const success = True
        Dim resList As New List(Of Size)
        Try
            dBug.Print("parseRes")
            For Each line As String In txtResolutions.Lines
                dBug.Print($"""{line}""")
                If line = "" Then Continue For
                Dim parts() As String = line.ToUpper.Split("X")
                dBug.Print(parts(width) & " " & parts(height))
                If parts(width) < 400 OrElse parts(height) < 300 Then
                    CustomMessageBox.Show(Me, "Error: " & line & " is too small a resolution.", icon:=MessageBoxIcon.Error)
                    Return fail
                End If
                resList.Add(New Size(parts(width), parts(height)))
            Next
            If resList.Count = 0 Then
                CustomMessageBox.Show(Me, "Error: no resolutions defined.", icon:=MessageBoxIcon.Error)
                Return fail
            End If

            FrmMain.zooms = resList.ToArray
            FrmMain.cmbResolution.Items.Clear()
            FrmMain.cmbResolution.Items.Add($"{FrmMain.pbZoom.Width}x{FrmMain.pbZoom.Height}")
            For Each sz As Size In resList
                FrmMain.cmbResolution.Items.Add(sz.Width & "x" & sz.Height)
            Next

            'frmMain.cmbResolution.SelectedIndex = 0
            Return success
        Catch
            CustomMessageBox.Show(Me, "Error in resolutions")
            Return fail
        End Try
    End Function

    Private Sub BtnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click

        'save settings
        If My.Settings.resolutions <> txtResolutions.Text Then
            If Not ParseResolutions() Then Exit Sub
            FrmMain.cmbResolution.SelectedIndex = 0
        End If

        If My.Settings.links <> txtQuickLaunchPath.Text Then
            If Not FileIO.FileSystem.DirectoryExists(txtQuickLaunchPath.Text) Then
                CustomMessageBox.Show(Me, $"Directory {txtQuickLaunchPath.Text} does not exist!", "Error")
                Exit Sub
            End If
            FrmMain.iconCache.Clear()
            My.Settings.links = txtQuickLaunchPath.Text
            FrmMain.UpdateWatchers(My.Settings.links)
        End If

        My.Settings.resolutions = txtResolutions.Text

        'Me.chkDoAlign.Checked = False

        My.Settings.topmost = chkTopMost.Checked

        FrmMain.TopMost = chkTopMost.Checked
        If FrmMain.cboAlt.SelectedIndex > 0 Then
            FrmMain.AltPP.TopMost = chkTopMost.Checked
        End If

        If My.Settings.gameOnOverview <> chkOverViewIsGame.Checked Then
            My.Settings.gameOnOverview = chkOverViewIsGame.Checked
            AButton.ActiveOverview = My.Settings.gameOnOverview
            If Not My.Settings.gameOnOverview Then
                If FrmMain.pnlOverview.Visible Then
                    Me.Hide()
                    Me.ShowInTaskbar = False
                    FrmMain.Detach(True)
                    FrmMain.AltPP = Nothing
                End If
            End If
        End If

        My.Settings.roundCorners = chkRoundCorners.Checked

        FrmMain.cornerNW.Visible = chkRoundCorners.Checked
        FrmMain.cornerNE.Visible = chkRoundCorners.Checked
        FrmMain.cornerSE.Visible = chkRoundCorners.Checked
        FrmMain.cornerSW.Visible = chkRoundCorners.Checked

        'My.Settings.offset = New Point(numXoffset.Value, numYoffset.Value)
        My.Settings.exe = txtExe.Text
        My.Settings.className = txtClass.Text
        'manualNumUpdate = False

        My.Settings.SwitchToOverview = chkSwitchToOverview.Checked
        My.Settings.StoKey = StoKey

        My.Settings.StoAlt = If(chkStoAlt.Checked, Hotkey.KeyModifier.Alt, 0)
        My.Settings.StoCtrl = If(chkStoCtrl.Checked, Hotkey.KeyModifier.Control, 0)
        My.Settings.StoShift = If(chkStoShift.Checked, Hotkey.KeyModifier.Shift, 0)
        My.Settings.StoWin = If(chkStoWin.Checked, Hotkey.KeyModifier.Winkey, 0)

        dBug.Print($"My.Settings.StoAlt {My.Settings.StoAlt}")
        dBug.Print($"My.Settings.StoCtrl {My.Settings.StoCtrl}")
        dBug.Print($"My.Settings.StoShift {My.Settings.StoShift}")


        My.Settings.CycleAlt = chkCycleAlts.Checked
        My.Settings.CycleKeyFwd = CycleKeyDown
        My.Settings.CycleKeyBwd = CycleKeyUp

        My.Settings.CycleAltKeyFwd = If(chkCycleDownAlt.Checked, Hotkey.KeyModifier.Alt, 0)
        My.Settings.CycleShiftKeyFwd = If(chkCycleDownShift.Checked, Hotkey.KeyModifier.Alt, 0)
        My.Settings.CycleCtrlKeyFwd = If(chkCycleDownCtrl.Checked, Hotkey.KeyModifier.Control, 0)
        My.Settings.CycleWinKeyFwd = If(chkCycleDownWin.Checked, Hotkey.KeyModifier.Winkey, 0)

        My.Settings.CycleAltKeyBwd = If(chkCycleUpAlt.Checked, Hotkey.KeyModifier.Alt, 0)
        My.Settings.CycleShiftKeyBwd = If(chkCycleUpShift.Checked, Hotkey.KeyModifier.Shift, 0)
        My.Settings.CycleCtrlKeyBwd = If(chkCycleUpCtrl.Checked, Hotkey.KeyModifier.Control, 0)
        My.Settings.CycleWinKeyBwd = If(chkCycleUpWin.Checked, Hotkey.KeyModifier.Winkey, 0)

        My.Settings.CycleOnClose = chkCycleOnClose.Checked

        My.Settings.CloseAll = chkCloseAll.Checked
        My.Settings.CloseAllKey = CloseAllKey

        My.Settings.CloseAllAlt = If(chkCAALt.Checked, Hotkey.KeyModifier.Alt, 0)
        My.Settings.CloseAllShift = If(chkCAShift.Checked, Hotkey.KeyModifier.Shift, 0)
        My.Settings.CloseAllCtrl = If(chkCACtrl.Checked, Hotkey.KeyModifier.Control, 0)
        My.Settings.CloseAllWin = If(chkCAWin.Checked, Hotkey.KeyModifier.Winkey, 0)

        My.Settings.ToggleTop = chkToggleTopMost.Checked
        My.Settings.ToggleTopKey = TogTopKey

        My.Settings.TogTopAlt = If(chkTogTopAlt.Checked, Hotkey.KeyModifier.Alt, 0)
        My.Settings.TogTopShift = If(chkTogTopShift.Checked, Hotkey.KeyModifier.Shift, 0)
        My.Settings.TogTopCtrl = If(chkTogTopCtrl.Checked, Hotkey.KeyModifier.Control, 0)
        My.Settings.TogTopWin = If(chkTogTopWin.Checked, Hotkey.KeyModifier.Winkey, 0)


        My.Settings.AlterOverview = chkAlterOverview.Checked

        My.Settings.AlterOverviewMinKey = AlterOvervieKeyMin

        My.Settings.AlterOverviewMinAlt = If(chkAlterOverviewMinAlt.Checked, Hotkey.KeyModifier.Alt, 0)
        My.Settings.AlterOverviewMinShift = If(chkAlterOverviewMinShift.Checked, Hotkey.KeyModifier.Shift, 0)
        My.Settings.AlterOverviewMinCtrl = If(chkAlterOverviewMinCtrl.Checked, Hotkey.KeyModifier.Control, 0)
        My.Settings.AlterOverviewMinWin = If(chkAlterOverviewMinWin.Checked, Hotkey.KeyModifier.Winkey, 0)

        My.Settings.AlterOverviewPlusKey = AlterOvervieKeyPlus

        My.Settings.AlterOverviewPlusAlt = If(chkAlterOverviewPlusALt.Checked, Hotkey.KeyModifier.Alt, 0)
        My.Settings.AlterOverviewPlusShift = If(chkAlterOverviewPlusShift.Checked, Hotkey.KeyModifier.Shift, 0)
        My.Settings.AlterOverviewPlusCtrl = If(chkAlterOverviewPlusCtrl.Checked, Hotkey.KeyModifier.Control, 0)
        My.Settings.AlterOverviewPlusWin = If(chkAlterOverviewPlusWin.Checked, Hotkey.KeyModifier.Winkey, 0)

        My.Settings.AlterOverviewStarKey = AlterOvervieKeyStar

        My.Settings.AlterOverviewStarAlt = If(chkAlterOverviewStarAlt.Checked, Hotkey.KeyModifier.Alt, 0)
        My.Settings.AlterOverviewStarShift = If(chkAlterOverviewStarShift.Checked, Hotkey.KeyModifier.Shift, 0)
        My.Settings.AlterOverviewStarCtrl = If(chkAlterOverviewStarCtrl.Checked, Hotkey.KeyModifier.Control, 0)
        My.Settings.AlterOverviewStarWin = If(chkAlterOverviewStarWin.Checked, Hotkey.KeyModifier.Winkey, 0)

        My.Settings.topSort = txtTopSort.Text
        My.Settings.botSort = txtBotSort.Text

        My.Settings.SingleInstance = ChkSingleInstance.Checked

        My.Settings.MaxBorderTop = NumBorderTop.Value
        My.Settings.MaxBorderBot = NumBorderBot.Value
        My.Settings.MaxBorderLeft = NumBorderLeft.Value
        My.Settings.MaxBorderRight = NumBorderRight.Value

        My.Settings.ExtraMaxColRow = NumExtraMax.Value

        My.Settings.OneLessRowCol = ChkLessRowCol.Checked

        If My.Settings.Theme <> cmbTheme.SelectedIndex + 1 Then
            My.Settings.Theme = cmbTheme.SelectedIndex + 1

            Dim darkmode As Boolean = WinUsingDarkTheme()

            If My.Settings.Theme = 3 Then
                darkmode = True
            End If
            If My.Settings.Theme = 2 Then
                darkmode = False
            End If
            FrmMain.ApplyTheme(darkmode)
        End If


        If cboScalingMode.SelectedIndex <> My.Settings.ScalingMode Then
            My.Settings.ScalingMode = cboScalingMode.SelectedIndex
            If Not FrmMain.pnlOverview.Visible Then
                FrmMain.AltPP?.CenterBehind(FrmMain.pbZoom, SetWindowPosFlags.DoNotActivate, True, True)
                FrmMain.UpdateThumb(If(FrmMain.chkDebug.Checked, 128, 255))
            End If
        End If

        My.Settings.CheckForUpdate = chkCheckForUpdate.Checked

        restoreWhitelist = chkWhitelist.Checked

        Hotkey.UnregHotkey(FrmMain)

        My.Settings.StartMaximized = chkStartupMax.Checked

        My.Settings.SizingBorder = ChkSizeBorder.Checked

        If FrmMain.WindowState <> FormWindowState.Maximized Then
            FrmSizeBorder.Opacity = If(FrmMain.chkDebug.Checked, 1, 0.01)
            FrmSizeBorder.Opacity = If(My.Settings.SizingBorder, FrmSizeBorder.Opacity, 0)
        End If

        My.Settings.MinMin = ChkMinMin.Checked

        My.Settings.QLShowHidden = ChkQLShowHidden.Checked

        My.Settings.HoverActivate = chkHoverActivate.Checked

        My.Settings.ShowEnd = chkShowEnd.Checked

        FrmMain.tmrOverview.Interval = If(My.Settings.gameOnOverview, 33, 66)

        My.Settings.ApplyAlterNormal = chkApplyAlterNormal.Checked

        My.Settings.MaxNormOverview = chkMinMaxOnSwitch.Checked

        My.Settings.DisableWinKey = chkBlockWin.Checked
        My.Settings.NoAltTab = chkNoAltTab.Checked
        My.Settings.OnlyEsc = chkOnlyEsc.Checked

        My.Settings.AllowCtrlShiftEsc = chkAllowShiftEsc.Checked

        If My.Settings.DisableWinKey OrElse My.Settings.OnlyEsc OrElse My.Settings.NoAltTab Then
            keybHook.Hook()
        Else
            keybHook.Unhook()
        End If

        My.Settings.AutoCloseIdle = chkAutoCloseSomeone.Checked
        My.Settings.OnlyAutoCloseOnNoSomeone = chkAutoCloseOnlyOnNoSome.Checked

        My.Settings.Save()

        FrmSizeBorder.Invalidate()

        Hotkey.UnregHotkey(Me)

        Me.Close()
    End Sub
    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub FrmSettings_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        'Me.chkDoAlign.Checked = False
        Me.txtTopSort.Text = My.Settings.topSort
        Me.txtBotSort.Text = My.Settings.botSort
        'btnTest.PerformClick()
        chkWhitelist.Checked = restoreWhitelist
        BtnTest_Click(Nothing, Nothing)
        FrmMain.tmrHotkeys.Start()
    End Sub
    Private Async Sub FrmSettings_Closed(sender As Form, e As FormClosedEventArgs) Handles Me.Closed
        dBug.Print($"frmSettings.Closed {e.CloseReason} ""{sender.Owner}""")
        Await Task.Delay(100) 'hardcoded delay only partially effective. failsafe in wndproc
        FrmMain.Attach(FrmMain.AltPP, True)
    End Sub
    'Dim rcAstOffsetBase As Rectangle
    'Public ScalaMoved As Point
    'Dim rcAstOffsetNew As Rectangle
    'Private Sub TmrAlign_Tick(sender As Object, e As EventArgs) Handles tmrAlign.Tick
    '    manualNumUpdate = False
    '    GetWindowRect(FrmMain.AltPP.MainWindowHandle, rcAstOffsetNew)
    '    numXoffset.Value = My.Settings.offset.X + ScalaMoved.X - rcAstOffsetNew.Left + rcAstOffsetBase.Left
    '    numYoffset.Value = My.Settings.offset.Y + ScalaMoved.Y - rcAstOffsetNew.Top + rcAstOffsetBase.Top
    '    manualNumUpdate = True
    'End Sub

    'Public manualNumUpdate As Boolean = True

    'Private Sub NumXYoffsets_ValueChanged(sender As NumericUpDown, e As EventArgs) Handles numYoffset.ValueChanged, numXoffset.ValueChanged

    '    If manualNumUpdate Then
    '        dBug.print($"ManualNumUpdate")
    '        Dim ptMove As New Point(0, 0)
    '        If sender.Tag Then
    '            ptMove.Y += sender.Text - sender.Value
    '        Else
    '            ptMove.X += sender.Text - sender.Value
    '        End If

    '        SetWindowPos(FrmMain.AltPP.MainWindowHandle, FrmMain.Handle, rcAstOffsetNew.Left + ptMove.X, rcAstOffsetNew.Top + ptMove.Y, -1, -1, SetWindowPosFlags.IgnoreResize + SetWindowPosFlags.DoNotActivate)

    '    End If

    'End Sub

    Private Sub TxtResolutions_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtResolutions.KeyPress
        If Not (Char.IsDigit(e.KeyChar) Or Char.IsControl(e.KeyChar) Or e.KeyChar.ToString.ToLower = "x") Then
            e.Handled = True
        ElseIf e.KeyChar = "X" Then
            e.KeyChar = "x"
        End If
    End Sub
    Private Sub txtResolutions_KeyUp(sender As Object, e As KeyEventArgs) Handles txtResolutions.KeyUp
        Dim currentLineIndex As Integer = Math.Min(txtResolutions.GetLineFromCharIndex(txtResolutions.SelectionStart), txtResolutions.Lines.Length - 1)
        Dim currentLine As String = txtResolutions.Lines(currentLineIndex)

        If Not String.IsNullOrWhiteSpace(currentLine) Then
            SortResolutions()
        End If
    End Sub
    'Private Sub BtnResetAlign_Click(sender As Object, e As EventArgs)
    '    tmrAlign.Stop()
    '    chkDoAlign.Checked = False
    '    My.Settings.offset = New Point(0, 0)
    '    manualNumUpdate = False
    '    numXoffset.Text = 0
    '    numYoffset.Text = 0
    '    manualNumUpdate = True
    'End Sub

    Private Sub BtnOpenFolderDialog_Click(sender As Object, e As EventArgs) Handles btnOpenFolderDialog.Click
        txtQuickLaunchPath.SuspendLayout()
        txtQuickLaunchPath.Text = ChangeLinksDir(My.Settings.links)
        txtQuickLaunchPath.SelectionStart = txtQuickLaunchPath.TextLength
        txtQuickLaunchPath.ResumeLayout()
    End Sub

    Private Function ChangeLinksDir(current As String) As String
        dBug.Print("changeLinksDir")

        Try
            Dim fp As New FolderPicker With {
                .Title = "Select Folder Containing Your Shortcuts - ScalA",
                .Multiselect = False,
                .InputPath = IO.Path.GetFullPath(current)}
            If fp.ShowDialog(Me) = True Then
                If fp.ResultPath = System.IO.Path.GetPathRoot(fp.ResultPath) AndAlso
                        CustomMessageBox.Show(Me, "Warning: Selecting a root path is not recommended" & vbCrLf &
                                        $"Are you sure you want to use {fp.ResultPath}?", "Warning",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.No Then Throw New Exception("dummy")
                Return fp.ResultPath
            End If
        Catch
        End Try
        Return current
    End Function

    Private Sub TxtQuickLaunchPath_DoubleClick(sender As Object, e As EventArgs) Handles txtQuickLaunchPath.DoubleClick
        txtQuickLaunchPath.SelectionStart = 0
        txtQuickLaunchPath.SelectionLength = txtQuickLaunchPath.TextLength
    End Sub

    Private Sub OpenInFileExplorerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenInExplorerToolStripMenuItem.Click
        Dim pp As New Process With {.StartInfo = New ProcessStartInfo With {.FileName = txtQuickLaunchPath.Text}}

        Try
            pp.Start()
        Catch

        End Try
    End Sub

    Private Shared ReadOnly keyNames() As String = {"", "", "", "", "", "", "", "", "{Backspace}", "{Tab}", "", "", "", "{Enter}", "", "", ' 0-15
                                    "", "", "", "{Pause}", "", "", "", "", "", "", "", "{Escape}", "", "", "", "", ' 16-31
                                    "{Space}", "{PageUp}", "{PageDown}", "{End}", "{Home}", "{Left}", "{Up}", "{Right}", "{Down}", "", "", "", "{PrintSrcn}", "{Insert}", "{Delete}", "", ' 32-47
                                    "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "", "", "", "", "", "", '  48-63
                                    "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", '  64-79
                                    "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "", "", "", "", "", '  80-95
                                    "{Num 0}", "{Num 1}", "{Num 2}", "{Num 3}", "{Num 4}", "{Num 5}", "{Num 6}", "{Num 7}", "{Num 8}", "{Num 9}", "{Num *}", "{Num +}", "", "{Num -}", "{Num .}", "{Num /}", '  96-111
                                    "{F1}", "{F2}", "{F3}", "{F4}", "{F5}", "{F6}", "{F7}", "{F8}", "{F9}", "{F10}", "{F11}", "{F12}", "{F13}", "{F14}", "{F15}", "{F16}", ' 112-127
                                    "{F17}", "{F18}", "{F19}", "{F20}", "{F21}", "{F22}", "{F23}", "{F24}", "", "", "", "", "", "", "", "", ' 128-143
                                    "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ' 144-159
                                    "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ' 160-175
                                    "", "", "", "", "", "", "", "", "", "", ";", "=", ",", "-", ".", "/", ' 176-191
                                    "{Tilde}", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ' 192-207
                                    "", "", "", "", "", "", "", "", "", "", "", "[", "\", "]", "²", "", ' 208-223
                                    "", "", "\", "", "", "", "", "", "", "", "", "", "", "", "", "", ' 224-239
                                    "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""} ' 240-255
    Dim StoKey, CycleKeyUp, CycleKeyDown, CloseAllKey, TogTopKey, AlterOvervieKeyPlus, AlterOvervieKeyMin, AlterOvervieKeyStar As Integer


    Private Sub BtnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
        cmsRestore.Show(sender, sender.PointToClient(MousePosition))
    End Sub

    Private Sub BtnGenerate_Click(sender As Button, e As EventArgs) Handles btnGenerate.Click
        cmsGenerate.Show(sender, sender.PointToClient(MousePosition))
    End Sub

    Private Sub GenerateToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles X60043ToolStripMenuItem.Click, X720169ToolStripMenuItem.Click
        Dim sender_tag As String = sender.Tag

        Dim sb As New StringBuilder()

        Dim baseRes As New Size(Val(sender_tag),
                                Val(sender_tag.Substring(sender_tag.IndexOf("x") + 1)))

        dBug.Print($"baseRes {baseRes}")

        Dim gcd = Me.GCD(baseRes.Width, baseRes.Height)
        dBug.Print($"aspect  {baseRes.Width / gcd}:{baseRes.Height / gcd}")

        sb.AppendLine($"{baseRes.Width}x{baseRes.Height}")

        Dim x = baseRes.Width
        Dim y = baseRes.Height
        While x < 4400
            x += baseRes.Width / gcd * 20
            y += baseRes.Height / gcd * 20
            sb.AppendLine($"{x}x{y}")
        End While

        txtResolutions.Text = sb.ToString

    End Sub



    Private Function GCD(p As Integer, q As Integer) As Integer
        If q = 0 Then Return p
        Dim r As Integer = p Mod q
        Return GCD(q, r)
    End Function

    Private Sub FromToolStripMenuItem_DropDownOpening(sender As ToolStripMenuItem, e As EventArgs) Handles FromToolStripMenuItem.DropDownOpening
        sender.DropDownItems.Clear()

        For Each ap As AstoniaProcess In AstoniaProcess.Enumerate().OrderBy(Function(p) p.UserName)

            Dim rcC As Rectangle = ap.ClientRect

            Dim baseRes As New Size(rcC.Width, rcC.Height)
            Dim gcd As Integer = Me.GCD(baseRes.Width, baseRes.Height)
            Dim aspect As String = $"({baseRes.Width / gcd}:{baseRes.Height / gcd})"
            If aspect = "(8:5)" Then aspect = "(16:10)"

            sender.DropDownItems.Add($"{ap.UserName} {baseRes.Width}x{baseRes.Height} {aspect}", ap.GetIcon?.ToBitmap, AddressOf GenerateToolStripMenuItem_Click).Tag = $"{baseRes.Width}x{baseRes.Height}"
        Next
        If sender.DropDownItems.Count = 0 Then sender.DropDownItems.Add("(None)").Enabled = False

    End Sub

    Private Sub SortResolutions()

        Dim firstChar As Integer = txtResolutions.GetFirstCharIndexOfCurrentLine()
        Dim caretPositionInLine As Integer = txtResolutions.SelectionStart - firstChar
        Dim lineIndex As Integer = Math.Min(txtResolutions.GetLineFromCharIndex(firstChar), txtResolutions.Lines.Length - 1)
        Dim currentItem As String = txtResolutions.Lines(lineIndex)

        dBug.Print($"currentitem ""{currentItem}""")

        txtResolutions.Lines = txtResolutions.Lines() _
                            .OrderBy(Function(res) If(String.IsNullOrWhiteSpace(res), Integer.MaxValue, Val(res))) _ 'sort by width
                            .ThenBy(Function(res) Val(res.Split("x").Skip(1).FirstOrDefault)) _        'then by height
                            .Distinct(StringComparer.OrdinalIgnoreCase) _                                            'remove dups
                            .ToArray()

        'find currentitem and set caret to the correct position
        Dim newIndex As Integer = Array.FindIndex(txtResolutions.Lines, Function(line) line.Equals(currentItem, StringComparison.OrdinalIgnoreCase))

        If newIndex >= 0 Then
            txtResolutions.SelectionStart = txtResolutions.GetFirstCharIndexFromLine(newIndex) + caretPositionInLine
            txtResolutions.ScrollToCaret()
        End If
    End Sub

    Private Sub BtnHelp_Click(sender As Object, e As EventArgs) Handles btnHelp.Click
        Dim bl As String = vbTab & """" & String.Join($"""{vbCrLf & vbTab}""", txtTopSort.Lines.Intersect(txtBotSort.Lines).Where(Function(s) s <> "")) & """"
        If bl = vbTab & """""" Then bl = $"{vbTab}(None)"
        CustomMessageBox.Show(Me, $"Names are case sensitive{vbCrLf} and should NOT include tilte (eg. ""Sir"") or suffixes.{vbCrLf}Left list Sorts to top/topleft,{vbCrLf} Right one to bottom/bottomright.{vbCrLf}" &
                        $"If whitelist is enabled ScalA will only show alts in lists{vbCrLf}   except those that are blacklisted{vbCrLf}" &
                        $"Names appearing in both lists are blacklisted.{vbCrLf}{vbCrLf}" &
                        $"Current Blacklist:{vbCrLf}{bl}", "Sorting & Black/Whitelist Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub BtnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click

        dBug.Print("btnTest_Click")

        FrmMain.topSortList = txtTopSort.Lines.Where(Function(s) s <> "").ToList
        FrmMain.botSortList = txtBotSort.Lines.Where(Function(s) s <> "").ToList
        FrmMain.blackList = FrmMain.topSortList.Intersect(FrmMain.botSortList).Where(Function(s) s <> "").ToList
        FrmMain.topSortList = FrmMain.topSortList.Except(FrmMain.blackList).ToList
        FrmMain.botSortList = FrmMain.botSortList.Except(FrmMain.blackList).ToList

        FrmMain.apSorter = New AstoniaProcessSorter(FrmMain.topSortList, FrmMain.botSortList)

        My.Settings.Whitelist = chkWhitelist.Checked

        IPC.AddOrUpdateInstance(FrmMain.scalaPID, FrmMain.cboAlt.SelectedIndex = 0, If(FrmMain.cboAlt.SelectedIndex = 0, Nothing, FrmMain.cboAlt.SelectedItem.id), FrmMain.showingSomeone)

#If DEBUG Then
        dBug.Print("Top:")
        FrmMain.topSortList.ForEach(Sub(el) dBug.Print(el))
        dBug.Print("Bot:")
        FrmMain.botSortList.ForEach(Sub(el) dBug.Print(el))
        dBug.Print("blacklist:")
        FrmMain.blackList.ForEach(Sub(el) dBug.Print(el))
#End If
    End Sub

    Private performingBorderAdjust As Boolean = False
    Private Sub NumBorder_ValueChanged(sender As NumericUpDown, e As EventArgs) _
        Handles NumBorderLeft.ValueChanged, NumBorderRight.ValueChanged, NumBorderTop.ValueChanged, NumBorderBot.ValueChanged
        If performingBorderAdjust Then Exit Sub
        Dim anti As NumericUpDown
        Select Case sender.Name
            Case "NumBorderBot"
                anti = NumBorderTop
            Case "NumBorderLeft"
                anti = NumBorderRight
            Case "NumBorderRight"
                anti = NumBorderLeft
            Case Else
                anti = NumBorderBot
        End Select

        performingBorderAdjust = True
        While anti.Value + sender.Value > 750
            anti.Value -= 1
        End While
        performingBorderAdjust = False
    End Sub

    Private Sub btnGrabCurrent_Click(sender As Object, e As EventArgs) Handles btnGrabCurrent.Click
        Dim bounds = FrmMain.Bounds
        Dim workarea = Screen.FromControl(FrmMain).WorkingArea
        dBug.Print($"b {bounds} wa {workarea}")
        NumBorderTop.Value = Math.Max(0, (bounds.Top - workarea.Top) * 1000 / workarea.Height)
        NumBorderLeft.Value = Math.Max(0, (bounds.Left - workarea.Left) * 1000 / workarea.Width)
        NumBorderRight.Value = Math.Max(0, (workarea.Right - bounds.Right) * 1000 / workarea.Width)
        NumBorderBot.Value = Math.Max(0, (workarea.Bottom - bounds.Bottom) * 1000 / workarea.Height)
    End Sub

    Private Sub btnAddCurrentRes_Click(sender As Object, e As EventArgs) Handles btnAddCurrentRes.Click
        Dim res As String = FrmMain.cmbResolution.SelectedItem
        dBug.Print(res)
        If txtResolutions.Lines.Contains(res) Then
            dBug.Print("already present")
            txtResolutions.SelectionStart = txtResolutions.Text.IndexOf(res)
            txtResolutions.SelectionLength = res.Length
            txtResolutions.ScrollToCaret()
            txtResolutions.Focus()
            Exit Sub
        End If
        Dim resos As List(Of String) = txtResolutions.Lines.ToList
        Dim idx As Integer = 0
        For Each line In resos
            If Val(line) < Val(res) Then
                idx += 1
                Continue For
            End If
            If Val(line) = Val(res) AndAlso Val(line.Split("x")(1)) < Val(res.Split("x")(1)) Then
                idx += 1
                Continue For
            End If
            resos.Insert(idx, res)
            Exit For
        Next
        txtResolutions.Lines = resos.ToArray
        txtResolutions.SelectionStart = Math.Max(0, txtResolutions.GetFirstCharIndexFromLine(idx))
        txtResolutions.SelectionLength = res.Length
        txtResolutions.ScrollToCaret()
        txtResolutions.Focus()
    End Sub

    Private Sub chkSwitchToOverview_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkSwitchToOverview.CheckedChanged
        grpOverviewShortcut.Enabled = sender.Checked
    End Sub

    Private Sub chkCycleAlts_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkCycleAlts.CheckedChanged
        grpCycleShortcut.Enabled = sender.Checked
    End Sub

    Private Sub chkCloseAll_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkCloseAll.CheckedChanged
        grpCloseAllShortcut.Enabled = sender.Checked
    End Sub
    Private Sub chkToggleTopMost_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkToggleTopMost.CheckedChanged
        grpToggleTopMost.Enabled = sender.Checked
    End Sub


    Private Sub DefaultToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DefaultToolStripMenuItem.Click
        txtResolutions.Text = My.Settings.PropertyValues("resolutions").Property.DefaultValue
    End Sub

    Private Sub LastSavedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LastSavedToolStripMenuItem.Click
        txtResolutions.Text = My.Settings.resolutions
    End Sub

    Private Sub TxtShortcuts_PreviewKeyDown(sender As TextBox, e As PreviewKeyDownEventArgs) _
        Handles txtStoKey.PreviewKeyDown, txtCycleKeyUp.PreviewKeyDown, txtCycleKeyDown.PreviewKeyDown, txtCloseAll.PreviewKeyDown, txtTogTop.PreviewKeyDown,
                txtAlterOverviewMinKey.PreviewKeyDown, txtAlterOverviewPlusKey.PreviewKeyDown, txtAlterOverviewStarKey.PreviewKeyDown
        dBug.Print(e.KeyCode)
        If e.KeyCode = 16 OrElse 'shift
           e.KeyCode = 17 OrElse 'ctrl
           e.KeyCode = 18 OrElse 'alt
           e.KeyCode = 91 OrElse 'lwin
           e.KeyCode = 92 OrElse 'rwin
           e.KeyCode > 255 OrElse
           keyNames(e.KeyCode) = "" Then
            Exit Sub
        End If
        sender.Text = keyNames(e.KeyCode)
        Select Case sender.Name
            Case txtStoKey.Name
                StoKey = e.KeyCode
            Case txtCycleKeyDown.Name
                CycleKeyDown = e.KeyCode
            Case txtCycleKeyUp.Name
                CycleKeyUp = e.KeyCode
            Case txtCloseAll.Name
                CloseAllKey = e.KeyCode
            Case txtTogTop.Name
                TogTopKey = e.KeyCode
            Case txtAlterOverviewMinKey.Name
                AlterOvervieKeyMin = e.KeyCode
            Case txtAlterOverviewPlusKey.Name
                AlterOvervieKeyPlus = e.KeyCode
            Case txtAlterOverviewStarKey.Name
                AlterOvervieKeyStar = e.KeyCode

        End Select
        dBug.Print($"key: {e.KeyCode}")
    End Sub

    Private Async Sub CheckNowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CheckNowToolStripMenuItem.Click
        Try
            Using response As HttpResponseMessage = Await FrmMain.client.GetAsync("https://github.com/smoorke/ScalA/releases/download/ScalA/version")
                response.EnsureSuccessStatusCode()
                Dim responseBody As String = Await response.Content.ReadAsStringAsync()

                FrmMain.updateToVersion = responseBody
                If New Version(responseBody) > My.Application.Info.Version Then
                    FrmMain.pbUpdateAvailable_Click(FrmMain.pbUpdateAvailable, New MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0))
                Else
                    CustomMessageBox.Show(Me, $"ScalA v{responseBody} is the current version online", "No Update Available", icon:=MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            FrmMain.pnlUpdate.Visible = False
            FrmMain.updateToVersion = "Error"
            CustomMessageBox.Show(Me, "ScalA is unable to check for updates.", "Error", icon:=MessageBoxIcon.Error)
        End Try
    End Sub

    Private Async Sub OpenChangelogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenChangelogToolStripMenuItem.Click
        Await FrmMain.LogDownload(Me)
        Try
            Process.Start(New ProcessStartInfo(FileIO.SpecialDirectories.Temp & "\ScalA\ChangeLog.txt"))
        Catch
        End Try
    End Sub

    Private Sub chkAlterOverview_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkAlterOverview.CheckedChanged
        grpAlterOverview.Enabled = sender.Checked
    End Sub

    Private Sub ResetIconCacheToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResetIconCacheToolStripMenuItem.Click
        FrmMain.iconCache.Clear()
        FrmMain.DefURLicons.Clear()
    End Sub

    Private Sub btnGoToAdjustHotkey_Click(sender As Object, e As EventArgs) Handles btnGoToAdjustHotkey.Click
        tbcSettings.SelectedTab = tabHotkeys
        tabHotkeys.ScrollControlIntoView(grpAlterOverview)
    End Sub

    Private Sub chkBlockWin_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkBlockWin.CheckedChanged
        chkCAWin.Enabled = Not sender.Checked
        chkCycleDownWin.Enabled = Not sender.Checked
        chkCycleUpWin.Enabled = Not sender.Checked
        chkStoWin.Enabled = Not sender.Checked
        chkAlterOverviewMinWin.Enabled = Not sender.Checked
        chkAlterOverviewPlusWin.Enabled = Not sender.Checked
        chkAlterOverviewStarWin.Enabled = Not sender.Checked
    End Sub

    'Private Sub chkOverViewIsGame_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkOverViewIsGame.CheckedChanged
    '    chkHoverActivate.Enabled = sender.Checked
    'End Sub


    Private Sub TextVarious_KeyPress(sender As TextBox, e As KeyPressEventArgs) Handles _
            txtStoKey.KeyPress, txtCycleKeyUp.KeyPress, txtCycleKeyDown.KeyPress, txtCloseAll.KeyPress, txtTogTop.KeyPress,
            txtAlterOverviewMinKey.KeyPress, txtAlterOverviewPlusKey.KeyPress, txtAlterOverviewStarKey.KeyPress
        tabHotkeys.ScrollControlIntoView(sender)
        e.Handled = True
    End Sub

    Private Sub chkAutoCloseSomeone_CheckedChanged(sender As Object, e As EventArgs) Handles chkAutoCloseSomeone.CheckedChanged
        chkAutoCloseOnlyOnNoSome.Enabled = sender.checked
    End Sub

    Private init_validate As Boolean = True

    Private Sub chkOnlyEsc_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkOnlyEsc.CheckedChanged
        chkAllowShiftEsc.Enabled = sender.Checked
    End Sub

    Private Sub txtResolutions_Leave(sender As Object, e As EventArgs) Handles txtResolutions.Leave
        SortResolutions()
    End Sub

    Private Sub tabResolutions_Click(sender As Object, e As EventArgs) Handles tabResolutions.Click
        SortResolutions()
    End Sub

    Private Sub FrmSettings_Click(sender As Object, e As EventArgs) Handles MyBase.Click
        SortResolutions()
    End Sub

    Private Sub lblElevated_Click(sender As Object, e As EventArgs) Handles lblElevated.Click, pbUnElevate.Click
        btnOK.PerformClick()
        FrmMain.UnelevateSelf()
    End Sub

    Private Sub validate_hotkey(sender As Object, e As EventArgs) Handles _
                    chkSwitchToOverview.CheckedChanged, chkCycleAlts.CheckedChanged, chkCloseAll.CheckedChanged, chkToggleTopMost.CheckedChanged, chkAlterOverview.CheckedChanged,
                    chkStoAlt.CheckedChanged, chkStoCtrl.CheckedChanged, chkStoShift.CheckedChanged, txtStoKey.KeyUp, chkStoWin.CheckedChanged,
                    chkCycleUpAlt.CheckedChanged, chkCycleUpCtrl.CheckedChanged, chkCycleUpShift.CheckedChanged, txtCycleKeyUp.KeyUp, chkCycleUpWin.CheckedChanged,
                    chkCycleDownAlt.CheckedChanged, chkCycleDownCtrl.CheckedChanged, chkCycleDownShift.CheckedChanged, txtCycleKeyDown.KeyUp, chkCycleDownWin.CheckedChanged,
                    chkCAALt.CheckedChanged, chkCACtrl.CheckedChanged, chkCAShift.CheckedChanged, txtCloseAll.KeyUp, chkCAWin.CheckedChanged,
                    chkTogTopAlt.CheckedChanged, chkTogTopCtrl.CheckedChanged, chkTogTopShift.CheckedChanged, txtTogTop.KeyUp, chkTogTopWin.CheckedChanged,
                    chkAlterOverviewMinAlt.CheckedChanged, chkAlterOverviewMinCtrl.CheckedChanged, chkAlterOverviewMinShift.CheckedChanged, txtAlterOverviewMinKey.KeyUp, chkAlterOverviewMinWin.CheckedChanged,
                    chkAlterOverviewPlusALt.CheckedChanged, chkAlterOverviewPlusCtrl.CheckedChanged, chkAlterOverviewPlusShift.CheckedChanged, txtAlterOverviewPlusKey.KeyUp, chkAlterOverviewPlusWin.CheckedChanged,
                    chkAlterOverviewStarAlt.CheckedChanged, chkAlterOverviewStarCtrl.CheckedChanged, chkAlterOverviewStarShift.CheckedChanged, txtAlterOverviewStarKey.KeyUp, chkAlterOverviewStarWin.CheckedChanged,
                    chkBlockWin.CheckedChanged

        If sender Is Nothing Then init_validate = False
        If init_validate Then Exit Sub

        dBug.Print($"Validate {sender} {e?.GetType}")


        Dim modi As Hotkey.KeyModifier
        Hotkey.UnregHotkey(FrmMain)
        Hotkey.UnregHotkey(Me)

        If chkSwitchToOverview.Checked Then
            modi = Hotkey.KeyModifier.None
            If chkStoAlt.Checked Then modi = modi Or Hotkey.KeyModifier.Alt
            If chkStoCtrl.Checked Then modi = modi Or Hotkey.KeyModifier.Control
            If chkStoShift.Checked Then modi = modi Or Hotkey.KeyModifier.Shift
            If chkStoWin.Checked Then modi = modi Or If(chkBlockWin.Checked, 0, Hotkey.KeyModifier.Winkey)
            If Hotkey.RegisterHotkey(Me, 1, modi, StoKey) Then
                txtStoKey.ForeColor = Color.Black
            Else
                dBug.Print("Invalid hotkey")
                txtStoKey.ForeColor = Color.Red
            End If
        End If

        If chkCycleAlts.Checked Then
            modi = Hotkey.KeyModifier.None
            If chkCycleDownAlt.Checked Then modi = modi Or Hotkey.KeyModifier.Alt
            If chkCycleDownCtrl.Checked Then modi = modi Or Hotkey.KeyModifier.Control
            If chkCycleDownShift.Checked Then modi = modi Or Hotkey.KeyModifier.Shift
            If chkCycleDownWin.Checked Then modi = modi Or If(chkBlockWin.Checked, 0, Hotkey.KeyModifier.Winkey)
            If Hotkey.RegisterHotkey(Me, 3, modi, CycleKeyDown) Then
                txtCycleKeyDown.ForeColor = Color.Black
            Else
                txtCycleKeyDown.ForeColor = Color.Red
            End If

            modi = Hotkey.KeyModifier.None
            If chkCycleUpAlt.Checked Then modi = modi Or Hotkey.KeyModifier.Alt
            If chkCycleUpCtrl.Checked Then modi = modi Or Hotkey.KeyModifier.Control
            If chkCycleUpShift.Checked Then modi = modi Or Hotkey.KeyModifier.Shift
            If chkCycleUpWin.Checked Then modi = modi Or If(chkBlockWin.Checked, 0, Hotkey.KeyModifier.Winkey)
            If Hotkey.RegisterHotkey(Me, 2, modi, CycleKeyUp) Then
                txtCycleKeyUp.ForeColor = Color.Black
            Else
                txtCycleKeyUp.ForeColor = Color.Red
            End If
        End If

        If chkCloseAll.Checked Then
            modi = Hotkey.KeyModifier.None
            If chkCAALt.Checked Then modi = modi Or Hotkey.KeyModifier.Alt
            If chkCACtrl.Checked Then modi = modi Or Hotkey.KeyModifier.Control
            If chkCAShift.Checked Then modi = modi Or Hotkey.KeyModifier.Shift
            If chkCAWin.Checked Then modi = modi Or If(chkBlockWin.Checked, 0, Hotkey.KeyModifier.Winkey)
            If Hotkey.RegisterHotkey(Me, 4, modi, CloseAllKey) Then
                txtCloseAll.ForeColor = Color.Black
            Else
                txtCloseAll.ForeColor = Color.Red
            End If
        End If

        If chkToggleTopMost.Checked Then
            modi = Hotkey.KeyModifier.None
            If chkTogTopAlt.Checked Then modi = modi Or Hotkey.KeyModifier.Alt
            If chkTogTopCtrl.Checked Then modi = modi Or Hotkey.KeyModifier.Control
            If chkTogTopShift.Checked Then modi = modi Or Hotkey.KeyModifier.Shift
            If chkTogTopWin.Checked Then modi = modi Or Hotkey.KeyModifier.Winkey
            If Hotkey.RegisterHotkey(Me, 5, modi, TogTopKey) Then
                txtTogTop.ForeColor = Color.Black
            Else
                txtTogTop.ForeColor = Color.Red
            End If
        End If

        If chkAlterOverview.Checked Then
            modi = Hotkey.KeyModifier.None
            If chkAlterOverviewPlusALt.Checked Then modi = modi Or Hotkey.KeyModifier.Alt
            If chkAlterOverviewPlusCtrl.Checked Then modi = modi Or Hotkey.KeyModifier.Control
            If chkAlterOverviewPlusShift.Checked Then modi = modi Or Hotkey.KeyModifier.Shift
            If chkAlterOverviewPlusWin.Checked Then modi = modi Or If(chkBlockWin.Checked, 0, Hotkey.KeyModifier.Winkey)
            If Hotkey.RegisterHotkey(Me, 6, modi, AlterOvervieKeyPlus) Then
                txtAlterOverviewPlusKey.ForeColor = Color.Black
            Else
                txtAlterOverviewPlusKey.ForeColor = Color.Red
            End If

            modi = Hotkey.KeyModifier.None
            If chkAlterOverviewMinAlt.Checked Then modi = modi Or Hotkey.KeyModifier.Alt
            If chkAlterOverviewMinCtrl.Checked Then modi = modi Or Hotkey.KeyModifier.Control
            If chkAlterOverviewMinShift.Checked Then modi = modi Or Hotkey.KeyModifier.Shift
            If chkAlterOverviewMinWin.Checked Then modi = modi Or If(chkBlockWin.Checked, 0, Hotkey.KeyModifier.Winkey)
            If Hotkey.RegisterHotkey(Me, 7, modi, AlterOvervieKeyMin) Then
                txtAlterOverviewMinKey.ForeColor = Color.Black
            Else
                txtAlterOverviewMinKey.ForeColor = Color.Red
            End If

            modi = Hotkey.KeyModifier.None
            If chkAlterOverviewStarAlt.Checked Then modi = modi Or Hotkey.KeyModifier.Alt
            If chkAlterOverviewStarCtrl.Checked Then modi = modi Or Hotkey.KeyModifier.Control
            If chkAlterOverviewStarShift.Checked Then modi = modi Or Hotkey.KeyModifier.Shift
            If chkAlterOverviewStarWin.Checked Then modi = modi Or If(chkBlockWin.Checked, 0, Hotkey.KeyModifier.Winkey)
            If Hotkey.RegisterHotkey(Me, 8, modi, AlterOvervieKeyStar) Then
                txtAlterOverviewStarKey.ForeColor = Color.Black
            Else
                txtAlterOverviewStarKey.ForeColor = Color.Red
            End If
        End If

        Hotkey.UnregHotkey(Me)
    End Sub

    Private Sub tbcSettings_SelectedIndexChanged(sender As TabControl, e As EventArgs) Handles tbcSettings.SelectedIndexChanged
        My.Settings.remeberSettingsTab = sender.SelectedIndex
    End Sub

    Private Sub chkApplyAlterNormal_CheckedChanged(sender As Object, e As EventArgs) Handles chkMinMaxOnSwitch.CheckedChanged
        If sender.checked Then
            chkStartupMax.Checked = False
        End If
    End Sub

    Private Sub chkStartupMax_CheckedChanged(sender As Object, e As EventArgs) Handles chkStartupMax.CheckedChanged
        If sender.checked Then
            chkMinMaxOnSwitch.Checked = False
        End If
    End Sub

    Private Sub txtHotkeys_GotFocus(sender As TextBox, e As EventArgs) Handles txtStoKey.GotFocus, txtCycleKeyUp.GotFocus, txtCycleKeyDown.GotFocus,
        txtCloseAll.GotFocus, txtTogTop.GotFocus, txtAlterOverviewMinKey.GotFocus, txtAlterOverviewPlusKey.GotFocus, txtAlterOverviewStarKey.GotFocus

        sender.BackColor = Color.LightYellow
    End Sub
    Private Sub txtHotkeys_LostFocus(sender As Object, e As EventArgs) Handles txtStoKey.LostFocus, txtCycleKeyUp.LostFocus, txtCycleKeyDown.LostFocus,
        txtCloseAll.LostFocus, txtTogTop.LostFocus, txtAlterOverviewMinKey.LostFocus, txtAlterOverviewPlusKey.LostFocus, txtAlterOverviewStarKey.LostFocus
        sender.backColor = Color.White
    End Sub

    Private Sub FrmSettings_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If Not Me.startup Then
            FrmMain.CloseOtherDropDowns(FrmMain.cmsQuickLaunch.Items, Nothing)
            FrmMain.cmsQuickLaunch.Close()
        End If
    End Sub


End Class