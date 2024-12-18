Imports System.Runtime.InteropServices

Public Class frmDebug

    Private Sub frmDebug_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If logbuilder Is Nothing Then
            logbuilder = New System.Text.StringBuilder With {.Capacity = 100_000}
        End If
        dBug.Print("FrmDebug Load", 1)
    End Sub

    Private Sub tmrDebug_Tick(sender As Object, e As EventArgs) Handles tmrDebug.Tick
        If logbuilder IsNot Nothing Then
            If txtDebugLog.Text.Length <> logbuilder.Length Then
                If chkAutoScroll.Checked Then
                    txtDebugLog.Text = logbuilder.ToString
                    txtDebugLog.SelectionStart = txtDebugLog.TextLength
                    txtDebugLog.SelectionLength = 0
                    txtDebugLog.ScrollToCaret()
                End If
            End If
        Else
            dBug.Print("LogBuilder Nothing", 1)
        End If

        If chkPollDPI.Checked Then

            For Each item As ToolStripStatusLabel In ssDebug.Items.OfType(Of ToolStripStatusLabel)
                Dim bord As Boolean = False
                If FrmMain.AltPP IsNot Nothing AndAlso FrmMain.AltPP.Id <> 0 Then
                    Dim ap As AstoniaProcess = FrmMain.AltPP
                    Select Case item.Text
                        Case "DPI"
                            If ap.RegHighDpiAware Then
                                bord = True
                            End If
                        Case "MM+"
                            Screen.AllScreens.FirstOrDefault(Function(s) s.Primary).ScalingPercentTask() _
                              .ContinueWith(Sub(t As Task(Of Integer))
                                                If t.IsCompleted Then
                                                    If t.Result > 100 Then
                                                        bord = True
                                                    End If
                                                ElseIf t.IsFaulted Then
                                                    Debug.Print($"Error: {t.Exception?.Flatten()?.Message}")
                                                End If
                                            End Sub, TaskScheduler.FromCurrentSynchronizationContext())
                        Case "SM+"
                            Screen.AllScreens.FirstOrDefault(Function(s) Not s.Primary).ScalingPercentTask() _
                                          .ContinueWith(Sub(t As Task(Of Integer))
                                                            If t.IsCompleted Then
                                                                If t.Result > 100 Then
                                                                    bord = True
                                                                End If
                                                            ElseIf t.IsFaulted Then
                                                                Debug.Print($"Error: {t.Exception?.Flatten()?.Message}")
                                                            End If
                                                        End Sub, TaskScheduler.FromCurrentSynchronizationContext())
                        Case "SDL"
                            If ap.isSDL Then
                                bord = True
                            End If
                        Case "WS+"
                            If FrmMain.WindowsScaling > 100 Then
                                bord = True
                            End If
                        Case "AS+"
                            If ap.WindowsScaling > 100 Then
                                bord = True
                            End If
                        Case Else

                    End Select

                    If bord Then
                        item.BorderSides = ToolStripStatusLabelBorderSides.All
                        item.Font = New Font(item.Font.Name, 9, FontStyle.Bold)
                    Else
                        item.BorderSides = ToolStripStatusLabelBorderSides.None
                        item.Font = New Font(item.Font.Name, 9, FontStyle.Regular)
                    End If


                End If
            Next
        End If




    End Sub

    Public Const SB_VERT As Integer = 1
    Public Const SIF_RANGE As Integer = &H1
    Public Const SIF_POS As Integer = &H4
    Public Const SIF_TRACKPOS As Integer = &H10
    Public Const SIF_ALL As Integer = SIF_RANGE Or SIF_POS Or SIF_TRACKPOS

    <StructLayout(LayoutKind.Sequential)>
    Public Structure SCROLLINFO
        Public cbSize As UInteger
        Public fMask As UInteger
        Public nMin As Integer
        Public nMax As Integer
        Public nPage As UInteger
        Public nPos As Integer
        Public nTrackPos As Integer
    End Structure

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Shared Function GetScrollInfo(hWnd As IntPtr, nBar As Integer, ByRef si As SCROLLINFO) As Boolean : End Function

    Private Sub txtDebugLog_MouseWheel(sender As Object, e As MouseEventArgs) Handles txtDebugLog.MouseWheel
        If e.Delta > 0 Then ' Scrolling up disables auto-scroll
            chkAutoScroll.Checked = False
        Else
            Dim si As New SCROLLINFO With {.cbSize = Marshal.SizeOf(GetType(SCROLLINFO)), .fMask = SIF_ALL}

            If GetScrollInfo(txtDebugLog.Handle, SB_VERT, si) Then
                ' Calculate the visible lines and check if scrollbar is at the bottom
                Dim visibleLines As Integer = txtDebugLog.ClientSize.Height \ txtDebugLog.Font.Height
                Dim isAtBottom As Boolean = (si.nPos + visibleLines) > si.nMax

                ' Update auto-scroll based on scrollbar position
                chkAutoScroll.Checked = isAtBottom
            End If
        End If
    End Sub

    Private Sub btnMonInfo_Click(sender As Object, e As EventArgs) Handles btnMonInfo.Click
        For Each scrn As Screen In Screen.AllScreens.OrderByDescending(Function(s) s.Primary)
            dBug.Print($"{scrn.DeviceName} {scrn.Bounds} {scrn.ScalingPercent}%{If(scrn.Primary, " Primary", "")}", 1)
        Next
    End Sub

    Private Sub btnAltInfo_Click(sender As Object, e As EventArgs) Handles btnAltInfo.Click
        If FrmMain.AltPP IsNot Nothing AndAlso FrmMain.AltPP.Id <> 0 Then
            Dim ap As AstoniaProcess = FrmMain.AltPP
            dBug.Print($"{ap.Name} {ap.MainWindowHandle} DPI:{ap.DpiAware} SDL:{ap.isSDL} WS:{FrmMain.WindowsScaling}% AS:{ap.WindowsScaling}%", 1)
        Else
            dBug.Print("No Alt Proc Active", 1)
        End If
    End Sub
End Class