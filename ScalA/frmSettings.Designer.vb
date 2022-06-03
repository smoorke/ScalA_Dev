<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
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
        Dim grpQLPath As System.Windows.Forms.GroupBox
        Dim Label3 As System.Windows.Forms.Label
        Dim Label2 As System.Windows.Forms.Label
        Dim Label1 As System.Windows.Forms.Label
        Dim ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Dim Label4 As System.Windows.Forms.Label
        Dim Label5 As System.Windows.Forms.Label
        Dim Label7 As System.Windows.Forms.Label
        Dim Label8 As System.Windows.Forms.Label
        Dim Label9 As System.Windows.Forms.Label
        Dim Label10 As System.Windows.Forms.Label
        Dim Label11 As System.Windows.Forms.Label
        Dim Label12 As System.Windows.Forms.Label
        Dim Label13 As System.Windows.Forms.Label
        Dim Label14 As System.Windows.Forms.Label
        Dim Label6 As System.Windows.Forms.Label
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmSettings))
        Me.txtQuickLaunchPath = New System.Windows.Forms.TextBox()
        Me.btnOpenFolderDialog = New System.Windows.Forms.Button()
        Me.cmsQLFolder = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenInExplorerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.tmrAlign = New System.Windows.Forms.Timer(Me.components)
        Me.ttSettings = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkCycleOnClose = New System.Windows.Forms.CheckBox()
        Me.ChkSingleInstance = New System.Windows.Forms.CheckBox()
        Me.grpReserveSpace = New System.Windows.Forms.GroupBox()
        Me.NumBorderBot = New System.Windows.Forms.NumericUpDown()
        Me.NumBorderRight = New System.Windows.Forms.NumericUpDown()
        Me.NumBorderTop = New System.Windows.Forms.NumericUpDown()
        Me.NumBorderLeft = New System.Windows.Forms.NumericUpDown()
        Me.ChkLessRowCol = New System.Windows.Forms.CheckBox()
        Me.chkOverViewIsGame = New System.Windows.Forms.CheckBox()
        Me.tbcSettings = New System.Windows.Forms.TabControl()
        Me.tabResolutions = New System.Windows.Forms.TabPage()
        Me.btnRestore = New System.Windows.Forms.Button()
        Me.btnSort = New System.Windows.Forms.Button()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.cmsGenerate = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.X60043ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.X720169ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FromToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DummyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtResolutions = New System.Windows.Forms.TextBox()
        Me.tabHotkeys = New System.Windows.Forms.TabPage()
        Me.grpCycleShortcut = New System.Windows.Forms.GroupBox()
        Me.chkCycleDownShift = New System.Windows.Forms.CheckBox()
        Me.chkCycleUpShift = New System.Windows.Forms.CheckBox()
        Me.chkCycleDownAlt = New System.Windows.Forms.CheckBox()
        Me.chkCycleDownCtrl = New System.Windows.Forms.CheckBox()
        Me.txtCycleKeyDown = New System.Windows.Forms.TextBox()
        Me.chkCycleAlts = New System.Windows.Forms.CheckBox()
        Me.chkCycleUpAlt = New System.Windows.Forms.CheckBox()
        Me.chkCycleUpCtrl = New System.Windows.Forms.CheckBox()
        Me.txtCycleKeyUp = New System.Windows.Forms.TextBox()
        Me.grpOverviewShortcut = New System.Windows.Forms.GroupBox()
        Me.chkStoShift = New System.Windows.Forms.CheckBox()
        Me.txtStoKey = New System.Windows.Forms.TextBox()
        Me.chkStoAlt = New System.Windows.Forms.CheckBox()
        Me.chkStoCtrl = New System.Windows.Forms.CheckBox()
        Me.chkSwitchToOverview = New System.Windows.Forms.CheckBox()
        Me.tabSortAndBL = New System.Windows.Forms.TabPage()
        Me.btnTest = New System.Windows.Forms.Button()
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.txtBotSort = New System.Windows.Forms.TextBox()
        Me.txtTopSort = New System.Windows.Forms.TextBox()
        Me.chkWhitelist = New System.Windows.Forms.CheckBox()
        Me.tabMaximized = New System.Windows.Forms.TabPage()
        Me.NumExtraMax = New System.Windows.Forms.NumericUpDown()
        Me.tabMisc = New System.Windows.Forms.TabPage()
        Me.chkDoAlign = New System.Windows.Forms.CheckBox()
        Me.grpAlign = New System.Windows.Forms.GroupBox()
        Me.btnResetAlign = New System.Windows.Forms.Button()
        Me.numXoffset = New System.Windows.Forms.NumericUpDown()
        Me.numYoffset = New System.Windows.Forms.NumericUpDown()
        Me.txtExe = New System.Windows.Forms.TextBox()
        Me.chkTopMost = New System.Windows.Forms.CheckBox()
        Me.chkRoundCorners = New System.Windows.Forms.CheckBox()
        Me.txtClass = New System.Windows.Forms.TextBox()
        grpQLPath = New System.Windows.Forms.GroupBox()
        Label3 = New System.Windows.Forms.Label()
        Label2 = New System.Windows.Forms.Label()
        Label1 = New System.Windows.Forms.Label()
        ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Label4 = New System.Windows.Forms.Label()
        Label5 = New System.Windows.Forms.Label()
        Label7 = New System.Windows.Forms.Label()
        Label8 = New System.Windows.Forms.Label()
        Label9 = New System.Windows.Forms.Label()
        Label10 = New System.Windows.Forms.Label()
        Label11 = New System.Windows.Forms.Label()
        Label12 = New System.Windows.Forms.Label()
        Label13 = New System.Windows.Forms.Label()
        Label14 = New System.Windows.Forms.Label()
        Label6 = New System.Windows.Forms.Label()
        grpQLPath.SuspendLayout()
        Me.cmsQLFolder.SuspendLayout()
        Me.grpReserveSpace.SuspendLayout()
        CType(Me.NumBorderBot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumBorderRight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumBorderTop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumBorderLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbcSettings.SuspendLayout()
        Me.tabResolutions.SuspendLayout()
        Me.cmsGenerate.SuspendLayout()
        Me.tabHotkeys.SuspendLayout()
        Me.grpCycleShortcut.SuspendLayout()
        Me.grpOverviewShortcut.SuspendLayout()
        Me.tabSortAndBL.SuspendLayout()
        Me.tabMaximized.SuspendLayout()
        CType(Me.NumExtraMax, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabMisc.SuspendLayout()
        Me.grpAlign.SuspendLayout()
        CType(Me.numXoffset, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numYoffset, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpQLPath
        '
        grpQLPath.Controls.Add(Me.txtQuickLaunchPath)
        grpQLPath.Controls.Add(Me.btnOpenFolderDialog)
        grpQLPath.Location = New System.Drawing.Point(7, -2)
        grpQLPath.Name = "grpQLPath"
        grpQLPath.Size = New System.Drawing.Size(291, 40)
        grpQLPath.TabIndex = 16
        grpQLPath.TabStop = False
        grpQLPath.Text = "QuickLaunch Path"
        '
        'txtQuickLaunchPath
        '
        Me.txtQuickLaunchPath.Location = New System.Drawing.Point(5, 13)
        Me.txtQuickLaunchPath.Name = "txtQuickLaunchPath"
        Me.txtQuickLaunchPath.Size = New System.Drawing.Size(259, 20)
        Me.txtQuickLaunchPath.TabIndex = 0
        Me.txtQuickLaunchPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnOpenFolderDialog
        '
        Me.btnOpenFolderDialog.ContextMenuStrip = Me.cmsQLFolder
        Me.btnOpenFolderDialog.FlatAppearance.BorderSize = 0
        Me.btnOpenFolderDialog.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOpenFolderDialog.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOpenFolderDialog.Location = New System.Drawing.Point(263, 13)
        Me.btnOpenFolderDialog.Name = "btnOpenFolderDialog"
        Me.btnOpenFolderDialog.Size = New System.Drawing.Size(22, 20)
        Me.btnOpenFolderDialog.TabIndex = 1
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
        'Label3
        '
        Label3.AutoSize = True
        Label3.Location = New System.Drawing.Point(166, 92)
        Label3.Name = "Label3"
        Label3.Size = New System.Drawing.Size(27, 13)
        Label3.TabIndex = 18
        Label3.Text = ".exe"
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
        'ToolStripSeparator1
        '
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New System.Drawing.Size(151, 6)
        '
        'Label4
        '
        Label4.AutoSize = True
        Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Label4.Location = New System.Drawing.Point(52, 3)
        Label4.Name = "Label4"
        Label4.Size = New System.Drawing.Size(18, 9)
        Label4.TabIndex = 3
        Label4.Text = "Top"
        '
        'Label5
        '
        Label5.AutoSize = True
        Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Label5.Location = New System.Drawing.Point(212, 3)
        Label5.Name = "Label5"
        Label5.Size = New System.Drawing.Size(29, 9)
        Label5.TabIndex = 4
        Label5.Text = "Bottom"
        '
        'Label7
        '
        Label7.AutoSize = True
        Label7.Location = New System.Drawing.Point(5, 39)
        Label7.Name = "Label7"
        Label7.Size = New System.Drawing.Size(26, 13)
        Label7.TabIndex = 0
        Label7.Text = "Top"
        '
        'Label8
        '
        Label8.AutoSize = True
        Label8.Location = New System.Drawing.Point(5, 20)
        Label8.Name = "Label8"
        Label8.Size = New System.Drawing.Size(25, 13)
        Label8.TabIndex = 1
        Label8.Text = "Left"
        '
        'Label9
        '
        Label9.AutoSize = True
        Label9.Location = New System.Drawing.Point(107, 20)
        Label9.Name = "Label9"
        Label9.Size = New System.Drawing.Size(32, 13)
        Label9.TabIndex = 2
        Label9.Text = "Right"
        '
        'Label10
        '
        Label10.AutoSize = True
        Label10.Location = New System.Drawing.Point(107, 39)
        Label10.Name = "Label10"
        Label10.Size = New System.Drawing.Size(23, 13)
        Label10.TabIndex = 3
        Label10.Text = "Bot"
        '
        'Label11
        '
        Label11.AutoSize = True
        Label11.Location = New System.Drawing.Point(86, 20)
        Label11.Name = "Label11"
        Label11.Size = New System.Drawing.Size(18, 13)
        Label11.TabIndex = 8
        Label11.Text = "‰"
        '
        'Label12
        '
        Label12.AutoSize = True
        Label12.Location = New System.Drawing.Point(86, 39)
        Label12.Name = "Label12"
        Label12.Size = New System.Drawing.Size(18, 13)
        Label12.TabIndex = 9
        Label12.Text = "‰"
        '
        'Label13
        '
        Label13.AutoSize = True
        Label13.Location = New System.Drawing.Point(188, 20)
        Label13.Name = "Label13"
        Label13.Size = New System.Drawing.Size(18, 13)
        Label13.TabIndex = 10
        Label13.Text = "‰"
        '
        'Label14
        '
        Label14.AutoSize = True
        Label14.Location = New System.Drawing.Point(188, 39)
        Label14.Name = "Label14"
        Label14.Size = New System.Drawing.Size(18, 13)
        Label14.TabIndex = 11
        Label14.Text = "‰"
        '
        'Label6
        '
        Label6.AutoSize = True
        Label6.Location = New System.Drawing.Point(86, 104)
        Label6.Name = "Label6"
        Label6.Size = New System.Drawing.Size(106, 13)
        Label6.TabIndex = 28
        Label6.Text = "Extra Columns/Rows"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(141, 166)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(77, 23)
        Me.btnOK.TabIndex = 2
        Me.btnOK.TabStop = False
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(224, 166)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(77, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.TabStop = False
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'tmrAlign
        '
        '
        'ttSettings
        '
        Me.ttSettings.AutoPopDelay = 10000
        Me.ttSettings.InitialDelay = 500
        Me.ttSettings.ReshowDelay = 100
        '
        'chkCycleOnClose
        '
        Me.chkCycleOnClose.AutoSize = True
        Me.chkCycleOnClose.Location = New System.Drawing.Point(20, 69)
        Me.chkCycleOnClose.Name = "chkCycleOnClose"
        Me.chkCycleOnClose.Size = New System.Drawing.Size(96, 17)
        Me.chkCycleOnClose.TabIndex = 4
        Me.chkCycleOnClose.Text = "Cycle on Close"
        Me.ttSettings.SetToolTip(Me.chkCycleOnClose, "Closing an alt will cycle to the next one.")
        Me.chkCycleOnClose.UseVisualStyleBackColor = True
        '
        'ChkSingleInstance
        '
        Me.ChkSingleInstance.AutoSize = True
        Me.ChkSingleInstance.Checked = True
        Me.ChkSingleInstance.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkSingleInstance.Location = New System.Drawing.Point(172, 39)
        Me.ChkSingleInstance.Name = "ChkSingleInstance"
        Me.ChkSingleInstance.Size = New System.Drawing.Size(99, 17)
        Me.ChkSingleInstance.TabIndex = 5
        Me.ChkSingleInstance.Text = "Single Instance"
        Me.ttSettings.SetToolTip(Me.ChkSingleInstance, "Running ScalA for a 2nd time or more will bring an" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  already open ScalA from the" &
        " same profile to the front." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "To make additional profiles copy, rename and/or mov" &
        "e ScalA.exe" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        Me.ChkSingleInstance.UseVisualStyleBackColor = True
        '
        'grpReserveSpace
        '
        Me.grpReserveSpace.Controls.Add(Me.NumBorderBot)
        Me.grpReserveSpace.Controls.Add(Me.NumBorderRight)
        Me.grpReserveSpace.Controls.Add(Me.NumBorderTop)
        Me.grpReserveSpace.Controls.Add(Me.NumBorderLeft)
        Me.grpReserveSpace.Controls.Add(Label10)
        Me.grpReserveSpace.Controls.Add(Label9)
        Me.grpReserveSpace.Controls.Add(Label8)
        Me.grpReserveSpace.Controls.Add(Label7)
        Me.grpReserveSpace.Controls.Add(Label11)
        Me.grpReserveSpace.Controls.Add(Label14)
        Me.grpReserveSpace.Controls.Add(Label13)
        Me.grpReserveSpace.Controls.Add(Label12)
        Me.grpReserveSpace.Location = New System.Drawing.Point(49, 9)
        Me.grpReserveSpace.Name = "grpReserveSpace"
        Me.grpReserveSpace.Size = New System.Drawing.Size(212, 67)
        Me.grpReserveSpace.TabIndex = 2
        Me.grpReserveSpace.TabStop = False
        Me.grpReserveSpace.Text = "Reserve Border"
        Me.ttSettings.SetToolTip(Me.grpReserveSpace, "Reserve a border around ScalA when Maximized" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Doesn't apply untill you Re-Maximiz" &
        "e" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Note: Values are promille")
        '
        'NumBorderBot
        '
        Me.NumBorderBot.Location = New System.Drawing.Point(140, 37)
        Me.NumBorderBot.Maximum = New Decimal(New Integer() {750, 0, 0, 0})
        Me.NumBorderBot.Name = "NumBorderBot"
        Me.NumBorderBot.Size = New System.Drawing.Size(50, 20)
        Me.NumBorderBot.TabIndex = 3
        Me.NumBorderBot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'NumBorderRight
        '
        Me.NumBorderRight.Location = New System.Drawing.Point(140, 18)
        Me.NumBorderRight.Maximum = New Decimal(New Integer() {750, 0, 0, 0})
        Me.NumBorderRight.Name = "NumBorderRight"
        Me.NumBorderRight.Size = New System.Drawing.Size(50, 20)
        Me.NumBorderRight.TabIndex = 1
        Me.NumBorderRight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'NumBorderTop
        '
        Me.NumBorderTop.Location = New System.Drawing.Point(38, 37)
        Me.NumBorderTop.Maximum = New Decimal(New Integer() {750, 0, 0, 0})
        Me.NumBorderTop.Name = "NumBorderTop"
        Me.NumBorderTop.Size = New System.Drawing.Size(50, 20)
        Me.NumBorderTop.TabIndex = 2
        Me.NumBorderTop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'NumBorderLeft
        '
        Me.NumBorderLeft.Location = New System.Drawing.Point(38, 18)
        Me.NumBorderLeft.Maximum = New Decimal(New Integer() {750, 0, 0, 0})
        Me.NumBorderLeft.Name = "NumBorderLeft"
        Me.NumBorderLeft.Size = New System.Drawing.Size(50, 20)
        Me.NumBorderLeft.TabIndex = 0
        Me.NumBorderLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ChkLessRowCol
        '
        Me.ChkLessRowCol.AutoSize = True
        Me.ChkLessRowCol.Checked = True
        Me.ChkLessRowCol.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkLessRowCol.Location = New System.Drawing.Point(87, 82)
        Me.ChkLessRowCol.Name = "ChkLessRowCol"
        Me.ChkLessRowCol.Size = New System.Drawing.Size(136, 17)
        Me.ChkLessRowCol.TabIndex = 4
        Me.ChkLessRowCol.Text = "One Less Row/Column"
        Me.ttSettings.SetToolTip(Me.ChkLessRowCol, "When Width is bigger than Height have one less Row " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "If Height is bigger than Wid" &
        "th have one less Column")
        Me.ChkLessRowCol.UseVisualStyleBackColor = True
        '
        'chkOverViewIsGame
        '
        Me.chkOverViewIsGame.AutoSize = True
        Me.chkOverViewIsGame.Location = New System.Drawing.Point(172, 54)
        Me.chkOverViewIsGame.Name = "chkOverViewIsGame"
        Me.chkOverViewIsGame.Size = New System.Drawing.Size(107, 17)
        Me.chkOverViewIsGame.TabIndex = 22
        Me.chkOverViewIsGame.Text = "Active Overview "
        Me.ttSettings.SetToolTip(Me.chkOverViewIsGame, "Have overview thumbnails function as game.")
        Me.chkOverViewIsGame.UseVisualStyleBackColor = True
        '
        'tbcSettings
        '
        Me.tbcSettings.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tbcSettings.Controls.Add(Me.tabResolutions)
        Me.tbcSettings.Controls.Add(Me.tabHotkeys)
        Me.tbcSettings.Controls.Add(Me.tabSortAndBL)
        Me.tbcSettings.Controls.Add(Me.tabMaximized)
        Me.tbcSettings.Controls.Add(Me.tabMisc)
        Me.tbcSettings.Location = New System.Drawing.Point(0, 0)
        Me.tbcSettings.Multiline = True
        Me.tbcSettings.Name = "tbcSettings"
        Me.tbcSettings.SelectedIndex = 0
        Me.tbcSettings.Size = New System.Drawing.Size(316, 165)
        Me.tbcSettings.TabIndex = 14
        Me.tbcSettings.TabStop = False
        '
        'tabResolutions
        '
        Me.tabResolutions.Controls.Add(Me.btnRestore)
        Me.tabResolutions.Controls.Add(Me.btnSort)
        Me.tabResolutions.Controls.Add(Me.btnGenerate)
        Me.tabResolutions.Controls.Add(Me.txtResolutions)
        Me.tabResolutions.Location = New System.Drawing.Point(4, 25)
        Me.tabResolutions.Name = "tabResolutions"
        Me.tabResolutions.Padding = New System.Windows.Forms.Padding(3)
        Me.tabResolutions.Size = New System.Drawing.Size(308, 136)
        Me.tabResolutions.TabIndex = 0
        Me.tabResolutions.Text = "Resolutions"
        Me.tabResolutions.UseVisualStyleBackColor = True
        '
        'btnRestore
        '
        Me.btnRestore.Location = New System.Drawing.Point(170, 104)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(75, 23)
        Me.btnRestore.TabIndex = 5
        Me.btnRestore.Text = "Restore"
        Me.btnRestore.UseVisualStyleBackColor = True
        '
        'btnSort
        '
        Me.btnSort.Location = New System.Drawing.Point(170, 38)
        Me.btnSort.Name = "btnSort"
        Me.btnSort.Size = New System.Drawing.Size(75, 23)
        Me.btnSort.TabIndex = 3
        Me.btnSort.Text = "Sort"
        Me.btnSort.UseVisualStyleBackColor = True
        '
        'btnGenerate
        '
        Me.btnGenerate.ContextMenuStrip = Me.cmsGenerate
        Me.btnGenerate.Location = New System.Drawing.Point(170, 9)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(75, 23)
        Me.btnGenerate.TabIndex = 2
        Me.btnGenerate.Text = "Generate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'cmsGenerate
        '
        Me.cmsGenerate.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.X60043ToolStripMenuItem, Me.X720169ToolStripMenuItem, ToolStripSeparator1, Me.FromToolStripMenuItem})
        Me.cmsGenerate.Name = "cmsGenerate"
        Me.cmsGenerate.Size = New System.Drawing.Size(155, 76)
        Me.cmsGenerate.Tag = "800x600 (4:3)"
        '
        'X60043ToolStripMenuItem
        '
        Me.X60043ToolStripMenuItem.Name = "X60043ToolStripMenuItem"
        Me.X60043ToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.X60043ToolStripMenuItem.Tag = "800x600 (4:3)"
        Me.X60043ToolStripMenuItem.Text = "800x600 (4:3)"
        '
        'X720169ToolStripMenuItem
        '
        Me.X720169ToolStripMenuItem.Name = "X720169ToolStripMenuItem"
        Me.X720169ToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.X720169ToolStripMenuItem.Tag = "1280x720 (16:9)"
        Me.X720169ToolStripMenuItem.Text = "1280x720 (16:9)"
        '
        'FromToolStripMenuItem
        '
        Me.FromToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DummyToolStripMenuItem})
        Me.FromToolStripMenuItem.Image = Global.ScalA.My.Resources.Resources.moa31
        Me.FromToolStripMenuItem.Name = "FromToolStripMenuItem"
        Me.FromToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.FromToolStripMenuItem.Text = "From"
        '
        'DummyToolStripMenuItem
        '
        Me.DummyToolStripMenuItem.Enabled = False
        Me.DummyToolStripMenuItem.Name = "DummyToolStripMenuItem"
        Me.DummyToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
        Me.DummyToolStripMenuItem.Text = "(Dummy)"
        '
        'txtResolutions
        '
        Me.txtResolutions.Location = New System.Drawing.Point(70, 0)
        Me.txtResolutions.Multiline = True
        Me.txtResolutions.Name = "txtResolutions"
        Me.txtResolutions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtResolutions.Size = New System.Drawing.Size(94, 135)
        Me.txtResolutions.TabIndex = 1
        Me.txtResolutions.Text = "800x600" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1200x900" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1600x1200" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2000x1500" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2400x1800" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2800x2100" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3200x2400" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3600x27" &
    "00" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "4000x3000" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "4400x3300"
        '
        'tabHotkeys
        '
        Me.tabHotkeys.Controls.Add(Me.grpCycleShortcut)
        Me.tabHotkeys.Controls.Add(Me.grpOverviewShortcut)
        Me.tabHotkeys.Location = New System.Drawing.Point(4, 25)
        Me.tabHotkeys.Name = "tabHotkeys"
        Me.tabHotkeys.Size = New System.Drawing.Size(308, 136)
        Me.tabHotkeys.TabIndex = 2
        Me.tabHotkeys.Text = "Hotkeys"
        Me.tabHotkeys.UseVisualStyleBackColor = True
        '
        'grpCycleShortcut
        '
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleDownShift)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleUpShift)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleDownAlt)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleDownCtrl)
        Me.grpCycleShortcut.Controls.Add(Me.txtCycleKeyDown)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleAlts)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleUpAlt)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleUpCtrl)
        Me.grpCycleShortcut.Controls.Add(Me.txtCycleKeyUp)
        Me.grpCycleShortcut.Location = New System.Drawing.Point(46, 59)
        Me.grpCycleShortcut.Name = "grpCycleShortcut"
        Me.grpCycleShortcut.Size = New System.Drawing.Size(214, 67)
        Me.grpCycleShortcut.TabIndex = 1
        Me.grpCycleShortcut.TabStop = False
        '
        'chkCycleDownShift
        '
        Me.chkCycleDownShift.AutoSize = True
        Me.chkCycleDownShift.Location = New System.Drawing.Point(84, 47)
        Me.chkCycleDownShift.Name = "chkCycleDownShift"
        Me.chkCycleDownShift.Size = New System.Drawing.Size(47, 17)
        Me.chkCycleDownShift.TabIndex = 15
        Me.chkCycleDownShift.TabStop = False
        Me.chkCycleDownShift.Text = "Shift"
        Me.chkCycleDownShift.UseVisualStyleBackColor = True
        '
        'chkCycleUpShift
        '
        Me.chkCycleUpShift.AutoSize = True
        Me.chkCycleUpShift.Location = New System.Drawing.Point(84, 26)
        Me.chkCycleUpShift.Name = "chkCycleUpShift"
        Me.chkCycleUpShift.Size = New System.Drawing.Size(47, 17)
        Me.chkCycleUpShift.TabIndex = 14
        Me.chkCycleUpShift.TabStop = False
        Me.chkCycleUpShift.Text = "Shift"
        Me.chkCycleUpShift.UseVisualStyleBackColor = True
        '
        'chkCycleDownAlt
        '
        Me.chkCycleDownAlt.AutoSize = True
        Me.chkCycleDownAlt.Location = New System.Drawing.Point(46, 47)
        Me.chkCycleDownAlt.Name = "chkCycleDownAlt"
        Me.chkCycleDownAlt.Size = New System.Drawing.Size(38, 17)
        Me.chkCycleDownAlt.TabIndex = 13
        Me.chkCycleDownAlt.TabStop = False
        Me.chkCycleDownAlt.Text = "Alt"
        Me.chkCycleDownAlt.UseVisualStyleBackColor = True
        '
        'chkCycleDownCtrl
        '
        Me.chkCycleDownCtrl.AutoSize = True
        Me.chkCycleDownCtrl.Location = New System.Drawing.Point(6, 47)
        Me.chkCycleDownCtrl.Name = "chkCycleDownCtrl"
        Me.chkCycleDownCtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkCycleDownCtrl.TabIndex = 12
        Me.chkCycleDownCtrl.TabStop = False
        Me.chkCycleDownCtrl.Text = "Ctrl"
        Me.chkCycleDownCtrl.UseVisualStyleBackColor = True
        '
        'txtCycleKeyDown
        '
        Me.txtCycleKeyDown.Location = New System.Drawing.Point(134, 45)
        Me.txtCycleKeyDown.Name = "txtCycleKeyDown"
        Me.txtCycleKeyDown.Size = New System.Drawing.Size(74, 20)
        Me.txtCycleKeyDown.TabIndex = 11
        Me.txtCycleKeyDown.TabStop = False
        '
        'chkCycleAlts
        '
        Me.chkCycleAlts.AutoSize = True
        Me.chkCycleAlts.Location = New System.Drawing.Point(0, 4)
        Me.chkCycleAlts.Name = "chkCycleAlts"
        Me.chkCycleAlts.Size = New System.Drawing.Size(122, 17)
        Me.chkCycleAlts.TabIndex = 10
        Me.chkCycleAlts.TabStop = False
        Me.chkCycleAlts.Text = "Cycle Alts Up/Down"
        Me.chkCycleAlts.UseVisualStyleBackColor = True
        '
        'chkCycleUpAlt
        '
        Me.chkCycleUpAlt.AutoSize = True
        Me.chkCycleUpAlt.Location = New System.Drawing.Point(46, 26)
        Me.chkCycleUpAlt.Name = "chkCycleUpAlt"
        Me.chkCycleUpAlt.Size = New System.Drawing.Size(38, 17)
        Me.chkCycleUpAlt.TabIndex = 9
        Me.chkCycleUpAlt.TabStop = False
        Me.chkCycleUpAlt.Text = "Alt"
        Me.chkCycleUpAlt.UseVisualStyleBackColor = True
        '
        'chkCycleUpCtrl
        '
        Me.chkCycleUpCtrl.AutoSize = True
        Me.chkCycleUpCtrl.Location = New System.Drawing.Point(6, 26)
        Me.chkCycleUpCtrl.Name = "chkCycleUpCtrl"
        Me.chkCycleUpCtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkCycleUpCtrl.TabIndex = 8
        Me.chkCycleUpCtrl.TabStop = False
        Me.chkCycleUpCtrl.Text = "Ctrl"
        Me.chkCycleUpCtrl.UseVisualStyleBackColor = True
        '
        'txtCycleKeyUp
        '
        Me.txtCycleKeyUp.Location = New System.Drawing.Point(134, 24)
        Me.txtCycleKeyUp.Name = "txtCycleKeyUp"
        Me.txtCycleKeyUp.Size = New System.Drawing.Size(74, 20)
        Me.txtCycleKeyUp.TabIndex = 7
        Me.txtCycleKeyUp.TabStop = False
        '
        'grpOverviewShortcut
        '
        Me.grpOverviewShortcut.Controls.Add(Me.chkStoShift)
        Me.grpOverviewShortcut.Controls.Add(Me.txtStoKey)
        Me.grpOverviewShortcut.Controls.Add(Me.chkStoAlt)
        Me.grpOverviewShortcut.Controls.Add(Me.chkStoCtrl)
        Me.grpOverviewShortcut.Controls.Add(Me.chkSwitchToOverview)
        Me.grpOverviewShortcut.Location = New System.Drawing.Point(46, 8)
        Me.grpOverviewShortcut.Name = "grpOverviewShortcut"
        Me.grpOverviewShortcut.Size = New System.Drawing.Size(214, 45)
        Me.grpOverviewShortcut.TabIndex = 0
        Me.grpOverviewShortcut.TabStop = False
        '
        'chkStoShift
        '
        Me.chkStoShift.AutoSize = True
        Me.chkStoShift.Location = New System.Drawing.Point(84, 21)
        Me.chkStoShift.Name = "chkStoShift"
        Me.chkStoShift.Size = New System.Drawing.Size(47, 17)
        Me.chkStoShift.TabIndex = 7
        Me.chkStoShift.TabStop = False
        Me.chkStoShift.Text = "Shift"
        Me.chkStoShift.UseVisualStyleBackColor = True
        '
        'txtStoKey
        '
        Me.txtStoKey.Location = New System.Drawing.Point(134, 18)
        Me.txtStoKey.Name = "txtStoKey"
        Me.txtStoKey.Size = New System.Drawing.Size(74, 20)
        Me.txtStoKey.TabIndex = 6
        Me.txtStoKey.TabStop = False
        '
        'chkStoAlt
        '
        Me.chkStoAlt.AutoSize = True
        Me.chkStoAlt.Location = New System.Drawing.Point(46, 21)
        Me.chkStoAlt.Name = "chkStoAlt"
        Me.chkStoAlt.Size = New System.Drawing.Size(38, 17)
        Me.chkStoAlt.TabIndex = 5
        Me.chkStoAlt.TabStop = False
        Me.chkStoAlt.Text = "Alt"
        Me.chkStoAlt.UseVisualStyleBackColor = True
        '
        'chkStoCtrl
        '
        Me.chkStoCtrl.AutoSize = True
        Me.chkStoCtrl.Location = New System.Drawing.Point(6, 21)
        Me.chkStoCtrl.Name = "chkStoCtrl"
        Me.chkStoCtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkStoCtrl.TabIndex = 3
        Me.chkStoCtrl.TabStop = False
        Me.chkStoCtrl.Text = "Ctrl"
        Me.chkStoCtrl.UseVisualStyleBackColor = True
        '
        'chkSwitchToOverview
        '
        Me.chkSwitchToOverview.AutoSize = True
        Me.chkSwitchToOverview.Location = New System.Drawing.Point(0, 0)
        Me.chkSwitchToOverview.Name = "chkSwitchToOverview"
        Me.chkSwitchToOverview.Size = New System.Drawing.Size(118, 17)
        Me.chkSwitchToOverview.TabIndex = 2
        Me.chkSwitchToOverview.TabStop = False
        Me.chkSwitchToOverview.Text = "Switch to Overview"
        Me.chkSwitchToOverview.UseVisualStyleBackColor = True
        '
        'tabSortAndBL
        '
        Me.tabSortAndBL.Controls.Add(Me.btnTest)
        Me.tabSortAndBL.Controls.Add(Label5)
        Me.tabSortAndBL.Controls.Add(Label4)
        Me.tabSortAndBL.Controls.Add(Me.btnHelp)
        Me.tabSortAndBL.Controls.Add(Me.txtBotSort)
        Me.tabSortAndBL.Controls.Add(Me.txtTopSort)
        Me.tabSortAndBL.Controls.Add(Me.chkWhitelist)
        Me.tabSortAndBL.Location = New System.Drawing.Point(4, 25)
        Me.tabSortAndBL.Margin = New System.Windows.Forms.Padding(0)
        Me.tabSortAndBL.Name = "tabSortAndBL"
        Me.tabSortAndBL.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSortAndBL.Size = New System.Drawing.Size(308, 136)
        Me.tabSortAndBL.TabIndex = 3
        Me.tabSortAndBL.Text = "Sort"
        Me.tabSortAndBL.UseVisualStyleBackColor = True
        '
        'btnTest
        '
        Me.btnTest.Location = New System.Drawing.Point(142, 47)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(21, 61)
        Me.btnTest.TabIndex = 5
        Me.btnTest.Text = "Test"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'btnHelp
        '
        Me.btnHelp.BackgroundImage = Global.ScalA.My.Resources.Resources.About
        Me.btnHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnHelp.Location = New System.Drawing.Point(142, 18)
        Me.btnHelp.Margin = New System.Windows.Forms.Padding(0)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(22, 23)
        Me.btnHelp.TabIndex = 2
        Me.btnHelp.UseVisualStyleBackColor = True
        '
        'txtBotSort
        '
        Me.txtBotSort.Location = New System.Drawing.Point(168, 15)
        Me.txtBotSort.Multiline = True
        Me.txtBotSort.Name = "txtBotSort"
        Me.txtBotSort.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtBotSort.Size = New System.Drawing.Size(130, 120)
        Me.txtBotSort.TabIndex = 1
        Me.txtBotSort.Text = "Someone"
        '
        'txtTopSort
        '
        Me.txtTopSort.Location = New System.Drawing.Point(8, 15)
        Me.txtTopSort.Multiline = True
        Me.txtTopSort.Name = "txtTopSort"
        Me.txtTopSort.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtTopSort.Size = New System.Drawing.Size(130, 120)
        Me.txtTopSort.TabIndex = 0
        Me.txtTopSort.Text = "Someone"
        '
        'chkWhitelist
        '
        Me.chkWhitelist.AutoSize = True
        Me.chkWhitelist.Location = New System.Drawing.Point(115, -1)
        Me.chkWhitelist.Name = "chkWhitelist"
        Me.chkWhitelist.Size = New System.Drawing.Size(66, 17)
        Me.chkWhitelist.TabIndex = 6
        Me.chkWhitelist.Text = "Whitelist"
        Me.chkWhitelist.UseVisualStyleBackColor = True
        '
        'tabMaximized
        '
        Me.tabMaximized.Controls.Add(Me.ChkLessRowCol)
        Me.tabMaximized.Controls.Add(Me.NumExtraMax)
        Me.tabMaximized.Controls.Add(Label6)
        Me.tabMaximized.Controls.Add(Me.grpReserveSpace)
        Me.tabMaximized.Location = New System.Drawing.Point(4, 25)
        Me.tabMaximized.Name = "tabMaximized"
        Me.tabMaximized.Size = New System.Drawing.Size(308, 136)
        Me.tabMaximized.TabIndex = 4
        Me.tabMaximized.Text = "Maximized"
        Me.tabMaximized.UseVisualStyleBackColor = True
        '
        'NumExtraMax
        '
        Me.NumExtraMax.Location = New System.Drawing.Point(193, 102)
        Me.NumExtraMax.Maximum = New Decimal(New Integer() {9, 0, 0, 0})
        Me.NumExtraMax.Name = "NumExtraMax"
        Me.NumExtraMax.Size = New System.Drawing.Size(40, 20)
        Me.NumExtraMax.TabIndex = 5
        Me.NumExtraMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tabMisc
        '
        Me.tabMisc.Controls.Add(Me.chkOverViewIsGame)
        Me.tabMisc.Controls.Add(Me.ChkSingleInstance)
        Me.tabMisc.Controls.Add(Me.chkDoAlign)
        Me.tabMisc.Controls.Add(Me.chkCycleOnClose)
        Me.tabMisc.Controls.Add(Me.grpAlign)
        Me.tabMisc.Controls.Add(Me.txtExe)
        Me.tabMisc.Controls.Add(Me.chkTopMost)
        Me.tabMisc.Controls.Add(Me.chkRoundCorners)
        Me.tabMisc.Controls.Add(grpQLPath)
        Me.tabMisc.Controls.Add(Label3)
        Me.tabMisc.Controls.Add(Me.txtClass)
        Me.tabMisc.Location = New System.Drawing.Point(4, 25)
        Me.tabMisc.Name = "tabMisc"
        Me.tabMisc.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMisc.Size = New System.Drawing.Size(308, 136)
        Me.tabMisc.TabIndex = 1
        Me.tabMisc.Text = "Misc"
        Me.tabMisc.UseVisualStyleBackColor = True
        '
        'chkDoAlign
        '
        Me.chkDoAlign.AutoSize = True
        Me.chkDoAlign.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDoAlign.Location = New System.Drawing.Point(200, 70)
        Me.chkDoAlign.Name = "chkDoAlign"
        Me.chkDoAlign.Size = New System.Drawing.Size(66, 16)
        Me.chkDoAlign.TabIndex = 8
        Me.chkDoAlign.Text = "Alignment"
        Me.chkDoAlign.UseVisualStyleBackColor = True
        '
        'grpAlign
        '
        Me.grpAlign.Controls.Add(Me.btnResetAlign)
        Me.grpAlign.Controls.Add(Me.numXoffset)
        Me.grpAlign.Controls.Add(Label2)
        Me.grpAlign.Controls.Add(Label1)
        Me.grpAlign.Controls.Add(Me.numYoffset)
        Me.grpAlign.Enabled = False
        Me.grpAlign.Location = New System.Drawing.Point(200, 70)
        Me.grpAlign.Name = "grpAlign"
        Me.grpAlign.Size = New System.Drawing.Size(100, 62)
        Me.grpAlign.TabIndex = 21
        Me.grpAlign.TabStop = False
        '
        'btnResetAlign
        '
        Me.btnResetAlign.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnResetAlign.Location = New System.Drawing.Point(65, -1)
        Me.btnResetAlign.Name = "btnResetAlign"
        Me.btnResetAlign.Size = New System.Drawing.Size(33, 17)
        Me.btnResetAlign.TabIndex = 11
        Me.btnResetAlign.Text = "Reset"
        Me.btnResetAlign.UseVisualStyleBackColor = True
        '
        'numXoffset
        '
        Me.numXoffset.Location = New System.Drawing.Point(26, 17)
        Me.numXoffset.Maximum = New Decimal(New Integer() {4000, 0, 0, 0})
        Me.numXoffset.Minimum = New Decimal(New Integer() {4000, 0, 0, -2147483648})
        Me.numXoffset.Name = "numXoffset"
        Me.numXoffset.Size = New System.Drawing.Size(72, 20)
        Me.numXoffset.TabIndex = 9
        Me.numXoffset.Tag = "0"
        Me.numXoffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'numYoffset
        '
        Me.numYoffset.Location = New System.Drawing.Point(26, 38)
        Me.numYoffset.Maximum = New Decimal(New Integer() {4000, 0, 0, 0})
        Me.numYoffset.Minimum = New Decimal(New Integer() {4000, 0, 0, -2147483648})
        Me.numYoffset.Name = "numYoffset"
        Me.numYoffset.Size = New System.Drawing.Size(72, 20)
        Me.numYoffset.TabIndex = 10
        Me.numYoffset.Tag = "1"
        Me.numYoffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtExe
        '
        Me.txtExe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExe.Location = New System.Drawing.Point(12, 89)
        Me.txtExe.Name = "txtExe"
        Me.txtExe.Size = New System.Drawing.Size(154, 20)
        Me.txtExe.TabIndex = 6
        Me.txtExe.Text = "moac | new"
        '
        'chkTopMost
        '
        Me.chkTopMost.AutoSize = True
        Me.chkTopMost.Location = New System.Drawing.Point(20, 54)
        Me.chkTopMost.Name = "chkTopMost"
        Me.chkTopMost.Size = New System.Drawing.Size(98, 17)
        Me.chkTopMost.TabIndex = 3
        Me.chkTopMost.Text = "Always On Top"
        Me.chkTopMost.UseVisualStyleBackColor = True
        '
        'chkRoundCorners
        '
        Me.chkRoundCorners.AutoSize = True
        Me.chkRoundCorners.Location = New System.Drawing.Point(20, 39)
        Me.chkRoundCorners.Name = "chkRoundCorners"
        Me.chkRoundCorners.Size = New System.Drawing.Size(109, 17)
        Me.chkRoundCorners.TabIndex = 2
        Me.chkRoundCorners.Text = "Rounded Corners"
        Me.chkRoundCorners.UseVisualStyleBackColor = True
        '
        'txtClass
        '
        Me.txtClass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClass.Location = New System.Drawing.Point(12, 110)
        Me.txtClass.Name = "txtClass"
        Me.txtClass.Size = New System.Drawing.Size(182, 20)
        Me.txtClass.TabIndex = 7
        Me.txtClass.Text = "MAINWNDMOAC | 䅍义乗䵄䅏C"
        '
        'FrmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ClientSize = New System.Drawing.Size(314, 198)
        Me.Controls.Add(Me.tbcSettings)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FrmSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ScalA Settings"
        Me.TopMost = True
        grpQLPath.ResumeLayout(False)
        grpQLPath.PerformLayout()
        Me.cmsQLFolder.ResumeLayout(False)
        Me.grpReserveSpace.ResumeLayout(False)
        Me.grpReserveSpace.PerformLayout()
        CType(Me.NumBorderBot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumBorderRight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumBorderTop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumBorderLeft, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbcSettings.ResumeLayout(False)
        Me.tabResolutions.ResumeLayout(False)
        Me.tabResolutions.PerformLayout()
        Me.cmsGenerate.ResumeLayout(False)
        Me.tabHotkeys.ResumeLayout(False)
        Me.grpCycleShortcut.ResumeLayout(False)
        Me.grpCycleShortcut.PerformLayout()
        Me.grpOverviewShortcut.ResumeLayout(False)
        Me.grpOverviewShortcut.PerformLayout()
        Me.tabSortAndBL.ResumeLayout(False)
        Me.tabSortAndBL.PerformLayout()
        Me.tabMaximized.ResumeLayout(False)
        Me.tabMaximized.PerformLayout()
        CType(Me.NumExtraMax, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabMisc.ResumeLayout(False)
        Me.tabMisc.PerformLayout()
        Me.grpAlign.ResumeLayout(False)
        Me.grpAlign.PerformLayout()
        CType(Me.numXoffset, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numYoffset, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents tmrAlign As Timer
    Friend WithEvents cmsQLFolder As ContextMenuStrip
    Friend WithEvents OpenInExplorerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ttSettings As ToolTip
    Friend WithEvents tabResolutions As TabPage
    Friend WithEvents txtResolutions As TextBox
    Friend WithEvents tabHotkeys As TabPage
    Friend WithEvents grpCycleShortcut As GroupBox
    Friend WithEvents txtCycleKeyUp As TextBox
    Friend WithEvents grpOverviewShortcut As GroupBox
    Friend WithEvents txtStoKey As TextBox
    Friend WithEvents chkStoAlt As CheckBox
    Friend WithEvents chkStoCtrl As CheckBox
    Friend WithEvents chkSwitchToOverview As CheckBox
    Friend WithEvents tabMisc As TabPage
    Friend WithEvents chkDoAlign As CheckBox
    Friend WithEvents grpAlign As GroupBox
    Friend WithEvents btnResetAlign As Button
    Friend WithEvents numYoffset As NumericUpDown
    Friend WithEvents numXoffset As NumericUpDown
    Friend WithEvents txtExe As TextBox
    Friend WithEvents chkTopMost As CheckBox
    Friend WithEvents chkRoundCorners As CheckBox
    Friend WithEvents btnOpenFolderDialog As Button
    Friend WithEvents txtQuickLaunchPath As TextBox
    Friend WithEvents txtClass As TextBox
    Friend WithEvents chkCycleAlts As CheckBox
    Friend WithEvents chkCycleUpAlt As CheckBox
    Friend WithEvents chkCycleUpCtrl As CheckBox
    Friend WithEvents chkStoShift As CheckBox
    Friend WithEvents chkCycleDownShift As CheckBox
    Friend WithEvents chkCycleUpShift As CheckBox
    Friend WithEvents chkCycleDownAlt As CheckBox
    Friend WithEvents chkCycleDownCtrl As CheckBox
    Friend WithEvents txtCycleKeyDown As TextBox
    Friend WithEvents btnRestore As Button
    Friend WithEvents btnSort As Button
    Friend WithEvents btnGenerate As Button
    Friend WithEvents cmsGenerate As ContextMenuStrip
    Friend WithEvents X60043ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents X720169ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FromToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DummyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents chkCycleOnClose As CheckBox
    Friend WithEvents tabSortAndBL As TabPage
    Friend WithEvents btnHelp As Button
    Friend WithEvents txtBotSort As TextBox
    Friend WithEvents txtTopSort As TextBox
    Friend WithEvents btnTest As Button
    Friend WithEvents chkWhitelist As CheckBox
    Friend WithEvents ChkSingleInstance As CheckBox
    Friend WithEvents tabMaximized As TabPage
    Friend WithEvents grpReserveSpace As GroupBox
    Friend WithEvents NumBorderBot As NumericUpDown
    Friend WithEvents NumBorderRight As NumericUpDown
    Friend WithEvents NumBorderTop As NumericUpDown
    Friend WithEvents NumBorderLeft As NumericUpDown
    Friend WithEvents tbcSettings As TabControl
    Friend WithEvents ChkLessRowCol As CheckBox
    Friend WithEvents NumExtraMax As NumericUpDown
    Friend WithEvents chkOverViewIsGame As CheckBox
End Class
