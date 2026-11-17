Imports System.Runtime.InteropServices
Imports System.Text

Public Class ExplorerUndoRedoHelper

    ' --- Win32 Constants ---
    Private Const CMF_NORMAL As UInteger = &H0
    Private Const GCS_VERBW As UInteger = &H4
    Private Const MF_BYPOSITION As UInteger = &H400
    Private Const SVGIO_BACKGROUND As UInteger = &H0
    Private Const ID_CMD_FIRST As UInteger = 1
    Private Const ID_CMD_LAST As UInteger = &H7FFF

    Private Const WM_COMMAND As UInteger = &H111
    Private Const FCIDM_SHVIEW_UNDO As Integer = &H701B ' 28699 - Canonical DefView Undo command ID
    Private Const FCIDM_SHVIEW_REDO As Integer = &H7020 ' 28704 - Canonical DefView Redo command ID

    ' --- COM Interfaces ---
    <ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214E6-0000-0000-C000-000000000046")>
    Private Interface IShellFolder
        Sub ParseDisplayName(hwnd As IntPtr, pbc As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> pszDisplayName As String, ByRef pchEaten As UInteger, ByRef ppidl As IntPtr, ByRef pdwAttributes As UInteger)
        Sub EnumObjects(hwnd As IntPtr, grfFlags As UInteger, ByRef ppenumIDList As IntPtr)
        Sub BindToObject(pidl As IntPtr, pbc As IntPtr, ByRef riid As Guid, <MarshalAs(UnmanagedType.Interface)> ByRef ppv As Object)
        Sub BindToStorage(pidl As IntPtr, pbc As IntPtr, ByRef riid As Guid, <MarshalAs(UnmanagedType.Interface)> ByRef ppv As Object)
        <PreserveSig> Function CompareIDs(lParam As IntPtr, pidl1 As IntPtr, pidl2 As IntPtr) As Integer
        Sub CreateViewObject(hwndOwner As IntPtr, ByRef riid As Guid, <MarshalAs(UnmanagedType.Interface)> ByRef ppv As Object)
        Sub GetAttributesOf(cidl As UInteger, apidl As IntPtr, ByRef rgfInOut As UInteger)
        Sub GetUIObjectOf(hwndOwner As IntPtr, cidl As UInteger, apidl As IntPtr, ByRef riid As Guid, rgfReserved As IntPtr, <MarshalAs(UnmanagedType.Interface)> ByRef ppv As Object)
        Sub GetDisplayNameOf(pidl As IntPtr, uFlags As UInteger, pName As IntPtr)
        Sub SetNameOf(hwnd As IntPtr, pidl As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> pszName As String, uFlags As UInteger, ByRef ppidlOut As IntPtr)
    End Interface

    <ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214E3-0000-0000-C000-000000000046")>
    Private Interface IShellView
        <PreserveSig> Function GetWindow(ByRef phwnd As IntPtr) As Integer
        <PreserveSig> Function ContextSensitiveHelp(fEnterMode As Boolean) As Integer
        <PreserveSig> Function TranslateAccelerator(pmsg As IntPtr) As Integer
        <PreserveSig> Function EnableModeless(fEnable As Boolean) As Integer
        <PreserveSig> Function UIActivate(uState As UInteger) As Integer
        <PreserveSig> Function Refresh() As Integer
        <PreserveSig> Function CreateViewWindow(psvPrevious As IShellView, pfs As IntPtr, psb As IntPtr, prcView As IntPtr, ByRef phWnd As IntPtr) As Integer
        <PreserveSig> Function DestroyViewWindow() As Integer
        <PreserveSig> Function GetCurrentInfo(pfs As IntPtr) As Integer
        <PreserveSig> Function AddPropertySheetPages(dwReserved As UInteger, pfn As IntPtr, lparam As IntPtr) As Integer
        <PreserveSig> Function SaveViewState() As Integer
        <PreserveSig> Function SelectItem(pidlItem As IntPtr, uFlags As UInteger) As Integer
        <PreserveSig> Function GetItemObject(uItem As UInteger, ByRef riid As Guid, <MarshalAs(UnmanagedType.Interface)> ByRef ppv As Object) As Integer
    End Interface

    <ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214e4-0000-0000-c000-000000000046")>
    Private Interface IContextMenu
        <PreserveSig> Function QueryContextMenu(hmenu As IntPtr, indexMenu As UInteger, idCmdFirst As UInteger, idCmdLast As UInteger, uFlags As UInteger) As Integer
        <PreserveSig> Function InvokeCommand(ByRef pici As IntPtr) As Integer
        <PreserveSig> Function GetCommandString(idCmd As UIntPtr, uType As UInteger, pReserved As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> pszName As StringBuilder, cchMax As UInteger) As Integer
    End Interface

    ' --- P/Invoke Declarations ---
    <DllImport("shell32.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function SHParseDisplayName(pszName As String, pbc As IntPtr, ByRef ppidl As IntPtr, sfgaoIn As UInteger, ByRef psfgaoOut As UInteger) As Integer
    End Function

    <DllImport("shell32.dll")>
    Private Shared Function SHBindToObject(psf As IShellFolder, pidl As IntPtr, pbc As IntPtr, ByRef riid As Guid, <MarshalAs(UnmanagedType.Interface)> ByRef ppv As Object) As Integer
    End Function

    <DllImport("shell32.dll")>
    Private Shared Sub ILFree(pidl As IntPtr)
    End Sub

    <DllImport("user32.dll")>
    Private Shared Function CreatePopupMenu() As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Shared Function DestroyMenu(hMenu As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll")>
    Private Shared Function GetMenuItemCount(hMenu As IntPtr) As Integer
    End Function

    <DllImport("user32.dll")>
    Private Shared Function GetMenuItemID(hMenu As IntPtr, nPos As Integer) As UInteger
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function GetMenuString(hMenu As IntPtr, uIDItem As UInteger, lpString As StringBuilder, nMaxCount As Integer, uFlag As UInteger) As Integer
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function FindWindowW(lpClassName As String, lpWindowName As String) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function FindWindowExW(hwndParent As IntPtr, hwndChildAfter As IntPtr, lpszClass As String, lpszWindow As String) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Shared Function PostMessageW(hWnd As IntPtr, Msg As UInteger, wParam As IntPtr, lParam As IntPtr) As Boolean
    End Function

    ''' <summary>
    ''' Inspects the File Explorer background menu for Undo/Redo availability and localized labels,
    ''' and optionally executes "undo" or "redo" on the active Windows Explorer shell view.
    ''' </summary>
    Public Shared Function InspectExplorerMenu(Optional ByRef UndoVisible As Boolean = False, Optional ByRef RedoVisible As Boolean = False, Optional ByRef UndoName As String = Nothing, Optional ByRef RedoName As String = Nothing, Optional execute As String = Nothing) As Boolean
        UndoVisible = False
        RedoVisible = False
        UndoName = Nothing
        RedoName = Nothing

        Dim pidlFolder As IntPtr = IntPtr.Zero
        Dim folderObj As Object = Nothing
        Dim shellFolder As IShellFolder = Nothing
        Dim viewObj As Object = Nothing
        Dim shellView As IShellView = Nothing
        Dim menuObj As Object = Nothing
        Dim contextMenu As IContextMenu = Nothing
        Dim hMenu As IntPtr = IntPtr.Zero

        Try
            ' 1. Query background context menu for Undo/Redo visibility and localized names
            Dim rootPath As String = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 3)
            Dim sfgaoOut As UInteger = 0
            If SHParseDisplayName(rootPath, IntPtr.Zero, pidlFolder, 0, sfgaoOut) <> 0 OrElse pidlFolder = IntPtr.Zero Then
                Return False
            End If

            Dim iidShellFolder As New Guid("000214E6-0000-0000-C000-000000000046")
            If SHBindToObject(Nothing, pidlFolder, IntPtr.Zero, iidShellFolder, folderObj) <> 0 Then Return False
            shellFolder = TryCast(folderObj, IShellFolder)
            If shellFolder Is Nothing Then Return False

            Dim iidShellView As New Guid("000214E3-0000-0000-C000-000000000046")
            shellFolder.CreateViewObject(IntPtr.Zero, iidShellView, viewObj)
            shellView = TryCast(viewObj, IShellView)
            If shellView Is Nothing Then Return False

            Dim iidContextMenu As New Guid("000214e4-0000-0000-c000-000000000046")
            If shellView.GetItemObject(SVGIO_BACKGROUND, iidContextMenu, menuObj) <> 0 Then Return False
            contextMenu = TryCast(menuObj, IContextMenu)
            If contextMenu Is Nothing Then Return False

            hMenu = CreatePopupMenu()
            If hMenu = IntPtr.Zero Then Return False

            If contextMenu.QueryContextMenu(hMenu, 0, ID_CMD_FIRST, ID_CMD_LAST, CMF_NORMAL) < 0 Then Return False

            Dim itemCount As Integer = GetMenuItemCount(hMenu)
            For i As Integer = 0 To itemCount - 1
                Dim cmdId As UInteger = GetMenuItemID(hMenu, i)
                If cmdId >= ID_CMD_FIRST AndAlso cmdId <= ID_CMD_LAST Then
                    Dim offset As UInteger = cmdId - ID_CMD_FIRST
                    Dim sbVerb As New StringBuilder(256)

                    If contextMenu.GetCommandString(New UIntPtr(offset), GCS_VERBW, IntPtr.Zero, sbVerb, CUInt(sbVerb.Capacity)) = 0 Then
                        Dim verb As String = sbVerb.ToString()
                        If String.Equals(verb, "undo", StringComparison.OrdinalIgnoreCase) Then
                            UndoVisible = True
                            UndoName = ReadCleanMenuLabel(hMenu, CUInt(i))
                        ElseIf String.Equals(verb, "redo", StringComparison.OrdinalIgnoreCase) Then
                            RedoVisible = True
                            RedoName = ReadCleanMenuLabel(hMenu, CUInt(i))
                        End If
                    End If
                End If
            Next

            ' 2. Execute Undo or Redo by posting WM_COMMAND to Explorer's SHELLDLL_DefView
            If Not String.IsNullOrEmpty(execute) Then
                Dim cmdToSend As Integer = 0
                If String.Equals(execute, "undo", StringComparison.OrdinalIgnoreCase) AndAlso UndoVisible Then
                    cmdToSend = FCIDM_SHVIEW_UNDO
                ElseIf String.Equals(execute, "redo", StringComparison.OrdinalIgnoreCase) AndAlso RedoVisible Then
                    cmdToSend = FCIDM_SHVIEW_REDO
                Else
                    Return False
                End If
                Dim hwndDefView As IntPtr = FindExplorerDefView()
                If hwndDefView = IntPtr.Zero Then Return False
                Return PostMessageW(hwndDefView, WM_COMMAND, New IntPtr(cmdToSend), IntPtr.Zero)
            End If
            Return True
        Finally
            If hMenu <> IntPtr.Zero Then DestroyMenu(hMenu)
            If contextMenu IsNot Nothing Then Marshal.ReleaseComObject(contextMenu)
            If shellView IsNot Nothing Then Marshal.ReleaseComObject(shellView)
            If shellFolder IsNot Nothing Then Marshal.ReleaseComObject(shellFolder)
            If pidlFolder <> IntPtr.Zero Then ILFree(pidlFolder)
        End Try
    End Function
    ''' 
    ''' Locates an active SHELLDLL_DefView window hosted by Windows Explorer (Desktop or open Explorer folder).
    ''' 
    Private Shared Function FindExplorerDefView() As IntPtr
        ' 1. Check Desktop's Progman -> SHELLDLL_DefView (always exists as long as explorer.exe is running)
        Dim hwndProgman As IntPtr = FindWindowW("Progman", Nothing)
        If hwndProgman <> IntPtr.Zero Then
            Dim hwndDefView As IntPtr = FindWindowExW(hwndProgman, IntPtr.Zero, "SHELLDLL_DefView", Nothing)
            If hwndDefView <> IntPtr.Zero Then Return hwndDefView
        End If
        ' 2. If Desktop wallpaper slideshow/WorkerW is active, SHELLDLL_DefView sits inside a WorkerW window
        Dim hwndWorker As IntPtr = IntPtr.Zero
        Do
            hwndWorker = FindWindowExW(IntPtr.Zero, hwndWorker, "WorkerW", Nothing)
            If hwndWorker <> IntPtr.Zero Then
                Dim hwndDefView As IntPtr = FindWindowExW(hwndWorker, IntPtr.Zero, "SHELLDLL_DefView", Nothing)
                If hwndDefView <> IntPtr.Zero Then Return hwndDefView
            End If
        Loop While hwndWorker <> IntPtr.Zero
        Return IntPtr.Zero
    End Function
    Private Shared Function ReadCleanMenuLabel(hMenu As IntPtr, position As UInteger) As String
        Dim sbLabel As New StringBuilder(256)
        If GetMenuString(hMenu, position, sbLabel, sbLabel.Capacity, MF_BYPOSITION) > 0 Then
            Dim rawText As String = sbLabel.ToString()
            Dim tabIndex As Integer = rawText.IndexOf(vbTab)
            If tabIndex >= 0 Then
                rawText = rawText.Substring(0, tabIndex)
            End If
            Return rawText.Replace("&", "").Trim()
        End If
        Return Nothing
    End Function
End Class