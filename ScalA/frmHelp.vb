''' <summary>
''' Help and FAQ window with categorized documentation
''' </summary>
Public Class frmHelp
    Dim DesignedClientSize As Size
    Public Sub New(Optional initialPath As String = "")
        InitializeComponent()
        DesignedClientSize = Me.ClientSize
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
