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
        Me.tabControl = New System.Windows.Forms.TabControl()
        Me.tabSingle = New System.Windows.Forms.TabPage()
        Me.tabBulk = New System.Windows.Forms.TabPage()
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
        ' Bulk Add controls
        Me.grpSourceShortcut = New System.Windows.Forms.GroupBox()
        Me.lblSourceShortcut = New System.Windows.Forms.Label()
        Me.cboSourceShortcut = New System.Windows.Forms.ComboBox()
        Me.btnRefreshShortcuts = New System.Windows.Forms.Button()
        Me.lblSourceInfo = New System.Windows.Forms.Label()
        Me.grpBulkAccounts = New System.Windows.Forms.GroupBox()
        Me.dgvUsernames = New System.Windows.Forms.DataGridView()
        Me.lblBulkPassword = New System.Windows.Forms.Label()
        Me.txtBulkPassword = New System.Windows.Forms.TextBox()
        Me.grpTargetFolder = New System.Windows.Forms.GroupBox()
        Me.lblTargetFolder = New System.Windows.Forms.Label()
        Me.cboTargetFolder = New System.Windows.Forms.ComboBox()
        Me.btnBrowseFolder = New System.Windows.Forms.Button()
        Me.chkOverwrite = New System.Windows.Forms.CheckBox()
        Me.btnCreateBulk = New System.Windows.Forms.Button()
        ' Edit tab controls
        Me.tabEdit = New System.Windows.Forms.TabPage()
        Me.lblEditFolder = New System.Windows.Forms.Label()
        Me.cboEditFolder = New System.Windows.Forms.ComboBox()
        Me.btnRefreshEdit = New System.Windows.Forms.Button()
        Me.dgvEditShortcuts = New System.Windows.Forms.DataGridView()
        Me.btnCreateFolder = New System.Windows.Forms.Button()
        Me.txtNewFolder = New System.Windows.Forms.TextBox()
        Me.btnApplyRenames = New System.Windows.Forms.Button()

        Me.tabControl.SuspendLayout()
        Me.tabSingle.SuspendLayout()
        Me.tabBulk.SuspendLayout()
        Me.tabEdit.SuspendLayout()
        CType(Me.dgvEditShortcuts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpTemplates.SuspendLayout()
        Me.pnlDropZone.SuspendLayout()
        Me.grpCreateShortcut.SuspendLayout()
        CType(Me.numWidth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numHeight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSourceShortcut.SuspendLayout()
        Me.grpBulkAccounts.SuspendLayout()
        CType(Me.dgvUsernames, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpTargetFolder.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabControl
        '
        Me.tabControl.Controls.Add(Me.tabSingle)
        Me.tabControl.Controls.Add(Me.tabBulk)
        Me.tabControl.Controls.Add(Me.tabEdit)
        Me.tabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControl.Location = New System.Drawing.Point(0, 0)
        Me.tabControl.Name = "tabControl"
        Me.tabControl.SelectedIndex = 0
        Me.tabControl.Size = New System.Drawing.Size(400, 420)
        Me.tabControl.TabIndex = 0
        '
        'tabSingle
        '
        Me.tabSingle.Controls.Add(Me.grpTemplates)
        Me.tabSingle.Controls.Add(Me.grpCreateShortcut)
        Me.tabSingle.Location = New System.Drawing.Point(4, 22)
        Me.tabSingle.Name = "tabSingle"
        Me.tabSingle.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSingle.Size = New System.Drawing.Size(392, 394)
        Me.tabSingle.TabIndex = 0
        Me.tabSingle.Text = "Single"
        Me.tabSingle.UseVisualStyleBackColor = True
        '
        'tabBulk
        '
        Me.tabBulk.Controls.Add(Me.grpSourceShortcut)
        Me.tabBulk.Controls.Add(Me.grpBulkAccounts)
        Me.tabBulk.Controls.Add(Me.grpTargetFolder)
        Me.tabBulk.Controls.Add(Me.chkOverwrite)
        Me.tabBulk.Controls.Add(Me.btnCreateBulk)
        Me.tabBulk.Location = New System.Drawing.Point(4, 22)
        Me.tabBulk.Name = "tabBulk"
        Me.tabBulk.Padding = New System.Windows.Forms.Padding(3)
        Me.tabBulk.Size = New System.Drawing.Size(392, 394)
        Me.tabBulk.TabIndex = 1
        Me.tabBulk.Text = "Bulk Add"
        Me.tabBulk.UseVisualStyleBackColor = True
        '
        'tabEdit
        '
        Me.tabEdit.Controls.Add(Me.lblEditFolder)
        Me.tabEdit.Controls.Add(Me.cboEditFolder)
        Me.tabEdit.Controls.Add(Me.btnRefreshEdit)
        Me.tabEdit.Controls.Add(Me.dgvEditShortcuts)
        Me.tabEdit.Controls.Add(Me.txtNewFolder)
        Me.tabEdit.Controls.Add(Me.btnCreateFolder)
        Me.tabEdit.Controls.Add(Me.btnApplyRenames)
        Me.tabEdit.Location = New System.Drawing.Point(4, 22)
        Me.tabEdit.Name = "tabEdit"
        Me.tabEdit.Padding = New System.Windows.Forms.Padding(3)
        Me.tabEdit.Size = New System.Drawing.Size(392, 394)
        Me.tabEdit.TabIndex = 2
        Me.tabEdit.Text = "Edit"
        Me.tabEdit.UseVisualStyleBackColor = True
        '
        'lblEditFolder
        '
        Me.lblEditFolder.AutoSize = True
        Me.lblEditFolder.Location = New System.Drawing.Point(6, 9)
        Me.lblEditFolder.Name = "lblEditFolder"
        Me.lblEditFolder.Size = New System.Drawing.Size(39, 13)
        Me.lblEditFolder.TabIndex = 0
        Me.lblEditFolder.Text = "Folder:"
        '
        'cboEditFolder
        '
        Me.cboEditFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboEditFolder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEditFolder.FormattingEnabled = True
        Me.cboEditFolder.Location = New System.Drawing.Point(51, 6)
        Me.cboEditFolder.Name = "cboEditFolder"
        Me.cboEditFolder.Size = New System.Drawing.Size(304, 21)
        Me.cboEditFolder.TabIndex = 1
        '
        'btnRefreshEdit
        '
        Me.btnRefreshEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefreshEdit.Location = New System.Drawing.Point(361, 4)
        Me.btnRefreshEdit.Name = "btnRefreshEdit"
        Me.btnRefreshEdit.Size = New System.Drawing.Size(25, 23)
        Me.btnRefreshEdit.TabIndex = 2
        Me.btnRefreshEdit.Text = "↻"
        Me.btnRefreshEdit.UseVisualStyleBackColor = True
        '
        'dgvEditShortcuts
        '
        Me.dgvEditShortcuts.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvEditShortcuts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEditShortcuts.Location = New System.Drawing.Point(6, 33)
        Me.dgvEditShortcuts.Name = "dgvEditShortcuts"
        Me.dgvEditShortcuts.Size = New System.Drawing.Size(380, 280)
        Me.dgvEditShortcuts.TabIndex = 3
        '
        'txtNewFolder
        '
        Me.txtNewFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtNewFolder.Location = New System.Drawing.Point(6, 319)
        Me.txtNewFolder.Name = "txtNewFolder"
        Me.txtNewFolder.Size = New System.Drawing.Size(180, 20)
        Me.txtNewFolder.TabIndex = 4
        '
        'btnCreateFolder
        '
        Me.btnCreateFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCreateFolder.Location = New System.Drawing.Point(192, 317)
        Me.btnCreateFolder.Name = "btnCreateFolder"
        Me.btnCreateFolder.Size = New System.Drawing.Size(100, 23)
        Me.btnCreateFolder.TabIndex = 5
        Me.btnCreateFolder.Text = "Create Folder"
        Me.btnCreateFolder.UseVisualStyleBackColor = True
        '
        'btnApplyRenames
        '
        Me.btnApplyRenames.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApplyRenames.Location = New System.Drawing.Point(261, 362)
        Me.btnApplyRenames.Name = "btnApplyRenames"
        Me.btnApplyRenames.Size = New System.Drawing.Size(125, 26)
        Me.btnApplyRenames.TabIndex = 6
        Me.btnApplyRenames.Text = "Apply Renames"
        Me.btnApplyRenames.UseVisualStyleBackColor = True
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
        Me.grpTemplates.Size = New System.Drawing.Size(386, 130)
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
        Me.pnlDropZone.Size = New System.Drawing.Size(168, 69)
        Me.pnlDropZone.TabIndex = 1
        '
        'lblDropZone
        '
        Me.lblDropZone.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDropZone.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblDropZone.Location = New System.Drawing.Point(0, 0)
        Me.lblDropZone.Name = "lblDropZone"
        Me.lblDropZone.Size = New System.Drawing.Size(166, 67)
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
        Me.grpCreateShortcut.Size = New System.Drawing.Size(386, 170)
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
        Me.btnCreateShortcut.Location = New System.Drawing.Point(273, 140)
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
        'grpSourceShortcut
        '
        Me.grpSourceShortcut.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSourceShortcut.Controls.Add(Me.lblSourceShortcut)
        Me.grpSourceShortcut.Controls.Add(Me.cboSourceShortcut)
        Me.grpSourceShortcut.Controls.Add(Me.btnRefreshShortcuts)
        Me.grpSourceShortcut.Controls.Add(Me.lblSourceInfo)
        Me.grpSourceShortcut.Location = New System.Drawing.Point(3, 3)
        Me.grpSourceShortcut.Name = "grpSourceShortcut"
        Me.grpSourceShortcut.Size = New System.Drawing.Size(386, 85)
        Me.grpSourceShortcut.TabIndex = 0
        Me.grpSourceShortcut.TabStop = False
        Me.grpSourceShortcut.Text = "Copy Settings From"
        '
        'lblSourceShortcut
        '
        Me.lblSourceShortcut.AutoSize = True
        Me.lblSourceShortcut.Location = New System.Drawing.Point(6, 22)
        Me.lblSourceShortcut.Name = "lblSourceShortcut"
        Me.lblSourceShortcut.Size = New System.Drawing.Size(50, 13)
        Me.lblSourceShortcut.TabIndex = 0
        Me.lblSourceShortcut.Text = "Shortcut:"
        '
        'cboSourceShortcut
        '
        Me.cboSourceShortcut.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSourceShortcut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSourceShortcut.FormattingEnabled = True
        Me.cboSourceShortcut.Location = New System.Drawing.Point(62, 19)
        Me.cboSourceShortcut.Name = "cboSourceShortcut"
        Me.cboSourceShortcut.Size = New System.Drawing.Size(287, 21)
        Me.cboSourceShortcut.TabIndex = 1
        '
        'btnRefreshShortcuts
        '
        Me.btnRefreshShortcuts.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefreshShortcuts.Location = New System.Drawing.Point(355, 17)
        Me.btnRefreshShortcuts.Name = "btnRefreshShortcuts"
        Me.btnRefreshShortcuts.Size = New System.Drawing.Size(25, 23)
        Me.btnRefreshShortcuts.TabIndex = 2
        Me.btnRefreshShortcuts.Text = "↻"
        Me.btnRefreshShortcuts.UseVisualStyleBackColor = True
        '
        'lblSourceInfo
        '
        Me.lblSourceInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSourceInfo.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblSourceInfo.Location = New System.Drawing.Point(6, 45)
        Me.lblSourceInfo.Name = "lblSourceInfo"
        Me.lblSourceInfo.Size = New System.Drawing.Size(374, 35)
        Me.lblSourceInfo.TabIndex = 3
        Me.lblSourceInfo.Text = "Select a shortcut to copy settings from"
        '
        'grpBulkAccounts
        '
        Me.grpBulkAccounts.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpBulkAccounts.Controls.Add(Me.dgvUsernames)
        Me.grpBulkAccounts.Controls.Add(Me.lblBulkPassword)
        Me.grpBulkAccounts.Controls.Add(Me.txtBulkPassword)
        Me.grpBulkAccounts.Location = New System.Drawing.Point(3, 94)
        Me.grpBulkAccounts.Name = "grpBulkAccounts"
        Me.grpBulkAccounts.Size = New System.Drawing.Size(386, 190)
        Me.grpBulkAccounts.TabIndex = 1
        Me.grpBulkAccounts.TabStop = False
        Me.grpBulkAccounts.Text = "Accounts"
        '
        'dgvUsernames
        '
        Me.dgvUsernames.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvUsernames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUsernames.Location = New System.Drawing.Point(6, 19)
        Me.dgvUsernames.Name = "dgvUsernames"
        Me.dgvUsernames.Size = New System.Drawing.Size(374, 140)
        Me.dgvUsernames.TabIndex = 0
        '
        'lblBulkPassword
        '
        Me.lblBulkPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblBulkPassword.AutoSize = True
        Me.lblBulkPassword.Location = New System.Drawing.Point(6, 167)
        Me.lblBulkPassword.Name = "lblBulkPassword"
        Me.lblBulkPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblBulkPassword.TabIndex = 1
        Me.lblBulkPassword.Text = "Password:"
        '
        'txtBulkPassword
        '
        Me.txtBulkPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtBulkPassword.Location = New System.Drawing.Point(68, 164)
        Me.txtBulkPassword.Name = "txtBulkPassword"
        Me.txtBulkPassword.Size = New System.Drawing.Size(150, 20)
        Me.txtBulkPassword.TabIndex = 2
        Me.txtBulkPassword.UseSystemPasswordChar = True
        '
        'grpTargetFolder
        '
        Me.grpTargetFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpTargetFolder.Controls.Add(Me.lblTargetFolder)
        Me.grpTargetFolder.Controls.Add(Me.cboTargetFolder)
        Me.grpTargetFolder.Controls.Add(Me.btnBrowseFolder)
        Me.grpTargetFolder.Location = New System.Drawing.Point(3, 290)
        Me.grpTargetFolder.Name = "grpTargetFolder"
        Me.grpTargetFolder.Size = New System.Drawing.Size(386, 48)
        Me.grpTargetFolder.TabIndex = 2
        Me.grpTargetFolder.TabStop = False
        Me.grpTargetFolder.Text = "Save To"
        '
        'lblTargetFolder
        '
        Me.lblTargetFolder.AutoSize = True
        Me.lblTargetFolder.Location = New System.Drawing.Point(6, 22)
        Me.lblTargetFolder.Name = "lblTargetFolder"
        Me.lblTargetFolder.Size = New System.Drawing.Size(39, 13)
        Me.lblTargetFolder.TabIndex = 0
        Me.lblTargetFolder.Text = "Folder:"
        '
        'cboTargetFolder
        '
        Me.cboTargetFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboTargetFolder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTargetFolder.FormattingEnabled = True
        Me.cboTargetFolder.Location = New System.Drawing.Point(51, 19)
        Me.cboTargetFolder.Name = "cboTargetFolder"
        Me.cboTargetFolder.Size = New System.Drawing.Size(298, 21)
        Me.cboTargetFolder.TabIndex = 1
        '
        'btnBrowseFolder
        '
        Me.btnBrowseFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseFolder.Location = New System.Drawing.Point(355, 17)
        Me.btnBrowseFolder.Name = "btnBrowseFolder"
        Me.btnBrowseFolder.Size = New System.Drawing.Size(25, 23)
        Me.btnBrowseFolder.TabIndex = 2
        Me.btnBrowseFolder.Text = "..."
        Me.btnBrowseFolder.UseVisualStyleBackColor = True
        '
        'chkOverwrite
        '
        Me.chkOverwrite.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkOverwrite.AutoSize = True
        Me.chkOverwrite.Location = New System.Drawing.Point(6, 344)
        Me.chkOverwrite.Name = "chkOverwrite"
        Me.chkOverwrite.Size = New System.Drawing.Size(114, 17)
        Me.chkOverwrite.TabIndex = 3
        Me.chkOverwrite.Text = "Overwrite existing"
        Me.chkOverwrite.UseVisualStyleBackColor = True
        '
        'btnCreateBulk
        '
        Me.btnCreateBulk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCreateBulk.Location = New System.Drawing.Point(264, 340)
        Me.btnCreateBulk.Name = "btnCreateBulk"
        Me.btnCreateBulk.Size = New System.Drawing.Size(125, 26)
        Me.btnCreateBulk.TabIndex = 3
        Me.btnCreateBulk.Text = "Create Shortcuts"
        Me.btnCreateBulk.UseVisualStyleBackColor = True
        '
        'LauncherSetupControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.tabControl)
        Me.Name = "LauncherSetupControl"
        Me.Size = New System.Drawing.Size(400, 420)
        Me.tabControl.ResumeLayout(False)
        Me.tabSingle.ResumeLayout(False)
        Me.tabBulk.ResumeLayout(False)
        Me.tabEdit.ResumeLayout(False)
        Me.tabEdit.PerformLayout()
        CType(Me.dgvEditShortcuts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpTemplates.ResumeLayout(False)
        Me.pnlDropZone.ResumeLayout(False)
        Me.grpCreateShortcut.ResumeLayout(False)
        Me.grpCreateShortcut.PerformLayout()
        CType(Me.numWidth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numHeight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSourceShortcut.ResumeLayout(False)
        Me.grpSourceShortcut.PerformLayout()
        Me.grpBulkAccounts.ResumeLayout(False)
        Me.grpBulkAccounts.PerformLayout()
        CType(Me.dgvUsernames, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpTargetFolder.ResumeLayout(False)
        Me.grpTargetFolder.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents tabControl As TabControl
    Friend WithEvents tabSingle As TabPage
    Friend WithEvents tabBulk As TabPage
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
    ' Bulk Add controls
    Friend WithEvents grpSourceShortcut As GroupBox
    Friend WithEvents lblSourceShortcut As Label
    Friend WithEvents cboSourceShortcut As ComboBox
    Friend WithEvents btnRefreshShortcuts As Button
    Friend WithEvents lblSourceInfo As Label
    Friend WithEvents grpBulkAccounts As GroupBox
    Friend WithEvents dgvUsernames As DataGridView
    Friend WithEvents lblBulkPassword As Label
    Friend WithEvents txtBulkPassword As TextBox
    Friend WithEvents grpTargetFolder As GroupBox
    Friend WithEvents lblTargetFolder As Label
    Friend WithEvents cboTargetFolder As ComboBox
    Friend WithEvents btnBrowseFolder As Button
    Friend WithEvents chkOverwrite As CheckBox
    Friend WithEvents btnCreateBulk As Button
    ' Edit tab controls
    Friend WithEvents tabEdit As TabPage
    Friend WithEvents lblEditFolder As Label
    Friend WithEvents cboEditFolder As ComboBox
    Friend WithEvents btnRefreshEdit As Button
    Friend WithEvents dgvEditShortcuts As DataGridView
    Friend WithEvents txtNewFolder As TextBox
    Friend WithEvents btnCreateFolder As Button
    Friend WithEvents btnApplyRenames As Button

End Class
