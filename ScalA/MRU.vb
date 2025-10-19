Imports System.Collections.Concurrent


Namespace MRU
    Module MRU
        ' ===========================================================
        ' Configuration
        ' ===========================================================
        Public MRUFilePath As String = IO.Path.Combine(FileIO.SpecialDirectories.Temp, "ScalA\MRU.txt")
        Public Const MaxMRUItems As Integer = 250
        Public Const MRUStabilityTimeoutMs As Integer = 1000
        Public Const MRUStabilityCheckIntervalMs As Integer = 100
        Public Const MaxPathLength As Integer = 260
        Private Const MRUDebounceDelayMs As Integer = 300

        ' ===========================================================
        ' Public state
        ' ===========================================================
        Public MRUList As New List(Of String)
        Public MRULock As New Object()
        Public MRUWatcher As IO.FileSystemWatcher
        Public InitDone As Boolean = False
        Private MRULastSaveTime As DateTime
        Private MRUDebounceTimer As System.Threading.Timer

        ' ===========================================================
        ' Initialize MRU
        ' ===========================================================
        Public Sub InitializeMRU()
            SyncLock MRULock
                If InitDone Then Return

                Dim dir As String = IO.Path.GetDirectoryName(MRUFilePath)
                If Not IO.Directory.Exists(dir) Then IO.Directory.CreateDirectory(dir)

                If Not IO.File.Exists(MRUFilePath) Then IO.File.WriteAllText(MRUFilePath, "")

                Load()
                Save() ' cleans + ensures valid state

                MRUWatcher = New IO.FileSystemWatcher()
                MRUWatcher.Path = IO.Path.GetDirectoryName(MRUFilePath)
                MRUWatcher.Filter = IO.Path.GetFileName(MRUFilePath)
                MRUWatcher.NotifyFilter = IO.NotifyFilters.LastWrite Or IO.NotifyFilters.Size Or IO.NotifyFilters.FileName
                AddHandler MRUWatcher.Changed, AddressOf OnMRUFileChanged
                AddHandler MRUWatcher.Created, AddressOf OnMRUFileChanged
                AddHandler MRUWatcher.Renamed, AddressOf OnMRUFileChanged
                AddHandler MRUWatcher.Deleted, AddressOf OnMRUFileChanged
                MRUWatcher.EnableRaisingEvents = True

                InitDone = True
                dBug.Print("InitializeMRU: Completed. MRU count=" & MRUList.Count)
            End SyncLock
        End Sub

        ' ===========================================================
        ' Load MRU from file (.lnk only, valid length/existence)
        ' ===========================================================
        Public Sub Load()
            SyncLock MRULock
                Try
                    If Not IO.File.Exists(MRUFilePath) Then Exit Sub

                    Dim lines() As String = IO.File.ReadAllLines(MRUFilePath)
                    MRUList.Clear()

                    For Each line In lines
                        Dim entry As String = line.Trim()
                        If entry.Length > 0 AndAlso
                           entry.Length <= MaxPathLength AndAlso
                           entry.ToLowerInvariant().EndsWith(".lnk") AndAlso
                           IO.File.Exists(entry) Then

                            MRUList.Add(entry)
                        End If
                    Next

                    dBug.Print("LoadMRU: Loaded " & MRUList.Count & " valid entries.")
                Catch ex As Exception
                    dBug.Print("LoadMRU error: " & ex.Message)
                End Try
            End SyncLock
        End Sub

        ' ===========================================================
        ' Save MRU to file (auto-evict invalid/missing)
        ' ===========================================================
        Public Sub Save()
            SyncLock MRULock
                Try
                    Dim removedCount As Integer = 0
                    For i As Integer = MRUList.Count - 1 To 0 Step -1
                        Dim entry = MRUList(i)
                        If Not entry.ToLowerInvariant().EndsWith(".lnk") OrElse
                       entry.Length > MaxPathLength OrElse
                       Not IO.File.Exists(entry) Then

                            MRUList.RemoveAt(i)
                            removedCount += 1
                        End If
                    Next

                    MRULastSaveTime = DateTime.UtcNow
                    IO.File.WriteAllLines(MRUFilePath, MRUList)

                    dBug.Print("SaveMRU: Saved " & MRUList.Count &
                            " entries." & If(removedCount > 0, " (Evicted " & removedCount & " invalid/missing)", ""))
                Catch ex As Exception
                    dBug.Print("SaveMRU error: " & ex.Message)
                End Try
            End SyncLock
        End Sub

        ' ===========================================================
        ' Add new item to MRU (validate .lnk, existence, length)
        ' ===========================================================
        Public Sub Add(ByVal path As String)

            If String.IsNullOrWhiteSpace(path) Then Exit Sub
            If path.Length > MaxPathLength Then
                dBug.Print("AddToMRU: Ignored overlong path (" & path.Length & " chars): " & path)
                Exit Sub
            End If
            If Not path.ToLowerInvariant().EndsWith(".lnk") Then
                dBug.Print("AddToMRU: Ignored non-.lnk file: " & path)
                Exit Sub
            End If
            If Not IO.File.Exists(path) Then
                dBug.Print("AddToMRU: Ignored non-existent file: " & path)
                Exit Sub
            End If

            SyncLock MRULock
                If Not InitDone Then InitializeMRU()
                If MRUList.Contains(path) Then MRUList.Remove(path)
                MRUList.Insert(0, path)
                If MRUList.Count > MaxMRUItems Then MRUList.RemoveRange(MaxMRUItems, MRUList.Count - MaxMRUItems)
            End SyncLock

            Save()
        End Sub

        ' ===========================================================
        ' Bump an existing .lnk to top of MRU
        ' Does nothing if not already in MRU
        ' ===========================================================
        Public Sub Bump(lnkPath As String)
            If String.IsNullOrWhiteSpace(lnkPath) Then Exit Sub

            SyncLock MRULock
                If Not InitDone Then InitializeMRU()

                If MRUList.Contains(lnkPath) Then
                    MRUList.Remove(lnkPath)
                    MRUList.Insert(0, lnkPath)
                    Save()
                    dBug.Print("BumpExistingToTop: bumped " & lnkPath & " to MRU top.")
                End If
            End SyncLock
        End Sub

        ' ===========================================================
        ' Parallel search returning all matching shortcuts in MRU order
        ' ===========================================================
        Public Function FindAllMatchingShortcutsParrallel(processPath As String, processArgs As String, workDir As String) As List(Of String)
            Dim results As New List(Of String)

            If String.IsNullOrWhiteSpace(processPath) Then Return results

            ' Ensure MRU is initialized
            SyncLock MRULock
                If Not InitDone Then InitializeMRU()
            End SyncLock

            Dim matches As New ConcurrentBag(Of Tuple(Of Integer, String))

            Try
                ' Enumerate MRU with index to preserve order
                Parallel.ForEach(MRUList.Where(Function(l) IO.File.Exists(l)).Select(Function(p, i) Tuple.Create(i, p)),
                                 Sub(item)
                                     Try
                                         Dim sli As New ShellLinkInfo()
                                         If sli.Load(item.Item2) Then
                                             Dim targetMatch = String.Compare(sli.TargetPath?.Trim(), processPath?.Trim(), True) = 0
                                             Dim argsMatch = String.Compare(sli.Arguments?.Trim(), processArgs?.Trim(), True) = 0
                                             Dim workDirMatch = String.Compare(sli.WorkingDirectory?.Trim().TrimEnd("\"c), workDir?.Trim().TrimEnd("\"c), True) = 0

                                             If targetMatch AndAlso argsMatch AndAlso workDirMatch Then
                                                 matches.Add(item)
                                             End If
                                         End If
                                     Catch ex As Exception
                                         dBug.Print("Parallel FindAllMatchingShortcuts failed for " & item.Item2 & ": " & ex.Message)
                                     End Try
                                 End Sub)

                ' Sort matches by MRU index to preserve order
                results = matches.OrderBy(Function(m) m.Item1).Select(Function(m) m.Item2).ToList()

            Catch ex As Exception
                dBug.Print("Parallel FindAllMatchingShortcuts error: " & ex.Message)
            End Try

            Return results
        End Function

        ' ===========================================================
        ' Sequential search returning all matching shortcuts in MRU order
        ' ===========================================================
        Public Function FindAllMatchingShortcutsSequential(processPath As String, processArgs As String, workDir As String) As List(Of String)
            Dim results As New List(Of String)

            If String.IsNullOrWhiteSpace(processPath) Then Return results

            ' Ensure MRU is initialized
            SyncLock MRULock
                If Not InitDone Then InitializeMRU()
            End SyncLock

            Try
                ' Iterate MRUList sequentially
                For Each lnkPath In MRUList.Where(Function(l) IO.File.Exists(l))
                    Try
                        Dim sli As New ShellLinkInfo()
                        If sli.Load(lnkPath) Then
                            Dim targetMatch = String.Compare(sli.TargetPath?.Trim(), processPath?.Trim(), True) = 0
                            Dim argsMatch = String.Compare(sli.Arguments?.Trim(), processArgs?.Trim(), True) = 0
                            Dim workDirMatch = String.Compare(sli.WorkingDirectory?.Trim().TrimEnd("\"c), workDir?.Trim().TrimEnd("\"c), True) = 0

                            If targetMatch AndAlso argsMatch AndAlso workDirMatch Then
                                results.Add(lnkPath)
                            End If
                        End If
                    Catch ex As Exception
                        dBug.Print("FindAllMatchingShortcutsSequential failed for " & lnkPath & ": " & ex.Message)
                    End Try
                Next
            Catch ex As Exception
                dBug.Print("FindAllMatchingShortcutsSequential error: " & ex.Message)
            End Try

            Return results
        End Function

        ' ===========================================================
        ' Returns the first MRU .lnk that matches the running process
        ' ===========================================================
        Public Function FindFirstMatchingShortcut(processPath As String, processArgs As String, workDir As String) As String
            If String.IsNullOrWhiteSpace(processPath) Then Return Nothing

            ' Ensure MRU is initialized
            SyncLock MRULock
                If Not InitDone Then InitializeMRU()
            End SyncLock

            Try
                For Each lnkPath In MRUList.Where(Function(l) IO.File.Exists(l))
                    Try
                        Dim sli As New ShellLinkInfo()
                        If sli.Load(lnkPath) Then
                            Dim targetMatch = String.Compare(sli.TargetPath?.Trim(), processPath?.Trim(), True) = 0
                            Dim argsMatch = String.Compare(sli.Arguments?.Trim(), processArgs?.Trim(), True) = 0
                            Dim workDirMatch = String.Compare(sli.WorkingDirectory?.Trim().TrimEnd("\"c), workDir?.Trim().TrimEnd("\"c), True) = 0

                            If targetMatch AndAlso argsMatch AndAlso workDirMatch Then
                                Return lnkPath
                            End If
                        End If
                    Catch ex As Exception
                        dBug.Print("FindFirstMatchingShortcut failed for " & lnkPath & ": " & ex.Message)
                    End Try
                Next
            Catch ex As Exception
                dBug.Print("FindFirstMatchingShortcut error: " & ex.Message)
            End Try

            Return Nothing
        End Function

        ' ===========================================================
        ' Handle file changes (async)
        ' ===========================================================
        Private Sub OnMRUFileChanged(sender As Object, e As IO.FileSystemEventArgs)
            ' Ignore changes from own save
            If (DateTime.UtcNow - MRULastSaveTime).TotalMilliseconds < 500 Then Return

            ' Reset/start debounce timer
            If MRUDebounceTimer IsNot Nothing Then
                MRUDebounceTimer.Change(MRUDebounceDelayMs, System.Threading.Timeout.Infinite)
            Else
                MRUDebounceTimer = New System.Threading.Timer(Async Sub(state)
                                                                  Try
                                                                      ' Wait for file to stabilize
                                                                      Dim stable As Boolean = Await WaitForFileStableAsync(MRUFilePath, MRUStabilityTimeoutMs, MRUStabilityCheckIntervalMs)
                                                                      If stable Then
                                                                          Load()
                                                                          dBug.Print("FileSystemWatcher: MRU updated by external application.")
                                                                      Else
                                                                          dBug.Print("FileSystemWatcher: Timeout waiting for stable file.")
                                                                      End If
                                                                  Catch ex As Exception
                                                                      dBug.Print("Debounce timer error: " & ex.Message)
                                                                  End Try
                                                              End Sub,
                                   Nothing, MRUDebounceDelayMs, System.Threading.Timeout.Infinite)
            End If
        End Sub

        ' ===========================================================
        ' Wait for stable file (async)
        ' ===========================================================
        Private Async Function WaitForFileStableAsync(filePath As String, timeoutMs As Integer, checkIntervalMs As Integer) As Task(Of Boolean)
            Dim sw As Stopwatch = Stopwatch.StartNew()
            Dim lastWrite As DateTime = DateTime.MinValue
            Dim stableCount As Integer = 0

            Do While sw.ElapsedMilliseconds < timeoutMs
                Try
                    If Not IO.File.Exists(filePath) Then Return True
                    Dim info As New IO.FileInfo(filePath)
                    Dim currentWrite = info.LastWriteTimeUtc

                    Using fs As New IO.FileStream(filePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                        ' just test access
                    End Using

                    If currentWrite = lastWrite Then
                        stableCount += 1
                        If stableCount >= 2 Then Return True
                    Else
                        stableCount = 0
                    End If

                    lastWrite = currentWrite
                Catch ex As IO.IOException
                Catch ex As UnauthorizedAccessException
                End Try

                Await Task.Delay(checkIntervalMs)
            Loop

            Return False
        End Function

        ' ===========================================================
        ' Cleanup
        ' ===========================================================
        Public Sub CloseMRU()
            Try
                If MRUWatcher IsNot Nothing Then
                    MRUWatcher.EnableRaisingEvents = False
                    RemoveHandler MRUWatcher.Changed, AddressOf OnMRUFileChanged
                    RemoveHandler MRUWatcher.Created, AddressOf OnMRUFileChanged
                    RemoveHandler MRUWatcher.Renamed, AddressOf OnMRUFileChanged
                    RemoveHandler MRUWatcher.Deleted, AddressOf OnMRUFileChanged
                    MRUWatcher.Dispose()
                End If
                dBug.Print("CloseMRU: resources disposed.")
            Catch ex As Exception
                dBug.Print("CloseMRU error: " & ex.Message)
            End Try
        End Sub

    End Module
End Namespace
