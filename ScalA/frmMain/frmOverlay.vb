Public Class frmOverlay
    Private Sub frmOverlay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Owner = FrmMain
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case WM_WINDOWPOSCHANGING
                If Not startup AndAlso FrmMain.AltPP IsNot Nothing Then
                    Dim winpos As WINDOWPOS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                    Dim tr = New Rectangle(671.Map(0, FrmMain.AltPP.ClientRect.Width, 0, winpos.cx),
                                                    10.Map(0, FrmMain.AltPP.ClientRect.Height, 0, winpos.cy),
                                                    24.Map(0, FrmMain.AltPP.ClientRect.Width, 0, winpos.cx),
                                                    24.Map(0, FrmMain.AltPP.ClientRect.Height, 0, winpos.cy))
                    If tr <> Rectangle.Empty Then
                        pbRestart.Bounds = tr
                    End If


                    'pbRestart.Location = New Point(671.Map(0, FrmMain.AltPP.ClientRect.Width, 0, winpos.cx),
                    '                                10.Map(0, FrmMain.AltPP.ClientRect.Height, 0, winpos.cy))
                    'pbRestart.Size = New Size(24.Map(0, FrmMain.AltPP.ClientRect.Width, 0, winpos.cx),
                    '                          24.Map(0, FrmMain.AltPP.ClientRect.Height, 0, winpos.cy))
                End If
        End Select
        MyBase.WndProc(m)
    End Sub



    Private Async Sub pbRestart_Click(sender As Object, e As EventArgs) Handles pbRestart.Click
        Dim targetname As String = FrmMain.AltPP.loggedInAs
        dBug.Print($"restarting {targetname}")

        Me.pbRestart.Hide()

        FrmMain.AltPP.restart()
        FrmMain.Cursor = Cursors.WaitCursor
        Dim count As Integer = 0

        While Not String.IsNullOrEmpty(targetname)
            count += 1
            Await Task.Delay(50)
            Dim targetPPs As AstoniaProcess() = AstoniaProcess.Enumerate(FrmMain.blackList).Where(Function(ap) ap.Name = targetname).ToArray()
            If targetPPs.Length > 0 AndAlso targetPPs(0) IsNot Nothing AndAlso targetPPs(0).Id <> 0 Then
                FrmMain.PopDropDown(FrmMain.cboAlt)
                FrmMain.cboAlt.SelectedItem = targetPPs(0)
                Exit While
            End If
            If count >= 100 Then
                CustomMessageBox.Show(FrmMain, "Restarting failed")
                Exit While
            End If
        End While
        FrmMain.Cursor = Cursors.Default
    End Sub

    Private Sub pbRestart_Resize(sender As Object, e As EventArgs) Handles pbRestart.Resize
        dBug.Print($"pbRestart.Size {pbRestart.Size}")
    End Sub
End Class