Imports System.IO

''' <summary>
''' UserControl for managing launcher templates and creating character shortcuts
''' </summary>
Public Class LauncherSetupControl

    ''' <summary>
    ''' The folder where shortcuts will be created (QL root folder)
    ''' </summary>
    Public Property TargetFolder As String = ""

    ''' <summary>
    ''' Event raised when a shortcut is successfully created
    ''' </summary>
    Public Event ShortcutCreated(sender As Object, shortcutPath As String)

    Private Sub LauncherSetupControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RefreshTemplateList()
        UpdateUIState()

        ' Setup Bulk Add tab
        PopulateSourceShortcuts()
        PopulateTargetFolders()
        SetupUsernamesGrid()

        ' Setup Edit tab
        SetupEditGrid()
        PopulateEditFolders()
    End Sub

#Region "Template List Management"

    ''' <summary>
    ''' Refreshes the template list and dropdown
    ''' </summary>
    Public Sub RefreshTemplateList()
        lstTemplates.Items.Clear()
        cboTemplate.Items.Clear()

        Dim templates = LauncherTemplateManager.GetTemplates()
        For Each t In templates
            lstTemplates.Items.Add(t)
            cboTemplate.Items.Add(t)
        Next

        If cboTemplate.Items.Count > 0 Then
            cboTemplate.SelectedIndex = 0
        End If

        UpdateUIState()
    End Sub

    Private Sub UpdateUIState()
        Dim hasTemplates = lstTemplates.Items.Count > 0
        Dim hasSelection = lstTemplates.SelectedIndex >= 0

        btnEditTemplate.Enabled = hasSelection
        btnDeleteTemplate.Enabled = hasSelection

        grpCreateShortcut.Enabled = hasTemplates
    End Sub

    Private Sub lstTemplates_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstTemplates.SelectedIndexChanged
        UpdateUIState()

        ' Sync selection with dropdown
        If lstTemplates.SelectedItem IsNot Nothing Then
            Dim template = DirectCast(lstTemplates.SelectedItem, LauncherTemplate)
            For i = 0 To cboTemplate.Items.Count - 1
                Dim cboItem = DirectCast(cboTemplate.Items(i), LauncherTemplate)
                If cboItem.Name = template.Name Then
                    cboTemplate.SelectedIndex = i
                    Exit For
                End If
            Next
        End If
    End Sub

#End Region

#Region "Drag & Drop"

    Private Sub pnlDropZone_DragEnter(sender As Object, e As DragEventArgs) Handles pnlDropZone.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files = CType(e.Data.GetData(DataFormats.FileDrop), String())
            If files.Any(Function(f) f.ToLower().EndsWith(".exe")) Then
                e.Effect = DragDropEffects.Copy
                pnlDropZone.BackColor = Color.FromArgb(200, 230, 200)
                Return
            End If
        End If
        e.Effect = DragDropEffects.None
    End Sub

    Private Sub pnlDropZone_DragLeave(sender As Object, e As EventArgs) Handles pnlDropZone.DragLeave
        pnlDropZone.BackColor = SystemColors.ControlLight
    End Sub

    Private Sub pnlDropZone_DragDrop(sender As Object, e As DragEventArgs) Handles pnlDropZone.DragDrop
        pnlDropZone.BackColor = SystemColors.ControlLight

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files = CType(e.Data.GetData(DataFormats.FileDrop), String())
            For Each filePath In files
                If filePath.ToLower().EndsWith(".exe") AndAlso File.Exists(filePath) Then
                    AddNewTemplate(filePath)
                End If
            Next
        End If
    End Sub

    Private Sub AddNewTemplate(exePath As String)
        ' Create a new template from the exe
        Dim template As New LauncherTemplate(exePath)

        ' Check for duplicate name
        Dim baseName = template.Name
        Dim counter = 1
        While LauncherTemplateManager.TemplateExists(template.Name)
            template.Name = $"{baseName} ({counter})"
            counter += 1
        End While

        ' Show edit dialog for user to customize
        Using editForm As New frmLauncherTemplateEdit(template, isNew:=True)
            If editForm.ShowDialog(Me.ParentForm) = DialogResult.OK Then
                LauncherTemplateManager.SaveTemplate(editForm.Result)
                RefreshTemplateList()
            End If
        End Using
    End Sub

#End Region

#Region "Template Edit/Delete"

    Private Sub btnEditTemplate_Click(sender As Object, e As EventArgs) Handles btnEditTemplate.Click
        If lstTemplates.SelectedItem Is Nothing Then Return

        Dim template = DirectCast(lstTemplates.SelectedItem, LauncherTemplate)
        Dim oldName = template.Name

        Using editForm As New frmLauncherTemplateEdit(template, isNew:=False)
            If editForm.ShowDialog(Me.ParentForm) = DialogResult.OK Then
                LauncherTemplateManager.UpdateTemplate(oldName, editForm.Result)
                RefreshTemplateList()
            End If
        End Using
    End Sub

    Private Sub btnDeleteTemplate_Click(sender As Object, e As EventArgs) Handles btnDeleteTemplate.Click
        If lstTemplates.SelectedItem Is Nothing Then Return

        Dim template = DirectCast(lstTemplates.SelectedItem, LauncherTemplate)

        Dim result = MessageBox.Show(
            $"Delete the launcher template '{template.Name}'?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            LauncherTemplateManager.DeleteTemplate(template.Name)
            RefreshTemplateList()
        End If
    End Sub

#End Region

#Region "Shortcut Creation"

    Private Sub cboTemplate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTemplate.SelectedIndexChanged
        ' Update resolution/options with template defaults
        If cboTemplate.SelectedItem IsNot Nothing Then
            Dim template = DirectCast(cboTemplate.SelectedItem, LauncherTemplate)
            numWidth.Value = Math.Max(numWidth.Minimum, Math.Min(numWidth.Maximum, template.DefaultWidth))
            numHeight.Value = Math.Max(numHeight.Minimum, Math.Min(numHeight.Maximum, template.DefaultHeight))
            txtOptions.Text = template.DefaultOptions
        End If
    End Sub

    Private Sub chkOverrideResolution_CheckedChanged(sender As Object, e As EventArgs) Handles chkOverrideResolution.CheckedChanged
        numWidth.Enabled = chkOverrideResolution.Checked
        numHeight.Enabled = chkOverrideResolution.Checked
        lblResX.Enabled = chkOverrideResolution.Checked
    End Sub

    Private Sub chkOverrideOptions_CheckedChanged(sender As Object, e As EventArgs) Handles chkOverrideOptions.CheckedChanged
        txtOptions.Enabled = chkOverrideOptions.Checked
    End Sub

    Private Sub btnCreateShortcut_Click(sender As Object, e As EventArgs) Handles btnCreateShortcut.Click
        ' Validate
        If cboTemplate.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a launcher template.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If String.IsNullOrWhiteSpace(txtCharacter.Text) Then
            MessageBox.Show("Please enter a character name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCharacter.Focus()
            Return
        End If

        If String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MessageBox.Show("Please enter a password.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPassword.Focus()
            Return
        End If

        ' Determine target folder
        Dim folder = TargetFolder
        If String.IsNullOrWhiteSpace(folder) Then
            folder = My.Settings.links
        End If

        If String.IsNullOrWhiteSpace(folder) OrElse Not Directory.Exists(folder) Then
            MessageBox.Show("Quick Launch folder is not configured or does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Create shortcut
        Dim template = DirectCast(cboTemplate.SelectedItem, LauncherTemplate)
        Dim widthOverride As Integer? = If(chkOverrideResolution.Checked, CInt(numWidth.Value), Nothing)
        Dim heightOverride As Integer? = If(chkOverrideResolution.Checked, CInt(numHeight.Value), Nothing)
        Dim optionsOverride As String = If(chkOverrideOptions.Checked, txtOptions.Text, Nothing)

        Dim success = LauncherTemplateManager.CreateShortcut(
            template,
            txtCharacter.Text.Trim(),
            txtPassword.Text,
            folder,
            widthOverride,
            heightOverride,
            optionsOverride)

        If success Then
            Dim charName = txtCharacter.Text.Trim()
            MessageBox.Show($"Shortcut for '{charName}' created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Raise event before clearing
            Dim shortcutPath = IO.Path.Combine(folder, charName & ".lnk")
            RaiseEvent ShortcutCreated(Me, shortcutPath)

            ' Clear fields for next entry
            txtCharacter.Clear()
            txtPassword.Clear()
            txtCharacter.Focus()
        Else
            MessageBox.Show("Failed to create shortcut. Check that the launcher exe exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

#End Region

#Region "Bulk Add"

    Private Sub SetupUsernamesGrid()
        dgvUsernames.Columns.Clear()
        dgvUsernames.Columns.Add("Username", "Username")
        dgvUsernames.Columns.Add("Nickname", "Nickname (optional)")
        dgvUsernames.Columns(0).Width = 150
        dgvUsernames.Columns(1).Width = 150
        dgvUsernames.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub PopulateSourceShortcuts()
        cboSourceShortcut.Items.Clear()
        Dim folder = If(String.IsNullOrEmpty(TargetFolder), My.Settings.links, TargetFolder)
        If String.IsNullOrEmpty(folder) OrElse Not Directory.Exists(folder) Then Return

        ' Recursively find all .lnk files
        For Each lnkFile In Directory.GetFiles(folder, "*.lnk", SearchOption.AllDirectories)
            cboSourceShortcut.Items.Add(lnkFile)
        Next
    End Sub

    Private Sub PopulateTargetFolders()
        cboTargetFolder.Items.Clear()
        Dim rootFolder = If(String.IsNullOrEmpty(TargetFolder), My.Settings.links, TargetFolder)
        If String.IsNullOrEmpty(rootFolder) OrElse Not Directory.Exists(rootFolder) Then Return

        ' Add root folder
        cboTargetFolder.Items.Add(rootFolder)

        ' Add all subdirectories
        For Each subDir In Directory.GetDirectories(rootFolder, "*", SearchOption.AllDirectories)
            cboTargetFolder.Items.Add(subDir)
        Next

        If cboTargetFolder.Items.Count > 0 Then
            cboTargetFolder.SelectedIndex = 0
        End If
    End Sub

    Private Sub btnRefreshShortcuts_Click(sender As Object, e As EventArgs) Handles btnRefreshShortcuts.Click
        PopulateSourceShortcuts()
        PopulateTargetFolders()
    End Sub

    Private Sub btnBrowseFolder_Click(sender As Object, e As EventArgs) Handles btnBrowseFolder.Click
        Using fbd As New FolderBrowserDialog()
            fbd.SelectedPath = If(cboTargetFolder.SelectedItem?.ToString(), My.Settings.links)
            If fbd.ShowDialog() = DialogResult.OK Then
                If Not cboTargetFolder.Items.Contains(fbd.SelectedPath) Then
                    cboTargetFolder.Items.Add(fbd.SelectedPath)
                End If
                cboTargetFolder.SelectedItem = fbd.SelectedPath
            End If
        End Using
    End Sub

    Private Sub cboSourceShortcut_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSourceShortcut.SelectedIndexChanged
        If cboSourceShortcut.SelectedItem Is Nothing Then
            lblSourceInfo.Text = "Select a shortcut to copy settings from"
            Return
        End If

        Dim sli As New ShellLinkInfo()
        If sli.Load(cboSourceShortcut.SelectedItem.ToString()) Then
            Dim args = ParseShortcutArguments(sli.Arguments)
            Dim w = If(args.ContainsKey("w"), args("w"), "?")
            Dim h = If(args.ContainsKey("h"), args("h"), "?")
            Dim o = If(args.ContainsKey("o"), args("o"), "")
            lblSourceInfo.Text = $"Exe: {IO.Path.GetFileName(sli.TargetPath)}" & vbCrLf &
                                 $"Resolution: {w}x{h}  |  Options: {If(String.IsNullOrEmpty(o), "(none)", o)}"
        Else
            lblSourceInfo.Text = "Failed to read shortcut"
        End If
    End Sub

    Private Function ParseShortcutArguments(args As String) As Dictionary(Of String, String)
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

    Private Sub btnCreateBulk_Click(sender As Object, e As EventArgs) Handles btnCreateBulk.Click
        ' Validate source shortcut
        If cboSourceShortcut.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a source shortcut to copy settings from.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Validate usernames and collect nicknames
        Dim accounts As New List(Of Tuple(Of String, String))  ' (username, nickname)
        For Each row As DataGridViewRow In dgvUsernames.Rows
            If row.IsNewRow Then Continue For
            Dim username = row.Cells(0).Value?.ToString()?.Trim()
            Dim nickname = row.Cells(1).Value?.ToString()?.Trim()
            If Not String.IsNullOrEmpty(username) Then
                accounts.Add(Tuple.Create(username, If(String.IsNullOrEmpty(nickname), username, nickname)))
            End If
        Next

        If accounts.Count = 0 Then
            MessageBox.Show("Please add at least one username.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Validate password
        If String.IsNullOrWhiteSpace(txtBulkPassword.Text) Then
            MessageBox.Show("Please enter a password.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtBulkPassword.Focus()
            Return
        End If

        ' Validate target folder
        Dim bulkTargetFolder As String = cboTargetFolder.SelectedItem?.ToString()
        If String.IsNullOrEmpty(bulkTargetFolder) OrElse Not Directory.Exists(bulkTargetFolder) Then
            MessageBox.Show("Please select a valid target folder.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Load source shortcut
        Dim sli As New ShellLinkInfo()
        If Not sli.Load(cboSourceShortcut.SelectedItem.ToString()) Then
            MessageBox.Show("Failed to read the source shortcut.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim args = ParseShortcutArguments(sli.Arguments)

        ' Create shortcuts for each account by copying source and replacing -u and -p
        Dim created = 0
        Dim failed = 0
        Dim doOverwrite = chkOverwrite.Checked

        For Each account In accounts
            Dim username = account.Item1
            Dim displayName = account.Item2  ' nickname or username
            Try
                ' Build new arguments: copy all from source, replace -u and -p
                args("u") = username
                args("p") = txtBulkPassword.Text
                Dim newArgs = String.Join(" ", args.Select(Function(kvp) $"-{kvp.Key}{kvp.Value}"))

                ' Build shortcut path using nickname/displayName for filename
                Dim shortcutName = LauncherTemplateManager.SanitizeFileName(displayName) & ".lnk"
                Dim shortcutPath = IO.Path.Combine(bulkTargetFolder, shortcutName)

                ' Handle duplicate names (unless overwrite is enabled)
                If Not doOverwrite Then
                    Dim counter = 1
                    While IO.File.Exists(shortcutPath)
                        shortcutName = $"{LauncherTemplateManager.SanitizeFileName(displayName)} ({counter}).lnk"
                        shortcutPath = IO.Path.Combine(bulkTargetFolder, shortcutName)
                        counter += 1
                        If counter > 100 Then Continue For
                    End While
                End If

                ' Create shortcut
                Dim newSli As New ShellLinkInfo()
                newSli.TargetPath = sli.TargetPath
                newSli.WorkingDirectory = sli.WorkingDirectory
                newSli.Arguments = newArgs
                newSli.Description = $"Astonia - {displayName}"
                newSli.IconPath = sli.IconPath
                newSli.IconIndex = sli.IconIndex
                newSli.Save(shortcutPath)

                created += 1
                RaiseEvent ShortcutCreated(Me, shortcutPath)
            Catch ex As Exception
                failed += 1
            End Try
        Next

        ' Show result
        If failed = 0 Then
            MessageBox.Show($"Successfully created {created} shortcut(s).", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show($"Created {created} shortcut(s), {failed} failed.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        ' Clear usernames grid, keep password
        dgvUsernames.Rows.Clear()
    End Sub

#End Region

#Region "Edit Tab"

    Private Sub SetupEditGrid()
        dgvEditShortcuts.Columns.Clear()
        dgvEditShortcuts.Columns.Add("CurrentName", "Current Name")
        dgvEditShortcuts.Columns.Add("NewName", "New Name")
        dgvEditShortcuts.Columns(0).ReadOnly = True
        dgvEditShortcuts.Columns(0).Width = 180
        dgvEditShortcuts.Columns(1).Width = 180
        dgvEditShortcuts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvEditShortcuts.AllowUserToAddRows = False
        dgvEditShortcuts.AllowUserToDeleteRows = False
    End Sub

    Private Sub PopulateEditFolders()
        cboEditFolder.Items.Clear()
        Dim rootFolder = If(String.IsNullOrEmpty(TargetFolder), My.Settings.links, TargetFolder)
        If String.IsNullOrEmpty(rootFolder) OrElse Not Directory.Exists(rootFolder) Then Return

        ' Add root folder
        cboEditFolder.Items.Add(rootFolder)

        ' Add all subdirectories
        For Each subDir In Directory.GetDirectories(rootFolder, "*", SearchOption.AllDirectories)
            cboEditFolder.Items.Add(subDir)
        Next

        If cboEditFolder.Items.Count > 0 Then
            cboEditFolder.SelectedIndex = 0
        End If
    End Sub

    Private Sub btnRefreshEdit_Click(sender As Object, e As EventArgs) Handles btnRefreshEdit.Click
        PopulateEditFolders()
    End Sub

    Private Sub cboEditFolder_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboEditFolder.SelectedIndexChanged
        LoadEditShortcuts()
    End Sub

    Private Sub LoadEditShortcuts()
        dgvEditShortcuts.Rows.Clear()
        If cboEditFolder.SelectedItem Is Nothing Then Return

        Dim folder = cboEditFolder.SelectedItem.ToString()
        If Not Directory.Exists(folder) Then Return

        For Each lnkFile In Directory.GetFiles(folder, "*.lnk")
            Dim name = IO.Path.GetFileNameWithoutExtension(lnkFile)
            dgvEditShortcuts.Rows.Add(name, name)
        Next
    End Sub

    Private Sub btnCreateFolder_Click(sender As Object, e As EventArgs) Handles btnCreateFolder.Click
        If String.IsNullOrWhiteSpace(txtNewFolder.Text) Then
            MessageBox.Show("Please enter a folder name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNewFolder.Focus()
            Return
        End If

        ' Get parent folder (currently selected or root)
        Dim parentFolder As String
        If cboEditFolder.SelectedItem IsNot Nothing Then
            parentFolder = cboEditFolder.SelectedItem.ToString()
        Else
            parentFolder = If(String.IsNullOrEmpty(TargetFolder), My.Settings.links, TargetFolder)
        End If

        If String.IsNullOrEmpty(parentFolder) OrElse Not Directory.Exists(parentFolder) Then
            MessageBox.Show("No valid parent folder selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            Dim newFolderPath = IO.Path.Combine(parentFolder, LauncherTemplateManager.SanitizeFileName(txtNewFolder.Text.Trim()))
            If Directory.Exists(newFolderPath) Then
                MessageBox.Show("Folder already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Directory.CreateDirectory(newFolderPath)
            MessageBox.Show($"Folder '{txtNewFolder.Text.Trim()}' created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtNewFolder.Clear()

            ' Refresh folder list and select the new folder
            PopulateEditFolders()
            PopulateTargetFolders()  ' Also refresh Bulk Add target folders
            For i = 0 To cboEditFolder.Items.Count - 1
                If cboEditFolder.Items(i).ToString() = newFolderPath Then
                    cboEditFolder.SelectedIndex = i
                    Exit For
                End If
            Next
        Catch ex As Exception
            MessageBox.Show($"Failed to create folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnApplyRenames_Click(sender As Object, e As EventArgs) Handles btnApplyRenames.Click
        If cboEditFolder.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a folder.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim folder = cboEditFolder.SelectedItem.ToString()
        Dim renamed = 0
        Dim failed = 0

        For Each row As DataGridViewRow In dgvEditShortcuts.Rows
            Dim currentName = row.Cells(0).Value?.ToString()
            Dim newName = row.Cells(1).Value?.ToString()?.Trim()

            If String.IsNullOrEmpty(currentName) OrElse String.IsNullOrEmpty(newName) Then Continue For
            If currentName = newName Then Continue For

            Try
                Dim currentPath = IO.Path.Combine(folder, currentName & ".lnk")
                Dim newPath = IO.Path.Combine(folder, LauncherTemplateManager.SanitizeFileName(newName) & ".lnk")

                If Not File.Exists(currentPath) Then Continue For
                If File.Exists(newPath) AndAlso Not currentPath.Equals(newPath, StringComparison.OrdinalIgnoreCase) Then
                    failed += 1
                    Continue For  ' Target name already exists
                End If

                File.Move(currentPath, newPath)
                renamed += 1
            Catch ex As Exception
                failed += 1
            End Try
        Next

        If renamed > 0 OrElse failed > 0 Then
            If failed = 0 Then
                MessageBox.Show($"Renamed {renamed} shortcut(s).", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show($"Renamed {renamed} shortcut(s), {failed} failed.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
            LoadEditShortcuts()  ' Refresh the grid
            PopulateSourceShortcuts()  ' Refresh Bulk Add source shortcuts
        Else
            MessageBox.Show("No changes to apply.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

#End Region

End Class
