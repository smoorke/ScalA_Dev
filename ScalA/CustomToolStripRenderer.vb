Public NotInheritable Class CustomToolStripRenderer : Inherits ToolStripProfessionalRenderer

    Public Sub New()
        MyBase.New(New CustomColorTable())
    End Sub

    Public animTimer As New Timer() With {.Interval = 99}

    Private _menustrip As ContextMenuStrip

    Public Sub InitAnimationTimer(cms As ContextMenuStrip)
        _menustrip = cms
        AddHandler animTimer.Tick, AddressOf AnimTimer_Tick
        If AnimsEnabled Then
            animTimer.Start()
        End If
    End Sub
    Private Shared Function EvenInt(dbl As Single) As Integer
        Dim int As Integer = CInt(dbl)
        If int Mod 2 <> 0 Then int -= 1
        Return int
    End Function
    Private Shared TriangleDict As New Concurrent.ConcurrentDictionary(Of Single, Bitmap)
    Private Shared Function GetTriangle(scaling As Single) As Bitmap

        Dim col = Color.Black

        Return TriangleDict.GetOrAdd(scaling,
                    Function()

                        Dim width = EvenInt(3 + scaling)
                        Dim height = width * 2 - 1

                        ' Height is proportional to the length
                        Dim bmp As New Bitmap(width, height)

                        Debug.Print($"Triangle {scaling} size {bmp.Size}")

                        ' Loop horizontally from the left base to the right tip
                        For x As Integer = 0 To width - 1
                            For y As Integer = x To height - x - 1
                                bmp.SetPixel(x, y, col)
                            Next
                        Next
                        Return bmp
                    End Function)
    End Function
    Protected Overrides Sub OnRenderArrow(e As ToolStripArrowRenderEventArgs)
        Dim tri = GetTriangle(ScaledScrollable.Scaling)
        e.Graphics.DrawImageUnscaled(tri, New Rectangle(e.ArrowRectangle.Right - tri.Width - 3, (e.ArrowRectangle.Height + 1.5) \ 2 - tri.Height \ 2, tri.Width, tri.Height))
    End Sub

    Private Sub AnimTimer_Tick(sender As Object, e As EventArgs)
        If _menustrip?.Visible Then InvalidateCheckedItems(_menustrip.Items)
    End Sub

    Private animSW As Stopwatch = Stopwatch.StartNew()
    Private pattrn As Single() = {4.0F, 2.0F, 1.0F, 2.0F}
    Private totalPat As Single = pattrn.Sum()

    Protected Overrides Sub OnRenderItemCheck(e As ToolStripItemImageRenderEventArgs)
        Using dashedPen As New Pen(If(clipBoardInfo.Action.HasFlag(DragDropEffects.Move), Color.Red, If(My.Settings.DarkMode, If(e.Item.Selected, Color.DarkBlue, Color.White), Color.Black)), 1)

            ' Define a fixed dash pattern (on/off lengths in pixels)
            dashedPen.DashPattern = pattrn

            ' Animate offset: elapsed time mod total cycle
            Dim cycle As Single = totalPat
            Dim offset As Single = CSng((animSW.ElapsedMilliseconds \ 200) Mod cycle)
            dashedPen.DashOffset = offset * If(clipBoardInfo.Action.HasFlag(DragDropEffects.Copy), -1, 1)
            Dim size As Integer = Math.Max(19, e.Item.Height - 3)
            'Debug.Print($"rendItemCheck {size} {e.Item.Height}")
            e.Graphics.DrawRectangle(dashedPen, New Rectangle(3, 1, size, size))
        End Using
    End Sub

    Protected Overrides Sub OnRenderItemImage(e As ToolStripItemImageRenderEventArgs)
        e.Graphics.DrawImage(e.Item.Image, e.ImageRectangle)
    End Sub

    Private Sub InvalidateCheckedItems(items As ToolStripItemCollection)
        For Each item As ToolStripMenuItem In items.OfType(Of ToolStripMenuItem)
            ' Only invalidate checked + visible ones
            If item.Checked AndAlso item.Visible Then
                Dim sz = Math.Max(20, item.Size.Height - 2)
                item.Invalidate(New Rectangle(New Point(3, 1), New Size(sz, sz)))
            End If

            ' Recurse visible submenus
            If item.HasDropDownItems AndAlso item.DropDown?.Visible Then
                InvalidateCheckedItems(item.DropDownItems)
            End If
        Next
    End Sub

End Class

Public NotInheritable Class CustomColorTable

    Inherits ProfessionalColorTable

    Public Overrides ReadOnly Property ToolStripBorder As Color
        Get
            Return If(My.Settings.DarkMode, Color.Gray, MyBase.ToolStripBorder)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
        Get
            Return If(My.Settings.DarkMode, Color.DarkGray, MyBase.ToolStripDropDownBackground)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripGradientBegin As Color
        Get
            Return If(My.Settings.DarkMode, Color.DarkGray, MyBase.ToolStripGradientBegin)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripGradientEnd As Color
        Get
            Return If(My.Settings.DarkMode, Color.DarkGray, MyBase.ToolStripGradientEnd)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripGradientMiddle As Color
        Get
            Return If(My.Settings.DarkMode, Color.DarkGray, MyBase.ToolStripGradientMiddle)
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
        Get
            Return If(My.Settings.DarkMode, Color.FromArgb(&HFFA2A2A2), MyBase.ImageMarginGradientBegin)
        End Get
    End Property
    Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
        Get
            Return If(My.Settings.DarkMode, Color.FromArgb(&HFFA2A2A2), MyBase.ImageMarginGradientMiddle)
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
        Get
            Return If(My.Settings.DarkMode, Color.FromArgb(&HFFA2A2A2), MyBase.ImageMarginGradientEnd)
        End Get
    End Property

End Class
