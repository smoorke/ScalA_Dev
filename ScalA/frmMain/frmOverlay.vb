Public Class frmOverlay
    Private Sub frmOverlay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Owner = FrmMain
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case WM_WINDOWPOSCHANGING
                If Not startup Then
                    If Not Owner?.IsDisposed Then
                        If FrmMain.AltPP IsNot Nothing Then


                            Dim winpos As WINDOWPOS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                                'Debug.Print(winpos.flags.ToString)
                                Dim nudge As Integer = 0
                                If FrmMain.AltPP.isSDL Then
                                    nudge = (FrmMain.AltPP.ClientRect.Width Mod 800) \ 2
                                End If
                            'Debug.Print(nudge)

                            Dim tr = New Rectangle(671.Map(0, 800, 0, winpos.cx),
                                                        10.Map(0, 600, 0, winpos.cy),
                                                        24.Map(0, 800, 0, winpos.cx),
                                                        24.Map(0, 600, 0, winpos.cy))
                                If tr <> Rectangle.Empty Then
                                    pbRestart.Bounds = tr
                                End If


                            'pbRestart.Location = New Point(671.Map(0, FrmMain.AltPP.ClientRect.Width, 0, winpos.cx),
                            '                                10.Map(0, FrmMain.AltPP.ClientRect.Height, 0, winpos.cy))
                            'pbRestart.Size = New Size(24.Map(0, FrmMain.AltPP.ClientRect.Width, 0, winpos.cx),
                            '                          24.Map(0, FrmMain.AltPP.ClientRect.Height, 0, winpos.cy))

                        End If
                    End If
                End If
        End Select
        MyBase.WndProc(m)
    End Sub


    Private Async Sub pbRestart_Click(sender As PictureBox, e As MouseEventArgs) Handles pbRestart.MouseClick
        If e.Button <> MouseButtons.Left Then
            Exit Sub
        End If

        'TODO: add getting name from MOS commandlineargs in AstoniaProcess.vb
        Dim targetname As String = If(FrmMain.AltPP?.Name = "Someone", FrmMain.AltPP?.loggedInAs, FrmMain.AltPP.Name)

        '        Dim targetname As String = If(FrmMain.AltPP?.Name = "Someone", If(String.IsNullOrEmpty(FrmMain.AltPP?.loggedInAs), FrmMain.AltPP.Name, FrmMain.AltPP?.loggedInAs))

        ' targetname = targetname.FirstToUpper()

        dBug.Print($"restarting {targetname}")

        Me.pbRestart.Hide()

        FrmMain.Cursor = Cursors.WaitCursor
        Me.UseWaitCursor = True
        FrmMain.AltPP.restart()
        Dim count As Integer = 0

        While Not String.IsNullOrEmpty(targetname)
            count += 1
            Await Task.Delay(50)
            Dim targetPPs As AstoniaProcess = AstoniaProcess.Enumerate(FrmMain.blackList).FirstOrDefault(Function(ap) ap.Name.Contains(targetname))
            If targetPPs IsNot Nothing AndAlso targetPPs.Id <> 0 Then
                FrmMain.PopDropDown(FrmMain.cboAlt)
                FrmMain.cboAlt.SelectedItem = targetPPs
                Exit While
            End If
            If count >= 100 Then
                FrmMain.Cursor = Cursors.Default
                Me.UseWaitCursor = False
                CustomMessageBox.Show(FrmMain, "Restarting failed")
                Exit While
            End If
        End While
        FrmMain.Cursor = Cursors.Default
        Me.UseWaitCursor = False
    End Sub

    Private Sub pbRestart_Resize(sender As Object, e As EventArgs) Handles pbRestart.Resize
        dBug.Print($"pbRestart.Size {pbRestart.Size}")
    End Sub

    Private Sub HideThisToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HideThisToolStripMenuItem.Click
        If FrmMain.AltPP IsNot Nothing Then
            FrmMain.AltPP.hideRestart = True
        End If
    End Sub

    Private Sub cmsRestartHide_Opened(sender As ContextMenuStrip, e As EventArgs) Handles cmsRestartHide.Opened
        sender.Location = pbRestart.PointToScreen(New Point(0, pbRestart.Height))
    End Sub


End Class