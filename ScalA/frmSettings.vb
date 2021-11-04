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

        txtQuickLaunchPath.Text = My.Settings.links
        txtQuickLaunchPath.SelectionStart = txtQuickLaunchPath.TextLength

        txtExe.Text = My.Settings.exe
        txtClass.Text = My.Settings.className

        btnOpenFolderDialog.Image = GetStockIconImage(SIID.folder)
        If btnOpenFolderDialog.Image IsNot Nothing Then btnOpenFolderDialog.Text = ""

    End Sub
    'https://docs.microsoft.com/en-us/windows/win32/api/shellapi/ne-shellapi-shstockiconid
    Enum SIID As UInteger
        folder = 3
        folderopen = 4
        shield = 77
    End Enum

    Private Shared Function GetStockIconImage(type As SIID) As Image
        Dim info As New SHSTOCKICONINFO()
        info.cbSize = Runtime.InteropServices.Marshal.SizeOf(info)

        SHGetStockIconInfo(type, &H101, info)

        Using ico As Icon = Icon.FromHandle(info.hIcon).Clone()
            DestroyIcon(info.hIcon)
            Dim bm As Bitmap = ico.ToBitmap
            DestroyIcon(ico.Handle)
            Return bm
        End Using
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint:="DestroyIcon")>
    Private Shared Function DestroyIcon(ByVal hIcon As System.IntPtr) As <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)> Boolean
    End Function

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, CharSet:=Runtime.InteropServices.CharSet.Unicode)>
    Public Structure SHSTOCKICONINFO

        Public cbSize As UInteger
        Public hIcon As IntPtr
        Public iSysIconIndex As Integer
        Public iIcon As Integer
        <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=260)>
        Public szPath As String
    End Structure



    Declare Function SHGetStockIconInfo Lib "Shell32.dll" (ssid As UInteger, uFlags As UInteger, ByRef pssi As SHSTOCKICONINFO) As Integer

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

        If My.Settings.links <> txtQuickLaunchPath.Text Then
            If Not FileIO.FileSystem.DirectoryExists(txtQuickLaunchPath.Text) Then
                MessageBox.Show($"Directory {txtQuickLaunchPath.Text} does not exist!", "Error")
                Exit Sub
            End If
        End If

        My.Settings.links = txtQuickLaunchPath.Text
        FrmMain.UpdateWatchers(My.Settings.links)

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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnOpenFolderDialog.Click
        txtQuickLaunchPath.SuspendLayout()
        txtQuickLaunchPath.Text = ChangeLinksDir(My.Settings.links)
        txtQuickLaunchPath.SelectionStart = txtQuickLaunchPath.TextLength
        txtQuickLaunchPath.ResumeLayout()
    End Sub

    Private Function ChangeLinksDir(current As String) As String
        Debug.Print("changeLinksDir")
        Me.TopMost = False


        'Using fb As New FolderBrowserDialog
        Try
            Using fb As New Ookii.Dialogs.WinForms.VistaFolderBrowserDialog
                fb.Description = "Select Folder Containing Your Shortcuts - ScalA"
                fb.UseDescriptionForTitle = True
                fb.ShowNewFolderButton = False
                fb.RootFolder = Environment.SpecialFolder.Desktop
                fb.SelectedPath = current

                If fb.ShowDialog() = DialogResult.OK Then
                    ' Warning for Root folder with throw for dialog cancel
                    If fb.SelectedPath = System.IO.Path.GetPathRoot(fb.SelectedPath) AndAlso
                        MessageBox.Show("Warning: Selecting a root path is not recommended" & vbCrLf &
                                        $"Are you sure you want to use {fb.SelectedPath}?", "Warning",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.No Then Throw New Exception("dummy")
                    'If My.Settings.links <> fb.SelectedPath Then
                    '    My.Settings.links = fb.SelectedPath
                    '    FrmMain.UpdateWatchers(My.Settings.links)
                    'Else
                    Return fb.SelectedPath
                    'End If
                End If
            End Using
        Catch
        Finally
            Me.TopMost = My.Settings.topmost
        End Try
        Return current
    End Function

    Private Sub txtQuickLaunchPath_DoubleClick(sender As Object, e As EventArgs) Handles txtQuickLaunchPath.DoubleClick
        txtQuickLaunchPath.SelectionStart = 0
        txtQuickLaunchPath.SelectionLength = txtQuickLaunchPath.TextLength
    End Sub
End Class