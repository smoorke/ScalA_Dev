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
        Me.tmrOverview = New System.Windows.Forms.Timer(Me.components)
        Me.tmrActive = New System.Windows.Forms.Timer(Me.components)
        Me.cmsAlt = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SelectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TopMostToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.tmrMove = New System.Windows.Forms.Timer(Me.components)
        ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.pnlSys.SuspendLayout()
        Me.cmsQuickLaunch.SuspendLayout()
        Me.pnlTitleBar.SuspendLayout()
        Me.pnlButtons.SuspendLayout()
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
        'tmrOverview
        '
        Me.tmrOverview.Enabled = True
        Me.tmrOverview.Interval = 33
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
        'tmrMove
        '
        Me.tmrMove.Interval = 25
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
        Me.Controls.Add(Me.pnlOverview)
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
    Friend WithEvents cmsAlt As ContextMenuStrip
    Friend WithEvents SelectToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TopMostToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents pnlOverview As FlowLayoutPanel
    Friend WithEvents pnlMessage As Panel
    Friend WithEvents chkHideMessage As CheckBox
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
    Friend WithEvents cornerNW As PictureBox
    Friend WithEvents cornerNE As PictureBox
    Friend WithEvents cornerSW As PictureBox
    Friend WithEvents cornerSE As PictureBox
    Friend WithEvents tmrMove As Timer
End Class
