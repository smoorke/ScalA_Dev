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

End Class
