<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial NotInheritable Class FrmSettings
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
        Dim ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
        Dim Label3 As System.Windows.Forms.Label
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
        Dim Label15 As System.Windows.Forms.Label
        Dim Label19 As System.Windows.Forms.Label
        Dim Label18 As System.Windows.Forms.Label
        Dim Label17 As System.Windows.Forms.Label
        Dim grpAlterOverviewMinMax As System.Windows.Forms.GroupBox
        Dim Label6 As System.Windows.Forms.Label
        Dim grpQLPath As System.Windows.Forms.GroupBox
        Dim gbFilter As System.Windows.Forms.GroupBox
        Dim Label1 As System.Windows.Forms.Label
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmSettings))
        Me.btnGoToAdjustHotkey = New System.Windows.Forms.Button()
        Me.chkApplyAlterNormal = New System.Windows.Forms.CheckBox()
        Me.ChkLessRowCol = New System.Windows.Forms.CheckBox()
        Me.NumExtraMax = New System.Windows.Forms.NumericUpDown()
        Me.btnOpenFolderDialog = New System.Windows.Forms.Button()
        Me.cmsQLFolder = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenInExplorerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResetIconCacheToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtQuickLaunchPath = New System.Windows.Forms.TextBox()
        Me.TxtFilterAddExt = New System.Windows.Forms.TextBox()
        Me.lblDefaultFilter = New System.Windows.Forms.Label()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.tmrAlign = New System.Windows.Forms.Timer(Me.components)
        Me.ttSettings = New System.Windows.Forms.ToolTip(Me.components)
        Me.ChkSingleInstance = New System.Windows.Forms.CheckBox()
        Me.grpReserveSpace = New System.Windows.Forms.GroupBox()
        Me.btnGrabCurrent = New System.Windows.Forms.Button()
        Me.NumBorderBot = New System.Windows.Forms.NumericUpDown()
        Me.NumBorderRight = New System.Windows.Forms.NumericUpDown()
        Me.NumBorderTop = New System.Windows.Forms.NumericUpDown()
        Me.NumBorderLeft = New System.Windows.Forms.NumericUpDown()
        Me.chkOverViewIsGame = New System.Windows.Forms.CheckBox()
        Me.cboScalingMode = New System.Windows.Forms.ComboBox()
        Me.chkCycleOnClose = New System.Windows.Forms.CheckBox()
        Me.chkCheckForUpdate = New System.Windows.Forms.CheckBox()
        Me.cmsUpdate = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CheckNowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenChangelogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChkMinMin = New System.Windows.Forms.CheckBox()
        Me.chkCloseAll = New System.Windows.Forms.CheckBox()
        Me.chkHoverActivate = New System.Windows.Forms.CheckBox()
        Me.chkShowEnd = New System.Windows.Forms.CheckBox()
        Me.chkAlterOverview = New System.Windows.Forms.CheckBox()
        Me.txtAlterOverviewStarKey = New System.Windows.Forms.TextBox()
        Me.txtAlterOverviewMinKey = New System.Windows.Forms.TextBox()
        Me.txtAlterOverviewPlusKey = New System.Windows.Forms.TextBox()
        Me.chkOnlyEsc = New System.Windows.Forms.CheckBox()
        Me.chkAutoCloseSomeone = New System.Windows.Forms.CheckBox()
        Me.chkNoAltTab = New System.Windows.Forms.CheckBox()
        Me.pb100PWarning = New System.Windows.Forms.PictureBox()
        Me.chkAutoCloseOnlyOnNoSome = New System.Windows.Forms.CheckBox()
        Me.chkAllowShiftEsc = New System.Windows.Forms.CheckBox()
        Me.lblElevated = New System.Windows.Forms.Label()
        Me.pbUnElevate = New System.Windows.Forms.PictureBox()
        Me.ChkQLShowHidden = New System.Windows.Forms.CheckBox()
        Me.btnRefreshICdisplay = New System.Windows.Forms.Button()
        Me.lblICSize = New System.Windows.Forms.Label()
        Me.ChkQLResolveLnk = New System.Windows.Forms.CheckBox()
        Me.tbcSettings = New System.Windows.Forms.TabControl()
        Me.tabResolutions = New System.Windows.Forms.TabPage()
        Me.ChkSizeBorder = New System.Windows.Forms.CheckBox()
        Me.btnAddCurrentRes = New System.Windows.Forms.Button()
        Me.btnRestore = New System.Windows.Forms.Button()
        Me.cmsRestore = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.LastSavedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DefaultToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.cmsGenerate = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.X60043ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.X720169ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FromToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DummyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtResolutions = New System.Windows.Forms.TextBox()
        Me.tabHotkeys = New System.Windows.Forms.TabPage()
        Me.chkBlockWin = New System.Windows.Forms.CheckBox()
        Me.grpAlterOverview = New System.Windows.Forms.GroupBox()
        Me.chkAlterOverviewStarAlt = New System.Windows.Forms.CheckBox()
        Me.chkAlterOverviewStarShift = New System.Windows.Forms.CheckBox()
        Me.chkAlterOverviewStarWin = New System.Windows.Forms.CheckBox()
        Me.chkAlterOverviewStarCtrl = New System.Windows.Forms.CheckBox()
        Me.chkAlterOverviewMinAlt = New System.Windows.Forms.CheckBox()
        Me.chkAlterOverviewPlusALt = New System.Windows.Forms.CheckBox()
        Me.chkAlterOverviewMinShift = New System.Windows.Forms.CheckBox()
        Me.chkAlterOverviewPlusShift = New System.Windows.Forms.CheckBox()
        Me.chkAlterOverviewMinWin = New System.Windows.Forms.CheckBox()
        Me.chkAlterOverviewPlusWin = New System.Windows.Forms.CheckBox()
        Me.chkAlterOverviewMinCtrl = New System.Windows.Forms.CheckBox()
        Me.chkAlterOverviewPlusCtrl = New System.Windows.Forms.CheckBox()
        Me.chkToggleTopMost = New System.Windows.Forms.CheckBox()
        Me.grpToggleTopMost = New System.Windows.Forms.GroupBox()
        Me.txtTogTop = New System.Windows.Forms.TextBox()
        Me.chkTogTopAlt = New System.Windows.Forms.CheckBox()
        Me.chkTogTopShift = New System.Windows.Forms.CheckBox()
        Me.chkTogTopWin = New System.Windows.Forms.CheckBox()
        Me.chkTogTopCtrl = New System.Windows.Forms.CheckBox()
        Me.grpCloseAllShortcut = New System.Windows.Forms.GroupBox()
        Me.txtCloseAll = New System.Windows.Forms.TextBox()
        Me.chkCAALt = New System.Windows.Forms.CheckBox()
        Me.chkCAShift = New System.Windows.Forms.CheckBox()
        Me.chkCAWin = New System.Windows.Forms.CheckBox()
        Me.chkCACtrl = New System.Windows.Forms.CheckBox()
        Me.chkSwitchToOverview = New System.Windows.Forms.CheckBox()
        Me.chkCycleAlts = New System.Windows.Forms.CheckBox()
        Me.grpCycleShortcut = New System.Windows.Forms.GroupBox()
        Me.chkCycleDownAlt = New System.Windows.Forms.CheckBox()
        Me.txtCycleKeyDown = New System.Windows.Forms.TextBox()
        Me.chkCycleUpAlt = New System.Windows.Forms.CheckBox()
        Me.txtCycleKeyUp = New System.Windows.Forms.TextBox()
        Me.chkCycleDownShift = New System.Windows.Forms.CheckBox()
        Me.chkCycleUpShift = New System.Windows.Forms.CheckBox()
        Me.chkCycleDownWin = New System.Windows.Forms.CheckBox()
        Me.chkCycleUpWin = New System.Windows.Forms.CheckBox()
        Me.chkCycleDownCtrl = New System.Windows.Forms.CheckBox()
        Me.chkCycleUpCtrl = New System.Windows.Forms.CheckBox()
        Me.grpOverviewShortcut = New System.Windows.Forms.GroupBox()
        Me.txtStoKey = New System.Windows.Forms.TextBox()
        Me.chkStoAlt = New System.Windows.Forms.CheckBox()
        Me.chkStoShift = New System.Windows.Forms.CheckBox()
        Me.chkStoWin = New System.Windows.Forms.CheckBox()
        Me.chkStoCtrl = New System.Windows.Forms.CheckBox()
        Me.tabSortAndBL = New System.Windows.Forms.TabPage()
        Me.btnTest = New System.Windows.Forms.Button()
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.txtBotSort = New System.Windows.Forms.TextBox()
        Me.txtTopSort = New System.Windows.Forms.TextBox()
        Me.chkWhitelist = New System.Windows.Forms.CheckBox()
        Me.tabMaximized = New System.Windows.Forms.TabPage()
        Me.chkMinMaxOnSwitch = New System.Windows.Forms.CheckBox()
        Me.chkStartupMax = New System.Windows.Forms.CheckBox()
        Me.TabQL = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblICacheCount = New System.Windows.Forms.Label()
        Me.btnResetCache = New System.Windows.Forms.Button()
        Me.tabMisc = New System.Windows.Forms.TabPage()
        Me.cmbPriority = New System.Windows.Forms.ComboBox()
        Me.cmbTheme = New System.Windows.Forms.ComboBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtExe = New System.Windows.Forms.TextBox()
        Me.txtClass = New System.Windows.Forms.TextBox()
        Me.chkTopMost = New System.Windows.Forms.CheckBox()
        Me.chkRoundCorners = New System.Windows.Forms.CheckBox()
        Me.pnlElevation = New System.Windows.Forms.Panel()
        ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Label3 = New System.Windows.Forms.Label()
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
        Label15 = New System.Windows.Forms.Label()
        Label19 = New System.Windows.Forms.Label()
        Label18 = New System.Windows.Forms.Label()
        Label17 = New System.Windows.Forms.Label()
        grpAlterOverviewMinMax = New System.Windows.Forms.GroupBox()
        Label6 = New System.Windows.Forms.Label()
        grpQLPath = New System.Windows.Forms.GroupBox()
        gbFilter = New System.Windows.Forms.GroupBox()
        Label1 = New System.Windows.Forms.Label()
        grpAlterOverviewMinMax.SuspendLayout()
        CType(Me.NumExtraMax, System.ComponentModel.ISupportInitialize).BeginInit()
        grpQLPath.SuspendLayout()
        Me.cmsQLFolder.SuspendLayout()
        gbFilter.SuspendLayout()
        Me.grpReserveSpace.SuspendLayout()
        CType(Me.NumBorderBot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumBorderRight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumBorderTop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumBorderLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmsUpdate.SuspendLayout()
        CType(Me.pb100PWarning, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbUnElevate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbcSettings.SuspendLayout()
        Me.tabResolutions.SuspendLayout()
        Me.cmsRestore.SuspendLayout()
        Me.cmsGenerate.SuspendLayout()
        Me.tabHotkeys.SuspendLayout()
        Me.grpAlterOverview.SuspendLayout()
        Me.grpToggleTopMost.SuspendLayout()
        Me.grpCloseAllShortcut.SuspendLayout()
        Me.grpCycleShortcut.SuspendLayout()
        Me.grpOverviewShortcut.SuspendLayout()
        Me.tabSortAndBL.SuspendLayout()
        Me.tabMaximized.SuspendLayout()
        Me.TabQL.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.tabMisc.SuspendLayout()
        Me.pnlElevation.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStripMenuItem1
        '
        ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        ToolStripMenuItem1.Size = New System.Drawing.Size(179, 6)
        '
        'Label3
        '
        Label3.AutoSize = True
        Label3.Location = New System.Drawing.Point(297, 140)
        Label3.Name = "Label3"
        Label3.Size = New System.Drawing.Size(27, 13)
        Label3.TabIndex = 18
        Label3.Text = ".exe"
        '
        'ToolStripSeparator1
        '
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New System.Drawing.Size(150, 6)
        '
        'Label4
        '
        Label4.AutoSize = True
        Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Label4.Location = New System.Drawing.Point(54, 3)
        Label4.Name = "Label4"
        Label4.Size = New System.Drawing.Size(18, 9)
        Label4.TabIndex = 3
        Label4.Text = "Top"
        '
        'Label5
        '
        Label5.AutoSize = True
        Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Label5.Location = New System.Drawing.Point(225, 3)
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
        'Label15
        '
        Label15.AutoSize = True
        Label15.Location = New System.Drawing.Point(211, 36)
        Label15.Name = "Label15"
        Label15.Size = New System.Drawing.Size(72, 13)
        Label15.TabIndex = 7
        Label15.Text = "Scaling Mode"
        Me.ttSettings.SetToolTip(Label15, "Auto will use Pixel Mode when scaling factor is 2x or more.")
        '
        'Label19
        '
        Label19.AutoSize = True
        Label19.Location = New System.Drawing.Point(255, 71)
        Label19.Name = "Label19"
        Label19.Size = New System.Drawing.Size(11, 13)
        Label19.TabIndex = 26
        Label19.Text = "*"
        Me.ttSettings.SetToolTip(Label19, "Toggle One Less Row/Column")
        '
        'Label18
        '
        Label18.AutoSize = True
        Label18.Location = New System.Drawing.Point(255, 43)
        Label18.Name = "Label18"
        Label18.Size = New System.Drawing.Size(10, 13)
        Label18.TabIndex = 20
        Label18.Text = "-"
        Me.ttSettings.SetToolTip(Label18, "Decrease Extra Columns/Rows")
        '
        'Label17
        '
        Label17.AutoSize = True
        Label17.Location = New System.Drawing.Point(254, 20)
        Label17.Name = "Label17"
        Label17.Size = New System.Drawing.Size(13, 13)
        Label17.TabIndex = 19
        Label17.Text = "+"
        Me.ttSettings.SetToolTip(Label17, "Increase Extra Columns/Rows")
        '
        'grpAlterOverviewMinMax
        '
        grpAlterOverviewMinMax.Controls.Add(Me.btnGoToAdjustHotkey)
        grpAlterOverviewMinMax.Controls.Add(Me.chkApplyAlterNormal)
        grpAlterOverviewMinMax.Controls.Add(Me.ChkLessRowCol)
        grpAlterOverviewMinMax.Controls.Add(Me.NumExtraMax)
        grpAlterOverviewMinMax.Controls.Add(Label6)
        grpAlterOverviewMinMax.Location = New System.Drawing.Point(28, 86)
        grpAlterOverviewMinMax.Name = "grpAlterOverviewMinMax"
        grpAlterOverviewMinMax.Size = New System.Drawing.Size(156, 66)
        grpAlterOverviewMinMax.TabIndex = 31
        grpAlterOverviewMinMax.TabStop = False
        '
        'btnGoToAdjustHotkey
        '
        Me.btnGoToAdjustHotkey.Location = New System.Drawing.Point(140, 5)
        Me.btnGoToAdjustHotkey.Name = "btnGoToAdjustHotkey"
        Me.btnGoToAdjustHotkey.Size = New System.Drawing.Size(17, 12)
        Me.btnGoToAdjustHotkey.TabIndex = 33
        Me.btnGoToAdjustHotkey.Text = "H"
        Me.ttSettings.SetToolTip(Me.btnGoToAdjustHotkey, "Go to Hotkey Settings")
        Me.btnGoToAdjustHotkey.UseVisualStyleBackColor = True
        '
        'chkApplyAlterNormal
        '
        Me.chkApplyAlterNormal.AutoSize = True
        Me.chkApplyAlterNormal.Location = New System.Drawing.Point(14, 0)
        Me.chkApplyAlterNormal.Name = "chkApplyAlterNormal"
        Me.chkApplyAlterNormal.Size = New System.Drawing.Size(122, 17)
        Me.chkApplyAlterNormal.TabIndex = 32
        Me.chkApplyAlterNormal.Text = "Also Apply in Normal"
        Me.chkApplyAlterNormal.UseVisualStyleBackColor = True
        '
        'ChkLessRowCol
        '
        Me.ChkLessRowCol.AutoSize = True
        Me.ChkLessRowCol.Checked = True
        Me.ChkLessRowCol.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkLessRowCol.Location = New System.Drawing.Point(6, 20)
        Me.ChkLessRowCol.Name = "ChkLessRowCol"
        Me.ChkLessRowCol.Size = New System.Drawing.Size(136, 17)
        Me.ChkLessRowCol.TabIndex = 29
        Me.ChkLessRowCol.Text = "One Less Row/Column"
        Me.ttSettings.SetToolTip(Me.ChkLessRowCol, "When Width is bigger than Height have one less Row " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "If Height is bigger than Wid" &
        "th have one less Column")
        Me.ChkLessRowCol.UseVisualStyleBackColor = True
        '
        'NumExtraMax
        '
        Me.NumExtraMax.Location = New System.Drawing.Point(110, 40)
        Me.NumExtraMax.Maximum = New Decimal(New Integer() {9, 0, 0, 0})
        Me.NumExtraMax.Name = "NumExtraMax"
        Me.NumExtraMax.Size = New System.Drawing.Size(40, 20)
        Me.NumExtraMax.TabIndex = 30
        Me.NumExtraMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Label6.AutoSize = True
        Label6.Location = New System.Drawing.Point(3, 42)
        Label6.Name = "Label6"
        Label6.Size = New System.Drawing.Size(106, 13)
        Label6.TabIndex = 31
        Label6.Text = "Extra Columns/Rows"
        '
        'grpQLPath
        '
        grpQLPath.Controls.Add(Me.btnOpenFolderDialog)
        grpQLPath.Controls.Add(Me.txtQuickLaunchPath)
        grpQLPath.Location = New System.Drawing.Point(9, 16)
        grpQLPath.Name = "grpQLPath"
        grpQLPath.Size = New System.Drawing.Size(315, 40)
        grpQLPath.TabIndex = 17
        grpQLPath.TabStop = False
        grpQLPath.Text = "QuickLaunch Root Folder"
        '
        'btnOpenFolderDialog
        '
        Me.btnOpenFolderDialog.ContextMenuStrip = Me.cmsQLFolder
        Me.btnOpenFolderDialog.FlatAppearance.BorderSize = 0
        Me.btnOpenFolderDialog.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOpenFolderDialog.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOpenFolderDialog.Location = New System.Drawing.Point(286, 15)
        Me.btnOpenFolderDialog.Name = "btnOpenFolderDialog"
        Me.btnOpenFolderDialog.Size = New System.Drawing.Size(22, 20)
        Me.btnOpenFolderDialog.TabIndex = 1
        Me.btnOpenFolderDialog.Text = ".."
        Me.btnOpenFolderDialog.UseVisualStyleBackColor = True
        '
        'cmsQLFolder
        '
        Me.cmsQLFolder.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenInExplorerToolStripMenuItem, ToolStripMenuItem1, Me.ResetIconCacheToolStripMenuItem})
        Me.cmsQLFolder.Name = "cmsQLFolder"
        Me.cmsQLFolder.Size = New System.Drawing.Size(183, 54)
        '
        'OpenInExplorerToolStripMenuItem
        '
        Me.OpenInExplorerToolStripMenuItem.Name = "OpenInExplorerToolStripMenuItem"
        Me.OpenInExplorerToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.OpenInExplorerToolStripMenuItem.Text = "Open in File Explorer"
        '
        'ResetIconCacheToolStripMenuItem
        '
        Me.ResetIconCacheToolStripMenuItem.Name = "ResetIconCacheToolStripMenuItem"
        Me.ResetIconCacheToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.ResetIconCacheToolStripMenuItem.Text = "Reset Icon Cache"
        '
        'txtQuickLaunchPath
        '
        Me.txtQuickLaunchPath.Location = New System.Drawing.Point(6, 14)
        Me.txtQuickLaunchPath.Name = "txtQuickLaunchPath"
        Me.txtQuickLaunchPath.Size = New System.Drawing.Size(274, 20)
        Me.txtQuickLaunchPath.TabIndex = 0
        Me.txtQuickLaunchPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'gbFilter
        '
        gbFilter.Controls.Add(Me.TxtFilterAddExt)
        gbFilter.Controls.Add(Me.lblDefaultFilter)
        gbFilter.Location = New System.Drawing.Point(9, 85)
        gbFilter.Name = "gbFilter"
        gbFilter.Size = New System.Drawing.Size(315, 40)
        gbFilter.TabIndex = 2
        gbFilter.TabStop = False
        gbFilter.Text = "Extention Filter"
        '
        'TxtFilterAddExt
        '
        Me.TxtFilterAddExt.Location = New System.Drawing.Point(72, 14)
        Me.TxtFilterAddExt.Name = "TxtFilterAddExt"
        Me.TxtFilterAddExt.Size = New System.Drawing.Size(234, 20)
        Me.TxtFilterAddExt.TabIndex = 31
        '
        'lblDefaultFilter
        '
        Me.lblDefaultFilter.AutoSize = True
        Me.lblDefaultFilter.Location = New System.Drawing.Point(5, 17)
        Me.lblDefaultFilter.Name = "lblDefaultFilter"
        Me.lblDefaultFilter.Size = New System.Drawing.Size(70, 13)
        Me.lblDefaultFilter.TabIndex = 30
        Me.lblDefaultFilter.Text = "exe | lnk | url |"
        '
        'Label1
        '
        Label1.AutoSize = True
        Label1.Location = New System.Drawing.Point(147, 86)
        Label1.Name = "Label1"
        Label1.Size = New System.Drawing.Size(38, 13)
        Label1.TabIndex = 28
        Label1.Text = "Priority"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(171, 214)
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
        Me.btnCancel.Location = New System.Drawing.Point(254, 214)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(77, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.TabStop = False
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'ttSettings
        '
        Me.ttSettings.AutoPopDelay = 10000
        Me.ttSettings.InitialDelay = 500
        Me.ttSettings.ReshowDelay = 100
        '
        'ChkSingleInstance
        '
        Me.ChkSingleInstance.AutoSize = True
        Me.ChkSingleInstance.Location = New System.Drawing.Point(26, 85)
        Me.ChkSingleInstance.Name = "ChkSingleInstance"
        Me.ChkSingleInstance.Size = New System.Drawing.Size(99, 17)
        Me.ChkSingleInstance.TabIndex = 6
        Me.ChkSingleInstance.Text = "Single Instance"
        Me.ttSettings.SetToolTip(Me.ChkSingleInstance, "Running ScalA for a 2nd time or more will bring an" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  already open ScalA from the" &
        " same profile to the front." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "To make additional profiles copy, rename and/or mov" &
        "e ScalA.exe" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        Me.ChkSingleInstance.UseVisualStyleBackColor = True
        '
        'grpReserveSpace
        '
        Me.grpReserveSpace.Controls.Add(Me.btnGrabCurrent)
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
        Me.grpReserveSpace.Location = New System.Drawing.Point(63, 17)
        Me.grpReserveSpace.Name = "grpReserveSpace"
        Me.grpReserveSpace.Size = New System.Drawing.Size(212, 67)
        Me.grpReserveSpace.TabIndex = 2
        Me.grpReserveSpace.TabStop = False
        Me.grpReserveSpace.Text = "Reserve Border"
        Me.ttSettings.SetToolTip(Me.grpReserveSpace, "Reserve a border around ScalA when Maximized" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Doesn't apply untill you Re-Maximiz" &
        "e" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Note: Values are promille")
        '
        'btnGrabCurrent
        '
        Me.btnGrabCurrent.Image = Global.ScalA.My.Resources.Resources.scrollbar_arrow_small_hot
        Me.btnGrabCurrent.Location = New System.Drawing.Point(196, 5)
        Me.btnGrabCurrent.Name = "btnGrabCurrent"
        Me.btnGrabCurrent.Size = New System.Drawing.Size(17, 12)
        Me.btnGrabCurrent.TabIndex = 30
        Me.ttSettings.SetToolTip(Me.btnGrabCurrent, "Grab borders from current position")
        Me.btnGrabCurrent.UseVisualStyleBackColor = True
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
        'chkOverViewIsGame
        '
        Me.chkOverViewIsGame.AutoSize = True
        Me.chkOverViewIsGame.Location = New System.Drawing.Point(196, 10)
        Me.chkOverViewIsGame.Name = "chkOverViewIsGame"
        Me.chkOverViewIsGame.Size = New System.Drawing.Size(107, 17)
        Me.chkOverViewIsGame.TabIndex = 7
        Me.chkOverViewIsGame.Text = "Active Overview "
        Me.ttSettings.SetToolTip(Me.chkOverViewIsGame, "Have overview thumbnails function as game.")
        Me.chkOverViewIsGame.UseVisualStyleBackColor = True
        '
        'cboScalingMode
        '
        Me.cboScalingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboScalingMode.FormattingEnabled = True
        Me.cboScalingMode.Items.AddRange(New Object() {"Auto", "Blur", "Pixel"})
        Me.cboScalingMode.Location = New System.Drawing.Point(206, 52)
        Me.cboScalingMode.Name = "cboScalingMode"
        Me.cboScalingMode.Size = New System.Drawing.Size(106, 21)
        Me.cboScalingMode.TabIndex = 8
        Me.ttSettings.SetToolTip(Me.cboScalingMode, "Auto will use Pixel Mode when scaling factor is 2x or more.")
        '
        'chkCycleOnClose
        '
        Me.chkCycleOnClose.AutoSize = True
        Me.chkCycleOnClose.Location = New System.Drawing.Point(165, 129)
        Me.chkCycleOnClose.Name = "chkCycleOnClose"
        Me.chkCycleOnClose.Size = New System.Drawing.Size(96, 17)
        Me.chkCycleOnClose.TabIndex = 16
        Me.chkCycleOnClose.TabStop = False
        Me.chkCycleOnClose.Text = "Cycle on Close"
        Me.ttSettings.SetToolTip(Me.chkCycleOnClose, "Closing an alt will cycle to the next one.")
        Me.chkCycleOnClose.UseVisualStyleBackColor = True
        '
        'chkCheckForUpdate
        '
        Me.chkCheckForUpdate.AutoSize = True
        Me.chkCheckForUpdate.ContextMenuStrip = Me.cmsUpdate
        Me.chkCheckForUpdate.Location = New System.Drawing.Point(26, 111)
        Me.chkCheckForUpdate.Name = "chkCheckForUpdate"
        Me.chkCheckForUpdate.Size = New System.Drawing.Size(165, 17)
        Me.chkCheckForUpdate.TabIndex = 23
        Me.chkCheckForUpdate.Text = "Check For Update on Startup"
        Me.ttSettings.SetToolTip(Me.chkCheckForUpdate, "Check for Updates when ScalA starts." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Right Click for more options.")
        Me.chkCheckForUpdate.UseVisualStyleBackColor = True
        '
        'cmsUpdate
        '
        Me.cmsUpdate.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CheckNowToolStripMenuItem, Me.OpenChangelogToolStripMenuItem})
        Me.cmsUpdate.Name = "cmsUpdate"
        Me.cmsUpdate.Size = New System.Drawing.Size(165, 48)
        '
        'CheckNowToolStripMenuItem
        '
        Me.CheckNowToolStripMenuItem.Image = Global.ScalA.My.Resources.Resources.Sync
        Me.CheckNowToolStripMenuItem.Name = "CheckNowToolStripMenuItem"
        Me.CheckNowToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.CheckNowToolStripMenuItem.Text = "Check Now"
        '
        'OpenChangelogToolStripMenuItem
        '
        Me.OpenChangelogToolStripMenuItem.Image = Global.ScalA.My.Resources.Resources.List
        Me.OpenChangelogToolStripMenuItem.Name = "OpenChangelogToolStripMenuItem"
        Me.OpenChangelogToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.OpenChangelogToolStripMenuItem.Text = "Open Changelog"
        '
        'ChkMinMin
        '
        Me.ChkMinMin.AutoSize = True
        Me.ChkMinMin.Location = New System.Drawing.Point(191, 126)
        Me.ChkMinMin.Name = "ChkMinMin"
        Me.ChkMinMin.Size = New System.Drawing.Size(104, 17)
        Me.ChkMinMin.TabIndex = 30
        Me.ChkMinMin.Text = "Min. on Minimize"
        Me.ttSettings.SetToolTip(Me.ChkMinMin, "Minimizing ScalA will also Minimize Astonia" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Note: This has no effect on legacy c" &
        "lients")
        Me.ChkMinMin.UseVisualStyleBackColor = True
        '
        'chkCloseAll
        '
        Me.chkCloseAll.AutoSize = True
        Me.chkCloseAll.Location = New System.Drawing.Point(40, 202)
        Me.chkCloseAll.Name = "chkCloseAll"
        Me.chkCloseAll.Size = New System.Drawing.Size(152, 17)
        Me.chkCloseAll.TabIndex = 19
        Me.chkCloseAll.TabStop = False
        Me.chkCloseAll.Text = "Close All Clients and ScalA"
        Me.ttSettings.SetToolTip(Me.chkCloseAll, "Note: Whitelist setting from Sorting tab is in effect here")
        Me.chkCloseAll.UseVisualStyleBackColor = True
        '
        'chkHoverActivate
        '
        Me.chkHoverActivate.AutoSize = True
        Me.chkHoverActivate.Location = New System.Drawing.Point(196, 35)
        Me.chkHoverActivate.Name = "chkHoverActivate"
        Me.chkHoverActivate.Size = New System.Drawing.Size(112, 17)
        Me.chkHoverActivate.TabIndex = 26
        Me.chkHoverActivate.Text = "Activate on Hover"
        Me.ttSettings.SetToolTip(Me.chkHoverActivate, "Hovering over a client in ScalA wil activate and bring it to front.")
        Me.chkHoverActivate.UseVisualStyleBackColor = True
        '
        'chkShowEnd
        '
        Me.chkShowEnd.AutoSize = True
        Me.chkShowEnd.Location = New System.Drawing.Point(26, 10)
        Me.chkShowEnd.Name = "chkShowEnd"
        Me.chkShowEnd.Size = New System.Drawing.Size(111, 17)
        Me.chkShowEnd.TabIndex = 27
        Me.chkShowEnd.Text = "Always Show End"
        Me.ttSettings.SetToolTip(Me.chkShowEnd, "Always show Endurance on Overview Big HealthBars")
        Me.chkShowEnd.UseVisualStyleBackColor = True
        '
        'chkAlterOverview
        '
        Me.chkAlterOverview.AutoSize = True
        Me.chkAlterOverview.Location = New System.Drawing.Point(24, 299)
        Me.chkAlterOverview.Name = "chkAlterOverview"
        Me.chkAlterOverview.Size = New System.Drawing.Size(130, 17)
        Me.chkAlterOverview.TabIndex = 23
        Me.chkAlterOverview.TabStop = False
        Me.chkAlterOverview.Text = "Alter Overview Layout"
        Me.ttSettings.SetToolTip(Me.chkAlterOverview, "Note: Whitelist setting from Sorting tab is in effect here")
        Me.chkAlterOverview.UseVisualStyleBackColor = True
        '
        'txtAlterOverviewStarKey
        '
        Me.txtAlterOverviewStarKey.Location = New System.Drawing.Point(181, 64)
        Me.txtAlterOverviewStarKey.Name = "txtAlterOverviewStarKey"
        Me.txtAlterOverviewStarKey.Size = New System.Drawing.Size(73, 20)
        Me.txtAlterOverviewStarKey.TabIndex = 21
        Me.txtAlterOverviewStarKey.TabStop = False
        Me.ttSettings.SetToolTip(Me.txtAlterOverviewStarKey, "Toggle One Less Row/Column")
        '
        'txtAlterOverviewMinKey
        '
        Me.txtAlterOverviewMinKey.Location = New System.Drawing.Point(181, 40)
        Me.txtAlterOverviewMinKey.Name = "txtAlterOverviewMinKey"
        Me.txtAlterOverviewMinKey.Size = New System.Drawing.Size(73, 20)
        Me.txtAlterOverviewMinKey.TabIndex = 11
        Me.txtAlterOverviewMinKey.TabStop = False
        Me.ttSettings.SetToolTip(Me.txtAlterOverviewMinKey, "Decrease Extra Columns/Rows")
        '
        'txtAlterOverviewPlusKey
        '
        Me.txtAlterOverviewPlusKey.Location = New System.Drawing.Point(181, 16)
        Me.txtAlterOverviewPlusKey.Name = "txtAlterOverviewPlusKey"
        Me.txtAlterOverviewPlusKey.Size = New System.Drawing.Size(73, 20)
        Me.txtAlterOverviewPlusKey.TabIndex = 7
        Me.txtAlterOverviewPlusKey.TabStop = False
        Me.ttSettings.SetToolTip(Me.txtAlterOverviewPlusKey, "Increase Extra Columns/Rows")
        '
        'chkOnlyEsc
        '
        Me.chkOnlyEsc.AutoSize = True
        Me.chkOnlyEsc.Location = New System.Drawing.Point(40, 38)
        Me.chkOnlyEsc.Name = "chkOnlyEsc"
        Me.chkOnlyEsc.Size = New System.Drawing.Size(240, 17)
        Me.chkOnlyEsc.TabIndex = 25
        Me.chkOnlyEsc.TabStop = False
        Me.chkOnlyEsc.Text = "Only Send Esc on pressing Alt-Esc or Ctrl-Esc"
        Me.ttSettings.SetToolTip(Me.chkOnlyEsc, "Only Send Esc to Astonia when pressing Alt-Esc or Ctrl-Esc")
        Me.chkOnlyEsc.UseVisualStyleBackColor = True
        '
        'chkAutoCloseSomeone
        '
        Me.chkAutoCloseSomeone.AutoSize = True
        Me.chkAutoCloseSomeone.Location = New System.Drawing.Point(16, 169)
        Me.chkAutoCloseSomeone.Name = "chkAutoCloseSomeone"
        Me.chkAutoCloseSomeone.Size = New System.Drawing.Size(120, 17)
        Me.chkAutoCloseSomeone.TabIndex = 7
        Me.chkAutoCloseSomeone.Text = "AutoClose Idled Alts"
        Me.ttSettings.SetToolTip(Me.chkAutoCloseSomeone, "Alts actually named Someone are exempt" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  and will require a manual close.")
        Me.chkAutoCloseSomeone.UseVisualStyleBackColor = True
        '
        'chkNoAltTab
        '
        Me.chkNoAltTab.AutoSize = True
        Me.chkNoAltTab.Location = New System.Drawing.Point(40, 20)
        Me.chkNoAltTab.Name = "chkNoAltTab"
        Me.chkNoAltTab.Size = New System.Drawing.Size(209, 17)
        Me.chkNoAltTab.TabIndex = 26
        Me.chkNoAltTab.TabStop = False
        Me.chkNoAltTab.Text = "Disable Alt-Tab when Astonia Is Active"
        Me.ttSettings.SetToolTip(Me.chkNoAltTab, "Note you can still use Ctrl-Alt-Tab to bring up the task switcher" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "and then press" &
        " Space to switch applications.")
        Me.chkNoAltTab.UseVisualStyleBackColor = True
        '
        'pb100PWarning
        '
        Me.pb100PWarning.BackgroundImage = Global.ScalA.My.Resources.Resources.Warning
        Me.pb100PWarning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pb100PWarning.Location = New System.Drawing.Point(287, 32)
        Me.pb100PWarning.Name = "pb100PWarning"
        Me.pb100PWarning.Size = New System.Drawing.Size(18, 18)
        Me.pb100PWarning.TabIndex = 10
        Me.pb100PWarning.TabStop = False
        Me.ttSettings.SetToolTip(Me.pb100PWarning, "Windows Monitor Scaling not 100%" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Pixel Mode Disabled." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        '
        'chkAutoCloseOnlyOnNoSome
        '
        Me.chkAutoCloseOnlyOnNoSome.AutoSize = True
        Me.chkAutoCloseOnlyOnNoSome.Location = New System.Drawing.Point(135, 169)
        Me.chkAutoCloseOnlyOnNoSome.Name = "chkAutoCloseOnlyOnNoSome"
        Me.chkAutoCloseOnlyOnNoSome.Size = New System.Drawing.Size(163, 17)
        Me.chkAutoCloseOnlyOnNoSome.TabIndex = 15
        Me.chkAutoCloseOnlyOnNoSome.Text = "When not showing Someone"
        Me.ttSettings.SetToolTip(Me.chkAutoCloseOnlyOnNoSome, resources.GetString("chkAutoCloseOnlyOnNoSome.ToolTip"))
        Me.chkAutoCloseOnlyOnNoSome.UseVisualStyleBackColor = True
        '
        'chkAllowShiftEsc
        '
        Me.chkAllowShiftEsc.AutoSize = True
        Me.chkAllowShiftEsc.Location = New System.Drawing.Point(55, 56)
        Me.chkAllowShiftEsc.Name = "chkAllowShiftEsc"
        Me.chkAllowShiftEsc.Size = New System.Drawing.Size(224, 17)
        Me.chkAllowShiftEsc.TabIndex = 27
        Me.chkAllowShiftEsc.Text = "Allow Ctrl-Shift-Esc to Open TaskManager"
        Me.ttSettings.SetToolTip(Me.chkAllowShiftEsc, "This will send Esc to Astonia in addition to Opening TaskManager")
        Me.chkAllowShiftEsc.UseVisualStyleBackColor = True
        '
        'lblElevated
        '
        Me.lblElevated.AutoSize = True
        Me.lblElevated.Location = New System.Drawing.Point(28, 7)
        Me.lblElevated.Name = "lblElevated"
        Me.lblElevated.Size = New System.Drawing.Size(57, 13)
        Me.lblElevated.TabIndex = 17
        Me.lblElevated.Text = "UnElevate"
        Me.ttSettings.SetToolTip(Me.lblElevated, "ScalA is running as Administrtor." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Click here to drop Admin rights." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Note: ScalA " &
        "will ReElevate when it needs to." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        '
        'pbUnElevate
        '
        Me.pbUnElevate.Location = New System.Drawing.Point(11, 6)
        Me.pbUnElevate.Name = "pbUnElevate"
        Me.pbUnElevate.Size = New System.Drawing.Size(24, 24)
        Me.pbUnElevate.TabIndex = 18
        Me.pbUnElevate.TabStop = False
        Me.ttSettings.SetToolTip(Me.pbUnElevate, "ScalA is running as Administrtor." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Click here to drop Admin rights." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Note: ScalA " &
        "will ReElevate when it needs to." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        '
        'ChkQLShowHidden
        '
        Me.ChkQLShowHidden.AutoSize = True
        Me.ChkQLShowHidden.Location = New System.Drawing.Point(32, 62)
        Me.ChkQLShowHidden.Name = "ChkQLShowHidden"
        Me.ChkQLShowHidden.Size = New System.Drawing.Size(126, 17)
        Me.ChkQLShowHidden.TabIndex = 26
        Me.ChkQLShowHidden.Text = "Always Show Hidden"
        Me.ttSettings.SetToolTip(Me.ChkQLShowHidden, "Always Show Hidden and System Items." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Press Ctrl + Shift to override when this se" &
        "tting is off.")
        Me.ChkQLShowHidden.UseVisualStyleBackColor = True
        '
        'btnRefreshICdisplay
        '
        Me.btnRefreshICdisplay.Location = New System.Drawing.Point(207, 12)
        Me.btnRefreshICdisplay.Name = "btnRefreshICdisplay"
        Me.btnRefreshICdisplay.Size = New System.Drawing.Size(23, 23)
        Me.btnRefreshICdisplay.TabIndex = 33
        Me.btnRefreshICdisplay.Text = "↺"
        Me.ttSettings.SetToolTip(Me.btnRefreshICdisplay, "Recalculate Icon Cache Count & Size")
        Me.btnRefreshICdisplay.UseVisualStyleBackColor = True
        '
        'lblICSize
        '
        Me.lblICSize.AutoSize = True
        Me.lblICSize.Location = New System.Drawing.Point(104, 18)
        Me.lblICSize.Name = "lblICSize"
        Me.lblICSize.Size = New System.Drawing.Size(56, 13)
        Me.lblICSize.TabIndex = 19
        Me.lblICSize.Text = "Size: ? KB"
        Me.ttSettings.SetToolTip(Me.lblICSize, "There will always be a small overhead even with 0 items in the Cache")
        '
        'ChkQLResolveLnk
        '
        Me.ChkQLResolveLnk.AutoSize = True
        Me.ChkQLResolveLnk.Location = New System.Drawing.Point(167, 62)
        Me.ChkQLResolveLnk.Name = "ChkQLResolveLnk"
        Me.ChkQLResolveLnk.Size = New System.Drawing.Size(119, 17)
        Me.ChkQLResolveLnk.TabIndex = 27
        Me.ChkQLResolveLnk.Text = "Resolve Lnk to Dirs"
        Me.ttSettings.SetToolTip(Me.ChkQLResolveLnk, "Have .Lnk Shortcuts pointing to Directories be parsed as Folders" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        Me.ChkQLResolveLnk.UseVisualStyleBackColor = True
        '
        'tbcSettings
        '
        Me.tbcSettings.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tbcSettings.Controls.Add(Me.tabResolutions)
        Me.tbcSettings.Controls.Add(Me.tabHotkeys)
        Me.tbcSettings.Controls.Add(Me.tabSortAndBL)
        Me.tbcSettings.Controls.Add(Me.tabMaximized)
        Me.tbcSettings.Controls.Add(Me.TabQL)
        Me.tbcSettings.Controls.Add(Me.tabMisc)
        Me.tbcSettings.Location = New System.Drawing.Point(-1, 0)
        Me.tbcSettings.Multiline = True
        Me.tbcSettings.Name = "tbcSettings"
        Me.tbcSettings.SelectedIndex = 0
        Me.tbcSettings.Size = New System.Drawing.Size(341, 212)
        Me.tbcSettings.TabIndex = 14
        Me.tbcSettings.TabStop = False
        '
        'tabResolutions
        '
        Me.tabResolutions.Controls.Add(Me.pb100PWarning)
        Me.tabResolutions.Controls.Add(Me.ChkSizeBorder)
        Me.tabResolutions.Controls.Add(Me.cboScalingMode)
        Me.tabResolutions.Controls.Add(Label15)
        Me.tabResolutions.Controls.Add(Me.btnAddCurrentRes)
        Me.tabResolutions.Controls.Add(Me.btnRestore)
        Me.tabResolutions.Controls.Add(Me.btnGenerate)
        Me.tabResolutions.Controls.Add(Me.txtResolutions)
        Me.tabResolutions.Location = New System.Drawing.Point(4, 25)
        Me.tabResolutions.Name = "tabResolutions"
        Me.tabResolutions.Padding = New System.Windows.Forms.Padding(3)
        Me.tabResolutions.Size = New System.Drawing.Size(333, 183)
        Me.tabResolutions.TabIndex = 0
        Me.tabResolutions.Text = "Resolution"
        Me.tabResolutions.UseVisualStyleBackColor = True
        '
        'ChkSizeBorder
        '
        Me.ChkSizeBorder.AutoSize = True
        Me.ChkSizeBorder.Location = New System.Drawing.Point(206, 80)
        Me.ChkSizeBorder.Name = "ChkSizeBorder"
        Me.ChkSizeBorder.Size = New System.Drawing.Size(88, 17)
        Me.ChkSizeBorder.TabIndex = 9
        Me.ChkSizeBorder.Text = "Sizing Border"
        Me.ChkSizeBorder.UseVisualStyleBackColor = True
        '
        'btnAddCurrentRes
        '
        Me.btnAddCurrentRes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAddCurrentRes.Location = New System.Drawing.Point(119, 89)
        Me.btnAddCurrentRes.Name = "btnAddCurrentRes"
        Me.btnAddCurrentRes.Size = New System.Drawing.Size(75, 23)
        Me.btnAddCurrentRes.TabIndex = 6
        Me.btnAddCurrentRes.Text = "Add Current"
        Me.btnAddCurrentRes.UseVisualStyleBackColor = True
        '
        'btnRestore
        '
        Me.btnRestore.ContextMenuStrip = Me.cmsRestore
        Me.btnRestore.Location = New System.Drawing.Point(119, 126)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(75, 23)
        Me.btnRestore.TabIndex = 5
        Me.btnRestore.Text = "Restore"
        Me.btnRestore.UseVisualStyleBackColor = True
        '
        'cmsRestore
        '
        Me.cmsRestore.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LastSavedToolStripMenuItem, Me.DefaultToolStripMenuItem})
        Me.cmsRestore.Name = "cmsRestore"
        Me.cmsRestore.ShowImageMargin = False
        Me.cmsRestore.Size = New System.Drawing.Size(105, 48)
        '
        'LastSavedToolStripMenuItem
        '
        Me.LastSavedToolStripMenuItem.Name = "LastSavedToolStripMenuItem"
        Me.LastSavedToolStripMenuItem.Size = New System.Drawing.Size(104, 22)
        Me.LastSavedToolStripMenuItem.Text = "Last Saved"
        '
        'DefaultToolStripMenuItem
        '
        Me.DefaultToolStripMenuItem.Name = "DefaultToolStripMenuItem"
        Me.DefaultToolStripMenuItem.Size = New System.Drawing.Size(104, 22)
        Me.DefaultToolStripMenuItem.Text = "Default"
        '
        'btnGenerate
        '
        Me.btnGenerate.ContextMenuStrip = Me.cmsGenerate
        Me.btnGenerate.Location = New System.Drawing.Point(119, 31)
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
        Me.cmsGenerate.Size = New System.Drawing.Size(154, 76)
        Me.cmsGenerate.Tag = "800x600 (4:3)"
        '
        'X60043ToolStripMenuItem
        '
        Me.X60043ToolStripMenuItem.Name = "X60043ToolStripMenuItem"
        Me.X60043ToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.X60043ToolStripMenuItem.Tag = "800x600 (4:3)"
        Me.X60043ToolStripMenuItem.Text = "800x600 (4:3)"
        '
        'X720169ToolStripMenuItem
        '
        Me.X720169ToolStripMenuItem.Name = "X720169ToolStripMenuItem"
        Me.X720169ToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.X720169ToolStripMenuItem.Tag = "1280x720 (16:9)"
        Me.X720169ToolStripMenuItem.Text = "1280x720 (16:9)"
        '
        'FromToolStripMenuItem
        '
        Me.FromToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DummyToolStripMenuItem})
        Me.FromToolStripMenuItem.Image = Global.ScalA.My.Resources.Resources.moa31
        Me.FromToolStripMenuItem.Name = "FromToolStripMenuItem"
        Me.FromToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
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
        Me.txtResolutions.Location = New System.Drawing.Point(19, 0)
        Me.txtResolutions.Multiline = True
        Me.txtResolutions.Name = "txtResolutions"
        Me.txtResolutions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtResolutions.Size = New System.Drawing.Size(94, 183)
        Me.txtResolutions.TabIndex = 1
        Me.txtResolutions.Text = "800x600" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1200x900" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1600x1200" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2000x1500" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2400x1800" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2800x2100" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3200x2400" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3600x27" &
    "00" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "4000x3000" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "4400x3300"
        '
        'tabHotkeys
        '
        Me.tabHotkeys.AutoScroll = True
        Me.tabHotkeys.Controls.Add(Me.chkAllowShiftEsc)
        Me.tabHotkeys.Controls.Add(Me.chkNoAltTab)
        Me.tabHotkeys.Controls.Add(Me.chkOnlyEsc)
        Me.tabHotkeys.Controls.Add(Me.chkBlockWin)
        Me.tabHotkeys.Controls.Add(Me.chkAlterOverview)
        Me.tabHotkeys.Controls.Add(Me.grpAlterOverview)
        Me.tabHotkeys.Controls.Add(Me.chkToggleTopMost)
        Me.tabHotkeys.Controls.Add(Me.grpToggleTopMost)
        Me.tabHotkeys.Controls.Add(Me.chkCloseAll)
        Me.tabHotkeys.Controls.Add(Me.grpCloseAllShortcut)
        Me.tabHotkeys.Controls.Add(Me.chkCycleOnClose)
        Me.tabHotkeys.Controls.Add(Me.chkSwitchToOverview)
        Me.tabHotkeys.Controls.Add(Me.chkCycleAlts)
        Me.tabHotkeys.Controls.Add(Me.grpCycleShortcut)
        Me.tabHotkeys.Controls.Add(Me.grpOverviewShortcut)
        Me.tabHotkeys.Location = New System.Drawing.Point(4, 25)
        Me.tabHotkeys.Name = "tabHotkeys"
        Me.tabHotkeys.Size = New System.Drawing.Size(333, 183)
        Me.tabHotkeys.TabIndex = 2
        Me.tabHotkeys.Text = "Hotkey"
        Me.tabHotkeys.UseVisualStyleBackColor = True
        '
        'chkBlockWin
        '
        Me.chkBlockWin.AutoSize = True
        Me.chkBlockWin.Location = New System.Drawing.Point(40, 2)
        Me.chkBlockWin.Name = "chkBlockWin"
        Me.chkBlockWin.Size = New System.Drawing.Size(239, 17)
        Me.chkBlockWin.TabIndex = 24
        Me.chkBlockWin.TabStop = False
        Me.chkBlockWin.Text = "Disable Windows Key when Astonia is Active"
        Me.chkBlockWin.UseVisualStyleBackColor = True
        '
        'grpAlterOverview
        '
        Me.grpAlterOverview.Controls.Add(Me.chkAlterOverviewStarAlt)
        Me.grpAlterOverview.Controls.Add(Me.txtAlterOverviewStarKey)
        Me.grpAlterOverview.Controls.Add(Me.chkAlterOverviewStarShift)
        Me.grpAlterOverview.Controls.Add(Me.chkAlterOverviewStarWin)
        Me.grpAlterOverview.Controls.Add(Me.chkAlterOverviewStarCtrl)
        Me.grpAlterOverview.Controls.Add(Label19)
        Me.grpAlterOverview.Controls.Add(Me.chkAlterOverviewMinAlt)
        Me.grpAlterOverview.Controls.Add(Me.txtAlterOverviewMinKey)
        Me.grpAlterOverview.Controls.Add(Me.chkAlterOverviewPlusALt)
        Me.grpAlterOverview.Controls.Add(Me.txtAlterOverviewPlusKey)
        Me.grpAlterOverview.Controls.Add(Me.chkAlterOverviewMinShift)
        Me.grpAlterOverview.Controls.Add(Me.chkAlterOverviewPlusShift)
        Me.grpAlterOverview.Controls.Add(Me.chkAlterOverviewMinWin)
        Me.grpAlterOverview.Controls.Add(Me.chkAlterOverviewPlusWin)
        Me.grpAlterOverview.Controls.Add(Me.chkAlterOverviewMinCtrl)
        Me.grpAlterOverview.Controls.Add(Me.chkAlterOverviewPlusCtrl)
        Me.grpAlterOverview.Controls.Add(Label18)
        Me.grpAlterOverview.Controls.Add(Label17)
        Me.grpAlterOverview.Location = New System.Drawing.Point(27, 300)
        Me.grpAlterOverview.Name = "grpAlterOverview"
        Me.grpAlterOverview.Size = New System.Drawing.Size(269, 90)
        Me.grpAlterOverview.TabIndex = 22
        Me.grpAlterOverview.TabStop = False
        '
        'chkAlterOverviewStarAlt
        '
        Me.chkAlterOverviewStarAlt.AutoSize = True
        Me.chkAlterOverviewStarAlt.Location = New System.Drawing.Point(90, 66)
        Me.chkAlterOverviewStarAlt.Name = "chkAlterOverviewStarAlt"
        Me.chkAlterOverviewStarAlt.Size = New System.Drawing.Size(38, 17)
        Me.chkAlterOverviewStarAlt.TabIndex = 23
        Me.chkAlterOverviewStarAlt.TabStop = False
        Me.chkAlterOverviewStarAlt.Text = "Alt"
        Me.chkAlterOverviewStarAlt.UseVisualStyleBackColor = True
        '
        'chkAlterOverviewStarShift
        '
        Me.chkAlterOverviewStarShift.AutoSize = True
        Me.chkAlterOverviewStarShift.Location = New System.Drawing.Point(131, 66)
        Me.chkAlterOverviewStarShift.Name = "chkAlterOverviewStarShift"
        Me.chkAlterOverviewStarShift.Size = New System.Drawing.Size(47, 17)
        Me.chkAlterOverviewStarShift.TabIndex = 24
        Me.chkAlterOverviewStarShift.TabStop = False
        Me.chkAlterOverviewStarShift.Text = "Shift"
        Me.chkAlterOverviewStarShift.UseVisualStyleBackColor = True
        '
        'chkAlterOverviewStarWin
        '
        Me.chkAlterOverviewStarWin.AutoSize = True
        Me.chkAlterOverviewStarWin.Location = New System.Drawing.Point(46, 66)
        Me.chkAlterOverviewStarWin.Name = "chkAlterOverviewStarWin"
        Me.chkAlterOverviewStarWin.Size = New System.Drawing.Size(45, 17)
        Me.chkAlterOverviewStarWin.TabIndex = 25
        Me.chkAlterOverviewStarWin.TabStop = False
        Me.chkAlterOverviewStarWin.Text = "Win"
        Me.chkAlterOverviewStarWin.UseVisualStyleBackColor = True
        '
        'chkAlterOverviewStarCtrl
        '
        Me.chkAlterOverviewStarCtrl.AutoSize = True
        Me.chkAlterOverviewStarCtrl.Location = New System.Drawing.Point(6, 66)
        Me.chkAlterOverviewStarCtrl.Name = "chkAlterOverviewStarCtrl"
        Me.chkAlterOverviewStarCtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkAlterOverviewStarCtrl.TabIndex = 22
        Me.chkAlterOverviewStarCtrl.TabStop = False
        Me.chkAlterOverviewStarCtrl.Text = "Ctrl"
        Me.chkAlterOverviewStarCtrl.UseVisualStyleBackColor = True
        '
        'chkAlterOverviewMinAlt
        '
        Me.chkAlterOverviewMinAlt.AutoSize = True
        Me.chkAlterOverviewMinAlt.Location = New System.Drawing.Point(90, 42)
        Me.chkAlterOverviewMinAlt.Name = "chkAlterOverviewMinAlt"
        Me.chkAlterOverviewMinAlt.Size = New System.Drawing.Size(38, 17)
        Me.chkAlterOverviewMinAlt.TabIndex = 13
        Me.chkAlterOverviewMinAlt.TabStop = False
        Me.chkAlterOverviewMinAlt.Text = "Alt"
        Me.chkAlterOverviewMinAlt.UseVisualStyleBackColor = True
        '
        'chkAlterOverviewPlusALt
        '
        Me.chkAlterOverviewPlusALt.AutoSize = True
        Me.chkAlterOverviewPlusALt.Location = New System.Drawing.Point(90, 19)
        Me.chkAlterOverviewPlusALt.Name = "chkAlterOverviewPlusALt"
        Me.chkAlterOverviewPlusALt.Size = New System.Drawing.Size(38, 17)
        Me.chkAlterOverviewPlusALt.TabIndex = 9
        Me.chkAlterOverviewPlusALt.TabStop = False
        Me.chkAlterOverviewPlusALt.Text = "Alt"
        Me.chkAlterOverviewPlusALt.UseVisualStyleBackColor = True
        '
        'chkAlterOverviewMinShift
        '
        Me.chkAlterOverviewMinShift.AutoSize = True
        Me.chkAlterOverviewMinShift.Location = New System.Drawing.Point(131, 42)
        Me.chkAlterOverviewMinShift.Name = "chkAlterOverviewMinShift"
        Me.chkAlterOverviewMinShift.Size = New System.Drawing.Size(47, 17)
        Me.chkAlterOverviewMinShift.TabIndex = 15
        Me.chkAlterOverviewMinShift.TabStop = False
        Me.chkAlterOverviewMinShift.Text = "Shift"
        Me.chkAlterOverviewMinShift.UseVisualStyleBackColor = True
        '
        'chkAlterOverviewPlusShift
        '
        Me.chkAlterOverviewPlusShift.AutoSize = True
        Me.chkAlterOverviewPlusShift.Location = New System.Drawing.Point(131, 19)
        Me.chkAlterOverviewPlusShift.Name = "chkAlterOverviewPlusShift"
        Me.chkAlterOverviewPlusShift.Size = New System.Drawing.Size(47, 17)
        Me.chkAlterOverviewPlusShift.TabIndex = 14
        Me.chkAlterOverviewPlusShift.TabStop = False
        Me.chkAlterOverviewPlusShift.Text = "Shift"
        Me.chkAlterOverviewPlusShift.UseVisualStyleBackColor = True
        '
        'chkAlterOverviewMinWin
        '
        Me.chkAlterOverviewMinWin.AutoSize = True
        Me.chkAlterOverviewMinWin.Location = New System.Drawing.Point(46, 42)
        Me.chkAlterOverviewMinWin.Name = "chkAlterOverviewMinWin"
        Me.chkAlterOverviewMinWin.Size = New System.Drawing.Size(45, 17)
        Me.chkAlterOverviewMinWin.TabIndex = 17
        Me.chkAlterOverviewMinWin.TabStop = False
        Me.chkAlterOverviewMinWin.Text = "Win"
        Me.chkAlterOverviewMinWin.UseVisualStyleBackColor = True
        '
        'chkAlterOverviewPlusWin
        '
        Me.chkAlterOverviewPlusWin.AutoSize = True
        Me.chkAlterOverviewPlusWin.Location = New System.Drawing.Point(46, 19)
        Me.chkAlterOverviewPlusWin.Name = "chkAlterOverviewPlusWin"
        Me.chkAlterOverviewPlusWin.Size = New System.Drawing.Size(45, 17)
        Me.chkAlterOverviewPlusWin.TabIndex = 16
        Me.chkAlterOverviewPlusWin.TabStop = False
        Me.chkAlterOverviewPlusWin.Text = "Win"
        Me.chkAlterOverviewPlusWin.UseVisualStyleBackColor = True
        '
        'chkAlterOverviewMinCtrl
        '
        Me.chkAlterOverviewMinCtrl.AutoSize = True
        Me.chkAlterOverviewMinCtrl.Location = New System.Drawing.Point(6, 42)
        Me.chkAlterOverviewMinCtrl.Name = "chkAlterOverviewMinCtrl"
        Me.chkAlterOverviewMinCtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkAlterOverviewMinCtrl.TabIndex = 12
        Me.chkAlterOverviewMinCtrl.TabStop = False
        Me.chkAlterOverviewMinCtrl.Text = "Ctrl"
        Me.chkAlterOverviewMinCtrl.UseVisualStyleBackColor = True
        '
        'chkAlterOverviewPlusCtrl
        '
        Me.chkAlterOverviewPlusCtrl.AutoSize = True
        Me.chkAlterOverviewPlusCtrl.Location = New System.Drawing.Point(6, 19)
        Me.chkAlterOverviewPlusCtrl.Name = "chkAlterOverviewPlusCtrl"
        Me.chkAlterOverviewPlusCtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkAlterOverviewPlusCtrl.TabIndex = 8
        Me.chkAlterOverviewPlusCtrl.TabStop = False
        Me.chkAlterOverviewPlusCtrl.Text = "Ctrl"
        Me.chkAlterOverviewPlusCtrl.UseVisualStyleBackColor = True
        '
        'chkToggleTopMost
        '
        Me.chkToggleTopMost.AutoSize = True
        Me.chkToggleTopMost.Location = New System.Drawing.Point(40, 250)
        Me.chkToggleTopMost.Name = "chkToggleTopMost"
        Me.chkToggleTopMost.Size = New System.Drawing.Size(215, 18)
        Me.chkToggleTopMost.TabIndex = 21
        Me.chkToggleTopMost.TabStop = False
        Me.chkToggleTopMost.Text = "Toggle Other Window TopMost Status"
        Me.chkToggleTopMost.UseCompatibleTextRendering = True
        Me.chkToggleTopMost.UseVisualStyleBackColor = True
        '
        'grpToggleTopMost
        '
        Me.grpToggleTopMost.Controls.Add(Me.txtTogTop)
        Me.grpToggleTopMost.Controls.Add(Me.chkTogTopAlt)
        Me.grpToggleTopMost.Controls.Add(Me.chkTogTopShift)
        Me.grpToggleTopMost.Controls.Add(Me.chkTogTopWin)
        Me.grpToggleTopMost.Controls.Add(Me.chkTogTopCtrl)
        Me.grpToggleTopMost.Location = New System.Drawing.Point(27, 252)
        Me.grpToggleTopMost.Name = "grpToggleTopMost"
        Me.grpToggleTopMost.Size = New System.Drawing.Size(269, 43)
        Me.grpToggleTopMost.TabIndex = 20
        Me.grpToggleTopMost.TabStop = False
        '
        'txtTogTop
        '
        Me.txtTogTop.Location = New System.Drawing.Point(181, 16)
        Me.txtTogTop.Name = "txtTogTop"
        Me.txtTogTop.Size = New System.Drawing.Size(82, 20)
        Me.txtTogTop.TabIndex = 21
        Me.txtTogTop.TabStop = False
        '
        'chkTogTopAlt
        '
        Me.chkTogTopAlt.AutoSize = True
        Me.chkTogTopAlt.Location = New System.Drawing.Point(90, 19)
        Me.chkTogTopAlt.Name = "chkTogTopAlt"
        Me.chkTogTopAlt.Size = New System.Drawing.Size(38, 17)
        Me.chkTogTopAlt.TabIndex = 23
        Me.chkTogTopAlt.TabStop = False
        Me.chkTogTopAlt.Text = "Alt"
        Me.chkTogTopAlt.UseVisualStyleBackColor = True
        '
        'chkTogTopShift
        '
        Me.chkTogTopShift.AutoSize = True
        Me.chkTogTopShift.Location = New System.Drawing.Point(131, 19)
        Me.chkTogTopShift.Name = "chkTogTopShift"
        Me.chkTogTopShift.Size = New System.Drawing.Size(47, 17)
        Me.chkTogTopShift.TabIndex = 22
        Me.chkTogTopShift.TabStop = False
        Me.chkTogTopShift.Text = "Shift"
        Me.chkTogTopShift.UseVisualStyleBackColor = True
        '
        'chkTogTopWin
        '
        Me.chkTogTopWin.AutoSize = True
        Me.chkTogTopWin.Location = New System.Drawing.Point(46, 19)
        Me.chkTogTopWin.Name = "chkTogTopWin"
        Me.chkTogTopWin.Size = New System.Drawing.Size(45, 17)
        Me.chkTogTopWin.TabIndex = 20
        Me.chkTogTopWin.TabStop = False
        Me.chkTogTopWin.Text = "Win"
        Me.chkTogTopWin.UseVisualStyleBackColor = True
        '
        'chkTogTopCtrl
        '
        Me.chkTogTopCtrl.AutoSize = True
        Me.chkTogTopCtrl.Location = New System.Drawing.Point(6, 19)
        Me.chkTogTopCtrl.Name = "chkTogTopCtrl"
        Me.chkTogTopCtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkTogTopCtrl.TabIndex = 19
        Me.chkTogTopCtrl.TabStop = False
        Me.chkTogTopCtrl.Text = "Ctrl"
        Me.chkTogTopCtrl.UseVisualStyleBackColor = True
        '
        'grpCloseAllShortcut
        '
        Me.grpCloseAllShortcut.Controls.Add(Me.txtCloseAll)
        Me.grpCloseAllShortcut.Controls.Add(Me.chkCAALt)
        Me.grpCloseAllShortcut.Controls.Add(Me.chkCAShift)
        Me.grpCloseAllShortcut.Controls.Add(Me.chkCAWin)
        Me.grpCloseAllShortcut.Controls.Add(Me.chkCACtrl)
        Me.grpCloseAllShortcut.Location = New System.Drawing.Point(27, 203)
        Me.grpCloseAllShortcut.Name = "grpCloseAllShortcut"
        Me.grpCloseAllShortcut.Size = New System.Drawing.Size(269, 43)
        Me.grpCloseAllShortcut.TabIndex = 17
        Me.grpCloseAllShortcut.TabStop = False
        '
        'txtCloseAll
        '
        Me.txtCloseAll.Location = New System.Drawing.Point(181, 16)
        Me.txtCloseAll.Name = "txtCloseAll"
        Me.txtCloseAll.Size = New System.Drawing.Size(82, 20)
        Me.txtCloseAll.TabIndex = 21
        Me.txtCloseAll.TabStop = False
        '
        'chkCAALt
        '
        Me.chkCAALt.AutoSize = True
        Me.chkCAALt.Location = New System.Drawing.Point(90, 19)
        Me.chkCAALt.Name = "chkCAALt"
        Me.chkCAALt.Size = New System.Drawing.Size(38, 17)
        Me.chkCAALt.TabIndex = 20
        Me.chkCAALt.TabStop = False
        Me.chkCAALt.Text = "Alt"
        Me.chkCAALt.UseVisualStyleBackColor = True
        '
        'chkCAShift
        '
        Me.chkCAShift.AutoSize = True
        Me.chkCAShift.Location = New System.Drawing.Point(131, 19)
        Me.chkCAShift.Name = "chkCAShift"
        Me.chkCAShift.Size = New System.Drawing.Size(47, 17)
        Me.chkCAShift.TabIndex = 22
        Me.chkCAShift.TabStop = False
        Me.chkCAShift.Text = "Shift"
        Me.chkCAShift.UseVisualStyleBackColor = True
        '
        'chkCAWin
        '
        Me.chkCAWin.AutoSize = True
        Me.chkCAWin.Location = New System.Drawing.Point(46, 19)
        Me.chkCAWin.Name = "chkCAWin"
        Me.chkCAWin.Size = New System.Drawing.Size(45, 17)
        Me.chkCAWin.TabIndex = 23
        Me.chkCAWin.TabStop = False
        Me.chkCAWin.Text = "Win"
        Me.chkCAWin.UseVisualStyleBackColor = True
        '
        'chkCACtrl
        '
        Me.chkCACtrl.AutoSize = True
        Me.chkCACtrl.Location = New System.Drawing.Point(6, 19)
        Me.chkCACtrl.Name = "chkCACtrl"
        Me.chkCACtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkCACtrl.TabIndex = 19
        Me.chkCACtrl.TabStop = False
        Me.chkCACtrl.Text = "Ctrl"
        Me.chkCACtrl.UseVisualStyleBackColor = True
        '
        'chkSwitchToOverview
        '
        Me.chkSwitchToOverview.AutoSize = True
        Me.chkSwitchToOverview.Location = New System.Drawing.Point(40, 80)
        Me.chkSwitchToOverview.Name = "chkSwitchToOverview"
        Me.chkSwitchToOverview.Size = New System.Drawing.Size(118, 17)
        Me.chkSwitchToOverview.TabIndex = 2
        Me.chkSwitchToOverview.TabStop = False
        Me.chkSwitchToOverview.Text = "Switch to Overview"
        Me.chkSwitchToOverview.UseVisualStyleBackColor = True
        '
        'chkCycleAlts
        '
        Me.chkCycleAlts.AutoSize = True
        Me.chkCycleAlts.Location = New System.Drawing.Point(40, 129)
        Me.chkCycleAlts.Name = "chkCycleAlts"
        Me.chkCycleAlts.Size = New System.Drawing.Size(102, 17)
        Me.chkCycleAlts.TabIndex = 10
        Me.chkCycleAlts.TabStop = False
        Me.chkCycleAlts.Text = "Cycle Up/Down"
        Me.chkCycleAlts.UseVisualStyleBackColor = True
        '
        'grpCycleShortcut
        '
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleDownAlt)
        Me.grpCycleShortcut.Controls.Add(Me.txtCycleKeyDown)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleUpAlt)
        Me.grpCycleShortcut.Controls.Add(Me.txtCycleKeyUp)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleDownShift)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleUpShift)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleDownWin)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleUpWin)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleDownCtrl)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleUpCtrl)
        Me.grpCycleShortcut.Location = New System.Drawing.Point(27, 131)
        Me.grpCycleShortcut.Name = "grpCycleShortcut"
        Me.grpCycleShortcut.Size = New System.Drawing.Size(269, 67)
        Me.grpCycleShortcut.TabIndex = 1
        Me.grpCycleShortcut.TabStop = False
        '
        'chkCycleDownAlt
        '
        Me.chkCycleDownAlt.AutoSize = True
        Me.chkCycleDownAlt.Location = New System.Drawing.Point(90, 42)
        Me.chkCycleDownAlt.Name = "chkCycleDownAlt"
        Me.chkCycleDownAlt.Size = New System.Drawing.Size(38, 17)
        Me.chkCycleDownAlt.TabIndex = 13
        Me.chkCycleDownAlt.TabStop = False
        Me.chkCycleDownAlt.Text = "Alt"
        Me.chkCycleDownAlt.UseVisualStyleBackColor = True
        '
        'txtCycleKeyDown
        '
        Me.txtCycleKeyDown.Location = New System.Drawing.Point(181, 40)
        Me.txtCycleKeyDown.Name = "txtCycleKeyDown"
        Me.txtCycleKeyDown.Size = New System.Drawing.Size(82, 20)
        Me.txtCycleKeyDown.TabIndex = 11
        Me.txtCycleKeyDown.TabStop = False
        '
        'chkCycleUpAlt
        '
        Me.chkCycleUpAlt.AutoSize = True
        Me.chkCycleUpAlt.Location = New System.Drawing.Point(90, 19)
        Me.chkCycleUpAlt.Name = "chkCycleUpAlt"
        Me.chkCycleUpAlt.Size = New System.Drawing.Size(38, 17)
        Me.chkCycleUpAlt.TabIndex = 9
        Me.chkCycleUpAlt.TabStop = False
        Me.chkCycleUpAlt.Text = "Alt"
        Me.chkCycleUpAlt.UseVisualStyleBackColor = True
        '
        'txtCycleKeyUp
        '
        Me.txtCycleKeyUp.Location = New System.Drawing.Point(181, 16)
        Me.txtCycleKeyUp.Name = "txtCycleKeyUp"
        Me.txtCycleKeyUp.Size = New System.Drawing.Size(82, 20)
        Me.txtCycleKeyUp.TabIndex = 7
        Me.txtCycleKeyUp.TabStop = False
        '
        'chkCycleDownShift
        '
        Me.chkCycleDownShift.AutoSize = True
        Me.chkCycleDownShift.Location = New System.Drawing.Point(131, 42)
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
        Me.chkCycleUpShift.Location = New System.Drawing.Point(131, 19)
        Me.chkCycleUpShift.Name = "chkCycleUpShift"
        Me.chkCycleUpShift.Size = New System.Drawing.Size(47, 17)
        Me.chkCycleUpShift.TabIndex = 14
        Me.chkCycleUpShift.TabStop = False
        Me.chkCycleUpShift.Text = "Shift"
        Me.chkCycleUpShift.UseVisualStyleBackColor = True
        '
        'chkCycleDownWin
        '
        Me.chkCycleDownWin.AutoSize = True
        Me.chkCycleDownWin.Location = New System.Drawing.Point(46, 42)
        Me.chkCycleDownWin.Name = "chkCycleDownWin"
        Me.chkCycleDownWin.Size = New System.Drawing.Size(45, 17)
        Me.chkCycleDownWin.TabIndex = 17
        Me.chkCycleDownWin.TabStop = False
        Me.chkCycleDownWin.Text = "Win"
        Me.chkCycleDownWin.UseVisualStyleBackColor = True
        '
        'chkCycleUpWin
        '
        Me.chkCycleUpWin.AutoSize = True
        Me.chkCycleUpWin.Location = New System.Drawing.Point(46, 19)
        Me.chkCycleUpWin.Name = "chkCycleUpWin"
        Me.chkCycleUpWin.Size = New System.Drawing.Size(45, 17)
        Me.chkCycleUpWin.TabIndex = 16
        Me.chkCycleUpWin.TabStop = False
        Me.chkCycleUpWin.Text = "Win"
        Me.chkCycleUpWin.UseVisualStyleBackColor = True
        '
        'chkCycleDownCtrl
        '
        Me.chkCycleDownCtrl.AutoSize = True
        Me.chkCycleDownCtrl.Location = New System.Drawing.Point(6, 42)
        Me.chkCycleDownCtrl.Name = "chkCycleDownCtrl"
        Me.chkCycleDownCtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkCycleDownCtrl.TabIndex = 12
        Me.chkCycleDownCtrl.TabStop = False
        Me.chkCycleDownCtrl.Text = "Ctrl"
        Me.chkCycleDownCtrl.UseVisualStyleBackColor = True
        '
        'chkCycleUpCtrl
        '
        Me.chkCycleUpCtrl.AutoSize = True
        Me.chkCycleUpCtrl.Location = New System.Drawing.Point(6, 19)
        Me.chkCycleUpCtrl.Name = "chkCycleUpCtrl"
        Me.chkCycleUpCtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkCycleUpCtrl.TabIndex = 8
        Me.chkCycleUpCtrl.TabStop = False
        Me.chkCycleUpCtrl.Text = "Ctrl"
        Me.chkCycleUpCtrl.UseVisualStyleBackColor = True
        '
        'grpOverviewShortcut
        '
        Me.grpOverviewShortcut.Controls.Add(Me.txtStoKey)
        Me.grpOverviewShortcut.Controls.Add(Me.chkStoAlt)
        Me.grpOverviewShortcut.Controls.Add(Me.chkStoShift)
        Me.grpOverviewShortcut.Controls.Add(Me.chkStoWin)
        Me.grpOverviewShortcut.Controls.Add(Me.chkStoCtrl)
        Me.grpOverviewShortcut.Location = New System.Drawing.Point(27, 82)
        Me.grpOverviewShortcut.Name = "grpOverviewShortcut"
        Me.grpOverviewShortcut.Size = New System.Drawing.Size(269, 43)
        Me.grpOverviewShortcut.TabIndex = 0
        Me.grpOverviewShortcut.TabStop = False
        '
        'txtStoKey
        '
        Me.txtStoKey.Location = New System.Drawing.Point(181, 16)
        Me.txtStoKey.Name = "txtStoKey"
        Me.txtStoKey.Size = New System.Drawing.Size(82, 20)
        Me.txtStoKey.TabIndex = 6
        Me.txtStoKey.TabStop = False
        '
        'chkStoAlt
        '
        Me.chkStoAlt.AutoSize = True
        Me.chkStoAlt.Location = New System.Drawing.Point(90, 19)
        Me.chkStoAlt.Name = "chkStoAlt"
        Me.chkStoAlt.Size = New System.Drawing.Size(38, 17)
        Me.chkStoAlt.TabIndex = 5
        Me.chkStoAlt.TabStop = False
        Me.chkStoAlt.Text = "Alt"
        Me.chkStoAlt.UseVisualStyleBackColor = True
        '
        'chkStoShift
        '
        Me.chkStoShift.AutoSize = True
        Me.chkStoShift.Location = New System.Drawing.Point(131, 19)
        Me.chkStoShift.Name = "chkStoShift"
        Me.chkStoShift.Size = New System.Drawing.Size(47, 17)
        Me.chkStoShift.TabIndex = 7
        Me.chkStoShift.TabStop = False
        Me.chkStoShift.Text = "Shift"
        Me.chkStoShift.UseVisualStyleBackColor = True
        '
        'chkStoWin
        '
        Me.chkStoWin.AutoSize = True
        Me.chkStoWin.Location = New System.Drawing.Point(46, 19)
        Me.chkStoWin.Name = "chkStoWin"
        Me.chkStoWin.Size = New System.Drawing.Size(45, 17)
        Me.chkStoWin.TabIndex = 8
        Me.chkStoWin.TabStop = False
        Me.chkStoWin.Text = "Win"
        Me.chkStoWin.UseVisualStyleBackColor = True
        '
        'chkStoCtrl
        '
        Me.chkStoCtrl.AutoSize = True
        Me.chkStoCtrl.Location = New System.Drawing.Point(6, 19)
        Me.chkStoCtrl.Name = "chkStoCtrl"
        Me.chkStoCtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkStoCtrl.TabIndex = 3
        Me.chkStoCtrl.TabStop = False
        Me.chkStoCtrl.Text = "Ctrl"
        Me.chkStoCtrl.UseVisualStyleBackColor = True
        '
        'tabSortAndBL
        '
        Me.tabSortAndBL.Controls.Add(Me.chkAutoCloseOnlyOnNoSome)
        Me.tabSortAndBL.Controls.Add(Me.chkAutoCloseSomeone)
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
        Me.tabSortAndBL.Size = New System.Drawing.Size(333, 183)
        Me.tabSortAndBL.TabIndex = 3
        Me.tabSortAndBL.Text = "Sort"
        Me.tabSortAndBL.UseVisualStyleBackColor = True
        '
        'btnTest
        '
        Me.btnTest.Location = New System.Drawing.Point(155, 47)
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
        Me.btnHelp.Location = New System.Drawing.Point(155, 18)
        Me.btnHelp.Margin = New System.Windows.Forms.Padding(0)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(22, 23)
        Me.btnHelp.TabIndex = 2
        Me.btnHelp.UseVisualStyleBackColor = True
        '
        'txtBotSort
        '
        Me.txtBotSort.Location = New System.Drawing.Point(181, 15)
        Me.txtBotSort.Multiline = True
        Me.txtBotSort.Name = "txtBotSort"
        Me.txtBotSort.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtBotSort.Size = New System.Drawing.Size(140, 150)
        Me.txtBotSort.TabIndex = 1
        Me.txtBotSort.Text = "Someone"
        '
        'txtTopSort
        '
        Me.txtTopSort.Location = New System.Drawing.Point(10, 15)
        Me.txtTopSort.Multiline = True
        Me.txtTopSort.Name = "txtTopSort"
        Me.txtTopSort.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtTopSort.Size = New System.Drawing.Size(140, 150)
        Me.txtTopSort.TabIndex = 0
        Me.txtTopSort.Text = "Someone"
        '
        'chkWhitelist
        '
        Me.chkWhitelist.AutoSize = True
        Me.chkWhitelist.Location = New System.Drawing.Point(128, -1)
        Me.chkWhitelist.Name = "chkWhitelist"
        Me.chkWhitelist.Size = New System.Drawing.Size(66, 17)
        Me.chkWhitelist.TabIndex = 6
        Me.chkWhitelist.Text = "Whitelist"
        Me.chkWhitelist.UseVisualStyleBackColor = True
        '
        'tabMaximized
        '
        Me.tabMaximized.Controls.Add(Me.chkMinMaxOnSwitch)
        Me.tabMaximized.Controls.Add(grpAlterOverviewMinMax)
        Me.tabMaximized.Controls.Add(Me.ChkMinMin)
        Me.tabMaximized.Controls.Add(Me.chkStartupMax)
        Me.tabMaximized.Controls.Add(Me.grpReserveSpace)
        Me.tabMaximized.Location = New System.Drawing.Point(4, 25)
        Me.tabMaximized.Name = "tabMaximized"
        Me.tabMaximized.Size = New System.Drawing.Size(333, 183)
        Me.tabMaximized.TabIndex = 4
        Me.tabMaximized.Text = "Min/Max"
        Me.tabMaximized.UseVisualStyleBackColor = True
        '
        'chkMinMaxOnSwitch
        '
        Me.chkMinMaxOnSwitch.AutoSize = True
        Me.chkMinMaxOnSwitch.Location = New System.Drawing.Point(39, 158)
        Me.chkMinMaxOnSwitch.Name = "chkMinMaxOnSwitch"
        Me.chkMinMaxOnSwitch.Size = New System.Drawing.Size(257, 17)
        Me.chkMinMaxOnSwitch.TabIndex = 32
        Me.chkMinMaxOnSwitch.Text = "Maxim-/Normalize on switching to/from Overview"
        Me.chkMinMaxOnSwitch.UseVisualStyleBackColor = True
        '
        'chkStartupMax
        '
        Me.chkStartupMax.AutoSize = True
        Me.chkStartupMax.Location = New System.Drawing.Point(191, 104)
        Me.chkStartupMax.Name = "chkStartupMax"
        Me.chkStartupMax.Size = New System.Drawing.Size(100, 17)
        Me.chkStartupMax.TabIndex = 29
        Me.chkStartupMax.Text = "Start Maximized"
        Me.chkStartupMax.UseVisualStyleBackColor = True
        '
        'TabQL
        '
        Me.TabQL.Controls.Add(Me.GroupBox1)
        Me.TabQL.Controls.Add(gbFilter)
        Me.TabQL.Controls.Add(Me.ChkQLResolveLnk)
        Me.TabQL.Controls.Add(Me.ChkQLShowHidden)
        Me.TabQL.Controls.Add(grpQLPath)
        Me.TabQL.Location = New System.Drawing.Point(4, 25)
        Me.TabQL.Name = "TabQL"
        Me.TabQL.Size = New System.Drawing.Size(333, 183)
        Me.TabQL.TabIndex = 5
        Me.TabQL.Text = "QL"
        Me.TabQL.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnRefreshICdisplay)
        Me.GroupBox1.Controls.Add(Me.lblICSize)
        Me.GroupBox1.Controls.Add(Me.lblICacheCount)
        Me.GroupBox1.Controls.Add(Me.btnResetCache)
        Me.GroupBox1.Location = New System.Drawing.Point(9, 130)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(312, 40)
        Me.GroupBox1.TabIndex = 32
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Icon Cache"
        '
        'lblICacheCount
        '
        Me.lblICacheCount.AutoSize = True
        Me.lblICacheCount.Location = New System.Drawing.Point(6, 18)
        Me.lblICacheCount.Name = "lblICacheCount"
        Me.lblICacheCount.Size = New System.Drawing.Size(47, 13)
        Me.lblICacheCount.TabIndex = 1
        Me.lblICacheCount.Text = "Count: 1"
        '
        'btnResetCache
        '
        Me.btnResetCache.Location = New System.Drawing.Point(231, 12)
        Me.btnResetCache.Name = "btnResetCache"
        Me.btnResetCache.Size = New System.Drawing.Size(75, 23)
        Me.btnResetCache.TabIndex = 0
        Me.btnResetCache.Text = "Reset"
        Me.btnResetCache.UseVisualStyleBackColor = True
        '
        'tabMisc
        '
        Me.tabMisc.Controls.Add(Me.cmbPriority)
        Me.tabMisc.Controls.Add(Label1)
        Me.tabMisc.Controls.Add(Me.chkShowEnd)
        Me.tabMisc.Controls.Add(Me.chkHoverActivate)
        Me.tabMisc.Controls.Add(Me.cmbTheme)
        Me.tabMisc.Controls.Add(Me.Label16)
        Me.tabMisc.Controls.Add(Me.txtExe)
        Me.tabMisc.Controls.Add(Label3)
        Me.tabMisc.Controls.Add(Me.txtClass)
        Me.tabMisc.Controls.Add(Me.chkOverViewIsGame)
        Me.tabMisc.Controls.Add(Me.ChkSingleInstance)
        Me.tabMisc.Controls.Add(Me.chkCheckForUpdate)
        Me.tabMisc.Controls.Add(Me.chkTopMost)
        Me.tabMisc.Controls.Add(Me.chkRoundCorners)
        Me.tabMisc.Location = New System.Drawing.Point(4, 25)
        Me.tabMisc.Name = "tabMisc"
        Me.tabMisc.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMisc.Size = New System.Drawing.Size(333, 183)
        Me.tabMisc.TabIndex = 1
        Me.tabMisc.Text = "Misc"
        Me.tabMisc.UseVisualStyleBackColor = True
        '
        'cmbPriority
        '
        Me.cmbPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPriority.FormattingEnabled = True
        Me.cmbPriority.Items.AddRange(New Object() {"High : 13", "Above Normal : 10", "Normal : 8", "Below Norlmal : 6", "Idle : 4 "})
        Me.cmbPriority.Location = New System.Drawing.Point(189, 82)
        Me.cmbPriority.Name = "cmbPriority"
        Me.cmbPriority.Size = New System.Drawing.Size(113, 21)
        Me.cmbPriority.TabIndex = 29
        '
        'cmbTheme
        '
        Me.cmbTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTheme.FormattingEnabled = True
        Me.cmbTheme.Items.AddRange(New Object() {"System", "Light", "Dark"})
        Me.cmbTheme.Location = New System.Drawing.Point(220, 59)
        Me.cmbTheme.Name = "cmbTheme"
        Me.cmbTheme.Size = New System.Drawing.Size(82, 21)
        Me.cmbTheme.TabIndex = 25
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(176, 63)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(40, 13)
        Me.Label16.TabIndex = 24
        Me.Label16.Text = "Theme"
        '
        'txtExe
        '
        Me.txtExe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExe.Location = New System.Drawing.Point(12, 137)
        Me.txtExe.Name = "txtExe"
        Me.txtExe.Size = New System.Drawing.Size(286, 20)
        Me.txtExe.TabIndex = 8
        Me.txtExe.Text = "moac | new"
        '
        'txtClass
        '
        Me.txtClass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClass.Location = New System.Drawing.Point(12, 163)
        Me.txtClass.Name = "txtClass"
        Me.txtClass.Size = New System.Drawing.Size(312, 20)
        Me.txtClass.TabIndex = 9
        Me.txtClass.Text = "MAINWNDMOAC | 䅍义乗䵄䅏C"
        '
        'chkTopMost
        '
        Me.chkTopMost.AutoSize = True
        Me.chkTopMost.Location = New System.Drawing.Point(26, 60)
        Me.chkTopMost.Name = "chkTopMost"
        Me.chkTopMost.Size = New System.Drawing.Size(98, 17)
        Me.chkTopMost.TabIndex = 3
        Me.chkTopMost.Text = "Always On Top"
        Me.chkTopMost.UseVisualStyleBackColor = True
        '
        'chkRoundCorners
        '
        Me.chkRoundCorners.AutoSize = True
        Me.chkRoundCorners.Location = New System.Drawing.Point(26, 35)
        Me.chkRoundCorners.Name = "chkRoundCorners"
        Me.chkRoundCorners.Size = New System.Drawing.Size(109, 17)
        Me.chkRoundCorners.TabIndex = 2
        Me.chkRoundCorners.Text = "Rounded Corners"
        Me.chkRoundCorners.UseVisualStyleBackColor = True
        '
        'pnlElevation
        '
        Me.pnlElevation.Controls.Add(Me.lblElevated)
        Me.pnlElevation.Controls.Add(Me.pbUnElevate)
        Me.pnlElevation.Location = New System.Drawing.Point(7, 212)
        Me.pnlElevation.Name = "pnlElevation"
        Me.pnlElevation.Size = New System.Drawing.Size(92, 28)
        Me.pnlElevation.TabIndex = 17
        Me.pnlElevation.Visible = False
        '
        'FrmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ClientSize = New System.Drawing.Size(339, 244)
        Me.Controls.Add(Me.pnlElevation)
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
        grpAlterOverviewMinMax.ResumeLayout(False)
        grpAlterOverviewMinMax.PerformLayout()
        CType(Me.NumExtraMax, System.ComponentModel.ISupportInitialize).EndInit()
        grpQLPath.ResumeLayout(False)
        grpQLPath.PerformLayout()
        Me.cmsQLFolder.ResumeLayout(False)
        gbFilter.ResumeLayout(False)
        gbFilter.PerformLayout()
        Me.grpReserveSpace.ResumeLayout(False)
        Me.grpReserveSpace.PerformLayout()
        CType(Me.NumBorderBot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumBorderRight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumBorderTop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumBorderLeft, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmsUpdate.ResumeLayout(False)
        CType(Me.pb100PWarning, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbUnElevate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbcSettings.ResumeLayout(False)
        Me.tabResolutions.ResumeLayout(False)
        Me.tabResolutions.PerformLayout()
        Me.cmsRestore.ResumeLayout(False)
        Me.cmsGenerate.ResumeLayout(False)
        Me.tabHotkeys.ResumeLayout(False)
        Me.tabHotkeys.PerformLayout()
        Me.grpAlterOverview.ResumeLayout(False)
        Me.grpAlterOverview.PerformLayout()
        Me.grpToggleTopMost.ResumeLayout(False)
        Me.grpToggleTopMost.PerformLayout()
        Me.grpCloseAllShortcut.ResumeLayout(False)
        Me.grpCloseAllShortcut.PerformLayout()
        Me.grpCycleShortcut.ResumeLayout(False)
        Me.grpCycleShortcut.PerformLayout()
        Me.grpOverviewShortcut.ResumeLayout(False)
        Me.grpOverviewShortcut.PerformLayout()
        Me.tabSortAndBL.ResumeLayout(False)
        Me.tabSortAndBL.PerformLayout()
        Me.tabMaximized.ResumeLayout(False)
        Me.tabMaximized.PerformLayout()
        Me.TabQL.ResumeLayout(False)
        Me.TabQL.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tabMisc.ResumeLayout(False)
        Me.tabMisc.PerformLayout()
        Me.pnlElevation.ResumeLayout(False)
        Me.pnlElevation.PerformLayout()
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
    Friend WithEvents txtExe As TextBox
    Friend WithEvents chkTopMost As CheckBox
    Friend WithEvents chkRoundCorners As CheckBox
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
    Friend WithEvents btnGenerate As Button
    Friend WithEvents cmsGenerate As ContextMenuStrip
    Friend WithEvents X60043ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents X720169ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FromToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DummyToolStripMenuItem As ToolStripMenuItem
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
    Friend WithEvents chkOverViewIsGame As CheckBox
    Friend WithEvents btnGrabCurrent As Button
    Friend WithEvents btnAddCurrentRes As Button
    Friend WithEvents cboScalingMode As ComboBox
    Friend WithEvents chkCycleOnClose As CheckBox
    Friend WithEvents chkCheckForUpdate As CheckBox
    Friend WithEvents chkStartupMax As CheckBox
    Friend WithEvents ChkSizeBorder As CheckBox
    Friend WithEvents cmsRestore As ContextMenuStrip
    Friend WithEvents LastSavedToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DefaultToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChkMinMin As CheckBox
    Friend WithEvents grpCloseAllShortcut As GroupBox
    Friend WithEvents chkCAShift As CheckBox
    Friend WithEvents txtCloseAll As TextBox
    Friend WithEvents chkCAALt As CheckBox
    Friend WithEvents chkCACtrl As CheckBox
    Friend WithEvents chkCloseAll As CheckBox
    Friend WithEvents cmbTheme As ComboBox
    Friend WithEvents Label16 As Label
    Friend WithEvents cmsUpdate As ContextMenuStrip
    Friend WithEvents CheckNowToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenChangelogToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents chkHoverActivate As CheckBox
    Friend WithEvents chkShowEnd As CheckBox
    Friend WithEvents chkToggleTopMost As CheckBox
    Friend WithEvents grpToggleTopMost As GroupBox
    Friend WithEvents chkTogTopShift As CheckBox
    Friend WithEvents txtTogTop As TextBox
    Friend WithEvents chkTogTopWin As CheckBox
    Friend WithEvents chkTogTopCtrl As CheckBox
    Friend WithEvents chkStoWin As CheckBox
    Friend WithEvents chkTogTopAlt As CheckBox
    Friend WithEvents chkCAWin As CheckBox
    Friend WithEvents chkCycleDownWin As CheckBox
    Friend WithEvents chkCycleUpWin As CheckBox
    Friend WithEvents chkAlterOverview As CheckBox
    Friend WithEvents grpAlterOverview As GroupBox
    Friend WithEvents chkAlterOverviewMinAlt As CheckBox
    Friend WithEvents txtAlterOverviewMinKey As TextBox
    Friend WithEvents chkAlterOverviewPlusALt As CheckBox
    Friend WithEvents txtAlterOverviewPlusKey As TextBox
    Friend WithEvents chkAlterOverviewMinShift As CheckBox
    Friend WithEvents chkAlterOverviewPlusShift As CheckBox
    Friend WithEvents chkAlterOverviewMinWin As CheckBox
    Friend WithEvents chkAlterOverviewPlusWin As CheckBox
    Friend WithEvents chkAlterOverviewMinCtrl As CheckBox
    Friend WithEvents chkAlterOverviewPlusCtrl As CheckBox
    Friend WithEvents chkAlterOverviewStarAlt As CheckBox
    Friend WithEvents txtAlterOverviewStarKey As TextBox
    Friend WithEvents chkAlterOverviewStarShift As CheckBox
    Friend WithEvents chkAlterOverviewStarWin As CheckBox
    Friend WithEvents chkAlterOverviewStarCtrl As CheckBox
    Friend WithEvents ChkLessRowCol As CheckBox
    Friend WithEvents NumExtraMax As NumericUpDown
    Friend WithEvents chkApplyAlterNormal As CheckBox
    Friend WithEvents chkMinMaxOnSwitch As CheckBox
    Friend WithEvents ResetIconCacheToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents btnGoToAdjustHotkey As Button
    Friend WithEvents chkBlockWin As CheckBox
    Friend WithEvents chkOnlyEsc As CheckBox
    Friend WithEvents chkAutoCloseSomeone As CheckBox
    Friend WithEvents chkNoAltTab As CheckBox
    Friend WithEvents pb100PWarning As PictureBox
    Friend WithEvents chkAutoCloseOnlyOnNoSome As CheckBox
    Friend WithEvents chkAllowShiftEsc As CheckBox
    Friend WithEvents pnlElevation As Panel
    Friend WithEvents pbUnElevate As PictureBox
    Friend WithEvents lblElevated As Label
    Friend WithEvents TabQL As TabPage
    Friend WithEvents ChkQLResolveLnk As CheckBox
    Friend WithEvents ChkQLShowHidden As CheckBox
    Friend WithEvents btnOpenFolderDialog As Button
    Friend WithEvents txtQuickLaunchPath As TextBox
    Friend WithEvents lblDefaultFilter As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents btnRefreshICdisplay As Button
    Friend WithEvents lblICSize As Label
    Friend WithEvents lblICacheCount As Label
    Friend WithEvents btnResetCache As Button
    Friend WithEvents TxtFilterAddExt As TextBox
    Friend WithEvents cmbPriority As ComboBox
End Class
