﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDebug
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
        Me.components = New System.ComponentModel.Container()
        Me.txtDebugLog = New System.Windows.Forms.TextBox()
        Me.tmrDebug = New System.Windows.Forms.Timer(Me.components)
        Me.chkAutoScroll = New System.Windows.Forms.CheckBox()
        Me.ssDebug = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel0 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabe3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btnMonInfo = New System.Windows.Forms.Button()
        Me.chkPollDPI = New System.Windows.Forms.CheckBox()
        Me.btnAltInfo = New System.Windows.Forms.Button()
        Me.ssDebug.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtDebugLog
        '
        Me.txtDebugLog.Location = New System.Drawing.Point(0, 0)
        Me.txtDebugLog.Multiline = True
        Me.txtDebugLog.Name = "txtDebugLog"
        Me.txtDebugLog.ReadOnly = True
        Me.txtDebugLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtDebugLog.Size = New System.Drawing.Size(663, 396)
        Me.txtDebugLog.TabIndex = 0
        Me.txtDebugLog.WordWrap = False
        '
        'tmrDebug
        '
        Me.tmrDebug.Enabled = True
        Me.tmrDebug.Interval = 250
        '
        'chkAutoScroll
        '
        Me.chkAutoScroll.AutoSize = True
        Me.chkAutoScroll.Checked = True
        Me.chkAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAutoScroll.Location = New System.Drawing.Point(669, 379)
        Me.chkAutoScroll.Name = "chkAutoScroll"
        Me.chkAutoScroll.Size = New System.Drawing.Size(74, 17)
        Me.chkAutoScroll.TabIndex = 1
        Me.chkAutoScroll.Text = "AutoScroll"
        Me.chkAutoScroll.UseVisualStyleBackColor = True
        '
        'ssDebug
        '
        Me.ssDebug.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel0, Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel2, Me.ToolStripStatusLabe3})
        Me.ssDebug.Location = New System.Drawing.Point(0, 428)
        Me.ssDebug.Name = "ssDebug"
        Me.ssDebug.Size = New System.Drawing.Size(748, 22)
        Me.ssDebug.SizingGrip = False
        Me.ssDebug.TabIndex = 2
        Me.ssDebug.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel0
        '
        Me.ToolStripStatusLabel0.Name = "ToolStripStatusLabel0"
        Me.ToolStripStatusLabel0.Size = New System.Drawing.Size(25, 17)
        Me.ToolStripStatusLabel0.Text = "DPI"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(27, 17)
        Me.ToolStripStatusLabel1.Text = "SDL"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(29, 17)
        Me.ToolStripStatusLabel2.Text = "AS+"
        '
        'ToolStripStatusLabe3
        '
        Me.ToolStripStatusLabe3.Name = "ToolStripStatusLabe3"
        Me.ToolStripStatusLabe3.Size = New System.Drawing.Size(32, 17)
        Me.ToolStripStatusLabe3.Text = "WS+"
        '
        'btnMonInfo
        '
        Me.btnMonInfo.Location = New System.Drawing.Point(73, 402)
        Me.btnMonInfo.Name = "btnMonInfo"
        Me.btnMonInfo.Size = New System.Drawing.Size(75, 23)
        Me.btnMonInfo.TabIndex = 3
        Me.btnMonInfo.Text = "MonInfo"
        Me.btnMonInfo.UseVisualStyleBackColor = True
        '
        'chkPollDPI
        '
        Me.chkPollDPI.AutoSize = True
        Me.chkPollDPI.Location = New System.Drawing.Point(12, 406)
        Me.chkPollDPI.Name = "chkPollDPI"
        Me.chkPollDPI.Size = New System.Drawing.Size(61, 17)
        Me.chkPollDPI.TabIndex = 4
        Me.chkPollDPI.Text = "PollDPI"
        Me.chkPollDPI.UseVisualStyleBackColor = True
        '
        'btnAltInfo
        '
        Me.btnAltInfo.Location = New System.Drawing.Point(154, 402)
        Me.btnAltInfo.Name = "btnAltInfo"
        Me.btnAltInfo.Size = New System.Drawing.Size(75, 23)
        Me.btnAltInfo.TabIndex = 5
        Me.btnAltInfo.Text = "AltInfo"
        Me.btnAltInfo.UseVisualStyleBackColor = True
        '
        'frmDebug
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(748, 450)
        Me.Controls.Add(Me.btnAltInfo)
        Me.Controls.Add(Me.btnMonInfo)
        Me.Controls.Add(Me.chkPollDPI)
        Me.Controls.Add(Me.ssDebug)
        Me.Controls.Add(Me.chkAutoScroll)
        Me.Controls.Add(Me.txtDebugLog)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmDebug"
        Me.Text = "ScalA Debug Window"
        Me.ssDebug.ResumeLayout(False)
        Me.ssDebug.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtDebugLog As TextBox
    Friend WithEvents tmrDebug As Timer
    Friend WithEvents chkAutoScroll As CheckBox
    Friend WithEvents ssDebug As StatusStrip
    Friend WithEvents ToolStripStatusLabel0 As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabe3 As ToolStripStatusLabel
    Friend WithEvents btnMonInfo As Button
    Friend WithEvents chkPollDPI As CheckBox
    Friend WithEvents btnAltInfo As Button
End Class
