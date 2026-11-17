Imports System.Runtime.InteropServices
''' <summary>
''' Utility for accessing window IShell* interfaces in order to use them to launch a process unelevated
''' </summary>
Public Module SystemUtitlity
    ''' <summary>
    ''' We are elevated And should launch the process unelevated. We can't create the
    ''' process directly without it becoming elevated. So to workaround this, we have
    ''' explorer do the process creation (explorer Is typically running unelevated).
    ''' </summary>
    ''' <param name="process"></param>
    ''' <param name="args"></param>
    ''' <param name="currentDirectory"></param>
    Public Sub ExecuteProcessUnElevated(process As String, args As String, Optional currentDirectory As String = "", Optional verb As String = "")

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
        Dim shellDispatch As IShellDispatch2 = folderView.Application

        ' Use the dispatch (which Is unelevated) to launch the process for us
        shellDispatch.ShellExecute(process, args, currentDirectory, verb, SW_SHOWNORMAL)
    End Sub

    ''' <summary>
    ''' Invokes a Shell context-menu verb ("copy", "cut", "paste", etc.) inside explorer.exe's process
    ''' so clipboard data persists even after this application exits or restarts.
    ''' </summary>
    Public Sub ExplorerVerbDispatch(fullPath As String, verb As String)
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

        ' 3. Get IShellDispatch2 hosted inside explorer.exe
        Dim dispatch As Guid = GetType(IDispatch).GUID
        Dim folderView As IShellFolderViewDual = CType(shellBrowser.QueryActiveShellView().GetItemObject(SVGIO_BACKGROUND, dispatch), IShellFolderViewDual)
        Dim shellDispatch As IShellDispatch2 = CType(folderView.Application, IShellDispatch2)

        ' 4. Bind to parent Folder and target FolderItem inside explorer.exe, then invoke the verb
        Dim parentPath As String = IO.Path.GetDirectoryName(cleanPath)
        Dim itemName As String = IO.Path.GetFileName(cleanPath)

        ' Handle drive root edge case (e.g., "C:\")
        If String.IsNullOrEmpty(parentPath) Then
            Dim rootFolder As Folder = shellDispatch.NameSpace(cleanPath)
            If rootFolder IsNot Nothing Then
                Dim rootItem As FolderItem = TryCast(rootFolder, Folder2)?.Self
                If rootItem IsNot Nothing Then rootItem.InvokeVerb(verb)
            End If
            Exit Sub
        End If

        Dim parentFolder As Folder = shellDispatch.NameSpace(parentPath)
        If parentFolder IsNot Nothing Then
            Dim targetItem As FolderItem = parentFolder.ParseName(itemName)
            If targetItem IsNot Nothing Then
                targetItem.InvokeVerb(verb)
            End If
        End If
    End Sub



    <ComImport, Guid("BBCBDE60-C3FF-11CE-8350-444553540000"), InterfaceType(ComInterfaceType.InterfaceIsDual)>
    Public Interface Folder
        <DispId(0)>
        ReadOnly Property Title As String

        <DispId(&H60020001)>
        ReadOnly Property Application As <MarshalAs(UnmanagedType.IDispatch)> Object

        <DispId(&H60020002)>
        ReadOnly Property Parent As <MarshalAs(UnmanagedType.IDispatch)> Object

        <DispId(&H60020003)>
        ReadOnly Property ParentFolder As Folder

        <DispId(&H60020004)>
        Function Items() As <MarshalAs(UnmanagedType.Interface)> Object ' FolderItems

        <DispId(&H60020005)>
        Function ParseName(<MarshalAs(UnmanagedType.BStr)> bName As String) As FolderItem

        <DispId(&H60020006)>
        Sub NewFolder(<MarshalAs(UnmanagedType.BStr)> bName As String, <[Optional], MarshalAs(UnmanagedType.Struct)> vOptions As Object)

        <DispId(&H60020007)>
        Sub MoveHere(<MarshalAs(UnmanagedType.Struct)> vItem As Object, <[Optional], MarshalAs(UnmanagedType.Struct)> vOptions As Object)

        <DispId(&H60020008)>
        Sub CopyHere(<MarshalAs(UnmanagedType.Struct)> vItem As Object, <[Optional], MarshalAs(UnmanagedType.Struct)> vOptions As Object)

        <DispId(&H60020009)>
        Function GetDetailsOf(<MarshalAs(UnmanagedType.Struct)> vItem As Object, iColumn As Integer) As <MarshalAs(UnmanagedType.BStr)> String
    End Interface

    <ComImport, Guid("F0D2D8EF-3890-11D2-BF8B-00C04FB93661"), InterfaceType(ComInterfaceType.InterfaceIsDual)>
    Public Interface Folder2
        ' --- Inherited from Folder (must appear in exact vtable order) ---
        <DispId(0)>
        ReadOnly Property Title As String

        <DispId(&H60020001)>
        ReadOnly Property Application As <MarshalAs(UnmanagedType.IDispatch)> Object

        <DispId(&H60020002)>
        ReadOnly Property Parent As <MarshalAs(UnmanagedType.IDispatch)> Object

        <DispId(&H60020003)>
        ReadOnly Property ParentFolder As Folder

        <DispId(&H60020004)>
        Function Items() As <MarshalAs(UnmanagedType.Interface)> Object

        <DispId(&H60020005)>
        Function ParseName(<MarshalAs(UnmanagedType.BStr)> bName As String) As FolderItem

        <DispId(&H60020006)>
        Sub NewFolder(<MarshalAs(UnmanagedType.BStr)> bName As String, <[Optional], MarshalAs(UnmanagedType.Struct)> vOptions As Object)

        <DispId(&H60020007)>
        Sub MoveHere(<MarshalAs(UnmanagedType.Struct)> vItem As Object, <[Optional], MarshalAs(UnmanagedType.Struct)> vOptions As Object)

        <DispId(&H60020008)>
        Sub CopyHere(<MarshalAs(UnmanagedType.Struct)> vItem As Object, <[Optional], MarshalAs(UnmanagedType.Struct)> vOptions As Object)

        <DispId(&H60020009)>
        Function GetDetailsOf(<MarshalAs(UnmanagedType.Struct)> vItem As Object, iColumn As Integer) As <MarshalAs(UnmanagedType.BStr)> String

        ' --- Folder2 specific members ---
        <DispId(&H60030000)>
        ReadOnly Property Self As FolderItem

        <DispId(&H60030001)>
        ReadOnly Property OfflineStatus As Integer

        <DispId(&H60030002)>
        Sub Synchronize()

        <DispId(1)>
        ReadOnly Property HaveToShowWebViewBarricade As Boolean

        <DispId(&H60030004)>
        Sub DismissedWebViewBarricade()
    End Interface

    <ComImport, Guid("FAC32C80-CBE4-11CE-8350-444553540000"), InterfaceType(ComInterfaceType.InterfaceIsDual)>
    Public Interface FolderItem
        <DispId(&H60020000)>
        ReadOnly Property Application As <MarshalAs(UnmanagedType.IDispatch)> Object

        <DispId(&H60020001)>
        ReadOnly Property Parent As <MarshalAs(UnmanagedType.IDispatch)> Object

        <DispId(0)>
        Property Name As <MarshalAs(UnmanagedType.BStr)> String

        <DispId(&H60020003)>
        ReadOnly Property Path As <MarshalAs(UnmanagedType.BStr)> String

        <DispId(&H60020004)>
        ReadOnly Property GetLink As <MarshalAs(UnmanagedType.IDispatch)> Object

        <DispId(&H60020005)>
        ReadOnly Property GetFolder As <MarshalAs(UnmanagedType.IDispatch)> Object

        <DispId(&H60020006)>
        ReadOnly Property IsLink As Boolean

        <DispId(&H60020007)>
        ReadOnly Property IsFolder As Boolean

        <DispId(&H60020008)>
        ReadOnly Property IsFileSystem As Boolean

        <DispId(&H60020009)>
        ReadOnly Property IsBrowsable As Boolean

        <DispId(&H6002000A)>
        Property ModifyDate As Date

        <DispId(&H6002000C)>
        ReadOnly Property Size As Integer

        <DispId(&H6002000D)>
        ReadOnly Property Type As <MarshalAs(UnmanagedType.BStr)> String

        <DispId(&H6002000E)>
        Function Verbs() As <MarshalAs(UnmanagedType.Interface)> Object ' FolderItemVerbs

        <DispId(&H6002000F)>
        Sub InvokeVerb(<[Optional], MarshalAs(UnmanagedType.Struct)> vVerb As Object)
    End Interface



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
        <DispId(&H60020002)>
        Function [NameSpace](<MarshalAs(UnmanagedType.Struct)> vDir As Object) As Folder

        <DispId(&H60030000)>
        Sub ShellExecute(<MarshalAs(UnmanagedType.BStr)> File As String,
                     <MarshalAs(UnmanagedType.Struct)> vArgs As Object,
                     <MarshalAs(UnmanagedType.Struct)> vDir As Object,
                     <MarshalAs(UnmanagedType.Struct)> vOperation As Object,
                     <MarshalAs(UnmanagedType.Struct)> vShow As Object)
    End Interface
End Module
