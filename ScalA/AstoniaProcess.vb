Imports System.IO.MemoryMappedFiles
Imports System.Runtime.InteropServices

Public NotInheritable Class AstoniaProcess : Implements IDisposable

    Private ReadOnly _proc As Process

    'Public thumbID As IntPtr

    Public Sub New(Optional process As Process = Nothing)
        _proc = process
    End Sub

    Public Sub ResetCache()
        _rcc = Nothing
        _CO = Nothing
        _rcSource = Nothing
    End Sub

    Private _rcc? As Rectangle
    Public ReadOnly Property ClientRect() As Rectangle
        Get
            If _rcc IsNot Nothing AndAlso _rcc <> New Rectangle Then
                Return _rcc
            Else
                'Debug.Print("called get ClientRect")
                Dim rcc As New Rectangle
                If _proc Is Nothing Then Return New Rectangle
                GetClientRect(Me.MainWindowHandle, rcc)
                If rcc <> New Rectangle Then _rcc = rcc
                Return rcc
            End If
        End Get
    End Property
    Public ReadOnly Property WindowRect() As Rectangle
        Get
            Dim rcw As New Rectangle
            If _proc Is Nothing Then Return New Rectangle

            GetWindowRect(Me.MainWindowHandle, rcw)

            Return rcw
        End Get
    End Property
    Public Sub CloseOrKill()
        If _proc Is Nothing Then Exit Sub
        Try
            Dim thumb As IntPtr
            If FrmMain.startThumbsDict.TryGetValue(_proc.Id, thumb) Then
                DwmUnregisterThumbnail(thumb)
            End If
            Dim dummy = _proc.HasExited() 'test to see if proc is elevated
            _proc.CloseMainWindow()
        Catch ex As System.ComponentModel.Win32Exception
            Try
                _proc.Kill()
            Catch
            End Try
        End Try
    End Sub
    Public Function IsMinimized() As Boolean
        If _proc Is Nothing Then Return False
        Return IsIconic(Me.MainWindowHandle)
    End Function

    Public Function Restore() As Integer
        Dim ret As Integer = ShowWindow(Me.MainWindowHandle, SW_RESTORE)
        Me.ResetCache()
        Return ret
    End Function
    Public Function Hide() As Integer
        Return ShowWindow(Me.MainWindowHandle, SW_MINIMIZE)
    End Function

    Private _restoreLoc As Point? = Nothing
    Private _wasTopmost As Boolean = False
#Disable Warning IDE0140 ' Object creation can be simplified 'needs to be declared and assigned for parrallel.foreach
    Private Shared ReadOnly _restoreDic As Concurrent.ConcurrentDictionary(Of Integer, AstoniaProcess) = New Concurrent.ConcurrentDictionary(Of Integer, AstoniaProcess)
#Enable Warning IDE0140 ' Object creation can be simplified
    Public Shared Sub RestorePos(Optional keep As Boolean = False)
        Dim behind As IntPtr
        If IPC.SidebarSender IsNot Nothing AndAlso Not IPC.SidebarSender.HasExitedSafe Then
            behind = IPC.SidebarSenderhWnd
        Else
            IPC.SidebarSender = Nothing
            behind = FrmMain.ScalaHandle
        End If
        Parallel.ForEach(_restoreDic.Values,
                         Sub(ap As AstoniaProcess)
                             Try
                                 If ap._restoreLoc IsNot Nothing AndAlso Not ap.HasExited() Then
                                     Dim beh = If(ap._wasTopmost, SWP_HWND.TOPMOST, behind)
                                     SetWindowPos(ap.MainWindowHandle, beh,
                                                  ap._restoreLoc?.X, ap._restoreLoc?.Y, -1, -1,
                                                  SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate Or SetWindowPosFlags.DoNotChangeOwnerZOrder)
                                     If Not keep Then ap._restoreLoc = Nothing
                                 End If
                             Catch
                             End Try
                         End Sub)
        If Not keep Then _restoreDic.Clear()
    End Sub
    Public Sub RestoreSinglePos(Optional behind As Integer = 0)
        If Me._restoreLoc IsNot Nothing AndAlso Not Me.HasExited() Then
            Debug.Print($"restoresingle {behind}")
            If behind = 0 Then
                SetWindowPos(Me.MainWindowHandle, If(Me._wasTopmost, SWP_HWND.TOPMOST, FrmMain.ScalaHandle),
                             Me._restoreLoc?.X, Me._restoreLoc?.Y, -1, -1,
                             SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate Or SetWindowPosFlags.DoNotChangeOwnerZOrder)
            Else
                _restoreDic.TryRemove(Me.Id, Nothing)
                SetWindowPos(Me.MainWindowHandle, behind,
                             Me._restoreLoc?.X, Me._restoreLoc?.Y, -1, -1,
                             SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate Or SetWindowPosFlags.DoNotChangeOwnerZOrder)
            End If
        End If
    End Sub

    Public Sub SavePos(Optional pt As Point? = Nothing, Optional overwrite As Boolean = True)
        If pt Is Nothing Then
            Dim rc As Rectangle
            GetWindowRect(Me.MainWindowHandle, rc)
            pt = rc.Location
        End If
        If overwrite OrElse Not _restoreDic.ContainsKey(_proc.Id) Then Me._restoreLoc = pt
        If Not _restoreDic.ContainsKey(_proc.Id) Then _restoreDic.TryAdd(_proc.Id, Me)
        Me._wasTopmost = Me.TopMost()
    End Sub

    Private _CO? As Point
    Public ReadOnly Property ClientOffset() As Point
        Get
            If _CO IsNot Nothing AndAlso _CO <> New Point Then
                Return _CO
            Else
                If _proc Is Nothing Then
                    Return New Point
                End If
                Try
                    If Me.HasExited Then
                        Return New Point
                    End If
                    Dim ptt As New Point
                    ClientToScreen(Me.MainWindowHandle, ptt)
                    Dim rcW As New RECT
                    GetWindowRect(Me.MainWindowHandle, rcW)
                    Dim extrapixel As Integer = 0
                    Select Case Me.WindowsScaling
                        Case 125
                            extrapixel = 1
                        Case 175
                            extrapixel = 1
                    End Select
                    _CO = New Point(ptt.X - rcW.left, ptt.Y - rcW.top + extrapixel)
                    Debug.Print($"co {_CO}")
                    Return _CO
                Catch ex As Exception
                    Debug.Print($"ex on CO {ex.Message}")
                    Return New Point
                End Try
            End If
        End Get
    End Property

    Private _rcSource? As Rectangle
    Public Function rcSource(TargetSZ As Size, mode As Integer) As Rectangle

        'Dim mode = My.Settings.ScalingMode
        'Debug.Print($"rcSource target {TargetSZ}")
        'If mode = 0 Then
        '    Dim compsz As Size = TargetSZ
        '    If (compsz.Width / ClientRect.Width >= 2) AndAlso
        '       (compsz.Height / ClientRect.Height >= 2) Then
        '        mode = 2
        '    Else
        '        mode = 1
        '    End If
        'End If
        If mode = 1 Then 'blurred
            Return ClientRect 'todo handle non 100% scaling
        ElseIf mode = 2 Then 'pixel note: does not support non 100% windows scaling
            If _rcSource IsNot Nothing AndAlso _rcSource <> New Rectangle Then
                Return _rcSource
            Else
                _rcSource = New Rectangle(ClientOffset.X, ClientOffset.Y, ClientRect.Width + ClientOffset.X, ClientRect.Height + ClientOffset.Y)
                Return _rcSource
            End If
        End If

    End Function

    Public Function WindowsScaling() As Integer
        Const DWMWA_EXTENDED_FRAME_BOUNDS As Integer = 9
        Dim rcFrame As RECT
        DwmGetWindowAttribute(Me.MainWindowHandle, DWMWA_EXTENDED_FRAME_BOUNDS, rcFrame, System.Runtime.InteropServices.Marshal.SizeOf(rcFrame))
        Dim rcWind As RECT
        GetWindowRect(Me.MainWindowHandle, rcWind)
        Return Int((rcFrame.right - rcFrame.left) / (rcWind.right - rcWind.left) * 100 / 25) * 25 + 25
    End Function

    Public Function CenterBehind(centerOn As Control, Optional extraSWPFlags As UInteger = 0, Optional force As Boolean = False, Optional fixThumb As Boolean = False) As Boolean
        If Not force AndAlso centerOn.RectangleToScreen(centerOn.Bounds).Contains(Form.MousePosition) Then Return False
        Dim pt As Point = centerOn.PointToScreen(New Point(centerOn.Width / 2, centerOn.Height / 2))
        Return CenterWindowPos(centerOn.FindForm.Handle, pt.X, pt.Y, extraSWPFlags, fixThumb)
    End Function
    Public Function CenterWindowPos(hWndInsertAfter As IntPtr, x As Integer, y As Integer, Optional extraSWPFlags As UInteger = 0, Optional fixThumb As Boolean = False) As Boolean
        Try
            y = y - ClientRect.Height / 2 - ClientOffset.Y - My.Settings.offset.Y
            If fixThumb Then
                Dim rc As RECT
                GetWindowRect(FrmMain.ScalaHandle, rc)
                If y <= rc.top Then y = rc.top + 1
            End If
            Return SetWindowPos(Me.MainWindowHandle, hWndInsertAfter,
                                x - ClientRect.Width / 2 - ClientOffset.X - My.Settings.offset.X,
                                y,
                                -1, -1,
                                SetWindowPosFlags.IgnoreResize Or extraSWPFlags)
        Catch
            Debug.Print("CenterWindowPos exception")
            Return False
        End Try
    End Function


    Public Shared Narrowing Operator CType(ByVal d As AstoniaProcess) As Process
        Return d?._proc
    End Operator
    Public Shared Widening Operator CType(ByVal b As Process) As AstoniaProcess
        Return GetFromCache(b)
    End Operator

    Public ReadOnly Property Id As Integer
        Get
            Return If(_proc?.Id, 0)
        End Get
    End Property

    Public Sub Activate()
        If _proc Is Nothing Then Exit Sub
        Try
            AllowSetForegroundWindow(FrmMain.scalaPID)
            AppActivate(_proc.Id)
        Catch ex As Exception
            Debug.Print("activate exception")
        End Try
    End Sub

    Private _mwhCache As IntPtr = IntPtr.Zero
    Public ReadOnly Property MainWindowHandle() As IntPtr
        Get
            If _mwhCache <> IntPtr.Zero Then Return _mwhCache
            Try
                _mwhCache = If(_proc?.MainWindowHandle, IntPtr.Zero)
                Return _mwhCache
            Catch
                Debug.Print("MainWindowHandle exeption")
                Return IntPtr.Zero
            End Try
        End Get
    End Property

    'Private Shared ReadOnly nameCache As Concurrent.ConcurrentDictionary(Of Integer, String) = New Concurrent.ConcurrentDictionary(Of Integer, String)
    'Public ReadOnly Property Name As String
    '    Get
    '        If _proc Is Nothing Then Return "Someone"
    '        Try
    '            _proc?.Refresh()
    '            If _proc?.MainWindowTitle = "" Then
    '                Return nameCache.GetValueOrDefault(_proc.Id, "Someone")
    '            End If
    '            Dim nam = Strings.Left(_proc?.MainWindowTitle, _proc?.MainWindowTitle.IndexOf(" - "))
    '            Return nameCache.AddOrUpdate(_proc.Id, nam, Function() nam)
    '        Catch
    '            Return "Someone"
    '        End Try
    '    End Get
    'End Property

    Private Shared ReadOnly memCache As New System.Runtime.Caching.MemoryCache("nameCache")
    Private Shared ReadOnly cacheItemPolicy As New System.Runtime.Caching.CacheItemPolicy With {
                    .SlidingExpiration = TimeSpan.FromMinutes(1)} ' Cache for 1 minute with sliding expiration
    Public ReadOnly Property Name As String
        Get
            If _proc Is Nothing Then Return "Someone"
            Try
                _proc.Refresh()
                If _proc.MainWindowTitle = "" Then
                    Dim nm As String = TryCast(memCache.Get(_proc.Id), String)
                    If Not String.IsNullOrEmpty(nm) Then
                        Debug.Print($"name fail {nm} ""{Me.WindowClass}""")
                        Return nm
                    End If
                    Return "Someone"
                End If
                Dim nam As String = Strings.Left(_proc.MainWindowTitle, _proc.MainWindowTitle.IndexOf(" - "))

                memCache.Set(_proc.Id, nam, cacheItemPolicy)
                Return nam
            Catch
                Debug.Print("Name exception")
                Return String.Empty
            End Try
        End Get
    End Property

    Public Property TopMost() As Boolean
        Get

            Try
                Return (GetWindowLong(Me.MainWindowHandle, GWL_EXSTYLE) And WindowStylesEx.WS_EX_TOPMOST) = WindowStylesEx.WS_EX_TOPMOST
            Catch
                Return False
            End Try
        End Get
        Set(value As Boolean)
            Try
                SetWindowPos(Me.MainWindowHandle, If(value, SWP_HWND.TOPMOST, SWP_HWND.NOTOPMOST),
                                                  -1, -1, -1, -1,
                                                  SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate)
            Catch
            End Try
        End Set
    End Property

    Public Function IsRunning() As Boolean
        'todo: replace with limited access check
        Try
            Return _proc IsNot Nothing AndAlso Not _proc.HasExited
        Catch e As Exception
            FrmMain.tmrActive.Enabled = False
            FrmMain.tmrOverview.Enabled = False
            FrmMain.tmrTick.Enabled = False
            FrmMain.SaveLocation()
            My.Settings.Save()
            FrmMain.RestartSelf(True)
            End 'program
        End Try
    End Function

    Public Function IsActive() As Boolean
        Dim hWnd As IntPtr = GetForegroundWindow()
        Dim ProcessID As UInteger = 0

        GetWindowThreadProcessId(hWnd, ProcessID)

        Return _proc?.Id = ProcessID
    End Function

    Public Function IsBelow(hwnd As IntPtr) As Boolean
        Dim curr As IntPtr = Me.MainWindowHandle
        Do While True
            curr = GetWindow(curr, 3)
            If curr = IntPtr.Zero Then Return False
            If curr = hwnd Then Return True
        Loop
        Return False
    End Function

    Public Function IsAbove(hwnd As IntPtr) As Boolean
        Dim curr As IntPtr = Me.MainWindowHandle
        Do While True
            curr = GetWindow(curr, 2)
            If curr = IntPtr.Zero Then Return False
            If curr = hwnd Then Return True
        Loop
        Return False
    End Function

    Public Function MainWindowTitle() As String
        Try
            _proc?.Refresh()
            Return _proc?.MainWindowTitle
        Catch
            Debug.Print("MainWindowTitle exception")
            Return ""
        End Try
    End Function

    Private Shared classCache As String = String.Empty
    Private Shared classCacheSet As New HashSet(Of String)
    Private Shared ReadOnly pipe As Char() = {"|"c}
    Private Shared ReadOnly sysMenClass As String() = {"#32768", "SysShadow"}
    ''' <summary>
    ''' Returns True if WindowClass is in My.settings.className 
    ''' </summary>
    ''' <returns></returns>
    ''' 
    Public Function IsAstoniaClass() As Boolean
        Dim classes = My.Settings.className
        If classCache <> classes Then
            classCacheSet.Clear()
            classCacheSet = New HashSet(Of String)(classes.Split(pipe, StringSplitOptions.RemoveEmptyEntries) _
                                                          .Select(Function(wc) Strings.Trim(wc)).Concat(sysMenClass))
            classCache = classes
        End If
        Return classCacheSet.Contains(Me.WindowClass)
    End Function

    Private _wc As String
    Public ReadOnly Property WindowClass() As String
        Get
            If Not String.IsNullOrEmpty(_wc) Then Return _wc
            If Me.MainWindowHandle <> IntPtr.Zero Then _wc = GetWindowClass(Me.MainWindowHandle)
            Return _wc
        End Get
    End Property

    'Public Overrides Function ToString() As String
    '    If _proc Is Nothing Then Return "Someone"
    '    Return Me.Name()
    'End Function

    Shared ReadOnly nameIconCache As New Concurrent.ConcurrentDictionary(Of Integer, Tuple(Of Icon, String)) 'PID, icon, name
    Shared ReadOnly pathIcnCache As New Concurrent.ConcurrentDictionary(Of String, Icon) 'path, icon
    Public Function GetIcon(Optional invalidateCache As Boolean = False) As Icon
        If invalidateCache Then
            nameIconCache.Clear()
            pathIcnCache.Clear()
        End If
        If _proc Is Nothing Then Return Nothing
        Try
            Dim ID As Integer = _proc.Id

            Dim IcoNam As New Tuple(Of Icon, String)(Nothing, Nothing)

            If ID > 0 AndAlso nameIconCache.TryGetValue(ID, IcoNam) AndAlso IcoNam.Item2 = Me.Name Then
                Return IcoNam.Item1
            Else
                nameIconCache.TryRemove(ID, Nothing)
            End If

            Dim path As String = Me.Path()

            If Not String.IsNullOrEmpty(path) Then
                Dim ico As Icon = Nothing
                If pathIcnCache.TryGetValue(path, ico) Then
                    nameIconCache.TryAdd(ID, New Tuple(Of Icon, String)(ico, Me.Name))
                    Return ico
                Else
                    ico = Icon.ExtractAssociatedIcon(path)
                    If ico IsNot Nothing Then
                        nameIconCache.TryAdd(ID, New Tuple(Of Icon, String)(ico, Me.Name))
                        pathIcnCache.TryAdd(path, ico)
                    End If
                    Return ico
                End If
            End If

        Catch ex As Exception
            Debug.Print($"Error retrieving icon: {ex.Message}")
        End Try

        Return Nothing
    End Function

    Dim pathCache As String = Nothing
    Private Function Path() As String
        If String.IsNullOrEmpty(pathCache) Then pathCache = _proc?.Path()
        Return pathCache
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim proc2 As AstoniaProcess = TryCast(obj, AstoniaProcess)
        'Debug.Print($"obj {proc2?._proc?.Id} eqals _proc {_proc?.Id}")
        Return proc2?._proc IsNot Nothing AndAlso Me._proc IsNot Nothing AndAlso proc2._proc.Id = Me._proc.Id AndAlso proc2.Name = Me.Name
    End Function
    'Public Shared Operator =(left As AstoniaProcess, right As AstoniaProcess) As Boolean
    '    Debug.Print("AP ==")
    '    Return left.Equals(right)
    'End Operator
    'Public Shared Operator <>(left As AstoniaProcess, right As AstoniaProcess) As Boolean
    '    Debug.Print("AP <>")
    '    Return Not left.Equals(right)
    'End Operator
    Private Shared exeCache As IEnumerable(Of String) = My.Settings.exe.Split(pipe, StringSplitOptions.RemoveEmptyEntries).Select(Function(s) s.Trim).ToList
    Private Shared exeSettingCache As String = My.Settings.exe

    Public Shared Function EnumSomeone() As IEnumerable(Of AstoniaProcess)
        If exeSettingCache <> My.Settings.exe Then
            exeCache = My.Settings.exe.Split(pipe, StringSplitOptions.RemoveEmptyEntries).Select(Function(s) s.Trim).ToList
        End If
        Return exeCache.SelectMany(Function(s) Process.GetProcessesByName(s).Select(Function(p) New AstoniaProcess(p))) _
            .Where(Function(ap) ap.Name = "Someone" AndAlso ap.IsAstoniaClass())
    End Function

    Public Shared Function EnumAll() As IEnumerable(Of AstoniaProcess)
        If exeSettingCache <> My.Settings.exe Then
            exeCache = My.Settings.exe.Split(pipe, StringSplitOptions.RemoveEmptyEntries).Select(Function(s) s.Trim).ToList
        End If
        Return exeCache.SelectMany(Function(s) Process.GetProcessesByName(s).Select(Function(p) New AstoniaProcess(p))) _
            .Where(Function(ap) ap.IsAstoniaClass())
    End Function

    Private Shared Function ListProcesses(blacklist As IEnumerable(Of String), useCache As Boolean) As List(Of AstoniaProcess)
        'todo move updating cache to frmSettings
        If exeSettingCache <> My.Settings.exe Then
            exeCache = My.Settings.exe.Split(pipe, StringSplitOptions.RemoveEmptyEntries).Select(Function(s) s.Trim).ToList
        End If

        Return exeCache.SelectMany(Function(s) Process.GetProcessesByName(s).Select(Function(p) If(useCache, GetFromCache(p), New AstoniaProcess(p)))) _
                    .Where(Function(ap)
                               Dim nam = ap.Name
                               Return Not String.IsNullOrEmpty(nam) AndAlso Not blacklist.Contains(nam) AndAlso
                                     (Not My.Settings.Whitelist OrElse FrmMain.topSortList.Concat(FrmMain.botSortList).Contains(nam)) AndAlso
                                      ap.IsAstoniaClass()
                           End Function).ToList
    End Function
    Public Shared Function Enumerate(Optional useCache As Boolean = False) As IEnumerable(Of AstoniaProcess)
        Return AstoniaProcess.Enumerate({}, useCache)
    End Function
    Private Shared _ProcCache As New List(Of AstoniaProcess)
    Private Shared _CacheCounter As Integer = 0
    Private Shared Function GetFromCache(p As Process) As AstoniaProcess

        Return If(_ProcCache.Find(Function(ap)
                                      If ap.HasExited Then Return False
                                      Return ap.Id = p.Id
                                  End Function), New AstoniaProcess(p))
    End Function

    Public Shared Function Enumerate(blacklist As IEnumerable(Of String), Optional useCache As Boolean = False, Optional resetCacheFirst As Boolean = False) As IEnumerable(Of AstoniaProcess)
        If resetCacheFirst Then
            _CacheCounter = 0
            _ProcCache.Clear()
        End If
        If useCache Then
            If _CacheCounter = 0 Then
                _ProcCache = ListProcesses(blacklist, True)
            End If
            _CacheCounter += 1
            If _CacheCounter > 5 Then
                _CacheCounter = 0
                _ProcCache.RemoveAll(Function(ap) ap.HasExited)
            End If
            Return _ProcCache
        End If
        Return ListProcesses(blacklist, False)
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function PrintWindow(ByVal hwnd As IntPtr, ByVal hDC As IntPtr, ByVal nFlags As UInteger) As Boolean : End Function
    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function GetClientRect(ByVal hWnd As IntPtr, ByRef lpRect As Rectangle) As Boolean : End Function
    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function GetClientRect(ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Boolean : End Function

    Public Function GetClientBitmap() As Bitmap
        If _proc Is Nothing Then Return Nothing
        Try
            Dim rcc As Rectangle
            If Not GetClientRect(Me.MainWindowHandle, rcc) Then Return Nothing 'GetClientRect fails if astonia is running fullscreen and is tabbed out

            If rcc.Width = 0 OrElse rcc.Height = 0 Then Return Nothing

            Dim bmp As New Bitmap(rcc.Width, rcc.Height)

            Using gBM As Graphics = Graphics.FromImage(bmp)
                Dim hdcBm As IntPtr
                Try
                    hdcBm = gBM.GetHdc
                Catch ex As Exception
                    Debug.Print("GetHdc error")
                    Return Nothing
                End Try
                PrintWindow(Me.MainWindowHandle, hdcBm, 1)
                gBM.ReleaseHdc()
            End Using
            Return bmp
        Catch
            Return Nothing
        End Try
    End Function
    Private Shared ReadOnly validColors As Integer() = {&HFFFF0000, &HFFFF0400, &HFFFF7B29, &HFFFF7D29, &HFF297BFF, &HFF297DFF, &HFF000000, &HFF000400, &HFFFFFFFF} 'red, orange, lightblue, black, white (troy,base)
    Private _isSDL? As Boolean = Nothing
    Public ReadOnly Property isSDL() As Boolean
        Get
            If _isSDL Is Nothing Then
                va.Read(0, shm)
                If shm.pID = Me.Id Then
                    _isSDL = True
                Else
                    _isSDL = False
                End If
                'Debug.Print($"isSDL {Me.Name} {_isSDL} {shm.pID} {Me.Id}")
            End If
            Return _isSDL
        End Get
    End Property
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto, Pack:=0)>
    Structure MoacSharedMem
        Dim pID As UInt32
        Dim hp, shield, [end], mana As Byte
        Dim base As UInt64
        Dim key, isprite, offX, offY As Integer
        Dim flags, fsprite As Integer
        Dim swapped As Byte
    End Structure
    Private shm As MoacSharedMem
    Private _map As MemoryMappedFile = Nothing
    Private ReadOnly Property map As MemoryMappedFile
        Get
            If _map Is Nothing Then
                _map = MemoryMappedFile.CreateOrOpen($"MOAC{Me.Id}", Marshal.SizeOf(shm))
            End If
            Return _map
        End Get
    End Property
    Private _va As MemoryMappedViewAccessor = Nothing
    Private ReadOnly Property va As MemoryMappedViewAccessor
        Get
            If _va Is Nothing Then
                _va = map.CreateViewAccessor(0, Marshal.SizeOf(shm), MemoryMappedFileAccess.Read)
            End If
            Return _va
        End Get
    End Property

    Public Function hasBorder() As Boolean
        Return GetWindowLong(Me.MainWindowHandle, GWL_STYLE) And WindowStyles.WS_BORDER
    End Function

    Dim br As New SolidBrush(Color.FromArgb(&HFFFF0400))
    Dim bo As New SolidBrush(Color.FromArgb(&HFFFF7D29))
    Dim by As New SolidBrush(Color.FromArgb(186, 186, 30))
    Dim bl As New SolidBrush(Color.FromArgb(217, 217, 30))
    Dim bb As New SolidBrush(Color.FromArgb(&HFF297DFF))

    Public Function GetHealthbar(Optional width As Integer = 75, Optional height As Integer = 15) As Bitmap
        Dim bmp As New Bitmap(width, height)

        'struct sharedmem {
        '    unsigned int pid; 0
        '    Char hp, shield,end, mana; 4 5 6 7
        If Me.isSDL Then
            va.Read(0, shm)

            'Dim bars(4) As Byte
            'va.ReadArray(Of Byte)(4, bars, 0, 4)
            Using g As Graphics = Graphics.FromImage(bmp)

                g.Clear(Color.Black)
                If shm.mana = 255 Then
                    g.FillRectangle(br, New Rectangle(0, 0, shm.hp / 100 * width, height / 5 * 2))
                    g.FillRectangle(bo, New Rectangle(0, height / 5 * 2, shm.shield / 100 * width, height / 5 * 2))
                    g.FillRectangle(If(My.Settings.DarkMode, by, bl), New Rectangle(0, height / 5 * 4, shm.end / 100 * width, height / 5))
                Else
                    Dim itemheight As Integer = height / 15 * 5
                    If My.Settings.ShowEnd Then
                        itemheight = height / 15 * 4
                    End If
                    g.FillRectangle(br, New Rectangle(0, 0, shm.hp / 100 * width, itemheight))
                    g.FillRectangle(bo, New Rectangle(0, itemheight, shm.shield / 100 * width, itemheight))
                    g.FillRectangle(bb, New Rectangle(0, itemheight * 2, shm.mana / 100 * width, itemheight))
                    If My.Settings.ShowEnd Then
                        g.FillRectangle(If(My.Settings.DarkMode, by, bl), New Rectangle(0, itemheight * 3, shm.end / 100 * width, height - (itemheight * 3)))
                    End If
                End If
            End Using
            Return bmp
        End If

        Using g As Graphics = Graphics.FromImage(bmp), grab As Bitmap = GetClientBitmap()

            If grab Is Nothing Then Return Nothing
            If grab.Width = 0 OrElse grab.Height = 0 Then Return Nothing

            Dim rows = 3

            'Dim barX As Integer = 388
            Dim barX As Integer = grab.Width / 2 - 12.Map(0, 800, 0, grab.Width)
            'Dim barY As Integer = 205 + RebornResolutionOffset(grab.Height)
            Dim barY As Integer = grab.Height / 2 - 95.Map(0, 600, 0, grab.Height)

            Dim BadColorCount As Integer
            Dim blackCount As Integer = 0

            For dy As Integer = 0 To grab.Height / 300 Step grab.Height / 600
                BadColorCount = 0
                blackCount = 0
                For dx As Integer = 0 To grab.Width / 32 - 1
                    Dim currentCol As Integer = grab.GetPixel(barX + dx, barY + dy).ToArgb
                    'Debug.Print($"current{dy}/{dx}:{currentCol:X8}")
                    If {&HFF000000, &HFF000400}.Contains(currentCol) Then
                        blackCount += 1
                    End If

                    If Not validColors.Contains(currentCol) Then
                        'Debug.Print($"badcolor row{dy} &H{grab.GetPixel(barX + dx, 205 + dy).ToArgb.ToString("X8")}")
                        BadColorCount += 1
                    End If
                    If dy = 0 Then
                        If BadColorCount > 1 OrElse blackCount = 25 Then
                            'Debug.Print("Pane open?")
                            blackCount = 0
                            barX += 110.Map(0, 800, 0, grab.Width)
                            Exit For
                        End If
                    End If
                    If BadColorCount >= 5 Then Exit For
                Next dx
                If BadColorCount >= 5 OrElse blackCount = 25 Then
                    rows -= 1
                    Continue For
                End If
            Next dy

            g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
            g.DrawImage(grab,
                        New Rectangle(0, 0, bmp.Width, bmp.Height),
                        New Rectangle(barX, barY, grab.Width / 32, rows * grab.Height / 600),
                        GraphicsUnit.Pixel)
        End Using 'g, grab
        Return bmp
    End Function

    Friend Function GetCurrentDirectory() As String
        Return _proc?.GetCurrentDirectory
    End Function

    Private alreadylaunched As Boolean = False
    Private disposedValue As Boolean

    Friend Async Function ReOpenAsWindowed() As Task

        If alreadylaunched Then
            Return
        End If

        Debug.Print($"runasWindowed: {Me.Name} {Me.Id}")

        Dim shortcutlink As String = FileIO.SpecialDirectories.Temp & "\ScalA\tmp.lnk"

        Dim mos As Management.ManagementObject = New Management.ManagementObjectSearcher($“Select * from Win32_Process WHERE ProcessID={_proc.Id}").Get()(0)

        Dim arguments As String = mos("commandline")

        Debug.Print($"arguments:""{arguments}""")
        Debug.Print($"exePath:""{mos("ExecutablePath")}""")

        If arguments = "" Then
            If CustomMessageBox.Show(FrmMain, "Access denied!" & vbCrLf &
                           "Elevate ScalA to Administrator?",
                               "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) _
               = DialogResult.Cancel Then Return
            FrmMain.RestartSelf(True)
            End 'program
            Return
        End If
        Debug.Print("cmdline:" & arguments)
        If arguments.StartsWith("""") Then
            'arguments = arguments.Substring(1) 'skipped with startindex
            arguments = arguments.Substring(arguments.IndexOf("""", 1) + 1)
        Else
            For Each exe As String In My.Settings.exe.Split(pipe, StringSplitOptions.RemoveEmptyEntries)
                If arguments.ToLower.StartsWith(exe.Trim) Then
                    arguments = arguments.Substring(exe.Trim.Length + 4) '+ ".exe".Length)
                End If
            Next
        End If

        Dim newargs As New List(Of String)

        For Each arg As String In arguments.Split("-".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            Dim item As String = arg.Trim
            If item.StartsWith("w") OrElse item.StartsWith("h") Then
                Continue For
            End If
            If item.StartsWith("o") AndAlso item.Length > 1 Then
                Dim value As Integer = Val(arg.Substring(1))

                If (value And 8) = 8 Then value -= 8 'remove compact bot flag
                If (value And 256) = 256 Then value -= 256 'remove fullscreen flag

                item = $"o{value}"
            End If
            newargs.Add(item)
        Next

        newargs.Add("w800")
        newargs.Add("h600")

        arguments = Strings.Join(newargs.ToArray, " -")
        Debug.Print($"args {arguments}")

        Dim exepath As String = ""
        Try
            exepath = mos("ExecutablePath")
        Catch

        End Try
        'Dim workdir As String = exepath.Substring(0, exepath.LastIndexOf("\")) 'todo: replace with function found at https://stackoverflow.com/a/23842609/7433250
        Dim workdir = _proc.GetCurrentDirectory()

        Debug.Print($"wd {workdir}")

        If workdir.EndsWith("bin") Then
            workdir = workdir.Substring(0, workdir.LastIndexOf("\"))
        End If


        Dim oLink As Object
        Try

            oLink = CreateObject("WScript.Shell").CreateShortcut(shortcutlink)

            oLink.TargetPath = exepath
            oLink.Arguments = arguments.Trim()
            oLink.WorkingDirectory = workdir
            oLink.WindowStyle = 1
            oLink.Save()
        Catch ex As Exception
            Return
        End Try

        Dim targetName As String = Me.Name

        'SendMessage(Me.MainWindowHandle, &H100, Keys.F12, IntPtr.Zero)
        Me.CloseOrKill()

        Dim pp As Process

        Dim bat As String = "\noAdmin.bat"
        Dim tmpDir As String = FileIO.SpecialDirectories.Temp & "\ScalA"

        If Not FileIO.FileSystem.DirectoryExists(tmpDir) Then FileIO.FileSystem.CreateDirectory(tmpDir)
        If Not FileIO.FileSystem.FileExists(tmpDir & bat) Then FileIO.FileSystem.WriteAllText(tmpDir & bat, My.Resources.AsInvoker, False)

        pp = New Process With {.StartInfo = New ProcessStartInfo With {.FileName = tmpDir & bat,
                                                                       .Arguments = """" & shortcutlink & """",
                                                                       .WindowStyle = ProcessWindowStyle.Hidden,
                                                                       .CreateNoWindow = True}}
        Try
            alreadylaunched = True
            pp.Start()
        Catch
        Finally
            pp.Dispose()
        End Try



        FrmMain.Cursor = Cursors.WaitCursor
        Dim count As Integer = 0

        While True
            count += 1
            Await Task.Delay(50)
            Dim targetPPs As AstoniaProcess() = AstoniaProcess.Enumerate(FrmMain.blackList).Where(Function(ap) ap.Name = targetName).ToArray()
            If targetPPs.Length > 0 AndAlso targetPPs(0) IsNot Nothing AndAlso targetPPs(0).Id <> 0 Then
                FrmMain.PopDropDown(FrmMain.cboAlt)
                FrmMain.cboAlt.SelectedItem = targetPPs(0)
                Exit While
            End If
            If count >= 100 Then
                CustomMessageBox.Show(FrmMain, "Windowing failed")
                Exit While
            End If
        End While
        FrmMain.Cursor = Cursors.Default

        If System.IO.File.Exists(shortcutlink) Then
            Debug.Print("Deleting shortcut")
            System.IO.File.Delete(shortcutlink)
        End If

        alreadylaunched = False
    End Function

    Private Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects)
                _va?.Dispose()
                _map?.Dispose()
                _proc?.Dispose()

            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
            ' TODO: set large fields to null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
    ' Protected Overrides Sub Finalize()
    '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
    Dim Elevated As Boolean = False
    Friend Function HasExited() As Boolean
        If _proc Is Nothing Then Return True
        If Elevated Then Return _proc.HasExitedSafe
        Try
            Return _proc.HasExited
        Catch ex As Exception
            Debug.Print("HasExited Exception")
            Elevated = True
            Return _proc.HasExitedSafe
        End Try
    End Function

    Friend Function IsElevated() As Boolean
        If _proc Is Nothing Then Return False
        Try
            Dim dummy = _proc.HasExited
        Catch ex As Exception
            Elevated = True
        End Try
        Return Elevated
    End Function
End Class

NotInheritable Class AstoniaProcessSorter
    Implements IComparer(Of String)

    Private ReadOnly topOrder As List(Of String)
    Private ReadOnly botOrder As List(Of String)

    Public Sub New(topOrder As List(Of String), botOrder As List(Of String))
        Me.topOrder = topOrder
        Me.botOrder = botOrder
    End Sub

    Public Function Compare(ap1 As String, ap2 As String) As Integer Implements IComparer(Of String).Compare
        'equal = 0, ap1 > ap2 = 1, ap1 < ap2 = -1

        Dim top1 As Boolean = topOrder.Contains(ap1)
        Dim top2 As Boolean = topOrder.Contains(ap2)

        Dim bot1 As Boolean = botOrder.Contains(ap1)
        Dim bot2 As Boolean = botOrder.Contains(ap2)

        'Debug.Print($"comp:{ap1} {ap2}")
        'Debug.Print($"top: {top1} {top2}")
        'Debug.Print($"bot: {bot1} {bot2}")

        If top1 AndAlso bot2 Then Return -1
        If bot1 AndAlso top2 Then Return 1

        If bot1 AndAlso bot2 Then Return botOrder.IndexOf(ap1) - botOrder.IndexOf(ap2)
        If top1 AndAlso top2 Then Return topOrder.IndexOf(ap1) - topOrder.IndexOf(ap2)

        If bot1 Then Return 1
        If bot2 Then Return -1

        If top1 Then Return -1
        If top2 Then Return 1

        Return Comparer(Of String).Default.Compare(ap1, ap2)

    End Function
End Class
NotInheritable Class AstoniaProcessEqualityComparer
    Implements IEqualityComparer(Of AstoniaProcess)

    Public Overloads Function Equals(ap1 As AstoniaProcess, ap2 As AstoniaProcess) As Boolean Implements IEqualityComparer(Of AstoniaProcess).Equals
        If ap1 Is Nothing AndAlso ap2 Is Nothing Then
            Return True
        ElseIf ap1 Is Nothing Or ap2 Is Nothing Then
            Return False
        ElseIf ap1.Id = ap2.Id Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Overloads Function GetHashCode(ap As AstoniaProcess) As Integer Implements IEqualityComparer(Of AstoniaProcess).GetHashCode
        Return ap.Id.GetHashCode
    End Function

End Class


Module ProcessExtensions

    <System.Runtime.CompilerServices.Extension()>
    Public Function HasExitedSafe(ByVal this As Process) As Boolean
        Dim exitCode As Integer = 0
        Dim processHandle As IntPtr = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, False, this.Id)
        Try
            If processHandle <> IntPtr.Zero AndAlso GetExitCodeProcess(processHandle, exitCode) Then Return exitCode <> 259
        Catch
            CustomMessageBox.Show(FrmMain, "Exception on HasExitedSafe")
        Finally
            CloseHandle(processHandle)
        End Try
        Return True
    End Function
    ''' <summary>
    ''' Returns the executable path of a process.
    ''' </summary>
    ''' <param name="this"></param>
    ''' <returns></returns>
    <System.Runtime.CompilerServices.Extension()>
    Public Function Path(ByVal this As Process) As String
        Dim processPath As String = ""

        Dim processHandle As IntPtr = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, False, this?.Id)
        Try
            If Not processHandle = IntPtr.Zero Then
                Dim buffer As New System.Text.StringBuilder(1024)
                If QueryFullProcessImageName(processHandle, 0, buffer, buffer.Capacity) Then
                    processPath = buffer.ToString()
                End If
            End If
        Finally
            CloseHandle(processHandle)
        End Try

        Return processPath
    End Function
    Private classCache As String = String.Empty
    Private classCacheSet As New HashSet(Of String)
    Private ReadOnly pipe As Char() = {"|"c}
    ''' <summary>
    ''' Checks if a processes classname is in pipe separated string
    ''' </summary>
    ''' <param name="pp"></param>
    ''' <param name="classes"></param>
    ''' <returns></returns>
    <System.Runtime.CompilerServices.Extension()>
    Public Function HasClassNameIn(pp As Process, classes As String) As Boolean
        If classCache <> classes Then
            classCacheSet.Clear()
            classCacheSet = New HashSet(Of String)(classes.Split(pipe, StringSplitOptions.RemoveEmptyEntries) _
                                                          .Select(Function(wc) Strings.Trim(wc)))
            classCache = classes
        End If
        Return classCacheSet.Contains(GetWindowClass(pp.MainWindowHandle))
    End Function
    Private exeCache As String = String.Empty
    Private exeCacheSet As New HashSet(Of String)
    <System.Runtime.CompilerServices.Extension()>
    Public Function IsAstonia(pp As Process) As Boolean
        If classCache <> My.Settings.className Then
            classCacheSet.Clear()
            classCacheSet = New HashSet(Of String)(My.Settings.className.Split(pipe, StringSplitOptions.RemoveEmptyEntries) _
                                                          .Select(Function(wc) Strings.Trim(wc)))
            classCache = My.Settings.className
        End If
        If exeCache <> My.Settings.exe Then
            exeCacheSet.Clear()
            exeCacheSet = New HashSet(Of String)(My.Settings.exe.Split(pipe, StringSplitOptions.RemoveEmptyEntries) _
                                                          .Select(Function(x) Strings.Trim(x)))
            exeCache = My.Settings.exe
        End If
        Return classCacheSet.Contains(GetWindowClass(pp.MainWindowHandle)) AndAlso exeCache.Contains(pp.ProcessName)
    End Function
End Module
