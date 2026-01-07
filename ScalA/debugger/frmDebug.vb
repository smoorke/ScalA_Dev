Imports System.Runtime.InteropServices
Public Class frmDebug
#If DEBUG Then
    Private Sub frmDebug_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If logbuilder Is Nothing Then
            logbuilder = New System.Text.StringBuilder With {.Capacity = 100_000}
        End If

        Me.TopMost = True

        Me.Location = New Point(
        FrmMain.Location.X + (FrmMain.Width - Me.Width) \ 2,
        FrmMain.Location.Y + (FrmMain.Height - Me.Height) \ 2)

        tbLogLevel.Value = dBug.minLogLevel
        lblLogLevel.Text = $"Log Level {dBug.minLogLevel}"

        Me.Owner = FrmMain

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
            dBug.Print($"{ap.Name} {ap.MainWindowHandle} DPI:{ap.DpiAware} SDL:{ap.isSDL} WS:{FrmMain.WindowsScaling}% AS:{ap.WindowsScalingV2}%", 1)
            dBug.Print($"co: {ap.ClientOffset} rcc:{ap.ClientRect} rcw:{ap.WindowRect}", 1)
        Else
            dBug.Print("No Alt Proc Active", 1)
        End If
    End Sub



    Private Sub btnSaveLog_Click(sender As Object, e As EventArgs) Handles btnSaveLog.Click
        dBug.Print($"Saving Log", 1)
        Try
            Using sfd As New SaveFileDialog With {
                    .Filter = "Text Files (*.txt)|*.txt",
                    .Title = "Save Debug Log",
                    .FileName = $"ScalA_DebugLog_{Now:yyyyMMdd_HHmmss}.txt",
                    .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                    }

                If sfd.ShowDialog = DialogResult.OK Then
                    dBug.Print($"Saving as {sfd.FileName.Replace(Environment.UserName, "%USERNAME%")}", 1)
                    IO.File.WriteAllText(sfd.FileName, logbuilder.ToString)
                Else
                    dBug.Print($"Save Cancelled", 1)
                End If

            End Using
        Catch ex As Exception
            dBug.Print($"Error Saving Log", 1)
            dBug.Print(ex.Message, 0)
        End Try
    End Sub

    Private Sub btnClearLog_Click(sender As Object, e As EventArgs) Handles btnClearLog.Click
        logbuilder.Clear()
        dBug.Print("Log Cleared", 1)
    End Sub

    Private Sub tbLogLevel_Scroll(sender As TrackBar, e As EventArgs) Handles tbLogLevel.Scroll
        dBug.minLogLevel = sender.Value
        lblLogLevel.Text = $"Log Level {dBug.minLogLevel}"
        dBug.Print($"Set Minimum Log Level to {dBug.minLogLevel}", 1)
    End Sub

    Private Sub chkPollDPI_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkPollDPI.CheckedChanged
        If sender.Checked Then
            For Each item As ToolStripStatusLabel In ssDebug.Items.OfType(Of ToolStripStatusLabel)
                item.ForeColor = SystemColors.ControlText
            Next
        Else
            For Each item As ToolStripStatusLabel In ssDebug.Items.OfType(Of ToolStripStatusLabel)
                item.ForeColor = SystemColors.Control
                item.BorderSides = ToolStripStatusLabelBorderSides.None
            Next
        End If
    End Sub

    Private Sub cmsSSDebug_Opening(sender As ContextMenuStrip, e As System.ComponentModel.CancelEventArgs) Handles cmsSSDebug.Opening
        For Each it As ToolStripMenuItem In sender.Items.OfType(Of ToolStripMenuItem)
            it.Visible = False
        Next

        If sender.SourceControl Is ssDebug Then
            Dim ss As StatusStrip = CType(sender.SourceControl, StatusStrip)
            If ss IsNot Nothing Then
                Dim mousePos As Point = ss.PointToClient(Control.MousePosition)
                Dim clickedItem As ToolStripStatusLabel = ss.Items.OfType(Of ToolStripStatusLabel).FirstOrDefault(Function(i) i.Bounds.Contains(mousePos))

                If clickedItem IsNot Nothing Then
                    dBug.Print($"Opening Context Menu for {ss.Name} {clickedItem.Text}", 0)
                    Select Case clickedItem.Text
                        Case "DPI"
                            'sender.Items.Add(New ToolStripMenuItem("Toggle DPI Aware", Nothing, AddressOf ToggleDpiAware))
                            If chkPollDPI.Checked AndAlso FrmMain.AltPP IsNot Nothing AndAlso FrmMain.AltPP.Id <> 0 Then
                                ToggleDPIToolStripMenuItem.Visible = True
                                Return
                            End If
                        Case Else
                            'dBug.Print($"e.Cancel 1", 1)
                            e.Cancel = True
                    End Select
                Else
                    'dBug.Print($"e.Cancel 2", 1)
                    e.Cancel = True
                End If
            End If
        End If
        'dBug.Print($"e.Cancel 3", 1)
        e.Cancel = True
    End Sub

    Private Sub ToggleDPIToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToggleDPIToolStripMenuItem.Click
        If FrmMain.AltPP IsNot Nothing AndAlso FrmMain.AltPP.Id <> 0 Then
            FrmMain.AltPP.RegHighDpiAware = Not FrmMain.AltPP.RegHighDpiAware
            dBug.RestartClient(Nothing, Nothing)
            dBug.Print($"Toggled DPI aware mode for {FrmMain.AltPP.FinalPath.Replace(Environment.UserName, "%USERNAME%")} to {Not FrmMain.AltPP.RegHighDpiAware}", 1)
        Else
            dBug.Print("No Alt Proc Active", 1)
        End If
    End Sub

    Private Sub btnTestScreenManager_Click(sender As Object, e As EventArgs) Handles btnTestScreenManager.Click
        ScreenManagerTests.RunTests(Screen.FromControl(Me))
    End Sub

    Private Sub chkForceShowUpdate_CheckedChanged(sender As Object, e As EventArgs) Handles chkForceShowUpdate.CheckedChanged
        FrmMain.pnlUpdate.Visible = Not FrmMain.pnlUpdate.Visible
    End Sub


    Private crtMode As Boolean = frmCrt.Visible


    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        'If FrmMain.AltPP Is Nothing Then
        '    dBug.Print("Alt is nothing", 1)
        '    Exit Sub
        'End If


        'FrmMain.AltPP?.DebugListAllArgs()
        dBug.Print("linkwatchers")
        For Each kvp In ResolvedLinkWatchers
            dBug.Print(kvp.Key)
        Next
        dBug.Print("---")
        'FrmMain.pnlUpdate.Visible = Not FrmMain.pnlUpdate.Visible

        'If Not crtMode Then
        '    If frmCrt.IsDisposed Then frmCrt = New CrtForm
        '    frmCrt.Show()
        'Else
        '    frmCrt.Close()
        'End If



        'crtMode = Not crtMode



        'Dim style As UInteger = GetWindowLong(hwnd, GWL_STYLE)
        'style = style Or WindowStyles.WS_SIZEFRAME ' Add resizing capability
        ' SetWindowLong(hwnd, GWL_STYLE, style)

        'SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 1600, 1200, SetWindowPosFlags.DoNotReposition)
        'Const WM_ERASEBKGND = &H14
        'Const WM_SYNCPAINT = &H88
        'SendMessage(hwnd, WM_ERASEBKGND, 0, 0)
        'SendMessage(hwnd, WM_SYNCPAINT, 0, 0)

    End Sub

    Private Sub btnIpcInfo_Click(sender As Object, e As EventArgs) Handles btnIpcInfo.Click
        dBug.Print($"IPC: {IPC.getInstances.Count}")
        For Each ip In IPC.getInstances
            dBug.Print($"{ip.AltPPid} {ip.handle.ToString("X8")} {AstoniaProcess.FromHWnd(Process.GetProcessById(ip.AltPPid)?.MainWindowHandle)?.UserName}")
        Next
    End Sub

    Private Sub DumpWatchersToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DumpWatchersToolStripMenuItem.Click
        dBug.Print("-Begin DumpWatchers-")
        For Each watcher In ResolvedLinkWatchers
            dBug.Print(watcher.Key)
        Next
        dBug.Print("---")
        For Each kvp In ResolvedLinkLinks
            dBug.Print($"{kvp.Key} {kvp.Value}")
        Next
        dBug.Print("-End DumpWatchers-")
    End Sub

    Private Sub PurgeIconCacheMiscToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PurgeIconCacheMiscToolStripMenuItem.Click
        Dim counter As Integer = 0
        Dim hasUrl As Boolean = False
        For Each key In FrmMain.iconCache.Keys.Where(Function(k)
                                                         Dim it As String = k.ToLower
                                                         Return Not (k.EndsWith("\") OrElse k.EndsWith(".lnk") OrElse k.EndsWith(".exe"))
                                                     End Function)
            If FrmMain.iconCache.TryRemove(key, Nothing) Then
                If key.ToLower.EndsWith(".url") Then hasUrl = True
                counter += 1
                dBug.Print($"removed {key}")
            End If
        Next
        If hasUrl Then FrmMain.DefURLicons.Clear()
        dBug.Print($"{counter} items removed, urlcache.size {FrmMain.DefURLicons.count}")
    End Sub

    Private Sub TootltiptestToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TootltiptestToolStripMenuItem.Click
        'MenuToolTip.InitializeTooltip(Me.Handle)
        'MenuToolTip.ShowTooltipWithDelay("Test123", 0, 0)
    End Sub
#End If
End Class
#If DEBUG Then
Module glob
    Public NotInheritable Class CrtForm : Inherits Form
        Sub New()
            Me.Owner = FrmMain
            Me.BackColor = Color.Red
            Me.TransparencyKey = Me.BackColor
            Me.Opacity = 0.125
            Me.FormBorderStyle = BorderStyle.None
            Me.ShowInTaskbar = False
        End Sub

        Sub ctr_shown(sender As Object, e As EventArgs) Handles Me.Shown
            dBug.Print($"{FrmMain.pbZoom.Bounds}", 1)
            dBug.Print($"{FrmMain.RectangleToScreen(FrmMain.pbZoom.Bounds)}", 1)
            dBug.Print($"{FrmMain.pbZoom.RectangleToScreen(FrmMain.pbZoom.Bounds)}", 1)

            Me.Bounds = FrmMain.RectangleToScreen(FrmMain.pbZoom.Bounds)
        End Sub
        Protected Overrides ReadOnly Property CreateParams As CreateParams
            Get
                Dim cp As CreateParams = MyBase.CreateParams
                'cp.Style = cp.Style Or WindowStyles.WS_SYSMENU Or WindowStyles.WS_MINIMIZEBOX
                cp.ExStyle = cp.ExStyle Or WindowStylesEx.WS_EX_TRANSPARENT
                'cp.ClassStyle = cp.ClassStyle Or CS_DROPSHADOW
                Return cp
            End Get
        End Property
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            Dim p As Pen = New Pen(If(My.Settings.DarkMode, Color.Black, Color.White))
            For i = 1 To Me.Height - 1 Step 3
                e.Graphics.DrawLine(p, 0, i, Me.Width, i)
            Next
        End Sub
    End Class
    Public frmCrt As New CrtForm
End Module
#End If
