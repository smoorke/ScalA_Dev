Module Globals
    Public Function WinUsingDarkTheme() As Boolean
        Using key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Themes\Personalize")
            Dim value = key?.GetValue("AppsUseLightTheme")
            If value IsNot Nothing AndAlso value = 0 Then
                Return True
            End If
        End Using
        Return False
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
        SendInput(1, MouseInput, Runtime.InteropServices.Marshal.SizeOf(GetType(INPUT)))
    End Sub


End Module
