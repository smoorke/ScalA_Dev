<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLauncherTemplateEdit
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
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblExePath = New System.Windows.Forms.Label()
        Me.txtExePath = New System.Windows.Forms.TextBox()
        Me.btnBrowseExe = New System.Windows.Forms.Button()
        Me.lblWorkDir = New System.Windows.Forms.Label()
        Me.txtWorkDir = New System.Windows.Forms.TextBox()
        Me.btnBrowseWorkDir = New System.Windows.Forms.Button()
        Me.grpResolution = New System.Windows.Forms.GroupBox()
        Me.numHeight = New System.Windows.Forms.NumericUpDown()
        Me.lblX = New System.Windows.Forms.Label()
        Me.numWidth = New System.Windows.Forms.NumericUpDown()
        Me.lblWidth = New System.Windows.Forms.Label()
        Me.lblOptions = New System.Windows.Forms.Label()
        Me.txtOptions = New System.Windows.Forms.TextBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.ofdExe = New System.Windows.Forms.OpenFileDialog()
        Me.fbdWorkDir = New System.Windows.Forms.FolderBrowserDialog()
        Me.grpResolution.SuspendLayout()
        CType(Me.numHeight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numWidth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(12, 15)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(38, 13)
        Me.lblName.TabIndex = 0
        Me.lblName.Text = "Name:"
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(80, 12)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(242, 20)
        Me.txtName.TabIndex = 1
        '
        'lblExePath
        '
        Me.lblExePath.AutoSize = True
        Me.lblExePath.Location = New System.Drawing.Point(12, 41)
        Me.lblExePath.Name = "lblExePath"
        Me.lblExePath.Size = New System.Drawing.Size(53, 13)
        Me.lblExePath.TabIndex = 2
        Me.lblExePath.Text = "Exe Path:"
        '
        'txtExePath
        '
        Me.txtExePath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtExePath.Location = New System.Drawing.Point(80, 38)
        Me.txtExePath.Name = "txtExePath"
        Me.txtExePath.Size = New System.Drawing.Size(207, 20)
        Me.txtExePath.TabIndex = 3
        '
        'btnBrowseExe
        '
        Me.btnBrowseExe.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseExe.Location = New System.Drawing.Point(293, 36)
        Me.btnBrowseExe.Name = "btnBrowseExe"
        Me.btnBrowseExe.Size = New System.Drawing.Size(29, 23)
        Me.btnBrowseExe.TabIndex = 4
        Me.btnBrowseExe.Text = "..."
        Me.btnBrowseExe.UseVisualStyleBackColor = True
        '
        'lblWorkDir
        '
        Me.lblWorkDir.AutoSize = True
        Me.lblWorkDir.Location = New System.Drawing.Point(12, 67)
        Me.lblWorkDir.Name = "lblWorkDir"
        Me.lblWorkDir.Size = New System.Drawing.Size(52, 13)
        Me.lblWorkDir.TabIndex = 5
        Me.lblWorkDir.Text = "Work Dir:"
        '
        'txtWorkDir
        '
        Me.txtWorkDir.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWorkDir.Location = New System.Drawing.Point(80, 64)
        Me.txtWorkDir.Name = "txtWorkDir"
        Me.txtWorkDir.Size = New System.Drawing.Size(207, 20)
        Me.txtWorkDir.TabIndex = 6
        '
        'btnBrowseWorkDir
        '
        Me.btnBrowseWorkDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseWorkDir.Location = New System.Drawing.Point(293, 62)
        Me.btnBrowseWorkDir.Name = "btnBrowseWorkDir"
        Me.btnBrowseWorkDir.Size = New System.Drawing.Size(29, 23)
        Me.btnBrowseWorkDir.TabIndex = 7
        Me.btnBrowseWorkDir.Text = "..."
        Me.btnBrowseWorkDir.UseVisualStyleBackColor = True
        '
        'grpResolution
        '
        Me.grpResolution.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpResolution.Controls.Add(Me.numHeight)
        Me.grpResolution.Controls.Add(Me.lblX)
        Me.grpResolution.Controls.Add(Me.numWidth)
        Me.grpResolution.Controls.Add(Me.lblWidth)
        Me.grpResolution.Location = New System.Drawing.Point(12, 90)
        Me.grpResolution.Name = "grpResolution"
        Me.grpResolution.Size = New System.Drawing.Size(310, 50)
        Me.grpResolution.TabIndex = 8
        Me.grpResolution.TabStop = False
        Me.grpResolution.Text = "Default Resolution"
        '
        'numHeight
        '
        Me.numHeight.Location = New System.Drawing.Point(155, 19)
        Me.numHeight.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.numHeight.Minimum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.numHeight.Name = "numHeight"
        Me.numHeight.Size = New System.Drawing.Size(60, 20)
        Me.numHeight.TabIndex = 3
        Me.numHeight.Value = New Decimal(New Integer() {600, 0, 0, 0})
        '
        'lblX
        '
        Me.lblX.AutoSize = True
        Me.lblX.Location = New System.Drawing.Point(137, 21)
        Me.lblX.Name = "lblX"
        Me.lblX.Size = New System.Drawing.Size(12, 13)
        Me.lblX.TabIndex = 2
        Me.lblX.Text = "x"
        '
        'numWidth
        '
        Me.numWidth.Location = New System.Drawing.Point(68, 19)
        Me.numWidth.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.numWidth.Minimum = New Decimal(New Integer() {400, 0, 0, 0})
        Me.numWidth.Name = "numWidth"
        Me.numWidth.Size = New System.Drawing.Size(60, 20)
        Me.numWidth.TabIndex = 1
        Me.numWidth.Value = New Decimal(New Integer() {800, 0, 0, 0})
        '
        'lblWidth
        '
        Me.lblWidth.AutoSize = True
        Me.lblWidth.Location = New System.Drawing.Point(6, 21)
        Me.lblWidth.Name = "lblWidth"
        Me.lblWidth.Size = New System.Drawing.Size(57, 13)
        Me.lblWidth.TabIndex = 0
        Me.lblWidth.Text = "Width x H:"
        '
        'lblOptions
        '
        Me.lblOptions.AutoSize = True
        Me.lblOptions.Location = New System.Drawing.Point(12, 149)
        Me.lblOptions.Name = "lblOptions"
        Me.lblOptions.Size = New System.Drawing.Size(64, 13)
        Me.lblOptions.TabIndex = 9
        Me.lblOptions.Text = "Options (-o):"
        '
        'txtOptions
        '
        Me.txtOptions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOptions.Location = New System.Drawing.Point(80, 146)
        Me.txtOptions.Name = "txtOptions"
        Me.txtOptions.Size = New System.Drawing.Size(100, 20)
        Me.txtOptions.TabIndex = 10
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(166, 180)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 11
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(247, 180)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'ofdExe
        '
        Me.ofdExe.Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*"
        Me.ofdExe.Title = "Select Astonia Executable"
        '
        'fbdWorkDir
        '
        Me.fbdWorkDir.Description = "Select Working Directory"
        '
        'frmLauncherTemplateEdit
        '
        Me.AcceptButton = Me.btnSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(334, 215)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtOptions)
        Me.Controls.Add(Me.lblOptions)
        Me.Controls.Add(Me.grpResolution)
        Me.Controls.Add(Me.btnBrowseWorkDir)
        Me.Controls.Add(Me.txtWorkDir)
        Me.Controls.Add(Me.lblWorkDir)
        Me.Controls.Add(Me.btnBrowseExe)
        Me.Controls.Add(Me.txtExePath)
        Me.Controls.Add(Me.lblExePath)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.lblName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLauncherTemplateEdit"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Edit Launcher Template"
        Me.grpResolution.ResumeLayout(False)
        Me.grpResolution.PerformLayout()
        CType(Me.numHeight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numWidth, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblName As Label
    Friend WithEvents txtName As TextBox
    Friend WithEvents lblExePath As Label
    Friend WithEvents txtExePath As TextBox
    Friend WithEvents btnBrowseExe As Button
    Friend WithEvents lblWorkDir As Label
    Friend WithEvents txtWorkDir As TextBox
    Friend WithEvents btnBrowseWorkDir As Button
    Friend WithEvents grpResolution As GroupBox
    Friend WithEvents numHeight As NumericUpDown
    Friend WithEvents lblX As Label
    Friend WithEvents numWidth As NumericUpDown
    Friend WithEvents lblWidth As Label
    Friend WithEvents lblOptions As Label
    Friend WithEvents txtOptions As TextBox
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents ofdExe As OpenFileDialog
    Friend WithEvents fbdWorkDir As FolderBrowserDialog

End Class
