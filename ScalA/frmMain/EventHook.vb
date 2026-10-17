Imports System.Runtime.InteropServices

Module EventHook
    Public ForegroundHook As IntPtr
    Public ForegroundDelegate As WinEventDelegate = AddressOf ForegroundHookProc

    Public Sub startWinEventHooks()
        If ForegroundHook = IntPtr.Zero Then
            SetWinEventHook(ForegroundHook, EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, ForegroundDelegate, 0, 0, WINEVENT_OUTOFCONTEXT)
        End If
    End Sub



    Private Sub ForegroundHookProc(hWinEventHook As IntPtr, eventType As UInteger, hwnd As IntPtr, idObject As Integer, idChild As Integer, idEventThread As UInteger, msEventTime As UInteger)

        Dim slaveActive As Boolean = (FrmMain.AltPP IsNot Nothing AndAlso hwnd = FrmMain.AltPP.MainWindowHandle) OrElse hwnd = ScalaHandle

        Debug.Print($"SlaveActive {slaveActive} {hwnd}")
        Debug.Print($"Altpp {FrmMain.AltPP?.MainWindowHandle} ScalA {ScalaHandle}")


        If isActive <> slaveActive Then
            isActive = slaveActive
            Debug.Print($"isActive Changed {isActive}")
            If Not FrmMain.IsDisposed Then FrmMain.BeginInvoke(Sub()
                                                                   FrmMain.suppressNCActivate = True
                                                                   SendMessage(ScalaHandle, WM_NCACTIVATE, isActive, ScalaHandle)
                                                                   FrmMain.suppressNCActivate = False
                                                               End Sub)
        End If

    End Sub

    Public Sub stopWinEventHooks()
        If ForegroundHook <> IntPtr.Zero Then
            UnhookWinEvent(ForegroundHook)
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
