Imports System.Runtime.InteropServices

Module NativeMethods
    Public Const SHGFI_ICON As Integer = &H100
    Public Const SHGFI_LARGEICON As Integer = &H0
    Public Const SHGFI_SMALLICON As Integer = &H1
    Public Const SHGFI_SYSICONINDEX As Integer = &H4000

    <DllImport("user32.dll", CharSet:=CharSet.Auto, ExactSpelling:=True)>
    Public Function MonitorFromPoint(pt As Point, dwFlags As UInt32) As IntPtr : End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Public Function GetMonitorInfo(hmonitor As IntPtr, ByRef info As MonitorInfo) As Boolean : End Function

    Public Enum MONITOR As UInt32
        DEFAULTTONULL = &H0
        DEFAULTTOPRIMARY = &H1
        DEFAULTTONEAREST = &H2
    End Enum

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto, Pack:=4)>
    Public Structure MonitorInfo
        Public cbSize As UInteger
        Public rcMonitor As RECT
        Public rcWork As RECT
        Public dwFlags As UInteger
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
        Public Sub New(ByVal Rectangle As Rectangle)
            Me.New(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom)
        End Sub
        Public Sub New(ByVal Left As Integer, ByVal Top As Integer, ByVal Right As Integer, ByVal Bottom As Integer)
            Me.Left = Left
            Me.Top = Top
            Me.Right = Right
            Me.Bottom = Bottom
        End Sub
        Public Function ToRectangle() As Rectangle
            Return Rectangle.FromLTRB(Me.Left, Me.Top, Me.Right, Me.Bottom)
        End Function
    End Structure
    <Runtime.CompilerServices.Extension>
    Public Function ToRECT(Rectangle As Rectangle) As RECT
        Return New RECT(Rectangle)
    End Function
    <Runtime.CompilerServices.Extension>
    Public Function LOWORD(param As IntPtr) As Short
        Return CType(param, Integer) And &HFFFF
    End Function
    <Runtime.CompilerServices.Extension>
    Public Function HIWORD(param As IntPtr) As Short
        Return (CType(param, Integer) And &HFFFF0000) >> 16
    End Function


    <StructLayout(LayoutKind.Sequential)>
    Structure APPBARDATA
        Public cbSize As Integer
        Public hWnd As IntPtr
        Public uCallbackMessage As Integer
        Public uEdge As Integer
        Public rc As RECT
        Public lParam As Boolean
    End Structure

    Public Enum ABM As UInteger
        [NEW] = &H0
        REMOVE = &H1
        QUERYPOS = &H2
        SETPOS = &H3
        GETSTATE = &H4
        GETTASKBARPOS = &H5
        ACTIVATE = &H6
        GETAUTOHIDEBAR = &H7
        SETAUTOHIDEBAR = &H8
        WINDOWPOSCHANGED = &H9
        SETSTATE = &HA
        GETAUTOHIDEBAREX = &HB
        SETAUTOHIDEBAREX = &HC
    End Enum

    <DllImport("shell32.dll", CallingConvention:=CallingConvention.StdCall)>
    Public Function SHAppBarMessage(ByVal dwMessage As Integer, ByRef pData As APPBARDATA) As IntPtr : End Function

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.[Unicode])>
    Public Structure SHFILEINFOW
        Public hIcon As IntPtr
        Public iIcon As Integer
        Public dwAttributes As Integer
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)> Public szDisplayName As String
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)> Public szTypeName As String
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure WINDOWPOS
        Public hwnd As IntPtr
        Public hwndInsertAfter As IntPtr
        Public x As Integer
        Public y As Integer
        Public cx As Integer
        Public cy As Integer
        Public flags As Integer
    End Structure

    <DllImport("shell32.dll", EntryPoint:="SHGetFileInfoW", SetLastError:=True)>
    Public Function SHGetFileInfoW(<InAttribute(), MarshalAs(UnmanagedType.LPTStr)> ByVal pszPath As String, ByVal dwFileAttributes As Integer, ByRef psfi As SHFILEINFOW, ByVal cbFileInfo As Integer, ByVal uFlags As Integer) As Integer
    End Function



    <DllImport("user32.dll", EntryPoint:="DestroyIcon")>
    Public Function DestroyIcon(ByVal hIcon As System.IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("comctl32.dll", SetLastError:=True)>
    Public Function ImageList_GetIcon(hIml As IntPtr, index As Integer, flags As UInteger) As IntPtr 'this tends to fail in MTA (need coinit?)
    End Function

    <DllImport("comctl32.dll", SetLastError:=True)>
    Public Function ImageList_Destroy(hIml As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=Runtime.InteropServices.CharSet.Auto)>
    Public Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr : End Function
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Public Function FindWindowEx(ByVal parentHandle As IntPtr,
                      ByVal childAfter As IntPtr,
                      ByVal lclassName As String,
                      ByVal windowTitle As String) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Public Function GetWindow(hWnd As IntPtr, uCmd As UInteger) As IntPtr : End Function



    Public Structure SHELLEXECUTEINFO
        Public cbSize As Integer
        Public fMask As Integer
        Public hwnd As IntPtr
        <MarshalAs(UnmanagedType.LPTStr)> Public lpVerb As String
        <MarshalAs(UnmanagedType.LPTStr)> Public lpFile As String
        <MarshalAs(UnmanagedType.LPTStr)> Public lpParameters As String
        <MarshalAs(UnmanagedType.LPTStr)> Public lpDirectory As String
        Dim nShow As Integer
        Dim hInstApp As IntPtr
        Dim lpIDList As IntPtr
        <MarshalAs(UnmanagedType.LPTStr)> Public lpClass As String
        Public hkeyClass As IntPtr
        Public dwHotKey As Integer
        Public hIcon As IntPtr
        Public hProcess As IntPtr
    End Structure


    Public Const SEE_MASK_INVOKEIDLIST = &HC
    Public Const SEE_MASK_NOCLOSEPROCESS = &H40
    Public Const SEE_MASK_FLAG_NO_UI = &H400
    Public Const SW_SHOW As Short = 5

    <DllImport("Shell32", CharSet:=CharSet.Auto, SetLastError:=True)>
    Public Function ShellExecuteEx(ByRef lpExecInfo As SHELLEXECUTEINFO) As Boolean
    End Function

    <DllImport("user32.dll")>
    Public Function LoadImage(hinst As IntPtr, lpszName As String, uType As UInt32, cxDesired As Integer, cyDesired As Integer, fuLoad As UInt32) As IntPtr : End Function
    <DllImport("user32.dll")>
    Public Function SetWindowLong(ByVal hwnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As UInteger) As Integer : End Function
    <DllImport("user32.dll")>
    Public Function GetWindowLong(ByVal hwnd As IntPtr, ByVal nIndex As Integer) As UInteger : End Function

    Public Const GWL_HWNDPARENT As Integer = -8
    <Flags()>
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
    <Flags()>
    Public Enum WindowStylesEx As UInteger
        ''' <summary>Specifies a window that accepts drag-drop files.</summary>
        WS_EX_ACCEPTFILES = &H10

        ''' <summary>Forces a top-level window onto the taskbar when the window is visible.</summary>
        WS_EX_APPWINDOW = &H40000

        ''' <summary>Specifies a window that has a border with a sunken edge.</summary>
        WS_EX_CLIENTEDGE = &H200

        ''' <summary>
        ''' Specifies a window that paints all descendants in bottom-to-top painting order using double-buffering.
        ''' This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC. This style is not supported in Windows 2000.
        ''' </summary>
        ''' <remarks>
        ''' With WS_EX_COMPOSITED set, all descendants of a window get bottom-to-top painting order using double-buffering.
        ''' Bottom-to-top painting order allows a descendent window to have translucency (alpha) and transparency (color-key) effects,
        ''' but only if the descendent window also has the WS_EX_TRANSPARENT bit set.
        ''' Double-buffering allows the window and its descendents to be painted without flicker.
        ''' </remarks>
        WS_EX_COMPOSITED = &H2000000

        ''' <summary>
        ''' Specifies a window that includes a question mark in the title bar. When the user clicks the question mark,
        ''' the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message.
        ''' The child window should pass the message to the parent window procedure, which should call the WinHelp function using the HELP_WM_HELP command.
        ''' The Help application displays a pop-up window that typically contains help for the child window.
        ''' WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
        ''' </summary>
        WS_EX_CONTEXTHELP = &H400

        ''' <summary>
        ''' Specifies a window which contains child windows that should take part in dialog box navigation.
        ''' If this style is specified, the dialog manager recurses into children of this window when performing navigation operations
        ''' such as handling the TAB key, an arrow key, or a keyboard mnemonic.
        ''' </summary>
        WS_EX_CONTROLPARENT = &H10000

        ''' <summary>Specifies a window that has a double border.</summary>
        WS_EX_DLGMODALFRAME = &H1

        ''' <summary>
        ''' Specifies a window that is a layered window.
        ''' This cannot be used for child windows or if the window has a class style of either CS_OWNDC or CS_CLASSDC.
        ''' </summary>
        WS_EX_LAYERED = &H80000

        ''' <summary>
        ''' Specifies a window with the horizontal origin on the right edge. Increasing horizontal values advance to the left.
        ''' The shell language must support reading-order alignment for this to take effect.
        ''' </summary>
        WS_EX_LAYOUTRTL = &H400000

        ''' <summary>Specifies a window that has generic left-aligned properties. This is the default.</summary>
        WS_EX_LEFT = &H0

        ''' <summary>
        ''' Specifies a window with the vertical scroll bar (if present) to the left of the client area.
        ''' The shell language must support reading-order alignment for this to take effect.
        ''' </summary>
        WS_EX_LEFTSCROLLBAR = &H4000

        ''' <summary>
        ''' Specifies a window that displays text using left-to-right reading-order properties. This is the default.
        ''' </summary>
        WS_EX_LTRREADING = &H0

        ''' <summary>
        ''' Specifies a multiple-document interface (MDI) child window.
        ''' </summary>
        WS_EX_MDICHILD = &H40

        ''' <summary>
        ''' Specifies a top-level window created with this style does not become the foreground window when the user clicks it.
        ''' The system does not bring this window to the foreground when the user minimizes or closes the foreground window.
        ''' The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the WS_EX_APPWINDOW style.
        ''' To activate the window, use the SetActiveWindow or SetForegroundWindow function.
        ''' </summary>
        WS_EX_NOACTIVATE = &H8000000

        ''' <summary>
        ''' Specifies a window which does not pass its window layout to its child windows.
        ''' </summary>
        WS_EX_NOINHERITLAYOUT = &H100000

        ''' <summary>
        ''' Specifies that a child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed.
        ''' </summary>
        WS_EX_NOPARENTNOTIFY = &H4

        ''' <summary>
        ''' The window does not render to a redirection surface.
        ''' This is for windows that do not have visible content or that use mechanisms other than surfaces to provide their visual.
        ''' </summary>
        WS_EX_NOREDIRECTIONBITMAP = &H200000

        ''' <summary>Specifies an overlapped window.</summary>
        WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE Or WS_EX_CLIENTEDGE

        ''' <summary>Specifies a palette window, which is a modeless dialog box that presents an array of commands.</summary>
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE Or WS_EX_TOOLWINDOW Or WS_EX_TOPMOST

        ''' <summary>
        ''' Specifies a window that has generic "right-aligned" properties. This depends on the window class.
        ''' The shell language must support reading-order alignment for this to take effect.
        ''' Using the WS_EX_RIGHT style has the same effect as using the SS_RIGHT (static), ES_RIGHT (edit), and BS_RIGHT/BS_RIGHTBUTTON (button) control styles.
        ''' </summary>
        WS_EX_RIGHT = &H1000

        ''' <summary>Specifies a window with the vertical scroll bar (if present) to the right of the client area. This is the default.</summary>
        WS_EX_RIGHTSCROLLBAR = &H0

        ''' <summary>
        ''' Specifies a window that displays text using right-to-left reading-order properties.
        ''' The shell language must support reading-order alignment for this to take effect.
        ''' </summary>
        WS_EX_RTLREADING = &H2000

        ''' <summary>Specifies a window with a three-dimensional border style intended to be used for items that do not accept user input.</summary>
        WS_EX_STATICEDGE = &H20000

        ''' <summary>
        ''' Specifies a window that is intended to be used as a floating toolbar.
        ''' A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font.
        ''' A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB.
        ''' If a tool window has a system menu, its icon is not displayed on the title bar.
        ''' However, you can display the system menu by right-clicking or by typing ALT+SPACE.
        ''' </summary>
        WS_EX_TOOLWINDOW = &H80

        ''' <summary>
        ''' Specifies a window that should be placed above all non-topmost windows and should stay above them, even when the window is deactivated.
        ''' To add or remove this style, use the SetWindowPos function.
        ''' </summary>
        WS_EX_TOPMOST = &H8

        ''' <summary>
        ''' Specifies a window that should not be painted until siblings beneath the window (that were created by the same thread) have been painted.
        ''' The window appears transparent because the bits of underlying sibling windows have already been painted.
        ''' To achieve transparency without these restrictions, use the SetWindowRgn function.
        ''' </summary>
        WS_EX_TRANSPARENT = &H20

        ''' <summary>Specifies a window that has a border with a raised edge.</summary>
        WS_EX_WINDOWEDGE = &H100
    End Enum
    <DllImport("user32.dll")>
    Public Function GetWindowRect(ByVal hWnd As IntPtr, ByRef lpRect As Rectangle) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function GetClientRect(ByVal hWnd As IntPtr, ByRef lpRect As Rectangle) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function ClientToScreen(ByVal hWnd As IntPtr, ByRef lpPoint As Point) As Boolean : End Function
    <DllImport("user32.dll", SetLastError:=True)>
    Public Function ScreenToClient(ByVal hWnd As IntPtr, ByRef lpPoint As Point) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function GetForegroundWindow() As IntPtr : End Function
    <DllImport("user32.dll")>
    Public Function GetWindowThreadProcessId(ByVal hWnd As IntPtr, <Out()> ByRef lpdwProcessId As UInteger) As UInteger : End Function
#Region " SetWindowPos "

    Enum SWP_HWND As Integer
        ''' <summary>
        ''' 1 Places the window at the bottom Of the Z order. If the hWnd parameter identifies a topmost window, the window loses its topmost status And Is placed at the bottom Of all other windows.
        ''' </summary>
        BOTTOM = 1
        ''' <summary> 
        ''' -2 Places the window above all non-topmost windows (that Is, behind all topmost windows). This flag has no effect If the window Is already a non-topmost window.
        ''' </summary>
        NOTOPMOST = -2
        ''' <summary>
        ''' 0 Places the window at the top Of the Z order. 
        ''' </summary>
        TOP = 0
        ''' <summary>
        ''' -1 Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated. 
        ''' </summary>
        TOPMOST = -1
    End Enum

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

    <DllImport("user32.dll", SetLastError:=True)>
    Public Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As IntPtr,
                                 ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer,
                                 ByVal uFlags As SetWindowPosFlags) As Boolean
    End Function
#End Region

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Public Structure SHSTOCKICONINFO
        Public cbSize As UInteger
        Public hIcon As IntPtr
        Public iSysIconIndex As Integer
        Public iIcon As Integer
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)>
        Public szPath As String
    End Structure
    <DllImport("shell32.dll", SetLastError:=True)>
    Public Function SHGetStockIconInfo(ssid As UInteger, uFlags As UInteger, ByRef pssi As SHSTOCKICONINFO) As Integer : End Function


    <StructLayout(LayoutKind.Sequential)>
    Structure PT
        Public x As Int32
        Public y As Int32
    End Structure
    <StructLayout(LayoutKind.Sequential)>
    Structure CURSORINFO
        Public cbSize As Int32
        Public flags As Int32
        Public hCursor As IntPtr
        Public ptScreenpos As PT
    End Structure
    <DllImport("user32.dll")>
    Public Function GetCursorInfo(ByRef pci As CURSORINFO) As Boolean : End Function



    <DllImport("user32.dll")>
    Public Function GetSystemMenu(ByVal hwnd As IntPtr, ByVal bRevert As Boolean) As Integer : End Function
    <DllImport("user32.dll")>
    Public Function ModifyMenuA(hMenu As Integer, uItem As Integer, fByPos As Integer, newID As Integer, lpNewIem As String) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function SetMenuItemBitmaps(hMenu As Integer, uitem As Integer, fByPos As Integer, hBitmapUnchecked As Integer, hBitmapChecked As Integer) As Boolean : End Function
    <DllImport("gdi32.dll")>
    Public Function DeleteObject(hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean : End Function
    <DllImport("user32.dll")>
    Public Function InsertMenuA(ByVal hMenu As IntPtr, ByVal nPosition As Integer, ByVal wFlags As Integer, uIDNewItem As Integer, lpNewItem As String) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function RemoveMenu(ByVal hMenu As IntPtr, ByVal nPosition As Integer, ByVal wFlags As Integer) As Integer : End Function
    <DllImport("user32.dll")>
    Public Function SetMenuDefaultItem(hMenu As IntPtr, uItem As Integer, fByPos As Integer) As Boolean : End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Function GetMenuItemInfo(hMenu As IntPtr, uItem As UInt32, fByPosition As Boolean, ByRef lpmii As MENUITEMINFO) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function SetMenuItemInfo(hMenu As IntPtr, uItem As UInt32, fByPosition As Boolean, ByRef lpmii As MENUITEMINFO) As Boolean : End Function
    Public Structure MENUITEMINFO
        Dim cbSize As Integer
        Dim fMask As Integer
        Dim fType As Integer
        Dim fState As Integer
        Dim wID As Integer
        Dim hSubMenu As Integer
        Dim hbmpChecked As IntPtr
        Dim hbmpUnchecked As IntPtr
        Dim dwItemData As Integer
        Dim dwTypeData As String
        Dim cch As Integer
        Dim hbmpItem As IntPtr
    End Structure
    Public Enum MFS As Long
        ''' <summary>
        '''Checks the menu item. For more information about selected menu items, see the hbmpChecked member.
        ''' </summary>
        CHECKED = &H8L
        ''' <summary>
        ''' Specifies that the menu item Is the Default. A menu can contain only one Default menu item, which Is displayed In bold.
        ''' </summary>
        [DEFAULT] = &H1000L
        ''' <summary>
        ''' Disables the menu item And grays it so that it cannot be selected. This Is equivalent To MFS_GRAYED.
        ''' </summary>
        DISABLED = &H3L
        ''' <summary>
        ''' Enables the menu item so that it can be selected. This Is the Default state.
        ''' </summary>
        ENABLED = &H0L
        ''' <summary>
        ''' Disables the menu item And grays it so that it cannot be selected. This Is equivalent To MFS_DISABLED.
        ''' </summary>
        GRAYED = &H3L
        ''' <summary>
        ''' Highlights the menu item.
        ''' </summary>
        HILITE = &H80L
        ''' <summary>
        ''' Unchecks the menu item. For more information about clear menu items, see the hbmpChecked member.
        ''' </summary>
        UNCHECKED = &H0L
        ''' <summary>
        '''Removes the highlight from the menu item. This Is the Default state.  
        ''' </summary>
        UNHILITE = &H0L
    End Enum
    <Flags>
    Public Enum MIIM As Long
        ''' <summary>
        ''' Retrieves Or sets the hbmpItem member.
        ''' </summary>
        BITMAP = &H80
        ''' <summary>
        ''' Retrieves Or sets the hbmpChecked And hbmpUnchecked members.
        ''' </summary>
        CHECKMARKS = &H8
        ''' <summary>
        ''' Retrieves Or sets the dwItemData member.
        ''' </summary>
        DATA = &H20
        ''' <summary>
        ''' Retrieves Or sets the fType member.
        ''' </summary>
        FTYPE = &H100
        ''' <summary>
        ''' Retrieves Or sets the wID member.
        ''' </summary>
        ID = &H2
        ''' <summary>
        ''' Retrieves Or sets the fState member.
        ''' </summary>
        STATE = &H1
        ''' <summary>
        ''' Retrieves Or sets the dwTypeData member.
        ''' </summary>
        [STRING] = &H40
        ''' <summary>
        ''' Retrieves Or sets the hSubMenu member.
        ''' </summary>
        SUBMENU = &H4
        ''' <summary>
        ''' Retrieves Or sets the fType And dwTypeData members.
        ''' MIIM_TYPE Is replaced by MIIM_BITMAP, MIIM_FTYPE, And MIIM_STRING.
        ''' </summary>
        TYPE = &H10
    End Enum

    'Const MF_STRING = &H0
    'Const MF_REMOVE = &H1000&

    Public Const MF_BYCOMMAND = &H0
    Public Const MF_BYPOSITION = &H400
    Public Const MF_SEPARATOR = &H800

    Public Const HTCAPTION As Integer = 2

    Public Const WM_MOVE = &H3
    Public Const WM_SIZE = &H5

    Public Const WM_WININICHANGE = &H1A

    Public Const WM_CANCELMODE = &H1F
    Public Const WM_SETCURSOR = &H20

    Public Const WM_WINDOWPOSCHANGING = &H46
    Public Const WM_WINDOWPOSCHANGED = &H47

    Public Const WM_NCHITTEST As Integer = &H84
    Public Const WM_NCLBUTTONDOWN As Integer = &HA1
    Public Const WM_NCLBUTTONUP As Integer = &HA2

    Public Const WM_SYSCOMMAND = &H112
    'Public Const WM_MENUSELECT = &H11F

    'Public Const WM_ENTERIDLE = &H121

    Public Const WM_MOUSEMOVE = &H200

    Public Const WM_RBUTTONDOWN = &H204
    Public Const WM_RBUTTONUP = &H205

    Public Const WM_MBUTTONDOWN = &H207
    Public Const WM_MBUTTONUP = &H208

    Public Const WM_ENTERMENULOOP = &H211
    Public Const WM_EXITMENULOOP = &H212
    Public Const WM_EXITSIZEMOVE = &H232

    Public Const SC_SIZE As Integer = &HF000
    Public Const SC_MOVE As Integer = &HF010
    Public Const SC_MINIMIZE As Integer = &HF020
    Public Const SC_MAXIMIZE As Integer = &HF030
    Public Const SC_CLOSE As Integer = &HF060
    Public Const SC_RESTORE As Integer = &HF120

    Public Const TPM_RECURSE = &H1
    Public Const TPM_RIGHTBUTTON = &H2
    Public Const TPM_RETURNCMD = &H100

    <DllImport("User32.Dll")>
    Public Function TrackPopupMenuEx(ByVal hmenu As IntPtr, ByVal fuFlags As UInteger, ByVal x As Integer, ByVal y As Integer, ByVal hwnd As IntPtr, ByVal lptpm As Integer) As Integer : End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As IntPtr) As IntPtr : End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Function PostMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As IntPtr) As Boolean : End Function

End Module
