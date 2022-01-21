﻿Public Class AstoniaProcess

    Private ReadOnly _proc As Process

    Public Sub New(Optional process As Process = Nothing)
        _proc = process
    End Sub

    Private _rcc As Rectangle
    Public ReadOnly Property ClientRect() As Rectangle
        Get
            If _rcc <> New Rectangle Then
                Return _rcc
            Else
                Debug.Print("called get ClientRect")
                Dim rcc As New Rectangle
                GetClientRect(_proc.MainWindowHandle, rcc)
                _rcc = rcc
                Return rcc
            End If
        End Get
    End Property


    Private _CO As Point
    Public ReadOnly Property ClientOffset() As Point
        Get
            If _CO <> New Point Then
                Return _CO
            Else
                Debug.Print("called get ClientOffset")
                Dim ptt As Point
                ClientToScreen(_proc?.MainWindowHandle, ptt)
                Dim rcW As Rectangle
                GetWindowRect(_proc?.MainWindowHandle, rcW)

                _CO = New Point(ptt.X - rcW.Left, ptt.Y - rcW.Top)
                Return _CO
            End If
        End Get
    End Property

    Public Function CenterWindowPos(hWndInsertAfter As IntPtr, x As Integer, y As Integer, Optional extraSWPFlags As UInteger = 0) As Boolean
        Try
            Return SetWindowPos(_proc?.MainWindowHandle, hWndInsertAfter,
                                x - ClientRect.Width / 2 - ClientOffset.X - My.Settings.offset.X,
                                y - ClientRect.Height / 2 - ClientOffset.Y - My.Settings.offset.Y,
                                -1, -1,
                                SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.ASyncWindowPosition Or extraSWPFlags)
        Catch
            Return False
        End Try
    End Function


    'Public Shared Narrowing Operator CType(ByVal d As AstoniaProcess) As Process
    '    Return d?._proc
    'End Operator
    'Public Shared Widening Operator CType(ByVal b As Process) As AstoniaProcess
    '    Return New AstoniaProcess(b)
    'End Operator
    Public ReadOnly Property Id As Integer
        Get
            Return If(_proc?.Id, 0)
        End Get
    End Property

    Public ReadOnly Property MainWindowHandle() As IntPtr
        Get
            Try
                Return If(_proc?.MainWindowHandle, IntPtr.Zero)
            Catch
                Return IntPtr.Zero
            End Try
        End Get
    End Property
    Public ReadOnly Property Name As String
        Get
            If _proc Is Nothing Then Return "Someone"
            Try
                _proc?.Refresh()
                Return Strings.Left(_proc?.MainWindowTitle, _proc?.MainWindowTitle.IndexOf(" - "))
            Catch
                Return String.Empty
            End Try
        End Get
    End Property

    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function GetWindowLong(ByVal hwnd As IntPtr, ByVal nIndex As Integer) As UInteger
    End Function
    Public ReadOnly Property IsTopMost() As Boolean
        Get
            Const GWL_EXSTYLE = -20
            Const WS_EX_TOPMOST = 8L
            Return (GetWindowLong(_proc?.MainWindowHandle, GWL_EXSTYLE) And WS_EX_TOPMOST) = WS_EX_TOPMOST
        End Get
    End Property

    Public Function IsRunning() As Boolean
        'todo: replace with limited access check
        Try
            Return _proc IsNot Nothing AndAlso Not _proc.HasExited
        Catch e As Exception
            FrmMain.tmrActive.Enabled = False
            FrmMain.tmrOverview.Enabled = False
            FrmMain.tmrTick.Enabled = False
            My.Settings.Save()
            FrmMain.ElevateSelf()
            End 'program
        End Try
    End Function

    Public Function IsActive() As Boolean
        Dim hWnd As IntPtr = GetForegroundWindow()
        Dim ProcessID As UInteger = 0

        GetWindowThreadProcessId(hWnd, ProcessID)

        Return _proc?.Id = ProcessID
    End Function


    Public Function MainWindowTitle() As String
        _proc?.Refresh()
        Return _proc?.MainWindowTitle
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll", CharSet:=System.Runtime.InteropServices.CharSet.Auto)>
    Private Shared Sub GetClassName(ByVal hWnd As System.IntPtr, ByVal lpClassName As System.Text.StringBuilder, ByVal nMaxCount As Integer)
    End Sub
    Private Shared Function GetWindowClass(ByVal hwnd As Long) As String
        Dim sClassName As New System.Text.StringBuilder("", 256)
        Call GetClassName(hwnd, sClassName, 256)
        Return sClassName.ToString
    End Function
    ''' <summary>
    ''' Returns True if WindowClass is in pipe seperated string of classes 
    ''' </summary>
    ''' <param name="classes">Pipe seperated string of classes</param>
    ''' <returns></returns>
    Public Function HasClassNameIn(classes As String) As Boolean
        Return classes.Split({"|"c}, StringSplitOptions.RemoveEmptyEntries) _
                      .Select(Function(wc) Strings.Trim(wc)) _
                      .Contains(GetWindowClass(_proc?.MainWindowHandle))
    End Function

    'Public Overrides Function ToString() As String
    '    If _proc Is Nothing Then Return "Someone"
    '    Return Me.Name()
    'End Function

    Shared ReadOnly exeIconCache As New Dictionary(Of Integer, Tuple(Of Icon, String)) 'PID, icon, name
    Public Function GetIcon(Optional invalidateCache As Boolean = False) As Icon
        If invalidateCache Then exeIconCache.Clear()
        Try
            Dim ID As Integer = _proc?.Id
            If exeIconCache.ContainsKey(ID) AndAlso (exeIconCache(ID).Item2 = Me.Name) Then
                Return exeIconCache(ID).Item1
            Else
                Dim path As String = _proc.Path()
                Debug.Print($"ExeIconCacheMiss {Me.Name} {ID} {path}")
                Dim ico = Icon.ExtractAssociatedIcon(path)
                exeIconCache(ID) = New Tuple(Of Icon, String)(ico, Me.Name)
                Return ico
            End If
        Catch
            Return Nothing
        End Try
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim proc2 As AstoniaProcess = TryCast(obj, AstoniaProcess)
        Debug.Print($"obj {proc2?._proc?.Id} eqals _proc {_proc?.Id}")
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

    Private Shared Function enumProcessesByNameArray(strings() As String) As IEnumerable(Of Process)
        Dim IEnum As IEnumerable(Of Process) = {}
        For Each exe As String In strings
            IEnum = IEnum.Concat(Process.GetProcessesByName(Trim(exe)))
        Next
        Return IEnum
    End Function
    Public Shared Function Enumerate(Optional listSomeone As Boolean = False) As IEnumerable(Of AstoniaProcess)
        Return enumProcessesByNameArray(My.Settings.exe.Split({"|"c}, StringSplitOptions.RemoveEmptyEntries)) _
            .Where(Function(p) listSomeone OrElse Not p.MainWindowTitle.StartsWith("Someone ")) _
            .OrderBy(Function(p) p.MainWindowTitle) _
            .Select(Function(p) New AstoniaProcess(p)) _
            .Where(Function(ap) ap.HasClassNameIn(classes:=My.Settings.className))
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function PrintWindow(ByVal hwnd As IntPtr, ByVal hDC As IntPtr, ByVal nFlags As UInteger) As Boolean : End Function
    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function GetClientRect(ByVal hWnd As IntPtr, ByRef lpRect As Rectangle) As Boolean : End Function

    Public Function getClientBitmap() As Bitmap
        If _proc Is Nothing Then Return Nothing

        Dim rcc As Rectangle
        If Not GetClientRect(_proc.MainWindowHandle, rcc) Then Return Nothing 'GetClientRect fails if astonia is running fullscreen and is tabbed out

        If rcc.Width = 0 OrElse rcc.Height = 0 Then Return Nothing

        Static Dim bmp As Bitmap = New Bitmap(rcc.Width, rcc.Height)

        Using gBM As Graphics = Graphics.FromImage(bmp)
            Dim hdcBm As IntPtr
            Try
                hdcBm = gBM.GetHdc
            Catch ex As Exception
                Debug.Print("GetHdc error")
                Return Nothing
            End Try
            PrintWindow(_proc.MainWindowHandle, hdcBm, 1)
            gBM.ReleaseHdc()
        End Using
        Return bmp
    End Function
    Private Shared ReadOnly validColors As Integer() = {&HFFFF0000, &HFFFF0400, &HFFFF7B29, &HFFFF7D29, &HFF297BFF, &HFF297DFF, &HFF000000, &HFF000400, &HFFFFFFFF} 'red, orange, lightblue, black, white (troy,base)

    Public Function getHealthbar(Optional width As Integer = 75, Optional height As Integer = 15) As Bitmap
        Static Dim bmp As Bitmap = New Bitmap(width, height)
        Using g As Graphics = Graphics.FromImage(bmp), grab As Bitmap = getClientBitmap()

            If grab Is Nothing Then Return Nothing

            Dim rows = 3

            Dim barX As Integer = 388

            Dim BadColorCount As Integer
            Dim blackCount As Integer = 0

            For dy As Integer = 0 To 2
                BadColorCount = 0
                blackCount = 0
                For dx As Integer = 0 To 24
                    Dim currentCol As Integer = grab.GetPixel(barX + dx, 205 + dy).ToArgb
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
                            Debug.Print("Pane open?")
                            blackCount = 0
                            barX += 110
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
                        New Rectangle(barX, 205, 25, rows),
                        GraphicsUnit.Pixel)
        End Using 'g, grab
        Return bmp
    End Function

End Class

Class AstoniaProcessEqualityComparer
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
    <System.Runtime.InteropServices.DllImport("kernel32.dll")>
    Private Function QueryFullProcessImageName(hprocess As IntPtr, dwFlags As Integer, lpExeName As System.Text.StringBuilder, ByRef size As Integer) As Boolean
    End Function
    <System.Runtime.InteropServices.DllImport("kernel32.dll")>
    Private Function OpenProcess(dwDesiredAccess As ProcessAccessFlags, bInheritHandle As Boolean, dwProcessId As Integer) As IntPtr
    End Function
    <System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError:=True)>
    Private Function CloseHandle(hHandle As IntPtr) As Boolean
    End Function
    Enum ProcessAccessFlags As UInteger
        All = &H1F0FFF
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
    ''' <summary>
    ''' Returns the executable path of a process.
    ''' </summary>
    ''' <param name="this"></param>
    ''' <returns></returns>
    <System.Runtime.CompilerServices.Extension()>
    Public Function Path(ByVal this As Process) As String
        Dim processPath As String = ""

        ' The new QueryLimitedInformation flag is only available on Windows Vista and up.
        If Environment.OSVersion.Version.Major >= 6 Then
            Dim processHandle As IntPtr = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, False, this?.Id)
            Try
                If Not processHandle = IntPtr.Zero Then
                    Dim buffer As System.Text.StringBuilder = New System.Text.StringBuilder(1024)
                    If QueryFullProcessImageName(processHandle, 0, buffer, buffer.Capacity) Then
                        processPath = buffer.ToString()
                    End If
                End If
            Finally
                CloseHandle(processHandle)
            End Try
        Else
            processPath = this.MainModule.FileName
        End If

        Return processPath
    End Function
    ''' <summary>
    ''' Checks if a processes classname is in pipe separated string
    ''' </summary>
    ''' <param name="pp"></param>
    ''' <param name="classes"></param>
    ''' <returns></returns>
    <System.Runtime.CompilerServices.Extension()>
    Public Function IsClassNameIn(pp As Process, classes As String) As Boolean
        Dim wndClass As String() = classes.Split({"|"}, StringSplitOptions.RemoveEmptyEntries)
        For i As Integer = 0 To UBound(wndClass)
            wndClass(i) = Trim(wndClass(i))
        Next
        'Debug.Print("""" & wndClass(0) & """")
        Return wndClass.Contains(pp.GetWindowClass())
    End Function
    <Runtime.InteropServices.DllImport("user32.dll", CharSet:=Runtime.InteropServices.CharSet.Auto)>
    Private Sub GetClassName(ByVal hWnd As System.IntPtr, ByVal lpClassName As System.Text.StringBuilder, ByVal nMaxCount As Integer)
    End Sub
    <Runtime.CompilerServices.Extension()>
    Private Function GetWindowClass(pp As Process) As String
        Dim sClassName As New System.Text.StringBuilder("", 256)
        Call GetClassName(pp.MainWindowHandle, sClassName, 256)
        Return sClassName.ToString
    End Function
End Module
