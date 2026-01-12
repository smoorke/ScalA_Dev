''' <summary>
''' Dialog form for setting up launcher templates and creating shortcuts
''' </summary>
Public Class frmLauncherSetup

    Private Sub frmLauncherSetup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set target folder to QL root
        launcherSetupControl1.TargetFolder = My.Settings.links
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class
