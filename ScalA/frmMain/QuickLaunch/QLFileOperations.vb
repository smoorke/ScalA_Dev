Namespace QL

    ''' <summary>
    ''' Module for QuickLaunch file operation helpers
    ''' </summary>
    Public Module QLFileOperations

        ''' <summary>
        ''' Windows reserved file names that cannot be used
        ''' </summary>
        Public ReadOnly ReservedNames As String() = {
            "CON", "PRN", "AUX", "NUL",
            "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
        }

        ''' <summary>
        ''' File extensions whose extensions should be hidden in display
        ''' </summary>
        Public ReadOnly HiddenExtensions As String() = {".lnk", ".url"}

        ''' <summary>
        ''' Generates a unique folder name in the specified path
        ''' </summary>
        ''' <param name="parentPath">Parent directory path</param>
        ''' <param name="baseName">Base name for the folder</param>
        ''' <returns>Full path to a non-existing folder</returns>
        Public Function GenerateUniqueFolderPath(parentPath As String, Optional baseName As String = "New Folder") As String
            Dim newPath As String = IO.Path.Combine(parentPath, baseName)

            If Not IO.Directory.Exists(newPath) Then
                Return newPath
            End If

            Dim i As Integer = 2
            While IO.Directory.Exists(newPath)
                newPath = IO.Path.Combine(parentPath, $"{baseName} ({i})")
                i += 1
            End While

            Return newPath
        End Function

        ''' <summary>
        ''' Checks if a filename is a Windows reserved name
        ''' </summary>
        ''' <param name="fileName">Filename to check (with or without extension)</param>
        ''' <returns>True if name is reserved</returns>
        Public Function IsReservedName(fileName As String) As Boolean
            Dim nameWithoutExt As String = IO.Path.GetFileNameWithoutExtension(fileName).ToUpperInvariant()
            Return ReservedNames.Contains(nameWithoutExt)
        End Function

        ''' <summary>
        ''' Checks if a filename contains invalid characters
        ''' </summary>
        ''' <param name="fileName">Filename to check</param>
        ''' <returns>True if name contains invalid characters</returns>
        Public Function ContainsInvalidCharacters(fileName As String) As Boolean
            Return fileName.Any(Function(c) IO.Path.GetInvalidFileNameChars().Contains(c))
        End Function

        ''' <summary>
        ''' Cleans a filename by removing invalid characters and trimming
        ''' </summary>
        ''' <param name="fileName">Filename to clean</param>
        ''' <returns>Cleaned filename</returns>
        Public Function CleanFileName(fileName As String) As String
            If String.IsNullOrEmpty(fileName) Then Return String.Empty

            ' Remove invalid characters
            Dim clean As String = New String(fileName.Where(Function(c) Not IO.Path.GetInvalidFileNameChars().Contains(c)).ToArray())

            ' Trim leading spaces and trailing spaces/dots
            Return clean.TrimStart().TrimEnd(" "c, "."c)
        End Function

        ''' <summary>
        ''' Checks if extension is being changed during rename
        ''' </summary>
        ''' <param name="oldName">Original filename</param>
        ''' <param name="newName">New filename</param>
        ''' <param name="isFolder">Whether this is a folder</param>
        ''' <param name="originalPath">Original full path (to check if extension is hidden)</param>
        ''' <returns>True if extension is being changed</returns>
        Public Function IsExtensionChanging(oldName As String, newName As String, isFolder As Boolean, originalPath As String) As Boolean
            If isFolder Then Return False

            Dim originalExt As String = IO.Path.GetExtension(originalPath).ToLower()
            If HiddenExtensions.Contains(originalExt) Then Return False

            Dim oldExt As String = IO.Path.GetExtension(oldName).ToLower()
            Dim newExt As String = IO.Path.GetExtension(newName).ToLower()

            Return oldExt <> newExt
        End Function

        ''' <summary>
        ''' Gets the display name for a file (hides extension for .lnk, .url)
        ''' </summary>
        ''' <param name="path">Full path to the file</param>
        ''' <returns>Display name</returns>
        Public Function GetDisplayName(path As String) As String
            Dim ext As String = IO.Path.GetExtension(path).ToLower()
            If HiddenExtensions.Contains(ext) Then
                Return IO.Path.GetFileNameWithoutExtension(path)
            End If
            Return IO.Path.GetFileName(path)
        End Function

        ''' <summary>
        ''' Appends the original extension when renaming files with hidden extensions
        ''' </summary>
        ''' <param name="newName">The new name entered by user</param>
        ''' <param name="originalPath">Original full path</param>
        ''' <returns>Name with extension restored if needed</returns>
        Public Function RestoreHiddenExtension(newName As String, originalPath As String) As String
            Dim originalExt As String = IO.Path.GetExtension(originalPath).ToLower()
            If HiddenExtensions.Contains(originalExt) Then
                Return newName & IO.Path.GetExtension(originalPath)
            End If
            Return newName
        End Function

        ''' <summary>
        ''' Gets the new full path after rename
        ''' </summary>
        ''' <param name="originalPath">Original full path</param>
        ''' <param name="newName">New filename</param>
        ''' <returns>New full path</returns>
        Public Function GetRenamedPath(originalPath As String, newName As String) As String
            Dim cleanPath As String = originalPath.TrimEnd("\"c)
            Dim parentDir As String = IO.Path.GetDirectoryName(cleanPath)
            Return IO.Path.Combine(parentDir, newName)
        End Function

        ''' <summary>
        ''' Folder statistics structure
        ''' </summary>
        Public Structure FolderStats
            Public FolderCount As Integer
            Public FileCount As Integer
        End Structure

        ''' <summary>
        ''' Gets folder statistics for delete confirmation
        ''' </summary>
        ''' <param name="folderPath">Path to folder</param>
        ''' <returns>FolderStats with folder and file counts</returns>
        Public Function GetFolderStats(folderPath As String) As FolderStats
            Dim result As New FolderStats()
            Try
                result.FolderCount = IO.Directory.GetDirectories(folderPath, "*.*", IO.SearchOption.AllDirectories).Length
                result.FileCount = IO.Directory.GetFiles(folderPath, "*.*", IO.SearchOption.AllDirectories) _
                    .Where(Function(f) Not IO.Path.GetFileName(f).Equals("desktop.ini", StringComparison.OrdinalIgnoreCase)) _
                    .Count()
            Catch
                result.FolderCount = 0
                result.FileCount = 0
            End Try
            Return result
        End Function

        ''' <summary>
        ''' Formats a delete confirmation message
        ''' </summary>
        ''' <param name="path">Path to item being deleted</param>
        ''' <param name="isPermanent">Whether delete is permanent (Shift held)</param>
        ''' <returns>Formatted confirmation message</returns>
        Public Function GetDeleteConfirmationMessage(path As String, isPermanent As Boolean) As String
            Dim isFolder As Boolean = path.EndsWith("\")
            Dim name As String = If(isFolder, IO.Path.GetFileName(path.TrimEnd("\"c)), IO.Path.GetFileName(path))

            Dim sb As New Text.StringBuilder()

            If isFolder Then
                Dim stats = GetFolderStats(path)
                Dim folS As String = If(stats.FolderCount = 1, "", "s")
                Dim filS As String = If(stats.FileCount = 1, "", "s")
                sb.AppendLine($"This folder contains {stats.FolderCount} folder{folS} and {stats.FileCount} file{filS}.")
            End If

            If isPermanent Then
                sb.Append($"Are you sure you want to permanently delete ""{name}""?")
            Else
                sb.Append($"Are you sure you want to move ""{name}"" to the Recycle Bin?")
            End If

            Return sb.ToString()
        End Function

        ''' <summary>
        ''' Validates a rename operation
        ''' </summary>
        ''' <param name="newName">The new name to validate</param>
        ''' <returns>Error message or empty string if valid</returns>
        Public Function ValidateRename(newName As String) As String
            If String.IsNullOrWhiteSpace(newName) Then
                Return "Name cannot be empty."
            End If

            If IsReservedName(newName) Then
                Return "The specified device name is invalid."
            End If

            If ContainsInvalidCharacters(newName) Then
                Return "A file name can't contain any of the following characters: \ / : * ? "" < > |"
            End If

            Return String.Empty
        End Function

        ''' <summary>
        ''' Selection range structure for rename dialog
        ''' </summary>
        Public Structure SelectionRange
            Public Start As Integer
            Public Length As Integer

            Public Sub New(start As Integer, length As Integer)
                Me.Start = start
                Me.Length = length
            End Sub
        End Structure

        ''' <summary>
        ''' Gets the selection range for filename in rename dialog
        ''' Selects filename without extension for files, full name for folders and hidden extensions
        ''' </summary>
        ''' <param name="path">Full path to item</param>
        ''' <param name="displayName">Current display name</param>
        ''' <returns>SelectionRange with start and length</returns>
        Public Function GetRenameSelectionRange(path As String, displayName As String) As SelectionRange
            Dim isFolder As Boolean = path.EndsWith("\")
            Dim ext As String = IO.Path.GetExtension(path).ToLower()
            Dim isHiddenExt As Boolean = HiddenExtensions.Contains(ext)

            If isFolder OrElse isHiddenExt Then
                Return New SelectionRange(0, displayName.Length)
            End If

            If displayName.Contains(".") Then
                Dim lastDot As Integer = displayName.LastIndexOf(".")
                If lastDot > 0 Then
                    Return New SelectionRange(0, lastDot)
                End If
            End If

            Return New SelectionRange(0, displayName.Length)
        End Function

    End Module

End Namespace ' QL
