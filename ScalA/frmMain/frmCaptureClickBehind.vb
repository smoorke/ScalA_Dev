Public Class FrmCaptureClickBehind
    Private Sub FrmCaptureClickBehind_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        Debug.Print($"behind down {e.Button}")
        Try
            AppActivate(FrmMain.AltPP.Id)
        Catch ex As Exception

        End Try

        Dim msg As Integer = WM_XBUTTONDOWN
        Select Case e.Button
            Case MouseButtons.Left
                msg = WM_LBUTTONDOWN
            Case MouseButtons.Right
                msg = WM_RBUTTONDOWN
            Case MouseButtons.Middle
                msg = WM_MBUTTONDOWN
        End Select

        SendMessage(FrmMain.AltPP?.MainWindowHandle, msg, 0, IntPtr.Zero)

    End Sub

    Private Sub FrmCaptureClickBehind_Click(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        Debug.Print($"behind up {e.Button}")
        Try
            AppActivate(FrmMain.AltPP.Id)
        Catch ex As Exception

        End Try

        Dim msg As Integer = WM_XBUTTONDOWN
        Select Case e.Button
            Case MouseButtons.Left
                msg = WM_LBUTTONDOWN
            Case MouseButtons.Right
                msg = WM_RBUTTONDOWN
            Case MouseButtons.Middle
                msg = WM_MBUTTONDOWN
        End Select

        SendMessage(FrmMain.AltPP?.MainWindowHandle, msg, 0, IntPtr.Zero)

    End Sub

End Class