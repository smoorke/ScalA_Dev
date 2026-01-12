Imports System.ComponentModel

Namespace QL

    ''' <summary>
    ''' Base event args for QuickLaunch operations
    ''' </summary>
    Public Class QLEventArgs
        Inherits EventArgs

        ''' <summary>
        ''' The file or folder path involved in the operation
        ''' </summary>
        Public Property Path As String

        ''' <summary>
        ''' Whether the operation completed successfully
        ''' </summary>
        Public Property Success As Boolean = True

        Public Sub New()
        End Sub

        Public Sub New(path As String)
            Me.Path = path
        End Sub
    End Class

    ''' <summary>
    ''' Event args when a QuickLaunch item is opened/launched
    ''' </summary>
    Public Class QLItemOpenedEventArgs
        Inherits QLEventArgs

        ''' <summary>
        ''' For shortcuts, the resolved target path
        ''' </summary>
        Public Property Target As String

        ''' <summary>
        ''' Whether the opened item was a .lnk shortcut
        ''' </summary>
        Public Property WasShortcut As Boolean

        ''' <summary>
        ''' Whether this was a folder (opened in explorer)
        ''' </summary>
        Public Property IsFolder As Boolean

        Public Sub New()
        End Sub

        Public Sub New(path As String, Optional target As String = Nothing, Optional wasShortcut As Boolean = False, Optional isFolder As Boolean = False)
            MyBase.New(path)
            Me.Target = target
            Me.WasShortcut = wasShortcut
            Me.IsFolder = isFolder
        End Sub
    End Class

    ''' <summary>
    ''' Event args when a paste operation completes
    ''' </summary>
    Public Class QLPasteCompleteEventArgs
        Inherits QLEventArgs

        ''' <summary>
        ''' The folder where items were pasted
        ''' </summary>
        Public Property TargetPath As String

        ''' <summary>
        ''' The source paths that were pasted
        ''' </summary>
        Public Property SourcePaths As List(Of String)

        ''' <summary>
        ''' Whether this was a move (cut) or copy operation
        ''' </summary>
        Public Property WasCut As Boolean

        Public Sub New()
        End Sub

        Public Sub New(targetPath As String, sourcePaths As List(Of String), Optional wasCut As Boolean = False)
            MyBase.New(targetPath)
            Me.TargetPath = targetPath
            Me.SourcePaths = sourcePaths
            Me.WasCut = wasCut
        End Sub
    End Class

    ''' <summary>
    ''' Event args when a rename is requested
    ''' </summary>
    Public Class QLRenameEventArgs
        Inherits QLEventArgs

        ''' <summary>
        ''' The current name of the item
        ''' </summary>
        Public Property CurrentName As String

        ''' <summary>
        ''' The new name (set after rename completes)
        ''' </summary>
        Public Property NewName As String

        ''' <summary>
        ''' Whether this is a folder
        ''' </summary>
        Public Property IsFolder As Boolean

        Public Sub New()
        End Sub

        Public Sub New(path As String, currentName As String, Optional isFolder As Boolean = False)
            MyBase.New(path)
            Me.CurrentName = currentName
            Me.IsFolder = isFolder
        End Sub
    End Class

    ''' <summary>
    ''' Event args when a delete is requested
    ''' </summary>
    Public Class QLDeleteEventArgs
        Inherits QLEventArgs

        ''' <summary>
        ''' Whether the item is a folder
        ''' </summary>
        Public Property IsFolder As Boolean

        ''' <summary>
        ''' Whether to permanently delete (Shift held) vs recycle
        ''' </summary>
        Public Property PermanentDelete As Boolean

        ''' <summary>
        ''' Number of files in folder (if IsFolder)
        ''' </summary>
        Public Property FileCount As Integer

        ''' <summary>
        ''' Number of subfolders (if IsFolder)
        ''' </summary>
        Public Property FolderCount As Integer

        Public Sub New()
        End Sub

        Public Sub New(path As String, isFolder As Boolean, Optional permanentDelete As Boolean = False)
            MyBase.New(path)
            Me.IsFolder = isFolder
            Me.PermanentDelete = permanentDelete
        End Sub
    End Class

    ''' <summary>
    ''' Event args when a shortcut is created
    ''' </summary>
    Public Class QLShortcutCreatedEventArgs
        Inherits QLEventArgs

        ''' <summary>
        ''' The path to the created .lnk file
        ''' </summary>
        Public Property LinkPath As String

        ''' <summary>
        ''' The target the shortcut points to
        ''' </summary>
        Public Property TargetPath As String

        Public Sub New()
        End Sub

        Public Sub New(linkPath As String, targetPath As String)
            MyBase.New(linkPath)
            Me.LinkPath = linkPath
            Me.TargetPath = targetPath
        End Sub
    End Class

    ''' <summary>
    ''' Event args when a new folder is created
    ''' </summary>
    Public Class QLFolderCreatedEventArgs
        Inherits QLEventArgs

        ''' <summary>
        ''' The name of the new folder
        ''' </summary>
        Public Property FolderName As String

        Public Sub New()
        End Sub

        Public Sub New(path As String, folderName As String)
            MyBase.New(path)
            Me.FolderName = folderName
        End Sub
    End Class

    ''' <summary>
    ''' Event args for drag-drop reordering
    ''' </summary>
    Public Class QLReorderEventArgs
        Inherits QLEventArgs

        ''' <summary>
        ''' The folder containing the reordered items
        ''' </summary>
        Public Property FolderPath As String

        ''' <summary>
        ''' The item that was moved
        ''' </summary>
        Public Property MovedItemPath As String

        ''' <summary>
        ''' The new index position
        ''' </summary>
        Public Property NewIndex As Integer

        Public Sub New()
        End Sub

        Public Sub New(folderPath As String, movedItemPath As String, newIndex As Integer)
            MyBase.New(folderPath)
            Me.FolderPath = folderPath
            Me.MovedItemPath = movedItemPath
            Me.NewIndex = newIndex
        End Sub
    End Class

    ''' <summary>
    ''' Event args for errors
    ''' </summary>
    Public Class QLErrorEventArgs
        Inherits QLEventArgs

        ''' <summary>
        ''' Error message to display
        ''' </summary>
        Public Property Message As String

        ''' <summary>
        ''' The exception that occurred (if any)
        ''' </summary>
        Public Property Exception As Exception

        ''' <summary>
        ''' The operation that was being performed
        ''' </summary>
        Public Property Operation As String

        Public Sub New()
        End Sub

        Public Sub New(message As String, Optional ex As Exception = Nothing, Optional operation As String = Nothing)
            Me.Message = message
            Me.Exception = ex
            Me.Operation = operation
            Me.Success = False
        End Sub
    End Class

    ''' <summary>
    ''' Cancelable event args for operations that can be prevented
    ''' </summary>
    Public Class QLCancelEventArgs
        Inherits CancelEventArgs

        ''' <summary>
        ''' The file or folder path involved
        ''' </summary>
        Public Property Path As String

        Public Sub New()
        End Sub

        Public Sub New(path As String)
            Me.Path = path
        End Sub
    End Class

End Namespace ' QL
