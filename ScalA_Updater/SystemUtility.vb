Imports System.Runtime.InteropServices
''' <summary>
''' Utility for accessing window IShell* interfaces in order to use them to launch a process unelevated
''' </summary>
Public Module SystemUtility
    ''' <summary>
    ''' We are elevated And should launch the process unelevated. We can't create the
    ''' process directly without it becoming elevated. So to workaround this, we have
    ''' explorer do the process creation (explorer Is typically running unelevated).
    ''' </summary>
    ''' <param name="process"></param>
    ''' <param name="args"></param>
    ''' <param name="currentDirectory"></param>
    Public Sub ExecuteProcessUnElevated(process As String, args As String, Optional currentDirectory As String = "")

        Dim shellWindows As IShellWindows = New CShellWindows()

        ' Get the desktop window
        Dim Loc() As Object = {CSIDL_Desktop}
        Dim unused As New Object()
        Dim hwnd As Integer
        Dim serviceProvider As IServiceProvider = CType(shellWindows.FindWindowSW(Loc, unused, SWC_DESKTOP, hwnd, SWFO_NEEDDISPATCH), IServiceProvider)

        ' Get the shell browser
        Dim serviceGuid As Guid = SID_STopLevelBrowser
        Dim interfaceGuid As Guid = GetType(IShellBrowser).GUID
        Dim shellBrowser As IShellBrowser = CType(serviceProvider.QueryService(serviceGuid, interfaceGuid), IShellBrowser)

        ' Get the shell dispatch
        Dim dispatch As Guid = GetType(IDispatch).GUID
        Dim folderView As IShellFolderViewDual = shellBrowser.QueryActiveShellView().GetItemObject(SVGIO_BACKGROUND, dispatch)
        Dim shellDispatch As IShellDispatch2 = folderView.application

        ' Use the dispatch (which Is unelevated) to launch the process for us
        shellDispatch.ShellExecute(process, args, currentDirectory, String.Empty, SW_SHOWNORMAL)
    End Sub

    ''' <summary>
    ''' Interop definitions
    ''' </summary>
    Private Const CSIDL_Desktop As Integer = 0
    Private Const SWC_DESKTOP As Integer = 8
    Private Const SWFO_NEEDDISPATCH As Integer = 1
    Private Const SW_SHOWNORMAL As Integer = 1
    Private Const SVGIO_BACKGROUND As Integer = 0
    Private ReadOnly SID_STopLevelBrowser As New Guid("4C96BE40-915C-11CF-99D3-00AA004AE837")

    <ComImport(), Guid("9BA05972-F6A8-11CF-A442-00A0C90A8F39"), ClassInterfaceAttribute(ClassInterfaceType.None)>
    Private NotInheritable Class CShellWindows
    End Class

    <ComImport(), Guid("85CB6900-4D95-11CF-960C-0080C7F4EE85"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)>
    Private Interface IShellWindows
        Function FindWindowSW(<MarshalAs(UnmanagedType.Struct)> ByRef pvarloc As Object, <MarshalAs(UnmanagedType.Struct)> ByRef pvarlocRoot As Object, swClass As Integer, ByRef pHWND As Integer, swfwOptions As Integer) As <MarshalAs(UnmanagedType.IDispatch)> Object
    End Interface

    <ComImport(), Guid("6d5140c1-7436-11ce-8034-00aa006009fa"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IServiceProvider
        Function QueryService(ByRef guidService As Guid, ByRef riid As Guid) As <MarshalAs(UnmanagedType.Interface)> Object
    End Interface

    <ComImport(), Guid("000214E2-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IShellBrowser
        Function VTableGap01() ' GetWindow
        Function VTableGap02() ' ContextSensitiveHelp
        Function VTableGap03() ' InsertMenusSB
        Function VTableGap04() ' SetMenuSB
        Function VTableGap05() ' RemoveMenusSB
        Function VTableGap06() ' SetStatusTextSB
        Function VTableGap07() ' EnableModelessSB
        Function VTableGap08() ' TranslateAcceleratorSB
        Function VTableGap09() ' BrowseObject
        Function VTableGap10() ' GetViewStateStream
        Function VTableGap11() ' GetControlWindow
        Function VTableGap12() ' SendControlMsg
        Function QueryActiveShellView() As IShellView
    End Interface

    <ComImport(), Guid("000214E3-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IShellView
        Function VTableGap01() ' GetWindow
        Function VTableGap02() ' ContextSensitiveHelp
        Function VTableGap03() ' TranslateAcceleratorA
        Function VTableGap04() ' EnableModeless
        Function VTableGap05() ' UIActivate
        Function VTableGap06() ' Refresh
        Function VTableGap07() ' CreateViewWindow
        Function VTableGap08() ' DestroyViewWindow
        Function VTableGap09() ' GetCurrentInfo
        Function VTableGap10() ' AddPropertySheetPages
        Function VTableGap11() ' SaveViewState
        Function VTableGap12() ' SelectItem
        Function GetItemObject(aspectOfView As UInt32, ByRef riid As Guid) As <MarshalAs(UnmanagedType.Interface)> Object
    End Interface

    <ComImport(), Guid("00020400-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)>
    Private Interface IDispatch
    End Interface

    <ComImport(), Guid("E7A1AF80-4D96-11CF-960C-0080C7F4EE85"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)>
    Private Interface IShellFolderViewDual
        ReadOnly Property Application As <MarshalAs(UnmanagedType.IDispatch)> Object
    End Interface

    <ComImport(), Guid("A4C6892C-3BA9-11D2-9DEA-00C04FB16162"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)>
    Private Interface IShellDispatch2
        Sub ShellExecute(<MarshalAs(UnmanagedType.BStr)> File As String,
                         <MarshalAs(UnmanagedType.Struct)> vArgs As Object,
                         <MarshalAs(UnmanagedType.Struct)> vDir As Object,
                         <MarshalAs(UnmanagedType.Struct)> vOperation As Object,
                         <MarshalAs(UnmanagedType.Struct)> vShow As Object)
    End Interface
End Module
