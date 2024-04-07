Public Class CustomMessageBox
    Public Shared visible As Boolean = False
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
        Dim result As DialogResult = MessageBox.Show(owner, message, caption, buttons, icon, defaultbutton)
        visible = False
        Return result
    End Function
End Class
