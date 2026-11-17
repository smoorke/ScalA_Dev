Imports System.Runtime.InteropServices

Public Class ShellFileOperations

    ' --- Win32 / Shell Constants ---
    Private Const FOF_NOCONFIRMATION As UInteger = &H10
    Private Const FOF_NOERRORUI As UInteger = &H400
    Private Const FOF_SILENT As UInteger = &H4
    Private Const FOFX_ADDUNDORECORD As UInteger = &H20000000 ' Registers the operation in Explorer's global Undo stack

    ' --- COM Interfaces ---
    <ComImport, Guid("947AAB5F-0A5C-4C13-B4D6-4BF7836FC9F8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IFileOperation
        Function Advise(pfops As IntPtr, ByRef pdwCookie As UInteger) As Integer
        Function Unadvise(dwCookie As UInteger) As Integer
        Function SetOperationFlags(dwOperationFlags As UInteger) As Integer
        Function SetProgressMessage(<MarshalAs(UnmanagedType.LPWStr)> pszMessage As String) As Integer
        Function SetProgressDialog(popd As IntPtr) As Integer
        Function SetProperties(pproparray As IntPtr) As Integer
        Function SetOwnerWindow(hwndParent As IntPtr) As Integer
        Function ApplyPropertiesToItem(psiItem As IShellItem) As Integer
        Function ApplyPropertiesToItems(punkItems As IntPtr) As Integer
        Function RenameItem(psiItem As IShellItem, <MarshalAs(UnmanagedType.LPWStr)> pszNewName As String, pfopsItem As IntPtr) As Integer
        Function RenameItems(pUnkItems As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> pszNewName As String) As Integer
        Function MoveItem(psiItem As IShellItem, psiDestinationFolder As IShellItem, <MarshalAs(UnmanagedType.LPWStr)> pszNewName As String, pfopsItem As IntPtr) As Integer
        Function MoveItems(punkItems As IntPtr, psiDestinationFolder As IShellItem) As Integer
        Function CopyItem(psiItem As IShellItem, psiDestinationFolder As IShellItem, <MarshalAs(UnmanagedType.LPWStr)> pszNewName As String, pfopsItem As IntPtr) As Integer
        Function CopyItems(punkItems As IntPtr, psiDestinationFolder As IShellItem) As Integer
        Function DeleteItem(psiItem As IShellItem, pfopsItem As IntPtr) As Integer
        Function DeleteItems(punkItems As IntPtr) As Integer
        Function NewItem(psiDestinationFolder As IShellItem, dwFileAttributes As UInteger, <MarshalAs(UnmanagedType.LPWStr)> pszName As String, <MarshalAs(UnmanagedType.LPWStr)> pszTemplateName As String, pfopsItem As IntPtr) As Integer
        Function PerformOperations() As Integer
        Function GetAnyOperationsAborted(<MarshalAs(UnmanagedType.Bool)> ByRef pfAnyOperationsAborted As Boolean) As Integer
    End Interface

    <ComImport, Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IShellItem
        Function BindToHandler(pbc As IntPtr, ByRef bhid As Guid, ByRef riid As Guid, ByRef ppv As IntPtr) As Integer
        Function GetParent(ByRef ppsi As IShellItem) As Integer
        Function GetDisplayName(sigdnName As UInteger, ByRef ppszName As IntPtr) As Integer
        Function GetAttributes(sfgaoMask As UInteger, ByRef psfgaoAttribs As UInteger) As Integer
        Function Compare(psi As IShellItem, hint As UInteger, ByRef piOrder As Integer) As Integer
    End Interface

    ' --- P/Invoke Declarations ---
    <DllImport("shell32.dll", CharSet:=CharSet.Unicode, PreserveSig:=False)>
    Private Shared Sub SHCreateItemFromParsingName(
        <MarshalAs(UnmanagedType.LPWStr)> pszPath As String,
        pbc As IntPtr,
        ByRef riid As Guid,
        <MarshalAs(UnmanagedType.Interface)> ByRef ppv As IShellItem)
    End Sub

    ''' <summary>
    ''' Renames a file or folder via the Windows Shell so the operation is recorded in File Explorer's Undo stack.
    ''' </summary>
    ''' <param name="fullPath">Full path to the existing file or folder (e.g. "C:\Temp\old.txt").</param>
    ''' <param name="newFileName">The new file or folder name only, without directory path (e.g. "new.txt").</param>
    ''' <param name="silent">If True, suppresses Shell progress and confirmation dialogs.</param>
    ''' <returns>True if the rename succeeded and was added to the Undo stack.</returns>
    Public Shared Function RenameWithUndo(fullPath As String, newFileName As String, Optional silent As Boolean = True) As Boolean
        If String.IsNullOrWhiteSpace(fullPath) OrElse String.IsNullOrWhiteSpace(newFileName) Then
            Return False
        End If

        newFileName = IO.Path.GetFileName(newFileName)

        Dim fileOpType As Type = Type.GetTypeFromCLSID(New Guid("3AD05575-8857-4850-9277-11B85BDB8E09"))
        Dim fileOp As IFileOperation = Nothing
        Dim shellItem As IShellItem = Nothing

        Try
            fileOp = DirectCast(Activator.CreateInstance(fileOpType), IFileOperation)
            fileOp.SetOperationFlags(BuildFlags(silent))

            Dim iidShellItem As New Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")
            SHCreateItemFromParsingName(fullPath, IntPtr.Zero, iidShellItem, shellItem)

            fileOp.RenameItem(shellItem, newFileName, IntPtr.Zero)
            fileOp.PerformOperations()

            Dim aborted As Boolean = False
            fileOp.GetAnyOperationsAborted(aborted)
            Return Not aborted

        Finally
            If shellItem IsNot Nothing Then Marshal.ReleaseComObject(shellItem)
            If fileOp IsNot Nothing Then Marshal.ReleaseComObject(fileOp)
        End Try
    End Function

    ''' <summary>
    ''' Creates a new folder via the Windows Shell so the creation appears in File Explorer's Undo stack ("Undo New Folder").
    ''' </summary>
    ''' <param name="parentFolderPath">Existing parent directory path (e.g. "C:\Docs").</param>
    ''' <param name="newFolderName">Name of the new folder to create (e.g. "Archive").</param>
    ''' <param name="silent">If True, suppresses Shell progress and confirmation dialogs.</param>
    ''' <returns>True if the folder was created and added to the Undo stack.</returns>
    Public Shared Function CreateFolderWithUndo(parentFolderPath As String, newFolderName As String, Optional silent As Boolean = True) As Boolean
        If String.IsNullOrWhiteSpace(parentFolderPath) OrElse String.IsNullOrWhiteSpace(newFolderName) Then
            Return False
        End If

        If Not IO.Directory.Exists(parentFolderPath) Then
            Return False
        End If

        newFolderName = IO.Path.GetFileName(newFolderName.TrimEnd(IO.Path.DirectorySeparatorChar, IO.Path.AltDirectorySeparatorChar))

        Dim fileOpType As Type = Type.GetTypeFromCLSID(New Guid("3AD05575-8857-4850-9277-11B85BDB8E09"))
        Dim fileOp As IFileOperation = Nothing
        Dim parentShellItem As IShellItem = Nothing

        Try
            fileOp = DirectCast(Activator.CreateInstance(fileOpType), IFileOperation)
            fileOp.SetOperationFlags(BuildFlags(silent))

            Dim iidShellItem As New Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")
            SHCreateItemFromParsingName(parentFolderPath, IntPtr.Zero, iidShellItem, parentShellItem)

            ' FILE_ATTRIBUTE_DIRECTORY (&H10) tells IFileOperation.NewItem to create a folder instead of an empty file
            fileOp.NewItem(parentShellItem, FILE_ATTRIBUTE_DIRECTORY, newFolderName, Nothing, IntPtr.Zero)
            fileOp.PerformOperations()

            Dim aborted As Boolean = False
            fileOp.GetAnyOperationsAborted(aborted)
            Return Not aborted

        Finally
            If parentShellItem IsNot Nothing Then Marshal.ReleaseComObject(parentShellItem)
            If fileOp IsNot Nothing Then Marshal.ReleaseComObject(fileOp)
        End Try
    End Function

    Private Shared Function BuildFlags(silent As Boolean) As UInteger
        Dim flags As UInteger = FOFX_ADDUNDORECORD
        If silent Then
            flags = flags Or FOF_SILENT Or FOF_NOCONFIRMATION Or FOF_NOERRORUI
        End If
        Return flags
    End Function

End Class