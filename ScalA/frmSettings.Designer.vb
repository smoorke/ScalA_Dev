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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmSettings))
        Me.btnOpenFolderDialog = New System.Windows.Forms.Button()
        Me.cmsQLFolder = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenInExplorerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtQuickLaunchPath = New System.Windows.Forms.TextBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.tmrAlign = New System.Windows.Forms.Timer(Me.components)
        Me.ttSettings = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkOverViewIsGame = New System.Windows.Forms.CheckBox()
        Me.tbcSettings = New System.Windows.Forms.TabControl()
        Me.tabResolutions = New System.Windows.Forms.TabPage()
        Me.txtResolutions = New System.Windows.Forms.TextBox()
        Me.tabShortcuts = New System.Windows.Forms.TabPage()
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
        Me.tabMisc = New System.Windows.Forms.TabPage()
        Me.chkDoAlign = New System.Windows.Forms.CheckBox()
        Me.grpAlign = New System.Windows.Forms.GroupBox()
        Me.btnResetAlign = New System.Windows.Forms.Button()
        Me.numYoffset = New System.Windows.Forms.NumericUpDown()
        Me.numXoffset = New System.Windows.Forms.NumericUpDown()
        Me.txtExe = New System.Windows.Forms.TextBox()
        Me.chkTopMost = New System.Windows.Forms.CheckBox()
        Me.chkRoundCorners = New System.Windows.Forms.CheckBox()
        Me.txtClass = New System.Windows.Forms.TextBox()
        grpQLPath = New System.Windows.Forms.GroupBox()
        Label3 = New System.Windows.Forms.Label()
        Label2 = New System.Windows.Forms.Label()
        Label1 = New System.Windows.Forms.Label()
        grpQLPath.SuspendLayout()
        Me.cmsQLFolder.SuspendLayout()
        Me.tbcSettings.SuspendLayout()
        Me.tabResolutions.SuspendLayout()
        Me.tabShortcuts.SuspendLayout()
        Me.grpCycleShortcut.SuspendLayout()
        Me.grpOverviewShortcut.SuspendLayout()
        Me.tabMisc.SuspendLayout()
        Me.grpAlign.SuspendLayout()
        CType(Me.numYoffset, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numXoffset, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpQLPath
        '
        grpQLPath.Controls.Add(Me.btnOpenFolderDialog)
        grpQLPath.Controls.Add(Me.txtQuickLaunchPath)
        grpQLPath.Location = New System.Drawing.Point(7, 5)
        grpQLPath.Name = "grpQLPath"
        grpQLPath.Size = New System.Drawing.Size(213, 42)
        grpQLPath.TabIndex = 16
        grpQLPath.TabStop = False
        grpQLPath.Text = "QuickLaunch Path"
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
        'Label3
        '
        Label3.AutoSize = True
        Label3.Location = New System.Drawing.Point(78, 94)
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
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(72, 170)
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
        Me.btnCancel.Location = New System.Drawing.Point(155, 170)
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
        'chkOverViewIsGame
        '
        Me.chkOverViewIsGame.AutoSize = True
        Me.chkOverViewIsGame.Location = New System.Drawing.Point(7, 46)
        Me.chkOverViewIsGame.Name = "chkOverViewIsGame"
        Me.chkOverViewIsGame.Size = New System.Drawing.Size(107, 17)
        Me.chkOverViewIsGame.TabIndex = 20
        Me.chkOverViewIsGame.Text = "Active Overview "
        Me.ttSettings.SetToolTip(Me.chkOverViewIsGame, "When this is enabled" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "the overview thumbnails" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "will function as game.")
        Me.chkOverViewIsGame.UseVisualStyleBackColor = True
        '
        'tbcSettings
        '
        Me.tbcSettings.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tbcSettings.Controls.Add(Me.tabResolutions)
        Me.tbcSettings.Controls.Add(Me.tabShortcuts)
        Me.tbcSettings.Controls.Add(Me.tabMisc)
        Me.tbcSettings.Location = New System.Drawing.Point(0, 0)
        Me.tbcSettings.Name = "tbcSettings"
        Me.tbcSettings.SelectedIndex = 0
        Me.tbcSettings.Size = New System.Drawing.Size(238, 169)
        Me.tbcSettings.TabIndex = 14
        Me.tbcSettings.TabStop = False
        '
        'tabResolutions
        '
        Me.tabResolutions.Controls.Add(Me.txtResolutions)
        Me.tabResolutions.Location = New System.Drawing.Point(4, 25)
        Me.tabResolutions.Name = "tabResolutions"
        Me.tabResolutions.Padding = New System.Windows.Forms.Padding(3)
        Me.tabResolutions.Size = New System.Drawing.Size(230, 140)
        Me.tabResolutions.TabIndex = 0
        Me.tabResolutions.Text = "Resolutions"
        Me.tabResolutions.UseVisualStyleBackColor = True
        '
        'txtResolutions
        '
        Me.txtResolutions.Location = New System.Drawing.Point(0, 0)
        Me.txtResolutions.Multiline = True
        Me.txtResolutions.Name = "txtResolutions"
        Me.txtResolutions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtResolutions.Size = New System.Drawing.Size(94, 140)
        Me.txtResolutions.TabIndex = 1
        Me.txtResolutions.Text = "800x600" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1200x900" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1600x1200" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2000x1500" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2400x1800" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2800x2100" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3200x2400" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3600x27" &
    "00" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "4000x3000" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "4400x3300"
        '
        'tabShortcuts
        '
        Me.tabShortcuts.Controls.Add(Me.grpCycleShortcut)
        Me.tabShortcuts.Controls.Add(Me.grpOverviewShortcut)
        Me.tabShortcuts.Location = New System.Drawing.Point(4, 25)
        Me.tabShortcuts.Name = "tabShortcuts"
        Me.tabShortcuts.Size = New System.Drawing.Size(230, 140)
        Me.tabShortcuts.TabIndex = 2
        Me.tabShortcuts.Text = "Shortcuts"
        Me.tabShortcuts.UseVisualStyleBackColor = True
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
        Me.grpCycleShortcut.Location = New System.Drawing.Point(8, 59)
        Me.grpCycleShortcut.Name = "grpCycleShortcut"
        Me.grpCycleShortcut.Size = New System.Drawing.Size(214, 67)
        Me.grpCycleShortcut.TabIndex = 1
        Me.grpCycleShortcut.TabStop = False
        '
        'chkCycleDownShift
        '
        Me.chkCycleDownShift.AutoSize = True
        Me.chkCycleDownShift.Location = New System.Drawing.Point(84, 42)
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
        Me.chkCycleUpShift.Location = New System.Drawing.Point(84, 21)
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
        Me.chkCycleDownAlt.Location = New System.Drawing.Point(46, 42)
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
        Me.chkCycleDownCtrl.Location = New System.Drawing.Point(6, 42)
        Me.chkCycleDownCtrl.Name = "chkCycleDownCtrl"
        Me.chkCycleDownCtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkCycleDownCtrl.TabIndex = 12
        Me.chkCycleDownCtrl.TabStop = False
        Me.chkCycleDownCtrl.Text = "Ctrl"
        Me.chkCycleDownCtrl.UseVisualStyleBackColor = True
        '
        'txtCycleKeyDown
        '
        Me.txtCycleKeyDown.Location = New System.Drawing.Point(134, 40)
        Me.txtCycleKeyDown.Name = "txtCycleKeyDown"
        Me.txtCycleKeyDown.Size = New System.Drawing.Size(74, 20)
        Me.txtCycleKeyDown.TabIndex = 11
        Me.txtCycleKeyDown.TabStop = False
        '
        'chkCycleAlts
        '
        Me.chkCycleAlts.AutoSize = True
        Me.chkCycleAlts.Location = New System.Drawing.Point(0, -1)
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
        Me.chkCycleUpAlt.Location = New System.Drawing.Point(46, 21)
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
        Me.chkCycleUpCtrl.Location = New System.Drawing.Point(6, 21)
        Me.chkCycleUpCtrl.Name = "chkCycleUpCtrl"
        Me.chkCycleUpCtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkCycleUpCtrl.TabIndex = 8
        Me.chkCycleUpCtrl.TabStop = False
        Me.chkCycleUpCtrl.Text = "Ctrl"
        Me.chkCycleUpCtrl.UseVisualStyleBackColor = True
        '
        'txtCycleKeyUp
        '
        Me.txtCycleKeyUp.Location = New System.Drawing.Point(134, 19)
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
        Me.grpOverviewShortcut.Location = New System.Drawing.Point(8, 8)
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
        'tabMisc
        '
        Me.tabMisc.Controls.Add(Me.chkDoAlign)
        Me.tabMisc.Controls.Add(Me.grpAlign)
        Me.tabMisc.Controls.Add(Me.txtExe)
        Me.tabMisc.Controls.Add(Me.chkTopMost)
        Me.tabMisc.Controls.Add(Me.chkRoundCorners)
        Me.tabMisc.Controls.Add(grpQLPath)
        Me.tabMisc.Controls.Add(Label3)
        Me.tabMisc.Controls.Add(Me.txtClass)
        Me.tabMisc.Controls.Add(Me.chkOverViewIsGame)
        Me.tabMisc.Location = New System.Drawing.Point(4, 25)
        Me.tabMisc.Name = "tabMisc"
        Me.tabMisc.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMisc.Size = New System.Drawing.Size(230, 140)
        Me.tabMisc.TabIndex = 1
        Me.tabMisc.Text = "Misc"
        Me.tabMisc.UseVisualStyleBackColor = True
        '
        'chkDoAlign
        '
        Me.chkDoAlign.AutoSize = True
        Me.chkDoAlign.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDoAlign.Location = New System.Drawing.Point(114, 70)
        Me.chkDoAlign.Name = "chkDoAlign"
        Me.chkDoAlign.Size = New System.Drawing.Size(66, 16)
        Me.chkDoAlign.TabIndex = 22
        Me.chkDoAlign.Text = "Alignment"
        Me.chkDoAlign.UseVisualStyleBackColor = True
        '
        'grpAlign
        '
        Me.grpAlign.Controls.Add(Me.btnResetAlign)
        Me.grpAlign.Controls.Add(Me.numYoffset)
        Me.grpAlign.Controls.Add(Me.numXoffset)
        Me.grpAlign.Controls.Add(Label2)
        Me.grpAlign.Controls.Add(Label1)
        Me.grpAlign.Enabled = False
        Me.grpAlign.Location = New System.Drawing.Point(114, 70)
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
        'txtExe
        '
        Me.txtExe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExe.Location = New System.Drawing.Point(7, 91)
        Me.txtExe.Name = "txtExe"
        Me.txtExe.Size = New System.Drawing.Size(66, 20)
        Me.txtExe.TabIndex = 15
        Me.txtExe.Text = "moac | new"
        '
        'chkTopMost
        '
        Me.chkTopMost.AutoSize = True
        Me.chkTopMost.Location = New System.Drawing.Point(7, 74)
        Me.chkTopMost.Name = "chkTopMost"
        Me.chkTopMost.Size = New System.Drawing.Size(98, 17)
        Me.chkTopMost.TabIndex = 14
        Me.chkTopMost.Text = "Always On Top"
        Me.chkTopMost.UseVisualStyleBackColor = True
        '
        'chkRoundCorners
        '
        Me.chkRoundCorners.AutoSize = True
        Me.chkRoundCorners.Location = New System.Drawing.Point(7, 60)
        Me.chkRoundCorners.Name = "chkRoundCorners"
        Me.chkRoundCorners.Size = New System.Drawing.Size(109, 17)
        Me.chkRoundCorners.TabIndex = 19
        Me.chkRoundCorners.Text = "Rounded Corners"
        Me.chkRoundCorners.UseVisualStyleBackColor = True
        '
        'txtClass
        '
        Me.txtClass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClass.Location = New System.Drawing.Point(7, 113)
        Me.txtClass.Name = "txtClass"
        Me.txtClass.Size = New System.Drawing.Size(100, 20)
        Me.txtClass.TabIndex = 17
        Me.txtClass.Text = "MAINWNDMOAC | 䅍义乗䵄䅏C"
        '
        'FrmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(238, 198)
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
        Me.tbcSettings.ResumeLayout(False)
        Me.tabResolutions.ResumeLayout(False)
        Me.tabResolutions.PerformLayout()
        Me.tabShortcuts.ResumeLayout(False)
        Me.grpCycleShortcut.ResumeLayout(False)
        Me.grpCycleShortcut.PerformLayout()
        Me.grpOverviewShortcut.ResumeLayout(False)
        Me.grpOverviewShortcut.PerformLayout()
        Me.tabMisc.ResumeLayout(False)
        Me.tabMisc.PerformLayout()
        Me.grpAlign.ResumeLayout(False)
        Me.grpAlign.PerformLayout()
        CType(Me.numYoffset, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numXoffset, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents tmrAlign As Timer
    Friend WithEvents cmsQLFolder As ContextMenuStrip
    Friend WithEvents OpenInExplorerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ttSettings As ToolTip
    Friend WithEvents tbcSettings As TabControl
    Friend WithEvents tabResolutions As TabPage
    Friend WithEvents txtResolutions As TextBox
    Friend WithEvents tabShortcuts As TabPage
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
    Friend WithEvents chkOverViewIsGame As CheckBox
    Friend WithEvents chkCycleAlts As CheckBox
    Friend WithEvents chkCycleUpAlt As CheckBox
    Friend WithEvents chkCycleUpCtrl As CheckBox
    Friend WithEvents chkStoShift As CheckBox
    Friend WithEvents chkCycleDownShift As CheckBox
    Friend WithEvents chkCycleUpShift As CheckBox
    Friend WithEvents chkCycleDownAlt As CheckBox
    Friend WithEvents chkCycleDownCtrl As CheckBox
    Friend WithEvents txtCycleKeyDown As TextBox
End Class
