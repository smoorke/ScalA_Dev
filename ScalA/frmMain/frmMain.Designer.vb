<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial NotInheritable Class FrmMain
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
        Dim ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Dim NoneSortSeperator1 As System.Windows.Forms.ToolStripSeparator
        Dim NoneSortSeperator2 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.pnlSys = New System.Windows.Forms.Panel()
        Me.btnStart = New ScalA.ThemedStartButton()
        Me.cmsQuickLaunch = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DummyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmbResolution = New ScalA.ThemedComboBox()
        Me.cboAlt = New ScalA.ThemedComboBox()
        Me.pnlTitleBar = New System.Windows.Forms.Panel()
        Me.chkDebug = New System.Windows.Forms.CheckBox()
        Me.pbDpiWarning = New System.Windows.Forms.PictureBox()
        Me.pnlUpdate = New System.Windows.Forms.Panel()
        Me.pbUpdateAvailable = New System.Windows.Forms.PictureBox()
        Me.ChkEqLock = New System.Windows.Forms.CheckBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.pnlButtons = New System.Windows.Forms.Panel()
        Me.btnMin = New System.Windows.Forms.Button()
        Me.btnMax = New System.Windows.Forms.Button()
        Me.btnQuit = New System.Windows.Forms.Button()
        Me.cmsQuit = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CloseScalAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAstoniaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllIdleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseBothToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.CloseAllOverviewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllExceptToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tmrTick = New System.Windows.Forms.Timer(Me.components)
        Me.tmrOverview = New System.Windows.Forms.Timer(Me.components)
        Me.tmrActive = New System.Windows.Forms.Timer(Me.components)
        Me.cmsAlt = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SelectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReLaunchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SortSubToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TopFirstToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TopLastToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NoneSortToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BotFirstToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BotLastToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MoveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NoOtherOverviewsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.KeepToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TopMostToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ActiveOverviewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.SidebarModeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NoOtherScalAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllButNameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlOverview = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlMessage = New System.Windows.Forms.Panel()
        Me.chkHideMessage = New System.Windows.Forms.CheckBox()
        Me.pbMessage = New System.Windows.Forms.PictureBox()
        Me.pbZoom = New System.Windows.Forms.PictureBox()
        Me.sysTrayIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.cornerNW = New System.Windows.Forms.PictureBox()
        Me.cornerNE = New System.Windows.Forms.PictureBox()
        Me.cornerSW = New System.Windows.Forms.PictureBox()
        Me.cornerSE = New System.Windows.Forms.PictureBox()
        Me.PnlEqLock = New System.Windows.Forms.Panel()
        Me.ttMain = New System.Windows.Forms.ToolTip(Me.components)
        Me.tmrHotkeys = New System.Windows.Forms.Timer(Me.components)
        ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        NoneSortSeperator1 = New System.Windows.Forms.ToolStripSeparator()
        NoneSortSeperator2 = New System.Windows.Forms.ToolStripSeparator()
        ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.pnlSys.SuspendLayout()
        Me.cmsQuickLaunch.SuspendLayout()
        Me.pnlTitleBar.SuspendLayout()
        CType(Me.pbDpiWarning, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlUpdate.SuspendLayout()
        CType(Me.pbUpdateAvailable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlButtons.SuspendLayout()
        Me.cmsQuit.SuspendLayout()
        Me.cmsAlt.SuspendLayout()
        Me.pnlOverview.SuspendLayout()
        Me.pnlMessage.SuspendLayout()
        CType(Me.pbMessage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbZoom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cornerNW, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cornerNE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cornerSW, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cornerSE, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStripSeparator1
        '
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New System.Drawing.Size(173, 6)
        '
        'NoneSortSeperator1
        '
        NoneSortSeperator1.Name = "NoneSortSeperator1"
        NoneSortSeperator1.Size = New System.Drawing.Size(116, 6)
        '
        'NoneSortSeperator2
        '
        NoneSortSeperator2.Name = "NoneSortSeperator2"
        NoneSortSeperator2.Size = New System.Drawing.Size(116, 6)
        '
        'ToolStripSeparator2
        '
        ToolStripSeparator2.Name = "ToolStripSeparator2"
        ToolStripSeparator2.Size = New System.Drawing.Size(173, 6)
        '
        'ToolStripMenuItem1
        '
        ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        ToolStripMenuItem1.Size = New System.Drawing.Size(173, 6)
        '
        'pnlSys
        '
        Me.pnlSys.BackColor = System.Drawing.SystemColors.Control
        Me.pnlSys.Controls.Add(Me.btnStart)
        Me.pnlSys.Controls.Add(Me.cmbResolution)
        Me.pnlSys.Controls.Add(Me.cboAlt)
        Me.pnlSys.Location = New System.Drawing.Point(0, 0)
        Me.pnlSys.Name = "pnlSys"
        Me.pnlSys.Size = New System.Drawing.Size(270, 25)
        Me.pnlSys.TabIndex = 13
        '
        'btnStart
        '
        Me.btnStart.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnStart.Font = New System.Drawing.Font("Courier New", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStart.Location = New System.Drawing.Point(2, 2)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Padding = New System.Windows.Forms.Padding(5)
        Me.btnStart.Size = New System.Drawing.Size(21, 21)
        Me.btnStart.TabIndex = 15
        Me.ttMain.SetToolTip(Me.btnStart, "Switch to Overview" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "RMB: QuickLaunch")
        '
        'cmsQuickLaunch
        '
        Me.cmsQuickLaunch.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DummyToolStripMenuItem})
        Me.cmsQuickLaunch.Name = "cmsQuickLaunch"
        Me.cmsQuickLaunch.Size = New System.Drawing.Size(138, 26)
        '
        'DummyToolStripMenuItem
        '
        Me.DummyToolStripMenuItem.Name = "DummyToolStripMenuItem"
        Me.DummyToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.DummyToolStripMenuItem.Text = "--Dummy--"
        '
        'cmbResolution
        '
        Me.cmbResolution.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbResolution.DropDownHeight = 4096
        Me.cmbResolution.FormattingEnabled = True
        Me.cmbResolution.IntegralHeight = False
        Me.cmbResolution.Location = New System.Drawing.Point(187, 2)
        Me.cmbResolution.Name = "cmbResolution"
        Me.cmbResolution.Size = New System.Drawing.Size(80, 21)
        Me.cmbResolution.TabIndex = 17
        Me.ttMain.SetToolTip(Me.cmbResolution, "Change Resolution" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "RMB: Settings")
        '
        'cboAlt
        '
        Me.cboAlt.ContextMenuStrip = Me.cmsQuickLaunch
        Me.cboAlt.DisplayMember = "DisplayName"
        Me.cboAlt.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cboAlt.DropDownHeight = 4096
        Me.cboAlt.FormattingEnabled = True
        Me.cboAlt.IntegralHeight = False
        Me.cboAlt.Location = New System.Drawing.Point(25, 2)
        Me.cboAlt.Name = "cboAlt"
        Me.cboAlt.Size = New System.Drawing.Size(160, 21)
        Me.cboAlt.TabIndex = 16
        Me.ttMain.SetToolTip(Me.cboAlt, "Select Astonia Client" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "RMB: QuickLaunch")
        Me.cboAlt.ValueMember = "value"
        '
        'pnlTitleBar
        '
        Me.pnlTitleBar.BackColor = System.Drawing.SystemColors.Control
        Me.pnlTitleBar.Controls.Add(Me.chkDebug)
        Me.pnlTitleBar.Controls.Add(Me.pbDpiWarning)
        Me.pnlTitleBar.Controls.Add(Me.pnlUpdate)
        Me.pnlTitleBar.Controls.Add(Me.ChkEqLock)
        Me.pnlTitleBar.Controls.Add(Me.lblTitle)
        Me.pnlTitleBar.Location = New System.Drawing.Point(270, 0)
        Me.pnlTitleBar.Name = "pnlTitleBar"
        Me.pnlTitleBar.Size = New System.Drawing.Size(641, 25)
        Me.pnlTitleBar.TabIndex = 14
        '
        'chkDebug
        '
        Me.chkDebug.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkDebug.AutoSize = True
        Me.chkDebug.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDebug.Location = New System.Drawing.Point(508, 6)
        Me.chkDebug.Name = "chkDebug"
        Me.chkDebug.Size = New System.Drawing.Size(15, 14)
        Me.chkDebug.TabIndex = 11
        Me.chkDebug.TabStop = False
        Me.chkDebug.UseVisualStyleBackColor = True
        Me.chkDebug.Visible = False
        '
        'pbDpiWarning
        '
        Me.pbDpiWarning.BackgroundImage = Global.ScalA.My.Resources.Resources.Warning
        Me.pbDpiWarning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.pbDpiWarning.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbDpiWarning.Location = New System.Drawing.Point(100, 4)
        Me.pbDpiWarning.Margin = New System.Windows.Forms.Padding(0)
        Me.pbDpiWarning.Name = "pbDpiWarning"
        Me.pbDpiWarning.Size = New System.Drawing.Size(18, 18)
        Me.pbDpiWarning.TabIndex = 15
        Me.pbDpiWarning.TabStop = False
        Me.ttMain.SetToolTip(Me.pbDpiWarning, "DPI Override not enabled" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Click to enable DPI Override for this client")
        Me.pbDpiWarning.Visible = False
        '
        'pnlUpdate
        '
        Me.pnlUpdate.Controls.Add(Me.pbUpdateAvailable)
        Me.pnlUpdate.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlUpdate.Location = New System.Drawing.Point(580, 0)
        Me.pnlUpdate.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlUpdate.Name = "pnlUpdate"
        Me.pnlUpdate.Size = New System.Drawing.Size(24, 25)
        Me.pnlUpdate.TabIndex = 14
        Me.pnlUpdate.Visible = False
        '
        'pbUpdateAvailable
        '
        Me.pbUpdateAvailable.BackgroundImage = Global.ScalA.My.Resources.Resources.About
        Me.pbUpdateAvailable.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.pbUpdateAvailable.Location = New System.Drawing.Point(3, 6)
        Me.pbUpdateAvailable.Margin = New System.Windows.Forms.Padding(0)
        Me.pbUpdateAvailable.Name = "pbUpdateAvailable"
        Me.pbUpdateAvailable.Size = New System.Drawing.Size(20, 19)
        Me.pbUpdateAvailable.TabIndex = 14
        Me.pbUpdateAvailable.TabStop = False
        Me.ttMain.SetToolTip(Me.pbUpdateAvailable, "An Update is Available")
        '
        'ChkEqLock
        '
        Me.ChkEqLock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ChkEqLock.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ChkEqLock.Checked = Global.ScalA.My.MySettings.Default.LockEq
        Me.ChkEqLock.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkEqLock.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.ScalA.My.MySettings.Default, "LockEq", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.ChkEqLock.Dock = System.Windows.Forms.DockStyle.Right
        Me.ChkEqLock.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkEqLock.Location = New System.Drawing.Point(604, 0)
        Me.ChkEqLock.Name = "ChkEqLock"
        Me.ChkEqLock.Padding = New System.Windows.Forms.Padding(0, 3, 3, 0)
        Me.ChkEqLock.Size = New System.Drawing.Size(37, 25)
        Me.ChkEqLock.TabIndex = 12
        Me.ChkEqLock.Text = "🔒"
        Me.ttMain.SetToolTip(Me.ChkEqLock, "EQ Lock" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Disable Left Click on worn gear" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Hold Alt or Shift-Key to override")
        Me.ChkEqLock.UseVisualStyleBackColor = True
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(-2, 5)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(78, 15)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Text = "- ScalA beta -"
        '
        'pnlButtons
        '
        Me.pnlButtons.BackColor = System.Drawing.SystemColors.Control
        Me.pnlButtons.Controls.Add(Me.btnMin)
        Me.pnlButtons.Controls.Add(Me.btnMax)
        Me.pnlButtons.Controls.Add(Me.btnQuit)
        Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlButtons.Location = New System.Drawing.Point(917, 0)
        Me.pnlButtons.MaximumSize = New System.Drawing.Size(135, 23)
        Me.pnlButtons.MinimumSize = New System.Drawing.Size(135, 23)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Size = New System.Drawing.Size(135, 23)
        Me.pnlButtons.TabIndex = 7
        '
        'btnMin
        '
        Me.btnMin.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnMin.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control
        Me.btnMin.FlatAppearance.BorderSize = 0
        Me.btnMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMin.Location = New System.Drawing.Point(0, 0)
        Me.btnMin.Name = "btnMin"
        Me.btnMin.Size = New System.Drawing.Size(45, 23)
        Me.btnMin.TabIndex = 8
        Me.btnMin.TabStop = False
        Me.btnMin.Text = "⎯"
        Me.ttMain.SetToolTip(Me.btnMin, "Minimize")
        Me.btnMin.UseVisualStyleBackColor = True
        '
        'btnMax
        '
        Me.btnMax.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnMax.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control
        Me.btnMax.FlatAppearance.BorderSize = 0
        Me.btnMax.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMax.Location = New System.Drawing.Point(45, 0)
        Me.btnMax.Name = "btnMax"
        Me.btnMax.Size = New System.Drawing.Size(45, 23)
        Me.btnMax.TabIndex = 9
        Me.btnMax.TabStop = False
        Me.btnMax.Text = "⧠"
        Me.ttMain.SetToolTip(Me.btnMax, "Maximize")
        Me.btnMax.UseVisualStyleBackColor = True
        '
        'btnQuit
        '
        Me.btnQuit.ContextMenuStrip = Me.cmsQuit
        Me.btnQuit.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnQuit.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control
        Me.btnQuit.FlatAppearance.BorderSize = 0
        Me.btnQuit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(102, Byte), Integer))
        Me.btnQuit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red
        Me.btnQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnQuit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnQuit.Location = New System.Drawing.Point(90, 0)
        Me.btnQuit.Name = "btnQuit"
        Me.btnQuit.Size = New System.Drawing.Size(45, 23)
        Me.btnQuit.TabIndex = 7
        Me.btnQuit.TabStop = False
        Me.btnQuit.Text = "╳"
        Me.ttMain.SetToolTip(Me.btnQuit, "Close ScalA")
        Me.btnQuit.UseVisualStyleBackColor = True
        '
        'cmsQuit
        '
        Me.cmsQuit.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CloseScalAToolStripMenuItem, Me.CloseAstoniaToolStripMenuItem, Me.CloseAllIdleToolStripMenuItem, Me.CloseBothToolStripMenuItem, Me.CloseAllSeparator, Me.CloseAllOverviewToolStripMenuItem, Me.CloseAllExceptToolStripMenuItem, Me.CloseAllToolStripMenuItem})
        Me.cmsQuit.Name = "cmsQuit"
        Me.cmsQuit.Size = New System.Drawing.Size(190, 164)
        '
        'CloseScalAToolStripMenuItem
        '
        Me.CloseScalAToolStripMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CloseScalAToolStripMenuItem.Image = Global.ScalA.My.Resources.Resources.Close
        Me.CloseScalAToolStripMenuItem.Name = "CloseScalAToolStripMenuItem"
        Me.CloseScalAToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.CloseScalAToolStripMenuItem.Text = "Close ScalA"
        '
        'CloseAstoniaToolStripMenuItem
        '
        Me.CloseAstoniaToolStripMenuItem.Image = Global.ScalA.My.Resources.Resources.F12
        Me.CloseAstoniaToolStripMenuItem.Name = "CloseAstoniaToolStripMenuItem"
        Me.CloseAstoniaToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.CloseAstoniaToolStripMenuItem.Text = "Close Astonia"
        '
        'CloseAllIdleToolStripMenuItem
        '
        Me.CloseAllIdleToolStripMenuItem.Image = Global.ScalA.My.Resources.Resources.moreF12
        Me.CloseAllIdleToolStripMenuItem.Name = "CloseAllIdleToolStripMenuItem"
        Me.CloseAllIdleToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.CloseAllIdleToolStripMenuItem.Text = "Close All Someone"
        '
        'CloseBothToolStripMenuItem
        '
        Me.CloseBothToolStripMenuItem.Image = Global.ScalA.My.Resources.Resources.CloseBoth
        Me.CloseBothToolStripMenuItem.Name = "CloseBothToolStripMenuItem"
        Me.CloseBothToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.CloseBothToolStripMenuItem.Text = "Close Name && Scala"
        '
        'CloseAllSeparator
        '
        Me.CloseAllSeparator.Name = "CloseAllSeparator"
        Me.CloseAllSeparator.Size = New System.Drawing.Size(186, 6)
        '
        'CloseAllOverviewToolStripMenuItem
        '
        Me.CloseAllOverviewToolStripMenuItem.Image = Global.ScalA.My.Resources.Resources.moreF12
        Me.CloseAllOverviewToolStripMenuItem.Name = "CloseAllOverviewToolStripMenuItem"
        Me.CloseAllOverviewToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.CloseAllOverviewToolStripMenuItem.Text = "Close All on Overview"
        '
        'CloseAllExceptToolStripMenuItem
        '
        Me.CloseAllExceptToolStripMenuItem.Image = Global.ScalA.My.Resources.Resources.moreF12
        Me.CloseAllExceptToolStripMenuItem.Name = "CloseAllExceptToolStripMenuItem"
        Me.CloseAllExceptToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.CloseAllExceptToolStripMenuItem.Text = "Close All Except"
        Me.CloseAllExceptToolStripMenuItem.Visible = False
        '
        'CloseAllToolStripMenuItem
        '
        Me.CloseAllToolStripMenuItem.Image = Global.ScalA.My.Resources.Resources.CloseAll
        Me.CloseAllToolStripMenuItem.Name = "CloseAllToolStripMenuItem"
        Me.CloseAllToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.CloseAllToolStripMenuItem.Text = "Close All && ScalA"
        '
        'tmrTick
        '
        Me.tmrTick.Interval = 1
        '
        'tmrOverview
        '
        Me.tmrOverview.Enabled = True
        Me.tmrOverview.Interval = 33
        '
        'tmrActive
        '
        Me.tmrActive.Enabled = True
        Me.tmrActive.Interval = 33
        '
        'cmsAlt
        '
        Me.cmsAlt.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectToolStripMenuItem, Me.ReLaunchToolStripMenuItem, ToolStripSeparator1, Me.SortSubToolStripMenuItem, Me.MoveToolStripMenuItem, Me.TopMostToolStripMenuItem, ToolStripSeparator2, Me.ActiveOverviewToolStripMenuItem, Me.ToolStripMenuItem2, Me.SidebarModeToolStripMenuItem, ToolStripMenuItem1, Me.CloseAllButNameToolStripMenuItem, Me.CloseToolStripMenuItem})
        Me.cmsAlt.Name = "cmsAlt"
        Me.cmsAlt.ShowItemToolTips = False
        Me.cmsAlt.Size = New System.Drawing.Size(177, 226)
        '
        'SelectToolStripMenuItem
        '
        Me.SelectToolStripMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.SelectToolStripMenuItem.Name = "SelectToolStripMenuItem"
        Me.SelectToolStripMenuItem.ShortcutKeyDisplayString = ""
        Me.SelectToolStripMenuItem.Size = New System.Drawing.Size(176, 22)
        Me.SelectToolStripMenuItem.Text = "Select"
        '
        'ReLaunchToolStripMenuItem
        '
        Me.ReLaunchToolStripMenuItem.Image = Global.ScalA.My.Resources.Resources.Refresh
        Me.ReLaunchToolStripMenuItem.Name = "ReLaunchToolStripMenuItem"
        Me.ReLaunchToolStripMenuItem.Size = New System.Drawing.Size(176, 22)
        Me.ReLaunchToolStripMenuItem.Text = "ReLaunch"
        '
        'SortSubToolStripMenuItem
        '
        Me.SortSubToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TopFirstToolStripMenuItem, Me.TopLastToolStripMenuItem, NoneSortSeperator1, Me.NoneSortToolStripMenuItem, NoneSortSeperator2, Me.BotFirstToolStripMenuItem, Me.BotLastToolStripMenuItem})
        Me.SortSubToolStripMenuItem.Name = "SortSubToolStripMenuItem"
        Me.SortSubToolStripMenuItem.Size = New System.Drawing.Size(176, 22)
        Me.SortSubToolStripMenuItem.Text = "Sort"
        '
        'TopFirstToolStripMenuItem
        '
        Me.TopFirstToolStripMenuItem.Name = "TopFirstToolStripMenuItem"
        Me.TopFirstToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.TopFirstToolStripMenuItem.Tag = "-2"
        Me.TopFirstToolStripMenuItem.Text = "Top First"
        '
        'TopLastToolStripMenuItem
        '
        Me.TopLastToolStripMenuItem.Name = "TopLastToolStripMenuItem"
        Me.TopLastToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.TopLastToolStripMenuItem.Tag = "-1"
        Me.TopLastToolStripMenuItem.Text = "Top Last"
        '
        'NoneSortToolStripMenuItem
        '
        Me.NoneSortToolStripMenuItem.Name = "NoneSortToolStripMenuItem"
        Me.NoneSortToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.NoneSortToolStripMenuItem.Tag = "0"
        Me.NoneSortToolStripMenuItem.Text = "None"
        '
        'BotFirstToolStripMenuItem
        '
        Me.BotFirstToolStripMenuItem.Name = "BotFirstToolStripMenuItem"
        Me.BotFirstToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.BotFirstToolStripMenuItem.Tag = "1"
        Me.BotFirstToolStripMenuItem.Text = "Bot First"
        '
        'BotLastToolStripMenuItem
        '
        Me.BotLastToolStripMenuItem.Name = "BotLastToolStripMenuItem"
        Me.BotLastToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.BotLastToolStripMenuItem.Tag = "2"
        Me.BotLastToolStripMenuItem.Text = "Bot Last"
        '
        'MoveToolStripMenuItem
        '
        Me.MoveToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NoOtherOverviewsToolStripMenuItem, Me.KeepToolStripMenuItem})
        Me.MoveToolStripMenuItem.Name = "MoveToolStripMenuItem"
        Me.MoveToolStripMenuItem.Size = New System.Drawing.Size(176, 22)
        Me.MoveToolStripMenuItem.Text = "Move To"
        '
        'NoOtherOverviewsToolStripMenuItem
        '
        Me.NoOtherOverviewsToolStripMenuItem.Enabled = False
        Me.NoOtherOverviewsToolStripMenuItem.Name = "NoOtherOverviewsToolStripMenuItem"
        Me.NoOtherOverviewsToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.NoOtherOverviewsToolStripMenuItem.Text = "(No Other Overviews)"
        '
        'KeepToolStripMenuItem
        '
        Me.KeepToolStripMenuItem.Name = "KeepToolStripMenuItem"
        Me.KeepToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.KeepToolStripMenuItem.Text = "Keep Here?"
        '
        'TopMostToolStripMenuItem
        '
        Me.TopMostToolStripMenuItem.Name = "TopMostToolStripMenuItem"
        Me.TopMostToolStripMenuItem.Size = New System.Drawing.Size(176, 22)
        Me.TopMostToolStripMenuItem.Text = "TopMost"
        '
        'ActiveOverviewToolStripMenuItem
        '
        Me.ActiveOverviewToolStripMenuItem.CheckOnClick = True
        Me.ActiveOverviewToolStripMenuItem.Name = "ActiveOverviewToolStripMenuItem"
        Me.ActiveOverviewToolStripMenuItem.Size = New System.Drawing.Size(176, 22)
        Me.ActiveOverviewToolStripMenuItem.Text = "Active Overview"
        Me.ActiveOverviewToolStripMenuItem.ToolTipText = "Enable this to make game respond to clicks on thumbnails." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Note: Active Overview " &
    "will forever be Beta, Results may vary." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(173, 6)
        '
        'SidebarModeToolStripMenuItem
        '
        Me.SidebarModeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NoOtherScalAsToolStripMenuItem})
        Me.SidebarModeToolStripMenuItem.Name = "SidebarModeToolStripMenuItem"
        Me.SidebarModeToolStripMenuItem.Size = New System.Drawing.Size(176, 22)
        Me.SidebarModeToolStripMenuItem.Text = "Sidebar Mode"
        Me.SidebarModeToolStripMenuItem.ToolTipText = resources.GetString("SidebarModeToolStripMenuItem.ToolTipText")
        '
        'NoOtherScalAsToolStripMenuItem
        '
        Me.NoOtherScalAsToolStripMenuItem.Enabled = False
        Me.NoOtherScalAsToolStripMenuItem.Name = "NoOtherScalAsToolStripMenuItem"
        Me.NoOtherScalAsToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.NoOtherScalAsToolStripMenuItem.Text = "(No Other ScalAs)"
        '
        'CloseAllButNameToolStripMenuItem
        '
        Me.CloseAllButNameToolStripMenuItem.Image = Global.ScalA.My.Resources.Resources.moreF12
        Me.CloseAllButNameToolStripMenuItem.Name = "CloseAllButNameToolStripMenuItem"
        Me.CloseAllButNameToolStripMenuItem.Size = New System.Drawing.Size(176, 22)
        Me.CloseAllButNameToolStripMenuItem.Text = "Close All but Name"
        Me.CloseAllButNameToolStripMenuItem.Visible = False
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Image = Global.ScalA.My.Resources.Resources.F12
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.ShortcutKeyDisplayString = ""
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(176, 22)
        Me.CloseToolStripMenuItem.Text = "Close This"
        '
        'pnlOverview
        '
        Me.pnlOverview.BackColor = System.Drawing.SystemColors.Control
        Me.pnlOverview.Controls.Add(Me.pnlMessage)
        Me.pnlOverview.Location = New System.Drawing.Point(1, 25)
        Me.pnlOverview.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlOverview.Name = "pnlOverview"
        Me.pnlOverview.Size = New System.Drawing.Size(898, 737)
        Me.pnlOverview.TabIndex = 10
        '
        'pnlMessage
        '
        Me.pnlMessage.Controls.Add(Me.chkHideMessage)
        Me.pnlMessage.Controls.Add(Me.pbMessage)
        Me.pnlMessage.Location = New System.Drawing.Point(0, 0)
        Me.pnlMessage.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlMessage.Name = "pnlMessage"
        Me.pnlMessage.Size = New System.Drawing.Size(200, 150)
        Me.pnlMessage.TabIndex = 13
        '
        'chkHideMessage
        '
        Me.chkHideMessage.AutoSize = True
        Me.chkHideMessage.Location = New System.Drawing.Point(129, 133)
        Me.chkHideMessage.Name = "chkHideMessage"
        Me.chkHideMessage.Size = New System.Drawing.Size(71, 17)
        Me.chkHideMessage.TabIndex = 11
        Me.chkHideMessage.Text = "Hide This"
        Me.chkHideMessage.UseVisualStyleBackColor = True
        '
        'pbMessage
        '
        Me.pbMessage.BackgroundImage = CType(resources.GetObject("pbMessage.BackgroundImage"), System.Drawing.Image)
        Me.pbMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pbMessage.Location = New System.Drawing.Point(3, 0)
        Me.pbMessage.Name = "pbMessage"
        Me.pbMessage.Size = New System.Drawing.Size(200, 150)
        Me.pbMessage.TabIndex = 12
        Me.pbMessage.TabStop = False
        '
        'pbZoom
        '
        Me.pbZoom.BackColor = System.Drawing.Color.Magenta
        Me.pbZoom.Location = New System.Drawing.Point(1, 25)
        Me.pbZoom.Name = "pbZoom"
        Me.pbZoom.Size = New System.Drawing.Size(800, 600)
        Me.pbZoom.TabIndex = 0
        Me.pbZoom.TabStop = False
        '
        'sysTrayIcon
        '
        Me.sysTrayIcon.ContextMenuStrip = Me.cmsQuickLaunch
        Me.sysTrayIcon.Icon = CType(resources.GetObject("sysTrayIcon.Icon"), System.Drawing.Icon)
        Me.sysTrayIcon.Text = "ScalA"
        '
        'cornerNW
        '
        Me.cornerNW.BackColor = System.Drawing.Color.Transparent
        Me.cornerNW.Image = Global.ScalA.My.Resources.Resources.cornerNW
        Me.cornerNW.Location = New System.Drawing.Point(907, 114)
        Me.cornerNW.Name = "cornerNW"
        Me.cornerNW.Size = New System.Drawing.Size(46, 41)
        Me.cornerNW.TabIndex = 15
        Me.cornerNW.TabStop = False
        '
        'cornerNE
        '
        Me.cornerNE.BackColor = System.Drawing.Color.Transparent
        Me.cornerNE.Image = Global.ScalA.My.Resources.Resources.cornerNE
        Me.cornerNE.Location = New System.Drawing.Point(959, 114)
        Me.cornerNE.Name = "cornerNE"
        Me.cornerNE.Size = New System.Drawing.Size(46, 41)
        Me.cornerNE.TabIndex = 16
        Me.cornerNE.TabStop = False
        '
        'cornerSW
        '
        Me.cornerSW.BackColor = System.Drawing.Color.Transparent
        Me.cornerSW.Image = Global.ScalA.My.Resources.Resources.cornerSW
        Me.cornerSW.Location = New System.Drawing.Point(907, 161)
        Me.cornerSW.Name = "cornerSW"
        Me.cornerSW.Size = New System.Drawing.Size(46, 41)
        Me.cornerSW.TabIndex = 17
        Me.cornerSW.TabStop = False
        '
        'cornerSE
        '
        Me.cornerSE.BackColor = System.Drawing.Color.Transparent
        Me.cornerSE.Image = Global.ScalA.My.Resources.Resources.cornerSE
        Me.cornerSE.Location = New System.Drawing.Point(959, 161)
        Me.cornerSE.Name = "cornerSE"
        Me.cornerSE.Size = New System.Drawing.Size(46, 41)
        Me.cornerSE.TabIndex = 18
        Me.cornerSE.TabStop = False
        '
        'PnlEqLock
        '
        Me.PnlEqLock.BackColor = System.Drawing.SystemColors.Control
        Me.PnlEqLock.Cursor = System.Windows.Forms.Cursors.No
        Me.PnlEqLock.Location = New System.Drawing.Point(782, 423)
        Me.PnlEqLock.Name = "PnlEqLock"
        Me.PnlEqLock.Size = New System.Drawing.Size(200, 54)
        Me.PnlEqLock.TabIndex = 22
        Me.PnlEqLock.Visible = False
        '
        'ttMain
        '
        Me.ttMain.ShowAlways = True
        '
        'tmrHotkeys
        '
        Me.tmrHotkeys.Enabled = True
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1052, 855)
        Me.Controls.Add(Me.PnlEqLock)
        Me.Controls.Add(Me.pnlOverview)
        Me.Controls.Add(Me.pbZoom)
        Me.Controls.Add(Me.cornerSE)
        Me.Controls.Add(Me.cornerSW)
        Me.Controls.Add(Me.cornerNE)
        Me.Controls.Add(Me.cornerNW)
        Me.Controls.Add(Me.pnlSys)
        Me.Controls.Add(Me.pnlButtons)
        Me.Controls.Add(Me.pnlTitleBar)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmMain"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ScalA"
        Me.TransparencyKey = System.Drawing.Color.Magenta
        Me.pnlSys.ResumeLayout(False)
        Me.cmsQuickLaunch.ResumeLayout(False)
        Me.pnlTitleBar.ResumeLayout(False)
        Me.pnlTitleBar.PerformLayout()
        CType(Me.pbDpiWarning, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlUpdate.ResumeLayout(False)
        CType(Me.pbUpdateAvailable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlButtons.ResumeLayout(False)
        Me.cmsQuit.ResumeLayout(False)
        Me.cmsAlt.ResumeLayout(False)
        Me.pnlOverview.ResumeLayout(False)
        Me.pnlMessage.ResumeLayout(False)
        Me.pnlMessage.PerformLayout()
        CType(Me.pbMessage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbZoom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cornerNW, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cornerNE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cornerSW, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cornerSE, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pbZoom As PictureBox
    Friend WithEvents tmrTick As Timer
    Friend WithEvents btnMin As Button
    Friend WithEvents btnQuit As Button
    Friend WithEvents btnMax As Button
    Friend WithEvents pnlButtons As Panel
    Friend WithEvents tmrOverview As Timer
    Friend WithEvents tmrActive As Timer
    Friend WithEvents SelectToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TopMostToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents pnlOverview As FlowLayoutPanel
    Friend WithEvents pnlMessage As Panel
    Friend WithEvents chkHideMessage As CheckBox
    Friend WithEvents pbMessage As PictureBox
    Friend WithEvents cmsQuickLaunch As ContextMenuStrip
    Friend WithEvents DummyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents pnlSys As Panel
    Friend WithEvents sysTrayIcon As NotifyIcon
    Friend WithEvents pnlTitleBar As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents chkDebug As CheckBox
    Friend WithEvents cornerNW As PictureBox
    Friend WithEvents cornerNE As PictureBox
    Friend WithEvents cornerSW As PictureBox
    Friend WithEvents cornerSE As PictureBox
    Friend WithEvents SortSubToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TopFirstToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TopLastToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BotFirstToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BotLastToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NoneSortToolStripMenuItem As ToolStripMenuItem
    Public WithEvents cmsAlt As ContextMenuStrip
    Friend WithEvents ChkEqLock As CheckBox
    Friend WithEvents PnlEqLock As Panel
    Friend WithEvents ttMain As ToolTip
    Public WithEvents cmbResolution As ThemedComboBox
    Friend WithEvents cboAlt As ThemedComboBox
    Friend WithEvents btnStart As ThemedStartButton
    Friend WithEvents pnlUpdate As Panel
    Friend WithEvents pbUpdateAvailable As PictureBox
    Friend WithEvents pbDpiWarning As PictureBox
    Friend WithEvents cmsQuit As ContextMenuStrip
    Friend WithEvents CloseScalAToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseAstoniaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseAllSeparator As ToolStripSeparator
    Friend WithEvents CloseAllToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseBothToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseAllIdleToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tmrHotkeys As Timer
    Friend WithEvents CloseAllOverviewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MoveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NoOtherOverviewsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents KeepToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SidebarModeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NoOtherScalAsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ReLaunchToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseAllExceptToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseAllButNameToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ActiveOverviewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
End Class
