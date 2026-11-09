Imports System.Runtime.InteropServices
Imports System.Security.AccessControl

Module DPIDragFix
    Private SizeMoveWindowHook As IntPtr
    Private MenuWindowHook As IntPtr

    Private _sizemoveWindowHookDelegate As WinEventDelegate = New WinEventDelegate(AddressOf SizeMoveWindowDelegate)
    Private _menuWindowHookDelegate As WinEventDelegate = New WinEventDelegate(AddressOf MenuWindowDelegate)

    Public Sub StartDragFix()
        If Environment.OSVersion.Version >= New Version(10, 0, 14393) Then
            If SizeMoveWindowHook = IntPtr.Zero Then
                SizeMoveWindowHook = SetWinEventHook(EVENT_SYSTEM_MOVESIZESTART, EVENT_SYSTEM_MOVESIZESTART, IntPtr.Zero, _sizemoveWindowHookDelegate, 0, 0, WINEVENT_OUTOFCONTEXT)
            End If
            If MenuWindowHook = IntPtr.Zero Then
                MenuWindowHook = SetWinEventHook(EVENT_SYSTEM_MENUSTART, EVENT_SYSTEM_MENUSTART, IntPtr.Zero, _menuWindowHookDelegate, 0, 0, WINEVENT_OUTOFCONTEXT)
            End If
        End If
        dBug.Print("DPIDragFix Started")
    End Sub

    Public Sub StopDragFix()
        If SizeMoveWindowHook <> IntPtr.Zero Then
            UnhookWinEvent(SizeMoveWindowHook)
            SizeMoveWindowHook = IntPtr.Zero
        End If
        If MenuWindowHook <> IntPtr.Zero Then
            UnhookWinEvent(MenuWindowHook)
            MenuWindowHook = IntPtr.Zero
        End If
        dBug.Print("DPIDragFix Stopped")
    End Sub
    Private Sub SizeMoveWindowDelegate(hWinEventHook As IntPtr, eventType As UInteger, hwnd As IntPtr, idObject As Integer, idChild As Integer, idEventThread As UInteger, msEventTime As UInteger)
        If eventType <> EVENT_SYSTEM_MOVESIZESTART Then Exit Sub
        If idObject <> OBJID_WINDOW Then Exit Sub
        If idChild <> CHILDID_SELF Then Exit Sub

        Dim exeNam As String = getExeName(hwnd)

        If Not isAstonia(hwnd) Then
            Debug.Print($"not interested in drag")
            Exit Sub
        End If

        Dim awc As Integer = GetAwarenessFromDpiAwarenessContext(GetWindowDpiAwarenessContext(hwnd))
        If awc <> 2 Then
            Debug.Print($"DpiOverride not set. allowing normal dragmove")
            Exit Sub
        End If

        If Threading.Interlocked.CompareExchange(sizeMoveActiveFlag, 1, 0) <> 0 Then
            SendMessage(hwnd, WM_CANCELMODE, IntPtr.Zero, IntPtr.Zero)
            Debug.Print("additional move event while drag active canceled")
            Exit Sub
        End If

        Task.Run(Sub() ReplaceMove(hwnd)) ' task.run to allow additional messages to be processed

    End Sub

    Private sizeMoveActiveFlag As Integer
    Dim dragstartWP As Point
    Dim dragstartCP As Point
    Public dragFixActive As Boolean
    Dim leftMouseDownInput As INPUT() = {
                        New INPUT With {.type = InputType.INPUT_MOUSE, .u = New InputUnion With {.mi = New MOUSEINPUT With {.dwFlags = MouseEventF.LeftDown}}}
                   }
    Dim gti As GUITHREADINFO = New GUITHREADINFO With {.cbSize = Marshal.SizeOf(Of GUITHREADINFO)}

    Public DragFixMutex As New Threading.Mutex(False, "Local\Astonia_DPIDragFix")
    Public DragFixMutexOwned As Boolean

    Public Sub ReplaceMove(hwnd As IntPtr)


        Dim mutexOwned As Boolean = False

        Try
            Try
                mutexOwned = DragFixMutex.WaitOne(0)
            Catch ex As Threading.AbandonedMutexException
                mutexOwned = True
            End Try

            If Not mutexOwned Then
                Debug.Print("Another process owns drag operation")
                Return
            End If

            Dim lmb As Boolean = (GetAsyncKeyState(Keys.LButton) And &H8000S) <> 0

            If Not lmb Then ' Assume the move is initiated from sysmenu.
                Do
                    ' Cancel the sysmenu move with Escape or when fgw changes
                    If (GetAsyncKeyState(Keys.Escape) And &H8000S) <> 0 Then
                        Exit Sub
                    End If
                    If GetForegroundWindow() <> hwnd Then Return
                    ' Any arrow key starts the move operation.
                    If (GetAsyncKeyState(Keys.Left) And &H8000S) <> 0 Then Exit Do
                    If (GetAsyncKeyState(Keys.Up) And &H8000S) <> 0 Then Exit Do
                    If (GetAsyncKeyState(Keys.Right) And &H8000S) <> 0 Then Exit Do
                    If (GetAsyncKeyState(Keys.Down) And &H8000S) <> 0 Then Exit Do

                    If (GetAsyncKeyState(Keys.LButton) And &H8000S) <> 0 Then Exit Do

                    Threading.Thread.Sleep(1)

                    ' Debug.Print($"sysmenuloop {Date.Now.Millisecond}")
                Loop

                'set lmb as down to 'simulate' left drag, rest of code depends on this and testing seems to show it is consistent with normal sysmenu dragmove behaviour.
                SendInput(1, leftMouseDownInput, Marshal.SizeOf(Of INPUT))

            End If

            Dim rcw As RECT
            GetWindowRect(hwnd, rcw)

            dragstartWP = New Point(rcw.left, rcw.top)
            dragstartCP = Cursor.Position

            If (GetAsyncKeyState(Keys.ShiftKey) And &H8000S) <> 0 Then
                Debug.Print("Shift Held Aborting ReplaceMove")
                Exit Sub
            End If

            If (GetAsyncKeyState(Keys.LButton) And &H8000S) <> 0 Then
                Debug.Print("CANCELMODE")
                SendMessage(hwnd, WM_CANCELMODE, IntPtr.Zero, IntPtr.Zero)

                Do Until Cursor.Position <> dragstartCP
                    Threading.Thread.Sleep(1)
                    If GetForegroundWindow() <> hwnd Then Return
                Loop

                Cursor.Position = dragstartCP

            End If

            Debug.Print("DragReplace Start")

            'start keyhook to swallow arrow keys
            keybHook.Hook()

            Do While (GetAsyncKeyState(Keys.LButton) And &H8000S) <> 0 AndAlso (GetAsyncKeyState(Keys.Return) And &H8000S) = 0
                dragFixActive = True
                If (GetAsyncKeyState(Keys.Escape) And &H8000S) <> 0 OrElse GetForegroundWindow() <> hwnd Then
                    SetWindowPos(hwnd, IntPtr.Zero, dragstartWP.X, dragstartWP.Y, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreZOrder Or SetWindowPosFlags.DoNotActivate)
                    Exit Do
                End If

                Dim newWP As Point = dragstartWP + Cursor.Position - dragstartCP

                SetWindowPos(hwnd, IntPtr.Zero, newWP.X, newWP.Y, 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreZOrder Or SetWindowPosFlags.DoNotActivate)

                Threading.Thread.Sleep(1)
                'Debug.Print($"dragloop {Date.Now.Millisecond}")
            Loop
            dragFixActive = False

            Dim rcFinal As RECT
            GetWindowRect(hwnd, rcFinal)

            Dim rcButtons As RECT
            DwmGetWindowAttribute(hwnd, DWMWA_CAPTION_BUTTON_BOUNDS, rcButtons, Marshal.SizeOf(Of RECT))

            Dim reserved As Integer = rcButtons.right - rcButtons.left + 100

            Dim ptLeft = New Point(rcFinal.left + reserved, rcFinal.top + rcButtons.bottom)
            Dim ptRight = New Point(rcFinal.right - reserved, rcFinal.top + rcButtons.bottom)

            Debug.Print($"pts {ptLeft} {ptRight}")

            Dim waLeft = Screen.FromPoint(ptLeft).WorkingArea
            Dim waRight = Screen.FromPoint(ptRight).WorkingArea

            Dim newtop = Math.Min(waLeft.Top, waRight.Top)

            Debug.Print($"screenL {Screen.FromPoint(ptLeft)}")
            Debug.Print($"screenR {Screen.FromPoint(ptRight)}")

            SetWindowPos(hwnd, IntPtr.Zero, rcFinal.left, Math.Max(rcFinal.top, newtop), 0, 0, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.IgnoreZOrder Or SetWindowPosFlags.DoNotActivate)


            Debug.Print("DragReplace End")
        Finally
            If mutexOwned Then DragFixMutex.ReleaseMutex()
            Threading.Interlocked.Exchange(sizeMoveActiveFlag, 0)
            'stop keyhook
            If Not (My.Settings.DisableWinKey OrElse My.Settings.OnlyEsc OrElse My.Settings.NoAltTab) Then
                keybHook.Unhook()
            End If

            dragFixActive = False
            Debug.Print("ReplaceMove End")
        End Try
    End Sub




    Private Sub MenuWindowDelegate(hWinEventHook As IntPtr, eventType As UInteger, hwnd As IntPtr, idObject As Integer, idChild As Integer, idEventThread As UInteger, msEventTime As UInteger)
        If eventType <> EVENT_SYSTEM_MENUSTART Then Exit Sub
        If idObject <> OBJID_SYSMENU Then Exit Sub

        If Threading.Interlocked.Read(sizeMoveActiveFlag) <> 0 Then
            If isAstonia(hwnd) Then
                Debug.Print("CANCELMENU")
                SendMessage(hwnd, WM_CANCELMODE, IntPtr.Zero, IntPtr.Zero)
            End If
        End If
    End Sub

End Module
