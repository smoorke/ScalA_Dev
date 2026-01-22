''' <summary>
''' Dialog form for setting up launcher templates and creating shortcuts
''' </summary>
Public Class frmLauncherSetup
    Dim DesignedClientSize As Size
    Public Sub New()
        InitializeComponent()
        DesignedClientSize = Me.ClientSize
    End Sub
    Private Sub frmLauncherSetup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set target folder to QL root
        launcherSetupControl1.TargetFolder = My.Settings.links

        ' fix scaling issue
        Dim rcC As RECT
        GetClientRect(Me.Handle, rcC)

        Me.Size = New Size(Me.Width - rcC.right + DesignedClientSize.Width,
                           Me.Height - rcC.bottom + DesignedClientSize.Height)
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

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class
