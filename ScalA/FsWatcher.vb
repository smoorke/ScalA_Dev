Imports System.Collections.Concurrent
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.Win32

Module FsWatcher

    Private usingLatestReg As Boolean? = Nothing
    Public Function GetUserChoiceKeyPath(protocol As String) As String
        Dim suff As String = ""
        Dim base = $"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\{protocol}\"
        If usingLatestReg IsNot Nothing Then
            If usingLatestReg Then
                suff = "UserChoiceLatest\ProgId"
            Else
                suff = "UserChoice"
            End If
        End If
        If String.IsNullOrEmpty(suff) Then
            Using latestKey = Registry.CurrentUser.OpenSubKey(base & "UserChoiceLatest")
                If latestKey IsNot Nothing Then
                    suff = "UserChoiceLatest\ProgId"
                    usingLatestReg = True
                Else
                    suff = "UserChoice"
                    usingLatestReg = False
                End If
            End Using
        End If
        Return base & suff
    End Function

    Public Class RegistryWatcher
        Implements IDisposable

        Shared ReadOnly regWatchers As New ConcurrentDictionary(Of String, RegistryWatcher)
        Public Shared ReadOnly regWatcherLock As New Object()

        Public Shared Sub Init(proto As String)
            Dim key As String = proto.ToLowerInvariant()
            If regWatchers.ContainsKey(key) Then Exit Sub

            SyncLock regWatcherLock
                If Not regWatchers.ContainsKey(key) Then
                    Dim watcher As New RegistryWatcher(key)
                    AddHandler watcher.RegistryChanged, AddressOf OnRegistryChanged
                    watcher.Start()
                    regWatchers.TryAdd(key, watcher)
                    dBug.Print($"Started watcher for protocol: {key}")
                End If
            End SyncLock
        End Sub

        Public Shared Sub OnRegistryChanged(proto As String)
            dBug.Print($"Registry changed for protocol: {proto}")
            FrmMain.DefURLicons.TryRemove(proto, Nothing)
            If FrmMain.DefURLicons.Keys.Contains(proto) Then Throw New Exception

            For Each key In FrmMain.iconCache.Keys.Where(Function(k) k.EndsWith(".url"))
                FrmMain.iconCache.TryRemove(key, Nothing)
            Next
            If FrmMain.iconCache.Keys.Any(Function(k) k.ToLower.EndsWith(".url")) Then Throw New Exception

        End Sub

        Private ReadOnly keyPath As String
        Private ReadOnly protocol As String
        Private watcherThread As Thread
        Private running As Boolean

        Public Event RegistryChanged(protocol As String)

        Public Sub New(protocol As String)
            Me.protocol = protocol.ToLowerInvariant()
            keyPath = GetUserChoiceKeyPath(Me.protocol)
        End Sub

        Public Sub Start()
            If watcherThread IsNot Nothing Then Return
            running = True
            watcherThread = New Thread(AddressOf WatchLoop) With {.IsBackground = True, .Priority = ThreadPriority.Lowest}
            watcherThread.Start()
        End Sub

        Private Sub WatchLoop()
            Using key = Registry.CurrentUser.OpenSubKey(keyPath, False)
                If key Is Nothing Then Return
                Dim handle = key.Handle.DangerousGetHandle()
                While running
                    RegNotifyChangeKeyValue(handle, False, RegChangeNotifyFilter.Value, IntPtr.Zero, False)
                    RaiseEvent RegistryChanged(protocol)
                End While
            End Using
        End Sub

        Private disposedValue As Boolean = False

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' No need to stop or join the thread
                    ' If you had other managed resources, dispose them here
                End If

                disposedValue = True
            End If
        End Sub

        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub

        <Flags>
        Private Enum RegChangeNotifyFilter As UInteger
            Name = 1
            Attributes = 2
            Value = 4
            Security = 8
        End Enum

        <DllImport("advapi32.dll", SetLastError:=True)>
        Private Shared Function RegNotifyChangeKeyValue(hKey As IntPtr, bWatchSubtree As Boolean, dwNotifyFilter As RegChangeNotifyFilter, hEvent As IntPtr, fAsynchronous As Boolean) As Integer : End Function
    End Class

    Public ReadOnly fsWatchers As New List(Of System.IO.FileSystemWatcher)

    Public ReadOnly ResolvedLinkWatchers As New Concurrent.ConcurrentDictionary(Of String, List(Of IO.FileSystemWatcher))

    Public Sub addLinkWatcher(pth As String)
        Task.Run(Sub()
                     If Not pth.Contains(My.Settings.links) AndAlso Not ResolvedLinkWatchers.Keys.Any(Function(wl As String) pth.Contains(wl)) Then
                         dBug.Print($"addLinkWatcher {pth}")
                         Dim ws As New List(Of FileSystemWatcher)
                         InitWatchers(pth, ws)
                         ResolvedLinkWatchers.TryAdd(pth, ws)
                     End If
                 End Sub)
    End Sub

    Public Sub removeLinkWatcher(pth As String)
        Dim removed As List(Of IO.FileSystemWatcher) = Nothing
        If ResolvedLinkWatchers.TryRemove(pth, removed) Then
            For Each watcher In removed
                Try
                    watcher.EnableRaisingEvents = False
                    watcher.Dispose()
                Catch ex As Exception
                    dBug.Print($"Error disposing watcher: {ex.Message}")
                End Try
            Next
            dBug.Print($"removeLinkWatcher {pth}")
        End If
    End Sub

    Public Sub UpdateWatchers(newPath As String)
        dBug.Print("updateWatchers")
        For Each w As System.IO.FileSystemWatcher In fsWatchers
            w.EnableRaisingEvents = False
            w.Path = newPath
            w.EnableRaisingEvents = True
        Next
        For Each ws As FileSystemWatcher In ResolvedLinkWatchers.Values.SelectMany(Function(fws As List(Of IO.FileSystemWatcher)) fws)
            ws.EnableRaisingEvents = False
            ws.Dispose()
        Next
        ResolvedLinkWatchers.Clear()
    End Sub
    Public Sub InitWatchers(path As String, ByRef watchers As List(Of IO.FileSystemWatcher))
        dBug.Print($"initWatchers {path}")
        For Each w As System.IO.FileSystemWatcher In watchers
            w.EnableRaisingEvents = False
            w.Dispose()
        Next
        watchers.Clear()

        Dim iniWatcher As New System.IO.FileSystemWatcher(path) With {
            .NotifyFilter = System.IO.NotifyFilters.LastWrite,
            .Filter = "desktop.ini",
            .IncludeSubdirectories = True
        }

        AddHandler iniWatcher.Changed, AddressOf OnChanged
        iniWatcher.EnableRaisingEvents = True

        watchers.Add(iniWatcher)



        Dim lnkWatcher As New System.IO.FileSystemWatcher(path) With {
            .NotifyFilter = System.IO.NotifyFilters.LastWrite Or
                            System.IO.NotifyFilters.FileName,
            .Filter = "*.lnk",
            .IncludeSubdirectories = True
        }

        AddHandler lnkWatcher.Renamed, AddressOf OnRenamed
        AddHandler lnkWatcher.Changed, AddressOf OnChanged
        lnkWatcher.EnableRaisingEvents = True

        watchers.Add(lnkWatcher)



        Dim urlWatcher As New System.IO.FileSystemWatcher(path) With {
            .NotifyFilter = System.IO.NotifyFilters.LastWrite Or
                            System.IO.NotifyFilters.FileName,
            .Filter = "*.url",
            .IncludeSubdirectories = True
        }

        AddHandler urlWatcher.Renamed, AddressOf OnRenamed
        AddHandler urlWatcher.Changed, AddressOf OnChanged
        urlWatcher.EnableRaisingEvents = True

        watchers.Add(urlWatcher)



        Dim dirWatcher As New System.IO.FileSystemWatcher(path) With {
            .NotifyFilter = System.IO.NotifyFilters.DirectoryName,
            .IncludeSubdirectories = True
        }

        AddHandler dirWatcher.Renamed, AddressOf OnRenamedDir
        AddHandler dirWatcher.Deleted, AddressOf OnDeleteDir
        dirWatcher.EnableRaisingEvents = True

        watchers.Add(dirWatcher)


    End Sub

    Private ReadOnly iconCache As ConcurrentDictionary(Of String, Bitmap) = FrmMain.iconCache

    Private Sub OnDeleteDir(sender As System.IO.FileSystemWatcher, e As System.IO.FileSystemEventArgs)
        dBug.Print($"Delete Dir: {sender.NotifyFilter}")
        dBug.Print($"      Type: {e.ChangeType}")
        dBug.Print($"      Path: {e.FullPath}")

        If e.ChangeType = IO.WatcherChangeTypes.Deleted Then
            For Each key In iconCache.Keys.Where(Function(k As String) k.StartsWith(e.FullPath & "\"))
                iconCache.TryRemove(key, Nothing)
            Next
        End If
    End Sub
    Private Sub OnRenamedDir(sender As System.IO.FileSystemWatcher, e As System.IO.RenamedEventArgs)
        dBug.Print($"Renamed Dir: {sender.NotifyFilter}")
        dBug.Print($"        Old: {e.OldFullPath}")
        dBug.Print($"        New: {e.FullPath}")

        For Each key In iconCache.Keys.Where(Function(k) k.StartsWith(e.OldFullPath & "\"))
            Dim item As Bitmap = Nothing
            If iconCache.TryRemove(key, item) Then iconCache.TryAdd(key.Replace(e.OldFullPath & "\", e.FullPath & "\"), item)
        Next
    End Sub
    Private Sub OnRenamed(sender As System.IO.FileSystemWatcher, e As System.IO.RenamedEventArgs)
        dBug.Print($"Renamed File: {sender.NotifyFilter}")
        dBug.Print($"         Old: {e.OldFullPath}")
        dBug.Print($"         New: {e.FullPath}")

        Dim item As Bitmap = Nothing
        If iconCache.TryRemove(e.OldFullPath, item) Then iconCache.TryAdd(e.FullPath, item)

    End Sub

    Private Sub OnChanged(sender As System.IO.FileSystemWatcher, e As System.IO.FileSystemEventArgs)
        If e.ChangeType = System.IO.WatcherChangeTypes.Changed Then
            dBug.Print(sender.ToString)
            dBug.Print($"Changed: {e.FullPath}")
            If e.FullPath.ToLower.EndsWith("desktop.ini") Then
                iconCache.TryRemove(e.FullPath.Substring(0, e.FullPath.LastIndexOf("\") + 1), Nothing)
            End If
            If hideExt.Contains(System.IO.Path.GetExtension(e.FullPath).ToLower) Then
                iconCache.TryRemove(e.FullPath, Nothing)
            End If
        End If
    End Sub

End Module
