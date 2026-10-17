Imports System.Runtime.InteropServices

Module EventHook
    Public ForegroundHook As IntPtr
    Public ForegroundDelegate As WinEventDelegate = AddressOf ForegroundHookProc
    Private _self As FrmMain

    Public Sub startWinEventHooks(self As FrmMain)
        If ForegroundHook = IntPtr.Zero Then
            _self = self
            SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, ForegroundDelegate, 0, 0, WINEVENT_OUTOFCONTEXT)
        End If
    End Sub

    Public Sub ForegroundHookProc(hWinEventHook As IntPtr, eventType As UInteger, hwnd As IntPtr, idObject As Integer, idChild As Integer, idEventThread As UInteger, msEventTime As UInteger)

        Dim slaveActive As Boolean = (_self.AltPP IsNot Nothing AndAlso hwnd = _self.AltPP?.MainWindowHandle) OrElse hwnd = ScalaHandle

        Debug.Print($"SlaveActive {slaveActive} {hwnd}")
        Debug.Print($"Altpp {_self.AltPP?.MainWindowHandle} ScalA {ScalaHandle}")

        If isActive <> slaveActive Then
            isActive = slaveActive
            Debug.Print($"isActive Changed {isActive}")
            If Not _self.IsDisposed Then _self.BeginInvoke(Sub()
                                                               FrmMain.suppressNCActivate = True
                                                               SendMessage(ScalaHandle, WM_NCACTIVATE, isActive, ScalaHandle)
                                                               FrmMain.suppressNCActivate = False
                                                           End Sub)
        End If

    End Sub

    Public Sub stopWinEventHooks()
        If ForegroundHook <> IntPtr.Zero Then
            UnhookWinEvent(ForegroundHook)
            ForegroundHook = IntPtr.Zero
        End If
    End Sub


    Public Const EVENT_SYSTEM_FOREGROUND As UInteger = &H3
    Public Const WINEVENT_OUTOFCONTEXT As UInteger = &H0

    Public Delegate Sub WinEventDelegate(hWinEventHook As IntPtr, eventType As UInteger, hwnd As IntPtr, idObject As Integer, idChild As Integer, idEventThread As UInteger, msEventTime As UInteger)

    <DllImport("user32.dll", SetLastError:=True)>
    Public Function SetWinEventHook(
        eventMin As UInteger,
        eventMax As UInteger,
        hmodWinEventProc As IntPtr,
        lpfnWinEventProc As WinEventDelegate,
        idProcess As UInteger,
        idThread As UInteger,
        flags As UInteger) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Public Function UnhookWinEvent(
        hWinEventHook As IntPtr) As Boolean
    End Function

End Module
