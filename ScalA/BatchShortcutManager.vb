Imports System.IO

Public Class BatchShortcutManager

    Private ReadOnly _initialPath As String

    ' Resolution preset structure
    Private Structure ResolutionPreset
        Public Name As String
        Public W As Integer
        Public H As Integer

        Public Sub New(name As String, w As Integer, h As Integer)
            Me.Name = name
            Me.W = w
            Me.H = h
        End Sub
    End Structure

    ' Resolution presets
    Private ReadOnly ResolutionPresets As ResolutionPreset() = {
        New ResolutionPreset("(Any)", 0, 0),
        New ResolutionPreset("800 x 600", 800, 600),
        New ResolutionPreset("1024 x 768", 1024, 768),
        New ResolutionPreset("1280 x 720", 1280, 720),
        New ResolutionPreset("1280 x 1024", 1280, 1024),
        New ResolutionPreset("1600 x 900", 1600, 900),
        New ResolutionPreset("1920 x 1080", 1920, 1080),
        New ResolutionPreset("Custom...", -1, -1)
    }
    Dim DesignedClientSize As Size
    Public Sub New(Optional initialPath As String = "")
        InitializeComponent()
        _initialPath = initialPath
        DesignedClientSize = Me.ClientSize
    End Sub

    Private Sub BatchShortcutManager_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set initial path
        If Not String.IsNullOrEmpty(_initialPath) AndAlso Directory.Exists(_initialPath) Then
            txtFolderPath.Text = _initialPath
        End If

        ' Populate resolution dropdowns
        For Each preset In ResolutionPresets
            cboResolutionOld.Items.Add(preset.Name)
            cboResolutionNew.Items.Add(preset.Name)
        Next
        cboResolutionOld.SelectedIndex = 0
        cboResolutionNew.SelectedIndex = 0

        ' fix scaling issue
        Dim rcC As RECT
        GetClientRect(Me.Handle, rcC)

        Me.Size = New Size(Me.Width - rcC.right + DesignedClientSize.Width,
                           Me.Height - rcC.bottom + DesignedClientSize.Height)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case WM_WINDOWPOSCHANGING
                Dim winpos As WINDOWPOS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                If StructureToPtrSupported Then
                    If Not winpos.flags.HasFlag(SetWindowPosFlags.IgnoreResize) Then
                        Dim rcC As RECT
                        GetClientRect(Me.Handle, rcC)
                        winpos.cx = Me.Width - rcC.right + DesignedClientSize.Width
                        winpos.cy = Me.Height - rcC.bottom + DesignedClientSize.Height
                        System.Runtime.InteropServices.Marshal.StructureToPtr(winpos, m.LParam, True)
                    End If
                End If
        End Select

        MyBase.WndProc(m)
    End Sub


    Private Sub ChkPassword_CheckedChanged(sender As Object, e As EventArgs) Handles chkPassword.CheckedChanged
        Dim enabled = chkPassword.Checked
        txtPasswordOld.Enabled = enabled
        txtPasswordNew.Enabled = enabled
        lblPasswordOld.Enabled = enabled
        lblPasswordNew.Enabled = enabled
        lblPasswordArrow.Enabled = enabled
    End Sub

    Private Sub ChkOption_CheckedChanged(sender As Object, e As EventArgs) Handles chkOption.CheckedChanged
        Dim enabled = chkOption.Checked
        txtOptionOld.Enabled = enabled
        txtOptionNew.Enabled = enabled
        lblOptionOld.Enabled = enabled
        lblOptionNew.Enabled = enabled
        lblOptionArrow.Enabled = enabled
        lblOptionHint.Enabled = enabled
    End Sub

    Private Sub ChkResolution_CheckedChanged(sender As Object, e As EventArgs) Handles chkResolution.CheckedChanged
        Dim enabled = chkResolution.Checked
        cboResolutionOld.Enabled = enabled
        cboResolutionNew.Enabled = enabled
        lblResOld.Enabled = enabled
        lblResNew.Enabled = enabled
        lblResArrow.Enabled = enabled
        UpdateCustomResolutionVisibility()
    End Sub

    Private Sub CboResolutionOld_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboResolutionOld.SelectedIndexChanged
        UpdateCustomResolutionVisibility()
    End Sub

    Private Sub CboResolutionNew_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboResolutionNew.SelectedIndexChanged
        UpdateCustomResolutionVisibility()
    End Sub

    Private Sub UpdateCustomResolutionVisibility()
        Dim oldIsCustom = cboResolutionOld.SelectedIndex = ResolutionPresets.Length - 1 ' "Custom..."
        Dim newIsCustom = cboResolutionNew.SelectedIndex = ResolutionPresets.Length - 1

        lblOldCustom.Visible = oldIsCustom AndAlso chkResolution.Checked
        numOldWidth.Visible = oldIsCustom AndAlso chkResolution.Checked
        numOldHeight.Visible = oldIsCustom AndAlso chkResolution.Checked
        numOldWidth.Enabled = chkResolution.Checked
        numOldHeight.Enabled = chkResolution.Checked

        lblNewCustom.Visible = newIsCustom AndAlso chkResolution.Checked
        numNewWidth.Visible = newIsCustom AndAlso chkResolution.Checked
        numNewHeight.Visible = newIsCustom AndAlso chkResolution.Checked
        numNewWidth.Enabled = chkResolution.Checked
        numNewHeight.Enabled = chkResolution.Checked
    End Sub

    Private Sub BtnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim fp As New FolderPicker With {
            .Title = "Select Folder Containing Shortcuts",
            .Multiselect = False,
            .InputPath = If(Directory.Exists(txtFolderPath.Text), txtFolderPath.Text, Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
        }
        If fp.ShowDialog(Me) = True Then
            txtFolderPath.Text = fp.ResultPath
        End If
    End Sub

    Private Sub BtnPreview_Click(sender As Object, e As EventArgs) Handles btnPreview.Click
        If Not ValidateInputs() Then Exit Sub

        Dim totalShortcuts As Integer = 0
        Dim matchCount As Integer = 0
        ScanAndCountMatches(totalShortcuts, matchCount)

        If totalShortcuts = 0 Then
            lblStatus.Text = "No shortcuts found"
            CustomMessageBox.Show(Me, "No Astonia shortcuts were found in the selected folder.", "Preview", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            lblStatus.Text = $"{matchCount}/{totalShortcuts} match"
            Dim msg = $"Found {totalShortcuts} Astonia shortcut(s) in folder.{vbCrLf}{vbCrLf}"
            If matchCount > 0 Then
                msg &= $"{matchCount} shortcut(s) will be modified."
            Else
                msg &= "No shortcuts match the specified criteria."
            End If
            CustomMessageBox.Show(Me, msg, "Preview", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub BtnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        If Not ValidateInputs() Then Exit Sub

        Dim totalShortcuts As Integer = 0
        Dim matchCount As Integer = 0
        ScanAndCountMatches(totalShortcuts, matchCount)

        If matchCount = 0 Then
            CustomMessageBox.Show(Me, "No shortcuts match the specified criteria.", "Apply", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Confirm
        Dim confirm = CustomMessageBox.Show(Me,
            $"About to modify {matchCount} shortcut(s).{vbCrLf}{vbCrLf}This cannot be undone. Continue?",
            "Confirm Changes",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning)

        If confirm <> DialogResult.Yes Then Exit Sub

        ' Apply changes
        Dim successCount As Integer = 0
        Dim failCount As Integer = 0
        ApplyChanges(successCount, failCount)
        lblStatus.Text = $"{successCount} updated"

        Dim msg = $"Successfully modified {successCount} shortcut(s)."
        If failCount > 0 Then
            msg &= $"{vbCrLf}{failCount} shortcut(s) failed to update."
        End If
        CustomMessageBox.Show(Me, msg, "Complete", MessageBoxButtons.OK,
            If(failCount > 0, MessageBoxIcon.Warning, MessageBoxIcon.Information))
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Function ValidateInputs() As Boolean
        If String.IsNullOrWhiteSpace(txtFolderPath.Text) Then
            CustomMessageBox.Show(Me, "Please select a target folder.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If Not Directory.Exists(txtFolderPath.Text) Then
            CustomMessageBox.Show(Me, "The selected folder does not exist.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If Not chkPassword.Checked AndAlso Not chkOption.Checked AndAlso Not chkResolution.Checked Then
            CustomMessageBox.Show(Me, "Please enable at least one change type.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        ' Validate new values are provided
        If chkPassword.Checked AndAlso String.IsNullOrEmpty(txtPasswordNew.Text) Then
            CustomMessageBox.Show(Me, "Please enter a new password value.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If chkOption.Checked AndAlso String.IsNullOrEmpty(txtOptionNew.Text) Then
            CustomMessageBox.Show(Me, "Please enter a new -o option value.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If chkResolution.Checked Then
            Dim newIdx = cboResolutionNew.SelectedIndex
            If newIdx = 0 Then ' "(Any)" is not valid for new
                CustomMessageBox.Show(Me, "Please select a specific resolution for the new value.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If
            If newIdx = ResolutionPresets.Length - 1 Then ' Custom
                If numNewWidth.Value = 0 OrElse numNewHeight.Value = 0 Then
                    CustomMessageBox.Show(Me, "Please enter custom resolution dimensions.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                End If
            End If
        End If

        Return True
    End Function

    Private Iterator Function ScanShortcuts(rootPath As String) As IEnumerable(Of String)
        Dim stack As New Stack(Of String)
        stack.Push(rootPath)

        While stack.Count > 0
            Dim dir = stack.Pop()

            ' Get subdirectories if recursive mode is enabled, skip reparse points
            If chkRecursive.Checked Then
                Try
                    For Each subDir In Directory.EnumerateDirectories(dir)
                        Try
                            Dim attrs = File.GetAttributes(subDir)
                            If (attrs And FileAttributes.ReparsePoint) = 0 Then
                                stack.Push(subDir)
                            End If
                        Catch
                            ' Skip inaccessible directories
                        End Try
                    Next
                Catch
                    ' Skip if can't enumerate
                End Try
            End If

            ' Yield .lnk files
            Try
                For Each lnk In Directory.EnumerateFiles(dir, "*.lnk")
                    Yield lnk
                Next
            Catch
                ' Skip if can't enumerate
            End Try
        End While
    End Function

    Private Function IsAstoniaShortcut(sli As ShellLinkInfo) As Boolean
        ' Check if target matches known Astonia exe names
        If String.IsNullOrEmpty(sli.TargetPath) Then Return False

        Dim targetName = IO.Path.GetFileNameWithoutExtension(sli.TargetPath)
        Dim exeList = My.Settings.exe.Split("|"c).Where(Function(s) Not String.IsNullOrEmpty(s)).Select(Function(s) s.Trim.ToLower)

        Return exeList.Contains(targetName.ToLower)
    End Function

    Private Function ParseArguments(args As String) As Dictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        If String.IsNullOrEmpty(args) Then Return result

        Dim parts = args.Split("-"c).Skip(1)
        For Each part In parts
            If part.Length > 0 Then
                Dim key = part.Substring(0, 1).ToLower()
                Dim val = part.Substring(1).Trim()
                If Not result.ContainsKey(key) Then
                    result.Add(key, val)
                End If
            End If
        Next
        Return result
    End Function

    Private Function RebuildArguments(dict As Dictionary(Of String, String)) As String
        Return String.Join(" ", dict.Select(Function(kvp) $"-{kvp.Key}{kvp.Value}"))
    End Function

    Private Sub GetOldResolution(ByRef w As Integer, ByRef h As Integer)
        Dim idx = cboResolutionOld.SelectedIndex
        If idx = ResolutionPresets.Length - 1 Then ' Custom
            w = CInt(numOldWidth.Value)
            h = CInt(numOldHeight.Value)
        Else
            w = ResolutionPresets(idx).W
            h = ResolutionPresets(idx).H
        End If
    End Sub

    Private Sub GetNewResolution(ByRef w As Integer, ByRef h As Integer)
        Dim idx = cboResolutionNew.SelectedIndex
        If idx = ResolutionPresets.Length - 1 Then ' Custom
            w = CInt(numNewWidth.Value)
            h = CInt(numNewHeight.Value)
        Else
            w = ResolutionPresets(idx).W
            h = ResolutionPresets(idx).H
        End If
    End Sub

    Private Function ShortcutMatches(sli As ShellLinkInfo) As Boolean
        Dim argDict = ParseArguments(sli.Arguments)
        Dim matches = True

        ' Check password match
        If chkPassword.Checked AndAlso Not String.IsNullOrEmpty(txtPasswordOld.Text) Then
            Dim currentPwd = If(argDict.ContainsKey("p"), argDict("p"), "")
            If currentPwd <> txtPasswordOld.Text Then matches = False
        End If

        ' Check -o option match
        If chkOption.Checked AndAlso Not String.IsNullOrEmpty(txtOptionOld.Text) Then
            Dim currentOpt = If(argDict.ContainsKey("o"), argDict("o"), "")
            If currentOpt <> txtOptionOld.Text Then matches = False
        End If

        ' Check resolution match
        If chkResolution.Checked Then
            Dim oldW As Integer = 0, oldH As Integer = 0
            GetOldResolution(oldW, oldH)
            If oldW > 0 AndAlso oldH > 0 Then ' Not "(Any)"
                Dim currentW = If(argDict.ContainsKey("w"), argDict("w"), "")
                Dim currentH = If(argDict.ContainsKey("h"), argDict("h"), "")
                If currentW <> oldW.ToString() OrElse currentH <> oldH.ToString() Then
                    matches = False
                End If
            End If
        End If

        Return matches
    End Function

    Private Sub ScanAndCountMatches(ByRef totalShortcuts As Integer, ByRef matchCount As Integer)
        totalShortcuts = 0
        matchCount = 0

        For Each lnkPath In ScanShortcuts(txtFolderPath.Text)
            Dim sli As New ShellLinkInfo()
            If Not sli.Load(lnkPath) Then Continue For
            If Not IsAstoniaShortcut(sli) Then Continue For

            totalShortcuts += 1
            If ShortcutMatches(sli) Then matchCount += 1
        Next
    End Sub

    Private Sub ApplyChanges(ByRef successCount As Integer, ByRef failCount As Integer)
        successCount = 0
        failCount = 0

        For Each lnkPath In ScanShortcuts(txtFolderPath.Text)
            Dim sli As New ShellLinkInfo()
            If Not sli.Load(lnkPath) Then Continue For
            If Not IsAstoniaShortcut(sli) Then Continue For
            If Not ShortcutMatches(sli) Then Continue For

            Try
                Dim argDict = ParseArguments(sli.Arguments)
                Dim modified = False

                ' Apply password change
                If chkPassword.Checked Then
                    argDict("p") = txtPasswordNew.Text
                    modified = True
                End If

                ' Apply -o option change
                If chkOption.Checked Then
                    argDict("o") = txtOptionNew.Text
                    modified = True
                End If

                ' Apply resolution change
                If chkResolution.Checked Then
                    Dim newW As Integer = 0, newH As Integer = 0
                    GetNewResolution(newW, newH)
                    argDict("w") = newW.ToString()
                    argDict("h") = newH.ToString()
                    modified = True
                End If

                If modified Then
                    sli.Arguments = RebuildArguments(argDict)
                    If sli.Save(lnkPath) Then
                        successCount += 1
                    Else
                        failCount += 1
                    End If
                End If
            Catch
                failCount += 1
            End Try
        Next
    End Sub

End Class
