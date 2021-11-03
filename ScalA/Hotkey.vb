Public Class Hotkey
    ''' <summary>
    ''' Declaration of winAPI function wrappers. The winAPI functions are used to register / unregister a hotkey
    ''' </summary>
    Private Declare Function RegisterHotKey Lib "user32" _
        (ByVal hwnd As IntPtr, ByVal id As Integer, ByVal fsModifiers As Integer, ByVal vk As Integer) As Integer

    Private Declare Function UnregisterHotKey Lib "user32" (ByVal hwnd As IntPtr, ByVal id As Integer) As Integer

    Public Const WM_HOTKEY As Integer = &H312

    Enum KeyModifier
        None = 0
        Alt = &H1
        Control = &H2
        Shift = &H4
        Winkey = &H8
        NoRepeat = &H4000
    End Enum 'This enum is just to make it easier to call the registerHotKey function: The modifier integer codes are replaced by a friendly "Alt","Shift" etc.

    '
    ' handle kotkey in wndProc

    Private Shared ReadOnly _HotkeyList As New List(Of Integer)
    Public Shared Sub RegisterHotkey(ByRef sourceForm As Form, ByVal hotkeyID As Integer, ByVal modifier As KeyModifier, ByVal triggerKey As Keys)
        Try
            If Not _HotkeyList.Contains(hotkeyID) Then
                RegisterHotKey(sourceForm.Handle, hotkeyID, modifier, triggerKey)
                _HotkeyList.Add(hotkeyID)
            End If
        Catch ex As Exception
            Debug.Print("registerHotkey failed")
            UnregisterHotKey(sourceForm.Handle, hotkeyID)
        End Try
    End Sub

    Public Shared Sub UnregHotkey(ByVal sourceForm As Form, Optional ByVal hotKeyID As Integer = 0)
        'Debug.Print("Unregister Hotkey " & hotKeyID)
        If hotKeyID = 0 Then
            For Each id As Integer In _HotkeyList
                UnregisterHotKey(sourceForm.Handle, id)  'Remember to call unregisterHotkeys() when closing your application.
            Next
            _HotkeyList.Clear()
            Exit Sub
        End If
        If _HotkeyList.Contains(hotKeyID) Then
            Debug.Print("unregisterHotkey " & hotKeyID)
            UnregisterHotKey(sourceForm.Handle, hotKeyID)  'Remember to call unregisterHotkeys() when closing your application.
            _HotkeyList.Remove(hotKeyID)
        End If
    End Sub
End Class