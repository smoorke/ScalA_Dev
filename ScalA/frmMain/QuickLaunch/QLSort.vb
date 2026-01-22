Namespace QL

    ''' <summary>
    ''' Module for QuickLaunch custom sort order persistence
    ''' </summary>
    Public Module QLSort

        ''' <summary>
        ''' Name of the hidden file that stores sort order
        ''' </summary>
        Public Const SORT_FILE_NAME As String = ".qlsort"

        ''' <summary>
        ''' Reads the custom sort order from the .qlsort file in the specified folder
        ''' </summary>
        ''' <param name="folderPath">The folder to read sort order from</param>
        ''' <returns>List of item names in custom order, or empty list if no sort file</returns>
        Public Function ReadSortOrder(folderPath As String) As List(Of String)
            Dim sortFile As String = IO.Path.Combine(folderPath, SORT_FILE_NAME)
            Dim result As New List(Of String)
            Try
                If IO.File.Exists(sortFile) Then
                    result = IO.File.ReadAllLines(sortFile).Where(Function(l) Not String.IsNullOrWhiteSpace(l)).ToList()
                End If
            Catch ex As Exception
                dBug.Print($"QLSort: Failed to read sort file: {ex.Message}")
            End Try
            Return result
        End Function

        ''' <summary>
        ''' Writes the custom sort order to the .qlsort file in the specified folder
        ''' </summary>
        ''' <param name="folderPath">The folder to write sort order to</param>
        ''' <param name="sortOrder">The item names in desired order</param>
        Public Sub WriteSortOrder(folderPath As String, sortOrder As IEnumerable(Of String))
            Dim sortFile As String = IO.Path.Combine(folderPath, SORT_FILE_NAME)
            Try
                ' Clear hidden attribute so we can write
                If IO.File.Exists(sortFile) Then
                    IO.File.SetAttributes(sortFile, IO.FileAttributes.Normal)
                End If

                IO.File.WriteAllLines(sortFile, sortOrder)

                ' Set file as hidden
                IO.File.SetAttributes(sortFile, IO.FileAttributes.Hidden)
            Catch ex As Exception
                dBug.Print($"QLSort: Failed to write sort file: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' Gets the sort index for an item. Items in sort order get their index, others get MaxValue.
        ''' </summary>
        ''' <param name="sortOrder">The custom sort order list</param>
        ''' <param name="itemName">The item name to find</param>
        ''' <returns>Index in sort order, or Integer.MaxValue if not found</returns>
        Public Function GetSortIndex(sortOrder As List(Of String), itemName As String) As Integer
            Dim idx As Integer = sortOrder.FindIndex(Function(s) s.Equals(itemName, StringComparison.OrdinalIgnoreCase))
            Return If(idx >= 0, idx, Integer.MaxValue)
        End Function

        ''' <summary>
        ''' Applies custom sort order to items (V1 algorithm - legacy)
        ''' </summary>
        Public Function ApplySortOrderV1(Of T)(items As IEnumerable(Of T), sortOrder As List(Of String), nameSelector As Func(Of T, String), nsSorter As IComparer(Of String)) As List(Of T)
            If sortOrder.Count = 0 Then
                Return items.OrderBy(nameSelector, nsSorter).ToList()
            End If

            ' Build pinned list in exact sortOrder
            Dim pinned As New List(Of T)
            For Each name In sortOrder
                For Each it In items
                    If String.Equals(nameSelector(it), name, StringComparison.OrdinalIgnoreCase) Then
                        pinned.Add(it)
                    End If
                Next
            Next

            ' Build free list
            Dim freeItems = items _
                .Where(Function(it) GetSortIndex(sortOrder, nameSelector(it)) = Integer.MaxValue) _
                .OrderBy(nameSelector, nsSorter) _
                .ToList()

            ' Merge
            Dim result As New List(Of T)
            Dim freeIdx As Integer = 0

            For Each p In pinned
                While freeIdx < freeItems.Count AndAlso
                      nsSorter.Compare(nameSelector(freeItems(freeIdx)), nameSelector(p)) < 0
                    result.Add(freeItems(freeIdx))
                    freeIdx += 1
                End While

                result.Add(p)
            Next

            ' Append remaining free items
            While freeIdx < freeItems.Count
                result.Add(freeItems(freeIdx))
                freeIdx += 1
            End While

            Return result
        End Function

        ''' <summary>
        ''' Applies custom sort order to items (V2 algorithm - current)
        ''' Unpinned items stay in natural order, pinned items follow sortOrder
        ''' </summary>
        Public Function ApplySortOrderV2(Of T)(items As IEnumerable(Of T), sortOrder As List(Of String), nameSelector As Func(Of T, String), nsSorter As IComparer(Of String)) As List(Of T)
            ' Natural sort all items
            Dim ol = items.OrderBy(nameSelector, nsSorter).ToList()
            If sortOrder.Count = 0 Then Return ol

            ' Extract pinned items in natural order
            Dim pl = ol.Where(Function(it) sortOrder.Any(Function(s) s.Equals(nameSelector(it), StringComparison.OrdinalIgnoreCase))).ToList()

            ' Sort pinned items to sortorder
            pl.Sort(Function(a, b) GetSortIndex(sortOrder, nameSelector(a)).CompareTo(GetSortIndex(sortOrder, nameSelector(b))))

            ' Replace pinned positions with sorted pinned items
            Dim i As Integer = 0
            For j = 0 To ol.Count - 1
                Dim oj = j
                If sortOrder.Any(Function(s) s.Equals(nameSelector(ol(oj)), StringComparison.OrdinalIgnoreCase)) Then
                    ol(j) = pl(i)
                    i += 1
                End If
            Next

            Return ol
        End Function
        Public Function ApplySortOrderV3(Of T)(items As IEnumerable(Of T), sortOrder As List(Of String), nameSelector As Func(Of T, String), nsSorter As IComparer(Of String)) As List(Of T)
            ' Build lookup: name -> index
            Dim orderMap As New Dictionary(Of String, Integer)(sortOrder.Count, StringComparer.OrdinalIgnoreCase)
            For i = 0 To sortOrder.Count - 1
                If Not orderMap.ContainsKey(sortOrder(i)) Then
                    orderMap(sortOrder(i)) = i
                End If
            Next

            Return items.Select(Function(it)
                                    Dim Name = nameSelector(it)
                                    Dim idx As Integer
                                    If Not orderMap.TryGetValue(name, idx) Then
                                        idx = Integer.MaxValue
                                    End If
                                    Return New With {.Item = it, .Index = idx, name}
                                End Function) _
                                .OrderBy(Function(x) x.Index).ThenBy(Function(x) x.Name, nsSorter) _
                                .Select(Function(x) x.Item).ToList()
        End Function
    End Module

End Namespace ' QL
