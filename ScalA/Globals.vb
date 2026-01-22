Imports System.Runtime.InteropServices
Module Globals

    ' Application constants
    Public Const APP_NAME As String = "ScalA"
    Public Const REGISTRY_COMPAT_LAYERS As String = "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers"
    Public Const DPI_AWARE_FLAG As String = " HIGHDPIAWARE"
    Public Const DPI_AWARE_REMOVE As String = "~ HIGHDPIAWARE"

    ' Theme color constants (ARGB hex values)
    Public ReadOnly COLOR_LIGHT_GRAY As Color = Color.FromArgb(&HFFE1E1E1)      ' Light gray background
    Public ReadOnly COLOR_MEDIUM_GRAY As Color = Color.FromArgb(&HFFA2A2A2)     ' Medium gray
    Public ReadOnly COLOR_BORDER_GRAY As Color = Color.FromArgb(&HFFADADAD)     ' Border gray
    Public ReadOnly COLOR_HIGHLIGHT_BLUE As Color = Color.FromArgb(&HFFE5F1FB)  ' Light blue highlight
    Public ReadOnly COLOR_WINDOWS_BLUE As Color = Color.FromArgb(&HFF0078D7)    ' Windows accent blue
    Public ReadOnly COLOR_CLOSE_PRESSED As Color = Color.FromArgb(255, 102, 102) ' Close button pressed

    Public Property Startup As Boolean = True

    Public Property KeybHook As New KeyboardHook

    Public Property StructureToPtrSupported As Boolean = False

    Public Property MouseButtonStale As MouseButtons

    Public ReadOnly hideExt As String() = {".lnk", ".url"}

    Public Property AnimsEnabled As Boolean = getAnimationsEnabled()
    Public Sub setScalAPriority(pri As Integer)
        Dim hProcess As IntPtr = GetCurrentProcess()
        Dim result As Boolean = SetPriorityClass(hProcess, pri)

        If result Then
            FrmMain.AltPP?.setPriority(pri)
            Debug.Print($"Process priority set to {pri}")
        Else
            Debug.Print("Failed to set priority. LastError = " & Marshal.GetLastWin32Error())
        End If
    End Sub


    Public Function DriveIncursSeekPenalty(driveLetter As Char) As Boolean
        Dim handle As IntPtr = CreateFile(
        "\\.\" & driveLetter & ":",
        GENERIC_READ,
        FILE_SHARE_READ Or FILE_SHARE_WRITE,
        IntPtr.Zero,
        OPEN_EXISTING,
        FILE_ATTRIBUTE_NORMAL,
        IntPtr.Zero)

        If handle = IntPtr.Zero Then Return False

        Dim query As New STORAGE_PROPERTY_QUERY With {
        .PropertyId = 7, ' StorageDeviceSeekPenaltyProperty
        .QueryType = 0,
        .AdditionalParameters = New Byte(0) {}
    }

        Dim outDesc As New DEVICE_SEEK_PENALTY_DESCRIPTOR()
        Dim bytesReturned As UInteger
        Dim size As Integer = Marshal.SizeOf(outDesc)
        Dim pQuery As IntPtr = IntPtr.Zero
        Dim pOut As IntPtr = IntPtr.Zero

        Try
            pQuery = Marshal.AllocHGlobal(Marshal.SizeOf(query))
            Marshal.StructureToPtr(query, pQuery, False)
            pOut = Marshal.AllocHGlobal(size)

            Dim ok As Boolean = DeviceIoControl(handle, IOCTL_STORAGE_QUERY_PROPERTY, pQuery,
            Marshal.SizeOf(query), pOut, size, bytesReturned, IntPtr.Zero)

            If Not ok Then Return False

            outDesc = CType(Marshal.PtrToStructure(pOut, GetType(DEVICE_SEEK_PENALTY_DESCRIPTOR)), DEVICE_SEEK_PENALTY_DESCRIPTOR)
            Return outDesc.IncursSeekPenalty
        Finally
            If pQuery <> IntPtr.Zero Then Marshal.FreeHGlobal(pQuery)
            If pOut <> IntPtr.Zero Then Marshal.FreeHGlobal(pOut)
            CloseHandle(handle)
        End Try
    End Function

    Public Function CallAsTaskWithTimeout(Of T, R)(fun As Func(Of T, R), arg As T, timeout As Integer, Optional FailVal As R = Nothing) As R
        Dim tsk As Task(Of R) = Task.Run(Function()
                                             Try
                                                 Return fun(arg)
                                             Catch ex As Exception
                                                 Return FailVal
                                             End Try
                                         End Function)
        If tsk.Wait(timeout) Then
            Return tsk.Result
        Else
            Return FailVal
        End If
    End Function

    Public Property QLFilter As String() = getQLFilter(My.Settings.AdditionalExtensions).ToArray()
    Public Function getQLFilter(addFts) As IEnumerable(Of String)
        Dim ft = CType(("exe | lnk | url |" & addFts).Split({"|"c}, StringSplitOptions.RemoveEmptyEntries), IEnumerable(Of String)).Select(Function(f As String)
                                                                                                                                               f = f.Trim().ToLowerInvariant()
                                                                                                                                               Return If(f.StartsWith("."), f, "." & f)
                                                                                                                                           End Function)
        dBug.Print($"QLFilter:")
        For Each fil In ft
            dBug.Print($"""{fil}""")
        Next
        dBug.Print("---")
        Return ft
    End Function

    Public Function WinUsingDarkTheme() As Boolean
        Using key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Themes\Personalize")
            Dim value = key?.GetValue("AppsUseLightTheme")
            If value IsNot Nothing AndAlso value = 0 Then
                Return True
            End If
        End Using
        Return False
    End Function
    Public Function GetActiveProcessID() As UInteger
        Dim hWnd As IntPtr = GetForegroundWindow()
        Dim ProcessID As UInteger = 0

        GetWindowThreadProcessId(hWnd, ProcessID)

        Return ProcessID
    End Function

    Public Function getAnimationsEnabled() As Boolean
        Dim enabled As Boolean = False
        If Not SystemParametersInfo(SPI.GETCLIENTAREAANIMATION, 0, enabled, 0) Then
            dBug.Print("SystemParametersInfo SPI_GETCLIENTAREAANIMATION FAIL!")
        End If
        Return enabled
    End Function

    Public Function IsDirectoryWritable(dirPath As String, Optional throwOnFail As Boolean = False) As Boolean
        Try
            Using fs As IO.FileStream = IO.File.Create(IO.Path.Combine(dirPath, IO.Path.GetRandomFileName()), 1, IO.FileOptions.DeleteOnClose)
            End Using
            Return True
        Catch
            If throwOnFail Then Throw
            Return False
        End Try
    End Function

    Private ReadOnly KeyInput() As INPUT = {
                   New INPUT With {.type = InputType.INPUT_KEYBOARD,
                           .u = New InputUnion With {.ki = New KEYBDINPUT With {.dwFlags = KeyEventF.KeyDown}}
                   },
                   New INPUT With {.type = InputType.INPUT_KEYBOARD,
                           .u = New InputUnion With {.ki = New KEYBDINPUT With {.dwFlags = KeyEventF.KeyUp}}
                   }}
    Private ReadOnly KeyInputEx() As INPUT = {
                   New INPUT With {.type = InputType.INPUT_KEYBOARD,
                           .u = New InputUnion With {.ki = New KEYBDINPUT With {.dwFlags = KeyEventF.KeyDown Or KeyEventF.ExtendedKey}}
                   },
                   New INPUT With {.type = InputType.INPUT_KEYBOARD,
                           .u = New InputUnion With {.ki = New KEYBDINPUT With {.dwFlags = KeyEventF.KeyUp Or KeyEventF.ExtendedKey}}
                   }}
    Public Sub SendScanKeyEx(scan As UShort)
        KeyInputEx(0).u.ki.wScan = &HE000 Or scan
        KeyInputEx(1).u.ki.wScan = &HE000 Or scan
        SendInput(2, KeyInputEx, Runtime.InteropServices.Marshal.SizeOf(GetType(INPUT)))
    End Sub
    Public Sub SendKey(keyCode As UShort)
        If keyCode = 16 OrElse keyCode = 17 Then Exit Sub
        KeyInput(0).u.ki.wScan = 0
        KeyInput(1).u.ki.wScan = 0
        KeyInput(0).u.ki.wVk = keyCode
        KeyInput(1).u.ki.wVk = keyCode
        SendInput(2, KeyInput, Runtime.InteropServices.Marshal.SizeOf(GetType(INPUT)))
    End Sub
    Public Sub SendScanKey(scan As UShort)
        KeyInput(0).u.ki.wVk = 0
        KeyInput(1).u.ki.wVk = 0
        KeyInput(0).u.ki.wScan = scan
        KeyInput(1).u.ki.wScan = scan
        SendInput(2, KeyInput, Runtime.InteropServices.Marshal.SizeOf(GetType(INPUT)))
    End Sub

    Private ReadOnly MouseInput() As INPUT = {
                   New INPUT With {   '.type = InputType.INPUT_MOUSE, 'INPUT_MOUSE is 0 so we can omit
                           .u = New InputUnion With {.mi = New MOUSEINPUT}
                   }
   }
    Public Sub SendMouseInput(flags As MouseEventF)
        MouseInput(0).u.mi.dwFlags = flags
        MouseInput(0).u.mi.mouseData = 0
        SendInput(1, MouseInput, Runtime.InteropServices.Marshal.SizeOf(GetType(INPUT)))
    End Sub
    Public Sub SendMouseInput(flags As MouseEventF, mouseData As UInt32)
        MouseInput(0).u.mi.dwFlags = flags
        MouseInput(0).u.mi.mouseData = mouseData
        SendInput(1, MouseInput, Runtime.InteropServices.Marshal.SizeOf(GetType(INPUT)))
    End Sub

    Public ReadOnly scalaProc As Process = Process.GetCurrentProcess()
    Public ReadOnly scalaPID As Integer = scalaProc.Id

    Public ReadOnly Property usableCores() As Integer
        Get
            Dim mask As ULong = scalaProc.ProcessorAffinity.ToInt64()
            Dim uCores As Integer = 0
            While mask <> 0
                uCores += mask And 1UL
                mask >>= 1
            End While
            Return uCores
        End Get
    End Property


    Private _ScalAHandle As IntPtr
    Public Property ScalaHandle() As IntPtr
        Get
            If _ScalAHandle = IntPtr.Zero Then _ScalAHandle = FrmMain.Handle
            Return _ScalAHandle
        End Get
        Set(value As IntPtr)
            _ScalAHandle = value
        End Set
    End Property

    Private _ScalAClass As String
    Public ReadOnly Property ScalaClass() As String
        Get
            If String.IsNullOrEmpty(_ScalAClass) Then _ScalAClass = GetWindowClass(ScalaHandle)
            Return _ScalAClass
        End Get
    End Property

    Private _ScalAThreadId As Integer?
    Public ReadOnly Property ScalaThreadId() As Integer
        Get
            If _ScalAThreadId Is Nothing Then _ScalAThreadId = GetWindowThreadProcessId(ScalaHandle, Nothing)
            Return _ScalAThreadId
        End Get
    End Property

    Public shNotify As Integer = 0
    Public Sub RegisterShellNotify(hWnd As IntPtr)
        shNotify = SHChangeNotifyRegister(
            hWnd,
            SHCNRF_ShellLevel,    ' only shell-level notifications
            SHCNE_ASSOCCHANGED,   ' only care about assoc changes
            WM_SHNOTIFY,
            1,
            New SHChangeNotifyEntry()
        )
    End Sub

    Public Sub UnregisterShellNotify()
        If shNotify <> 0 Then
            SHChangeNotifyDeregister(shNotify)
            dBug.Print("Unregistered ShellNotify")
            shNotify = 0
        End If
    End Sub

End Module
