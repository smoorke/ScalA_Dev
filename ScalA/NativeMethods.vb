﻿Module NativeMethods
    Public Const SHGFI_ICON As Integer = &H100
    Public Const SHGFI_LARGEICON As Integer = &H0
    Public Const SHGFI_SMALLICON As Integer = &H1
    Public Const SHGFI_SYSICONINDEX As Integer = &H4000

    <System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet:=System.Runtime.InteropServices.CharSet.[Unicode])>
    Public Structure SHFILEINFOW
        Public hIcon As IntPtr
        Public iIcon As Integer
        Public dwAttributes As Integer
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=260)> Public szDisplayName As String
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=80)> Public szTypeName As String
    End Structure

    <System.Runtime.InteropServices.DllImport("shell32.dll", EntryPoint:="SHGetFileInfoW", SetLastError:=True)>
    Public Function SHGetFileInfoW(<System.Runtime.InteropServices.InAttribute(), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)> ByVal pszPath As String, ByVal dwFileAttributes As Integer, ByRef psfi As SHFILEINFOW, ByVal cbFileInfo As Integer, ByVal uFlags As Integer) As Integer
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint:="DestroyIcon")>
    Public Function DestroyIcon(ByVal hIcon As System.IntPtr) As <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)> Boolean
    End Function

    <System.Runtime.InteropServices.DllImport("comctl32.dll", SetLastError:=True)>
    Public Function ImageList_GetIcon(hIml As IntPtr, index As Integer, flags As UInteger) As IntPtr 'this tends to fail in MTA (need coinit?)
    End Function

    <System.Runtime.InteropServices.DllImport("comctl32.dll", SetLastError:=True)>
    Public Function ImageList_Destroy(hIml As IntPtr) As Boolean
    End Function

    <Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True, CharSet:=Runtime.InteropServices.CharSet.Auto)>
    Public Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    End Function

    Public Structure SHELLEXECUTEINFO
        Public cbSize As Integer
        Public fMask As Integer
        Public hwnd As IntPtr
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)> Public lpVerb As String
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)> Public lpFile As String
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)> Public lpParameters As String
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)> Public lpDirectory As String
        Dim nShow As Integer
        Dim hInstApp As IntPtr
        Dim lpIDList As IntPtr
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)> Public lpClass As String
        Public hkeyClass As IntPtr
        Public dwHotKey As Integer
        Public hIcon As IntPtr
        Public hProcess As IntPtr
    End Structure


    Public Const SEE_MASK_INVOKEIDLIST = &HC
    Public Const SEE_MASK_NOCLOSEPROCESS = &H40
    Public Const SEE_MASK_FLAG_NO_UI = &H400
    Public Const SW_SHOW As Short = 5

    <System.Runtime.InteropServices.DllImport("Shell32", CharSet:=System.Runtime.InteropServices.CharSet.Auto, SetLastError:=True)>
    Public Function ShellExecuteEx(ByRef lpExecInfo As SHELLEXECUTEINFO) As Boolean
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Public Function LoadImage(hinst As IntPtr, lpszName As String, uType As UInt32, cxDesired As Integer, cyDesired As Integer, fuLoad As UInt32) As IntPtr : End Function
    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Public Function SetWindowLong(ByVal hwnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As UInteger) As Integer : End Function
    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Public Function GetWindowLong(ByVal hwnd As IntPtr, ByVal nIndex As Integer) As UInteger : End Function

    Public Const GWL_HWNDPARENT As Integer = -8

    Public Enum WindowStyles As Long
        WS_BORDER = &H800000
        WS_CAPTION = &HC00000
        WS_CHILD = &H40000000
        WS_CLIPCHILDREN = &H2000000
        WS_CLIPSIBLINGS = &H4000000
        WS_DISABLED = &H8000000
        WS_DLGFRAME = &H400000
        WS_GROUP = &H20000
        WS_HSCROLL = &H100000
        WS_MAXIMIZE = &H1000000
        WS_MAXIMIZEBOX = &H10000
        WS_MINIMIZE = &H20000000
        WS_MINIMIZEBOX = &H20000
        WS_OVERLAPPED = &H0
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED Or WS_CAPTION Or WS_SYSMENU Or WS_SIZEFRAME Or WS_MINIMIZEBOX Or WS_MAXIMIZEBOX
        WS_POPUP = &H80000000UI
        WS_POPUPWINDOW = WS_POPUP Or WS_BORDER Or WS_SYSMENU
        WS_SIZEFRAME = &H40000
        WS_SYSMENU = &H80000
        WS_TABSTOP = &H10000
        WS_VISIBLE = &H10000000
        WS_VSCROLL = &H200000
    End Enum
    Public Enum WindowStylesEx As Long
        WS_EX_ACCEPTFILES = &H10
        WS_EX_APPWINDOW = &H40000
        WS_EX_CLIENTEDGE = &H200
        WS_EX_COMPOSITED = &H2000000
        WS_EX_CONTEXTHELP = &H400
        WS_EX_CONTROLPARENT = &H10000
        WS_EX_DLGMODALFRAME = &H1
        WS_EX_LAYERED = &H80000
        WS_EX_LAYOUTRTL = &H400000
        WS_EX_LEFT = &H0
        WS_EX_LEFTSCROLLBAR = &H4000
        WS_EX_LTRREADING = &H0
        WS_EX_MDICHILD = &H40
        WS_EX_NOACTIVATE = &H8000000
        WS_EX_NOINHERITLAYOUT = &H100000
        WS_EX_NOPARENTNOTIFY = &H4
        WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE Or WS_EX_CLIENTEDGE
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE Or WS_EX_TOOLWINDOW Or WS_EX_TOPMOST
        WS_EX_RIGHT = &H1000
        WS_EX_RIGHTSCROLLBAR = &H0
        WS_EX_RTLREADING = &H2000
        WS_EX_STATICEDGE = &H20000
        WS_EX_TOPMOST = &H8
        WS_EX_TOOLWINDOW = &H80
        WS_EX_TRANSPARENT = &H20
        WS_EX_WINDOWEDGE = &H100
    End Enum


    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Public Function GetWindowRect(ByVal hWnd As IntPtr, ByRef lpRect As Rectangle) As Boolean : End Function
    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Public Function GetClientRect(ByVal hWnd As IntPtr, ByRef lpRect As Rectangle) As Boolean : End Function
    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Public Function ClientToScreen(ByVal hWnd As IntPtr, ByRef lpPoint As Point) As Boolean : End Function
    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Public Function GetForegroundWindow() As IntPtr : End Function
    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Public Function GetWindowThreadProcessId(ByVal hWnd As IntPtr, <System.Runtime.InteropServices.Out()> ByRef lpdwProcessId As UInteger) As UInteger : End Function
#Region " SetWindowPos "
    <Flags>
    Public Enum SetWindowPosFlags As UInteger
        ''' <summary>If the calling thread and the thread that owns the window are attached to different input queues,
        ''' the system posts the request to the thread that owns the window. This prevents the calling thread from
        ''' blocking its execution while other threads process the request.</summary>
        ''' <remarks>SWP_ASYNCWINDOWPOS</remarks>
        ASyncWindowPosition = &H4000
        ''' <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
        ''' <remarks>SWP_DEFERERASE</remarks>
        DeferErase = &H2000
        ''' <summary>Draws a frame (defined in the window's class description) around the window.</summary>
        ''' <remarks>SWP_DRAWFRAME</remarks>
        DrawFrame = &H20
        ''' <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to
        ''' the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE
        ''' is sent only when the window's size is being changed.</summary>
        ''' <remarks>SWP_FRAMECHANGED</remarks>
        FrameChanged = &H20
        ''' <summary>Hides the window.</summary>
        ''' <remarks>SWP_HIDEWINDOW</remarks>
        HideWindow = &H80
        ''' <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the
        ''' top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter
        ''' parameter).</summary>
        ''' <remarks>SWP_NOACTIVATE</remarks>
        DoNotActivate = &H10
        ''' <summary>Discards the entire contents of the client area. If this flag is not specified, the valid
        ''' contents of the client area are saved and copied back into the client area after the window is sized or
        ''' repositioned.</summary>
        ''' <remarks>SWP_NOCOPYBITS</remarks>
        DoNotCopyBits = &H100
        ''' <summary>Retains the current position (ignores X and Y parameters).</summary>
        ''' <remarks>SWP_NOMOVE</remarks>
        IgnoreMove = &H2
        ''' <summary>Does not change the owner window's position in the Z order.</summary>
        ''' <remarks>SWP_NOOWNERZORDER</remarks>
        DoNotChangeOwnerZOrder = &H200
        ''' <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to
        ''' the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent
        ''' window uncovered as a result of the window being moved. When this flag is set, the application must
        ''' explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
        ''' <remarks>SWP_NOREDRAW</remarks>
        DoNotRedraw = &H8
        ''' <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
        ''' <remarks>SWP_NOREPOSITION</remarks>
        DoNotReposition = &H200
        ''' <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
        ''' <remarks>SWP_NOSENDCHANGING</remarks>
        DoNotSendChangingEvent = &H400
        ''' <summary>Retains the current size (ignores the cx and cy parameters).</summary>
        ''' <remarks>SWP_NOSIZE</remarks>
        IgnoreResize = &H1
        ''' <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
        ''' <remarks>SWP_NOZORDER</remarks>
        IgnoreZOrder = &H4
        ''' <summary>Displays the window.</summary>
        ''' <remarks>SWP_SHOWWINDOW</remarks>
        ShowWindow = &H40
    End Enum
    <System.Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True)>
    Public Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As SetWindowPosFlags) As Boolean
    End Function
#End Region

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, CharSet:=Runtime.InteropServices.CharSet.Unicode)>
    Public Structure SHSTOCKICONINFO

        Public cbSize As UInteger
        Public hIcon As IntPtr
        Public iSysIconIndex As Integer
        Public iIcon As Integer
        <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=260)>
        Public szPath As String
    End Structure

    <System.Runtime.InteropServices.DllImport("shell32.dll", SetLastError:=True)>
    Public Function SHGetStockIconInfo(ssid As UInteger, uFlags As UInteger, ByRef pssi As SHSTOCKICONINFO) As Integer : End Function

End Module
