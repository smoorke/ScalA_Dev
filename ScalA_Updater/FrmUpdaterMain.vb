﻿Friend Class FrmUpdaterMain
    Private args() As String
    Private Sub FrmUpdaterMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        args = Environment.GetCommandLineArgs()
        btnAgain_Click(btnAgain, EventArgs.Empty)
    End Sub
    Private Sub btnAgain_Click(sender As Object, e As EventArgs) Handles btnAgain.Click
        Try
            If args.Length = 1 Then Throw New ArgumentException("Incorrect Arguments Supplied")
            System.IO.File.Copy(FileIO.SpecialDirectories.Temp & "\ScalA\ScalA.exe", args(1), True)
            ExecuteProcessUnElevated(args(args.Length - 1), "", If(args.Length = 4, args(args.Length - 2), IO.Directory.GetCurrentDirectory()))
            End
        Catch ex As Exception
            txtErrorMsg.Text = ex.Message
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            ExecuteProcessUnElevated(args(Math.Max(0, args.Length - 1)), "", If(args.Length = 4, args(args.Length - 2), IO.Directory.GetCurrentDirectory()))
        Catch ex As Exception
            Debug.Print(ex.Message)
        Finally
            End
        End Try
    End Sub

End Class
