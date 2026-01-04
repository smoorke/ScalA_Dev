Namespace QL

    ''' <summary>
    ''' Information about a QuickLaunch menu item (file or folder)
    ''' </summary>
    Public Structure QLInfo
        ''' <summary>
        ''' Full path to the file or folder
        ''' </summary>
        Public path As String

        ''' <summary>
        ''' Whether the item has Hidden or System file attribute
        ''' </summary>
        Public hidden As Boolean

        ''' <summary>
        ''' Display name (may differ from filename for shortcuts)
        ''' </summary>
        Public name As String

        ''' <summary>
        ''' For .lnk shortcuts, the resolved target path
        ''' </summary>
        Public target As String

        ''' <summary>
        ''' For .lnk shortcuts, whether the target is a directory
        ''' </summary>
        Public pointsToDir As Boolean

        ''' <summary>
        ''' Whether this item represents a folder (or folder shortcut)
        ''' </summary>
        Public isFolder As Boolean

#If DEBUG Then
        Public test As String
#End If
    End Structure

    ''' <summary>
    ''' Tag structure for context menu items with clipboard actions
    ''' </summary>
    Public Structure MenuTag
        ''' <summary>
        ''' Path to the file/folder
        ''' </summary>
        Public path As String

        ''' <summary>
        ''' Action to perform (Cut, Copy, Paste, PasteLink, Delete)
        ''' </summary>
        Public action As String

        ''' <summary>
        ''' Tooltip text for the menu item
        ''' </summary>
        Public tooltip As String
    End Structure

End Namespace ' QL
