Imports System.Runtime.InteropServices
Imports System.Text

Module NativeMethods

    <DllImport("mpr.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Public Function WNetGetConnection(<MarshalAs(UnmanagedType.LPTStr)> localName As String,
        <MarshalAs(UnmanagedType.LPTStr)> remoteName As Text.StringBuilder, ByRef length As Integer) As Integer : End Function

    Public Const SHGFI_ICON As Integer = &H100
    Public Const SHGFI_LARGEICON As Integer = &H0
    Public Const SHGFI_SMALLICON As Integer = &H1
    Public Const SHGFI_SHELLICONSIZE As Integer = &H4
    Public Const SHGFI_USEFILEATTRIBUTES As Integer = &H10
    Public Const SHGFI_ICONLOCATION As Integer = &H1000

    Public Const SHGFI_SYSICONINDEX As Integer = &H4000


    Public Const FILE_ATTRIBUTE_NORMAL As UInteger = &H80
    Public Const FILE_ATTRIBUTE_DIRECTORY As UInteger = &H10

    <DllImport("user32.dll", CharSet:=CharSet.Auto, ExactSpelling:=True)>
    Public Function MonitorFromPoint(pt As Point, dwFlags As MONITORFLAGS) As IntPtr : End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Public Function GetMonitorInfo(hmonitor As IntPtr, ByRef info As MonitorInfo) As Boolean : End Function

    Public Enum MONITORFLAGS As UInt32
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

    <DllImport("user32.dll", SetLastError:=True)>
    Public Function SystemParametersInfo(uiAction As SPI, uiParam As UInteger,
        ByRef pvParam As IntPtr, fWinIni As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    Enum SPI As UInteger
        GETCLIENTAREAANIMATION = &H1042
        GETFOREGROUNDLOCKTIMEOUT = &H2000
        SETFOREGROUNDLOCKTIMEOUT = &H2001
    End Enum

    <DllImport("user32.dll")>
    Public Function FlashWindow(hwnd As IntPtr, <MarshalAs(UnmanagedType.Bool)> bInvert As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean : End Function

    <Runtime.CompilerServices.Extension>
    Public Function ToRECT(rct As Rectangle) As RECT
        Return New RECT(rct)
    End Function

    <System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)>
    Public Structure RECT
        Public left, top, right, bottom As Integer
        Public Sub New(left As Integer, top As Integer, right As Integer, bottom As Integer)
            Me.left = left
            Me.top = top
            Me.right = right
            Me.bottom = bottom
        End Sub
        Public Sub New(ByVal rct As Rectangle)
            Me.New(rct.Left, rct.Top, rct.Right, rct.Bottom)
        End Sub
        Public Function ToRectangle() As Rectangle
            Return Rectangle.FromLTRB(Me.left, Me.top, Me.right, Me.bottom)
        End Function
        Public Overrides Function ToString() As String
            Return $"{{{Me.left},{Me.top},{Me.right},{Me.bottom}}}"
        End Function
    End Structure

    Public Const IDLE_PRIORITY_CLASS As Integer = &H40
    Public Const BELOW_NORMAL_PRIORITY_CLASS As Integer = &H4000
    Public Const NORMAL_PRIORITY_CLASS As Integer = &H20
    Public Const ABOVE_NORMAL_PRIORITY_CLASS As Integer = &H8000
    Public Const HIGH_PRIORITY_CLASS As Integer = &H80
    Public Const REALTIME_PRIORITY_CLASS As Integer = &H100

    <DllImport("kernel32.dll", SetLastError:=True)>
    Public Function SetPriorityClass(hProcess As IntPtr, dwPriorityClass As UInteger) As Boolean
    End Function
    <DllImport("kernel32.dll")>
    Public Function GetCurrentThread() As IntPtr : End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Public Function SetThreadPriority(hThread As IntPtr, nPriority As Integer) As Boolean : End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Public Function GetCurrentProcess() As IntPtr
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Public Structure GUITHREADINFO
        Public cbSize As UInteger
        Public flags As UInteger
        Public hwndActive As IntPtr
        Public hwndFocus As IntPtr
        Public hwndCapture As IntPtr
        Public hwndMenuOwner As IntPtr
        Public hwndMoveSize As IntPtr
        Public hwndCaret As IntPtr
        Public rcCaret As RECT
    End Structure

    <DllImport("user32.dll")>
    Public Function GetGUIThreadInfo(idThread As UInteger, ByRef lpgui As GUITHREADINFO) As Boolean : End Function


    <StructLayout(LayoutKind.Explicit)>
    Public Structure LParamMap
        Public Sub New(value As IntPtr)
            lparam = value
        End Sub

        Public Sub New(lo As Short, hi As Short)
            Me.loword = lo
            Me.hiword = hi
        End Sub

        Public Sub New(pt As Point)
            Me.loword = pt.X
            Me.hiword = pt.Y
        End Sub

        Public Shared Widening Operator CType(value As LParamMap) As Point
            Return New Point(value.loword, value.hiword)
        End Operator
        Public Shared Widening Operator CType(value As LParamMap) As Size
            Return New Size(value.loword, value.hiword)
        End Operator
        Public Shared Widening Operator CType(value As LParamMap) As IntPtr
            Return value.lparam
        End Operator
        Public Shared Narrowing Operator CType(value As LParamMap) As Byte
            Return value.scan
        End Operator


        <FieldOffset(0)> Public lparam As IntPtr
        <FieldOffset(0)> Public loword As Short
        <FieldOffset(2)> Public hiword As Short
        <FieldOffset(2)> Public scan As Byte
    End Structure


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
        Public dwAttributes As UInteger
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
        Public flags As SetWindowPosFlags
    End Structure
    <StructLayout(LayoutKind.Sequential)>
    Structure NCCALCSIZE_PARAMS
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)>
        Public rgrc As RECT()
        Public lppos As WINDOWPOS
    End Structure

    <DllImport("shell32.dll", EntryPoint:="SHGetFileInfoW", SetLastError:=True)>
    Public Function SHGetFileInfoW(<InAttribute(), MarshalAs(UnmanagedType.LPTStr)> ByVal pszPath As String, ByVal dwFileAttributes As Integer, ByRef psfi As SHFILEINFOW, ByVal cbFileInfo As Integer, ByVal uFlags As Integer) As IntPtr
    End Function
    Public ASFW_ANY As UInteger = &HFFFFFFFFUI
    <DllImport("user32.dll", SetLastError:=True)>
    Public Function AllowSetForegroundWindow(dwProcessId As UInt32) As Integer : End Function

    Public Declare Function ExtractIcon Lib "shell32.dll" Alias "ExtractIconA" (ByVal hInst As IntPtr, ByVal lpszExeFileName As String, ByVal nIconIndex As Integer) As IntPtr
    <DllImport("shell32.dll", CharSet:=CharSet.Auto)>
    Public Function ExtractIconEx(ByVal lpszFile As String, ByVal nIconIndex As Integer, ByVal phiconLarge() As IntPtr, ByVal phiconSmall() As IntPtr, ByVal nIcons As Integer) As Integer : End Function

    <DllImport("user32.dll", EntryPoint:="DestroyIcon")>
    Public Function DestroyIcon(ByVal hIcon As System.IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("comctl32.dll", SetLastError:=True)>
    Public Function ImageList_GetIcon(hIml As IntPtr, index As Integer, flags As UInteger) As IntPtr
    End Function

    <DllImport("comctl32.dll", SetLastError:=True)>
    Public Function ImageList_Destroy(hIml As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=Runtime.InteropServices.CharSet.Auto)>
    Public Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr : End Function


    ''' <summary>
    ''' note: this may need GetAncestor(hWnd, GA_ROOT) since it can return child controls
    ''' </summary>
    ''' <param name="pt"></param>
    ''' <returns></returns>
    <DllImport("user32.dll", SetLastError:=True, CharSet:=Runtime.InteropServices.CharSet.Auto)>
    Public Function WindowFromPoint(pt As Point) As IntPtr : End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Public Function FindWindowEx(ByVal parentHandle As IntPtr,
                      ByVal childAfter As IntPtr,
                      ByVal lclassName As String,
                      ByVal windowTitle As String) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Public Function GetTopWindow(hwnd As IntPtr) As IntPtr : End Function

    <DllImport("user32.dll")>
    Public Function GetWindow(hWnd As IntPtr, uCmd As UInteger) As IntPtr : End Function

    Public Const GW_HWNDFIRST As UInteger = 0
    Public Const GW_HWNDLAST As UInteger = 1
    Public Const GW_HWNDNEXT As UInteger = 2
    Public Const GW_HWNDPREV As UInteger = 3
    Public Const GW_OWNER As UInteger = 4
    Public Const GW_CHILD As UInteger = 5
    Public Const GW_ENABLEDPOPUP As UInteger = 6

    'Public Declare Function GetWindowThreadProcessId Lib "user32.dll" (
    'ByVal hWnd As IntPtr,
    'ByRef lpdwProcessId As Integer) As Integer

    Public Declare Function AttachThreadInput Lib "user32.dll" (
    ByVal idAttach As Integer,
    ByVal idAttachTo As Integer,
    ByVal fAttach As Boolean) As Boolean


    <DllImport("user32.dll", SetLastError:=True)>
    Public Function IsWindowVisible(ByVal hWnd As IntPtr) As Boolean : End Function


    <DllImport("user32.dll")>
    Public Function RedrawWindow(hWnd As IntPtr, lprcUpdate As IntPtr, hrgnUpdate As IntPtr, flags As RedrawWindowFlags) As Boolean : End Function

    <Flags()>
    Public Enum RedrawWindowFlags As UInteger
        ''' <summary>
        ''' Invalidates the rectangle or region that you specify in lprcUpdate or hrgnUpdate.
        ''' You can set only one of these parameters to a non-NULL value. If both are NULL, RDW_INVALIDATE invalidates the entire window.
        ''' </summary>
        Invalidate = &H1

        ''' <summary>Causes the OS to post a WM_PAINT message to the window regardless of whether a portion of the window is invalid.</summary>
        InternalPaint = &H2

        ''' <summary>
        ''' Causes the window to receive a WM_ERASEBKGND message when the window is repainted.
        ''' Specify this value in combination with the RDW_INVALIDATE value; otherwise, RDW_ERASE has no effect.
        ''' </summary>
        [Erase] = &H4

        ''' <summary>
        ''' Validates the rectangle or region that you specify in lprcUpdate or hrgnUpdate.
        ''' You can set only one of these parameters to a non-NULL value. If both are NULL, RDW_VALIDATE validates the entire window.
        ''' This value does not affect internal WM_PAINT messages.
        ''' </summary>
        Validate = &H8

        NoInternalPaint = &H10

        ''' <summary>Suppresses any pending WM_ERASEBKGND messages.</summary>
        NoErase = &H20

        ''' <summary>Excludes child windows, if any, from the repainting operation.</summary>
        NoChildren = &H40

        ''' <summary>Includes child windows, if any, in the repainting operation.</summary>
        AllChildren = &H80

        ''' <summary>Causes the affected windows, which you specify by setting the RDW_ALLCHILDREN and RDW_NOCHILDREN values, to receive WM_ERASEBKGND and WM_PAINT messages before the RedrawWindow returns, if necessary.</summary>
        UpdateNow = &H100

        ''' <summary>
        ''' Causes the affected windows, which you specify by setting the RDW_ALLCHILDREN and RDW_NOCHILDREN values, to receive WM_ERASEBKGND messages before RedrawWindow returns, if necessary.
        ''' The affected windows receive WM_PAINT messages at the ordinary time.
        ''' </summary>
        EraseNow = &H200

        Frame = &H400

        NoFrame = &H800
    End Enum

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


    Public Const SW_HIDE = 0
    Public Const SW_SHOWNORMAL = 1
    Public Const SW_NORMAL = 1
    Public Const SW_SHOWMINIMIZED = 2
    Public Const SW_SHOWMAXIMIZED = 3
    Public Const SW_MAXIMIZE = 3
    Public Const SW_SHOWNOACTIVATE = 4
    Public Const SW_SHOW = 5
    Public Const SW_MINIMIZE = 6
    Public Const SW_SHOWMINNOACTIVE = 7
    Public Const SW_SHOWNA = 8
    Public Const SW_RESTORE = 9
    Public Const SW_SHOWDEFAULT = 10
    Public Const SW_FORCEMINIMIZE = 11

    Public Const SW_OTHERUNZOOM = 4   'The window Is being uncovered because a maximize window was restored Or minimized.
    Public Const SW_OTHERZOOM = 2     'The window Is being covered by another window that has been maximized.
    Public Const SW_PARENTCLOSING = 1 'The window's owner window is being minimized.
    Public Const SW_PARENTOPENING = 3 'The window's owner window is being restored.

    <DllImport("Shell32", CharSet:=CharSet.Auto, SetLastError:=True)>
    Public Function ShellExecuteEx(ByRef lpExecInfo As SHELLEXECUTEINFO) As Boolean : End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Public Sub SwitchToThisWindow(hWnd As IntPtr, fAltTab As Boolean) : End Sub

    <DllImport("user32.dll", SetLastError:=False)>
    Public Function GetDesktopWindow() As IntPtr : End Function

    <DllImport("user32.dll")>
    Public Function LoadImage(hinst As IntPtr, lpszName As String, uType As UInt32, cxDesired As Integer, cyDesired As Integer, fuLoad As UInt32) As IntPtr : End Function
    <DllImport("user32.dll")>
    Public Function SetWindowLong(ByVal hwnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As UInteger) As Long : End Function
    <DllImport("user32.dll")>
    Public Function GetWindowLong(ByVal hwnd As IntPtr, ByVal nIndex As Integer) As UInteger : End Function

    <System.Runtime.InteropServices.DllImport("user32.dll", CharSet:=System.Runtime.InteropServices.CharSet.Auto)>
    Public Function GetClassName(ByVal hWnd As System.IntPtr, ByVal lpClassName As System.Text.StringBuilder, ByVal nMaxCount As Integer) As Integer : End Function
    Public Function GetWindowClass(ByVal hwnd As IntPtr) As String
        Static sClassName As New System.Text.StringBuilder("", 256)
        Call GetClassName(hwnd, sClassName, 256 - 1)
        Return sClassName.ToString
    End Function

    Public Const GWL_HWNDPARENT As Integer = -8
    Public Const GWL_STYLE As Integer = -16
    Public Const GWL_EXSTYLE As Integer = -20

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Public Function EnumWindows(lpEnumFunc As EnumWindowsProc, lParam As IntPtr) As Boolean : End Function
    Public Delegate Function EnumWindowsProc(hWnd As IntPtr, lParam As IntPtr) As Boolean

    <DllImport("user32.dll")>
    Public Function EnumThreadWindows(dwThreadId As UInteger, lpfn As EnumThreadDelegate, lParam As IntPtr) As Boolean : End Function
    Public Delegate Function EnumThreadDelegate(hWnd As IntPtr, lParam As IntPtr) As Boolean

    Public Declare Function RedrawWindow Lib "user32.dll" (
    ByVal hWnd As IntPtr,
    ByRef lprcUpdate As RECT,
    ByVal hrgnUpdate As IntPtr,
    ByVal flags As UInteger
) As Boolean


    Public Const GA_PARENT As UInteger = 1
    Public Const GA_ROOT As UInteger = 2
    Public Const GA_ROOTOWNER As UInteger = 3

    <DllImport("user32.dll", SetLastError:=False)>
    Public Function GetAncestor(hWnd As IntPtr, gaFlags As UInteger) As IntPtr : End Function


    ' RedrawWindow flags
    Public Const RDW_INVALIDATE As UInteger = &H1
    Public Const RDW_INTERNALPAINT As UInteger = &H2
    Public Const RDW_ERASE As UInteger = &H4
    Public Const RDW_VALIDATE As UInteger = &H8
    Public Const RDW_NOINTERNALPAINT As UInteger = &H10
    Public Const RDW_NOERASE As UInteger = &H20
    Public Const RDW_NOCHILDREN As UInteger = &H40
    Public Const RDW_ALLCHILDREN As UInteger = &H80
    Public Const RDW_UPDATENOW As UInteger = &H100
    Public Const RDW_ERASENOW As UInteger = &H200
    Public Const RDW_FRAME As UInteger = &H400
    Public Const RDW_NOFRAME As UInteger = &H800

    <StructLayout(LayoutKind.Sequential)>
    Public Structure StyleStruct
        Public styleOld As UInteger
        Public styleNew As UInteger
    End Structure

    <Flags()>
    Public Enum WindowStyles As UInteger
        ''' <summary>
        ''' The window has a thin-line border.
        ''' </summary>
        WS_BORDER = &H800000

        ''' <summary>
        ''' The window has a title bar (includes the WS_BORDER style).
        ''' </summary>
        WS_CAPTION = &HC00000

        ''' <summary>
        ''' The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the WS_POPUP style.
        ''' </summary>
        WS_CHILD = &H40000000

        ''' <summary>
        ''' Excludes the area occupied by child windows when drawing occurs within the parent window. This style is used when creating the parent window.
        ''' </summary>
        WS_CLIPCHILDREN = &H2000000

        ''' <summary>
        ''' Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message, the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child window to be updated. If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, when drawing within the client area of a child window, to draw within the client area of a neighboring child window.
        ''' </summary>
        WS_CLIPSIBLINGS = &H4000000

        ''' <summary>
        ''' The window is initially disabled. A disabled window cannot receive input from the user.
        ''' </summary>
        WS_DISABLED = &H8000000

        ''' <summary>
        ''' The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.
        ''' </summary>
        WS_DLGFRAME = &H400000

        ''' <summary>
        ''' The window is the first control of a group of controls. The group consists of this first control and all controls defined after it, up to the next control with the WS_GROUP style. The first control in each group usually has the WS_TABSTOP style so that the user can move from group to group. The user can subsequently change the keyboard focus from one control in the group to the next control in the group by using the direction keys.
        ''' </summary>
        WS_GROUP = &H20000

        ''' <summary>
        ''' The window has a horizontal scroll bar.
        ''' </summary>
        WS_HSCROLL = &H100000

        ''' <summary>
        ''' The window is initially maximized.
        ''' </summary>
        WS_MAXIMIZE = &H1000000

        ''' <summary>
        ''' The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
        ''' </summary>
        WS_MAXIMIZEBOX = &H10000

        ''' <summary>
        ''' The window is initially minimized.
        ''' </summary>
        WS_MINIMIZE = &H20000000

        ''' <summary>
        ''' The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
        ''' </summary>
        WS_MINIMIZEBOX = &H20000

        ''' <summary>
        ''' The window is an overlapped window. An overlapped window has a title bar and a border. Same as the WS_TILED style.
        ''' </summary>
        WS_OVERLAPPED = &H0

        ''' <summary>
        ''' The window is an overlapped window. Same as the WS_TILEDWINDOW style.
        ''' </summary>
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED Or WS_CAPTION Or WS_SYSMENU Or WS_SIZEFRAME Or WS_MINIMIZEBOX Or WS_MAXIMIZEBOX

        ''' <summary>
        ''' The window is a pop-up window. This style cannot be used with the WS_CHILD style.
        ''' </summary>
        WS_POPUP = &H80000000UI

        ''' <summary>
        ''' The window is a pop-up window. The WS_BORDER, WS_POPUP, and WS_SYSMENU styles must be combined to make the window menu visible.
        ''' </summary>
        WS_POPUPWINDOW = WS_POPUP Or WS_BORDER Or WS_SYSMENU

        ''' <summary>
        ''' The window has a sizing border. Same as the WS_THICKFRAME style.
        ''' </summary>
        WS_SIZEFRAME = &H40000

        ''' <summary>
        ''' The window has a window menu on its title bar. The WS_CAPTION style must also be specified.
        ''' </summary>
        WS_SYSMENU = &H80000

        ''' <summary>
        ''' The window is a control that can receive the keyboard focus when the user presses the TAB key. Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style.
        ''' </summary>
        WS_TABSTOP = &H10000

        ''' <summary>
        ''' The window is initially visible. This style can be turned on and off by using the ShowWindow or SetWindowPos function.
        ''' </summary>
        WS_VISIBLE = &H10000000

        ''' <summary>
        ''' The window has a vertical scroll bar.
        ''' </summary>
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
    Public Function IsIconic(hWnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean : End Function
    <StructLayout(LayoutKind.Sequential)>
    Public Structure WINDOWPLACEMENT
        Public length As Integer
        Public flags As Integer
        Public showCmd As Integer
        Public minPosition As Point
        Public maxPosition As Point
        Public normalPosition As RECT
    End Structure
    <DllImport("user32.dll")>
    Public Function SetWindowPlacement(ByVal hWnd As IntPtr, ByRef lpwndpl As WINDOWPLACEMENT) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function GetWindowPlacement(ByVal hWnd As IntPtr, ByRef lpwndpl As WINDOWPLACEMENT) As Boolean : End Function

    <DllImport("user32.dll")>
    Public Function ShowWindow(Hwnd As IntPtr, iCmdShow As Integer) As Integer : End Function


    <DllImport("user32.dll")>
    Public Function InvalidateRect(hWnd As IntPtr, lpRect As IntPtr, bErase As Boolean) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function GetWindowRect(ByVal hWnd As IntPtr, ByRef lpRect As Rectangle) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function GetWindowRect(ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Boolean : End Function

    <DllImport("user32.dll")>
    Public Function GetClientRect(ByVal hWnd As IntPtr, ByRef lpRect As Rectangle) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function GetClientRect(ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function ClientToScreen(ByVal hWnd As IntPtr, ByRef lpPoint As Point) As Boolean : End Function
    <DllImport("user32.dll", SetLastError:=True)>
    Public Function ScreenToClient(ByVal hWnd As IntPtr, ByRef lpPoint As Point) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function GetForegroundWindow() As IntPtr : End Function
    <DllImport("user32.dll")>
    Public Function GetWindowThreadProcessId(ByVal hWnd As IntPtr, <Out()> ByRef lpdwProcessId As UInteger) As UInteger : End Function

    <DllImport("dwmapi.dll")>
    Public Function DwmGetWindowAttribute(hwnd As IntPtr, dwAttribute As Integer, ByRef pvAttribute As RECT, cbAttribute As Integer) As Integer : End Function
    <DllImport("user32.dll")>
    Public Function SetParent(hWndChild As IntPtr, hWndNewParent As IntPtr) As IntPtr : End Function
    'Public Declare Function DwmGetWindowAttribute Lib "dwmapi" (ByVal hwnd As IntPtr, ByVal dwAttribute As Integer, ByRef pvAttribute As RECT, ByVal cbAttribute As Integer) As Integer

    <DllImport("gdi32.dll")>
    Public Function GetDeviceCaps(ByVal hDC As IntPtr, ByVal nIndex As Integer) As Integer : End Function
    <DllImport("user32.dll")>
    Public Function GetDC(ByVal hwnd As IntPtr) As IntPtr : End Function
    <DllImport("user32.dll")>
    Public Function ReleaseDC(ByVal hWnd As IntPtr, ByVal hDC As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean : End Function
    <DllImport("user32.dll", SetLastError:=True)>
    Public Function SetLayeredWindowAttributes(hWnd As IntPtr, crKey As UInteger, bAlpha As Byte, dwFlags As UInteger) As Boolean : End Function
    Public Const LWA_COLORKEY As Integer = &H1
    Public Const LWA_ALPHA As Integer = &H2
    <DllImport("user32.dll", SetLastError:=True)>
    Public Function SetForegroundWindow(hWnd As IntPtr) As Boolean : End Function

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
        ''' <summary>Undocumented</summary>
        ''' <remarks>SWP_NOCLIENTSIZE</remarks>
        NoClientSize = &H800
        ''' <summary>Undocumented</summary>
        ''' <remarks>SWP_NOCLIENTMOVE</remarks>
        NoClientMove = &H1000
        ''' <summary>Undocumented</summary>
        ''' <remarks>SWP_STATECHANGED</remarks>
        StateChanged = &H8000
    End Enum

    <DllImport("user32.dll", SetLastError:=True)>
    Public Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As IntPtr,
                                 ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer,
                                 ByVal uFlags As SetWindowPosFlags) As Boolean
    End Function
    <DllImport("user32.dll", SetLastError:=True)>
    Public Function BeginDeferWindowPos(nNumWindows As Integer) As IntPtr : End Function
    <DllImport("User32.dll")>
    Public Function DeferWindowPos(hWinPosInfo As IntPtr, hWnd As IntPtr, hWndInsertAfter As IntPtr, X As Integer, Y As Integer,
                                   cx As Integer, cy As Integer, uFlags As SetWindowPosFlags) As Boolean : End Function
    <DllImport("user32.dll", SetLastError:=True)>
    Public Function EndDeferWindowPos(hWinPosInfo As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean : End Function
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


    Public Declare Function BlockInput Lib "user32.dll" (fBlockIt As Boolean) As Boolean

    Public Declare Function SendInput Lib "user32.dll" (nInputs As Integer, pInputs() As INPUT, cbSize As Integer) As UInteger

    Public Enum InputType As UInteger
        INPUT_MOUSE
        INPUT_KEYBOARD
        INPUT_HARDWARE
    End Enum
    <StructLayout(LayoutKind.Explicit)>
    Public Structure InputUnion
        <FieldOffset(0)> Public mi As MOUSEINPUT
        <FieldOffset(0)> Public ki As KEYBDINPUT
        <FieldOffset(0)> Public hi As HARDWAREINPUT
    End Structure
    Public Structure MOUSEINPUT
        Public dx As Integer
        Public dy As Integer
        Public mouseData As Integer
        Public dwFlags As Integer
        Public time As Integer
        Public dwExtraInfo As IntPtr
    End Structure
    Public Structure KEYBDINPUT
        Public wVk As UShort
        Public wScan As UShort
        Public dwFlags As Integer
        Public time As Integer
        Public dwExtraInfo As IntPtr
    End Structure
    Public Structure HARDWAREINPUT
        Public uMsg As Integer
        Public wParamL As Short
        Public wParamH As Short
    End Structure
    Public Structure INPUT
        Public type As Integer
        Public u As InputUnion
    End Structure

    <Flags>
    Public Enum KeyEventF
        KeyDown = &H0
        ExtendedKey = &H1
        KeyUp = &H2
        Unicode = &H4
        Scancode = &H8
    End Enum
    <Flags>
    Public Enum MouseEventF
        Move = &H1
        LeftDown = &H2
        LeftUp = &H4
        RightDown = &H8
        RightUp = &H10
        MiddleDown = &H20
        MiddleUp = &H40
        XDown = &H80
        XUp = &H100
        Wheel = &H800
        HWheel = &H1000
        Move_NoCoalece = &H2000
        VirtualDesk = &H4000
        ABSOLUTE = &H8000
    End Enum
    <DllImport("user32.dll")>
    Public Function GetMessageExtraInfo() As IntPtr : End Function

    <StructLayout(LayoutKind.Sequential)>
    Public Structure PT
        Public x As Int32
        Public y As Int32
    End Structure
    <StructLayout(LayoutKind.Sequential)>
    Public Structure CURSORINFO
        Public cbSize As Int32
        Public flags As Int32
        Public hCursor As IntPtr
        Public ptScreenpos As PT
    End Structure
    <DllImport("user32.dll")>
    Public Function GetCursorInfo(ByRef pci As CURSORINFO) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function ShowCursor(bShow As Boolean) As Integer : End Function

    <DllImport("user32.dll")>
    Public Function GetMenuItemCount(hMenu As IntPtr) As Integer : End Function
    <DllImport("user32.dll")>
    Public Function SetMenuItemBitmaps(hMenu As IntPtr, uPosition As UInteger, uFlags As UInteger, hBitmapUnchecked As IntPtr, hBitmapChecked As IntPtr) As Integer : End Function
    <DllImport("user32.dll")>
    Public Function GetSystemMenu(ByVal hwnd As IntPtr, ByVal bRevert As Boolean) As Integer : End Function
    <DllImport("user32.dll")>
    Public Function ModifyMenuA(hMenu As Integer, uItem As Integer, fByPos As Integer, newID As UIntPtr, lpNewIem As String) As Boolean : End Function
    <DllImport("user32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Public Function ModifyMenuW(hMenu As IntPtr, uItem As UInteger, fByPos As UInteger, newID As UIntPtr, lpNewItem As String) As Boolean : End Function

    '<DllImport("user32.dll")>
    'Public Function SetMenuItemBitmaps(hMenu As Integer, uitem As Integer, fByPos As Integer, hBitmapUnchecked As Integer, hBitmapChecked As Integer) As Boolean : End Function
    <DllImport("gdi32.dll")>
    Public Function DeleteObject(hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean : End Function
    <DllImport("user32.dll")>
    Public Function InsertMenuA(ByVal hMenu As IntPtr, ByVal nPosition As Integer, ByVal wFlags As Integer, uIDNewItem As Integer, lpNewItem As String) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function RemoveMenu(ByVal hMenu As IntPtr, ByVal nPosition As Integer, ByVal wFlags As Integer) As Integer : End Function
    <DllImport("user32.dll")>
    Public Function DeleteMenu(hMenu As IntPtr, uPosition As UInteger, uFlags As UInteger) As Boolean : End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Function DestroyMenu(hMenu As IntPtr) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function SetMenuDefaultItem(hMenu As IntPtr, uItem As Integer, fByPos As Integer) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function GetMenuItemID(ByVal hMenu As IntPtr, ByVal nPos As Integer) As UInteger : End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Function GetMenuItemInfo(hMenu As IntPtr, uItem As UInt32, fByPosition As Boolean, ByRef lpmii As MENUITEMINFO) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function SetMenuItemInfo(hMenu As IntPtr, uItem As UInt32, fByPosition As Boolean, ByRef lpmii As MENUITEMINFO) As Boolean : End Function


    <DllImport("user32.dll")>
    Public Function ChildWindowFromPoint(ByVal hWndParent As IntPtr, ByVal pt As Point) As IntPtr : End Function

    <DllImport("ntdll.dll")>
    Public Function NtQueryInformationProcess(hProcess As IntPtr, processInformationClass As Integer, ByRef pbi As PROCESS_BASIC_INFORMATION, ByVal size As Integer, ByRef returnLength As Integer) As Integer : End Function

    <StructLayout(LayoutKind.Sequential)>
    Public Structure PROCESS_BASIC_INFORMATION
        Public Reserved1 As IntPtr
        Public PebBaseAddress As IntPtr
        Public Reserved2_0 As IntPtr
        Public Reserved2_1 As IntPtr
        Public UniqueProcessId As IntPtr
        Public InheritedFromUniqueProcessId As IntPtr
    End Structure

    Public Structure MENUITEMINFO
        Dim cbSize As Integer
        Dim fMask As Integer
        Dim fType As Integer
        Dim fState As Integer
        Dim wID As Integer
        Dim hSubMenu As IntPtr
        Dim hbmpChecked As IntPtr
        Dim hbmpUnchecked As IntPtr
        Dim dwItemData As Integer
        Dim dwTypeData As String
        Dim cch As Integer 'GetMenuItemInfo only?
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
    Public Const MF_GRAYED As UInteger = &H1
    Public Const MF_BITMAP As UInteger = &H4
    Public Const MF_OWNERDRAW As UInteger = &H100


    Public Const MK_LBUTTON = &H1   ' The left mouse button Is down.
    Public Const MK_RBUTTON = &H2   ' The right mouse button Is down.
    Public Const MK_SHIFT = &H4     ' The SHIFT key Is down.
    Public Const MK_CONTROL = &H8   ' The CTRL key Is down.
    Public Const MK_MBUTTON = &H10  ' The middle mouse button Is down.
    Public Const MK_XBUTTON1 = &H20 ' The first X button Is down.
    Public Const MK_XBUTTON2 = &H40 ' The second X button is down.

    Public Const HTCAPTION As Integer = 2
    Public Const HTMAXBUTTON As Integer = 9

    Public Const WM_MOVE = &H3
    Public Const WM_SIZE = &H5
    Public Const WM_ACTIVATE = &H6

    Public Const WM_CLOSE As Integer = &H10

    Public Const WM_GETTEXT As Integer = &HD
    Public Const WM_GETTEXTLENGTH As Integer = &HE
    Public Const WM_PAINT = &HF

    Public Const WM_QUERYOPEN = &H13
    Public Const WM_ERASEBKGND = &H14

    Public Const WM_SHOWWINDOW = &H18
    Public Const WM_SETTINGCHANGE = &H1A

    Public Const WM_ACTIVATEAPP = &H1C

    Public Const WM_CANCELMODE = &H1F
    Public Const WM_SETCURSOR = &H20

    Public Const WM_WINDOWPOSCHANGING = &H46
    Public Const WM_WINDOWPOSCHANGED = &H47

    Public Const WM_STYLECHANGING As Integer = &H7C

    Public Const WM_DISPLAYCHANGE = &H7E

    Public Const WM_NCHITTEST As Integer = &H84
    Public Const WM_NCACTIVATE As Integer = &H86

    Public Const WM_NCLBUTTONDOWN As Integer = &HA1
    Public Const WM_NCLBUTTONUP As Integer = &HA2

    Public Const BM_CLICK As Integer = &HF5

    Public Const WM_KEYDOWN As Integer = &H100
    Public Const WM_KEYUP = &H101
    Public Const WM_CHAR As Integer = &H102

    Public Const WM_SYSKEYDOWN = &H104

    Public Const WM_SYSCOMMAND = &H112

    Public Const WM_INITMENU = &H116
    Public Const WM_INITMENUPOPUP = &H117

    'Public Const WM_MENUSELECT = &H11F

    'Public Const WM_ENTERIDLE = &H121

    Public Const WM_MOUSEMOVE = &H200

    Public Const WM_LBUTTONDOWN = &H201
    Public Const WM_LBUTTONUP = &H202

    Public Const WM_RBUTTONDOWN = &H204
    Public Const WM_RBUTTONUP = &H205

    Public Const WM_MBUTTONDOWN = &H207
    Public Const WM_MBUTTONUP = &H208

    Public Const WM_MOUSEWHEEL = &H20A
    Public Const WM_XBUTTONDOWN = &H20B
    Public Const WM_XBUTTONUP = &H20C

    Public Const WM_ENTERMENULOOP = &H211
    Public Const WM_EXITMENULOOP = &H212

    Public Const WM_SIZING = &H214

    Public Const WM_ENTERSIZEMOVE = &H231
    Public Const WM_EXITSIZEMOVE = &H232

    Public Const WM_MOUSEHOVER = &H2A1

    Public Const WM_CLIPBOARDUPDATE As Integer = &H31D

    Public Const WM_DWMCOLORIZATIONCOLORCHANGED = &H320

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
    Public Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr : End Function
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Public Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As StringBuilder) As IntPtr : End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Public Function AddClipboardFormatListener(hwnd As IntPtr) As Boolean : End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Public Function RemoveClipboardFormatListener(hwnd As IntPtr) As Boolean : End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Public Function GetClipboardOwner() As IntPtr : End Function

    Public Const WM_SHNOTIFY As Integer = &H401
    Public Const SHCNE_ASSOCCHANGED As Integer = &H8000000
    Public Const SHCNRF_ShellLevel As Integer = &H2

    Public Declare Function GetAsyncKeyState Lib "user32.dll" (ByVal vKey As Keys) As Short

    <StructLayout(LayoutKind.Sequential)>
    Public Structure SHChangeNotifyEntry
        Public pIdl As IntPtr
        <MarshalAs(UnmanagedType.Bool)>
        Public fRecursive As Boolean
    End Structure

    <DllImport("shell32.dll", CharSet:=CharSet.Auto)>
    Public Function SHChangeNotifyRegister(
        hWnd As IntPtr,
        fSources As Integer,
        fEvents As Integer,
        wMsg As Integer,
        cEntries As Integer,
        ByRef pshcne As SHChangeNotifyEntry
    ) As Integer
    End Function
    <DllImport("shell32.dll")>
    Public Function SHChangeNotifyDeregister(hNotify As Integer) As Boolean : End Function

    Public Function GetWindowText(hWnd As IntPtr) As String
        Dim length As Integer = NativeMethods.SendMessage(hWnd, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero).ToInt32()
        If length > 0 Then
            Dim sb As New StringBuilder(length + 1)
            NativeMethods.SendMessage(hWnd, WM_GETTEXT, sb.Capacity, sb)
            Return sb.ToString()
        Else
            Return String.Empty
        End If
    End Function


    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Function PostMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Boolean : End Function

    Public Function WM_MOUSEMOVE_CreateWParam() As IntPtr
        Dim wp As Integer
        wp = wp Or ((Control.MouseButtons >> 20) And &H3)   ' 00000011 (Extract left and right buttons)
        wp = wp Or ((Control.ModifierKeys >> 14) And &H300) ' 00001100 (Extract Shift and Ctrl keys)
        wp = wp Or ((Control.MouseButtons >> 18) And &H70)  ' 01110000 (Extract middle and X buttons)
        Return New IntPtr(wp)
    End Function
    Public Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" (
    ByVal hwnd As IntPtr,
    ByVal lpOperation As String,
    ByVal lpFile As String,
    ByVal lpParameters As String,
    ByVal lpDirectory As String,
    ByVal nShowCmd As Integer) As Integer
    Public Enum FileAccess
        Read = &H80000000
        Write = &H40000000
        ReadWrite = &H40000000 Or &H80000000
    End Enum

    Public Enum FileShare
        None = 0
        Read = 1
        Write = 2
        ReadWrite = 3
    End Enum

    Public Enum CreationDisposition
        OpenExisting = 3
    End Enum

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Public Structure SecurityAttributes
        Public Length As Integer
        Public SecurityDescriptor As IntPtr
        Public InheritHandle As Boolean
    End Structure

    <System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True, CallingConvention:=CallingConvention.Winapi)>
    Public Function CreateFile(
    ByVal lpFileName As String,
    ByVal dwDesiredAccess As FileAccess,
    ByVal dwShareMode As FileShare,
    ByRef lpSecurityAttributes As SecurityAttributes,
    ByVal dwCreationDisposition As CreationDisposition,
    ByVal dwFlagsAndAttributes As Integer,
    ByVal hTemplateFile As IntPtr
) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Public Function MoveFileW(ExistingFileName As String, NewFileName As String) As Boolean : End Function

    <DllImport("Kernel32", SetLastError:=True, CharSet:=CharSet.Unicode)>
    Public Function FormatMessage(ByVal dwFlags As Format_Message, ByVal lpSource As IntPtr, ByVal dwMessageId As Integer, ByVal dwLanguageId As Integer, lpBuffer As Text.StringBuilder, ByVal nSize As Integer, ByVal Arguments As IntPtr) As Integer : End Function
    Enum Format_Message
        FORMAT_MESSAGE_IGNORE_INSERTS = &H200
        FORMAT_MESSAGE_FROM_SYSTEM = &H1000
        FORMAT_MESSAGE_FROM_HMODULE = &H800
    End Enum

    <System.Runtime.InteropServices.DllImport("kernel32.dll")>
    Public Function QueryFullProcessImageName(hprocess As IntPtr, dwFlags As Integer, lpExeName As System.Text.StringBuilder, ByRef size As Integer) As Boolean : End Function

    <System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Public Function GetFinalPathNameByHandleW(hFile As IntPtr, ByVal lpszFilePath As System.Text.StringBuilder, ByVal cchFilePath As Integer, ByVal dwFlags As UInteger) As Integer : End Function


    Enum ProcessAccessFlags As UInteger
        All = &H1FFFFF
        Terminate = &H1
        CreateThread = &H2
        VMOperation = &H8
        VMRead = &H10
        VMWrite = &H20
        DupHandle = &H40
        SetInformation = &H200
        QueryInformation = &H400
        QueryLimitedInformation = &H1000
        Synchronize = &H100000
    End Enum
    <System.Runtime.InteropServices.DllImport("kernel32.dll")>
    Public Function OpenProcess(dwDesiredAccess As ProcessAccessFlags, bInheritHandle As Boolean, dwProcessId As Integer) As IntPtr : End Function
    <System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError:=True)>
    Public Function CloseHandle(hHandle As IntPtr) As Boolean : End Function
    <System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError:=True)>
    Public Function GetExitCodeProcess(hHandle As IntPtr, ByRef eCode As Integer) As Boolean : End Function

#Region "Undocumented"

    <System.Runtime.InteropServices.DllImport("uxtheme.dll", EntryPoint:="#135", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Public Function SetPreferredAppMode(preferredAppMode As Integer) As Integer : End Function

    <System.Runtime.InteropServices.DllImport("uxtheme.dll", EntryPoint:="#136", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Public Function FlushMenuThemes() As Integer : End Function

#End Region



End Module
