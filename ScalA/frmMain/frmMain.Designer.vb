<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.pnlSys = New System.Windows.Forms.Panel()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.cmsQuickLaunch = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DummyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmbResolution = New System.Windows.Forms.ComboBox()
        Me.cboAlt = New System.Windows.Forms.ComboBox()
        Me.pnlTitleBar = New System.Windows.Forms.Panel()
        Me.chkDebug = New System.Windows.Forms.CheckBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.pnlButtons = New System.Windows.Forms.Panel()
        Me.btnMin = New System.Windows.Forms.Button()
        Me.btnMax = New System.Windows.Forms.Button()
        Me.btnQuit = New System.Windows.Forms.Button()
        Me.tmrTick = New System.Windows.Forms.Timer(Me.components)
        Me.tmrStartup = New System.Windows.Forms.Timer(Me.components)
        Me.tmrActive = New System.Windows.Forms.Timer(Me.components)
        Me.cmsAlt = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SelectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TopMostToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlStartup = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlMessage = New System.Windows.Forms.Panel()
        Me.chkHideMessage = New System.Windows.Forms.CheckBox()
        Me.pbMessage = New System.Windows.Forms.PictureBox()
        Me.btnAlt1 = New System.Windows.Forms.Button()
        Me.btnAlt2 = New System.Windows.Forms.Button()
        Me.btnAlt3 = New System.Windows.Forms.Button()
        Me.btnAlt4 = New System.Windows.Forms.Button()
        Me.btnAlt5 = New System.Windows.Forms.Button()
        Me.btnAlt6 = New System.Windows.Forms.Button()
        Me.btnAlt7 = New System.Windows.Forms.Button()
        Me.btnAlt8 = New System.Windows.Forms.Button()
        Me.btnAlt9 = New System.Windows.Forms.Button()
        Me.btnAlt10 = New System.Windows.Forms.Button()
        Me.btnAlt11 = New System.Windows.Forms.Button()
        Me.btnAlt12 = New System.Windows.Forms.Button()
        Me.btnAlt13 = New System.Windows.Forms.Button()
        Me.btnAlt14 = New System.Windows.Forms.Button()
        Me.btnAlt15 = New System.Windows.Forms.Button()
        Me.btnAlt16 = New System.Windows.Forms.Button()
        Me.pbZoom = New System.Windows.Forms.PictureBox()
        Me.sysTrayIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.cornerNW = New System.Windows.Forms.PictureBox()
        Me.cornerNE = New System.Windows.Forms.PictureBox()
        Me.cornerSW = New System.Windows.Forms.PictureBox()
        Me.cornerSE = New System.Windows.Forms.PictureBox()
        ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.pnlSys.SuspendLayout()
        Me.cmsQuickLaunch.SuspendLayout()
        Me.pnlTitleBar.SuspendLayout()
        Me.pnlButtons.SuspendLayout()
        Me.cmsAlt.SuspendLayout()
        Me.pnlStartup.SuspendLayout()
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
        ToolStripSeparator1.Size = New System.Drawing.Size(124, 6)
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
        Me.btnStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStart.Location = New System.Drawing.Point(1, 1)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(23, 23)
        Me.btnStart.TabIndex = 15
        Me.btnStart.Text = "⊞"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'cmsQuickLaunch
        '
        Me.cmsQuickLaunch.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DummyToolStripMenuItem})
        Me.cmsQuickLaunch.Name = "cmsBlank"
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
        Me.cmbResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbResolution.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbResolution.FormattingEnabled = True
        Me.cmbResolution.Location = New System.Drawing.Point(187, 2)
        Me.cmbResolution.Name = "cmbResolution"
        Me.cmbResolution.Size = New System.Drawing.Size(80, 21)
        Me.cmbResolution.TabIndex = 17
        '
        'cboAlt
        '
        Me.cboAlt.ContextMenuStrip = Me.cmsQuickLaunch
        Me.cboAlt.DisplayMember = "name"
        Me.cboAlt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAlt.FormattingEnabled = True
        Me.cboAlt.Location = New System.Drawing.Point(25, 2)
        Me.cboAlt.Name = "cboAlt"
        Me.cboAlt.Size = New System.Drawing.Size(160, 21)
        Me.cboAlt.TabIndex = 16
        Me.cboAlt.ValueMember = "value"
        '
        'pnlTitleBar
        '
        Me.pnlTitleBar.BackColor = System.Drawing.SystemColors.Control
        Me.pnlTitleBar.Controls.Add(Me.chkDebug)
        Me.pnlTitleBar.Controls.Add(Me.lblTitle)
        Me.pnlTitleBar.Location = New System.Drawing.Point(270, 0)
        Me.pnlTitleBar.Name = "pnlTitleBar"
        Me.pnlTitleBar.Size = New System.Drawing.Size(641, 25)
        Me.pnlTitleBar.TabIndex = 14
        '
        'chkDebug
        '
        Me.chkDebug.AutoSize = True
        Me.chkDebug.Location = New System.Drawing.Point(384, 6)
        Me.chkDebug.Name = "chkDebug"
        Me.chkDebug.Size = New System.Drawing.Size(15, 14)
        Me.chkDebug.TabIndex = 11
        Me.chkDebug.TabStop = False
        Me.chkDebug.UseVisualStyleBackColor = True
        Me.chkDebug.Visible = False
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(-2, 6)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(71, 13)
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
        Me.btnMax.UseVisualStyleBackColor = True
        '
        'btnQuit
        '
        Me.btnQuit.BackColor = System.Drawing.Color.Transparent
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
        Me.btnQuit.UseVisualStyleBackColor = False
        '
        'tmrTick
        '
        Me.tmrTick.Interval = 1
        '
        'tmrStartup
        '
        Me.tmrStartup.Enabled = True
        Me.tmrStartup.Interval = 255
        '
        'tmrActive
        '
        Me.tmrActive.Enabled = True
        Me.tmrActive.Interval = 99
        '
        'cmsAlt
        '
        Me.cmsAlt.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectToolStripMenuItem, Me.TopMostToolStripMenuItem, ToolStripSeparator1, Me.CloseToolStripMenuItem})
        Me.cmsAlt.Name = "cmsAlt"
        Me.cmsAlt.Size = New System.Drawing.Size(128, 76)
        '
        'SelectToolStripMenuItem
        '
        Me.SelectToolStripMenuItem.Name = "SelectToolStripMenuItem"
        Me.SelectToolStripMenuItem.ShortcutKeyDisplayString = ""
        Me.SelectToolStripMenuItem.Size = New System.Drawing.Size(127, 22)
        Me.SelectToolStripMenuItem.Text = "Select"
        '
        'TopMostToolStripMenuItem
        '
        Me.TopMostToolStripMenuItem.Name = "TopMostToolStripMenuItem"
        Me.TopMostToolStripMenuItem.Size = New System.Drawing.Size(127, 22)
        Me.TopMostToolStripMenuItem.Text = "TopMost"
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.ShortcutKeyDisplayString = ""
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(127, 22)
        Me.CloseToolStripMenuItem.Text = "Close This"
        '
        'pnlStartup
        '
        Me.pnlStartup.BackColor = System.Drawing.SystemColors.Control
        Me.pnlStartup.Controls.Add(Me.pnlMessage)
        Me.pnlStartup.Controls.Add(Me.btnAlt1)
        Me.pnlStartup.Controls.Add(Me.btnAlt2)
        Me.pnlStartup.Controls.Add(Me.btnAlt3)
        Me.pnlStartup.Controls.Add(Me.btnAlt4)
        Me.pnlStartup.Controls.Add(Me.btnAlt5)
        Me.pnlStartup.Controls.Add(Me.btnAlt6)
        Me.pnlStartup.Controls.Add(Me.btnAlt7)
        Me.pnlStartup.Controls.Add(Me.btnAlt8)
        Me.pnlStartup.Controls.Add(Me.btnAlt9)
        Me.pnlStartup.Controls.Add(Me.btnAlt10)
        Me.pnlStartup.Controls.Add(Me.btnAlt11)
        Me.pnlStartup.Controls.Add(Me.btnAlt12)
        Me.pnlStartup.Controls.Add(Me.btnAlt13)
        Me.pnlStartup.Controls.Add(Me.btnAlt14)
        Me.pnlStartup.Controls.Add(Me.btnAlt15)
        Me.pnlStartup.Controls.Add(Me.btnAlt16)
        Me.pnlStartup.Location = New System.Drawing.Point(1, 25)
        Me.pnlStartup.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlStartup.Name = "pnlStartup"
        Me.pnlStartup.Size = New System.Drawing.Size(898, 737)
        Me.pnlStartup.TabIndex = 10
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
        'btnAlt1
        '
        Me.btnAlt1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt1.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt1.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt1.Location = New System.Drawing.Point(200, 0)
        Me.btnAlt1.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt1.Name = "btnAlt1"
        Me.btnAlt1.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt1.TabIndex = 13
        Me.btnAlt1.Tag = ""
        Me.btnAlt1.Text = "1"
        Me.btnAlt1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt1.UseVisualStyleBackColor = True
        '
        'btnAlt2
        '
        Me.btnAlt2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt2.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt2.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt2.Location = New System.Drawing.Point(400, 0)
        Me.btnAlt2.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt2.Name = "btnAlt2"
        Me.btnAlt2.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt2.TabIndex = 14
        Me.btnAlt2.Tag = ""
        Me.btnAlt2.Text = "2"
        Me.btnAlt2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt2.UseVisualStyleBackColor = True
        '
        'btnAlt3
        '
        Me.btnAlt3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt3.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt3.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt3.Location = New System.Drawing.Point(600, 0)
        Me.btnAlt3.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt3.Name = "btnAlt3"
        Me.btnAlt3.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt3.TabIndex = 17
        Me.btnAlt3.Tag = ""
        Me.btnAlt3.Text = "3"
        Me.btnAlt3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt3.UseVisualStyleBackColor = True
        '
        'btnAlt4
        '
        Me.btnAlt4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt4.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt4.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt4.Location = New System.Drawing.Point(0, 150)
        Me.btnAlt4.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt4.Name = "btnAlt4"
        Me.btnAlt4.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt4.TabIndex = 21
        Me.btnAlt4.Tag = ""
        Me.btnAlt4.Text = "4"
        Me.btnAlt4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt4.UseVisualStyleBackColor = True
        '
        'btnAlt5
        '
        Me.btnAlt5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt5.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt5.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt5.Location = New System.Drawing.Point(200, 150)
        Me.btnAlt5.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt5.Name = "btnAlt5"
        Me.btnAlt5.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt5.TabIndex = 23
        Me.btnAlt5.Tag = ""
        Me.btnAlt5.Text = "5"
        Me.btnAlt5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt5.UseVisualStyleBackColor = True
        '
        'btnAlt6
        '
        Me.btnAlt6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt6.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt6.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt6.Location = New System.Drawing.Point(400, 150)
        Me.btnAlt6.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt6.Name = "btnAlt6"
        Me.btnAlt6.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt6.TabIndex = 25
        Me.btnAlt6.Tag = ""
        Me.btnAlt6.Text = "6"
        Me.btnAlt6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt6.UseVisualStyleBackColor = True
        '
        'btnAlt7
        '
        Me.btnAlt7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt7.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt7.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt7.Location = New System.Drawing.Point(600, 150)
        Me.btnAlt7.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt7.Name = "btnAlt7"
        Me.btnAlt7.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt7.TabIndex = 27
        Me.btnAlt7.Tag = ""
        Me.btnAlt7.Text = "7"
        Me.btnAlt7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt7.UseVisualStyleBackColor = True
        '
        'btnAlt8
        '
        Me.btnAlt8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt8.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt8.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt8.Location = New System.Drawing.Point(0, 300)
        Me.btnAlt8.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt8.Name = "btnAlt8"
        Me.btnAlt8.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt8.TabIndex = 28
        Me.btnAlt8.Tag = ""
        Me.btnAlt8.Text = "8"
        Me.btnAlt8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt8.UseVisualStyleBackColor = True
        '
        'btnAlt9
        '
        Me.btnAlt9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt9.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt9.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt9.Location = New System.Drawing.Point(200, 300)
        Me.btnAlt9.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt9.Name = "btnAlt9"
        Me.btnAlt9.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt9.TabIndex = 31
        Me.btnAlt9.Tag = ""
        Me.btnAlt9.Text = "9"
        Me.btnAlt9.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt9.UseVisualStyleBackColor = True
        '
        'btnAlt10
        '
        Me.btnAlt10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt10.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt10.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt10.Location = New System.Drawing.Point(400, 300)
        Me.btnAlt10.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt10.Name = "btnAlt10"
        Me.btnAlt10.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt10.TabIndex = 32
        Me.btnAlt10.Tag = ""
        Me.btnAlt10.Text = "10"
        Me.btnAlt10.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt10.UseVisualStyleBackColor = True
        '
        'btnAlt11
        '
        Me.btnAlt11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt11.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt11.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt11.Location = New System.Drawing.Point(600, 300)
        Me.btnAlt11.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt11.Name = "btnAlt11"
        Me.btnAlt11.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt11.TabIndex = 33
        Me.btnAlt11.Tag = ""
        Me.btnAlt11.Text = "11"
        Me.btnAlt11.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt11.UseVisualStyleBackColor = True
        '
        'btnAlt12
        '
        Me.btnAlt12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt12.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt12.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt12.Location = New System.Drawing.Point(0, 450)
        Me.btnAlt12.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt12.Name = "btnAlt12"
        Me.btnAlt12.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt12.TabIndex = 34
        Me.btnAlt12.Tag = ""
        Me.btnAlt12.Text = "12"
        Me.btnAlt12.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt12.UseVisualStyleBackColor = True
        '
        'btnAlt13
        '
        Me.btnAlt13.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt13.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt13.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt13.Location = New System.Drawing.Point(200, 450)
        Me.btnAlt13.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt13.Name = "btnAlt13"
        Me.btnAlt13.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt13.TabIndex = 35
        Me.btnAlt13.Tag = ""
        Me.btnAlt13.Text = "13"
        Me.btnAlt13.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt13.UseVisualStyleBackColor = True
        '
        'btnAlt14
        '
        Me.btnAlt14.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt14.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt14.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt14.Location = New System.Drawing.Point(400, 450)
        Me.btnAlt14.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt14.Name = "btnAlt14"
        Me.btnAlt14.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt14.TabIndex = 36
        Me.btnAlt14.Tag = ""
        Me.btnAlt14.Text = "14"
        Me.btnAlt14.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt14.UseVisualStyleBackColor = True
        '
        'btnAlt15
        '
        Me.btnAlt15.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt15.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt15.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt15.Location = New System.Drawing.Point(600, 450)
        Me.btnAlt15.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt15.Name = "btnAlt15"
        Me.btnAlt15.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt15.TabIndex = 37
        Me.btnAlt15.Tag = ""
        Me.btnAlt15.Text = "15"
        Me.btnAlt15.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt15.UseVisualStyleBackColor = True
        '
        'btnAlt16
        '
        Me.btnAlt16.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAlt16.ContextMenuStrip = Me.cmsQuickLaunch
        Me.btnAlt16.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnAlt16.Location = New System.Drawing.Point(0, 600)
        Me.btnAlt16.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAlt16.Name = "btnAlt16"
        Me.btnAlt16.Size = New System.Drawing.Size(200, 150)
        Me.btnAlt16.TabIndex = 38
        Me.btnAlt16.Tag = ""
        Me.btnAlt16.Text = "16"
        Me.btnAlt16.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAlt16.UseVisualStyleBackColor = True
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
        Me.sysTrayIcon.Visible = True
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
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1052, 855)
        Me.Controls.Add(Me.cornerSE)
        Me.Controls.Add(Me.cornerSW)
        Me.Controls.Add(Me.cornerNE)
        Me.Controls.Add(Me.cornerNW)
        Me.Controls.Add(Me.pnlStartup)
        Me.Controls.Add(Me.pbZoom)
        Me.Controls.Add(Me.pnlSys)
        Me.Controls.Add(Me.pnlButtons)
        Me.Controls.Add(Me.pnlTitleBar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmMain"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ScalA Beta"
        Me.TransparencyKey = System.Drawing.Color.Magenta
        Me.pnlSys.ResumeLayout(False)
        Me.cmsQuickLaunch.ResumeLayout(False)
        Me.pnlTitleBar.ResumeLayout(False)
        Me.pnlTitleBar.PerformLayout()
        Me.pnlButtons.ResumeLayout(False)
        Me.cmsAlt.ResumeLayout(False)
        Me.pnlStartup.ResumeLayout(False)
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
    Friend WithEvents tmrStartup As Timer
    Friend WithEvents tmrActive As Timer
    Friend WithEvents cmsAlt As ContextMenuStrip
    Friend WithEvents SelectToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TopMostToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents pnlStartup As FlowLayoutPanel
    Friend WithEvents pnlMessage As Panel
    Friend WithEvents chkHideMessage As CheckBox
    Friend WithEvents btnAlt1 As Button
    Friend WithEvents btnAlt2 As Button
    Friend WithEvents btnAlt3 As Button
    Friend WithEvents btnAlt4 As Button
    Friend WithEvents btnAlt5 As Button
    Friend WithEvents btnAlt6 As Button
    Friend WithEvents btnAlt7 As Button
    Friend WithEvents btnAlt8 As Button
    Friend WithEvents btnAlt9 As Button
    Friend WithEvents pbMessage As PictureBox
    Friend WithEvents cmsQuickLaunch As ContextMenuStrip
    Friend WithEvents DummyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents btnStart As Button
    Public WithEvents cmbResolution As ComboBox
    Friend WithEvents cboAlt As ComboBox
    Friend WithEvents pnlSys As Panel
    Friend WithEvents sysTrayIcon As NotifyIcon
    Friend WithEvents pnlTitleBar As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents chkDebug As CheckBox
    Friend WithEvents btnAlt10 As Button
    Friend WithEvents btnAlt11 As Button
    Friend WithEvents btnAlt12 As Button
    Friend WithEvents btnAlt13 As Button
    Friend WithEvents btnAlt14 As Button
    Friend WithEvents btnAlt15 As Button
    Friend WithEvents btnAlt16 As Button
    Friend WithEvents cornerNW As PictureBox
    Friend WithEvents cornerNE As PictureBox
    Friend WithEvents cornerSW As PictureBox
    Friend WithEvents cornerSE As PictureBox
End Class
