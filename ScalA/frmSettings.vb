Imports System.Text

Public NotInheritable Class FrmSettings
    Public SysMenu As New SysMenu(Me)
    Private Sub FrmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'storeZoom = My.Settings.zoom

        Me.Owner = FrmMain
        If FrmMain.WindowState <> FormWindowState.Minimized Then Me.CenterToParent()

        If Me.Tag IsNot Nothing Then
            For Each TabPage As TabPage In tbcSettings.TabPages
                If TabPage.Text = Me.Tag Then
                    tbcSettings.SelectedTab = TabPage
                    Me.Tag = Nothing
                    Exit For
                End If
            Next
        End If

        Me.TopMost = My.Settings.topmost
        chkTopMost.Checked = My.Settings.topmost
        chkRoundCorners.Checked = My.Settings.roundCorners
        chkOverViewIsGame.Checked = My.Settings.gameOnOverview

        'chkAspect.Checked = My.Settings.lockAspect
        'cmbAnchor.SelectedIndex = My.Settings.anchor

        numXoffset.Value = My.Settings.offset.X
        numYoffset.Value = My.Settings.offset.Y

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
        Debug.Print($"My.Settings.StoAlt {My.Settings.StoAlt}")
        chkStoCtrl.Checked = My.Settings.StoCtrl = Hotkey.KeyModifier.Control
        Debug.Print($"My.Settings.StoCtrl {My.Settings.StoCtrl}")
        chkStoShift.Checked = My.Settings.StoShift = Hotkey.KeyModifier.Shift
        Debug.Print($"My.Settings.StoShift {My.Settings.StoShift}")

        chkCycleDownAlt.Checked = My.Settings.CycleAltKeyFwd = Hotkey.KeyModifier.Alt
        chkCycleDownCtrl.Checked = My.Settings.CycleCtrlKeyFwd = Hotkey.KeyModifier.Control
        chkCycleDownShift.Checked = My.Settings.CycleShiftKeyFwd = Hotkey.KeyModifier.Shift

        chkCycleUpAlt.Checked = My.Settings.CycleAltKeyBwd = Hotkey.KeyModifier.Alt
        chkCycleUpCtrl.Checked = My.Settings.CycleCtrlKeyBwd = Hotkey.KeyModifier.Control
        chkCycleUpShift.Checked = My.Settings.CycleShiftKeyBwd = Hotkey.KeyModifier.Shift

        txtStoKey.Text = keyNames(My.Settings.StoKey)

        txtCycleKeyUp.Text = keyNames(My.Settings.CycleKeyBwd)
        txtCycleKeyDown.Text = keyNames(My.Settings.CycleKeyFwd)

        StoKey = My.Settings.StoKey
        CycleKeyDown = My.Settings.CycleKeyFwd
        CycleKeyUp = My.Settings.CycleKeyBwd

        chkCycleOnClose.Checked = My.Settings.CycleOnClose

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

        ChkDark.Checked = My.Settings.DarkMode

        cboScalingMode.SelectedIndex = My.Settings.ScalingMode

        chkCheckForUpdate.Checked = My.Settings.CheckForUpdate

        chkStartupMax.Checked = My.Settings.StartMaximized

        ChkSizeBorder.Checked = My.Settings.SizingBorder

        ChkMinMin.Checked = My.Settings.MinMin

    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case WM_ENTERMENULOOP
                SysMenu.Visible = True
            Case WM_EXITMENULOOP
                SysMenu.Visible = False
        End Select
        MyBase.WndProc(m)
    End Sub

    Private ReadOnly restoreWhitelist As Boolean = My.Settings.Whitelist

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



    ReadOnly storeZoom As Integer = My.Settings.zoom
    Private Async Sub ChkDoAlign_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkDoAlign.CheckedChanged
        If sender.Checked AndAlso FrmMain.AltPP.Id = 0 Then
            MessageBox.Show(FrmMain, "To perform alignment an alt needs to be selected.", "ScalA Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            sender.Checked = False
            Exit Sub
        End If
        If FrmMain.WindowState = FormWindowState.Maximized Then
            FrmMain.btnMax.PerformClick()
        End If
        FrmMain.tmrTick.Enabled = Not sender.Checked
        Await Task.Delay(200) ' wait for tmrtick to stop running async code
        'If FrmMain.cmbResolution.SelectedIndex <> 0 Then
        '    FrmMain.cmbResolution.SelectedIndex = If(sender.Checked, 0, storeZoom)
        'Else ' SelectedIndex = 0
        '    Call FrmMain.CmbResolution_SelectedIndexChanged(FrmMain.cmbResolution, Nothing)
        'End If

        grpAlign.Enabled = sender.Checked
        If sender.Checked Then
            FrmMain.suppressResChange = False

            Debug.Print(rcAstOffsetBase.ToString)

            Dim rcClient As Rectangle = FrmMain.AltPP?.ClientRect
            'GetClientRect(FrmMain.AltPP.MainWindowHandle, rcClient)
            FrmMain.ReZoom(New Drawing.Size(rcClient.Width, rcClient.Height))
            FrmMain.cmbResolution.SelectedIndex = 0
            FrmMain.cmbResolution.Items(0) = "Aligning"
            FrmMain.suppressResChange = False

            Dim ptz As Point = FrmMain.pbZoom.PointToScreen(New Point)
            'FrmMain.AltPP.CenterBehind(FrmMain.pbZoom)
            SetWindowPos(FrmMain.AltPP.MainWindowHandle, FrmMain.ScalaHandle,
                                ptz.X - FrmMain.AltPP.ClientOffset.X,
                                ptz.Y - FrmMain.AltPP.ClientOffset.Y,
                                -1, -1,
                                SetWindowPosFlags.IgnoreResize)

            GetWindowRect(FrmMain.AltPP.MainWindowHandle, rcAstOffsetBase)
            manualNumUpdate = False
            numXoffset.Value = 0
            numYoffset.Value = 0
            manualNumUpdate = True
        Else
            FrmMain.cmbResolution.Items(0) = $"{My.Settings.resol.Width}x{My.Settings.resol.Height}"
            FrmMain.cmbResolution.SelectedIndex = My.Settings.zoom
        End If
        FrmMain.UpdateThumb(If(sender.Checked, 122, 255))
        FrmMain.cmbResolution.Enabled = Not sender.Checked
        tmrAlign.Enabled = sender.Checked
        chkDoAlign.Enabled = Not sender.Checked
        FrmMain.btnMin.Enabled = Not sender.Checked
        FrmMain.btnMax.Enabled = Not sender.Checked
    End Sub



    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.chkDoAlign.Checked = False
        Me.txtTopSort.Text = My.Settings.topSort
        Me.txtBotSort.Text = My.Settings.botSort
        'btnTest.PerformClick()
        BtnTest_Click(Nothing, Nothing)
        My.Settings.Whitelist = restoreWhitelist
        Me.Close()
    End Sub

    Private Function ParseResolutions() As Boolean
        Const width = 0
        Const height = 1
        Const fail = False
        Const success = True
        Dim resList As New List(Of Size)
        Try
            Debug.Print("parseRes")
            For Each line As String In txtResolutions.Lines
                Debug.Print($"""{line}""")
                If line = "" Then Continue For
                Dim parts() As String = line.ToUpper.Split("X")
                Debug.Print(parts(width) & " " & parts(height))
                If parts(width) < 400 OrElse parts(height) < 300 Then
                    MessageBox.Show("Error: " & line & " is too small a resolution.")
                    Return fail
                End If
                resList.Add(New Size(parts(width), parts(height)))
            Next
            If resList.Count = 0 Then
                MessageBox.Show("Error: no resolutions defined.")
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
            MessageBox.Show("Error in resolutions")
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
                MessageBox.Show($"Directory {txtQuickLaunchPath.Text} does not exist!", "Error")
                Exit Sub
            End If
        End If

        My.Settings.links = txtQuickLaunchPath.Text
        FrmMain.UpdateWatchers(My.Settings.links)

        My.Settings.resolutions = txtResolutions.Text

        Me.chkDoAlign.Checked = False

        My.Settings.topmost = chkTopMost.Checked

        FrmMain.TopMost = chkTopMost.Checked
        If FrmMain.cboAlt.SelectedIndex > 0 Then
            FrmMain.AltPP.TopMost = chkTopMost.Checked
        End If

        My.Settings.gameOnOverview = chkOverViewIsGame.Checked

        My.Settings.roundCorners = chkRoundCorners.Checked

        FrmMain.cornerNW.Visible = chkRoundCorners.Checked
        FrmMain.cornerNE.Visible = chkRoundCorners.Checked
        FrmMain.cornerSE.Visible = chkRoundCorners.Checked
        FrmMain.cornerSW.Visible = chkRoundCorners.Checked

        My.Settings.offset = New Point(numXoffset.Value, numYoffset.Value)
        My.Settings.exe = txtExe.Text
        My.Settings.className = txtClass.Text
        manualNumUpdate = False

        My.Settings.SwitchToOverview = chkSwitchToOverview.Checked
        My.Settings.StoKey = StoKey

        My.Settings.StoAlt = If(chkStoAlt.Checked, Hotkey.KeyModifier.Alt, 0)
        My.Settings.StoCtrl = If(chkStoCtrl.Checked, Hotkey.KeyModifier.Control, 0)
        My.Settings.StoShift = If(chkStoShift.Checked, Hotkey.KeyModifier.Shift, 0)

        Debug.Print($"My.Settings.StoAlt {My.Settings.StoAlt}")
        Debug.Print($"My.Settings.StoCtrl {My.Settings.StoCtrl}")
        Debug.Print($"My.Settings.StoShift {My.Settings.StoShift}")


        My.Settings.CycleAlt = chkCycleAlts.Checked
        My.Settings.CycleKeyFwd = CycleKeyDown
        My.Settings.CycleKeyBwd = CycleKeyUp

        My.Settings.CycleAltKeyFwd = If(chkCycleDownAlt.Checked, Hotkey.KeyModifier.Alt, 0)
        My.Settings.CycleShiftKeyFwd = If(chkCycleDownShift.Checked, Hotkey.KeyModifier.Shift, 0)
        My.Settings.CycleCtrlKeyFwd = If(chkCycleDownCtrl.Checked, Hotkey.KeyModifier.Control, 0)

        My.Settings.CycleAltKeyBwd = If(chkCycleUpAlt.Checked, Hotkey.KeyModifier.Alt, 0)
        My.Settings.CycleShiftKeyBwd = If(chkCycleUpShift.Checked, Hotkey.KeyModifier.Shift, 0)
        My.Settings.CycleCtrlKeyBwd = If(chkCycleUpCtrl.Checked, Hotkey.KeyModifier.Control, 0)

        My.Settings.CycleOnClose = chkCycleOnClose.Checked

        My.Settings.topSort = txtTopSort.Text
        My.Settings.botSort = txtBotSort.Text

        My.Settings.SingleInstance = ChkSingleInstance.Checked

        My.Settings.MaxBorderTop = NumBorderTop.Value
        My.Settings.MaxBorderBot = NumBorderBot.Value
        My.Settings.MaxBorderLeft = NumBorderLeft.Value
        My.Settings.MaxBorderRight = NumBorderRight.Value

        My.Settings.ExtraMaxColRow = NumExtraMax.Value

        My.Settings.OneLessRowCol = ChkLessRowCol.Checked

        If ChkDark.Checked <> My.Settings.DarkMode Then
            My.Settings.DarkMode = ChkDark.Checked
            FrmMain.ApplyTheme()
        End If

        If cboScalingMode.SelectedIndex <> My.Settings.ScalingMode Then
            My.Settings.ScalingMode = cboScalingMode.SelectedIndex
            If Not FrmMain.pnlOverview.Visible Then
                FrmMain.UpdateThumb(If(FrmMain.chkDebug.Checked, 128, 255))
            End If
        End If

        My.Settings.CheckForUpdate = chkCheckForUpdate.Checked

        BtnTest_Click(Nothing, Nothing) 'apply sorting & black/whitlelist, note: .PerformClick() doesn't work as button may not be visible

        Hotkey.UnregHotkey(FrmMain)

        My.Settings.StartMaximized = chkStartupMax.Checked

        My.Settings.SizingBorder = ChkSizeBorder.Checked

        FrmSizeBorder.Opacity = If(FrmMain.chkDebug.Checked, 1, 0.01)
        FrmSizeBorder.Opacity = If(My.Settings.SizingBorder, FrmSizeBorder.Opacity, 0)

        My.Settings.MinMin = ChkMinMin.Checked

        FrmMain.tmrOverview.Interval = If(My.Settings.gameOnOverview, 33, 66)

        My.Settings.Save()

        FrmSizeBorder.Invalidate()

        Me.Close()
    End Sub

    Dim rcAstOffsetBase As Rectangle
    Public ScalaMoved As Point
    Dim rcAstOffsetNew As Rectangle
    Private Sub TmrAlign_Tick(sender As Object, e As EventArgs) Handles tmrAlign.Tick
        manualNumUpdate = False
        GetWindowRect(FrmMain.AltPP.MainWindowHandle, rcAstOffsetNew)
        numXoffset.Value = My.Settings.offset.X + ScalaMoved.X - rcAstOffsetNew.Left + rcAstOffsetBase.Left
        numYoffset.Value = My.Settings.offset.Y + ScalaMoved.Y - rcAstOffsetNew.Top + rcAstOffsetBase.Top
        manualNumUpdate = True
    End Sub

    Public manualNumUpdate As Boolean = True

    Private Sub NumXYoffsets_ValueChanged(sender As NumericUpDown, e As EventArgs) Handles numYoffset.ValueChanged, numXoffset.ValueChanged

        If manualNumUpdate Then
            Debug.Print($"ManualNumUpdate")
            Dim ptMove As New Point(0, 0)
            If sender.Tag Then
                ptMove.Y += sender.Text - sender.Value
            Else
                ptMove.X += sender.Text - sender.Value
            End If

            SetWindowPos(FrmMain.AltPP.MainWindowHandle, FrmMain.Handle, rcAstOffsetNew.Left + ptMove.X, rcAstOffsetNew.Top + ptMove.Y, -1, -1, SetWindowPosFlags.IgnoreResize + SetWindowPosFlags.DoNotActivate)

        End If

    End Sub

    Private Sub TxtResolutions_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtResolutions.KeyPress
        If Not (Char.IsDigit(e.KeyChar) Or Char.IsControl(e.KeyChar) Or e.KeyChar.ToString.ToLower = "x") Then
            e.Handled = True
        ElseIf e.KeyChar = "X" Then
            e.KeyChar = "x"
        End If
    End Sub

    Private Sub BtnResetAlign_Click(sender As Object, e As EventArgs) Handles btnResetAlign.Click
        tmrAlign.Stop()
        chkDoAlign.Checked = False
        My.Settings.offset = New Point(0, 0)
        manualNumUpdate = False
        numXoffset.Text = 0
        numYoffset.Text = 0
        manualNumUpdate = True
    End Sub

    Private Sub BtnOpenFolderDialog_Click(sender As Object, e As EventArgs) Handles btnOpenFolderDialog.Click
        txtQuickLaunchPath.SuspendLayout()
        txtQuickLaunchPath.Text = ChangeLinksDir(My.Settings.links)
        txtQuickLaunchPath.SelectionStart = txtQuickLaunchPath.TextLength
        txtQuickLaunchPath.ResumeLayout()
    End Sub

    Private Function ChangeLinksDir(current As String) As String
        Debug.Print("changeLinksDir")
        Me.TopMost = False

        Try
            Dim fp As New FolderPicker With {
                .Title = "Select Folder Containing Your Shortcuts - ScalA",
                .Multiselect = False,
                .InputPath = IO.Path.GetFullPath(current)}
            If fp.ShowDialog(Me) = True Then
                If fp.ResultPath = System.IO.Path.GetPathRoot(fp.ResultPath) AndAlso
                        MessageBox.Show("Warning: Selecting a root path is not recommended" & vbCrLf &
                                        $"Are you sure you want to use {fp.ResultPath}?", "Warning",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.No Then Throw New Exception("dummy")
                Return fp.ResultPath
            End If
        Catch
        Finally
            Me.TopMost = My.Settings.topmost
        End Try
        Return current

#If 0 Then
        'Using fb As New FolderBrowserDialog
        Try
            Using fb As New Ookii.Dialogs.WinForms.VistaFolderBrowserDialog
                fb.Description = "Select Folder Containing Your Shortcuts - ScalA"
                fb.UseDescriptionForTitle = True
                fb.ShowNewFolderButton = True
                fb.RootFolder = Environment.SpecialFolder.Desktop
                fb.SelectedPath = current

                If fb.ShowDialog() = DialogResult.OK Then
                    ' Warning for Root folder with throw for dialog cancel
                    If fb.SelectedPath = System.IO.Path.GetPathRoot(fb.SelectedPath) AndAlso
                        MessageBox.Show("Warning: Selecting a root path is not recommended" & vbCrLf &
                                        $"Are you sure you want to use {fb.SelectedPath}?", "Warning",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.No Then Throw New Exception("dummy")
                    'If My.Settings.links <> fb.SelectedPath Then
                    '    My.Settings.links = fb.SelectedPath
                    '    FrmMain.UpdateWatchers(My.Settings.links)
                    'Else
                    Return fb.SelectedPath
                    'End If
                End If
            End Using
        Catch
        Finally
            Me.TopMost = My.Settings.topmost
        End Try
        Return current
#End If
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
    Dim StoKey, CycleKeyUp, CycleKeyDown As Integer


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

        Debug.Print($"baseRes {baseRes}")

        Dim gcd = Me.GCD(baseRes.Width, baseRes.Height)
        Debug.Print($"aspect  {baseRes.Width / gcd}:{baseRes.Height / gcd}")

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

        For Each ap As AstoniaProcess In AstoniaProcess.Enumerate().OrderBy(Function(p) p.Name)

            Dim rcC As Rectangle = ap.ClientRect

            Dim baseRes As New Size(rcC.Width, rcC.Height)
            Dim gcd As Integer = Me.GCD(baseRes.Width, baseRes.Height)
            Dim aspect As String = $"({baseRes.Width / gcd}:{baseRes.Height / gcd})"
            If aspect = "(8:5)" Then aspect = "(16:10)"

            sender.DropDownItems.Add($"{ap.Name} {baseRes.Width}x{baseRes.Height} {aspect}", ap.GetIcon?.ToBitmap, AddressOf GenerateToolStripMenuItem_Click).Tag = $"{baseRes.Width}x{baseRes.Height}"
        Next
        If sender.DropDownItems.Count = 0 Then sender.DropDownItems.Add("(None)").Enabled = False

    End Sub

    Private Sub BtnSort_Click(sender As Object, e As EventArgs) Handles btnSort.Click
        Dim sb As New StringBuilder
        For Each line In txtResolutions.Text.Split(vbCrLf.ToCharArray, StringSplitOptions.RemoveEmptyEntries).OrderBy(Function(res) Val(res))
            sb.AppendLine(line)
        Next
        sb.Remove(sb.Length - 2, 2)
        txtResolutions.Text = sb.ToString
    End Sub

    Private Sub BtnHelp_Click(sender As Object, e As EventArgs) Handles btnHelp.Click
        Dim bl As String = vbTab & """" & String.Join($"""{vbCrLf & vbTab}""", txtTopSort.Lines.Intersect(txtBotSort.Lines).Where(Function(s) s <> "")) & """"
        If bl = vbTab & """""" Then bl = $"{vbTab}(None)"
        MessageBox.Show($"Names are case sensitive.{vbCrLf}Left list Sorts to top, Right one to bottom.{vbCrLf}" &
                        $"If whitelist is enabled ScalA will only show alts in lists{vbCrLf}   except those that are blacklisted{vbCrLf}" &
                        $"Names appearing in both lists are blacklisted.{vbCrLf}{vbCrLf}" &
                        $"Current Blacklist:{vbCrLf}{bl}", "Sorting & Black/Whitelist Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub BtnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click

        Debug.Print("btnTest_Click")

        FrmMain.topSortList = txtTopSort.Lines.Where(Function(s) s <> "").ToList
        FrmMain.botSortList = txtBotSort.Lines.Where(Function(s) s <> "").ToList
        FrmMain.blackList = FrmMain.topSortList.Intersect(FrmMain.botSortList).Where(Function(s) s <> "").ToList
        FrmMain.topSortList = FrmMain.topSortList.Except(FrmMain.blackList).ToList
        FrmMain.botSortList = FrmMain.botSortList.Except(FrmMain.blackList).ToList

        FrmMain.apSorter = New AstoniaProcessSorter(FrmMain.topSortList, FrmMain.botSortList)

        My.Settings.Whitelist = chkWhitelist.Checked

#If DEBUG Then
        Debug.Print("Top:")
        FrmMain.topSortList.ForEach(Sub(el) Debug.Print(el))
        Debug.Print("Bot:")
        FrmMain.botSortList.ForEach(Sub(el) Debug.Print(el))
        Debug.Print("blacklist:")
        FrmMain.blackList.ForEach(Sub(el) Debug.Print(el))
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
        Debug.Print($"b {bounds} wa {workarea}")
        NumBorderTop.Value = Math.Max(0, (bounds.Top - workarea.Top) * 1000 / workarea.Height)
        NumBorderLeft.Value = Math.Max(0, (bounds.Left - workarea.Left) * 1000 / workarea.Width)
        NumBorderRight.Value = Math.Max(0, (workarea.Right - bounds.Right) * 1000 / workarea.Width)
        NumBorderBot.Value = Math.Max(0, (workarea.Bottom - bounds.Bottom) * 1000 / workarea.Height)
    End Sub

    Private Sub btnAddCurrentRes_Click(sender As Object, e As EventArgs) Handles btnAddCurrentRes.Click
        Dim res As String = FrmMain.cmbResolution.Items(FrmMain.cmbResolution.SelectedIndex)
        Debug.Print(res)
        If txtResolutions.Lines.Contains(res) Then
            Debug.Print("already present")
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

    Private Sub DefaultToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DefaultToolStripMenuItem.Click
        txtResolutions.Text = My.Settings.PropertyValues("resolutions").Property.DefaultValue
    End Sub

    Private Sub LastSavedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LastSavedToolStripMenuItem.Click
        txtResolutions.Text = My.Settings.resolutions
    End Sub

    Private Sub TxtShortcuts_PreviewKeyDown(sender As TextBox, e As PreviewKeyDownEventArgs) Handles txtStoKey.PreviewKeyDown, txtCycleKeyUp.PreviewKeyDown, txtCycleKeyDown.PreviewKeyDown
        Debug.Print(e.KeyCode)
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
        End Select
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtStoKey.KeyPress, txtCycleKeyUp.KeyPress, txtCycleKeyDown.KeyPress
        e.Handled = True
    End Sub

End Class