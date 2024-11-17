<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOverlay
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.pbRestart = New System.Windows.Forms.PictureBox()
        Me.ttOverlay = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.pbRestart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pbRestart
        '
        Me.pbRestart.BackColor = System.Drawing.Color.Transparent
        Me.pbRestart.BackgroundImage = Global.ScalA.My.Resources.Resources.Refresh
        Me.pbRestart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pbRestart.Location = New System.Drawing.Point(671, 7)
        Me.pbRestart.Margin = New System.Windows.Forms.Padding(0)
        Me.pbRestart.Name = "pbRestart"
        Me.pbRestart.Size = New System.Drawing.Size(24, 24)
        Me.pbRestart.TabIndex = 24
        Me.pbRestart.TabStop = False
        Me.ttOverlay.SetToolTip(Me.pbRestart, "Restart Client")
        Me.pbRestart.Visible = False
        '
        'frmOverlay
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.Controls.Add(Me.pbRestart)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmOverlay"
        Me.ShowInTaskbar = False
        Me.Text = "Overlay"
        Me.TransparencyKey = System.Drawing.Color.Black
        CType(Me.pbRestart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pbRestart As PictureBox
    Friend WithEvents ttOverlay As ToolTip
End Class
