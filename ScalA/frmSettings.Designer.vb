﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
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
        Dim Label15 As System.Windows.Forms.Label
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmSettings))
        Me.txtQuickLaunchPath = New System.Windows.Forms.TextBox()
        Me.ChkQLShowHidden = New System.Windows.Forms.CheckBox()
        Me.btnOpenFolderDialog = New System.Windows.Forms.Button()
        Me.cmsQLFolder = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenInExplorerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.ChkLessRowCol = New System.Windows.Forms.CheckBox()
        Me.chkOverViewIsGame = New System.Windows.Forms.CheckBox()
        Me.cboScalingMode = New System.Windows.Forms.ComboBox()
        Me.chkCycleOnClose = New System.Windows.Forms.CheckBox()
        Me.chkCheckForUpdate = New System.Windows.Forms.CheckBox()
        Me.cmsUpdate = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CheckNowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenChangelogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChkMinMin = New System.Windows.Forms.CheckBox()
        Me.chkCloseAll = New System.Windows.Forms.CheckBox()
        Me.tbcSettings = New System.Windows.Forms.TabControl()
        Me.tabResolutions = New System.Windows.Forms.TabPage()
        Me.ChkSizeBorder = New System.Windows.Forms.CheckBox()
        Me.btnAddCurrentRes = New System.Windows.Forms.Button()
        Me.btnRestore = New System.Windows.Forms.Button()
        Me.cmsRestore = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.LastSavedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DefaultToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnSort = New System.Windows.Forms.Button()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.cmsGenerate = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.X60043ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.X720169ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FromToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DummyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtResolutions = New System.Windows.Forms.TextBox()
        Me.tabHotkeys = New System.Windows.Forms.TabPage()
        Me.grpCloseAllShortcut = New System.Windows.Forms.GroupBox()
        Me.chkCAShift = New System.Windows.Forms.CheckBox()
        Me.txtCloseAll = New System.Windows.Forms.TextBox()
        Me.chkCAALt = New System.Windows.Forms.CheckBox()
        Me.chkCACtrl = New System.Windows.Forms.CheckBox()
        Me.chkSwitchToOverview = New System.Windows.Forms.CheckBox()
        Me.chkCycleAlts = New System.Windows.Forms.CheckBox()
        Me.grpCycleShortcut = New System.Windows.Forms.GroupBox()
        Me.chkCycleDownShift = New System.Windows.Forms.CheckBox()
        Me.chkCycleUpShift = New System.Windows.Forms.CheckBox()
        Me.chkCycleDownAlt = New System.Windows.Forms.CheckBox()
        Me.chkCycleDownCtrl = New System.Windows.Forms.CheckBox()
        Me.txtCycleKeyDown = New System.Windows.Forms.TextBox()
        Me.chkCycleUpAlt = New System.Windows.Forms.CheckBox()
        Me.chkCycleUpCtrl = New System.Windows.Forms.CheckBox()
        Me.txtCycleKeyUp = New System.Windows.Forms.TextBox()
        Me.grpOverviewShortcut = New System.Windows.Forms.GroupBox()
        Me.chkStoShift = New System.Windows.Forms.CheckBox()
        Me.txtStoKey = New System.Windows.Forms.TextBox()
        Me.chkStoAlt = New System.Windows.Forms.CheckBox()
        Me.chkStoCtrl = New System.Windows.Forms.CheckBox()
        Me.tabSortAndBL = New System.Windows.Forms.TabPage()
        Me.btnTest = New System.Windows.Forms.Button()
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.txtBotSort = New System.Windows.Forms.TextBox()
        Me.txtTopSort = New System.Windows.Forms.TextBox()
        Me.chkWhitelist = New System.Windows.Forms.CheckBox()
        Me.tabMaximized = New System.Windows.Forms.TabPage()
        Me.chkStartupMax = New System.Windows.Forms.CheckBox()
        Me.NumExtraMax = New System.Windows.Forms.NumericUpDown()
        Me.tabMisc = New System.Windows.Forms.TabPage()
        Me.cmbTheme = New System.Windows.Forms.ComboBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.chkDoAlign = New System.Windows.Forms.CheckBox()
        Me.grpAlign = New System.Windows.Forms.GroupBox()
        Me.btnResetAlign = New System.Windows.Forms.Button()
        Me.numXoffset = New System.Windows.Forms.NumericUpDown()
        Me.numYoffset = New System.Windows.Forms.NumericUpDown()
        Me.txtExe = New System.Windows.Forms.TextBox()
        Me.txtClass = New System.Windows.Forms.TextBox()
        Me.chkTopMost = New System.Windows.Forms.CheckBox()
        Me.chkRoundCorners = New System.Windows.Forms.CheckBox()
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
        Label15 = New System.Windows.Forms.Label()
        grpQLPath.SuspendLayout()
        Me.cmsQLFolder.SuspendLayout()
        Me.grpReserveSpace.SuspendLayout()
        CType(Me.NumBorderBot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumBorderRight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumBorderTop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumBorderLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmsUpdate.SuspendLayout()
        Me.tbcSettings.SuspendLayout()
        Me.tabResolutions.SuspendLayout()
        Me.cmsRestore.SuspendLayout()
        Me.cmsGenerate.SuspendLayout()
        Me.tabHotkeys.SuspendLayout()
        Me.grpCloseAllShortcut.SuspendLayout()
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
        grpQLPath.Controls.Add(Me.ChkQLShowHidden)
        grpQLPath.Controls.Add(Me.btnOpenFolderDialog)
        grpQLPath.Location = New System.Drawing.Point(7, -2)
        grpQLPath.Name = "grpQLPath"
        grpQLPath.Size = New System.Drawing.Size(291, 40)
        grpQLPath.TabIndex = 16
        grpQLPath.TabStop = False
        grpQLPath.Text = "QuickLaunch"
        '
        'txtQuickLaunchPath
        '
        Me.txtQuickLaunchPath.Location = New System.Drawing.Point(5, 16)
        Me.txtQuickLaunchPath.Name = "txtQuickLaunchPath"
        Me.txtQuickLaunchPath.Size = New System.Drawing.Size(259, 20)
        Me.txtQuickLaunchPath.TabIndex = 0
        Me.txtQuickLaunchPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ChkQLShowHidden
        '
        Me.ChkQLShowHidden.AutoSize = True
        Me.ChkQLShowHidden.Location = New System.Drawing.Point(140, 1)
        Me.ChkQLShowHidden.Name = "ChkQLShowHidden"
        Me.ChkQLShowHidden.Size = New System.Drawing.Size(126, 17)
        Me.ChkQLShowHidden.TabIndex = 26
        Me.ChkQLShowHidden.Text = "Always Show Hidden"
        Me.ttSettings.SetToolTip(Me.ChkQLShowHidden, "Always Show Hidden and System Items." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Press Ctrl + Shift to override when this se" &
        "tting is off.")
        Me.ChkQLShowHidden.UseVisualStyleBackColor = True
        '
        'btnOpenFolderDialog
        '
        Me.btnOpenFolderDialog.ContextMenuStrip = Me.cmsQLFolder
        Me.btnOpenFolderDialog.FlatAppearance.BorderSize = 0
        Me.btnOpenFolderDialog.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOpenFolderDialog.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOpenFolderDialog.Location = New System.Drawing.Point(263, 16)
        Me.btnOpenFolderDialog.Name = "btnOpenFolderDialog"
        Me.btnOpenFolderDialog.Size = New System.Drawing.Size(22, 20)
        Me.btnOpenFolderDialog.TabIndex = 1
        Me.btnOpenFolderDialog.Text = ".."
        Me.btnOpenFolderDialog.UseVisualStyleBackColor = True
        '
        'cmsQLFolder
        '
        Me.cmsQLFolder.ImageScalingSize = New System.Drawing.Size(20, 20)
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
        Label3.Location = New System.Drawing.Point(166, 140)
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
        Label6.Location = New System.Drawing.Point(15, 129)
        Label6.Name = "Label6"
        Label6.Size = New System.Drawing.Size(106, 13)
        Label6.TabIndex = 28
        Label6.Text = "Extra Columns/Rows"
        '
        'Label15
        '
        Label15.AutoSize = True
        Label15.Location = New System.Drawing.Point(200, 36)
        Label15.Name = "Label15"
        Label15.Size = New System.Drawing.Size(72, 13)
        Label15.TabIndex = 7
        Label15.Text = "Scaling Mode"
        Me.ttSettings.SetToolTip(Label15, "Auto will use Pixel Mode when scaling factor is 2x or more." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Note: Pixel Mode is " &
        "disabled when Windows Scaling is not 100%" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(141, 214)
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
        Me.btnCancel.Location = New System.Drawing.Point(224, 214)
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
        'ChkSingleInstance
        '
        Me.ChkSingleInstance.AutoSize = True
        Me.ChkSingleInstance.Location = New System.Drawing.Point(12, 90)
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
        Me.grpReserveSpace.Location = New System.Drawing.Point(49, 31)
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
        'ChkLessRowCol
        '
        Me.ChkLessRowCol.AutoSize = True
        Me.ChkLessRowCol.Checked = True
        Me.ChkLessRowCol.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkLessRowCol.Location = New System.Drawing.Point(18, 107)
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
        Me.chkOverViewIsGame.Location = New System.Drawing.Point(159, 44)
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
        Me.cboScalingMode.Location = New System.Drawing.Point(200, 52)
        Me.cboScalingMode.Name = "cboScalingMode"
        Me.cboScalingMode.Size = New System.Drawing.Size(97, 21)
        Me.cboScalingMode.TabIndex = 8
        Me.ttSettings.SetToolTip(Me.cboScalingMode, "Auto will use Pixel Mode when scaling factor is 2x or more." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Note: Pixel Mode is " &
        "disabled when Windows Scaling is not 100%" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        '
        'chkCycleOnClose
        '
        Me.chkCycleOnClose.AutoSize = True
        Me.chkCycleOnClose.Location = New System.Drawing.Point(161, 57)
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
        Me.chkCheckForUpdate.Location = New System.Drawing.Point(12, 113)
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
        Me.ChkMinMin.Location = New System.Drawing.Point(171, 129)
        Me.ChkMinMin.Name = "ChkMinMin"
        Me.ChkMinMin.Size = New System.Drawing.Size(104, 17)
        Me.ChkMinMin.TabIndex = 30
        Me.ChkMinMin.Text = "Min. on Minimize"
        Me.ttSettings.SetToolTip(Me.ChkMinMin, "Minimizing ScalA will also Minimise Astonia" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Note: This has no effect on legacy c" &
        "lients")
        Me.ChkMinMin.UseVisualStyleBackColor = True
        '
        'chkCloseAll
        '
        Me.chkCloseAll.AutoSize = True
        Me.chkCloseAll.Location = New System.Drawing.Point(49, 131)
        Me.chkCloseAll.Name = "chkCloseAll"
        Me.chkCloseAll.Size = New System.Drawing.Size(152, 17)
        Me.chkCloseAll.TabIndex = 19
        Me.chkCloseAll.TabStop = False
        Me.chkCloseAll.Text = "Close All Clients and ScalA"
        Me.ttSettings.SetToolTip(Me.chkCloseAll, "Note: Whitelist setting from Sorting tab is in effect here")
        Me.chkCloseAll.UseVisualStyleBackColor = True
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
        Me.tbcSettings.Size = New System.Drawing.Size(316, 212)
        Me.tbcSettings.TabIndex = 14
        Me.tbcSettings.TabStop = False
        '
        'tabResolutions
        '
        Me.tabResolutions.Controls.Add(Me.ChkSizeBorder)
        Me.tabResolutions.Controls.Add(Me.cboScalingMode)
        Me.tabResolutions.Controls.Add(Label15)
        Me.tabResolutions.Controls.Add(Me.btnAddCurrentRes)
        Me.tabResolutions.Controls.Add(Me.btnRestore)
        Me.tabResolutions.Controls.Add(Me.btnSort)
        Me.tabResolutions.Controls.Add(Me.btnGenerate)
        Me.tabResolutions.Controls.Add(Me.txtResolutions)
        Me.tabResolutions.Location = New System.Drawing.Point(4, 25)
        Me.tabResolutions.Name = "tabResolutions"
        Me.tabResolutions.Padding = New System.Windows.Forms.Padding(3)
        Me.tabResolutions.Size = New System.Drawing.Size(308, 183)
        Me.tabResolutions.TabIndex = 0
        Me.tabResolutions.Text = "Resolutions"
        Me.tabResolutions.UseVisualStyleBackColor = True
        '
        'ChkSizeBorder
        '
        Me.ChkSizeBorder.AutoSize = True
        Me.ChkSizeBorder.Location = New System.Drawing.Point(204, 80)
        Me.ChkSizeBorder.Name = "ChkSizeBorder"
        Me.ChkSizeBorder.Size = New System.Drawing.Size(88, 17)
        Me.ChkSizeBorder.TabIndex = 9
        Me.ChkSizeBorder.Text = "Sizing Border"
        Me.ChkSizeBorder.UseVisualStyleBackColor = True
        '
        'btnAddCurrentRes
        '
        Me.btnAddCurrentRes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAddCurrentRes.Location = New System.Drawing.Point(105, 89)
        Me.btnAddCurrentRes.Name = "btnAddCurrentRes"
        Me.btnAddCurrentRes.Size = New System.Drawing.Size(75, 23)
        Me.btnAddCurrentRes.TabIndex = 6
        Me.btnAddCurrentRes.Text = "Add Current"
        Me.btnAddCurrentRes.UseVisualStyleBackColor = True
        '
        'btnRestore
        '
        Me.btnRestore.ContextMenuStrip = Me.cmsRestore
        Me.btnRestore.Location = New System.Drawing.Point(105, 126)
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
        'btnSort
        '
        Me.btnSort.Location = New System.Drawing.Point(105, 60)
        Me.btnSort.Name = "btnSort"
        Me.btnSort.Size = New System.Drawing.Size(75, 23)
        Me.btnSort.TabIndex = 3
        Me.btnSort.Text = "Sort"
        Me.btnSort.UseVisualStyleBackColor = True
        '
        'btnGenerate
        '
        Me.btnGenerate.ContextMenuStrip = Me.cmsGenerate
        Me.btnGenerate.Location = New System.Drawing.Point(105, 31)
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
        Me.txtResolutions.Location = New System.Drawing.Point(5, 0)
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
        Me.tabHotkeys.Controls.Add(Me.chkCloseAll)
        Me.tabHotkeys.Controls.Add(Me.grpCloseAllShortcut)
        Me.tabHotkeys.Controls.Add(Me.chkCycleOnClose)
        Me.tabHotkeys.Controls.Add(Me.chkSwitchToOverview)
        Me.tabHotkeys.Controls.Add(Me.chkCycleAlts)
        Me.tabHotkeys.Controls.Add(Me.grpCycleShortcut)
        Me.tabHotkeys.Controls.Add(Me.grpOverviewShortcut)
        Me.tabHotkeys.Location = New System.Drawing.Point(4, 25)
        Me.tabHotkeys.Name = "tabHotkeys"
        Me.tabHotkeys.Size = New System.Drawing.Size(308, 183)
        Me.tabHotkeys.TabIndex = 2
        Me.tabHotkeys.Text = "Hotkeys"
        Me.tabHotkeys.UseVisualStyleBackColor = True
        '
        'grpCloseAllShortcut
        '
        Me.grpCloseAllShortcut.Controls.Add(Me.chkCAShift)
        Me.grpCloseAllShortcut.Controls.Add(Me.txtCloseAll)
        Me.grpCloseAllShortcut.Controls.Add(Me.chkCAALt)
        Me.grpCloseAllShortcut.Controls.Add(Me.chkCACtrl)
        Me.grpCloseAllShortcut.Location = New System.Drawing.Point(46, 132)
        Me.grpCloseAllShortcut.Name = "grpCloseAllShortcut"
        Me.grpCloseAllShortcut.Size = New System.Drawing.Size(214, 45)
        Me.grpCloseAllShortcut.TabIndex = 17
        Me.grpCloseAllShortcut.TabStop = False
        '
        'chkCAShift
        '
        Me.chkCAShift.AutoSize = True
        Me.chkCAShift.Location = New System.Drawing.Point(84, 21)
        Me.chkCAShift.Name = "chkCAShift"
        Me.chkCAShift.Size = New System.Drawing.Size(47, 17)
        Me.chkCAShift.TabIndex = 22
        Me.chkCAShift.TabStop = False
        Me.chkCAShift.Text = "Shift"
        Me.chkCAShift.UseVisualStyleBackColor = True
        '
        'txtCloseAll
        '
        Me.txtCloseAll.Location = New System.Drawing.Point(134, 18)
        Me.txtCloseAll.Name = "txtCloseAll"
        Me.txtCloseAll.Size = New System.Drawing.Size(74, 20)
        Me.txtCloseAll.TabIndex = 21
        Me.txtCloseAll.TabStop = False
        '
        'chkCAALt
        '
        Me.chkCAALt.AutoSize = True
        Me.chkCAALt.Location = New System.Drawing.Point(46, 21)
        Me.chkCAALt.Name = "chkCAALt"
        Me.chkCAALt.Size = New System.Drawing.Size(38, 17)
        Me.chkCAALt.TabIndex = 20
        Me.chkCAALt.TabStop = False
        Me.chkCAALt.Text = "Alt"
        Me.chkCAALt.UseVisualStyleBackColor = True
        '
        'chkCACtrl
        '
        Me.chkCACtrl.AutoSize = True
        Me.chkCACtrl.Location = New System.Drawing.Point(6, 21)
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
        Me.chkSwitchToOverview.Location = New System.Drawing.Point(48, 6)
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
        Me.chkCycleAlts.Location = New System.Drawing.Point(48, 57)
        Me.chkCycleAlts.Name = "chkCycleAlts"
        Me.chkCycleAlts.Size = New System.Drawing.Size(102, 17)
        Me.chkCycleAlts.TabIndex = 10
        Me.chkCycleAlts.TabStop = False
        Me.chkCycleAlts.Text = "Cycle Up/Down"
        Me.chkCycleAlts.UseVisualStyleBackColor = True
        '
        'grpCycleShortcut
        '
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleDownShift)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleUpShift)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleDownAlt)
        Me.grpCycleShortcut.Controls.Add(Me.chkCycleDownCtrl)
        Me.grpCycleShortcut.Controls.Add(Me.txtCycleKeyDown)
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
        Me.chkCycleDownShift.Location = New System.Drawing.Point(84, 43)
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
        Me.chkCycleUpShift.Location = New System.Drawing.Point(84, 20)
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
        Me.chkCycleDownAlt.Location = New System.Drawing.Point(46, 43)
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
        Me.chkCycleDownCtrl.Location = New System.Drawing.Point(6, 43)
        Me.chkCycleDownCtrl.Name = "chkCycleDownCtrl"
        Me.chkCycleDownCtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkCycleDownCtrl.TabIndex = 12
        Me.chkCycleDownCtrl.TabStop = False
        Me.chkCycleDownCtrl.Text = "Ctrl"
        Me.chkCycleDownCtrl.UseVisualStyleBackColor = True
        '
        'txtCycleKeyDown
        '
        Me.txtCycleKeyDown.Location = New System.Drawing.Point(134, 41)
        Me.txtCycleKeyDown.Name = "txtCycleKeyDown"
        Me.txtCycleKeyDown.Size = New System.Drawing.Size(74, 20)
        Me.txtCycleKeyDown.TabIndex = 11
        Me.txtCycleKeyDown.TabStop = False
        '
        'chkCycleUpAlt
        '
        Me.chkCycleUpAlt.AutoSize = True
        Me.chkCycleUpAlt.Location = New System.Drawing.Point(46, 20)
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
        Me.chkCycleUpCtrl.Location = New System.Drawing.Point(6, 20)
        Me.chkCycleUpCtrl.Name = "chkCycleUpCtrl"
        Me.chkCycleUpCtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkCycleUpCtrl.TabIndex = 8
        Me.chkCycleUpCtrl.TabStop = False
        Me.chkCycleUpCtrl.Text = "Ctrl"
        Me.chkCycleUpCtrl.UseVisualStyleBackColor = True
        '
        'txtCycleKeyUp
        '
        Me.txtCycleKeyUp.Location = New System.Drawing.Point(134, 18)
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
        Me.tabSortAndBL.Size = New System.Drawing.Size(308, 183)
        Me.tabSortAndBL.TabIndex = 3
        Me.tabSortAndBL.Text = "Sorting"
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
        Me.txtBotSort.Size = New System.Drawing.Size(130, 168)
        Me.txtBotSort.TabIndex = 1
        Me.txtBotSort.Text = "Someone"
        '
        'txtTopSort
        '
        Me.txtTopSort.Location = New System.Drawing.Point(8, 15)
        Me.txtTopSort.Multiline = True
        Me.txtTopSort.Name = "txtTopSort"
        Me.txtTopSort.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtTopSort.Size = New System.Drawing.Size(130, 168)
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
        Me.tabMaximized.Controls.Add(Me.ChkMinMin)
        Me.tabMaximized.Controls.Add(Me.chkStartupMax)
        Me.tabMaximized.Controls.Add(Me.ChkLessRowCol)
        Me.tabMaximized.Controls.Add(Me.NumExtraMax)
        Me.tabMaximized.Controls.Add(Label6)
        Me.tabMaximized.Controls.Add(Me.grpReserveSpace)
        Me.tabMaximized.Location = New System.Drawing.Point(4, 25)
        Me.tabMaximized.Name = "tabMaximized"
        Me.tabMaximized.Size = New System.Drawing.Size(308, 183)
        Me.tabMaximized.TabIndex = 4
        Me.tabMaximized.Text = "Min/Max"
        Me.tabMaximized.UseVisualStyleBackColor = True
        '
        'chkStartupMax
        '
        Me.chkStartupMax.AutoSize = True
        Me.chkStartupMax.Location = New System.Drawing.Point(171, 107)
        Me.chkStartupMax.Name = "chkStartupMax"
        Me.chkStartupMax.Size = New System.Drawing.Size(100, 17)
        Me.chkStartupMax.TabIndex = 29
        Me.chkStartupMax.Text = "Start Maximized"
        Me.chkStartupMax.UseVisualStyleBackColor = True
        '
        'NumExtraMax
        '
        Me.NumExtraMax.Location = New System.Drawing.Point(122, 127)
        Me.NumExtraMax.Maximum = New Decimal(New Integer() {9, 0, 0, 0})
        Me.NumExtraMax.Name = "NumExtraMax"
        Me.NumExtraMax.Size = New System.Drawing.Size(40, 20)
        Me.NumExtraMax.TabIndex = 5
        Me.NumExtraMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tabMisc
        '
        Me.tabMisc.Controls.Add(Me.cmbTheme)
        Me.tabMisc.Controls.Add(Me.Label16)
        Me.tabMisc.Controls.Add(Me.chkDoAlign)
        Me.tabMisc.Controls.Add(Me.grpAlign)
        Me.tabMisc.Controls.Add(Me.txtExe)
        Me.tabMisc.Controls.Add(grpQLPath)
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
        Me.tabMisc.Size = New System.Drawing.Size(308, 183)
        Me.tabMisc.TabIndex = 1
        Me.tabMisc.Text = "Misc"
        Me.tabMisc.UseVisualStyleBackColor = True
        '
        'cmbTheme
        '
        Me.cmbTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTheme.FormattingEnabled = True
        Me.cmbTheme.Items.AddRange(New Object() {"System", "Light", "Dark"})
        Me.cmbTheme.Location = New System.Drawing.Point(200, 63)
        Me.cmbTheme.Name = "cmbTheme"
        Me.cmbTheme.Size = New System.Drawing.Size(82, 21)
        Me.cmbTheme.TabIndex = 25
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(156, 68)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(40, 13)
        Me.Label16.TabIndex = 24
        Me.Label16.Text = "Theme"
        '
        'chkDoAlign
        '
        Me.chkDoAlign.AutoSize = True
        Me.chkDoAlign.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDoAlign.Location = New System.Drawing.Point(200, 121)
        Me.chkDoAlign.Name = "chkDoAlign"
        Me.chkDoAlign.Size = New System.Drawing.Size(66, 16)
        Me.chkDoAlign.TabIndex = 10
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
        Me.grpAlign.Location = New System.Drawing.Point(200, 121)
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
        Me.numXoffset.TabIndex = 12
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
        Me.numYoffset.TabIndex = 13
        Me.numYoffset.Tag = "1"
        Me.numYoffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtExe
        '
        Me.txtExe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExe.Location = New System.Drawing.Point(12, 137)
        Me.txtExe.Name = "txtExe"
        Me.txtExe.Size = New System.Drawing.Size(154, 20)
        Me.txtExe.TabIndex = 8
        Me.txtExe.Text = "moac | new"
        '
        'txtClass
        '
        Me.txtClass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClass.Location = New System.Drawing.Point(12, 163)
        Me.txtClass.Name = "txtClass"
        Me.txtClass.Size = New System.Drawing.Size(182, 20)
        Me.txtClass.TabIndex = 9
        Me.txtClass.Text = "MAINWNDMOAC | 䅍义乗䵄䅏C"
        '
        'chkTopMost
        '
        Me.chkTopMost.AutoSize = True
        Me.chkTopMost.Location = New System.Drawing.Point(12, 67)
        Me.chkTopMost.Name = "chkTopMost"
        Me.chkTopMost.Size = New System.Drawing.Size(98, 17)
        Me.chkTopMost.TabIndex = 3
        Me.chkTopMost.Text = "Always On Top"
        Me.chkTopMost.UseVisualStyleBackColor = True
        '
        'chkRoundCorners
        '
        Me.chkRoundCorners.AutoSize = True
        Me.chkRoundCorners.Location = New System.Drawing.Point(12, 44)
        Me.chkRoundCorners.Name = "chkRoundCorners"
        Me.chkRoundCorners.Size = New System.Drawing.Size(109, 17)
        Me.chkRoundCorners.TabIndex = 2
        Me.chkRoundCorners.Text = "Rounded Corners"
        Me.chkRoundCorners.UseVisualStyleBackColor = True
        '
        'FrmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ClientSize = New System.Drawing.Size(314, 244)
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
        Me.cmsUpdate.ResumeLayout(False)
        Me.tbcSettings.ResumeLayout(False)
        Me.tabResolutions.ResumeLayout(False)
        Me.tabResolutions.PerformLayout()
        Me.cmsRestore.ResumeLayout(False)
        Me.cmsGenerate.ResumeLayout(False)
        Me.tabHotkeys.ResumeLayout(False)
        Me.tabHotkeys.PerformLayout()
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
    Friend WithEvents ChkQLShowHidden As CheckBox
End Class
