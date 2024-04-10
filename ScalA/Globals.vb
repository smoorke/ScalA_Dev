Module Globals

    Public keybHook As New KeyboardHook

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
            Debug.Print("SystemParametersInfo SPI_GETCLIENTAREAANIMATION FAIL!")
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


End Module
