Imports System.Runtime.InteropServices
Imports System.Text

Public NotInheritable Class ShellLinkInfo
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

    Public Property LinkFile As String = ""
    Public Property TargetPath As String = ""
    Public Property WorkingDirectory As String = ""
    Public Property Description As String = ""
    Public Property Arguments As String = ""
    Public Property IconPath As String = ""
    Public Property IconIndex As Integer = 0

    Public Property ShowCmd As Integer = 1  ' 1 = normal window
    Public Property Hotkey As Short = 0

    Public Property PointsToDir As Boolean = False

    Public Sub New()

    End Sub

    Public Sub New(ByVal lnkFile As String)
        Load(lnkFile)
    End Sub

    Public Function Load(ByVal lnkFile As String) As Boolean
        Me.LinkFile = lnkFile
        Dim shellLink As IShellLinkW = Nothing
        Dim persFile As IPersistFile = Nothing
        Try

            shellLink = CType(New CShellLink(), IShellLinkW)
            persFile = CType(shellLink, IPersistFile)
            persFile.Load(lnkFile, 0)

            Dim sb As New StringBuilder(260)
            Dim wfd As New WIN32_FIND_DATAW
            shellLink.GetPath(sb, sb.Capacity, wfd, 0)
            Me.TargetPath = sb.ToString().Trim()

            sb.Clear() : sb.EnsureCapacity(260)
            shellLink.GetDescription(sb, sb.Capacity)
            Me.Description = sb.ToString().Trim()

            sb.Clear() : sb.EnsureCapacity(260)
            shellLink.GetWorkingDirectory(sb, sb.Capacity)
            Me.WorkingDirectory = sb.ToString().Trim()

            sb.Clear() : sb.EnsureCapacity(260)
            shellLink.GetArguments(sb, sb.Capacity)
            Me.Arguments = sb.ToString().Trim()

            sb.Clear() : sb.EnsureCapacity(260)
            Dim iconIndex As Integer
            shellLink.GetIconLocation(sb, sb.Capacity, iconIndex)
            IconPath = sb.ToString().Trim()
            Me.IconIndex = iconIndex

            Dim show As Integer
            shellLink.GetShowCmd(show)
            Me.ShowCmd = show

            Dim hotkey As Short
            shellLink.GetHotkey(hotkey)
            Me.Hotkey = hotkey

            ' determine if it pointed to a directory
            Me.PointsToDir = wfd.dwFileAttributes.HasFlag(IO.FileAttributes.Directory)

            ' check if the target currently exists
            ' this stalls on networked paths that aren't live
            ' done: move existence check into deferrediconloading task
            'Me.TargetExists = IO.File.Exists(TargetPath) OrElse IO.Directory.Exists(TargetPath) 

            Return True

        Catch ex As Exception
            Debug.Print("ShellLinkInfo.LoadLink error: " & ex.Message)
            Me.TargetPath = ""
            Me.PointsToDir = False

            Return False
        Finally
            If persFile IsNot Nothing Then Marshal.ReleaseComObject(persFile)
            If shellLink IsNot Nothing Then Marshal.ReleaseComObject(shellLink)
        End Try
    End Function

    Public Function Resolve(flags As UInteger) As Integer
        Dim shellLink As IShellLinkW
        shellLink = CType(New CShellLink(), IShellLinkW)
        CType(shellLink, IPersistFile).Load(Me.LinkFile, 0)
        Try
            Return shellLink.Resolve(ScalaHandle, flags)
        Finally
            If (flags And &H4UI) <> 0 AndAlso IO.File.Exists(Me.LinkFile) Then
                CType(shellLink, IPersistFile).Save(Me.LinkFile, True)
            End If
        End Try
    End Function

    Public Function Save(path As String) As Boolean
        Dim shellLink As IShellLinkW = Nothing
        Dim persistFile As IPersistFile = Nothing
        Try
            shellLink = CType(New CShellLink(), IShellLinkW)
            persistFile = CType(shellLink, IPersistFile)

            shellLink.SetPath(Me.TargetPath)
            shellLink.SetWorkingDirectory(Me.WorkingDirectory)
            shellLink.SetDescription(Me.Description)
            shellLink.SetArguments(Me.Arguments)
            shellLink.SetIconLocation(Me.IconPath, Me.IconIndex)

            shellLink.SetShowCmd(Me.ShowCmd)
            shellLink.SetHotkey(Me.Hotkey)

            persistFile.Save(path, True)
            persistFile.SaveCompleted(path)
            Return True

        Catch ex As Exception
            Debug.Print("ShellLinkInfo.SaveLink error: " & ex.Message)

            Return False
        Finally
            If persistFile IsNot Nothing Then Marshal.ReleaseComObject(persistFile)
            If shellLink IsNot Nothing Then Marshal.ReleaseComObject(shellLink)
        End Try

    End Function

End Class
