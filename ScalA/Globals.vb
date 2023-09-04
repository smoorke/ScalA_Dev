﻿Module Globals
    Public Function WinUsingDarkTheme() As Boolean
        Using key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Themes\Personalize")
            Dim value = key?.GetValue("AppsUseLightTheme")
            If value IsNot Nothing AndAlso value = 0 Then
                Return True
            End If
        End Using
        Return False
    End Function
End Module
