Imports System.Runtime.InteropServices

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
        Catch
        End Try
    End Sub

    Private Sub UpdateDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label2.Text = $"Would you like to update to v{FrmMain.updateToVersion}?"
    End Sub

    Public Sub New()
        Me.AutoScaleMode = AutoScaleMode.None
        InitializeComponent()
        DesignedClientSize = Me.ClientSize
    End Sub
    Dim DesignedClientSize As Size


    Protected Overrides Sub WndProc(ByRef m As Message)

        Select Case m.Msg
            Case WM_GETDPISCALEDSIZE
                m.Result = 1
                Exit Sub
            Case WM_WINDOWPOSCHANGING
                Dim winpos As WINDOWPOS = Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                If StructureToPtrSupported Then
                    If Not startup AndAlso Not winpos.flags.HasFlag(SetWindowPosFlags.IgnoreResize) Then
                        Dim rcC As RECT
                        GetClientRect(Me.Handle, rcC)
                        'Debug.Print($"rcC {rcC.right} {rcC.bottom}")
                        winpos.cx = Me.Width - rcC.right + DesignedClientSize.Width
                        winpos.cy = Me.Height - rcC.bottom + DesignedClientSize.Height
                        Marshal.StructureToPtr(winpos, m.LParam, True)
                    End If
                End If
            Case WM_DPICHANGED
                Dim gti As GUITHREADINFO = New GUITHREADINFO With {.cbSize = Marshal.SizeOf(Of GUITHREADINFO)}
                GetGUIThreadInfo(GetWindowThreadProcessId(Me.Handle, Nothing), gti)
                If gti.flags <> 2 Then
                    'gti flags needed to prevent inf loop, note WM_winpos takes over then
                    'begininvoke required to get size after change
                    ' this is for when user changes scaling, not when user drags to other monitor
                    Me.BeginInvoke(Sub()
                                       Dim rcC As RECT
                                       GetClientRect(Me.Handle, rcC)
                                       Dim cX As Integer = Me.Width - rcC.right + DesignedClientSize.Width
                                       Dim cY As Integer = Me.Height - rcC.bottom + DesignedClientSize.Height
                                       SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, cX, cY, SetWindowPosFlags.IgnoreMove Or SetWindowPosFlags.IgnoreZOrder)
                                   End Sub)
                End If
                m.Result = 0
                Exit Sub
        End Select

        MyBase.WndProc(m)
    End Sub
    Dim startup As Boolean = True
    Private Sub UpdateDialog_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Dim rcC As RECT
        GetClientRect(Me.Handle, rcC)

        Me.Width = Me.Width - rcC.right + DesignedClientSize.Width
        Me.Height = Me.Height - rcC.bottom + DesignedClientSize.Height

        Me.startup = False
    End Sub
End Class
