Imports System.Runtime.InteropServices
Imports System.Text


Public Class ShellLinkInfo

    ' COM interface declarations
    <ComImport(), Guid("000214F9-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IShellLinkW
        Function GetPath(<Out(), MarshalAs(UnmanagedType.LPWStr)> pszFile As StringBuilder,
                     cchMaxPath As Integer,
                     <Out()> ByRef pfd As WIN32_FIND_DATAW,
                     fFlags As UInteger) As Integer
        Function GetIDList(<Out()> ByRef ppidl As IntPtr) As Integer
        Function SetIDList(pidl As IntPtr) As Integer
        Function GetDescription(<Out(), MarshalAs(UnmanagedType.LPWStr)> pszName As StringBuilder,
                            cchMaxName As Integer) As Integer
        Function SetDescription(<MarshalAs(UnmanagedType.LPWStr)> pszName As String) As Integer
        Function GetWorkingDirectory(<Out(), MarshalAs(UnmanagedType.LPWStr)> pszDir As StringBuilder,
                                 cchMaxPath As Integer) As Integer
        Function SetWorkingDirectory(<MarshalAs(UnmanagedType.LPWStr)> pszDir As String) As Integer
        Function GetArguments(<Out(), MarshalAs(UnmanagedType.LPWStr)> pszArgs As StringBuilder,
                          cchMaxPath As Integer) As Integer
        Function SetArguments(<MarshalAs(UnmanagedType.LPWStr)> pszArgs As String) As Integer
        Function GetHotkey(<Out()> ByRef pwHotkey As Short) As Integer
        Function SetHotkey(wHotkey As Short) As Integer
        Function GetShowCmd(<Out()> ByRef piShowCmd As Integer) As Integer
        Function SetShowCmd(iShowCmd As Integer) As Integer
        Function GetIconLocation(<Out(), MarshalAs(UnmanagedType.LPWStr)> pszIconPath As StringBuilder,
                             cchIconPath As Integer,
                             <Out()> ByRef piIcon As Integer) As Integer
        Function SetIconLocation(<MarshalAs(UnmanagedType.LPWStr)> pszIconPath As String,
                             iIcon As Integer) As Integer
        Function SetRelativePath(<MarshalAs(UnmanagedType.LPWStr)> pszPathRel As String,
                             dwReserved As UInteger) As Integer
        Function Resolve(hwnd As IntPtr, fFlags As UInteger) As Integer
        Function SetPath(<MarshalAs(UnmanagedType.LPWStr)> pszFile As String) As Integer
    End Interface

    <ComImport(), Guid("0000010b-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IPersistFile
        Function GetClassID(ByRef pClassID As Guid) As Integer
        Function IsDirty() As Integer
        Function Load(<MarshalAs(UnmanagedType.LPWStr)> pszFileName As String, dwMode As UInteger) As Integer
        Function Save(<MarshalAs(UnmanagedType.LPWStr)> pszFileName As String, fRemember As Boolean) As Integer
        Function SaveCompleted(<MarshalAs(UnmanagedType.LPWStr)> pszFileName As String) As Integer
        Function GetCurFile(<MarshalAs(UnmanagedType.LPWStr)> ByRef ppszFileName As String) As Integer
    End Interface

    <ComImport(), Guid("00021401-0000-0000-C000-000000000046")>
    Private Class CShellLink
    End Class

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Private Structure WIN32_FIND_DATAW
        Public dwFileAttributes As UInteger
        Public ftCreationTime As Long
        Public ftLastAccessTime As Long
        Public ftLastWriteTime As Long
        Public nFileSizeHigh As UInteger
        Public nFileSizeLow As UInteger
        Public dwReserved0 As UInteger
        Public dwReserved1 As UInteger
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)>
        Public cFileName As String
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=14)>
        Public cAlternateFileName As String
        Public dwFileType As UInteger ' Obsolete. Do Not use.
        Public dwCreatorType As UInteger ' Obsolete. Do Not use
        Public wFinderFlags As UInteger ' Obsolete. Do Not use
    End Structure


    Private Const FILE_ATTRIBUTE_DIRECTORY As UInteger = &H10

    Public Property TargetPath As String = ""
    Public Property PointsToDir As Boolean = False
    Public Property TargetExists As Boolean = False
    Public Property [Error] As Boolean = False

    Public Sub New(ByVal lnkFile As String)
        LoadLink(lnkFile)
    End Sub

    Private Sub LoadLink(ByVal lnkFile As String)
        Dim shellLink As IShellLinkW = Nothing
        Try
            shellLink = CType(New CShellLink(), IShellLinkW)
            CType(shellLink, IPersistFile).Load(lnkFile, 0)

            Dim sb As New StringBuilder(260)
            Dim wfd As New WIN32_FIND_DATAW
            shellLink.GetPath(sb, sb.Capacity, wfd, 0)

            TargetPath = sb.ToString()

            ' determine if it pointed to a directory
            PointsToDir = (wfd.dwFileAttributes And FILE_ATTRIBUTE_DIRECTORY) <> 0

            ' check if the target currently exists
            TargetExists = IO.File.Exists(TargetPath) OrElse IO.Directory.Exists(TargetPath)

        Catch ex As Exception
            Debug.Print("ShellLinkInfo.LoadLink error: " & ex.Message)
            TargetPath = ""
            PointsToDir = False
            [Error] = True
        Finally
            If shellLink IsNot Nothing Then Marshal.ReleaseComObject(shellLink)
        End Try
    End Sub

End Class
