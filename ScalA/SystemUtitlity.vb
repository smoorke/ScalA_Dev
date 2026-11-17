Imports System.Reflection
Imports System.Runtime.InteropServices
''' <summary>
''' Utility for accessing window IShell* interfaces in order to use them to launch a process unelevated
''' </summary>
Public Class SystemUtility
    ''' <summary>
    ''' We are elevated And should launch the process unelevated. We can't create the
    ''' process directly without it becoming elevated. So to workaround this, we have
    ''' explorer do the process creation (explorer Is typically running unelevated).
    ''' </summary>
    ''' <param name="process"></param>
    ''' <param name="args"></param>
    ''' <param name="currentDirectory"></param>
    Public Shared Sub ExecuteProcessUnElevated(process As String, args As String, Optional currentDirectory As String = "", Optional verb As String = "")
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

        ' Get the folder view and its Application dispatch object via reflection
        Dim dispatch As Guid = GetType(IDispatch).GUID
        Dim folderView As Object = shellBrowser.QueryActiveShellView().GetItemObject(SVGIO_BACKGROUND, dispatch)
        If folderView Is Nothing Then Exit Sub

        Dim shellDispatch As Object = folderView.GetType().InvokeMember("Application", BindingFlags.GetProperty, Nothing, folderView, Nothing)
        If shellDispatch Is Nothing Then Exit Sub

        ' Invoke ShellExecute dynamically by name (bypasses interop DispID mismatch)
        shellDispatch.GetType().InvokeMember("ShellExecute",
                                             BindingFlags.InvokeMethod,
                                             Nothing,
                                             shellDispatch,
                                             New Object() {process, If(args, ""), If(currentDirectory, ""), If(verb, ""), SW_SHOWNORMAL})
    End Sub

    ''' <summary>
    ''' Invokes a Shell context-menu verb ("copy", "cut", "paste", etc.) inside explorer.exe's process
    ''' so clipboard data persists even after this application exits or restarts.
    ''' </summary>
    Public Shared Sub ExplorerVerbDispatch(fullPath As String, verb As String)
        If String.IsNullOrWhiteSpace(fullPath) OrElse String.IsNullOrWhiteSpace(verb) Then Exit Sub

        Dim cleanPath As String = fullPath.TrimEnd("\"c)
        If cleanPath.Length = 2 AndAlso cleanPath(1) = ":"c Then
            cleanPath &= "\"
        End If

        Dim shellWindows As IShellWindows = New CShellWindows()

        ' 1. Get the unelevated Desktop window from explorer.exe
        Dim Loc() As Object = {CSIDL_Desktop}
        Dim unused As New Object()
        Dim hwnd As Integer
        Dim serviceProvider As IServiceProvider = CType(shellWindows.FindWindowSW(Loc, unused, SWC_DESKTOP, hwnd, SWFO_NEEDDISPATCH), IServiceProvider)

        ' 2. Get the top-level IShellBrowser
        Dim serviceGuid As Guid = SID_STopLevelBrowser
        Dim interfaceGuid As Guid = GetType(IShellBrowser).GUID
        Dim shellBrowser As IShellBrowser = CType(serviceProvider.QueryService(serviceGuid, interfaceGuid), IShellBrowser)

        ' 3. Get folderView and shellDispatch via reflection
        Dim dispatch As Guid = GetType(IDispatch).GUID
        Dim folderView As Object = shellBrowser.QueryActiveShellView().GetItemObject(SVGIO_BACKGROUND, dispatch)
        If folderView Is Nothing Then Exit Sub

        Dim shellDispatch As Object = folderView.GetType().InvokeMember("Application", BindingFlags.GetProperty, Nothing, folderView, Nothing)
        If shellDispatch Is Nothing Then Exit Sub

        ' 4. Bind to parent Folder and target FolderItem using NameSpace / ParseName reflection
        Dim parentPath As String = IO.Path.GetDirectoryName(cleanPath)
        Dim itemName As String = IO.Path.GetFileName(cleanPath)

        Dim folder As Object = Nothing
        If String.IsNullOrEmpty(parentPath) Then
            folder = shellDispatch.GetType().InvokeMember("NameSpace", BindingFlags.InvokeMethod, Nothing, shellDispatch, New Object() {cleanPath})
            If folder IsNot Nothing Then
                Dim selfItem = folder.GetType().InvokeMember("Self", BindingFlags.GetProperty, Nothing, folder, Nothing)
                If selfItem IsNot Nothing Then
                    selfItem.GetType().InvokeMember("InvokeVerb", BindingFlags.InvokeMethod, Nothing, selfItem, New Object() {verb})
                End If
            End If
            Exit Sub
        End If

        folder = shellDispatch.GetType().InvokeMember("NameSpace", BindingFlags.InvokeMethod, Nothing, shellDispatch, New Object() {parentPath})
        If folder IsNot Nothing Then
            Dim targetItem = folder.GetType().InvokeMember("ParseName", BindingFlags.InvokeMethod, Nothing, folder, New Object() {itemName})
            If targetItem IsNot Nothing Then
                targetItem.GetType().InvokeMember("InvokeVerb", BindingFlags.InvokeMethod, Nothing, targetItem, New Object() {verb})
            End If
        End If
    End Sub

    ''' <summary>
    ''' Interop definitions
    ''' </summary>
    Private Const CSIDL_Desktop As Integer = 0
    Private Const SWC_DESKTOP As Integer = 8
    Private Const SWFO_NEEDDISPATCH As Integer = 1
    'Private Const SW_SHOWNORMAL As Integer = 1
    Private Const SVGIO_BACKGROUND As Integer = 0
    Private Shared ReadOnly SID_STopLevelBrowser As New Guid("4C96BE40-915C-11CF-99D3-00AA004AE837")

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

End Class
