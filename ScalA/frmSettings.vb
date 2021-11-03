Public Class FrmSettings
    Private Sub FrmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'storeZoom = My.Settings.zoom

        Me.Owner = FrmMain
        Me.CenterToParent()

        Me.TopMost = My.Settings.topmost
        chkTopMost.Checked = My.Settings.topmost

        'chkAspect.Checked = My.Settings.lockAspect
        'cmbAnchor.SelectedIndex = My.Settings.anchor

        numXoffset.Value = My.Settings.offset.X
        numYoffset.Value = My.Settings.offset.Y

        txtResolutions.Text = My.Settings.resolutions

        txtExe.Text = My.Settings.exe
        txtClass.Text = My.Settings.className

    End Sub


    Dim storeZoom As Integer = My.Settings.zoom
    Private Sub ChkDoAlign_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkDoAlign.CheckedChanged
        If sender.Checked AndAlso FrmMain.AltPP.Id = 0 Then
            MessageBox.Show(FrmMain, "To perform alignment an alt needs to be selected.", "ScalA Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            sender.Checked = False
            Exit Sub
        End If
        grpAlign.Enabled = sender.Checked
        FrmMain.tmrTick.Enabled = Not sender.Checked
        FrmMain.cmbResolution.SelectedIndex = If(sender.Checked, 0, storeZoom)
        FrmMain.UpdateThumb(If(sender.Checked, 122, 255))

        If sender.Checked Then
            FrmMain.SetWindowPos(FrmMain.AltPP.MainWindowHandle, FrmMain.Handle, FrmMain.newX, FrmMain.newY, -1, -1, FrmMain.SetWindowPosFlags.IgnoreResize + FrmMain.SetWindowPosFlags.DoNotActivate)
            FrmMain.GetWindowRect(FrmMain.AltPP.MainWindowHandle, rcAstOffsetBase)
            Debug.Print(rcAstOffsetBase.ToString)
        End If
        tmrAlign.Enabled = sender.Checked
        chkDoAlign.Enabled = Not sender.Checked
    End Sub



    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.chkDoAlign.Checked = False
        Me.Close()
    End Sub

    Private Function ParseResolutions() As Boolean
        Const width = 0
        Const height = 1
        Const fail = False
        Const success = True
        Dim resList As New List(Of Size)
        Try
            Debug.Print("parseRes")
            For Each line As String In txtResolutions.Lines
                Debug.Print(line)
                Dim parts() As String = line.ToUpper.Split("X")
                Debug.Print(parts(width) & " " & parts(height))
                If parts(width) < 400 OrElse parts(height) < 300 Then
                    MessageBox.Show("Error: " & line & " is too small a resolution.")
                    Return fail
                End If
                resList.Add(New Size(parts(width), parts(height)))
            Next
            If resList.Count = 0 Then
                MessageBox.Show("Error: no resolutions defined.")
                Return fail
            End If

            FrmMain.zooms = resList.ToArray
            FrmMain.cmbResolution.Items.Clear()
            For Each sz As Size In resList
                FrmMain.cmbResolution.Items.Add(sz.Width & "x" & sz.Height)
            Next

            'frmMain.cmbResolution.SelectedIndex = 0
            Return success
        Catch
            MessageBox.Show("Error in resolutions")
            Return fail
        End Try
    End Function

    Private Sub BtnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        'save settings
        If My.Settings.resolutions <> txtResolutions.Text Then
            If Not ParseResolutions() Then Exit Sub
            FrmMain.cmbResolution.SelectedIndex = 0
        End If
        My.Settings.resolutions = txtResolutions.Text

        Me.chkDoAlign.Checked = False
        My.Settings.topmost = chkTopMost.Checked
        FrmMain.TopMost = chkTopMost.Checked
        My.Settings.offset = New Point(numXoffset.Value, numYoffset.Value)
        My.Settings.exe = txtExe.Text
        My.Settings.className = txtClass.Text
        Me.Close()
    End Sub

    Dim rcAstOffsetBase As Rectangle
    Public ScalaMoved As Point
    Dim rcAstOffsetNew As Rectangle
    Private Sub TmrAlign_Tick(sender As Object, e As EventArgs) Handles tmrAlign.Tick
        FrmMain.GetWindowRect(FrmMain.AltPP.MainWindowHandle, rcAstOffsetNew)
        manualNumUpdate = False
        numXoffset.Value = My.Settings.offset.X + ScalaMoved.X - rcAstOffsetNew.Left + rcAstOffsetBase.Left
        numYoffset.Value = My.Settings.offset.Y + ScalaMoved.Y - rcAstOffsetNew.Top + rcAstOffsetBase.Top
        manualNumUpdate = True
    End Sub

    Public manualNumUpdate As Boolean = True

    Private Sub NumXYoffsets_ValueChanged(sender As NumericUpDown, e As EventArgs) Handles numYoffset.ValueChanged, numXoffset.ValueChanged

        If manualNumUpdate Then
            Dim ptMove As New Point(0, 0)
            If sender.Tag Then
                ptMove.Y += sender.Text - sender.Value
            Else
                ptMove.X += sender.Text - sender.Value
            End If

            FrmMain.SetWindowPos(FrmMain.AltPP.MainWindowHandle, FrmMain.Handle, rcAstOffsetNew.Left + ptMove.X, rcAstOffsetNew.Top + ptMove.Y, -1, -1, FrmMain.SetWindowPosFlags.IgnoreResize + FrmMain.SetWindowPosFlags.DoNotActivate)

        End If

    End Sub

    Private Sub TxtResolutions_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtResolutions.KeyPress
        If Not (Char.IsDigit(e.KeyChar) Or Char.IsControl(e.KeyChar) Or e.KeyChar.ToString.ToLower = "x") Then
            e.Handled = True
        ElseIf e.KeyChar = "X" Then
            e.KeyChar = "x"
        End If
    End Sub

    Private Sub BtnResetAlign_Click(sender As Object, e As EventArgs) Handles btnResetAlign.Click
        chkDoAlign.Checked = False
        My.Settings.offset = New Point(0, 0)
        numXoffset.Text = 0
        numYoffset.Text = 0
    End Sub
End Class