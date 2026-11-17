Imports System.Threading

Module StartupSequence

    Private AppMutex As Mutex
    Private ReadOnly StartupMutexName As String = $"ScalA_Startup_Mutex_{Reflection.Assembly.GetExecutingAssembly().Location.Replace("\", "").Replace(":", "")}"

    Sub Main()
        'todo move some things from frmMain.load here


        If Environment.OSVersion.Version.Major < 6 AndAlso Environment.OSVersion.Version.Minor < 1 Then
            MessageBox.Show("ScalA requires Windows 7 Or later", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Dim compEnabled As Boolean = False
        DwmIsCompositionEnabled(compEnabled)
        If Not compEnabled Then
            MessageBox.Show("ScalA requires DWM Composition to be on (Aero Theme)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Dim createdNew As Boolean
        If My.Settings.SingleInstance Then
            AppMutex = New Mutex(True, StartupMutexName, createdNew)
            If Not Environment.GetCommandLineArgs().Contains("--restart") Then
                If Not createdNew Then
                    Debug.Print("Second instance detected. Signaling first instance.")
                    'AllowSetForegroundWindow(ASFW_ANY)
                    'IPC.RequestActivation = True
                    'todo setforegroundwindow(firstinstance hwnd)

                    Dim primaryInfo = IPC.GetPrimaryWindowInfo()
                    Dim primaryHwnd As IntPtr = primaryInfo.Item1

                    If primaryHwnd <> IntPtr.Zero Then
                        ' If minimized, restore it first
                        If IsIconic(primaryHwnd) Then SendMessage(primaryHwnd, WM_SYSCOMMAND, SC_RESTORE, IntPtr.Zero)

                        ' Force it to the foreground
                        SetForegroundWindow(primaryHwnd)
                    End If

                    Return
                End If
            End If
        End If


        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(FrmMain)
        My.Settings.Save()
        AppMutex?.ReleaseMutex()
    End Sub
End Module
