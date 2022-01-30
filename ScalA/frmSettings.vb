﻿Imports System.Text

Public Class FrmSettings
    Private Sub FrmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'storeZoom = My.Settings.zoom

        Me.Owner = FrmMain
        Me.CenterToParent()

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

    End Sub
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
    Private Sub ChkDoAlign_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkDoAlign.CheckedChanged
        If sender.Checked AndAlso FrmMain.AltPP.Id = 0 Then
            MessageBox.Show(FrmMain, "To perform alignment an alt needs to be selected.", "ScalA Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            sender.Checked = False
            Exit Sub
        End If
        grpAlign.Enabled = sender.Checked
        FrmMain.tmrTick.Enabled = Not sender.Checked
        FrmMain.cmbResolution.SelectedIndex = If(sender.Checked, 0, storeZoom)
        FrmMain.UpdateThumb(If(sender.Checked, 122, 255))

        If sender.Checked Then
            SetWindowPos(FrmMain.AltPP.MainWindowHandle, FrmMain.Handle, FrmMain.newX, FrmMain.newY, -1, -1, SetWindowPosFlags.IgnoreResize + SetWindowPosFlags.DoNotActivate)
            GetWindowRect(FrmMain.AltPP.MainWindowHandle, rcAstOffsetBase)
            Debug.Print(rcAstOffsetBase.ToString)
        End If
        tmrAlign.Enabled = sender.Checked
        chkDoAlign.Enabled = Not sender.Checked
    End Sub



    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.chkDoAlign.Checked = False
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

        Hotkey.UnregHotkey(FrmMain)

        Me.Close()
    End Sub

    Dim rcAstOffsetBase As Rectangle
    Public ScalaMoved As Point
    Dim rcAstOffsetNew As Rectangle
    Private Sub TmrAlign_Tick(sender As Object, e As EventArgs) Handles tmrAlign.Tick
        GetWindowRect(FrmMain.AltPP.MainWindowHandle, rcAstOffsetNew)
        numXoffset.Value = My.Settings.offset.X + ScalaMoved.X - rcAstOffsetNew.Left + rcAstOffsetBase.Left
        numYoffset.Value = My.Settings.offset.Y + ScalaMoved.Y - rcAstOffsetNew.Top + rcAstOffsetBase.Top
        manualNumUpdate = True
    End Sub

    Public manualNumUpdate As Boolean = True

    Private Sub NumXYoffsets_ValueChanged(sender As NumericUpDown, e As EventArgs) Handles numYoffset.ValueChanged, numXoffset.ValueChanged

        If manualNumUpdate Then
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
        chkDoAlign.Checked = False
        My.Settings.offset = New Point(0, 0)
        numXoffset.Text = 0
        numYoffset.Text = 0
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        txtQuickLaunchPath.SuspendLayout()
        txtQuickLaunchPath.Text = ChangeLinksDir(My.Settings.links)
        txtQuickLaunchPath.SelectionStart = txtQuickLaunchPath.TextLength
        txtQuickLaunchPath.ResumeLayout()
    End Sub

    Private Function ChangeLinksDir(current As String) As String
        Debug.Print("changeLinksDir")
        Me.TopMost = False


        'Using fb As New FolderBrowserDialog
        Try
            Using fb As New Ookii.Dialogs.WinForms.VistaFolderBrowserDialog
                fb.Description = "Select Folder Containing Your Shortcuts - ScalA"
                fb.UseDescriptionForTitle = True
                fb.ShowNewFolderButton = False
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
    End Function

    Private Sub txtQuickLaunchPath_DoubleClick(sender As Object, e As EventArgs) Handles txtQuickLaunchPath.DoubleClick
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

    Dim keyNames() As String = {"", "", "", "", "", "", "", "", "{Backspace}", "{Tab}", "", "", "", "{Enter}", "", "", ' 0-15
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

    Private Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
        txtResolutions.Text = My.Settings.resolutions
    End Sub

    Private Sub btnGenerate_Click(sender As Button, e As EventArgs) Handles btnGenerate.Click
        cmsGenerate.Show(sender, sender.PointToClient(MousePosition))
    End Sub

    Private Sub GenerateToolStripMenuItem_Click(sender As ToolStripMenuItem, e As EventArgs) Handles X60043ToolStripMenuItem.Click, X720169ToolStripMenuItem.Click
        Dim sender_tag As String = sender.Tag

        Dim sb As StringBuilder = New StringBuilder()

        Dim baseRes As Size = New Size(Val(sender_tag),
                                       Val(sender_tag.Substring(sender_tag.IndexOf("x") + 1)))

        Debug.Print($"baseRes {baseRes}")

        Dim gcd = Me.GCD(baseRes.Width, baseRes.Height)
        Debug.Print($"aspect  {baseRes.Width / gcd}:{baseRes.Height / gcd}")

        sb.AppendLine($"{baseRes.Width}x{baseRes.Height}")

        Dim x = baseRes.Width
        Dim y = baseRes.Height
        While x < 4400
            x += baseRes.Width / gcd * 25
            y += baseRes.Height / gcd * 25
            sb.AppendLine($"{x}x{y}")
        End While

        txtResolutions.Text = sb.ToString

    End Sub



    Private Function GCD(p, q) As Integer
        If q = 0 Then Return p
        Dim r As Integer = p Mod q
        Return GCD(q, r)
    End Function

    Private Sub FromToolStripMenuItem_DropDownOpening(sender As ToolStripMenuItem, e As EventArgs) Handles FromToolStripMenuItem.DropDownOpening
        sender.DropDownItems.Clear()

        For Each ap As AstoniaProcess In AstoniaProcess.Enumerate(True)

            Dim rcC As Rectangle
            GetClientRect(ap.MainWindowHandle, rcC)

            Dim baseRes As Size = New Size(rcC.Width, rcC.Height)
            Dim gcd As Integer = Me.GCD(baseRes.Width, baseRes.Height)
            Dim aspect As String = $"({baseRes.Width / gcd}:{baseRes.Height / gcd})"
            If aspect = "(8:4)" Then aspect = "(16:10)"

            sender.DropDownItems.Add($"{ap.Name} {baseRes.Width}x{baseRes.Height} {aspect}", ap.GetIcon?.ToBitmap, AddressOf GenerateToolStripMenuItem_Click).Tag = $"{baseRes.Width}x{baseRes.Height}"
        Next
        If sender.DropDownItems.Count = 0 Then sender.DropDownItems.Add("(None)").Enabled = False

    End Sub

    Private Sub btnSort_Click(sender As Object, e As EventArgs) Handles btnSort.Click
        Dim sb As New StringBuilder
        For Each line In txtResolutions.Text.Split(vbCrLf.ToCharArray, StringSplitOptions.RemoveEmptyEntries).OrderBy(Function(res) Val(res))
            sb.AppendLine(line)
        Next
        sb.Remove(sb.Length - 1, 1)
        txtResolutions.Text = sb.ToString
    End Sub

    Private Sub txtShortcuts_PreviewKeyDown(sender As TextBox, e As PreviewKeyDownEventArgs) Handles txtStoKey.PreviewKeyDown, txtCycleKeyUp.PreviewKeyDown, txtCycleKeyDown.PreviewKeyDown
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