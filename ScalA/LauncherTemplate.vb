Imports System.IO

''' <summary>
''' Represents a launcher template for creating Astonia character shortcuts.
''' </summary>
Public Class LauncherTemplate

    Private Const DELIMITER As Char = "|"c
    Private Const FIELD_COUNT As Integer = 6

    ''' <summary>
    ''' Display name for this launcher (e.g., "Astonia Classic", "Astonia Reborn")
    ''' </summary>
    Public Property Name As String = ""

    ''' <summary>
    ''' Full path to the Astonia executable
    ''' </summary>
    Public Property ExePath As String = ""

    ''' <summary>
    ''' Working directory for the shortcut (typically the exe's folder)
    ''' </summary>
    Public Property WorkingDirectory As String = ""

    ''' <summary>
    ''' Default window width (-w argument)
    ''' </summary>
    Public Property DefaultWidth As Integer = 800

    ''' <summary>
    ''' Default window height (-h argument)
    ''' </summary>
    Public Property DefaultHeight As Integer = 600

    ''' <summary>
    ''' Default -o options value (may be empty)
    ''' </summary>
    Public Property DefaultOptions As String = ""

    ''' <summary>
    ''' Creates a new empty LauncherTemplate
    ''' </summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Creates a LauncherTemplate from an executable path
    ''' </summary>
    Public Sub New(exePath As String)
        Me.ExePath = exePath
        Me.WorkingDirectory = IO.Path.GetDirectoryName(exePath)
        Me.Name = IO.Path.GetFileNameWithoutExtension(exePath)
    End Sub

    ''' <summary>
    ''' Serializes the template to a pipe-delimited string for storage
    ''' </summary>
    Public Function Serialize() As String
        ' Escape any pipe characters in fields
        Dim escapedName = Name.Replace("|", "{{PIPE}}")
        Dim escapedPath = ExePath.Replace("|", "{{PIPE}}")
        Dim escapedWorkDir = WorkingDirectory.Replace("|", "{{PIPE}}")
        Dim escapedOptions = DefaultOptions.Replace("|", "{{PIPE}}")

        Return String.Join(DELIMITER,
            escapedName,
            escapedPath,
            escapedWorkDir,
            DefaultWidth.ToString(),
            DefaultHeight.ToString(),
            escapedOptions)
    End Function

    ''' <summary>
    ''' Deserializes a template from a pipe-delimited string
    ''' </summary>
    Public Shared Function Deserialize(data As String) As LauncherTemplate
        If String.IsNullOrWhiteSpace(data) Then Return Nothing

        Dim parts = data.Split(DELIMITER)
        If parts.Length < FIELD_COUNT Then Return Nothing

        Dim template As New LauncherTemplate()

        ' Unescape pipe characters
        template.Name = parts(0).Replace("{{PIPE}}", "|")
        template.ExePath = parts(1).Replace("{{PIPE}}", "|")
        template.WorkingDirectory = parts(2).Replace("{{PIPE}}", "|")

        Dim width As Integer
        If Integer.TryParse(parts(3), width) Then
            template.DefaultWidth = width
        End If

        Dim height As Integer
        If Integer.TryParse(parts(4), height) Then
            template.DefaultHeight = height
        End If

        template.DefaultOptions = parts(5).Replace("{{PIPE}}", "|")

        Return template
    End Function

    ''' <summary>
    ''' Builds the command-line arguments for a character shortcut
    ''' </summary>
    Public Function BuildArguments(username As String, password As String,
                                   Optional widthOverride As Integer? = Nothing,
                                   Optional heightOverride As Integer? = Nothing,
                                   Optional optionsOverride As String = Nothing) As String

        Dim args As New List(Of String)

        ' Username is required
        If Not String.IsNullOrWhiteSpace(username) Then
            args.Add($"-u{username}")
        End If

        ' Password
        If Not String.IsNullOrWhiteSpace(password) Then
            args.Add($"-p{password}")
        End If

        ' Width
        Dim w = If(widthOverride.HasValue, widthOverride.Value, DefaultWidth)
        If w > 0 Then
            args.Add($"-w{w}")
        End If

        ' Height
        Dim h = If(heightOverride.HasValue, heightOverride.Value, DefaultHeight)
        If h > 0 Then
            args.Add($"-h{h}")
        End If

        ' Options
        Dim opts = If(optionsOverride IsNot Nothing, optionsOverride, DefaultOptions)
        If Not String.IsNullOrWhiteSpace(opts) Then
            args.Add($"-o{opts}")
        End If

        Return String.Join(" ", args)
    End Function

    ''' <summary>
    ''' Validates the template has required fields
    ''' </summary>
    Public Function Validate(ByRef errorMessage As String) As Boolean
        If String.IsNullOrWhiteSpace(Name) Then
            errorMessage = "Template name is required."
            Return False
        End If

        If String.IsNullOrWhiteSpace(ExePath) Then
            errorMessage = "Executable path is required."
            Return False
        End If

        If Not File.Exists(ExePath) Then
            errorMessage = "The executable file does not exist."
            Return False
        End If

        If Not String.IsNullOrWhiteSpace(WorkingDirectory) AndAlso Not Directory.Exists(WorkingDirectory) Then
            errorMessage = "The working directory does not exist."
            Return False
        End If

        If DefaultWidth < 400 OrElse DefaultWidth > 9999 Then
            errorMessage = "Width must be between 400 and 9999."
            Return False
        End If

        If DefaultHeight < 300 OrElse DefaultHeight > 9999 Then
            errorMessage = "Height must be between 300 and 9999."
            Return False
        End If

        errorMessage = Nothing
        Return True
    End Function

    Public Overrides Function ToString() As String
        Return Name
    End Function

End Class
