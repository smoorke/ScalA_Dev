Imports System.Runtime.InteropServices

Public Class KeyboardHook : Implements IDisposable

    Public Delegate Function HookCallBack(nCode As Integer, wParam As IntPtr, lParam As IntPtr) As Integer

    <DllImport("Kernel32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function GetModuleHandle(ByVal ModuleName As String) As IntPtr : End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function SetWindowsHookEx(idHook As Integer, HookProc As HookCallBack,
           hInstance As IntPtr, ThreadId As Integer) As IntPtr : End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function CallNextHookEx(hHook As IntPtr, nCode As Integer,
           wParam As IntPtr, lParam As IntPtr) As Integer : End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function UnhookWindowsHookEx(hHook As IntPtr) As Boolean : End Function

    <StructLayout(LayoutKind.Sequential)>
    Private Structure KBDLLHOOKSTRUCT
        Public vkCode As UInt32
        Public scanCode As UInt32
        Public flags As KBDLLHOOKSTRUCTFlags
        Public time As UInt32
        Public dwExtraInfo As UIntPtr
    End Structure

    <Flags()>
    Private Enum KBDLLHOOKSTRUCTFlags As UInt32
        LLKHF_EXTENDED = &H1
        LLKHF_INJECTED = &H10
        LLKHF_ALTDOWN = &H20
        LLKHF_UP = &H80
    End Enum

    Public HookHandle As IntPtr = IntPtr.Zero

    Private Const WH_KEYBOARD_LL As Integer = 13
    Private Const HC_ACTION As Integer = 0
    Private Const WM_KEYDOWN = &H100
    Private Const WM_KEYUP = &H101
    Private Const WM_SYSKEYDOWN = &H104
    Private Const WM_SYSKEYUP = &H105

    Private alreadySendingEsc As Boolean = False

    Private Function KeyProc(
        ByVal nCode As Integer,
        ByVal wParam As IntPtr,
        ByVal lParam As IntPtr) As Integer

        If nCode < 0 OrElse nCode <> HC_ACTION Then Return CallNextHookEx(HookHandle, nCode, wParam, lParam)

        Select Case wParam.ToInt32

            Case WM_KEYDOWN
                Select Case Marshal.PtrToStructure(Of UInteger)(lParam)
                ' we only care about the first field so we marshal directly to it's type instead of using Marshal.PtrToStructure(Of KBDLLHOOKSTRUCT)(lParam).vkCode
                    Case Keys.LWin, Keys.RWin
                        If My.Settings.DisableWinKey Then
                            Using proc As Process = Process.GetProcessById(GetActiveProcessID())
                                If proc.IsAstonia OrElse (My.Settings.gameOnOverview AndAlso proc.IsScalA) Then
                                    dBug.print($"Win Key Blocked {wParam:X4} {proc.ProcessName}")
                                    Return 1
                                End If
                            End Using
                        End If
                    Case Keys.Escape
                        If Not alreadySendingEsc AndAlso My.Settings.OnlyEsc AndAlso My.Computer.Keyboard.CtrlKeyDown AndAlso Not (My.Settings.AllowCtrlShiftEsc AndAlso My.Computer.Keyboard.ShiftKeyDown) Then
                            Using proc As Process = Process.GetProcessById(GetActiveProcessID())
                                If proc.IsAstonia OrElse (My.Settings.gameOnOverview AndAlso proc.IsScalA) Then
                                    dBug.Print("ctrl esc")
                                    alreadySendingEsc = True
                                    Try
                                        BlockInput(True)
                                        SendInput(CtrlEscKeyInput.Count, CtrlEscKeyInput, Runtime.InteropServices.Marshal.SizeOf(GetType(INPUT)))
                                        Return 1
                                    Finally
                                        SendInput(1, CtrlDownInput, Runtime.InteropServices.Marshal.SizeOf(GetType(INPUT)))
                                        alreadySendingEsc = False
                                        BlockInput(False)
                                    End Try
                                End If
                            End Using
                        End If
                End Select
            Case WM_KEYUP
                If My.Settings.OnlyEsc AndAlso My.Computer.Keyboard.CtrlKeyDown AndAlso Marshal.PtrToStructure(Of UInteger)(lParam) = Keys.Escape Then
                    dBug.Print($"esc up {My.Computer.Keyboard.CtrlKeyDown} {alreadySendingEsc}")
                    Using proc As Process = Process.GetProcessById(GetActiveProcessID())
                        If proc.IsAstonia OrElse (My.Settings.gameOnOverview AndAlso proc.IsScalA) Then
                            SendInput(1, CtrlDownInput, Runtime.InteropServices.Marshal.SizeOf(GetType(INPUT)))
                        End If
                    End Using
                End If
            Case WM_SYSKEYDOWN
                Dim key As Keys = Marshal.PtrToStructure(Of UInteger)(lParam)
                If My.Settings.OnlyEsc AndAlso key = Keys.Escape Then
                    Using proc As Process = Process.GetProcessById(GetActiveProcessID())
                        If proc.IsAstonia OrElse (My.Settings.gameOnOverview AndAlso proc.IsScalA) Then
                            dBug.Print("alt esc")
                            SendInput(AltEscKeyInput.Count, AltEscKeyInput, Runtime.InteropServices.Marshal.SizeOf(GetType(INPUT)))
                            Return 1
                        End If
                    End Using
                End If
                If My.Settings.NoAltTab AndAlso key = Keys.Tab Then
                    Using proc As Process = Process.GetProcessById(GetActiveProcessID())
                        If proc.IsAstonia OrElse (My.Settings.gameOnOverview AndAlso proc.IsScalA) Then
                            Return 1
                        End If
                    End Using
                End If
        End Select

        Return CallNextHookEx(HookHandle, nCode, wParam, lParam)
    End Function

    Private ReadOnly AltEscKeyInput() As INPUT = {
                   New INPUT With {.type = InputType.INPUT_KEYBOARD,
                        .u = New InputUnion With {.ki = New KEYBDINPUT With {.dwFlags = KeyEventF.KeyUp, .wVk = Keys.Menu}}
                   },
                   New INPUT With {.type = InputType.INPUT_KEYBOARD,
                        .u = New InputUnion With {.ki = New KEYBDINPUT With {.dwFlags = KeyEventF.KeyDown, .wScan = 1, .wVk = Keys.Escape}}
                   },
                   New INPUT With {.type = InputType.INPUT_KEYBOARD,
                        .u = New InputUnion With {.ki = New KEYBDINPUT With {.dwFlags = KeyEventF.KeyDown, .wVk = Keys.Menu}}
                   }
             }

    Private ReadOnly CtrlEscKeyInput() As INPUT = {
                   New INPUT With {.type = InputType.INPUT_KEYBOARD,
                        .u = New InputUnion With {.ki = New KEYBDINPUT With {.dwFlags = KeyEventF.KeyUp, .wVk = Keys.ControlKey}}
                   },
                   New INPUT With {.type = InputType.INPUT_KEYBOARD,
                        .u = New InputUnion With {.ki = New KEYBDINPUT With {.dwFlags = KeyEventF.KeyDown, .wScan = 1, .wVk = Keys.Escape}}
                   }
             }

    Private ReadOnly CtrlDownInput() As INPUT = {
                    New INPUT With {.type = InputType.INPUT_KEYBOARD,
                        .u = New InputUnion With {.ki = New KEYBDINPUT With {.dwFlags = KeyEventF.KeyDown, .wVk = Keys.ControlKey}}
                    }
            }


    Private mhCallBack As HookCallBack = New HookCallBack(AddressOf KeyProc)
    Private disposedValue As Boolean

    Public Sub Hook()
        If HookHandle <> IntPtr.Zero Then Exit Sub
        HookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, mhCallBack,
            GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0)
        If HookHandle = IntPtr.Zero Then Throw New System.Exception("KeyboardHook failed")
    End Sub

    Public Sub Unhook()
        If HookHandle <> IntPtr.Zero Then
            UnhookWindowsHookEx(HookHandle)
            HookHandle = IntPtr.Zero
        End If
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects)
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
            Unhook()
            ' TODO: set large fields to null
            disposedValue = True
        End If
    End Sub

    ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
    Protected Overrides Sub Finalize()
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=False)
        MyBase.Finalize()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub

End Class