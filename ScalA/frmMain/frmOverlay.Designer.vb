﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.cmsRestart = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.HideThisToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ttOverlay = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.RestartWoClosingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.pbRestart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmsRestart.SuspendLayout()
        Me.SuspendLayout()
        '
        'pbRestart
        '
        Me.pbRestart.BackColor = System.Drawing.Color.Transparent
        Me.pbRestart.BackgroundImage = Global.ScalA.My.Resources.Resources.RefreshB
        Me.pbRestart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pbRestart.ContextMenuStrip = Me.cmsRestart
        Me.pbRestart.Location = New System.Drawing.Point(671, 7)
        Me.pbRestart.Margin = New System.Windows.Forms.Padding(0)
        Me.pbRestart.Name = "pbRestart"
        Me.pbRestart.Size = New System.Drawing.Size(24, 24)
        Me.pbRestart.TabIndex = 24
        Me.pbRestart.TabStop = False
        Me.ttOverlay.SetToolTip(Me.pbRestart, "Restart Client")
        Me.pbRestart.Visible = False
        '
        'cmsRestart
        '
        Me.cmsRestart.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RestartWoClosingToolStripMenuItem, Me.ToolStripMenuItem1, Me.HideThisToolStripMenuItem})
        Me.cmsRestart.Name = "cmsRestartHide"
        Me.cmsRestart.ShowImageMargin = False
        Me.cmsRestart.Size = New System.Drawing.Size(156, 76)
        '
        'HideThisToolStripMenuItem
        '
        Me.HideThisToolStripMenuItem.Name = "HideThisToolStripMenuItem"
        Me.HideThisToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.HideThisToolStripMenuItem.Text = "Hide This Button"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(152, 6)
        '
        'RestartWoClosingToolStripMenuItem
        '
        Me.RestartWoClosingToolStripMenuItem.Name = "RestartWoClosingToolStripMenuItem"
        Me.RestartWoClosingToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.RestartWoClosingToolStripMenuItem.Text = "Restart w/o Closing"
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
        Me.cmsRestart.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pbRestart As PictureBox
    Friend WithEvents ttOverlay As ToolTip
    Friend WithEvents cmsRestart As ContextMenuStrip
    Friend WithEvents HideThisToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RestartWoClosingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
End Class
