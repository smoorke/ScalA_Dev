Imports System.IO

''' <summary>
''' Dialog for editing launcher template properties
''' </summary>
Public Class frmLauncherTemplateEdit

    Private _originalName As String
    Private _isNew As Boolean

    ''' <summary>
    ''' The resulting template after editing
    ''' </summary>
    Public Property Result As LauncherTemplate

    ''' <summary>
    ''' Creates a new edit dialog for a template
    ''' </summary>
    ''' <param name="template">The template to edit</param>
    ''' <param name="isNew">True if this is a new template being created</param>
    Public Sub New(template As LauncherTemplate, isNew As Boolean)
        InitializeComponent()

        _isNew = isNew
        _originalName = template.Name
        Result = template

        ' Populate fields
        txtName.Text = template.Name
        txtExePath.Text = template.ExePath
        txtWorkDir.Text = template.WorkingDirectory
        numWidth.Value = Math.Max(numWidth.Minimum, Math.Min(numWidth.Maximum, template.DefaultWidth))
        numHeight.Value = Math.Max(numHeight.Minimum, Math.Min(numHeight.Maximum, template.DefaultHeight))
        txtOptions.Text = template.DefaultOptions

        ' Update title
        Me.Text = If(isNew, "Add Launcher Template", "Edit Launcher Template")
    End Sub

    Private Sub btnBrowseExe_Click(sender As Object, e As EventArgs) Handles btnBrowseExe.Click
        If Not String.IsNullOrWhiteSpace(txtExePath.Text) AndAlso File.Exists(txtExePath.Text) Then
            ofdExe.InitialDirectory = IO.Path.GetDirectoryName(txtExePath.Text)
            ofdExe.FileName = IO.Path.GetFileName(txtExePath.Text)
        End If

        If ofdExe.ShowDialog(Me) = DialogResult.OK Then
            txtExePath.Text = ofdExe.FileName

            ' Auto-fill working directory if empty
            If String.IsNullOrWhiteSpace(txtWorkDir.Text) Then
                txtWorkDir.Text = IO.Path.GetDirectoryName(ofdExe.FileName)
            End If

            ' Auto-fill name if empty or still default
            If String.IsNullOrWhiteSpace(txtName.Text) OrElse txtName.Text = _originalName Then
                txtName.Text = IO.Path.GetFileNameWithoutExtension(ofdExe.FileName)
            End If
        End If
    End Sub

    Private Sub btnBrowseWorkDir_Click(sender As Object, e As EventArgs) Handles btnBrowseWorkDir.Click
        If Not String.IsNullOrWhiteSpace(txtWorkDir.Text) AndAlso Directory.Exists(txtWorkDir.Text) Then
            fbdWorkDir.SelectedPath = txtWorkDir.Text
        ElseIf Not String.IsNullOrWhiteSpace(txtExePath.Text) AndAlso File.Exists(txtExePath.Text) Then
            fbdWorkDir.SelectedPath = IO.Path.GetDirectoryName(txtExePath.Text)
        End If

        If fbdWorkDir.ShowDialog(Me) = DialogResult.OK Then
            txtWorkDir.Text = fbdWorkDir.SelectedPath
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' Validate
        If Not ValidateInput() Then Return

        ' Build result
        Result = New LauncherTemplate() With {
            .Name = txtName.Text.Trim(),
            .ExePath = txtExePath.Text.Trim(),
            .WorkingDirectory = txtWorkDir.Text.Trim(),
            .DefaultWidth = CInt(numWidth.Value),
            .DefaultHeight = CInt(numHeight.Value),
            .DefaultOptions = txtOptions.Text.Trim()
        }

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Function ValidateInput() As Boolean
        ' Validate name
        If String.IsNullOrWhiteSpace(txtName.Text) Then
            MessageBox.Show("Please enter a template name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtName.Focus()
            Return False
        End If

        ' Check for duplicate name (only if name changed or is new)
        Dim newName = txtName.Text.Trim()
        If Not newName.Equals(_originalName, StringComparison.OrdinalIgnoreCase) Then
            If LauncherTemplateManager.TemplateExists(newName) Then
                MessageBox.Show("A template with this name already exists.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtName.Focus()
                Return False
            End If
        End If

        ' Validate exe path
        If String.IsNullOrWhiteSpace(txtExePath.Text) Then
            MessageBox.Show("Please enter an executable path.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtExePath.Focus()
            Return False
        End If

        If Not File.Exists(txtExePath.Text) Then
            MessageBox.Show("The executable file does not exist.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtExePath.Focus()
            Return False
        End If

        ' Validate working directory (optional but must exist if specified)
        If Not String.IsNullOrWhiteSpace(txtWorkDir.Text) AndAlso Not Directory.Exists(txtWorkDir.Text) Then
            MessageBox.Show("The working directory does not exist.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtWorkDir.Focus()
            Return False
        End If

        Return True
    End Function

End Class
