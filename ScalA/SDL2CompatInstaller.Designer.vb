<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SDL2CompatInstaller
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.lblPath = New System.Windows.Forms.Label()
        Me.txtPath = New System.Windows.Forms.TextBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.btnInstall = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.progressBar = New System.Windows.Forms.ProgressBar()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.linkGitHub = New System.Windows.Forms.LinkLabel()
        Me.chkBackup = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.Location = New System.Drawing.Point(12, 9)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(410, 55)
        Me.lblInfo.TabIndex = 0
        Me.lblInfo.Text = "SDL2-compat is a compatibility layer that allows SDL1 games to run using SDL2." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This can improve performance and fix issues on modern systems." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Select the game directory where SDL2.dll should be installed:"
        '
        'lblPath
        '
        Me.lblPath.AutoSize = True
        Me.lblPath.Location = New System.Drawing.Point(12, 73)
        Me.lblPath.Name = "lblPath"
        Me.lblPath.Size = New System.Drawing.Size(84, 13)
        Me.lblPath.TabIndex = 1
        Me.lblPath.Text = "Target Directory:"
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(12, 89)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(350, 20)
        Me.txtPath.TabIndex = 2
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(368, 87)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(54, 23)
        Me.btnBrowse.TabIndex = 3
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'btnInstall
        '
        Me.btnInstall.Location = New System.Drawing.Point(266, 165)
        Me.btnInstall.Name = "btnInstall"
        Me.btnInstall.Size = New System.Drawing.Size(75, 23)
        Me.btnInstall.TabIndex = 4
        Me.btnInstall.Text = "Install"
        Me.btnInstall.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(347, 165)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'progressBar
        '
        Me.progressBar.Location = New System.Drawing.Point(12, 136)
        Me.progressBar.Name = "progressBar"
        Me.progressBar.Size = New System.Drawing.Size(410, 23)
        Me.progressBar.TabIndex = 6
        Me.progressBar.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(12, 120)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(0, 13)
        Me.lblStatus.TabIndex = 7
        '
        'linkGitHub
        '
        Me.linkGitHub.AutoSize = True
        Me.linkGitHub.Location = New System.Drawing.Point(12, 170)
        Me.linkGitHub.Name = "linkGitHub"
        Me.linkGitHub.Size = New System.Drawing.Size(108, 13)
        Me.linkGitHub.TabIndex = 8
        Me.linkGitHub.TabStop = True
        Me.linkGitHub.Text = "SDL2-compat GitHub"
        '
        'chkBackup
        '
        Me.chkBackup.AutoSize = True
        Me.chkBackup.Checked = True
        Me.chkBackup.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkBackup.Location = New System.Drawing.Point(268, 115)
        Me.chkBackup.Name = "chkBackup"
        Me.chkBackup.Size = New System.Drawing.Size(154, 17)
        Me.chkBackup.TabIndex = 9
        Me.chkBackup.Text = "Backup existing SDL2.dll"
        Me.chkBackup.UseVisualStyleBackColor = True
        '
        'SDL2CompatInstaller
        '
        Me.AcceptButton = Me.btnInstall
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(434, 196)
        Me.Controls.Add(Me.chkBackup)
        Me.Controls.Add(Me.linkGitHub)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.progressBar)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnInstall)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.lblPath)
        Me.Controls.Add(Me.lblInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SDL2CompatInstaller"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "SDL2-compat Installer"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents lblPath As System.Windows.Forms.Label
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents btnInstall As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents progressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents linkGitHub As System.Windows.Forms.LinkLabel
    Friend WithEvents chkBackup As System.Windows.Forms.CheckBox
End Class
