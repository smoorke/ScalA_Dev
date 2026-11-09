Imports System.Runtime.InteropServices

Module EventHook
    Public ForegroundHook As IntPtr
    Public ForegroundDelegate As WinEventDelegate = AddressOf ForegroundHookProc
    Private _self As FrmMain

    Public Sub startWinEventHooks(self As FrmMain)
        If ForegroundHook = IntPtr.Zero Then
            _self = self
            ForegroundHook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, ForegroundDelegate, 0, 0, WINEVENT_OUTOFCONTEXT)
        End If
    End Sub

    Public Sub ForegroundHookProc(hWinEventHook As IntPtr, eventType As UInteger, hwnd As IntPtr, idObject As Integer, idChild As Integer, idEventThread As UInteger, msEventTime As UInteger)

        Dim slaveActive As Boolean = (_self.AltPP IsNot Nothing AndAlso hwnd = _self.AltPP?.MainWindowHandle) OrElse hwnd = ScalaHandle

        Debug.Print($"SlaveActive {slaveActive} {hwnd}")
        Debug.Print($"Altpp {_self.AltPP?.MainWindowHandle} ScalA {ScalaHandle}")

        If BorderIsActive <> slaveActive Then
            BorderIsActive = slaveActive
            Debug.Print($"isActive Changed {BorderIsActive}")
            If Not _self.IsDisposed Then _self.BeginInvoke(Sub()
                                                               FrmMain.suppressNCActivate = True
                                                               SendMessage(ScalaHandle, WM_NCACTIVATE, BorderIsActive, ScalaHandle)
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

End Module
