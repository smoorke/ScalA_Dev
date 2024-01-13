Public Class CustomMessageBox
    Public Shared visible As Boolean = False
    Public Shared Function Show(message As String, Optional caption As String = "",
                                Optional buttons As MessageBoxButtons = MessageBoxButtons.OK,
                                Optional icon As MessageBoxIcon = MessageBoxIcon.None) As DialogResult
        visible = True
        Dim result As DialogResult = MessageBox.Show(message, caption, buttons, icon)
        visible = False
        Return result
    End Function
    Public Shared Function Show(owner As IWin32Window,
                                message As String, Optional caption As String = "",
                                Optional buttons As MessageBoxButtons = MessageBoxButtons.OK,
                                Optional icon As MessageBoxIcon = MessageBoxIcon.None) As DialogResult
        visible = True
        Dim result As DialogResult = MessageBox.Show(owner, message, caption, buttons, icon)
        visible = False
        Return result
    End Function
End Class
