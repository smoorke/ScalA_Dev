Imports System.IO.MemoryMappedFiles
Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.Collections.Generic
Imports System.Diagnostics

Public Class WindowCornerManager
    Private Const MMF_NAME As String = "AstoniaProcess_CornerPID_MMF"
    Private Const MUTEX_NAME As String = "AstoniaProcess_CornerPID_Mutex"

    ' Generous capacity supporting up to 256 concurrent wrapper instances (takes only 1KB of memory)
    Private Const MAX_SLOTS As Integer = 256
    Private Const STRUCT_SIZE As Integer = 4 + (MAX_SLOTS * 4)

    Private Shared ReadOnly SharedFile As MemoryMappedFile = MemoryMappedFile.CreateOrOpen(MMF_NAME, STRUCT_SIZE)

    <DllImport("dwmapi.dll")>
    Private Shared Function DwmGetWindowAttribute(hwnd As IntPtr, dwAttribute As Integer, ByRef pvAttribute As Integer, cbAttribute As Integer) As Integer
    End Function

    <DllImport("dwmapi.dll")>
    Private Shared Function DwmSetWindowAttribute(hwnd As IntPtr, dwAttribute As Integer, ByRef pvAttribute As Integer, cbAttribute As Integer) As Integer
    End Function

    Private Const DWMWA_WINDOW_CORNER_PREFERENCE As Integer = 33

    Public Shared Sub ReduceCorners(clients As IEnumerable(Of AstoniaProcess), bReduce As Boolean)
        If clients Is Nothing Then Exit Sub
        If Environment.OSVersion.Version < New Version(10, 0, 22000) Then Exit Sub
        Task.Run(Sub()


                     Using mutex As New Mutex(False, MUTEX_NAME)
                         Dim hasOwnership As Boolean = False
                         Try
                             hasOwnership = mutex.WaitOne(2000)
                             If Not hasOwnership Then
                                 Debug.Print("ReduceCorners: Could not acquire Mutex lock!")
                                 Exit Sub
                             End If

                             Using accessor As MemoryMappedViewAccessor = SharedFile.CreateViewAccessor(0, STRUCT_SIZE)

                                 ' 1. Read existing PIDs and prune any dead/crashed processes
                                 Dim activePids As New List(Of Integer)()
                                 Dim count As Integer = accessor.ReadInt32(0)

                                 ' Safety check against corruption or out-of-bounds
                                 If count < 0 OrElse count > MAX_SLOTS Then count = 0

                                 For i As Integer = 0 To count - 1
                                     Dim pid As Integer = accessor.ReadInt32(4 + (i * 4))
                                     If Not HasExitedSafe(pid) Then
                                         activePids.Add(pid)
                                     End If
                                 Next

                                 ' 2. Update current process presence based on bReduce
                                 Dim isCurrentlyInList As Boolean = activePids.Contains(scalaPID)

                                 If bReduce AndAlso Not isCurrentlyInList Then
                                     If activePids.Count < MAX_SLOTS Then
                                         activePids.Add(scalaPID)
                                     End If
                                 ElseIf Not bReduce AndAlso isCurrentlyInList Then
                                     activePids.Remove(scalaPID)
                                 End If

                                 ' 3. Write cleaned list back to shared memory
                                 accessor.Write(0, activePids.Count)

                                 ' Clear remaining slots to keep memory clean
                                 For i As Integer = 0 To MAX_SLOTS - 1
                                     Dim pidToWrite As Integer = If(i < activePids.Count, activePids(i), 0)
                                     accessor.Write(4 + (i * 4), pidToWrite)
                                 Next

                                 accessor.Flush()

                                 Debug.Print($"Active healthy wrapper instances requesting rounded corners: {activePids.Count}")

                                 ' 4. Determine target preference ("True wins" if any valid instance wants it)
                                 Dim effectiveReduce As Boolean = (activePids.Count > 0)
                                 Dim targetPref As Integer = If(effectiveReduce, 3, 0)

                                 ' 5. Apply to all clients
                                 For Each client As AstoniaProcess In clients
                                     Dim hwnd As IntPtr = client.MainWindowHandle
                                     If hwnd <> IntPtr.Zero Then
                                         Dim currentPref As Integer = -1
                                         If DwmGetWindowAttribute(hwnd, DWMWA_WINDOW_CORNER_PREFERENCE, currentPref, 4) = 0 Then
                                             If currentPref <> targetPref Then
                                                 DwmSetWindowAttribute(hwnd, DWMWA_WINDOW_CORNER_PREFERENCE, targetPref, 4)
                                             End If
                                         End If
                                     End If
                                 Next

                             End Using

                         Catch ex As Exception
                             Debug.Print($"Error in ReduceCorners: {ex.Message}")
                         Finally
                             If hasOwnership Then
                                 mutex.ReleaseMutex()
                             End If
                         End Try
                     End Using
                 End Sub)
    End Sub
End Class