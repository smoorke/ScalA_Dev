Imports System.IO

''' <summary>
''' Quick dialog for creating a character shortcut from a template
''' </summary>
Public Class frmQuickShortcut

    Private _template As LauncherTemplate
    Private _rootFolder As String
    Private _initialFolder As String

    ''' <summary>
    ''' Folder item for the dropdown
    ''' </summary>
    Private Class FolderItem
        Public Property Path As String
        Public Property DisplayName As String

        Public Sub New(path As String, displayName As String)
            Me.Path = path
            Me.DisplayName = displayName
        End Sub

        Public Overrides Function ToString() As String
            Return DisplayName
        End Function
    End Class

    ''' <summary>
    ''' The character name entered
    ''' </summary>
    Public Property CharacterName As String

    ''' <summary>
    ''' The password entered
    ''' </summary>
    Public Property Password As String

    ''' <summary>
    ''' The selected target folder
    ''' </summary>
    Public Property SelectedFolder As String

    ''' <summary>
    ''' Creates a new quick shortcut dialog
    ''' </summary>
    ''' <param name="template">The launcher template to use</param>
    ''' <param name="rootFolder">The QL root folder</param>
    ''' <param name="initialFolder">The initially selected folder (can be subfolder)</param>
    Public Sub New(template As LauncherTemplate, rootFolder As String, Optional initialFolder As String = Nothing)
        InitializeComponent()

        _template = template
        _rootFolder = rootFolder.TrimEnd("\"c)
        _initialFolder = If(String.IsNullOrWhiteSpace(initialFolder), _rootFolder, initialFolder.TrimEnd("\"c))

        lblTemplate.Text = $"Template: {template.Name}"

        PopulateFolders()
    End Sub

    Private Sub PopulateFolders()
        cboFolder.Items.Clear()

        ' Add root folder
        Dim rootName = IO.Path.GetFileName(_rootFolder)
        If String.IsNullOrWhiteSpace(rootName) Then rootName = _rootFolder
        cboFolder.Items.Add(New FolderItem(_rootFolder, $"/ (Root)"))

        ' Add subfolders recursively
        Try
            AddSubfolders(_rootFolder, 1)
        Catch ex As Exception
            Debug.WriteLine($"Error enumerating folders: {ex.Message}")
        End Try

        ' Select the initial folder
        For i = 0 To cboFolder.Items.Count - 1
            Dim item = DirectCast(cboFolder.Items(i), FolderItem)
            If item.Path.Equals(_initialFolder, StringComparison.OrdinalIgnoreCase) Then
                cboFolder.SelectedIndex = i
                Exit For
            End If
        Next

        ' If nothing selected, select root
        If cboFolder.SelectedIndex < 0 AndAlso cboFolder.Items.Count > 0 Then
            cboFolder.SelectedIndex = 0
        End If
    End Sub

    Private Sub AddSubfolders(parentPath As String, level As Integer)
        If level > 5 Then Return ' Limit depth

        Try
            For Each subDir In Directory.EnumerateDirectories(parentPath)
                ' Skip hidden/system folders and reparse points
                Dim attrs = File.GetAttributes(subDir)
                If (attrs And FileAttributes.Hidden) <> 0 Then Continue For
                If (attrs And FileAttributes.System) <> 0 Then Continue For
                If (attrs And FileAttributes.ReparsePoint) <> 0 Then Continue For

                Dim folderName = IO.Path.GetFileName(subDir)
                Dim indent = New String(" "c, level * 2)
                cboFolder.Items.Add(New FolderItem(subDir, $"{indent}{folderName}"))

                ' Recurse
                AddSubfolders(subDir, level + 1)
            Next
        Catch ex As UnauthorizedAccessException
            ' Skip folders we can't access
        End Try
    End Sub

    Private Sub btnNewFolder_Click(sender As Object, e As EventArgs) Handles btnNewFolder.Click
        ' Get current selected folder as parent
        Dim parentFolder = _rootFolder
        If cboFolder.SelectedItem IsNot Nothing Then
            parentFolder = DirectCast(cboFolder.SelectedItem, FolderItem).Path
        End If

        ' Prompt for folder name
        Dim folderName = InputBox("Enter new folder name:", "New Folder", "New Folder")
        If String.IsNullOrWhiteSpace(folderName) Then Return

        ' Validate folder name
        Dim invalidChars = IO.Path.GetInvalidFileNameChars()
        For Each c In invalidChars
            If folderName.Contains(c) Then
                MessageBox.Show("Folder name contains invalid characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        Next

        ' Create the folder
        Dim newFolderPath = IO.Path.Combine(parentFolder, folderName)
        Try
            If Directory.Exists(newFolderPath) Then
                MessageBox.Show("A folder with that name already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Directory.CreateDirectory(newFolderPath)

            ' Refresh and select the new folder
            _initialFolder = newFolderPath
            PopulateFolders()

        Catch ex As Exception
            MessageBox.Show($"Failed to create folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        ' Validate folder selection
        If cboFolder.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a folder.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Validate character name
        If String.IsNullOrWhiteSpace(txtCharacter.Text) Then
            MessageBox.Show("Please enter a character name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCharacter.Focus()
            Return
        End If

        ' Validate password
        If String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MessageBox.Show("Please enter a password.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPassword.Focus()
            Return
        End If

        ' Store values
        CharacterName = txtCharacter.Text.Trim()
        Password = txtPassword.Text
        SelectedFolder = DirectCast(cboFolder.SelectedItem, FolderItem).Path

        ' Try to create the shortcut
        Dim success = LauncherTemplateManager.CreateShortcut(
            _template,
            CharacterName,
            Password,
            SelectedFolder)

        If success Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show("Failed to create shortcut. Check that the launcher executable exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class
