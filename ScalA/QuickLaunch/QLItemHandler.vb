Namespace QL

    ''' <summary>
    ''' Module for QuickLaunch item open/launch helpers
    ''' </summary>
    Public Module QLItemHandler

        ''' <summary>
        ''' Result of preparing an item for launch
        ''' </summary>
        Public Structure LaunchInfo
            ''' <summary>
            ''' The path to launch
            ''' </summary>
            Public Path As String

            ''' <summary>
            ''' Working directory for the process
            ''' </summary>
            Public WorkingDirectory As String

            ''' <summary>
            ''' Whether this is a folder to open in explorer
            ''' </summary>
            Public IsFolder As Boolean

            ''' <summary>
            ''' Whether this is a shortcut file
            ''' </summary>
            Public IsShortcut As Boolean

            ''' <summary>
            ''' The resolved target path (for shortcuts)
            ''' </summary>
            Public TargetPath As String

            ''' <summary>
            ''' Whether the target exists (for shortcuts)
            ''' </summary>
            Public TargetExists As Boolean

            ''' <summary>
            ''' Whether the shortcut points to a folder
            ''' </summary>
            Public PointsToDir As Boolean

            ''' <summary>
            ''' Error message if preparation failed
            ''' </summary>
            Public ErrorMessage As String

            ''' <summary>
            ''' Whether launch should proceed
            ''' </summary>
            Public CanLaunch As Boolean
        End Structure

        ''' <summary>
        ''' Prepares launch information for a QLInfo item
        ''' </summary>
        ''' <param name="qli">The QuickLaunch item info</param>
        ''' <returns>LaunchInfo with all necessary data</returns>
        Public Function PrepareLaunch(qli As QLInfo) As LaunchInfo
            Dim result As New LaunchInfo With {
                .Path = qli.path,
                .IsFolder = qli.isFolder,
                .IsShortcut = qli.path.ToLower().EndsWith(".lnk"),
                .TargetPath = qli.target,
                .PointsToDir = qli.pointsToDir,
                .CanLaunch = True
            }

            ' Determine working directory
            If result.IsFolder Then
                result.WorkingDirectory = result.Path.TrimEnd("\"c)
            Else
                result.WorkingDirectory = IO.Path.GetDirectoryName(result.Path)
            End If

            ' For shortcuts, check if target exists
            If result.IsShortcut AndAlso Not String.IsNullOrEmpty(result.TargetPath) Then
                result.TargetExists = IO.File.Exists(result.TargetPath) OrElse IO.Directory.Exists(result.TargetPath)
            Else
                result.TargetExists = True ' Non-shortcuts assumed valid
            End If

            Return result
        End Function

        ''' <summary>
        ''' Gets the display path for error messages
        ''' </summary>
        ''' <param name="path">Full path</param>
        ''' <returns>Shortened display path</returns>
        Public Function GetDisplayPath(path As String) As String
            Return path
        End Function

        ''' <summary>
        ''' Checks if a path should be launched (valid file/folder)
        ''' </summary>
        ''' <param name="path">Path to check</param>
        ''' <returns>True if launchable</returns>
        Public Function IsLaunchable(path As String) As Boolean
            Return IO.File.Exists(path) OrElse IO.Directory.Exists(path.TrimEnd("\"c))
        End Function

        ''' <summary>
        ''' Determines if a mouse event should trigger a launch
        ''' </summary>
        ''' <param name="button">Mouse button pressed</param>
        ''' <returns>True if this should trigger launch</returns>
        Public Function ShouldLaunch(button As MouseButtons) As Boolean
            Return button = MouseButtons.Left
        End Function

        ''' <summary>
        ''' Determines if a mouse event should show context menu
        ''' </summary>
        ''' <param name="button">Mouse button pressed</param>
        ''' <returns>True if this should show context menu</returns>
        Public Function ShouldShowContextMenu(button As MouseButtons) As Boolean
            Return button = MouseButtons.Right
        End Function

        ''' <summary>
        ''' Gets ProcessStartInfo for launching an item
        ''' </summary>
        ''' <param name="launchInfo">Prepared launch information</param>
        ''' <returns>ProcessStartInfo ready to use</returns>
        Public Function CreateProcessStartInfo(launchInfo As LaunchInfo) As ProcessStartInfo
            Return New ProcessStartInfo With {
                .FileName = launchInfo.Path,
                .WorkingDirectory = launchInfo.WorkingDirectory
            }
        End Function

        ''' <summary>
        ''' Gets ProcessStartInfo for opening a folder in explorer
        ''' </summary>
        ''' <param name="folderPath">Path to the folder</param>
        ''' <returns>ProcessStartInfo for explorer</returns>
        Public Function CreateExplorerStartInfo(folderPath As String) As ProcessStartInfo
            Return New ProcessStartInfo With {
                .FileName = folderPath.TrimEnd("\"c)
            }
        End Function

        ''' <summary>
        ''' Checks if the item is executable
        ''' </summary>
        ''' <param name="path">Path to check</param>
        ''' <returns>True if item is an executable file</returns>
        Public Function IsExecutable(path As String) As Boolean
            Dim ext As String = IO.Path.GetExtension(path).ToLower()
            Return {".exe", ".bat", ".cmd", ".com", ".msc", ".lnk", ".url"}.Contains(ext)
        End Function

        ''' <summary>
        ''' Gets all launchable items from a folder (for "Open All" functionality)
        ''' </summary>
        ''' <param name="items">Collection of menu items</param>
        ''' <returns>List of QLInfo items that can be launched</returns>
        Public Function GetLaunchableItems(items As ToolStripItemCollection) As List(Of QLInfo)
            Dim result As New List(Of QLInfo)

            For Each item As ToolStripItem In items
                If TypeOf item.Tag Is QLInfo Then
                    Dim qli As QLInfo = CType(item.Tag, QLInfo)
                    If Not qli.isFolder AndAlso IsExecutable(qli.path) Then
                        result.Add(qli)
                    End If
                End If
            Next

            Return result
        End Function

        ''' <summary>
        ''' Gets environment variable name for running as invoker (no UAC elevation)
        ''' </summary>
        Public Const COMPAT_LAYER_VAR As String = "__COMPAT_LAYER"

        ''' <summary>
        ''' Gets environment variable value for running as invoker
        ''' </summary>
        Public Const COMPAT_LAYER_VALUE As String = "RUNASINVOKER"

        ''' <summary>
        ''' Result of a launch operation
        ''' </summary>
        Public Structure LaunchResult
            Public Success As Boolean
            Public ProcessId As Integer
            Public ErrorMessage As String
        End Structure

        ''' <summary>
        ''' Launches an item with RUNASINVOKER compatibility
        ''' </summary>
        ''' <param name="launchInfo">Prepared launch information</param>
        ''' <returns>LaunchResult with success status</returns>
        Public Function LaunchItem(launchInfo As LaunchInfo) As LaunchResult
            Dim result As New LaunchResult With {.Success = False}

            Dim psi As New ProcessStartInfo With {
                .FileName = launchInfo.Path,
                .WorkingDirectory = launchInfo.WorkingDirectory
            }

            Try
                Environment.SetEnvironmentVariable(COMPAT_LAYER_VAR, COMPAT_LAYER_VALUE)
                Using proc As Process = Process.Start(psi)
                    result.Success = True
                    result.ProcessId = proc?.Id
                End Using
            Catch ex As Exception
                result.ErrorMessage = ex.Message
            Finally
                Environment.SetEnvironmentVariable(COMPAT_LAYER_VAR, Nothing)
            End Try

            Return result
        End Function

        ''' <summary>
        ''' Checks if a shortcut points to a missing directory
        ''' </summary>
        ''' <param name="sli">ShellLinkInfo for the shortcut</param>
        ''' <returns>True if shortcut points to missing directory</returns>
        Public Function IsBrokenDirectoryLink(sli As ShellLinkInfo) As Boolean
            Return sli.PointsToDir AndAlso
                   Not String.IsNullOrEmpty(sli.TargetPath) AndAlso
                   Not IO.Directory.Exists(sli.TargetPath)
        End Function

    End Module

End Namespace ' QL
