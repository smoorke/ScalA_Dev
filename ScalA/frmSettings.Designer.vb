﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmSettings
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
        Dim Label2 As System.Windows.Forms.Label
        Dim Label1 As System.Windows.Forms.Label
        Dim GroupBox1 As System.Windows.Forms.GroupBox
        Dim Label3 As System.Windows.Forms.Label
        Dim GroupBox2 As System.Windows.Forms.GroupBox
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmSettings))
        Me.txtResolutions = New System.Windows.Forms.TextBox()
        Me.btnOpenFolderDialog = New System.Windows.Forms.Button()
        Me.cmsQLFolder = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenInExplorerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtQuickLaunchPath = New System.Windows.Forms.TextBox()
        Me.chkTopMost = New System.Windows.Forms.CheckBox()
        Me.grpAlign = New System.Windows.Forms.GroupBox()
        Me.btnResetAlign = New System.Windows.Forms.Button()
        Me.numYoffset = New System.Windows.Forms.NumericUpDown()
        Me.numXoffset = New System.Windows.Forms.NumericUpDown()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.chkDoAlign = New System.Windows.Forms.CheckBox()
        Me.tmrAlign = New System.Windows.Forms.Timer(Me.components)
        Me.txtExe = New System.Windows.Forms.TextBox()
        Me.txtClass = New System.Windows.Forms.TextBox()
        Me.chkRoundCorners = New System.Windows.Forms.CheckBox()
        Me.chkOverViewIsGame = New System.Windows.Forms.CheckBox()
        Me.ttSettings = New System.Windows.Forms.ToolTip(Me.components)
        Label2 = New System.Windows.Forms.Label()
        Label1 = New System.Windows.Forms.Label()
        GroupBox1 = New System.Windows.Forms.GroupBox()
        Label3 = New System.Windows.Forms.Label()
        GroupBox2 = New System.Windows.Forms.GroupBox()
        GroupBox1.SuspendLayout()
        GroupBox2.SuspendLayout()
        Me.cmsQLFolder.SuspendLayout()
        Me.grpAlign.SuspendLayout()
        CType(Me.numYoffset, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numXoffset, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label2
        '
        Label2.AutoSize = True
        Label2.Location = New System.Drawing.Point(6, 41)
        Label2.Name = "Label2"
        Label2.Size = New System.Drawing.Size(14, 13)
        Label2.TabIndex = 7
        Label2.Text = "Y"
        '
        'Label1
        '
        Label1.AutoSize = True
        Label1.Location = New System.Drawing.Point(6, 20)
        Label1.Name = "Label1"
        Label1.Size = New System.Drawing.Size(14, 13)
        Label1.TabIndex = 6
        Label1.Text = "X"
        '
        'GroupBox1
        '
        GroupBox1.Controls.Add(Me.txtResolutions)
        GroupBox1.Location = New System.Drawing.Point(115, 50)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New System.Drawing.Size(107, 148)
        GroupBox1.TabIndex = 9
        GroupBox1.TabStop = False
        GroupBox1.Text = "Resolutions"
        '
        'txtResolutions
        '
        Me.txtResolutions.Location = New System.Drawing.Point(6, 19)
        Me.txtResolutions.Multiline = True
        Me.txtResolutions.Name = "txtResolutions"
        Me.txtResolutions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtResolutions.Size = New System.Drawing.Size(97, 123)
        Me.txtResolutions.TabIndex = 0
        Me.txtResolutions.Text = "800x600" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1200x900" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1600x1200" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2000x1500" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2400x1800" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2800x2100" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3200x2400" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3600x27" &
    "00" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "4000x3000" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "4400x3300"
        '
        'Label3
        '
        Label3.AutoSize = True
        Label3.Location = New System.Drawing.Point(80, 92)
        Label3.Name = "Label3"
        Label3.Size = New System.Drawing.Size(27, 13)
        Label3.TabIndex = 11
        Label3.Text = ".exe"
        '
        'GroupBox2
        '
        GroupBox2.Controls.Add(Me.btnOpenFolderDialog)
        GroupBox2.Controls.Add(Me.txtQuickLaunchPath)
        GroupBox2.Location = New System.Drawing.Point(9, 3)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New System.Drawing.Size(213, 42)
        GroupBox2.TabIndex = 1
        GroupBox2.TabStop = False
        GroupBox2.Text = "QuickLaunch Path"
        '
        'btnOpenFolderDialog
        '
        Me.btnOpenFolderDialog.ContextMenuStrip = Me.cmsQLFolder
        Me.btnOpenFolderDialog.FlatAppearance.BorderSize = 0
        Me.btnOpenFolderDialog.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOpenFolderDialog.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOpenFolderDialog.Location = New System.Drawing.Point(187, 16)
        Me.btnOpenFolderDialog.Name = "btnOpenFolderDialog"
        Me.btnOpenFolderDialog.Size = New System.Drawing.Size(22, 20)
        Me.btnOpenFolderDialog.TabIndex = 13
        Me.btnOpenFolderDialog.Text = ".."
        Me.btnOpenFolderDialog.UseVisualStyleBackColor = True
        '
        'cmsQLFolder
        '
        Me.cmsQLFolder.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenInExplorerToolStripMenuItem})
        Me.cmsQLFolder.Name = "cmsQLFolder"
        Me.cmsQLFolder.Size = New System.Drawing.Size(184, 26)
        '
        'OpenInExplorerToolStripMenuItem
        '
        Me.OpenInExplorerToolStripMenuItem.Name = "OpenInExplorerToolStripMenuItem"
        Me.OpenInExplorerToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.OpenInExplorerToolStripMenuItem.Text = "Open in File Explorer"
        '
        'txtQuickLaunchPath
        '
        Me.txtQuickLaunchPath.Location = New System.Drawing.Point(5, 16)
        Me.txtQuickLaunchPath.Name = "txtQuickLaunchPath"
        Me.txtQuickLaunchPath.Size = New System.Drawing.Size(182, 20)
        Me.txtQuickLaunchPath.TabIndex = 0
        Me.txtQuickLaunchPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkTopMost
        '
        Me.chkTopMost.AutoSize = True
        Me.chkTopMost.Location = New System.Drawing.Point(9, 72)
        Me.chkTopMost.Name = "chkTopMost"
        Me.chkTopMost.Size = New System.Drawing.Size(98, 17)
        Me.chkTopMost.TabIndex = 0
        Me.chkTopMost.Text = "Always On Top"
        Me.chkTopMost.UseVisualStyleBackColor = True
        '
        'grpAlign
        '
        Me.grpAlign.Controls.Add(Me.btnResetAlign)
        Me.grpAlign.Controls.Add(Me.numYoffset)
        Me.grpAlign.Controls.Add(Me.numXoffset)
        Me.grpAlign.Controls.Add(Label2)
        Me.grpAlign.Controls.Add(Label1)
        Me.grpAlign.Enabled = False
        Me.grpAlign.Location = New System.Drawing.Point(9, 136)
        Me.grpAlign.Name = "grpAlign"
        Me.grpAlign.Size = New System.Drawing.Size(100, 62)
        Me.grpAlign.TabIndex = 1
        Me.grpAlign.TabStop = False
        '
        'btnResetAlign
        '
        Me.btnResetAlign.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnResetAlign.Location = New System.Drawing.Point(65, -1)
        Me.btnResetAlign.Name = "btnResetAlign"
        Me.btnResetAlign.Size = New System.Drawing.Size(33, 17)
        Me.btnResetAlign.TabIndex = 10
        Me.btnResetAlign.Text = "Reset"
        Me.btnResetAlign.UseVisualStyleBackColor = True
        '
        'numYoffset
        '
        Me.numYoffset.Location = New System.Drawing.Point(26, 39)
        Me.numYoffset.Maximum = New Decimal(New Integer() {4000, 0, 0, 0})
        Me.numYoffset.Minimum = New Decimal(New Integer() {4000, 0, 0, -2147483648})
        Me.numYoffset.Name = "numYoffset"
        Me.numYoffset.Size = New System.Drawing.Size(72, 20)
        Me.numYoffset.TabIndex = 9
        Me.numYoffset.Tag = "1"
        Me.numYoffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'numXoffset
        '
        Me.numXoffset.Location = New System.Drawing.Point(26, 17)
        Me.numXoffset.Maximum = New Decimal(New Integer() {4000, 0, 0, 0})
        Me.numXoffset.Minimum = New Decimal(New Integer() {4000, 0, 0, -2147483648})
        Me.numXoffset.Name = "numXoffset"
        Me.numXoffset.Size = New System.Drawing.Size(72, 20)
        Me.numXoffset.TabIndex = 8
        Me.numXoffset.Tag = "0"
        Me.numXoffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(62, 200)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(77, 23)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(145, 200)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(77, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'chkDoAlign
        '
        Me.chkDoAlign.AutoSize = True
        Me.chkDoAlign.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDoAlign.Location = New System.Drawing.Point(9, 136)
        Me.chkDoAlign.Name = "chkDoAlign"
        Me.chkDoAlign.Size = New System.Drawing.Size(66, 16)
        Me.chkDoAlign.TabIndex = 5
        Me.chkDoAlign.Text = "Alignment"
        Me.chkDoAlign.UseVisualStyleBackColor = True
        '
        'tmrAlign
        '
        '
        'txtExe
        '
        Me.txtExe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExe.Location = New System.Drawing.Point(9, 89)
        Me.txtExe.Name = "txtExe"
        Me.txtExe.Size = New System.Drawing.Size(66, 20)
        Me.txtExe.TabIndex = 1
        Me.txtExe.Text = "moac | new"
        '
        'txtClass
        '
        Me.txtClass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClass.Location = New System.Drawing.Point(9, 111)
        Me.txtClass.Name = "txtClass"
        Me.txtClass.Size = New System.Drawing.Size(100, 20)
        Me.txtClass.TabIndex = 10
        Me.txtClass.Text = "MAINWNDMOAC | 䅍义乗䵄䅏C"
        '
        'chkRoundCorners
        '
        Me.chkRoundCorners.AutoSize = True
        Me.chkRoundCorners.Location = New System.Drawing.Point(9, 58)
        Me.chkRoundCorners.Name = "chkRoundCorners"
        Me.chkRoundCorners.Size = New System.Drawing.Size(109, 17)
        Me.chkRoundCorners.TabIndex = 12
        Me.chkRoundCorners.Text = "Rounded Corners"
        Me.chkRoundCorners.UseVisualStyleBackColor = True
        '
        'chkOverViewIsGame
        '
        Me.chkOverViewIsGame.AutoSize = True
        Me.chkOverViewIsGame.Location = New System.Drawing.Point(9, 44)
        Me.chkOverViewIsGame.Name = "chkOverViewIsGame"
        Me.chkOverViewIsGame.Size = New System.Drawing.Size(107, 17)
        Me.chkOverViewIsGame.TabIndex = 13
        Me.chkOverViewIsGame.Text = "Active Overview "
        Me.ttSettings.SetToolTip(Me.chkOverViewIsGame, "When this is enabled" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "the overview thumbnails" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "will function as game.")
        Me.chkOverViewIsGame.UseVisualStyleBackColor = True
        '
        'FrmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(228, 228)
        Me.Controls.Add(Me.txtExe)
        Me.Controls.Add(Me.chkTopMost)
        Me.Controls.Add(GroupBox1)
        Me.Controls.Add(Me.chkRoundCorners)
        Me.Controls.Add(GroupBox2)
        Me.Controls.Add(Label3)
        Me.Controls.Add(Me.txtClass)
        Me.Controls.Add(Me.chkDoAlign)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.grpAlign)
        Me.Controls.Add(Me.chkOverViewIsGame)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FrmSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ScalA Settings"
        Me.TopMost = True
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        GroupBox2.ResumeLayout(False)
        GroupBox2.PerformLayout()
        Me.cmsQLFolder.ResumeLayout(False)
        Me.grpAlign.ResumeLayout(False)
        Me.grpAlign.PerformLayout()
        CType(Me.numYoffset, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numXoffset, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents chkTopMost As CheckBox
    Friend WithEvents grpAlign As GroupBox
    Friend WithEvents numYoffset As NumericUpDown
    Friend WithEvents numXoffset As NumericUpDown
    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents tmrAlign As Timer
    Friend WithEvents chkDoAlign As CheckBox
    Friend WithEvents txtResolutions As TextBox
    Friend WithEvents btnResetAlign As Button
    Friend WithEvents txtExe As TextBox
    Friend WithEvents txtClass As TextBox
    Friend WithEvents txtQuickLaunchPath As TextBox
    Friend WithEvents btnOpenFolderDialog As Button
    Friend WithEvents cmsQLFolder As ContextMenuStrip
    Friend WithEvents OpenInExplorerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents chkRoundCorners As CheckBox
    Friend WithEvents chkOverViewIsGame As CheckBox
    Friend WithEvents ttSettings As ToolTip
End Class
