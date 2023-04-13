Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Public Class FolderPicker
    Private ReadOnly _resultPaths As List(Of String) = New List(Of String)()
    Private ReadOnly _resultNames As List(Of String) = New List(Of String)()

    Public ReadOnly Property ResultPaths As IReadOnlyList(Of String)
        Get
            Return _resultPaths
        End Get
    End Property

    Public ReadOnly Property ResultNames As IReadOnlyList(Of String)
        Get
            Return _resultNames
        End Get
    End Property

    Public ReadOnly Property ResultPath As String
        Get
            Return ResultPaths.FirstOrDefault()
        End Get
    End Property

    Public ReadOnly Property ResultName As String
        Get
            Return ResultNames.FirstOrDefault()
        End Get
    End Property

    Public Overridable Property InputPath As String
    Public Overridable Property ForceFileSystem As Boolean
    Public Overridable Property Multiselect As Boolean
    Public Overridable Property Title As String
    Public Overridable Property OkButtonLabel As String
    Public Overridable Property FileNameLabel As String

    Protected Overridable Function SetOptions(ByVal options As Integer) As Integer
        If ForceFileSystem Then
            options = options Or CInt(FOS.FOS_FORCEFILESYSTEM)
        End If

        If Multiselect Then
            options = options Or CInt(FOS.FOS_ALLOWMULTISELECT)
        End If

        Return options
    End Function

    Public Function ShowDialog(ByVal Optional owner As Form = Nothing, ByVal Optional throwOnError As Boolean = False) As Boolean?
        owner = If(owner, Application.OpenForms.Item(0))
        Return ShowDialog(If(owner IsNot Nothing, owner.Handle, IntPtr.Zero), throwOnError)
    End Function

    Public Overridable Function ShowDialog(ByVal owner As IntPtr, ByVal Optional throwOnError As Boolean = False) As Boolean?
        Dim dialog = CType(New FileOpenDialog(), IFileOpenDialog)
        Dim item As IShellItem = Nothing

        If Not String.IsNullOrEmpty(InputPath) Then
            If CheckHr(SHCreateItemFromParsingName(InputPath, Nothing, GetType(IShellItem).GUID, item), throwOnError) <> 0 Then Return Nothing
            dialog.SetFolder(item)
        End If

        Dim options = FOS.FOS_PICKFOLDERS
        options = CType(SetOptions(CInt(options)), FOS)
        dialog.SetOptions(options)

        If Title IsNot Nothing Then
            dialog.SetTitle(Title)
        End If

        If OkButtonLabel IsNot Nothing Then
            dialog.SetOkButtonLabel(OkButtonLabel)
        End If

        If FileNameLabel IsNot Nothing Then
            dialog.SetFileName(FileNameLabel)
        End If

        If owner = IntPtr.Zero Then
            owner = Process.GetCurrentProcess().MainWindowHandle

            If owner = IntPtr.Zero Then
                owner = GetDesktopWindow()
            End If
        End If

        Dim hr = dialog.Show(owner)
        If hr = ERROR_CANCELLED Then Return Nothing
        If CheckHr(hr, throwOnError) <> 0 Then Return Nothing
        Dim items As IShellItemArray = Nothing
        If CheckHr(dialog.GetResults(items), throwOnError) <> 0 Then Return Nothing
        Dim count As Integer = Nothing
        items.GetCount(count)
        item = Nothing
        Dim path As String = Nothing, name As String = Nothing

        For i = 0 To count - 1
            items.GetItemAt(i, item)
            CheckHr(item.GetDisplayName(SIGDN.SIGDN_DESKTOPABSOLUTEPARSING, path), throwOnError)
            CheckHr(item.GetDisplayName(SIGDN.SIGDN_DESKTOPABSOLUTEEDITING, name), throwOnError)

            If path IsNot Nothing OrElse name IsNot Nothing Then
                _resultPaths.Add(path)
                _resultNames.Add(name)
            End If
        Next

        Return True
    End Function

    Private Shared Function CheckHr(ByVal hr As Integer, ByVal throwOnError As Boolean) As Integer
        If hr <> 0 AndAlso throwOnError Then Marshal.ThrowExceptionForHR(hr)
        Return hr
    End Function

    <DllImport("shell32")>
    Private Shared Function SHCreateItemFromParsingName(
    <MarshalAs(UnmanagedType.LPWStr)> ByVal pszPath As String, ByVal pbc As IBindCtx,
    <MarshalAs(UnmanagedType.LPStruct)> ByVal riid As Guid, <Out> ByRef ppv As IShellItem) As Integer
    End Function
    <DllImport("user32")>
    Private Shared Function GetDesktopWindow() As IntPtr : End Function

    Private Const ERROR_CANCELLED As Integer = &H800704C7

    <ComImport, Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")>
    Private Class FileOpenDialog
    End Class

    <ComImport, Guid("d57c7288-d4ad-4768-be02-9d969532d960"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IFileOpenDialog
        <PreserveSig>
        Function Show(ByVal parent As IntPtr) As Integer
        <PreserveSig>
        Function SetFileTypes() As Integer
        <PreserveSig>
        Function SetFileTypeIndex(ByVal iFileType As Integer) As Integer
        <PreserveSig>
        Function GetFileTypeIndex(<Out> ByRef piFileType As Integer) As Integer
        <PreserveSig>
        Function Advise() As Integer
        <PreserveSig>
        Function Unadvise() As Integer
        <PreserveSig>
        Function SetOptions(ByVal fos As FOS) As Integer
        <PreserveSig>
        Function GetOptions(<Out> ByRef pfos As FOS) As Integer
        <PreserveSig>
        Function SetDefaultFolder(ByVal psi As IShellItem) As Integer
        <PreserveSig>
        Function SetFolder(ByVal psi As IShellItem) As Integer
        <PreserveSig>
        Function GetFolder(<Out> ByRef ppsi As IShellItem) As Integer
        <PreserveSig>
        Function GetCurrentSelection(<Out> ByRef ppsi As IShellItem) As Integer
        <PreserveSig>
        Function SetFileName(
        <MarshalAs(UnmanagedType.LPWStr)> ByVal pszName As String) As Integer
        <PreserveSig>
        Function GetFileName(<Out>
        <MarshalAs(UnmanagedType.LPWStr)> ByRef pszName As String) As Integer
        <PreserveSig>
        Function SetTitle(
        <MarshalAs(UnmanagedType.LPWStr)> ByVal pszTitle As String) As Integer
        <PreserveSig>
        Function SetOkButtonLabel(
        <MarshalAs(UnmanagedType.LPWStr)> ByVal pszText As String) As Integer
        <PreserveSig>
        Function SetFileNameLabel(
        <MarshalAs(UnmanagedType.LPWStr)> ByVal pszLabel As String) As Integer
        <PreserveSig>
        Function GetResult(<Out> ByRef ppsi As IShellItem) As Integer
        <PreserveSig>
        Function AddPlace(ByVal psi As IShellItem, ByVal alignment As Integer) As Integer
        <PreserveSig>
        Function SetDefaultExtension(
        <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDefaultExtension As String) As Integer
        <PreserveSig>
        Function Close(ByVal hr As Integer) As Integer
        <PreserveSig>
        Function SetClientGuid() As Integer
        <PreserveSig>
        Function ClearClientData() As Integer
        <PreserveSig>
        Function SetFilter(
        <MarshalAs(UnmanagedType.IUnknown)> ByVal pFilter As Object) As Integer
        <PreserveSig>
        Function GetResults(<Out> ByRef ppenum As IShellItemArray) As Integer
        <PreserveSig>
        Function GetSelectedItems(<Out>
        <MarshalAs(UnmanagedType.IUnknown)> ByRef ppsai As Object) As Integer
    End Interface

    <ComImport, Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IShellItem
        <PreserveSig>
        Function BindToHandler() As Integer
        <PreserveSig>
        Function GetParent() As Integer
        <PreserveSig>
        Function GetDisplayName(ByVal sigdnName As SIGDN, <Out>
        <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszName As String) As Integer
        <PreserveSig>
        Function GetAttributes() As Integer
        <PreserveSig>
        Function Compare() As Integer
    End Interface

    <ComImport, Guid("b63ea76d-1f85-456f-a19c-48159efa858b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IShellItemArray
        <PreserveSig>
        Function BindToHandler() As Integer
        <PreserveSig>
        Function GetPropertyStore() As Integer
        <PreserveSig>
        Function GetPropertyDescriptionList() As Integer
        <PreserveSig>
        Function GetAttributes() As Integer
        <PreserveSig>
        Function GetCount(<Out> ByRef pdwNumItems As Integer) As Integer
        <PreserveSig>
        Function GetItemAt(ByVal dwIndex As Integer, <Out> ByRef ppsi As IShellItem) As Integer
        <PreserveSig>
        Function EnumItems() As Integer
    End Interface

    Private Enum SIGDN
        SIGDN_DESKTOPABSOLUTEEDITING = &H8004C000
        SIGDN_DESKTOPABSOLUTEPARSING = &H80028000
        SIGDN_FILESYSPATH = &H80058000
        SIGDN_NORMALDISPLAY = 0
        SIGDN_PARENTRELATIVE = &H80080001
        SIGDN_PARENTRELATIVEEDITING = &H80031001
        SIGDN_PARENTRELATIVEFORADDRESSBAR = &H8007C001
        SIGDN_PARENTRELATIVEPARSING = &H80018001
        SIGDN_URL = &H80068000
    End Enum

    <Flags>
    Private Enum FOS
        FOS_OVERWRITEPROMPT = &H2
        FOS_STRICTFILETYPES = &H4
        FOS_NOCHANGEDIR = &H8
        FOS_PICKFOLDERS = &H20
        FOS_FORCEFILESYSTEM = &H40
        FOS_ALLNONSTORAGEITEMS = &H80
        FOS_NOVALIDATE = &H100
        FOS_ALLOWMULTISELECT = &H200
        FOS_PATHMUSTEXIST = &H800
        FOS_FILEMUSTEXIST = &H1000
        FOS_CREATEPROMPT = &H2000
        FOS_SHAREAWARE = &H4000
        FOS_NOREADONLYRETURN = &H8000
        FOS_NOTESTFILECREATE = &H10000
        FOS_HIDEMRUPLACES = &H20000
        FOS_HIDEPINNEDPLACES = &H40000
        FOS_NODEREFERENCELINKS = &H100000
        FOS_OKBUTTONNEEDSINTERACTION = &H200000
        FOS_DONTADDTORECENT = &H2000000
        FOS_FORCESHOWHIDDEN = &H10000000
        FOS_DEFAULTNOMINIMODE = &H20000000
        FOS_FORCEPREVIEWPANEON = &H40000000
        FOS_SUPPORTSTREAMABLEITEMS = &H80000000
    End Enum
End Class
