Imports System.Runtime.InteropServices

Public NotInheritable Class TaskbarManager

#Region " AppUserModelID "
    Public Shared ReadOnly Property IsPlatformSupported As Boolean
        Get
            Return Environment.OSVersion.Version.Major >= 6
        End Get
    End Property

    ' Sets the AppUserModelID for a specific window
    Public Shared Sub SetApplicationIdForSpecificWindow(hwnd As IntPtr, appId As String)
        If Not IsPlatformSupported Then Return
        SetWindowAppId(hwnd, appId)
    End Sub

    Private Shared Sub SetWindowAppId(hwnd As IntPtr, appId As String)
        Dim guid As New Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99") ' IID_IPropertyStore
        Dim propStore As IPropertyStore = Nothing
        Dim hr = SHGetPropertyStoreForWindow(hwnd, guid, propStore)
        If hr = 0 AndAlso propStore IsNot Nothing Then
            Dim pv As New PROPVARIANT()
            pv.SetString(appId)
            propStore.SetValue(PKEY_AppUserModel_ID, pv)
            propStore.Commit()
            pv.Clear()
            Marshal.ReleaseComObject(propStore)
        End If
    End Sub

    <DllImport("shell32.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function SHGetPropertyStoreForWindow(hwnd As IntPtr, ByRef riid As Guid, <Out> ByRef ppv As IPropertyStore) As Integer
    End Function

    <ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99")>
    Private Interface IPropertyStore
        Sub GetCount(<Out> ByRef cProps As UInteger)
        Sub GetAt(iProp As UInteger, <Out> ByRef pkey As PROPERTYKEY)
        Sub GetValue(ByRef key As PROPERTYKEY, <Out> ByRef pv As PROPVARIANT)
        Sub SetValue(ByRef key As PROPERTYKEY, ByRef pv As PROPVARIANT)
        Sub Commit()
    End Interface

    <StructLayout(LayoutKind.Sequential, Pack:=4)>
    Private Structure PROPERTYKEY
        Public fmtid As Guid
        Public pid As UInteger
        Public Sub New(formatId As Guid, propertyId As UInteger)
            fmtid = formatId
            pid = propertyId
        End Sub
    End Structure

    ' Manual PROPVARIANT implementation for VT_LPWSTR (string)
    <StructLayout(LayoutKind.Sequential)>
    Private Structure PROPVARIANT
        Public vt As UShort
        Public wReserved1 As UShort
        Public wReserved2 As UShort
        Public wReserved3 As UShort
        Public p As IntPtr
        Public p2 As IntPtr

        Public Sub SetString(value As String)
            Clear()
            If value Is Nothing Then
                vt = 31 ' VT_LPWSTR
                p = IntPtr.Zero
            Else
                vt = 31 ' VT_LPWSTR
                p = Marshal.StringToCoTaskMemUni(value)
            End If
        End Sub

        Public Sub Clear()
            If vt = 31 AndAlso p <> IntPtr.Zero Then
                Marshal.FreeCoTaskMem(p)
                p = IntPtr.Zero
            End If
            vt = 0
        End Sub
    End Structure

    ' PKEY_AppUserModel_ID definition
    Private Shared ReadOnly PKEY_AppUserModel_ID As PROPERTYKEY = New PROPERTYKEY(New Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 5)

#End Region

#Region " TaskBarList "

    Private Shared _TbList As ITaskbarList4 = Nothing

    Private Shared Sub TbLInit()
        If _TbList Is Nothing Then
            _TbList = CType(New CTaskbarList(), ITaskbarList4)
            _TbList.HrInit()
        End If
    End Sub

    Public Shared Sub GetUserAttention(ByVal hwnd As IntPtr)
        If Not IsPlatformSupported Then Return
        TbLInit()

        If IsWindows11OrHigher() Then
            _TbList.SetProgressState(hwnd, TaskbarProgressBarState.Indeterminate)
        Else
            _TbList.SetProgressState(hwnd, TaskbarProgressBarState.Paused)
            _TbList.SetProgressValue(hwnd, 100, 100)
        End If

        Debug.Print("Taskbar attention state set")
    End Sub

    Public Sub ClearTaskbarState(ByVal hwnd As IntPtr)
        If Not IsPlatformSupported Then Return
        TbLInit()

        _TbList.SetProgressState(hwnd, TaskbarProgressBarState.NoProgress)

        Debug.Print("Taskbar state cleared.")
    End Sub

    Private Shared Function IsWindows11OrHigher() As Boolean
        ' If RuntimeInformation.IsOSPlatform(OSPlatform.Windows) Then
        '' Note: only major distinction, exact build requires registry or WMI
        Dim version = Environment.OSVersion.Version
        '' Windows 10: Major 10, Build < 22000
        '' Windows 11: Major 10, Build >= 22000
        Return version.Major >= 10 AndAlso version.Build >= 22000
        'End If
        'Return False
    End Function
    <ComImport>
    <Guid("56FDF344-FD6D-11d0-958A-006097C9A090")>
    Private Class CTaskbarList : End Class
    ''' <remarks>
    ''' <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd562040(v=vs.85).aspx"/>
    ''' </remarks>
    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("C43DC798-95D1-4BEA-9030-BB99E2983A1A")>
    Private Interface ITaskbarList4

#Region " ITaskbarList "

        <PreserveSig>
        Sub HrInit()

        <PreserveSig>
        Sub AddTab(ByVal hwnd As IntPtr)

        <PreserveSig>
        Sub DeleteTab(ByVal hwnd As IntPtr)

        <PreserveSig>
        Sub ActivateTab(ByVal hwnd As IntPtr)

        <PreserveSig>
        Sub SetActiveAlt(ByVal hwnd As IntPtr)

#End Region

#Region " ITaskbarList2 "

        <PreserveSig>
        Sub MarkFullscreenWindow(ByVal hwnd As IntPtr, <MarshalAs(UnmanagedType.Bool)> ByVal fFullscreen As Boolean)

#End Region

#Region " ITaskbarList3 "

        <PreserveSig>
        Sub SetProgressValue(ByVal hwnd As IntPtr, ByVal ullCompleted As UInt64, ByVal ullTotal As UInt64)

        <PreserveSig>
        Sub SetProgressState(ByVal hwnd As IntPtr, ByVal tbpFlags As TaskbarProgressBarState)

        <PreserveSig>
        Sub RegisterTab(ByVal hwndTab As IntPtr, ByVal hwndMdi As IntPtr)

        <PreserveSig>
        Sub UnregisterTab(ByVal hwndTab As IntPtr)

        <PreserveSig>
        Sub SetTabOrder(ByVal hwndTab As IntPtr, ByVal hwndInsertBefore As IntPtr)

        <PreserveSig>
        Sub SetTabActive(ByVal hwndTab As IntPtr, ByVal hwndInsertBefore As IntPtr, ByVal dwReserved As UInteger)

        <PreserveSig>
        Function Fake1() As Integer
        ' Function ThumbBarAddButtons(byval hwnd As IntPtr, byval cButtons As UInteger, <MarshalAs(UnmanagedType.LPArray)> byval pButtons As ThumbButton()) As HResult

        <PreserveSig>
        Function Fake2() As Integer
        ' Function ThumbBarUpdateButtons(byval hwnd As IntPtr, byval cButtons As UInteger, <MarshalAs(UnmanagedType.LPArray)> byval pButtons As ThumbButton()) As HResult

        <PreserveSig>
        Sub Fake3()
        ' Sub ThumbBarSetImageList(byval hwnd As IntPtr, byval himl As IntPtr)

        <PreserveSig>
        Sub SetOverlayIcon(ByVal hwnd As IntPtr, ByVal hIcon As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDescription As String)

        <PreserveSig>
        Sub SetThumbnailTooltip(ByVal hwnd As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> ByVal pszTip As String)

        <PreserveSig>
        Sub SetThumbnailClip(ByVal hwnd As IntPtr, ByVal prcClip As IntPtr)

#End Region

#Region " ITaskbarList4 "

        Sub Fake4()
        ' Sub SetTabProperties(byval hwndTab As IntPtr, byval stpFlags As SetTabPropertiesOption)

#End Region

    End Interface
    ' Taskbar progress states
    Public Enum TaskbarProgressBarState
        NoProgress = 0
        Indeterminate = &H1
        Normal = &H2
        [Error] = &H4
        Paused = &H8
    End Enum
#End Region
End Class
