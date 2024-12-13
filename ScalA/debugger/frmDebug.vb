Public Class frmDebug

    Private Sub frmDebug_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If logBuilder Is Nothing Then
            logBuilder = New System.Text.StringBuilder With {.Capacity = 100_000}
        End If
        dBug.Print("FrmDebug Load", 1)
    End Sub

    Private Sub tmrDebug_Tick(sender As Object, e As EventArgs) Handles tmrDebug.Tick
        If logBuilder IsNot Nothing Then
            txtDebugLog.Text = logBuilder.ToString
        Else
            dBug.Print("LogBuilder Nothing", 1)
        End If
    End Sub
End Class