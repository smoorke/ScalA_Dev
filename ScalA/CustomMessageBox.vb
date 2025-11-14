Public Class CustomMessageBox
    Public Shared visible As Boolean = False
    Private Shared hWnd As IntPtr = IntPtr.Zero
    Public Shared Function Show(message As String, Optional caption As String = "",
                                Optional buttons As MessageBoxButtons = MessageBoxButtons.OK,
                                Optional icon As MessageBoxIcon = MessageBoxIcon.None,
                                Optional defaultbutton As MessageBoxDefaultButton = MessageBoxDefaultButton.Button1) As DialogResult
        visible = True
        Dim result As DialogResult = MessageBox.Show(message, caption, buttons, icon, defaultbutton)
        visible = False
        Return result
    End Function
    Public Shared Function Show(owner As IWin32Window,
                                message As String, Optional caption As String = "",
                                Optional buttons As MessageBoxButtons = MessageBoxButtons.OK,
                                Optional icon As MessageBoxIcon = MessageBoxIcon.None,
                                Optional defaultbutton As MessageBoxDefaultButton = MessageBoxDefaultButton.Button1) As DialogResult
        visible = True

        ' Task to find the specific MessageBox HWND
        Task.Run(Sub()
                     Dim attempts As Integer = 0
                     Do While visible AndAlso hWnd = IntPtr.Zero AndAlso attempts < 100
                         Dim hW As IntPtr = FindWindow("#32770", caption)
                         If hW <> IntPtr.Zero Then
                             Dim hStatic As IntPtr = FindWindowEx(hW, IntPtr.Zero, "Static", message)
                             If hStatic <> IntPtr.Zero Then
                                 hWnd = hW
                                 SendMessage(hWnd, WM_SETICON, ICON_BIG, My.Resources.moa3.Handle)
                                 Exit Do
                             End If
                         End If
                         Threading.Thread.Sleep(20)
                         attempts += 1
                     Loop
                 End Sub)

        ' Task to monitor ESC key
        If MessageBoxButtons.YesNo = buttons Then Task.Run(Sub()
                                                               Do While visible
                                                                   If hWnd <> IntPtr.Zero AndAlso (GetAsyncKeyState(Keys.Escape) And &H8000) <> 0 Then
                                                                       ' Find the last button in the MessageBox
                                                                       Dim hLastBtn As IntPtr = IntPtr.Zero
                                                                       Dim hBtn As IntPtr = IntPtr.Zero
                                                                       Do
                                                                           hBtn = FindWindowEx(hWnd, hBtn, "Button", Nothing)
                                                                           If hBtn <> IntPtr.Zero Then hLastBtn = hBtn
                                                                       Loop While hBtn <> IntPtr.Zero

                                                                       ' Click it if found
                                                                       If hLastBtn <> IntPtr.Zero Then
                                                                           SendMessage(hLastBtn, BM_CLICK, IntPtr.Zero, IntPtr.Zero)
                                                                           Debug.Print("MessageBox ESC -> last button clicked")
                                                                           Exit Do
                                                                       End If
                                                                   End If
                                                                   Threading.Thread.Sleep(25)
                                                               Loop
                                                           End Sub)


        Dim result As DialogResult = MessageBox.Show(owner, message, caption, buttons, icon, defaultbutton)
        hWnd = IntPtr.Zero
        visible = False
        Return result
    End Function
End Class
