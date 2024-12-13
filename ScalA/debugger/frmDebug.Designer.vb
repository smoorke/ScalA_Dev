<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDebug
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.txtDebugLog = New System.Windows.Forms.TextBox()
        Me.tmrDebug = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'txtDebugLog
        '
        Me.txtDebugLog.Location = New System.Drawing.Point(0, 0)
        Me.txtDebugLog.Multiline = True
        Me.txtDebugLog.Name = "txtDebugLog"
        Me.txtDebugLog.ReadOnly = True
        Me.txtDebugLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDebugLog.Size = New System.Drawing.Size(663, 396)
        Me.txtDebugLog.TabIndex = 0
        '
        'tmrDebug
        '
        Me.tmrDebug.Enabled = True
        Me.tmrDebug.Interval = 250
        '
        'frmDebug
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.txtDebugLog)
        Me.Name = "frmDebug"
        Me.Text = "ScalA Debug Window"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtDebugLog As TextBox
    Friend WithEvents tmrDebug As Timer
End Class
