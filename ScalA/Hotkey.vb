Public NotInheritable Class Hotkey
    ''' <summary>
    ''' Declaration of winAPI function wrappers. The winAPI functions are used to register / unregister a hotkey
    ''' </summary>
    Private Declare Function RegisterHotKey Lib "user32" _
        (ByVal hwnd As IntPtr, ByVal id As Integer, ByVal fsModifiers As Integer, ByVal vk As Integer) As Integer

    Private Declare Function UnregisterHotKey Lib "user32" (ByVal hwnd As IntPtr, ByVal id As Integer) As Integer

    Public Const WM_HOTKEY As Integer = &H312
    <Flags()>
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
    Public Shared Function RegisterHotkey(ByRef sourceForm As Form, ByVal hotkeyID As Integer, ByVal modifier As KeyModifier, ByVal triggerKey As Keys) As Boolean
        Try
            If Not _HotkeyList.Contains(hotkeyID) Then
                Dim ret = RegisterHotKey(sourceForm.Handle, hotkeyID, modifier, triggerKey)
                'dBug.print($"regHK {hotkeyID} {modifier} {triggerKey} {ret}")
                _HotkeyList.Add(hotkeyID)
                Return ret = 1
            End If
        Catch ex As Exception
            dBug.print("registerHotkey failed")
            UnregisterHotKey(sourceForm.Handle, hotkeyID)
            Return False
        End Try
        Return False
    End Function

    Public Shared Sub UnregHotkey(ByVal sourceForm As Form, Optional ByVal hotKeyID As Integer = 0)
        'dBug.print("Unregister Hotkey " & hotKeyID)
        If hotKeyID = 0 Then
            For Each id As Integer In _HotkeyList
                UnregisterHotKey(sourceForm.Handle, id)  'Remember to call unregisterHotkeys() when closing your application.
            Next
            _HotkeyList.Clear()
            Exit Sub
        End If
        If _HotkeyList.Contains(hotKeyID) Then
            'dBug.print("unregisterHotkey " & hotKeyID)
            UnregisterHotKey(sourceForm.Handle, hotKeyID)  'Remember to call unregisterHotkeys() when closing your application.
            _HotkeyList.Remove(hotKeyID)
        End If
    End Sub

    Public Shared Sub UnregHotKey(ByVal sourceForm As Form, ByVal hotKeyIDs As Integer())
        For Each id As Integer In hotKeyIDs
            UnregHotkey(sourceForm, id)
        Next
    End Sub

End Class

Partial NotInheritable Class frmmain

    Private Sub tmrHotkeys_Tick(sender As Object, e As EventArgs) Handles tmrHotkeys.Tick
        If (activeID = scalaPID OrElse activeIsAstonia) Then
            If My.Settings.SwitchToOverview Then
                Hotkey.RegisterHotkey(Me, 1, Hotkey.KeyModifier.NoRepeat Or My.Settings.StoCtrl Or My.Settings.StoShift Or My.Settings.StoAlt Or If(My.Settings.DisableWinKey, 0, My.Settings.StoWin), My.Settings.StoKey)
            Else
                Hotkey.UnregHotkey(Me, 1)
            End If
            If My.Settings.CycleAlt Then
                Hotkey.RegisterHotkey(Me, 2, Hotkey.KeyModifier.NoRepeat Or My.Settings.CycleAltKeyFwd Or My.Settings.CycleShiftKeyFwd Or My.Settings.CycleCtrlKeyFwd Or If(My.Settings.DisableWinKey, 0, My.Settings.CycleWinKeyFwd), My.Settings.CycleKeyFwd)
                Hotkey.RegisterHotkey(Me, 3, Hotkey.KeyModifier.NoRepeat Or My.Settings.CycleAltKeyBwd Or My.Settings.CycleShiftKeyBwd Or My.Settings.CycleCtrlKeyBwd Or If(My.Settings.DisableWinKey, 0, My.Settings.CycleWinKeyBwd), My.Settings.CycleKeyBwd)
            Else
                Hotkey.UnregHotkey(Me, 2)
                Hotkey.UnregHotkey(Me, 3)
            End If
            If My.Settings.CloseAll Then
                Hotkey.RegisterHotkey(Me, 4, Hotkey.KeyModifier.NoRepeat Or My.Settings.CloseAllAlt Or My.Settings.CloseAllShift Or My.Settings.CloseAllCtrl Or If(My.Settings.DisableWinKey, 0, My.Settings.CloseAllWin), My.Settings.CloseAllKey)
            Else
                Hotkey.UnregHotkey(Me, 4)
            End If

            If My.Settings.AlterOverview Then
                Hotkey.RegisterHotkey(Me, 6, Hotkey.KeyModifier.NoRepeat Or My.Settings.AlterOverviewPlusAlt Or My.Settings.AlterOverviewPlusCtrl Or My.Settings.AlterOverviewPlusShift Or If(My.Settings.DisableWinKey, 0, My.Settings.AlterOverviewPlusWin), My.Settings.AlterOverviewPlusKey)
                Hotkey.RegisterHotkey(Me, 7, Hotkey.KeyModifier.NoRepeat Or My.Settings.AlterOverviewMinAlt Or My.Settings.AlterOverviewMinCtrl Or My.Settings.AlterOverviewMinShift Or If(My.Settings.DisableWinKey, 0, My.Settings.AlterOverviewMinWin), My.Settings.AlterOverviewMinKey)
                Hotkey.RegisterHotkey(Me, 8, Hotkey.KeyModifier.NoRepeat Or My.Settings.AlterOverviewStarAlt Or My.Settings.AlterOverviewStarCtrl Or My.Settings.AlterOverviewStarShift Or If(My.Settings.DisableWinKey, 0, My.Settings.AlterOverviewStarWin), My.Settings.AlterOverviewStarKey)
            Else
                Hotkey.UnregHotkey(Me, 6)
                Hotkey.UnregHotkey(Me, 7)
                Hotkey.UnregHotkey(Me, 8)
            End If

        Else
            Hotkey.UnregHotKey(Me, {1, 2, 3, 4, 6, 7, 8})
            If My.Settings.ToggleTop Then
                Hotkey.RegisterHotkey(Me, 5, Hotkey.KeyModifier.NoRepeat Or My.Settings.TogTopAlt Or My.Settings.TogTopShift Or My.Settings.TogTopCtrl Or My.Settings.TogTopWin, My.Settings.ToggleTopKey)
            Else
                Hotkey.UnregHotkey(Me, 5)
            End If
        End If
    End Sub

End Class