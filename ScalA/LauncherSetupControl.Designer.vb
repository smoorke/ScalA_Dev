<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LauncherSetupControl
    Inherits System.Windows.Forms.UserControl

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
        Me.grpTemplates = New System.Windows.Forms.GroupBox()
        Me.lstTemplates = New System.Windows.Forms.ListBox()
        Me.pnlDropZone = New System.Windows.Forms.Panel()
        Me.lblDropZone = New System.Windows.Forms.Label()
        Me.btnEditTemplate = New System.Windows.Forms.Button()
        Me.btnDeleteTemplate = New System.Windows.Forms.Button()
        Me.grpCreateShortcut = New System.Windows.Forms.GroupBox()
        Me.cboTemplate = New System.Windows.Forms.ComboBox()
        Me.lblTemplate = New System.Windows.Forms.Label()
        Me.txtCharacter = New System.Windows.Forms.TextBox()
        Me.lblCharacter = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.numWidth = New System.Windows.Forms.NumericUpDown()
        Me.lblResolution = New System.Windows.Forms.Label()
        Me.numHeight = New System.Windows.Forms.NumericUpDown()
        Me.lblResX = New System.Windows.Forms.Label()
        Me.txtOptions = New System.Windows.Forms.TextBox()
        Me.lblOptions = New System.Windows.Forms.Label()
        Me.btnCreateShortcut = New System.Windows.Forms.Button()
        Me.chkOverrideResolution = New System.Windows.Forms.CheckBox()
        Me.chkOverrideOptions = New System.Windows.Forms.CheckBox()
        Me.grpTemplates.SuspendLayout()
        Me.pnlDropZone.SuspendLayout()
        Me.grpCreateShortcut.SuspendLayout()
        CType(Me.numWidth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numHeight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpTemplates
        '
        Me.grpTemplates.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpTemplates.Controls.Add(Me.btnDeleteTemplate)
        Me.grpTemplates.Controls.Add(Me.btnEditTemplate)
        Me.grpTemplates.Controls.Add(Me.pnlDropZone)
        Me.grpTemplates.Controls.Add(Me.lstTemplates)
        Me.grpTemplates.Location = New System.Drawing.Point(3, 3)
        Me.grpTemplates.Name = "grpTemplates"
        Me.grpTemplates.Size = New System.Drawing.Size(394, 130)
        Me.grpTemplates.TabIndex = 0
        Me.grpTemplates.TabStop = False
        Me.grpTemplates.Text = "Launcher Templates"
        '
        'lstTemplates
        '
        Me.lstTemplates.FormattingEnabled = True
        Me.lstTemplates.Location = New System.Drawing.Point(6, 19)
        Me.lstTemplates.Name = "lstTemplates"
        Me.lstTemplates.Size = New System.Drawing.Size(200, 69)
        Me.lstTemplates.TabIndex = 0
        '
        'pnlDropZone
        '
        Me.pnlDropZone.AllowDrop = True
        Me.pnlDropZone.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlDropZone.BackColor = System.Drawing.SystemColors.ControlLight
        Me.pnlDropZone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDropZone.Controls.Add(Me.lblDropZone)
        Me.pnlDropZone.Location = New System.Drawing.Point(212, 19)
        Me.pnlDropZone.Name = "pnlDropZone"
        Me.pnlDropZone.Size = New System.Drawing.Size(176, 69)
        Me.pnlDropZone.TabIndex = 1
        '
        'lblDropZone
        '
        Me.lblDropZone.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDropZone.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblDropZone.Location = New System.Drawing.Point(0, 0)
        Me.lblDropZone.Name = "lblDropZone"
        Me.lblDropZone.Size = New System.Drawing.Size(174, 67)
        Me.lblDropZone.TabIndex = 0
        Me.lblDropZone.Text = "Drag && drop .exe here" & vbCrLf & "to add a launcher"
        Me.lblDropZone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnEditTemplate
        '
        Me.btnEditTemplate.Location = New System.Drawing.Point(6, 94)
        Me.btnEditTemplate.Name = "btnEditTemplate"
        Me.btnEditTemplate.Size = New System.Drawing.Size(60, 23)
        Me.btnEditTemplate.TabIndex = 2
        Me.btnEditTemplate.Text = "Edit..."
        Me.btnEditTemplate.UseVisualStyleBackColor = True
        '
        'btnDeleteTemplate
        '
        Me.btnDeleteTemplate.Location = New System.Drawing.Point(72, 94)
        Me.btnDeleteTemplate.Name = "btnDeleteTemplate"
        Me.btnDeleteTemplate.Size = New System.Drawing.Size(60, 23)
        Me.btnDeleteTemplate.TabIndex = 3
        Me.btnDeleteTemplate.Text = "Delete"
        Me.btnDeleteTemplate.UseVisualStyleBackColor = True
        '
        'grpCreateShortcut
        '
        Me.grpCreateShortcut.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpCreateShortcut.Controls.Add(Me.chkOverrideOptions)
        Me.grpCreateShortcut.Controls.Add(Me.chkOverrideResolution)
        Me.grpCreateShortcut.Controls.Add(Me.btnCreateShortcut)
        Me.grpCreateShortcut.Controls.Add(Me.txtOptions)
        Me.grpCreateShortcut.Controls.Add(Me.lblOptions)
        Me.grpCreateShortcut.Controls.Add(Me.lblResX)
        Me.grpCreateShortcut.Controls.Add(Me.numHeight)
        Me.grpCreateShortcut.Controls.Add(Me.numWidth)
        Me.grpCreateShortcut.Controls.Add(Me.lblResolution)
        Me.grpCreateShortcut.Controls.Add(Me.txtPassword)
        Me.grpCreateShortcut.Controls.Add(Me.lblPassword)
        Me.grpCreateShortcut.Controls.Add(Me.txtCharacter)
        Me.grpCreateShortcut.Controls.Add(Me.lblCharacter)
        Me.grpCreateShortcut.Controls.Add(Me.cboTemplate)
        Me.grpCreateShortcut.Controls.Add(Me.lblTemplate)
        Me.grpCreateShortcut.Location = New System.Drawing.Point(3, 139)
        Me.grpCreateShortcut.Name = "grpCreateShortcut"
        Me.grpCreateShortcut.Size = New System.Drawing.Size(394, 175)
        Me.grpCreateShortcut.TabIndex = 1
        Me.grpCreateShortcut.TabStop = False
        Me.grpCreateShortcut.Text = "Create Character Shortcut"
        '
        'cboTemplate
        '
        Me.cboTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTemplate.FormattingEnabled = True
        Me.cboTemplate.Location = New System.Drawing.Point(75, 19)
        Me.cboTemplate.Name = "cboTemplate"
        Me.cboTemplate.Size = New System.Drawing.Size(200, 21)
        Me.cboTemplate.TabIndex = 1
        '
        'lblTemplate
        '
        Me.lblTemplate.AutoSize = True
        Me.lblTemplate.Location = New System.Drawing.Point(6, 22)
        Me.lblTemplate.Name = "lblTemplate"
        Me.lblTemplate.Size = New System.Drawing.Size(54, 13)
        Me.lblTemplate.TabIndex = 0
        Me.lblTemplate.Text = "Template:"
        '
        'txtCharacter
        '
        Me.txtCharacter.Location = New System.Drawing.Point(75, 46)
        Me.txtCharacter.Name = "txtCharacter"
        Me.txtCharacter.Size = New System.Drawing.Size(150, 20)
        Me.txtCharacter.TabIndex = 3
        '
        'lblCharacter
        '
        Me.lblCharacter.AutoSize = True
        Me.lblCharacter.Location = New System.Drawing.Point(6, 49)
        Me.lblCharacter.Name = "lblCharacter"
        Me.lblCharacter.Size = New System.Drawing.Size(56, 13)
        Me.lblCharacter.TabIndex = 2
        Me.lblCharacter.Text = "Character:"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(75, 72)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(150, 20)
        Me.txtPassword.TabIndex = 5
        Me.txtPassword.UseSystemPasswordChar = True
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(6, 75)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblPassword.TabIndex = 4
        Me.lblPassword.Text = "Password:"
        '
        'numWidth
        '
        Me.numWidth.Enabled = False
        Me.numWidth.Location = New System.Drawing.Point(126, 98)
        Me.numWidth.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.numWidth.Minimum = New Decimal(New Integer() {400, 0, 0, 0})
        Me.numWidth.Name = "numWidth"
        Me.numWidth.Size = New System.Drawing.Size(55, 20)
        Me.numWidth.TabIndex = 8
        Me.numWidth.Value = New Decimal(New Integer() {800, 0, 0, 0})
        '
        'lblResolution
        '
        Me.lblResolution.AutoSize = True
        Me.lblResolution.Location = New System.Drawing.Point(6, 100)
        Me.lblResolution.Name = "lblResolution"
        Me.lblResolution.Size = New System.Drawing.Size(60, 13)
        Me.lblResolution.TabIndex = 6
        Me.lblResolution.Text = "Resolution:"
        '
        'numHeight
        '
        Me.numHeight.Enabled = False
        Me.numHeight.Location = New System.Drawing.Point(200, 98)
        Me.numHeight.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.numHeight.Minimum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.numHeight.Name = "numHeight"
        Me.numHeight.Size = New System.Drawing.Size(55, 20)
        Me.numHeight.TabIndex = 10
        Me.numHeight.Value = New Decimal(New Integer() {600, 0, 0, 0})
        '
        'lblResX
        '
        Me.lblResX.AutoSize = True
        Me.lblResX.Enabled = False
        Me.lblResX.Location = New System.Drawing.Point(184, 100)
        Me.lblResX.Name = "lblResX"
        Me.lblResX.Size = New System.Drawing.Size(12, 13)
        Me.lblResX.TabIndex = 9
        Me.lblResX.Text = "x"
        '
        'txtOptions
        '
        Me.txtOptions.Enabled = False
        Me.txtOptions.Location = New System.Drawing.Point(126, 124)
        Me.txtOptions.Name = "txtOptions"
        Me.txtOptions.Size = New System.Drawing.Size(80, 20)
        Me.txtOptions.TabIndex = 13
        '
        'lblOptions
        '
        Me.lblOptions.AutoSize = True
        Me.lblOptions.Location = New System.Drawing.Point(6, 127)
        Me.lblOptions.Name = "lblOptions"
        Me.lblOptions.Size = New System.Drawing.Size(55, 13)
        Me.lblOptions.TabIndex = 11
        Me.lblOptions.Text = "Options -o:"
        '
        'btnCreateShortcut
        '
        Me.btnCreateShortcut.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCreateShortcut.Location = New System.Drawing.Point(281, 143)
        Me.btnCreateShortcut.Name = "btnCreateShortcut"
        Me.btnCreateShortcut.Size = New System.Drawing.Size(107, 26)
        Me.btnCreateShortcut.TabIndex = 14
        Me.btnCreateShortcut.Text = "Create Shortcut"
        Me.btnCreateShortcut.UseVisualStyleBackColor = True
        '
        'chkOverrideResolution
        '
        Me.chkOverrideResolution.AutoSize = True
        Me.chkOverrideResolution.Location = New System.Drawing.Point(75, 99)
        Me.chkOverrideResolution.Name = "chkOverrideResolution"
        Me.chkOverrideResolution.Size = New System.Drawing.Size(45, 17)
        Me.chkOverrideResolution.TabIndex = 7
        Me.chkOverrideResolution.Text = "Set:"
        Me.chkOverrideResolution.UseVisualStyleBackColor = True
        '
        'chkOverrideOptions
        '
        Me.chkOverrideOptions.AutoSize = True
        Me.chkOverrideOptions.Location = New System.Drawing.Point(75, 126)
        Me.chkOverrideOptions.Name = "chkOverrideOptions"
        Me.chkOverrideOptions.Size = New System.Drawing.Size(45, 17)
        Me.chkOverrideOptions.TabIndex = 12
        Me.chkOverrideOptions.Text = "Set:"
        Me.chkOverrideOptions.UseVisualStyleBackColor = True
        '
        'LauncherSetupControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpCreateShortcut)
        Me.Controls.Add(Me.grpTemplates)
        Me.Name = "LauncherSetupControl"
        Me.Size = New System.Drawing.Size(400, 320)
        Me.grpTemplates.ResumeLayout(False)
        Me.pnlDropZone.ResumeLayout(False)
        Me.grpCreateShortcut.ResumeLayout(False)
        Me.grpCreateShortcut.PerformLayout()
        CType(Me.numWidth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numHeight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpTemplates As GroupBox
    Friend WithEvents lstTemplates As ListBox
    Friend WithEvents pnlDropZone As Panel
    Friend WithEvents lblDropZone As Label
    Friend WithEvents btnEditTemplate As Button
    Friend WithEvents btnDeleteTemplate As Button
    Friend WithEvents grpCreateShortcut As GroupBox
    Friend WithEvents cboTemplate As ComboBox
    Friend WithEvents lblTemplate As Label
    Friend WithEvents txtCharacter As TextBox
    Friend WithEvents lblCharacter As Label
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents lblPassword As Label
    Friend WithEvents numWidth As NumericUpDown
    Friend WithEvents lblResolution As Label
    Friend WithEvents numHeight As NumericUpDown
    Friend WithEvents lblResX As Label
    Friend WithEvents txtOptions As TextBox
    Friend WithEvents lblOptions As Label
    Friend WithEvents btnCreateShortcut As Button
    Friend WithEvents chkOverrideResolution As CheckBox
    Friend WithEvents chkOverrideOptions As CheckBox

End Class
