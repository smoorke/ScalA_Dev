<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class BatchShortcutManager
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
        Me.grpFolder = New System.Windows.Forms.GroupBox()
        Me.chkRecursive = New System.Windows.Forms.CheckBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.txtFolderPath = New System.Windows.Forms.TextBox()
        Me.grpPassword = New System.Windows.Forms.GroupBox()
        Me.txtPasswordNew = New System.Windows.Forms.TextBox()
        Me.lblPasswordArrow = New System.Windows.Forms.Label()
        Me.txtPasswordOld = New System.Windows.Forms.TextBox()
        Me.lblPasswordNew = New System.Windows.Forms.Label()
        Me.lblPasswordOld = New System.Windows.Forms.Label()
        Me.chkPassword = New System.Windows.Forms.CheckBox()
        Me.grpLaunchOption = New System.Windows.Forms.GroupBox()
        Me.lblOptionHint = New System.Windows.Forms.Label()
        Me.txtOptionNew = New System.Windows.Forms.TextBox()
        Me.lblOptionArrow = New System.Windows.Forms.Label()
        Me.txtOptionOld = New System.Windows.Forms.TextBox()
        Me.lblOptionNew = New System.Windows.Forms.Label()
        Me.lblOptionOld = New System.Windows.Forms.Label()
        Me.chkOption = New System.Windows.Forms.CheckBox()
        Me.grpResolution = New System.Windows.Forms.GroupBox()
        Me.numNewHeight = New System.Windows.Forms.NumericUpDown()
        Me.numNewWidth = New System.Windows.Forms.NumericUpDown()
        Me.lblNewCustom = New System.Windows.Forms.Label()
        Me.numOldHeight = New System.Windows.Forms.NumericUpDown()
        Me.numOldWidth = New System.Windows.Forms.NumericUpDown()
        Me.lblOldCustom = New System.Windows.Forms.Label()
        Me.cboResolutionNew = New System.Windows.Forms.ComboBox()
        Me.lblResArrow = New System.Windows.Forms.Label()
        Me.cboResolutionOld = New System.Windows.Forms.ComboBox()
        Me.lblResNew = New System.Windows.Forms.Label()
        Me.lblResOld = New System.Windows.Forms.Label()
        Me.chkResolution = New System.Windows.Forms.CheckBox()
        Me.btnPreview = New System.Windows.Forms.Button()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.grpFolder.SuspendLayout()
        Me.grpPassword.SuspendLayout()
        Me.grpLaunchOption.SuspendLayout()
        Me.grpResolution.SuspendLayout()
        CType(Me.numNewHeight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numNewWidth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numOldHeight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numOldWidth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpFolder
        '
        Me.grpFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpFolder.Controls.Add(Me.chkRecursive)
        Me.grpFolder.Controls.Add(Me.btnBrowse)
        Me.grpFolder.Controls.Add(Me.txtFolderPath)
        Me.grpFolder.Location = New System.Drawing.Point(12, 12)
        Me.grpFolder.Name = "grpFolder"
        Me.grpFolder.Size = New System.Drawing.Size(360, 68)
        Me.grpFolder.TabIndex = 0
        Me.grpFolder.TabStop = False
        Me.grpFolder.Text = "Target Folder"
        '
        'chkRecursive
        '
        Me.chkRecursive.AutoSize = True
        Me.chkRecursive.Checked = True
        Me.chkRecursive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRecursive.Location = New System.Drawing.Point(6, 45)
        Me.chkRecursive.Name = "chkRecursive"
        Me.chkRecursive.Size = New System.Drawing.Size(102, 17)
        Me.chkRecursive.TabIndex = 2
        Me.chkRecursive.Text = "Scan subfolders"
        Me.chkRecursive.UseVisualStyleBackColor = True
        '
        'btnBrowse
        '
        Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowse.Location = New System.Drawing.Point(319, 19)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(35, 23)
        Me.btnBrowse.TabIndex = 1
        Me.btnBrowse.Text = "..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'txtFolderPath
        '
        Me.txtFolderPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFolderPath.Location = New System.Drawing.Point(6, 21)
        Me.txtFolderPath.Name = "txtFolderPath"
        Me.txtFolderPath.Size = New System.Drawing.Size(307, 20)
        Me.txtFolderPath.TabIndex = 0
        '
        'grpPassword
        '
        Me.grpPassword.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpPassword.Controls.Add(Me.txtPasswordNew)
        Me.grpPassword.Controls.Add(Me.lblPasswordArrow)
        Me.grpPassword.Controls.Add(Me.txtPasswordOld)
        Me.grpPassword.Controls.Add(Me.lblPasswordNew)
        Me.grpPassword.Controls.Add(Me.lblPasswordOld)
        Me.grpPassword.Controls.Add(Me.chkPassword)
        Me.grpPassword.Location = New System.Drawing.Point(12, 86)
        Me.grpPassword.Name = "grpPassword"
        Me.grpPassword.Size = New System.Drawing.Size(360, 70)
        Me.grpPassword.TabIndex = 1
        Me.grpPassword.TabStop = False
        Me.grpPassword.Text = "Password Change"
        '
        'txtPasswordNew
        '
        Me.txtPasswordNew.Enabled = False
        Me.txtPasswordNew.Location = New System.Drawing.Point(230, 42)
        Me.txtPasswordNew.Name = "txtPasswordNew"
        Me.txtPasswordNew.Size = New System.Drawing.Size(120, 20)
        Me.txtPasswordNew.TabIndex = 5
        '
        'lblPasswordArrow
        '
        Me.lblPasswordArrow.AutoSize = True
        Me.lblPasswordArrow.Enabled = False
        Me.lblPasswordArrow.Location = New System.Drawing.Point(195, 45)
        Me.lblPasswordArrow.Name = "lblPasswordArrow"
        Me.lblPasswordArrow.Size = New System.Drawing.Size(16, 13)
        Me.lblPasswordArrow.TabIndex = 4
        Me.lblPasswordArrow.Text = "->"
        '
        'txtPasswordOld
        '
        Me.txtPasswordOld.Enabled = False
        Me.txtPasswordOld.Location = New System.Drawing.Point(66, 42)
        Me.txtPasswordOld.Name = "txtPasswordOld"
        Me.txtPasswordOld.Size = New System.Drawing.Size(120, 20)
        Me.txtPasswordOld.TabIndex = 3
        '
        'lblPasswordNew
        '
        Me.lblPasswordNew.AutoSize = True
        Me.lblPasswordNew.Enabled = False
        Me.lblPasswordNew.Location = New System.Drawing.Point(227, 26)
        Me.lblPasswordNew.Name = "lblPasswordNew"
        Me.lblPasswordNew.Size = New System.Drawing.Size(29, 13)
        Me.lblPasswordNew.TabIndex = 2
        Me.lblPasswordNew.Text = "New"
        '
        'lblPasswordOld
        '
        Me.lblPasswordOld.AutoSize = True
        Me.lblPasswordOld.Enabled = False
        Me.lblPasswordOld.Location = New System.Drawing.Point(63, 26)
        Me.lblPasswordOld.Name = "lblPasswordOld"
        Me.lblPasswordOld.Size = New System.Drawing.Size(72, 13)
        Me.lblPasswordOld.TabIndex = 1
        Me.lblPasswordOld.Text = "Old (or empty)"
        '
        'chkPassword
        '
        Me.chkPassword.AutoSize = True
        Me.chkPassword.Location = New System.Drawing.Point(6, 19)
        Me.chkPassword.Name = "chkPassword"
        Me.chkPassword.Size = New System.Drawing.Size(59, 17)
        Me.chkPassword.TabIndex = 0
        Me.chkPassword.Text = "Enable"
        Me.chkPassword.UseVisualStyleBackColor = True
        '
        'grpLaunchOption
        '
        Me.grpLaunchOption.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpLaunchOption.Controls.Add(Me.lblOptionHint)
        Me.grpLaunchOption.Controls.Add(Me.txtOptionNew)
        Me.grpLaunchOption.Controls.Add(Me.lblOptionArrow)
        Me.grpLaunchOption.Controls.Add(Me.txtOptionOld)
        Me.grpLaunchOption.Controls.Add(Me.lblOptionNew)
        Me.grpLaunchOption.Controls.Add(Me.lblOptionOld)
        Me.grpLaunchOption.Controls.Add(Me.chkOption)
        Me.grpLaunchOption.Location = New System.Drawing.Point(12, 162)
        Me.grpLaunchOption.Name = "grpLaunchOption"
        Me.grpLaunchOption.Size = New System.Drawing.Size(360, 70)
        Me.grpLaunchOption.TabIndex = 2
        Me.grpLaunchOption.TabStop = False
        Me.grpLaunchOption.Text = "Launch Option (-o)"
        '
        'lblOptionHint
        '
        Me.lblOptionHint.AutoSize = True
        Me.lblOptionHint.Enabled = False
        Me.lblOptionHint.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblOptionHint.Location = New System.Drawing.Point(63, 26)
        Me.lblOptionHint.Name = "lblOptionHint"
        Me.lblOptionHint.Size = New System.Drawing.Size(135, 13)
        Me.lblOptionHint.TabIndex = 6
        Me.lblOptionHint.Text = "(Leave Old empty to set all)"
        '
        'txtOptionNew
        '
        Me.txtOptionNew.Enabled = False
        Me.txtOptionNew.Location = New System.Drawing.Point(230, 42)
        Me.txtOptionNew.Name = "txtOptionNew"
        Me.txtOptionNew.Size = New System.Drawing.Size(60, 20)
        Me.txtOptionNew.TabIndex = 5
        '
        'lblOptionArrow
        '
        Me.lblOptionArrow.AutoSize = True
        Me.lblOptionArrow.Enabled = False
        Me.lblOptionArrow.Location = New System.Drawing.Point(135, 45)
        Me.lblOptionArrow.Name = "lblOptionArrow"
        Me.lblOptionArrow.Size = New System.Drawing.Size(16, 13)
        Me.lblOptionArrow.TabIndex = 4
        Me.lblOptionArrow.Text = "->"
        '
        'txtOptionOld
        '
        Me.txtOptionOld.Enabled = False
        Me.txtOptionOld.Location = New System.Drawing.Point(66, 42)
        Me.txtOptionOld.Name = "txtOptionOld"
        Me.txtOptionOld.Size = New System.Drawing.Size(60, 20)
        Me.txtOptionOld.TabIndex = 3
        '
        'lblOptionNew
        '
        Me.lblOptionNew.AutoSize = True
        Me.lblOptionNew.Enabled = False
        Me.lblOptionNew.Location = New System.Drawing.Point(227, 26)
        Me.lblOptionNew.Name = "lblOptionNew"
        Me.lblOptionNew.Size = New System.Drawing.Size(29, 13)
        Me.lblOptionNew.TabIndex = 2
        Me.lblOptionNew.Text = "New"
        '
        'lblOptionOld
        '
        Me.lblOptionOld.AutoSize = True
        Me.lblOptionOld.Enabled = False
        Me.lblOptionOld.Location = New System.Drawing.Point(63, 26)
        Me.lblOptionOld.Name = "lblOptionOld"
        Me.lblOptionOld.Size = New System.Drawing.Size(0, 13)
        Me.lblOptionOld.TabIndex = 1
        '
        'chkOption
        '
        Me.chkOption.AutoSize = True
        Me.chkOption.Location = New System.Drawing.Point(6, 19)
        Me.chkOption.Name = "chkOption"
        Me.chkOption.Size = New System.Drawing.Size(59, 17)
        Me.chkOption.TabIndex = 0
        Me.chkOption.Text = "Enable"
        Me.chkOption.UseVisualStyleBackColor = True
        '
        'grpResolution
        '
        Me.grpResolution.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpResolution.Controls.Add(Me.numNewHeight)
        Me.grpResolution.Controls.Add(Me.numNewWidth)
        Me.grpResolution.Controls.Add(Me.lblNewCustom)
        Me.grpResolution.Controls.Add(Me.numOldHeight)
        Me.grpResolution.Controls.Add(Me.numOldWidth)
        Me.grpResolution.Controls.Add(Me.lblOldCustom)
        Me.grpResolution.Controls.Add(Me.cboResolutionNew)
        Me.grpResolution.Controls.Add(Me.lblResArrow)
        Me.grpResolution.Controls.Add(Me.cboResolutionOld)
        Me.grpResolution.Controls.Add(Me.lblResNew)
        Me.grpResolution.Controls.Add(Me.lblResOld)
        Me.grpResolution.Controls.Add(Me.chkResolution)
        Me.grpResolution.Location = New System.Drawing.Point(12, 238)
        Me.grpResolution.Name = "grpResolution"
        Me.grpResolution.Size = New System.Drawing.Size(360, 95)
        Me.grpResolution.TabIndex = 3
        Me.grpResolution.TabStop = False
        Me.grpResolution.Text = "Resolution (-w -h)"
        '
        'numNewHeight
        '
        Me.numNewHeight.Enabled = False
        Me.numNewHeight.Location = New System.Drawing.Point(300, 67)
        Me.numNewHeight.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.numNewHeight.Name = "numNewHeight"
        Me.numNewHeight.Size = New System.Drawing.Size(50, 20)
        Me.numNewHeight.TabIndex = 11
        Me.numNewHeight.Visible = False
        '
        'numNewWidth
        '
        Me.numNewWidth.Enabled = False
        Me.numNewWidth.Location = New System.Drawing.Point(230, 67)
        Me.numNewWidth.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.numNewWidth.Name = "numNewWidth"
        Me.numNewWidth.Size = New System.Drawing.Size(50, 20)
        Me.numNewWidth.TabIndex = 10
        Me.numNewWidth.Visible = False
        '
        'lblNewCustom
        '
        Me.lblNewCustom.AutoSize = True
        Me.lblNewCustom.Enabled = False
        Me.lblNewCustom.Location = New System.Drawing.Point(283, 69)
        Me.lblNewCustom.Name = "lblNewCustom"
        Me.lblNewCustom.Size = New System.Drawing.Size(12, 13)
        Me.lblNewCustom.TabIndex = 9
        Me.lblNewCustom.Text = "x"
        Me.lblNewCustom.Visible = False
        '
        'numOldHeight
        '
        Me.numOldHeight.Enabled = False
        Me.numOldHeight.Location = New System.Drawing.Point(136, 67)
        Me.numOldHeight.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.numOldHeight.Name = "numOldHeight"
        Me.numOldHeight.Size = New System.Drawing.Size(50, 20)
        Me.numOldHeight.TabIndex = 8
        Me.numOldHeight.Visible = False
        '
        'numOldWidth
        '
        Me.numOldWidth.Enabled = False
        Me.numOldWidth.Location = New System.Drawing.Point(66, 67)
        Me.numOldWidth.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.numOldWidth.Name = "numOldWidth"
        Me.numOldWidth.Size = New System.Drawing.Size(50, 20)
        Me.numOldWidth.TabIndex = 7
        Me.numOldWidth.Visible = False
        '
        'lblOldCustom
        '
        Me.lblOldCustom.AutoSize = True
        Me.lblOldCustom.Enabled = False
        Me.lblOldCustom.Location = New System.Drawing.Point(119, 69)
        Me.lblOldCustom.Name = "lblOldCustom"
        Me.lblOldCustom.Size = New System.Drawing.Size(12, 13)
        Me.lblOldCustom.TabIndex = 6
        Me.lblOldCustom.Text = "x"
        Me.lblOldCustom.Visible = False
        '
        'cboResolutionNew
        '
        Me.cboResolutionNew.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboResolutionNew.Enabled = False
        Me.cboResolutionNew.FormattingEnabled = True
        Me.cboResolutionNew.Location = New System.Drawing.Point(230, 40)
        Me.cboResolutionNew.Name = "cboResolutionNew"
        Me.cboResolutionNew.Size = New System.Drawing.Size(120, 21)
        Me.cboResolutionNew.TabIndex = 5
        '
        'lblResArrow
        '
        Me.lblResArrow.AutoSize = True
        Me.lblResArrow.Enabled = False
        Me.lblResArrow.Location = New System.Drawing.Point(195, 43)
        Me.lblResArrow.Name = "lblResArrow"
        Me.lblResArrow.Size = New System.Drawing.Size(16, 13)
        Me.lblResArrow.TabIndex = 4
        Me.lblResArrow.Text = "->"
        '
        'cboResolutionOld
        '
        Me.cboResolutionOld.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboResolutionOld.Enabled = False
        Me.cboResolutionOld.FormattingEnabled = True
        Me.cboResolutionOld.Location = New System.Drawing.Point(66, 40)
        Me.cboResolutionOld.Name = "cboResolutionOld"
        Me.cboResolutionOld.Size = New System.Drawing.Size(120, 21)
        Me.cboResolutionOld.TabIndex = 3
        '
        'lblResNew
        '
        Me.lblResNew.AutoSize = True
        Me.lblResNew.Enabled = False
        Me.lblResNew.Location = New System.Drawing.Point(227, 24)
        Me.lblResNew.Name = "lblResNew"
        Me.lblResNew.Size = New System.Drawing.Size(29, 13)
        Me.lblResNew.TabIndex = 2
        Me.lblResNew.Text = "New"
        '
        'lblResOld
        '
        Me.lblResOld.AutoSize = True
        Me.lblResOld.Enabled = False
        Me.lblResOld.Location = New System.Drawing.Point(63, 24)
        Me.lblResOld.Name = "lblResOld"
        Me.lblResOld.Size = New System.Drawing.Size(72, 13)
        Me.lblResOld.TabIndex = 1
        Me.lblResOld.Text = "Old (or empty)"
        '
        'chkResolution
        '
        Me.chkResolution.AutoSize = True
        Me.chkResolution.Location = New System.Drawing.Point(6, 19)
        Me.chkResolution.Name = "chkResolution"
        Me.chkResolution.Size = New System.Drawing.Size(59, 17)
        Me.chkResolution.TabIndex = 0
        Me.chkResolution.Text = "Enable"
        Me.chkResolution.UseVisualStyleBackColor = True
        '
        'btnPreview
        '
        Me.btnPreview.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPreview.Location = New System.Drawing.Point(135, 346)
        Me.btnPreview.Name = "btnPreview"
        Me.btnPreview.Size = New System.Drawing.Size(75, 23)
        Me.btnPreview.TabIndex = 4
        Me.btnPreview.Text = "Preview"
        Me.btnPreview.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApply.Location = New System.Drawing.Point(216, 346)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(75, 23)
        Me.btnApply.TabIndex = 5
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(297, 346)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.Location = New System.Drawing.Point(12, 346)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(117, 23)
        Me.lblStatus.TabIndex = 7
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'BatchShortcutManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(384, 379)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.btnPreview)
        Me.Controls.Add(Me.grpResolution)
        Me.Controls.Add(Me.grpLaunchOption)
        Me.Controls.Add(Me.grpPassword)
        Me.Controls.Add(Me.grpFolder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "BatchShortcutManager"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Batch Shortcut Manager"
        Me.grpFolder.ResumeLayout(False)
        Me.grpFolder.PerformLayout()
        Me.grpPassword.ResumeLayout(False)
        Me.grpPassword.PerformLayout()
        Me.grpLaunchOption.ResumeLayout(False)
        Me.grpLaunchOption.PerformLayout()
        Me.grpResolution.ResumeLayout(False)
        Me.grpResolution.PerformLayout()
        CType(Me.numNewHeight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numNewWidth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numOldHeight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numOldWidth, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpFolder As GroupBox
    Friend WithEvents btnBrowse As Button
    Friend WithEvents txtFolderPath As TextBox
    Friend WithEvents grpPassword As GroupBox
    Friend WithEvents txtPasswordNew As TextBox
    Friend WithEvents lblPasswordArrow As Label
    Friend WithEvents txtPasswordOld As TextBox
    Friend WithEvents lblPasswordNew As Label
    Friend WithEvents lblPasswordOld As Label
    Friend WithEvents chkPassword As CheckBox
    Friend WithEvents grpLaunchOption As GroupBox
    Friend WithEvents txtOptionNew As TextBox
    Friend WithEvents lblOptionArrow As Label
    Friend WithEvents txtOptionOld As TextBox
    Friend WithEvents lblOptionNew As Label
    Friend WithEvents lblOptionOld As Label
    Friend WithEvents chkOption As CheckBox
    Friend WithEvents grpResolution As GroupBox
    Friend WithEvents cboResolutionNew As ComboBox
    Friend WithEvents lblResArrow As Label
    Friend WithEvents cboResolutionOld As ComboBox
    Friend WithEvents lblResNew As Label
    Friend WithEvents lblResOld As Label
    Friend WithEvents chkResolution As CheckBox
    Friend WithEvents btnPreview As Button
    Friend WithEvents btnApply As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents numNewHeight As NumericUpDown
    Friend WithEvents numNewWidth As NumericUpDown
    Friend WithEvents lblNewCustom As Label
    Friend WithEvents numOldHeight As NumericUpDown
    Friend WithEvents numOldWidth As NumericUpDown
    Friend WithEvents lblOldCustom As Label
    Friend WithEvents lblOptionHint As Label
    Friend WithEvents chkRecursive As CheckBox
End Class
