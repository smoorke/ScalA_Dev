Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.Win32

Module DPIArbMain



    Public Function HasDpiOverride(exePath As String, ByRef errorMessage As String) As Boolean

        Dim registryPath As String = "Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers"

        Dim dpiFlags As String() = {"DPIUNAWARE", "HIGHDPIAWARE", "GDIDPISCALING"}

        Dim hives As RegistryHive() = {RegistryHive.CurrentUser, RegistryHive.LocalMachine}

        Dim views As RegistryView() = {RegistryView.Registry32, RegistryView.Registry64}

        Try
            For Each hive As RegistryHive In hives
                For Each view As RegistryView In views

                    Using baseKey As RegistryKey = RegistryKey.OpenBaseKey(hive, view)

                        Using key As RegistryKey = baseKey.OpenSubKey(registryPath, False)

                            If key Is Nothing Then Continue For

                            Dim value As Object = key.GetValue(exePath)

                            If value Is Nothing Then Continue For

                            Dim flags As String() = value.ToString().Split({" "c, ControlChars.Tab}, StringSplitOptions.RemoveEmptyEntries)

                            For Each flag As String In flags
                                Dim normalized As String = flag.TrimStart("~"c)

                                If dpiFlags.Contains(normalized, StringComparer.OrdinalIgnoreCase) Then

                                    Debug.Print("DPI compatibility override found: " & hive.ToString() & " " & exePath & " [" & flag & "]")

                                    Return True
                                End If
                            Next

                        End Using
                    End Using
                Next
            Next

            Return False

        Catch ex As Exception
            errorMessage = ex.Message
            Debug.Print("Unable to verify DPI overrides: " & vbCrLf & ex.Message)
            Return True
        End Try

    End Function



    Public Sub Main()

        ' Ensure only one DPI Arbiter instance is running.
        Dim createdNew As Boolean
        Dim arbiterMutex As New Mutex(True, "Local\ScalA_DPIArbiter_Mutex", createdNew)

        If Not createdNew Then
            Debug.Print("DPI Arbiter is already running.")
            arbiterMutex.Dispose()
            Return
        End If
        Try 'finally release mutex

            ' Check if dpi override is set
            Dim exePath As String = Process.GetCurrentProcess().MainModule.FileName
            Dim errorMessage As String = Nothing
            If HasDpiOverride(exePath, errorMessage) Then
                Debug.Print("Arbiter startup aborted. DPI configuration must be corrected.")
                If String.IsNullOrEmpty(errorMessage) Then
                    Dim sei As New SHELLEXECUTEINFO With {
                            .cbSize = Marshal.SizeOf(GetType(SHELLEXECUTEINFO)),
                            .lpVerb = "properties",
                            .lpFile = exePath,
                            .nShow = SW_SHOW,
                            .fMask = SEE_MASK_INVOKEIDLIST
                    }
                    ShellExecuteEx(sei)
                    MessageBox.Show("DPI Arbiter has an override set. Automatic unset has failed." & vbCrLf &
                                "Please unset the DPI override manually. Then restart ScalA", "ScalA DPI Arbiter Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Else
                    MessageBox.Show($"{errorMessage}", "ScalA DPI Aribter Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                Return
            End If
            Debug.Print("DPI override check passed.")

            ' TODO: Set up named-pipe IPC.



            ' TODO: Monitor shared Scala instance registry.
            ' TODO: Exit when no Scala instances remain.
        Finally
            arbiterMutex.ReleaseMutex()
            arbiterMutex.Dispose()
        End Try
    End Sub

End Module
Module NativeMethods
    Public Structure SHELLEXECUTEINFO
        Public cbSize As Integer
        Public fMask As Integer
        Public hwnd As IntPtr
        <MarshalAs(UnmanagedType.LPTStr)> Public lpVerb As String
        <MarshalAs(UnmanagedType.LPTStr)> Public lpFile As String
        <MarshalAs(UnmanagedType.LPTStr)> Public lpParameters As String
        <MarshalAs(UnmanagedType.LPTStr)> Public lpDirectory As String
        Dim nShow As Integer
        Dim hInstApp As IntPtr
        Dim lpIDList As IntPtr
        <MarshalAs(UnmanagedType.LPTStr)> Public lpClass As String
        Public hkeyClass As IntPtr
        Public dwHotKey As Integer
        Public hIcon As IntPtr
        Public hProcess As IntPtr
    End Structure


    Public Const SEE_MASK_INVOKEIDLIST = &HC
    Public Const SEE_MASK_NOCLOSEPROCESS = &H40
    Public Const SEE_MASK_FLAG_NO_UI = &H400


    Public Const SW_HIDE = 0
    Public Const SW_SHOWNORMAL = 1
    Public Const SW_NORMAL = 1
    Public Const SW_SHOWMINIMIZED = 2
    Public Const SW_SHOWMAXIMIZED = 3
    Public Const SW_MAXIMIZE = 3
    Public Const SW_SHOWNOACTIVATE = 4
    Public Const SW_SHOW = 5
    Public Const SW_MINIMIZE = 6
    Public Const SW_SHOWMINNOACTIVE = 7
    Public Const SW_SHOWNA = 8
    Public Const SW_RESTORE = 9
    Public Const SW_SHOWDEFAULT = 10
    Public Const SW_FORCEMINIMIZE = 11

    <DllImport("Shell32", CharSet:=CharSet.Auto, SetLastError:=True)>
    Public Function ShellExecuteEx(ByRef lpExecInfo As SHELLEXECUTEINFO) As Boolean : End Function
End Module
