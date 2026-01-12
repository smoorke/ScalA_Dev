<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLauncherSetup
    Inherits System.Windows.Forms.Form

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

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.launcherSetupControl1 = New ScalA.LauncherSetupControl()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'launcherSetupControl1
        '
        Me.launcherSetupControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.launcherSetupControl1.Location = New System.Drawing.Point(0, 0)
        Me.launcherSetupControl1.Name = "launcherSetupControl1"
        Me.launcherSetupControl1.Size = New System.Drawing.Size(414, 420)
        Me.launcherSetupControl1.TabIndex = 0
        Me.launcherSetupControl1.TargetFolder = ""
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(327, 426)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmLauncherSetup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(414, 461)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.launcherSetupControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLauncherSetup"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Launcher Setup"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents launcherSetupControl1 As LauncherSetupControl
    Friend WithEvents btnClose As Button

End Class
