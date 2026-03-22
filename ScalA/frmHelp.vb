Imports System.Runtime.InteropServices

''' <summary>
''' Help and FAQ window with categorized documentation
''' </summary>
Public Class frmHelp
    Dim CurrentClientSize As Size
    Public Sub New()
        InitializeComponent()
        CurrentClientSize = Me.ClientSize
    End Sub

    Protected Friend Overloads Sub Show(Optional owner As Form = Nothing)
        Me.BringToFront()
        If IsIconic(Me.Handle) Then Me.WndProc(New Message With {.HWnd = Me.Handle, .Msg = WM_SYSCOMMAND, .WParam = SC_RESTORE})
        If Not Me.Visible Then MyBase.Show(owner)
    End Sub


    Private Sub frmHelp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateCategories()
        ' Select welcome node
        If tvCategories.Nodes.Count > 0 Then
            tvCategories.SelectedNode = tvCategories.Nodes(0)
        End If

        ' fix scaling issue
        Dim rcC As RECT
        GetClientRect(Me.Handle, rcC)

        Me.Size = New Size(Me.Width - rcC.right + CurrentClientSize.Width,
                           Me.Height - rcC.bottom + CurrentClientSize.Height)
    End Sub

    Dim gti As New GUITHREADINFO With {.cbSize = Marshal.SizeOf(Of GUITHREADINFO)}
    Dim InMove As Boolean = False
    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case WM_WINDOWPOSCHANGING
                Dim winpos As WINDOWPOS = System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                If StructureToPtrSupported Then
                    If Not winpos.flags.HasFlag(SetWindowPosFlags.IgnoreResize) Then
                        Dim tid = GetWindowThreadProcessId(Me.Handle, Nothing)
                        If InMove Then ' GetGUIThreadInfo(tid, gti) AndAlso (gti.flags And &H2) = 2 AndAlso  SendMessage(Me.Handle, WM_NCHITTEST, IntPtr.Zero, New LParamMap(Control.MousePosition)) = HTCAPTION Then
                            Debug.Print($"help sizemove {gti.flags} {SendMessage(Me.Handle, WM_NCHITTEST, IntPtr.Zero, New LParamMap(Control.MousePosition)) = HTCAPTION}")
                            Dim rcC As RECT
                            GetClientRect(Me.Handle, rcC)
                            winpos.cx = Me.Width - rcC.right + CurrentClientSize.Width
                            winpos.cy = Me.Height - rcC.bottom + CurrentClientSize.Height
                            System.Runtime.InteropServices.Marshal.StructureToPtr(winpos, m.LParam, True)
                        End If
                    End If
                End If
            Case WM_NCLBUTTONDOWN
                If m.WParam = HTCAPTION Then
                    InMove = True
                End If
            Case WM_SYSCOMMAND
                If m.WParam = SC_MOVE Then
                    InMove = True
                End If
            Case WM_NCLBUTTONUP
                InMove = False
            Case WM_EXITSIZEMOVE
                InMove = False

            Case WM_GETDPISCALEDSIZE
                If StructureToPtrSupported Then
                    Dim sz = Marshal.PtrToStructure(Of Size)(m.LParam)
                    Dim rcW As RECT
                    GetWindowRect(Me.Handle, rcW)
                    Dim rcC As RECT
                    GetClientRect(Me.Handle, rcC)
                    Dim factor = (m.WParam.ToInt32 And &HFFFF) / currentDPI
                    Debug.Print($"rcC {rcC.right} {rcC.bottom} {factor} {rcW.bottom - rcW.top - rcC.bottom}")
                    sz.Width = CurrentClientSize.Width + (rcW.right - rcW.left - rcC.right) * factor
                    sz.Height = CurrentClientSize.Height + (rcW.bottom - rcW.top - rcC.bottom) * factor
                    Marshal.StructureToPtr(sz, m.LParam, False)
                End If
                m.Result = 1
                Exit Sub
            Case WM_DPICHANGED
                currentDPI = m.WParam.ToInt32 And &HFFFF

        End Select

        MyBase.WndProc(m)
        Select Case m.Msg
            Case WM_NCLBUTTONDOWN
                If m.WParam = HTCAPTION Then
                    InMove = False
                End If
        End Select
        CurrentClientSize = Me.ClientSize
    End Sub
    Dim currentDPI = 96
    Private Sub PopulateCategories()
        tvCategories.Nodes.Clear()

        ' All top-level nodes - condensed structure
        tvCategories.Nodes.Add("welcome", "Welcome")
        tvCategories.Nodes.Add("quick_start", "Quick Start Guide")
        tvCategories.Nodes.Add("client_management", "Client Management")
        tvCategories.Nodes.Add("quick_launch", "Quick Launch & Shortcuts")
        tvCategories.Nodes.Add("launcher_setup", "Launcher Setup")
        tvCategories.Nodes.Add("hotkeys", "Hotkeys & Key Bindings")
        tvCategories.Nodes.Add("settings", "Settings Reference")
        tvCategories.Nodes.Add("troubleshooting", "Troubleshooting")
        tvCategories.Nodes.Add("credits", "Credits & About")
    End Sub

    Private Sub tvCategories_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tvCategories.AfterSelect
        If e.Node Is Nothing Then Return

        Dim key = e.Node.Name
        Dim content = HelpContent.GetContent(key)
        DisplayContent(content)
    End Sub

    Private Sub DisplayContent(content As String)
        rtbContent.Clear()

        ' Simple formatting: lines starting with uppercase are headers
        Dim lines = content.Split({vbCrLf, vbLf}, StringSplitOptions.None)

        For Each line In lines
            If String.IsNullOrEmpty(line) Then
                rtbContent.AppendText(vbCrLf)
                Continue For
            End If

            ' Check if this is a header (all uppercase line)
            If IsHeader(line) Then
                AppendHeader(line)
            ElseIf line.StartsWith("  -") OrElse line.StartsWith("  -") Then
                ' Bullet point
                AppendBullet(line)
            ElseIf line.StartsWith("  ") AndAlso Char.IsDigit(line.TrimStart()(0)) Then
                ' Numbered item
                AppendNumbered(line)
            Else
                ' Normal text
                AppendNormal(line)
            End If
        Next
    End Sub

    Private Function IsHeader(line As String) As Boolean
        Dim trimmed = line.Trim()
        If String.IsNullOrEmpty(trimmed) Then Return False
        If trimmed.Length < 3 Then Return False

        ' Headers are all uppercase with possible spaces and special chars
        For Each c In trimmed
            If Char.IsLetter(c) AndAlso Not Char.IsUpper(c) Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub AppendHeader(text As String)
        Dim start = rtbContent.TextLength
        rtbContent.AppendText(text & vbCrLf)
        rtbContent.Select(start, text.Length)
        rtbContent.SelectionFont = New Font(rtbContent.Font.FontFamily, 11, FontStyle.Bold)
        rtbContent.SelectionColor = Color.FromArgb(0, 102, 204)
        rtbContent.Select(rtbContent.TextLength, 0)
    End Sub

    Private Sub AppendBullet(text As String)
        rtbContent.AppendText(text & vbCrLf)
    End Sub

    Private Sub AppendNumbered(text As String)
        Dim start = rtbContent.TextLength
        rtbContent.AppendText(text & vbCrLf)
        ' Bold the number
        rtbContent.Select(start, text.IndexOf(".") + 1)
        rtbContent.SelectionFont = New Font(rtbContent.Font, FontStyle.Bold)
        rtbContent.Select(rtbContent.TextLength, 0)
    End Sub

    Private Sub AppendNormal(text As String)
        rtbContent.AppendText(text & vbCrLf)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class
