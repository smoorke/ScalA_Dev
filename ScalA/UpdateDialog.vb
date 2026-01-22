Public Class UpdateDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Try
            Process.Start(New ProcessStartInfo(FileIO.SpecialDirectories.Temp & "\ScalA\ChangeLog.txt"))
        Catch ex As Exception
            dBug.Print($"Failed to open changelog link: {ex.Message}")
        End Try
    End Sub

    Dim DesignedClientSize As Size
    Public Sub New()
        InitializeComponent()
        DesignedClientSize = Me.ClientSize
    End Sub
    Private Sub UpdateDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim rcC As RECT
        GetClientRect(Me.Handle, rcC)

        Me.Size = New Size(Me.Width - rcC.right + DesignedClientSize.Width,
                           Me.Height - rcC.bottom + DesignedClientSize.Height)

        Label2.Text = $"Would you like to update to v{FrmMain.updateToVersion}?"
    End Sub
    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case WM_WINDOWPOSCHANGING
                Dim winpos As WINDOWPOS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                If StructureToPtrSupported Then
                    If Not winpos.flags.HasFlag(SetWindowPosFlags.IgnoreResize) Then
                        Dim rcC As RECT
                        GetClientRect(Me.Handle, rcC)
                        winpos.cx = Me.Width - rcC.right + DesignedClientSize.Width
                        winpos.cy = Me.Height - rcC.bottom + DesignedClientSize.Height
                        System.Runtime.InteropServices.Marshal.StructureToPtr(winpos, m.LParam, True)
                    End If
                End If
        End Select

        MyBase.WndProc(m)
    End Sub
End Class
