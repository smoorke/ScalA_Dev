<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmHelp
    Inherits System.Windows.Forms.Form

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
        Me.splitContainer = New System.Windows.Forms.SplitContainer()
        Me.tvCategories = New System.Windows.Forms.TreeView()
        Me.rtbContent = New System.Windows.Forms.RichTextBox()
        Me.pnlBottom = New System.Windows.Forms.Panel()
        Me.btnClose = New System.Windows.Forms.Button()
        CType(Me.splitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainer.Panel1.SuspendLayout()
        Me.splitContainer.Panel2.SuspendLayout()
        Me.splitContainer.SuspendLayout()
        Me.pnlBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'splitContainer
        '
        Me.splitContainer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.splitContainer.Location = New System.Drawing.Point(0, 0)
        Me.splitContainer.Name = "splitContainer"
        '
        'splitContainer.Panel1
        '
        Me.splitContainer.Panel1.Controls.Add(Me.tvCategories)
        Me.splitContainer.Panel1MinSize = 150
        '
        'splitContainer.Panel2
        '
        Me.splitContainer.Panel2.Controls.Add(Me.rtbContent)
        Me.splitContainer.Panel2MinSize = 300
        Me.splitContainer.Size = New System.Drawing.Size(684, 411)
        Me.splitContainer.SplitterDistance = 180
        Me.splitContainer.TabIndex = 0
        '
        'tvCategories
        '
        Me.tvCategories.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvCategories.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvCategories.FullRowSelect = True
        Me.tvCategories.HideSelection = False
        Me.tvCategories.ItemHeight = 24
        Me.tvCategories.Location = New System.Drawing.Point(0, 0)
        Me.tvCategories.Name = "tvCategories"
        Me.tvCategories.ShowLines = False
        Me.tvCategories.Size = New System.Drawing.Size(180, 411)
        Me.tvCategories.TabIndex = 0
        '
        'rtbContent
        '
        Me.rtbContent.BackColor = System.Drawing.SystemColors.Window
        Me.rtbContent.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbContent.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbContent.Location = New System.Drawing.Point(0, 0)
        Me.rtbContent.Name = "rtbContent"
        Me.rtbContent.ReadOnly = True
        Me.rtbContent.Size = New System.Drawing.Size(500, 411)
        Me.rtbContent.TabIndex = 0
        Me.rtbContent.Text = ""
        '
        'pnlBottom
        '
        Me.pnlBottom.Controls.Add(Me.btnClose)
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(0, 411)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(684, 40)
        Me.pnlBottom.TabIndex = 1
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(597, 8)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 25)
        Me.btnClose.TabIndex = 0
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmHelp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(684, 451)
        Me.Controls.Add(Me.splitContainer)
        Me.Controls.Add(Me.pnlBottom)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(500, 400)
        Me.Name = "frmHelp"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Help & FAQ"
        Me.splitContainer.Panel1.ResumeLayout(False)
        Me.splitContainer.Panel2.ResumeLayout(False)
        CType(Me.splitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainer.ResumeLayout(False)
        Me.pnlBottom.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents splitContainer As SplitContainer
    Friend WithEvents tvCategories As TreeView
    Friend WithEvents rtbContent As RichTextBox
    Friend WithEvents pnlBottom As Panel
    Friend WithEvents btnClose As Button

End Class
