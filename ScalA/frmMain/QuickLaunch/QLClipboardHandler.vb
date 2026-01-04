Namespace QL

    ''' <summary>
    ''' Module for QuickLaunch clipboard operation helpers
    ''' </summary>
    Public Module QLClipboardHandler

        ''' <summary>
        ''' File extensions whose extensions should be hidden in paste menu text
        ''' </summary>
        Private ReadOnly HideExtensions As String() = {".lnk", ".url"}

        ''' <summary>
        ''' Gets display text for a paste menu item
        ''' </summary>
        ''' <param name="clipInfo">Clipboard file information</param>
        ''' <param name="maxLength">Maximum length before truncation with ellipsis</param>
        ''' <returns>Formatted paste menu text</returns>
        Friend Function GetPasteMenuText(clipInfo As ClipboardFileInfo, Optional maxLength As Integer = 16) As String
            If clipInfo.Files Is Nothing OrElse clipInfo.Files.Count = 0 Then
                Return "Paste"
            End If

            Dim action As String = If(clipInfo.Action.HasFlag(DragDropEffects.Move), "Move", "Paste")

            If clipInfo.Files.Count = 1 Then
                Dim filePath As String = clipInfo.Files(0)
                Dim displayName As String = IO.Path.GetFileName(filePath.TrimEnd("\"c))

                If String.IsNullOrEmpty(displayName) Then
                    displayName = filePath
                End If

                ' Hide extension for certain file types
                Dim ext As String = IO.Path.GetExtension(displayName).ToLower()
                If HideExtensions.Contains(ext) Then
                    displayName = IO.Path.GetFileNameWithoutExtension(displayName)
                End If

                Return $"{action} ""{displayName.CapWithEllipsis(maxLength)}"""
            Else
                Return $"{action} Multiple ({clipInfo.Files.Count})"
            End If
        End Function

        ''' <summary>
        ''' Gets tooltip text for a paste menu item (full filename if truncated)
        ''' </summary>
        ''' <param name="clipInfo">Clipboard file information</param>
        ''' <param name="maxLength">Length threshold - show tooltip if name exceeds this</param>
        ''' <returns>Tooltip text or empty string</returns>
        Friend Function GetPasteMenuTooltip(clipInfo As ClipboardFileInfo, Optional maxLength As Integer = 16) As String
            If clipInfo.Files Is Nothing OrElse clipInfo.Files.Count = 0 Then
                Return String.Empty
            End If

            If clipInfo.Files.Count = 1 Then
                Dim filePath As String = clipInfo.Files(0)
                Dim displayName As String = IO.Path.GetFileName(filePath.TrimEnd("\"c))

                If String.IsNullOrEmpty(displayName) Then
                    displayName = filePath
                End If

                Dim ext As String = IO.Path.GetExtension(displayName).ToLower()
                If HideExtensions.Contains(ext) Then
                    displayName = IO.Path.GetFileNameWithoutExtension(displayName)
                End If

                Return If(displayName.Length > maxLength, displayName, String.Empty)
            Else
                ' Build tooltip showing first few files
                Dim sb As New Text.StringBuilder()
                Dim idx As Integer = 0
                Const MaxToShow As Integer = 5

                For Each clipPath As String In clipInfo.Files
                    Dim name As String = IO.Path.GetFileName(clipPath)
                    If IO.Directory.Exists(clipPath) Then
                        name &= "\"
                    End If
                    sb.AppendLine(name)
                    idx += 1

                    If idx >= MaxToShow Then
                        sb.AppendLine($"<and {clipInfo.Files.Count - idx} more>")
                        Exit For
                    End If
                Next

                Return sb.ToString().TrimEnd()
            End If
        End Function

        ''' <summary>
        ''' Checks if an item path is in the clipboard selection
        ''' </summary>
        ''' <param name="clipInfo">Clipboard file information</param>
        ''' <param name="itemPath">Path to check</param>
        ''' <returns>True if item is in clipboard</returns>
        Friend Function IsItemInClipboard(clipInfo As ClipboardFileInfo, itemPath As String) As Boolean
            If clipInfo.Files Is Nothing OrElse clipInfo.Files.Count = 0 Then
                Return False
            End If
            Return clipInfo.Files.Contains(itemPath.TrimEnd("\"c))
        End Function

        ''' <summary>
        ''' Checks if clipboard has valid paste sources
        ''' </summary>
        ''' <param name="clipInfo">Clipboard file information</param>
        ''' <returns>True if there are valid files/folders to paste</returns>
        Friend Function HasValidPasteSources(clipInfo As ClipboardFileInfo) As Boolean
            If clipInfo.Files Is Nothing OrElse clipInfo.Files.Count = 0 Then
                Return False
            End If
            Return clipInfo.Files.Any(Function(f) IO.File.Exists(f) OrElse IO.Directory.Exists(f))
        End Function

        ''' <summary>
        ''' Checks if paste link option should be available
        ''' </summary>
        ''' <param name="clipInfo">Clipboard file information</param>
        ''' <returns>True if paste link is available</returns>
        Friend Function CanPasteLink(clipInfo As ClipboardFileInfo) As Boolean
            ' Can paste link if action supports it
            If Not clipInfo.Action.HasFlag(DragDropEffects.Link) Then
                Return False
            End If

            ' Don't show paste link for single .lnk file (already a link)
            If clipInfo.Files IsNot Nothing AndAlso clipInfo.Files.Count = 1 Then
                If clipInfo.Files(0).ToLower().EndsWith(".lnk") Then
                    Return False
                End If
            End If

            Return True
        End Function

        ''' <summary>
        ''' Gets the target folder path from a menu item tag
        ''' </summary>
        ''' <param name="tagPath">Path from MenuTag</param>
        ''' <returns>Directory path for paste operation</returns>
        Public Function GetPasteTargetFolder(tagPath As String) As String
            Return IO.Path.GetDirectoryName(tagPath.TrimEnd("\"c))
        End Function

        ''' <summary>
        ''' Creates a set of clipboard item paths for quick lookup
        ''' </summary>
        ''' <param name="clipInfo">Clipboard file information</param>
        ''' <returns>HashSet of normalized paths</returns>
        Friend Function CreateClipboardPathSet(clipInfo As ClipboardFileInfo) As HashSet(Of String)
            If clipInfo.Files Is Nothing Then
                Return New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            End If
            Return New HashSet(Of String)(
                clipInfo.Files.Select(Function(p) p.TrimEnd("\"c)),
                StringComparer.OrdinalIgnoreCase)
        End Function

    End Module

End Namespace ' QL
