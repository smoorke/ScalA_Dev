Imports System.Collections.Concurrent
Imports System.Runtime.InteropServices

Namespace QL

    ''' <summary>
    ''' Module for caching and retrieving file/folder icons for QuickLaunch
    ''' </summary>
    Public Module QLIconCache

        ''' <summary>
        ''' Cache of file path to icon bitmap
        ''' </summary>
        Friend ReadOnly IconCache As New ConcurrentDictionary(Of String, Bitmap)

        ''' <summary>
        ''' Cache of URL protocol scheme to default browser icon
        ''' </summary>
        Public ReadOnly DefURLicons As New ConcurrentDictionary(Of String, Bitmap)

        ''' <summary>
        ''' Cached folder icon (lazy initialized)
        ''' </summary>
        Private _folderIcon As Bitmap

        ''' <summary>
        ''' Cached folder icon with shortcut overlay (lazy initialized)
        ''' </summary>
        Private _folderIconWithOverlay As Bitmap

        ''' <summary>
        ''' Gets the default folder icon
        ''' </summary>
        Public ReadOnly Property FolderIcon As Bitmap
            Get
                If _folderIcon Is Nothing Then
                    _folderIcon = GetIconFromFile(FileIO.SpecialDirectories.Temp & "\", deffolder:=True, supressCacheMiss:=True)
                End If
                Return _folderIcon
            End Get
        End Property

        ''' <summary>
        ''' Gets the folder icon with shortcut overlay
        ''' </summary>
        Public ReadOnly Property FolderIconWithOverlay As Bitmap
            Get
                If _folderIconWithOverlay Is Nothing Then
                    _folderIconWithOverlay = FolderIcon.addOverlay(My.Resources.shortcutOverlay, True)
                End If
                Return _folderIconWithOverlay
            End Get
        End Property

        ''' <summary>
        ''' Gets icon from cache, applying transparency for hidden items and overlay for shortcuts
        ''' </summary>
        ''' <param name="qli">The QuickLaunch item info</param>
        ''' <returns>The icon bitmap, or Nothing on error</returns>
        Public Function GetIconFromCache(qli As QLInfo) As Bitmap
            Try
                Return IconCache.GetOrAdd(qli.path, AddressOf GetIconFromFile) _
                                .AsTransparent(If(qli.hidden, If(My.Settings.DarkMode, 0.4, 0.5), 1)) _
                                .addOverlay(If(My.Settings.QLResolveLnk AndAlso ((qli.path.ToLower.EndsWith(".lnk") AndAlso qli.target?.EndsWith("\"c)) OrElse qli.pointsToDir), My.Resources.shortcutOverlay, Nothing), True)
            Catch ex As Exception
                dBug.Print($"GetIcon Exception {ex.Message}")
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Evicts an item from the icon cache
        ''' </summary>
        ''' <param name="item">The file path to evict</param>
        ''' <returns>True if item was removed</returns>
        Public Function EvictIconCacheItem(item As String) As Boolean
            If item.ToLower.EndsWith(".url") Then DefURLicons.Clear()
            Return IconCache.TryRemove(item, Nothing)
        End Function

        ''' <summary>
        ''' Evicts icons that are not watched file types (.lnk, .exe, .url) or folders
        ''' </summary>
        Public Sub EvictNonWatchedIcons()
            For Each pth In IconCache.Keys.ToList
                If Not {".lnk", ".exe", ".url"}.Contains(IO.Path.GetExtension(pth).ToLower) AndAlso Not pth.EndsWith("\") Then
                    IconCache.TryRemove(pth, Nothing)
                End If
            Next
        End Sub

        ''' <summary>
        ''' Gets icon from file system using shell APIs
        ''' </summary>
        ''' <param name="PathName">Path to the file or folder (folders end with \)</param>
        ''' <param name="deffolder">Use default folder icon instead of actual</param>
        ''' <param name="supressCacheMiss">Don't log cache miss debug messages</param>
        ''' <returns>Icon bitmap</returns>
        Public Function GetIconFromFile(PathName As String, Optional deffolder As Boolean = False, Optional supressCacheMiss As Boolean = False) As Bitmap
#If DEBUG Then
            If Not supressCacheMiss Then dBug.Print($"iconCahceMiss: {PathName}")
#End If

            Dim bm As Bitmap = Nothing
            Dim fi As New SHFILEINFOW
            Dim ico As Icon = Nothing

            If PathName.EndsWith("\") Then

                Dim flags As UInteger = SHGFI_ICON Or SHGFI_SMALLICON
                If deffolder Then flags = flags Or SHGFI_USEFILEATTRIBUTES

                SHGetFileInfoW(PathName, FILE_ATTRIBUTE_DIRECTORY, fi, Marshal.SizeOf(fi), flags)
                If fi.hIcon = IntPtr.Zero Then
                    dBug.Print("hIcon empty: " & Marshal.GetLastWin32Error)
                    Throw New Exception
                End If
                ico = Icon.FromHandle(fi.hIcon)
                bm = ico.ToBitmap
                DestroyIcon(ico.Handle)
                Return bm

            Else 'not a folder
                If PathName.ToLower.EndsWith(".url") Then
                    Try

                        Dim iconIndex As Integer = 0
                        Dim iconFilePath As String = Nothing
                        Dim URLTarget As String = Nothing


                        For Each line In IO.File.ReadLines(PathName)
                            If line.StartsWith("IconFile=", StringComparison.OrdinalIgnoreCase) Then
                                iconFilePath = line.Substring("IconFile=".Length).Trim()
                            ElseIf line.StartsWith("IconIndex=", StringComparison.OrdinalIgnoreCase) Then
                                Integer.TryParse(line.Substring("IconIndex=".Length).Trim(), iconIndex)
                            ElseIf line.StartsWith("URL=", StringComparison.OrdinalIgnoreCase) Then
                                URLTarget = line.Substring("URL=".Length).Trim()
                            End If
                        Next

                        If String.IsNullOrEmpty(iconFilePath) AndAlso Not String.IsNullOrEmpty(URLTarget) AndAlso URLTarget.Substring(0, 8).Contains("://") AndAlso URLTarget.Length > 3 Then
                            Dim proto = URLTarget.Split(":")(0)
                            If {"https", "http", "ftp"}.Contains(proto) Then bm = GetDefaultBrowserIcon(proto)
                            If bm IsNot Nothing Then Return bm
                        End If

                        If Not String.IsNullOrEmpty(iconFilePath) Then
                            Dim hIcoA As IntPtr() = {IntPtr.Zero}
                            Dim ret As Integer = ExtractIconEx(iconFilePath, iconIndex, Nothing, hIcoA, 1)
                            Debug.Print($"ExtractIconEx {ret} ""{iconFilePath}"" {iconIndex} ""{PathName}""")
                            ico = Icon.FromHandle(hIcoA(0))

                            bm = ico.ToBitmap
                            DestroyIcon(ico.Handle)

                            If bm IsNot Nothing Then Return bm
                        Else
                            Dim ret = SHGetFileInfoW(PathName, FILE_ATTRIBUTE_NORMAL, fi, Marshal.SizeOf(fi), SHGFI_USEFILEATTRIBUTES Or SHGFI_SMALLICON Or SHGFI_ICON)
                            ico = Icon.FromHandle(fi.hIcon)
                            bm = ico.ToBitmap
                            DestroyIcon(ico.Handle)
                            If bm IsNot Nothing Then Return bm
                        End If

                    Catch ex As Exception
                        Debug.Print("Failed to load .url icon: " & ex.Message)
                    End Try
                End If

                Dim list As IntPtr = SHGetFileInfoW(PathName, 0, fi, Marshal.SizeOf(fi), SHGFI_SYSICONINDEX Or SHGFI_SMALLICON)

                If list = IntPtr.Zero Then
                    Dim lastError = Marshal.GetLastWin32Error()
                    dBug.Print($"SHGetFileInfoW list empty: {lastError}")
                End If
                Dim hIcon As IntPtr = ImageList_GetIcon(list, fi.iIcon, 0)
                ImageList_Destroy(list)
                If hIcon = IntPtr.Zero Then
                    Dim lastError = Marshal.GetLastWin32Error()
                    dBug.Print($"ImageList_GetIcon empty: {lastError}")
                End If
                Try
                    ico = Icon.FromHandle(hIcon)
                    bm = ico.ToBitmap

                Catch ex As Exception
                    bm = Nothing
                End Try

            End If
            DestroyIcon(If(ico?.Handle, IntPtr.Zero))
            ico?.Dispose()

            Return bm
        End Function

        ''' <summary>
        ''' Gets the default browser icon for a URL protocol (http, https, ftp)
        ''' </summary>
        ''' <param name="urlScheme">The URL scheme (e.g., "https")</param>
        ''' <returns>Browser icon bitmap, or Nothing if not found</returns>
        Public Function GetDefaultBrowserIcon(urlScheme As String) As Bitmap
            urlScheme = urlScheme.ToLowerInvariant()
            Debug.Print($"getDefBrowserIcon {urlScheme} {DefURLicons.Keys.Contains(urlScheme)}")
            Return DefURLicons.GetOrAdd(urlScheme,
                               Function(proto)
                                   dBug.Print($"DefURLicon cachemiss {proto}")

                                   Dim progId As String = Nothing

                                   ' 1. Try UserChoice (per-user)
                                   Using key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(GetUserChoiceKeyPath(proto))
                                       If key IsNot Nothing Then
                                           progId = TryCast(key.GetValue("ProgId"), String)
                                       End If
                                   End Using

                                   If String.IsNullOrEmpty(progId) Then Return Nothing

                                   RegistryWatcher.Init(proto)

                                   ' 3. Get DefaultIcon for the ProgId
                                   Using iconKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(progId & "\DefaultIcon")
                                       If iconKey IsNot Nothing Then
                                           Dim iconValue = TryCast(iconKey.GetValue(Nothing), String)
                                           If Not String.IsNullOrEmpty(iconValue) Then
                                               iconValue = iconValue.Trim()
                                               Dim idx As Integer = 0
                                               Dim pth As String
                                               If iconValue.Contains(",") Then
                                                   Dim parts() As String = iconValue.Split(",")
                                                   pth = parts(0).Trim
                                                   Integer.TryParse(parts(1).Trim, idx)
                                               Else
                                                   pth = iconValue
                                               End If
                                               pth = pth.Trim(""""c)

                                               Dim icns() As IntPtr = {IntPtr.Zero}
                                               Dim ret = ExtractIconEx(pth, idx, {IntPtr.Zero}, icns, 1)

                                               If ret = 0 OrElse icns(0) = IntPtr.Zero Then Return Nothing

                                               Dim icn As Icon = Icon.FromHandle(icns(0))
                                               Dim bm = icn.ToBitmap
                                               DestroyIcon(icn.Handle)
                                               Return bm
                                           End If
                                       End If
                                   End Using

                                   Return Nothing
                               End Function)
        End Function

    End Module

End Namespace ' QL
