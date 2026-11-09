Imports System.Runtime.InteropServices

Module DpiSafe

    ' Once tripped, we permanently bypass Try/Catch and OS checks.
    Private s_hasFailedBefore As Boolean = False

    Public Function GetDpiForMonitorSafe(
         ByVal hmonitor As IntPtr,
         ByVal dpiType As Integer,
         ByRef dpiX As UInteger,
         ByRef dpiY As UInteger
     ) As Integer

        ' --- FAST PATH ---
        If s_hasFailedBefore Then
            Return FallbackToLegacyDpi(dpiX, dpiY)
        End If

        ' --- GUARD CLAUSE: OS Version Check ---
        If Environment.OSVersion.Version.Major < 6 OrElse
           (Environment.OSVersion.Version.Major = 6 AndAlso Environment.OSVersion.Version.Minor < 3) Then
            s_hasFailedBefore = True
            Return FallbackToLegacyDpi(dpiX, dpiY)
        End If

        Try
            Dim hr As Integer = GetDpiForMonitor(hmonitor, dpiType, dpiX, dpiY)

            ' Check if HRESULT indicates failure or returned invalid data
            If hr <> 0 OrElse dpiY = 0 Then
                s_hasFailedBefore = True
                Return FallbackToLegacyDpi(dpiX, dpiY)
            End If

            Return hr ' Returns 0 (S_OK)
        Catch
            s_hasFailedBefore = True
            Return FallbackToLegacyDpi(dpiX, dpiY)
        End Try
    End Function

    ''' <summary>
    ''' Helper that populates output refs with legacy global GDI DPI 
    ''' and returns a simulated failure HRESULT (-1 / E_FAIL equivalent).
    ''' </summary>
    Private Function FallbackToLegacyDpi(ByRef dpiX As UInteger, ByRef dpiY As UInteger) As Integer
        Dim legacyDpi As UInteger = CUInt(GetLegacyGlobalDpi())
        dpiX = legacyDpi
        dpiY = legacyDpi
        Return -1 ' E_FAIL equivalent to signal fallback was used
    End Function
    ''' <summary>
    ''' Drop-in safe wrapper for GetDpiForWindow. Falls back to legacy primary screen DC DPI on Win 7/8.
    ''' </summary>
    Public Function GetDpiForWindowSafe(hwnd As IntPtr) As UInteger
        If s_hasFailedBefore Then
            Return CUInt(GetLegacyGlobalDpi())
        End If

        ' Win 10+ check (Major version < 10)
        If Environment.OSVersion.Version.Major < 10 Then
            s_hasFailedBefore = True
            Return CUInt(GetLegacyGlobalDpi())
        End If

        Try
            Dim dpi As UInteger = GetDpiForWindow(hwnd)
            If dpi = 0 Then
                s_hasFailedBefore = True
                Return CUInt(GetLegacyGlobalDpi())
            End If
            Return dpi
        Catch
            s_hasFailedBefore = True
            Return CUInt(GetLegacyGlobalDpi())
        End Try
    End Function
    ''' <summary>
    ''' Drop-in safe wrapper for AdjustWindowRectExForDpi. Falls back to standard AdjustWindowRectEx on Win 7/8.
    ''' </summary>
    Public Function AdjustWindowRectExForDpiSafe(ByRef lpRect As RECT, dwStyle As UInteger, bMenu As Boolean, dwExStyle As UInteger, dpi As UInteger) As Boolean
        If Environment.OSVersion.Version.Major >= 10 Then
            Try
                If AdjustWindowRectExForDpi(lpRect, dwStyle, bMenu, dwExStyle, dpi) Then
                    Return True
                End If
            Catch
                ' Fall through to legacy calculation on exception
            End Try
        End If

        ' --- Legacy Fallback for Windows 7 / 8 ---
        ' On pre-Win10 systems, adjust using standard rect math scaled manually or standard API
        Return AdjustWindowRectEx(lpRect, dwStyle, bMenu, dwExStyle)
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Function AdjustWindowRectEx(ByRef lpRect As RECT, dwStyle As UInteger, bMenu As Boolean, dwExStyle As UInteger) As Boolean
    End Function

    Private Function GetLegacyGlobalDpi() As Integer
        Dim hdc As IntPtr = IntPtr.Zero
        Try
            hdc = GetDC(IntPtr.Zero)
            If hdc = IntPtr.Zero Then Return 96
            Return GetDeviceCaps(hdc, 90)
        Finally
            If hdc <> IntPtr.Zero Then ReleaseDC(IntPtr.Zero, hdc)
        End Try
    End Function


End Module
