Imports System.Threading

Namespace QL

    ''' <summary>
    ''' Module for QuickLaunch directory enumeration and parsing helpers
    ''' </summary>
    Public Module QLDirectoryParser

        ''' <summary>
        ''' Number of items to show initially in large folders
        ''' </summary>
        Public Const QL_INITIAL_ITEMS As Integer = 50

        ''' <summary>
        ''' Number of items to load when "Load More" is clicked
        ''' </summary>
        Public Const QL_LOAD_MORE_BATCH As Integer = 50

        ''' <summary>
        ''' Natural string sorter for proper numeric ordering (e.g., "2" before "10")
        ''' </summary>
        Public ReadOnly NsSorter As IComparer(Of String) = New NaturalStringSorter

        ''' <summary>
        ''' File extensions that QuickLaunch displays
        ''' </summary>
        Public ReadOnly QLFilter As String() = {".lnk", ".url", ".exe"}

        ''' <summary>
        ''' File extensions whose extension should be hidden in display
        ''' </summary>
        Public ReadOnly HideExtensions As String() = {".lnk", ".url"}

        ''' <summary>
        ''' Enumerates files or directories in a path using Win32 FindFirstFileExW for better performance
        ''' </summary>
        ''' <param name="pth">Directory path to enumerate</param>
        ''' <param name="dirs">True to enumerate directories, False for files</param>
        ''' <param name="ct">Cancellation token</param>
        ''' <returns>Iterator of WIN32_FIND_DATAW structures</returns>
        Friend Iterator Function EnumerateData(pth As String, dirs As Boolean, ct As CancellationToken) As IEnumerable(Of WIN32_FIND_DATAW)

            If ct.IsCancellationRequested Then Exit Function

            Dim findData As New WIN32_FIND_DATAW()
            Dim searchOp As Integer = If(dirs, 1, 0)

            Dim hFind As IntPtr = FindFirstFileExW(IO.Path.Combine(pth, "*.*"), FINDEX_INFO_LEVELS.FindExInfoBasic, findData, searchOp, IntPtr.Zero, 0)
            If hFind = New IntPtr(-1) Then
                Return
            End If
            Try
                Do
                    If ct.IsCancellationRequested Then Exit Function

                    Dim fname As String = findData.cFileName
                    If fname <> "." AndAlso fname <> ".." Then

                        Dim isDir As Boolean = findData.dwFileAttributes.HasFlag(IO.FileAttributes.Directory)
                        If dirs = isDir Then
                            Yield findData
                        End If

                    End If

                Loop While FindNextFileW(hFind, findData)
            Finally
                FindClose(hFind)
            End Try

        End Function

        ''' <summary>
        ''' Checks if a drive incurs seek penalty (HDD vs SSD)
        ''' </summary>
        ''' <param name="driveLetter">Drive letter to check</param>
        ''' <returns>True if drive is an HDD with seek penalty</returns>
        Public Function IsDriveHDD(driveLetter As Char) As Boolean
            Return DriveIncursSeekPenalty(driveLetter)
        End Function

        ''' <summary>
        ''' Gets appropriate degree of parallelism based on drive type
        ''' </summary>
        ''' <param name="path">Path to check</param>
        ''' <param name="usableCores">Number of usable CPU cores</param>
        ''' <returns>Max degree of parallelism</returns>
        Public Function GetMaxParallelism(path As String, usableCores As Integer) As Integer
            Dim driveLetter As Char = If(path.Length >= 1, path(0), "C"c)
            Dim isHDD As Boolean = IsDriveHDD(driveLetter)
            Return If(isHDD, Math.Max(1, usableCores \ 4), Math.Max(1, usableCores - 2))
        End Function

        ''' <summary>
        ''' Creates a QLInfo structure for a directory entry
        ''' </summary>
        ''' <param name="path">Full path to the directory</param>
        ''' <param name="findData">WIN32_FIND_DATA from enumeration</param>
        ''' <returns>Populated QLInfo structure</returns>
        Friend Function CreateDirectoryInfo(path As String, findData As WIN32_FIND_DATAW) As QLInfo
            Dim fullPath As String = IO.Path.Combine(path, findData.cFileName)
            Dim attr As IO.FileAttributes = findData.dwFileAttributes
            Dim hidden As Boolean = attr.HasFlag(IO.FileAttributes.Hidden) OrElse attr.HasFlag(IO.FileAttributes.System)
            Dim displayName As String = IO.Path.GetFileName(fullPath)

            Return New QLInfo With {
                .path = fullPath & "\",
                .hidden = hidden,
                .name = displayName,
                .isFolder = True
            }
        End Function

        ''' <summary>
        ''' Creates a QLInfo structure for a file entry
        ''' </summary>
        ''' <param name="path">Full path to the containing directory</param>
        ''' <param name="findData">WIN32_FIND_DATA from enumeration</param>
        ''' <returns>Populated QLInfo structure, or Nothing if file should be skipped</returns>
        Friend Function CreateFileInfo(path As String, findData As WIN32_FIND_DATAW) As QLInfo
            Dim fullPath As String = IO.Path.Combine(path, findData.cFileName)
            Dim attr As IO.FileAttributes = findData.dwFileAttributes
            Dim hidden As Boolean = attr.HasFlag(IO.FileAttributes.Hidden) OrElse attr.HasFlag(IO.FileAttributes.System)

            ' Determine display name
            Dim displayName As String
            Dim ext As String = IO.Path.GetExtension(fullPath).ToLower()
            If HideExtensions.Contains(ext) Then
                displayName = IO.Path.GetFileNameWithoutExtension(fullPath)
            Else
                displayName = IO.Path.GetFileName(fullPath)
            End If

            Return New QLInfo With {
                .path = fullPath,
                .hidden = hidden,
                .name = displayName,
                .isFolder = False
            }
        End Function

        ''' <summary>
        ''' Checks if an item should be visible based on hidden state and settings
        ''' </summary>
        ''' <param name="qli">QLInfo for the item</param>
        ''' <param name="ctrlShiftPressed">Whether Ctrl+Shift is being held</param>
        ''' <returns>True if item should be visible</returns>
        Public Function IsItemVisible(qli As QLInfo, ctrlShiftPressed As Boolean) As Boolean
            Return Not qli.hidden OrElse ctrlShiftPressed OrElse My.Settings.QLShowHidden
        End Function

        ''' <summary>
        ''' Checks if the current executable should be excluded from the list
        ''' </summary>
        ''' <param name="fullPath">Path to check</param>
        ''' <returns>True if this is the current executable and should be excluded</returns>
        Public Function IsSelfExecutable(fullPath As String) As Boolean
            Dim currentExe As String = Environment.GetCommandLineArgs(0)
            Return IO.Path.GetFileName(fullPath) = IO.Path.GetFileName(currentExe) OrElse
                   fullPath = currentExe
        End Function

    End Module

End Namespace ' QL
