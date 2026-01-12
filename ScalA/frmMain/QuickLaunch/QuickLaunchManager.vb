Imports System.ComponentModel

Namespace QL

    ''' <summary>
    ''' Manager class that coordinates QuickLaunch operations and raises events
    ''' for frmMain to handle UI updates.
    ''' </summary>
    Public Class QuickLaunchManager
        Implements IDisposable

#Region "Events"

        ''' <summary>
        ''' Raised when an item is opened/launched
        ''' </summary>
        Public Event ItemOpened As EventHandler(Of QLItemOpenedEventArgs)

        ''' <summary>
        ''' Raised when a paste operation completes
        ''' </summary>
        Public Event PasteComplete As EventHandler(Of QLPasteCompleteEventArgs)

        ''' <summary>
        ''' Raised when a rename is requested
        ''' </summary>
        Public Event RenameRequested As EventHandler(Of QLRenameEventArgs)

        ''' <summary>
        ''' Raised when a delete is requested
        ''' </summary>
        Public Event DeleteRequested As EventHandler(Of QLDeleteEventArgs)

        ''' <summary>
        ''' Raised when a shortcut is created
        ''' </summary>
        Public Event ShortcutCreated As EventHandler(Of QLShortcutCreatedEventArgs)

        ''' <summary>
        ''' Raised when a new folder is created
        ''' </summary>
        Public Event FolderCreated As EventHandler(Of QLFolderCreatedEventArgs)

        ''' <summary>
        ''' Raised when items are reordered via drag-drop
        ''' </summary>
        Public Event ItemsReordered As EventHandler(Of QLReorderEventArgs)

        ''' <summary>
        ''' Raised when an error occurs
        ''' </summary>
        Public Event ErrorOccurred As EventHandler(Of QLErrorEventArgs)

#End Region

#Region "Properties"

        ''' <summary>
        ''' The root folder path for QuickLaunch
        ''' </summary>
        Public Property RootPath As String

        ''' <summary>
        ''' Whether Ctrl+Shift is currently pressed (show hidden items)
        ''' </summary>
        Public Property CtrlShiftPressed As Boolean

        ''' <summary>
        ''' Whether a paste operation is in progress
        ''' </summary>
        Public Property IsPasting As Boolean

        ''' <summary>
        ''' Target path for current paste operation
        ''' </summary>
        Public Property PasteTargetPath As String

#End Region

#Region "Constructor"

        Public Sub New()
        End Sub

        Public Sub New(rootPath As String)
            Me.RootPath = rootPath
        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Opens/launches a QuickLaunch item
        ''' </summary>
        ''' <param name="qli">The item to open</param>
        Public Sub OpenItem(qli As QLInfo)
            Dim launchInfo = QLItemHandler.PrepareLaunch(qli)

            If Not launchInfo.CanLaunch Then
                RaiseEvent ErrorOccurred(Me, New QLErrorEventArgs(launchInfo.ErrorMessage, Nothing, "OpenItem"))
                Return
            End If

            RaiseEvent ItemOpened(Me, New QLItemOpenedEventArgs(
                qli.path,
                qli.target,
                qli.path.ToLower().EndsWith(".lnk"),
                qli.isFolder))
        End Sub

        ''' <summary>
        ''' Requests a rename operation
        ''' </summary>
        ''' <param name="qli">The item to rename</param>
        Public Sub RequestRename(qli As QLInfo)
            Dim displayName = QLFileOperations.GetDisplayName(qli.path)
            RaiseEvent RenameRequested(Me, New QLRenameEventArgs(qli.path, displayName, qli.isFolder))
        End Sub

        ''' <summary>
        ''' Requests a delete operation
        ''' </summary>
        ''' <param name="qli">The item to delete</param>
        ''' <param name="permanent">Whether to permanently delete (Shift held)</param>
        Public Sub RequestDelete(qli As QLInfo, permanent As Boolean)
            Dim args As New QLDeleteEventArgs(qli.path, qli.isFolder, permanent)

            If qli.isFolder Then
                Dim stats = QLFileOperations.GetFolderStats(qli.path)
                args.FileCount = stats.FileCount
                args.FolderCount = stats.FolderCount
            End If

            RaiseEvent DeleteRequested(Me, args)
        End Sub

        ''' <summary>
        ''' Creates a new folder
        ''' </summary>
        ''' <param name="parentPath">Parent directory path</param>
        ''' <returns>Path to the created folder, or Nothing on failure</returns>
        Public Function CreateNewFolder(parentPath As String) As String
            Try
                Dim newPath = QLFileOperations.GenerateUniqueFolderPath(parentPath)
                IO.Directory.CreateDirectory(newPath)

                If IO.Directory.Exists(newPath) Then
                    RaiseEvent FolderCreated(Me, New QLFolderCreatedEventArgs(newPath, IO.Path.GetFileName(newPath)))
                    Return newPath
                End If
            Catch ex As Exception
                RaiseEvent ErrorOccurred(Me, New QLErrorEventArgs($"Failed to create folder: {ex.Message}", ex, "CreateNewFolder"))
            End Try

            Return Nothing
        End Function

        ''' <summary>
        ''' Performs a reorder operation from drag-drop
        ''' </summary>
        ''' <param name="folderPath">The folder containing items</param>
        ''' <param name="items">The menu items collection</param>
        ''' <param name="draggedItem">The item that was dragged</param>
        ''' <param name="insertIndex">Where to insert</param>
        Public Sub PerformReorder(folderPath As String, items As ToolStripItemCollection, draggedItem As ToolStripItem, insertIndex As Integer)
            If QLDragDropHandler.PerformReorder(items, draggedItem, insertIndex, folderPath) Then
                Dim qli As QLInfo = CType(draggedItem.Tag, QLInfo)
                RaiseEvent ItemsReordered(Me, New QLReorderEventArgs(folderPath, qli.path, insertIndex))
            End If
        End Sub

        ''' <summary>
        ''' Reads the custom sort order for a folder
        ''' </summary>
        ''' <param name="folderPath">Path to the folder</param>
        ''' <returns>List of item names in custom order</returns>
        Public Function GetSortOrder(folderPath As String) As List(Of String)
            Return QLSort.ReadSortOrder(folderPath)
        End Function

        ''' <summary>
        ''' Gets an icon for a QuickLaunch item
        ''' </summary>
        ''' <param name="qli">The item to get icon for</param>
        ''' <returns>The icon bitmap</returns>
        Public Function GetIcon(qli As QLInfo) As Bitmap
            Return QLIconCache.GetIconFromCache(qli)
        End Function

        ''' <summary>
        ''' Evicts an icon from the cache
        ''' </summary>
        ''' <param name="path">Path of item to evict</param>
        Public Sub EvictIcon(path As String)
            QLIconCache.EvictIconCacheItem(path)
        End Sub

        ''' <summary>
        ''' Validates a filename for rename
        ''' </summary>
        ''' <param name="newName">The proposed new name</param>
        ''' <returns>Error message or empty string if valid</returns>
        Public Function ValidateFileName(newName As String) As String
            Return QLFileOperations.ValidateRename(newName)
        End Function

        ''' <summary>
        ''' Checks if clipboard has paste sources
        ''' </summary>
        ''' <returns>True if paste is available</returns>
        Public Function CanPaste() As Boolean
            Return QLClipboardHandler.HasValidPasteSources(clipBoardInfo)
        End Function

        ''' <summary>
        ''' Gets paste menu text
        ''' </summary>
        ''' <returns>Formatted paste menu text</returns>
        Public Function GetPasteMenuText() As String
            Return QLClipboardHandler.GetPasteMenuText(clipBoardInfo)
        End Function

#End Region

#Region "IDisposable"

        Private disposedValue As Boolean

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' Dispose managed resources
                End If
                disposedValue = True
            End If
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region

    End Class

End Namespace ' QL
