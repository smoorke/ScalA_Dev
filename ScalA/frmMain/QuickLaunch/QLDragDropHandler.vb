Namespace QL

    ''' <summary>
    ''' Holds the state of a drag-drop operation
    ''' </summary>
    Public Class DragState
        ''' <summary>
        ''' The item currently being dragged
        ''' </summary>
        Public DraggedItem As ToolStripMenuItem = Nothing

        ''' <summary>
        ''' Custom cursor shown during drag
        ''' </summary>
        Public Shared DragCursor As Cursor = Nothing

        ''' <summary>
        ''' The starting point of the drag operation
        ''' </summary>
        Public StartPoint As Point

        ''' <summary>
        ''' Whether a drag is actively in progress
        ''' </summary>
        Public IsActive As Boolean = False

        ''' <summary>
        ''' The current drop target during drag
        ''' </summary>
        Public DropTarget As ToolStripMenuItem = Nothing

        ''' <summary>
        ''' Path to the folder containing dragged items
        ''' </summary>
        Public FolderPath As String = Nothing

        ''' <summary>
        ''' Item being hovered over during drag
        ''' </summary>
        Public HoveredItem As ToolStripMenuItem = Nothing

        ''' <summary>
        ''' Tick count when last dropdown was opened
        ''' </summary>
        Public LastDropDownOpenTick As Integer = 0

        ''' <summary>
        ''' Resets the drag state after a drag operation completes
        ''' </summary>
        Public Sub Reset()
            If DropTarget IsNot Nothing Then
                DropTarget.BackColor = Color.Empty
            End If
            DraggedItem = Nothing
            IsActive = False
            DropTarget = Nothing
            FolderPath = Nothing
            HoveredItem = Nothing
        End Sub

        '''' <summary>
        '''' Starts a new drag operation
        '''' </summary>
        '''' <param name="item">Item being dragged</param>
        '''' <param name="startLocation">Starting mouse location</param>
        'Public Sub Start(item As ToolStripMenuItem, startLocation As Point)
        '    DraggedItem = item
        '    StartPoint = startLocation
        '    IsActive = False

        '    If TypeOf item.Tag Is QLInfo Then
        '        Dim qli As QLInfo = CType(item.Tag, QLInfo)
        '        FolderPath = IO.Path.GetDirectoryName(qli.path.TrimEnd("\"c))
        '    End If
        'End Sub
    End Class

    ''' <summary>
    ''' Module for QuickLaunch drag-drop reordering logic
    ''' </summary>
    Public Module QLDragDropHandler

        '''' <summary>
        '''' Shared drag state for the QuickLaunch menu
        '''' </summary>
        'Public ReadOnly State As New DragState()

        '''' <summary>
        '''' Drag threshold in pixels before drag operation starts
        '''' </summary>
        'Public Const QL_DRAG_THRESHOLD As Integer = 5

        ''' <summary>
        ''' Delay in milliseconds before opening a folder during drag hover
        ''' </summary>
        Public Const QL_DROPDOWN_OPEN_DELAY_MS As Integer = 200

        ''' <summary>
        ''' Result of calculating insert position during drag
        ''' </summary>
        Public Structure InsertPosition
            ''' <summary>
            ''' Index where item should be inserted (-1 if invalid)
            ''' </summary>
            Public Index As Integer

            ''' <summary>
            ''' Item above the insertion point (for visual indicator)
            ''' </summary>
            Public ItemAbove As ToolStripItem

            ''' <summary>
            ''' Item below the insertion point (for visual indicator)
            ''' </summary>
            Public ItemBelow As ToolStripItem

            ''' <summary>
            ''' Whether the drop is valid at this position
            ''' </summary>
            Public IsValid As Boolean
        End Structure

        ''' <summary>
        ''' Calculates the insert position for a dragged item based on cursor location
        ''' </summary>
        ''' <param name="items">Collection of menu items</param>
        ''' <param name="clientPoint">Cursor position in client coordinates</param>
        ''' <param name="draggedInfo">QLInfo of the item being dragged</param>
        ''' <param name="draggedItem">The item being dragged (to exclude from calculation)</param>
        ''' <returns>InsertPosition with calculated index and indicator items</returns>
        Public Function CalculateInsertPosition(items As ToolStripItemCollection,
                                                clientPoint As Point,
                                                draggedInfo As QLInfo,
                                                draggedItem As ToolStripItem) As InsertPosition

            Dim result As New InsertPosition With {.Index = -1, .IsValid = False}

            ' Get counts for folder/file boundary handling
            Dim qlItems = items.Cast(Of ToolStripItem).Where(Function(it) TypeOf it.Tag Is QLInfo).ToList()
            Dim lastFolderIndex As Integer = qlItems.Where(Function(it) CType(it.Tag, QLInfo).isFolder).Count - 1
            Dim lastIndex As Integer = qlItems.Count - 1

            If lastIndex < 0 Then Return result

            ' Find the item at cursor position
            For i As Integer = 0 To qlItems.Count - 1
                Dim item As ToolStripItem = items(i)
                If TypeOf item IsNot ToolStripMenuItem Then Continue For

                Dim info As QLInfo = CType(item.Tag, QLInfo)

                ' Only consider same-type items (folder with folder, file with file)
                ' Exception: allow dropping at the boundary between folders and files
                If info.isFolder <> draggedInfo.isFolder AndAlso i <> lastFolderIndex Then Continue For

                If item.Bounds.Contains(clientPoint) Then
                    Dim midY As Integer = item.Bounds.Y + (item.Bounds.Height \ 2)
                    result.Index = If(clientPoint.Y < midY, i, i + 1)
                    Exit For
                End If
            Next

            If result.Index < 0 Then Return result

            ' Determine visual indicator items
            If result.Index = 0 Then
                result.ItemAbove = Nothing
                result.ItemBelow = items(0)
            ElseIf result.Index >= items.Count Then
                result.ItemAbove = items(result.Index - 1)
                result.ItemBelow = Nothing
            Else
                Dim aboveIndex = result.Index - 1
                Dim belowIndex = result.Index

                ' Skip invisible items
                While aboveIndex > 0 AndAlso Not items(aboveIndex).Visible
                    aboveIndex -= 1
                End While
                While belowIndex < lastIndex AndAlso Not items(belowIndex).Visible
                    belowIndex += 1
                End While

                result.ItemAbove = items(aboveIndex)
                result.ItemBelow = items(belowIndex)

                ' Clear indicators if they cross folder/file boundary inappropriately
                If result.ItemAbove IsNot Nothing AndAlso TypeOf result.ItemAbove.Tag Is QLInfo Then
                    If CType(result.ItemAbove.Tag, QLInfo).isFolder <> draggedInfo.isFolder Then
                        result.ItemAbove = Nothing
                    End If
                End If
                If result.ItemBelow IsNot Nothing AndAlso TypeOf result.ItemBelow.Tag Is QLInfo Then
                    If CType(result.ItemBelow.Tag, QLInfo).isFolder <> draggedInfo.isFolder Then
                        result.ItemBelow = Nothing
                    End If
                End If
            End If

            ' Don't show indicator on the dragged item itself
            If result.ItemAbove Is draggedItem Then result.ItemAbove = Nothing
            If result.ItemBelow Is draggedItem Then result.ItemBelow = Nothing

            result.IsValid = True
            Return result
        End Function

        ''' <summary>
        ''' Performs the reorder operation and persists the new sort order
        ''' </summary>
        ''' <param name="items">Collection of menu items</param>
        ''' <param name="draggedItem">The item that was dragged</param>
        ''' <param name="insertIndex">The calculated insert index</param>
        ''' <param name="folderPath">Path to the folder (for persisting sort order)</param>
        ''' <returns>True if reorder was performed</returns>
        Public Function PerformReorder(items As ToolStripItemCollection,
                                       draggedItem As ToolStripItem,
                                       insertIndex As Integer,
                                       folderPath As String) As Boolean

            If insertIndex < 0 Then Return False

            Dim draggedInfo As QLInfo = CType(draggedItem.Tag, QLInfo)
            Dim currentIndex As Integer = items.IndexOf(draggedItem)

            ' Adjust index if moving down in the list
            If currentIndex < insertIndex Then insertIndex -= 1

            ' Check if actually moving and same type
            If currentIndex = insertIndex Then Return False
            If insertIndex >= 0 AndAlso insertIndex < items.Count Then
                Dim targetItem = items(insertIndex)
                If TypeOf targetItem.Tag Is QLInfo Then
                    If CType(targetItem.Tag, QLInfo).isFolder <> draggedInfo.isFolder Then
                        Return False
                    End If
                End If
            End If

            ' Perform the move
            items.Insert(insertIndex, draggedItem)

            ' Persist the new sort order
            Dim sortOrder = items.Cast(Of ToolStripItem) _
                                 .Where(Function(it) TypeOf it.Tag Is QLInfo) _
                                 .Select(Function(it) CType(it.Tag, QLInfo).path.TrimEnd("\"c))

            WriteSortOrder(folderPath, sortOrder)

            Return True
        End Function

        '''' <summary>
        '''' Builds a sort order list from menu items when no persisted order exists
        '''' </summary>
        '''' <param name="items">Collection of menu items</param>
        '''' <returns>List of item paths in current order</returns>
        'Public Function BuildSortOrderFromItems(items As ToolStripItemCollection) As List(Of String)
        '    Dim sortOrder As New List(Of String)

        '    For Each item As ToolStripItem In items
        '        If TypeOf item Is ToolStripMenuItem AndAlso TypeOf item.Tag Is QLInfo Then
        '            Dim qli As QLInfo = CType(item.Tag, QLInfo)
        '            Dim itemName As String = If(qli.path.EndsWith("\"), qli.name, IO.Path.GetFileName(qli.path))
        '            sortOrder.Add(itemName)
        '        End If
        '    Next

        '    Return sortOrder
        'End Function

        '''' <summary>
        '''' Updates sort order by moving an item to a new position
        '''' </summary>
        '''' <param name="folderPath">Path to the folder</param>
        '''' <param name="dragName">Name of the item being dragged</param>
        '''' <param name="dropName">Name of the item to drop before</param>
        '''' <param name="fallbackItems">Items to build order from if none exists</param>
        'Public Sub UpdateSortOrder(folderPath As String,
        '                           dragName As String,
        '                           dropName As String,
        '                           Optional fallbackItems As ToolStripItemCollection = Nothing)

        '    Dim sortOrder As List(Of String) = ReadSortOrder(folderPath)

        '    ' If no sort order exists, build from fallback items
        '    If sortOrder.Count = 0 AndAlso fallbackItems IsNot Nothing Then
        '        sortOrder = BuildSortOrderFromItems(fallbackItems)
        '    End If

        '    ' Remove drag item from current position
        '    sortOrder.RemoveAll(Function(s) s.Equals(dragName, StringComparison.OrdinalIgnoreCase))

        '    ' Find drop target position and insert
        '    Dim dropIndex As Integer = sortOrder.FindIndex(Function(s) s.Equals(dropName, StringComparison.OrdinalIgnoreCase))
        '    If dropIndex >= 0 Then
        '        sortOrder.Insert(dropIndex, dragName)
        '    Else
        '        sortOrder.Add(dragName)
        '    End If

        '    ' Save the new sort order
        '    WriteSortOrder(folderPath, sortOrder)
        'End Sub

        '''' <summary>
        '''' Checks if a drag has exceeded the threshold to start
        '''' </summary>
        '''' <param name="startPoint">Starting mouse position</param>
        '''' <param name="currentPoint">Current mouse position</param>
        '''' <returns>True if drag threshold exceeded</returns>
        'Public Function IsDragThresholdExceeded(startPoint As Point, currentPoint As Point) As Boolean
        '    Dim dx As Integer = Math.Abs(currentPoint.X - startPoint.X)
        '    Dim dy As Integer = Math.Abs(currentPoint.Y - startPoint.Y)
        '    Return dx > QL_DRAG_THRESHOLD OrElse dy > QL_DRAG_THRESHOLD
        'End Function

    End Module

End Namespace ' QL
