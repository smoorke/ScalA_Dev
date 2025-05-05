#If DEBUG Then
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDebug
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
        Dim lblMinimum As System.Windows.Forms.Label
        Dim ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDebug))
        Me.txtDebugLog = New System.Windows.Forms.TextBox()
        Me.tmrDebug = New System.Windows.Forms.Timer(Me.components)
        Me.chkAutoScroll = New System.Windows.Forms.CheckBox()
        Me.ssDebug = New System.Windows.Forms.StatusStrip()
        Me.cmsSSDebug = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToggleDPIToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripStatusLabel0 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabe3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btnMonInfo = New System.Windows.Forms.Button()
        Me.chkPollDPI = New System.Windows.Forms.CheckBox()
        Me.btnAltInfo = New System.Windows.Forms.Button()
        Me.btnClearLog = New System.Windows.Forms.Button()
        Me.btnSaveLog = New System.Windows.Forms.Button()
        Me.tbLogLevel = New System.Windows.Forms.TrackBar()
        Me.lblLogLevel = New System.Windows.Forms.Label()
        Me.btnTestScreenManager = New System.Windows.Forms.Button()
        Me.chkForceShowUpdate = New System.Windows.Forms.CheckBox()
        Me.chkShowWarning = New System.Windows.Forms.CheckBox()
        Me.btnTest = New System.Windows.Forms.Button()
        Me.btnIpcInfo = New System.Windows.Forms.Button()
        lblMinimum = New System.Windows.Forms.Label()
        ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ssDebug.SuspendLayout()
        Me.cmsSSDebug.SuspendLayout()
        CType(Me.tbLogLevel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblMinimum
        '
        lblMinimum.AutoSize = True
        lblMinimum.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblMinimum.Location = New System.Drawing.Point(679, 323)
        lblMinimum.Name = "lblMinimum"
        lblMinimum.Size = New System.Drawing.Size(44, 12)
        lblMinimum.TabIndex = 10
        lblMinimum.Text = "Minimum"
        '
        'ToolStripMenuItem1
        '
        ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        ToolStripMenuItem1.Size = New System.Drawing.Size(163, 6)
        '
        'ToolStripMenuItem2
        '
        ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        ToolStripMenuItem2.Size = New System.Drawing.Size(163, 6)
        '
        'txtDebugLog
        '
        Me.txtDebugLog.Location = New System.Drawing.Point(0, 0)
        Me.txtDebugLog.Multiline = True
        Me.txtDebugLog.Name = "txtDebugLog"
        Me.txtDebugLog.ReadOnly = True
        Me.txtDebugLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtDebugLog.Size = New System.Drawing.Size(663, 396)
        Me.txtDebugLog.TabIndex = 0
        Me.txtDebugLog.WordWrap = False
        '
        'tmrDebug
        '
        Me.tmrDebug.Enabled = True
        Me.tmrDebug.Interval = 250
        '
        'chkAutoScroll
        '
        Me.chkAutoScroll.AutoSize = True
        Me.chkAutoScroll.Checked = True
        Me.chkAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAutoScroll.Location = New System.Drawing.Point(669, 379)
        Me.chkAutoScroll.Name = "chkAutoScroll"
        Me.chkAutoScroll.Size = New System.Drawing.Size(74, 17)
        Me.chkAutoScroll.TabIndex = 1
        Me.chkAutoScroll.Text = "AutoScroll"
        Me.chkAutoScroll.UseVisualStyleBackColor = True
        '
        'ssDebug
        '
        Me.ssDebug.ContextMenuStrip = Me.cmsSSDebug
        Me.ssDebug.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel0, Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel2, Me.ToolStripStatusLabe3})
        Me.ssDebug.Location = New System.Drawing.Point(0, 428)
        Me.ssDebug.Name = "ssDebug"
        Me.ssDebug.Size = New System.Drawing.Size(748, 22)
        Me.ssDebug.SizingGrip = False
        Me.ssDebug.TabIndex = 2
        Me.ssDebug.Text = "StatusStrip1"
        '
        'cmsSSDebug
        '
        Me.cmsSSDebug.Items.AddRange(New System.Windows.Forms.ToolStripItem() {ToolStripMenuItem1, Me.ToggleDPIToolStripMenuItem, ToolStripMenuItem2})
        Me.cmsSSDebug.Name = "cmsSSDebug"
        Me.cmsSSDebug.Size = New System.Drawing.Size(167, 38)
        '
        'ToggleDPIToolStripMenuItem
        '
        Me.ToggleDPIToolStripMenuItem.Name = "ToggleDPIToolStripMenuItem"
        Me.ToggleDPIToolStripMenuItem.Size = New System.Drawing.Size(166, 22)
        Me.ToggleDPIToolStripMenuItem.Text = "Toggle DPI Aware"
        '
        'ToolStripStatusLabel0
        '
        Me.ToolStripStatusLabel0.ForeColor = System.Drawing.SystemColors.Control
        Me.ToolStripStatusLabel0.Name = "ToolStripStatusLabel0"
        Me.ToolStripStatusLabel0.Size = New System.Drawing.Size(25, 17)
        Me.ToolStripStatusLabel0.Text = "DPI"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.ForeColor = System.Drawing.SystemColors.Control
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(27, 17)
        Me.ToolStripStatusLabel1.Text = "SDL"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.ForeColor = System.Drawing.SystemColors.Control
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(29, 17)
        Me.ToolStripStatusLabel2.Text = "AS+"
        '
        'ToolStripStatusLabe3
        '
        Me.ToolStripStatusLabe3.ForeColor = System.Drawing.SystemColors.Control
        Me.ToolStripStatusLabe3.Name = "ToolStripStatusLabe3"
        Me.ToolStripStatusLabe3.Size = New System.Drawing.Size(32, 17)
        Me.ToolStripStatusLabe3.Text = "WS+"
        '
        'btnMonInfo
        '
        Me.btnMonInfo.Location = New System.Drawing.Point(73, 402)
        Me.btnMonInfo.Name = "btnMonInfo"
        Me.btnMonInfo.Size = New System.Drawing.Size(75, 23)
        Me.btnMonInfo.TabIndex = 3
        Me.btnMonInfo.Text = "MonInfo"
        Me.btnMonInfo.UseVisualStyleBackColor = True
        '
        'chkPollDPI
        '
        Me.chkPollDPI.AutoSize = True
        Me.chkPollDPI.Location = New System.Drawing.Point(12, 406)
        Me.chkPollDPI.Name = "chkPollDPI"
        Me.chkPollDPI.Size = New System.Drawing.Size(61, 17)
        Me.chkPollDPI.TabIndex = 4
        Me.chkPollDPI.Text = "PollDPI"
        Me.chkPollDPI.UseVisualStyleBackColor = True
        '
        'btnAltInfo
        '
        Me.btnAltInfo.Location = New System.Drawing.Point(154, 402)
        Me.btnAltInfo.Name = "btnAltInfo"
        Me.btnAltInfo.Size = New System.Drawing.Size(75, 23)
        Me.btnAltInfo.TabIndex = 5
        Me.btnAltInfo.Text = "AltInfo"
        Me.btnAltInfo.UseVisualStyleBackColor = True
        '
        'btnClearLog
        '
        Me.btnClearLog.Location = New System.Drawing.Point(668, 12)
        Me.btnClearLog.Name = "btnClearLog"
        Me.btnClearLog.Size = New System.Drawing.Size(75, 23)
        Me.btnClearLog.TabIndex = 6
        Me.btnClearLog.Text = "Clear"
        Me.btnClearLog.UseVisualStyleBackColor = True
        '
        'btnSaveLog
        '
        Me.btnSaveLog.Location = New System.Drawing.Point(668, 47)
        Me.btnSaveLog.Name = "btnSaveLog"
        Me.btnSaveLog.Size = New System.Drawing.Size(75, 23)
        Me.btnSaveLog.TabIndex = 7
        Me.btnSaveLog.Text = "Save"
        Me.btnSaveLog.UseVisualStyleBackColor = True
        '
        'tbLogLevel
        '
        Me.tbLogLevel.Location = New System.Drawing.Point(669, 348)
        Me.tbLogLevel.Maximum = 1
        Me.tbLogLevel.Name = "tbLogLevel"
        Me.tbLogLevel.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.tbLogLevel.Size = New System.Drawing.Size(67, 45)
        Me.tbLogLevel.TabIndex = 8
        Me.tbLogLevel.Value = 1
        '
        'lblLogLevel
        '
        Me.lblLogLevel.AutoSize = True
        Me.lblLogLevel.Location = New System.Drawing.Point(673, 332)
        Me.lblLogLevel.Name = "lblLogLevel"
        Me.lblLogLevel.Size = New System.Drawing.Size(63, 13)
        Me.lblLogLevel.TabIndex = 9
        Me.lblLogLevel.Text = "Log Level 1"
        '
        'btnTestScreenManager
        '
        Me.btnTestScreenManager.Location = New System.Drawing.Point(235, 402)
        Me.btnTestScreenManager.Name = "btnTestScreenManager"
        Me.btnTestScreenManager.Size = New System.Drawing.Size(75, 23)
        Me.btnTestScreenManager.TabIndex = 11
        Me.btnTestScreenManager.Text = "ScreenManager"
        Me.btnTestScreenManager.UseVisualStyleBackColor = True
        '
        'chkForceShowUpdate
        '
        Me.chkForceShowUpdate.AutoSize = True
        Me.chkForceShowUpdate.Location = New System.Drawing.Point(611, 399)
        Me.chkForceShowUpdate.Name = "chkForceShowUpdate"
        Me.chkForceShowUpdate.Size = New System.Drawing.Size(125, 17)
        Me.chkForceShowUpdate.TabIndex = 12
        Me.chkForceShowUpdate.Text = "Show Update Button"
        Me.chkForceShowUpdate.UseVisualStyleBackColor = True
        '
        'chkShowWarning
        '
        Me.chkShowWarning.AutoSize = True
        Me.chkShowWarning.Location = New System.Drawing.Point(611, 413)
        Me.chkShowWarning.Name = "chkShowWarning"
        Me.chkShowWarning.Size = New System.Drawing.Size(123, 17)
        Me.chkShowWarning.TabIndex = 13
        Me.chkShowWarning.Text = "Show Warning Picto"
        Me.chkShowWarning.UseVisualStyleBackColor = True
        '
        'btnTest
        '
        Me.btnTest.Location = New System.Drawing.Point(441, 402)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(75, 23)
        Me.btnTest.TabIndex = 14
        Me.btnTest.Text = "Test"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'btnIpcInfo
        '
        Me.btnIpcInfo.Location = New System.Drawing.Point(316, 402)
        Me.btnIpcInfo.Name = "btnIpcInfo"
        Me.btnIpcInfo.Size = New System.Drawing.Size(75, 23)
        Me.btnIpcInfo.TabIndex = 15
        Me.btnIpcInfo.Text = "IPC Info"
        Me.btnIpcInfo.UseVisualStyleBackColor = True
        '
        'frmDebug
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(748, 450)
        Me.Controls.Add(Me.btnIpcInfo)
        Me.Controls.Add(Me.btnTest)
        Me.Controls.Add(Me.btnTestScreenManager)
        Me.Controls.Add(lblMinimum)
        Me.Controls.Add(Me.lblLogLevel)
        Me.Controls.Add(Me.btnSaveLog)
        Me.Controls.Add(Me.btnClearLog)
        Me.Controls.Add(Me.btnAltInfo)
        Me.Controls.Add(Me.btnMonInfo)
        Me.Controls.Add(Me.chkPollDPI)
        Me.Controls.Add(Me.ssDebug)
        Me.Controls.Add(Me.chkAutoScroll)
        Me.Controls.Add(Me.txtDebugLog)
        Me.Controls.Add(Me.tbLogLevel)
        Me.Controls.Add(Me.chkShowWarning)
        Me.Controls.Add(Me.chkForceShowUpdate)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmDebug"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "ScalA Debug Window"
        Me.ssDebug.ResumeLayout(False)
        Me.ssDebug.PerformLayout()
        Me.cmsSSDebug.ResumeLayout(False)
        CType(Me.tbLogLevel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtDebugLog As TextBox
    Friend WithEvents tmrDebug As Timer
    Friend WithEvents chkAutoScroll As CheckBox
    Friend WithEvents ssDebug As StatusStrip
    Friend WithEvents ToolStripStatusLabel0 As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabe3 As ToolStripStatusLabel
    Friend WithEvents btnMonInfo As Button
    Friend WithEvents chkPollDPI As CheckBox
    Friend WithEvents btnAltInfo As Button
    Friend WithEvents btnClearLog As Button
    Friend WithEvents btnSaveLog As Button
    Friend WithEvents tbLogLevel As TrackBar
    Friend WithEvents lblLogLevel As Label
    Friend WithEvents cmsSSDebug As ContextMenuStrip
    Friend WithEvents ToggleDPIToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents btnTestScreenManager As Button
    Friend WithEvents chkForceShowUpdate As CheckBox
    Friend WithEvents chkShowWarning As CheckBox
    Friend WithEvents btnTest As Button
    Friend WithEvents btnIpcInfo As Button
End Class
#End If
