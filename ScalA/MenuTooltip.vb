Imports System.Runtime.InteropServices

Namespace MenuToolTip
    Module MenuTooltip
        ' ----- Constants -----
        Public Const TTS_ALWAYSTIP As Integer = &H1
        Public Const TTS_NOPREFIX As Integer = &H2
        Public Const TTF_SUBCLASS As Integer = &H10
        Public Const TTF_TRANSPARENT As Integer = &H100
        Public Const TTF_IDISHWND As Integer = &H40
        Public Const TTF_ABSOLUTE As Integer = &H80
        Public Const TTF_TRACK As Integer = &H20

        Public Const TTM_ADDTOOL As Integer = &H432
        Public Const TTM_UPDATETIPTEXT As Integer = &H457
        Public Const TTM_DELTOOL As Integer = &H433
        Public Const TTM_POP As Integer = &H41C
        Public Const TTM_SETMAXTIPWIDTH As Integer = &H418
        Public Const TTM_TRACKACTIVATE As Integer = &H411
        Public Const TTM_TRACKPOSITION As Integer = &H412
        Public Const TTM_GETDELAYTIME As Integer = &H421


        Public Const TTDT_RESHOW As Integer = 1
        Public Const TTDT_AUTOPOP As Integer = 2
        Public Const TTDT_INITIAL As Integer = 3

        Public Const WS_POPUP As Integer = &H80000000
        Public Const CW_USEDEFAULT As Integer = &H80000000

        ' ----- Structures -----
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
        Public Structure TOOLINFO
            Public cbSize As Integer
            Public uFlags As Integer
            Public hwnd As IntPtr
            Public uId As IntPtr
            Public rect As RECT
            Public hinst As IntPtr
            <MarshalAs(UnmanagedType.LPTStr)>
            Public lpszText As String
            Public lParam As IntPtr
        End Structure

        ' ----- API -----
        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
        Public Function CreateWindowEx(dwExStyle As Integer, lpClassName As String, lpWindowName As String,
                                       dwStyle As Integer, x As Integer, y As Integer,
                                       nWidth As Integer, nHeight As Integer,
                                       hWndParent As IntPtr, hMenu As IntPtr,
                                       hInstance As IntPtr, lpParam As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError:=True)>
        Public Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As IntPtr, ByRef lParam As TOOLINFO) As IntPtr : End Function
        <DllImport("user32.dll", SetLastError:=True)>
        Public Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr : End Function

        <DllImport("user32.dll")>
        Public Function PtInRect(ByRef lprc As RECT, pt As Point) As Boolean : End Function

        <DllImport("user32.dll", SetLastError:=True)>
        Public Function DestroyWindow(hWnd As IntPtr) As Boolean : End Function

        ' ----- Fields -----
        Public hWndTooltip As IntPtr
        Public tipInfo As TOOLINFO

        Public Sub InitializeTooltip()
            If hWndTooltip <> IntPtr.Zero Then DestroyWindow(hWndTooltip)

            hWndTooltip = CreateWindowEx(WindowStylesEx.WS_EX_TOPMOST, "tooltips_class32", Nothing,
                                     WS_POPUP Or TTS_NOPREFIX Or TTS_ALWAYSTIP,
                                     CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT,
                                     IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero)

            tipInfo = New TOOLINFO()
            tipInfo.cbSize = Marshal.SizeOf(tipInfo)
            tipInfo.uFlags = TTF_ABSOLUTE Or TTF_TRACK Or TTF_TRANSPARENT Or TTF_IDISHWND

            SendMessage(hWndTooltip, TTM_SETMAXTIPWIDTH, IntPtr.Zero, New IntPtr(400))
        End Sub

        ' ===== Show Tooltip =====
        Public Sub ShowTooltip(text As String, x As Integer, y As Integer, hwndMenu As IntPtr)
            If hWndTooltip = IntPtr.Zero Then InitializeTooltip()

            tipInfo.hwnd = hwndMenu
            tipInfo.uId = hwndMenu
            tipInfo.lpszText = text

            ' Add and activate
            SendMessage(hWndTooltip, TTM_ADDTOOL, IntPtr.Zero, tipInfo)
            SendMessage(hWndTooltip, TTM_UPDATETIPTEXT, IntPtr.Zero, tipInfo)

            ' Position & activate
            SendMessage(hWndTooltip, TTM_TRACKPOSITION, IntPtr.Zero, New IntPtr((y + 40 << 16) Or (x And &HFFFF)))
            SendMessage(hWndTooltip, TTM_TRACKACTIVATE, New IntPtr(1), tipInfo)
        End Sub


        Private delayTimer As Timer
        Private hideTimer As Timer
        Public Sub ShowTooltipWithDelay(text As String, hwndmenu As IntPtr, rcMenuItem As RECT)
            If hWndTooltip = IntPtr.Zero Then InitializeTooltip()

            ' Clean up timers
            If delayTimer IsNot Nothing Then
                RemoveHandler delayTimer.Tick, AddressOf DelayTimer_Tick
                delayTimer.Dispose()
            End If
            If hideTimer IsNot Nothing Then
                RemoveHandler hideTimer.Tick, AddressOf HideTimer_Tick
                hideTimer.Dispose()
            End If

            ' Query tooltip control’s delay times
            Dim initialDelay = SendMessage(hWndTooltip, TTM_GETDELAYTIME, New IntPtr(TTDT_INITIAL), IntPtr.Zero).ToInt32()
            ' Dim autoPopDelay = SendMessage(hWndTooltip, TTM_GETDELAYTIME, New IntPtr(TTDT_AUTOPOP), IntPtr.Zero).ToInt32()

            If initialDelay <= 0 Then initialDelay = 400


            delayTimer = New Timer() With {.Interval = initialDelay, .Tag = {text, hwndmenu, rcMenuItem}}
            AddHandler delayTimer.Tick, AddressOf DelayTimer_Tick
            delayTimer.Start()
        End Sub

        Private Sub DelayTimer_Tick(sender As Object, e As EventArgs)
            delayTimer.Stop()
            RemoveHandler delayTimer.Tick, AddressOf DelayTimer_Tick

            Dim args = DirectCast(delayTimer.Tag, Object())
            Dim text As String = CStr(args(0))
            Dim hwndMenu As IntPtr = CType(args(1), IntPtr)
            Dim rcMenuItem As RECT = CType(args(2), RECT)

            If Not PtInRect(rcMenuItem, Control.MousePosition) Then Exit Sub

            Dim autoPopDelay = SendMessage(hWndTooltip, TTM_GETDELAYTIME, New IntPtr(TTDT_AUTOPOP), IntPtr.Zero).ToInt32()
            If autoPopDelay <= 0 Then autoPopDelay = 5000

            ' Setup TOOLINFO
            tipInfo.hwnd = hwndMenu
            tipInfo.uId = hwndMenu
            tipInfo.lpszText = text

            ' Register tool and show
            SendMessage(hWndTooltip, TTM_ADDTOOL, IntPtr.Zero, tipInfo)
            SendMessage(hWndTooltip, TTM_UPDATETIPTEXT, IntPtr.Zero, tipInfo)
            SendMessage(hWndTooltip, TTM_TRACKPOSITION, IntPtr.Zero, New IntPtr((Control.MousePosition.Y + 35 << 16) Or (Control.MousePosition.X And &HFFFF)))
            SendMessage(hWndTooltip, TTM_TRACKACTIVATE, New IntPtr(1), tipInfo)

            ' Auto-hide timer
            hideTimer = New Timer() With {.Interval = autoPopDelay}
            AddHandler hideTimer.Tick, AddressOf HideTimer_Tick
            hideTimer.Start()

            Debug.Print($"Tooltip shown '{text}' (initial={delayTimer.Interval}ms, show={autoPopDelay}ms)")
        End Sub

        Private Sub HideTimer_Tick(sender As Object, e As EventArgs)
            HideTooltip()
            hideTimer.Stop()
            RemoveHandler hideTimer.Tick, AddressOf HideTimer_Tick
        End Sub

        ' ===== Hide Tooltip =====
        Public Sub HideTooltip()
            If hWndTooltip <> IntPtr.Zero Then
                SendMessage(hWndTooltip, TTM_TRACKACTIVATE, IntPtr.Zero, tipInfo)
                SendMessage(hWndTooltip, TTM_DELTOOL, IntPtr.Zero, tipInfo)
            End If
        End Sub

    End Module
End Namespace
