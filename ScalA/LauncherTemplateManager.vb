Imports System.Collections.Specialized
Imports System.IO

''' <summary>
''' Manages CRUD operations for launcher templates stored in My.Settings
''' </summary>
Public Module LauncherTemplateManager

    ''' <summary>
    ''' Gets all saved launcher templates
    ''' </summary>
    Public Function GetTemplates() As List(Of LauncherTemplate)
        Dim result As New List(Of LauncherTemplate)

        Dim collection = My.Settings.LauncherTemplates
        If collection Is Nothing Then Return result

        For Each item As String In collection
            Dim template = LauncherTemplate.Deserialize(item)
            If template IsNot Nothing Then
                result.Add(template)
            End If
        Next

        Return result
    End Function

    ''' <summary>
    ''' Gets a template by name (case-insensitive)
    ''' </summary>
    Public Function GetTemplate(name As String) As LauncherTemplate
        Return GetTemplates().FirstOrDefault(Function(t) t.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
    End Function

    ''' <summary>
    ''' Saves a new template or updates an existing one with the same name
    ''' </summary>
    Public Sub SaveTemplate(template As LauncherTemplate)
        If template Is Nothing Then Throw New ArgumentNullException(NameOf(template))

        EnsureCollectionExists()

        ' Remove existing template with same name
        DeleteTemplate(template.Name)

        ' Add the new/updated template
        My.Settings.LauncherTemplates.Add(template.Serialize())
        My.Settings.Save()
    End Sub

    ''' <summary>
    ''' Updates an existing template, allowing name change
    ''' </summary>
    Public Sub UpdateTemplate(oldName As String, template As LauncherTemplate)
        If template Is Nothing Then Throw New ArgumentNullException(NameOf(template))

        EnsureCollectionExists()

        ' Remove the old template
        DeleteTemplate(oldName)

        ' Add the updated template
        My.Settings.LauncherTemplates.Add(template.Serialize())
        My.Settings.Save()
    End Sub

    ''' <summary>
    ''' Deletes a template by name (case-insensitive)
    ''' </summary>
    Public Sub DeleteTemplate(name As String)
        If String.IsNullOrWhiteSpace(name) Then Return

        Dim collection = My.Settings.LauncherTemplates
        If collection Is Nothing Then Return

        ' Find and remove matching template
        Dim toRemove As New List(Of String)
        For Each item As String In collection
            Dim template = LauncherTemplate.Deserialize(item)
            If template IsNot Nothing AndAlso template.Name.Equals(name, StringComparison.OrdinalIgnoreCase) Then
                toRemove.Add(item)
            End If
        Next

        For Each item In toRemove
            collection.Remove(item)
        Next

        If toRemove.Count > 0 Then
            My.Settings.Save()
        End If
    End Sub

    ''' <summary>
    ''' Checks if a template with the given name exists
    ''' </summary>
    Public Function TemplateExists(name As String) As Boolean
        Return GetTemplate(name) IsNot Nothing
    End Function

    ''' <summary>
    ''' Gets the count of saved templates
    ''' </summary>
    Public Function GetTemplateCount() As Integer
        Dim collection = My.Settings.LauncherTemplates
        If collection Is Nothing Then Return 0
        Return collection.Count
    End Function

    ''' <summary>
    ''' Creates a shortcut file using the template and character info
    ''' </summary>
    Public Function CreateShortcut(template As LauncherTemplate,
                                   username As String,
                                   password As String,
                                   targetFolder As String,
                                   Optional widthOverride As Integer? = Nothing,
                                   Optional heightOverride As Integer? = Nothing,
                                   Optional optionsOverride As String = Nothing) As Boolean

        If template Is Nothing Then Return False
        If String.IsNullOrWhiteSpace(username) Then Return False
        If String.IsNullOrWhiteSpace(targetFolder) Then Return False

        Try
            ' Build shortcut path
            Dim shortcutName = SanitizeFileName(username) & ".lnk"
            Dim shortcutPath = IO.Path.Combine(targetFolder, shortcutName)

            ' Handle duplicate names
            Dim counter = 1
            While File.Exists(shortcutPath)
                shortcutName = $"{SanitizeFileName(username)} ({counter}).lnk"
                shortcutPath = IO.Path.Combine(targetFolder, shortcutName)
                counter += 1
                If counter > 100 Then Return False ' Safety limit
            End While

            ' Build arguments
            Dim arguments = template.BuildArguments(username, password, widthOverride, heightOverride, optionsOverride)

            ' Create shortcut using ShellLinkInfo
            Dim sli As New ShellLinkInfo()
            sli.TargetPath = template.ExePath
            sli.WorkingDirectory = If(String.IsNullOrWhiteSpace(template.WorkingDirectory),
                                      IO.Path.GetDirectoryName(template.ExePath),
                                      template.WorkingDirectory)
            sli.Arguments = arguments
            sli.Description = $"Astonia - {username}"
            sli.Save(shortcutPath)

            Return True

        Catch ex As Exception
            Debug.WriteLine($"Error creating shortcut: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Ensures the LauncherTemplates collection exists in settings
    ''' </summary>
    Private Sub EnsureCollectionExists()
        If My.Settings.LauncherTemplates Is Nothing Then
            My.Settings.LauncherTemplates = New StringCollection()
        End If
    End Sub

    ''' <summary>
    ''' Sanitizes a filename by removing invalid characters
    ''' </summary>
    Private Function SanitizeFileName(name As String) As String
        Dim invalid = IO.Path.GetInvalidFileNameChars()
        Dim result = name

        For Each c In invalid
            result = result.Replace(c, "_"c)
        Next

        ' Also check for reserved Windows names
        Dim reserved = {"CON", "PRN", "AUX", "NUL",
                        "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
                        "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"}

        If reserved.Contains(result.ToUpper()) Then
            result = "_" & result
        End If

        Return result.Trim()
    End Function

End Module
