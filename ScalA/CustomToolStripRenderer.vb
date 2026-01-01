Public NotInheritable Class CustomToolStripRenderer
    Inherits ToolStripProfessionalRenderer
    Implements IDisposable

    Public Sub New()
        MyBase.New(New CustomColorTable())
    End Sub

    Public animTimer As New Timer() With {.Interval = 99}

    Private _menustrip As ContextMenuStrip
    Private _disposed As Boolean = False

    Public Sub InitAnimationTimer(cms As ContextMenuStrip)
        _menustrip = cms
        AddHandler animTimer.Tick, AddressOf AnimTimer_Tick
        If AnimsEnabled Then
            animTimer.Start()
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        If Not _disposed Then
            animTimer?.Stop()
            RemoveHandler animTimer.Tick, AddressOf AnimTimer_Tick
            animTimer?.Dispose()
            _disposed = True
        End If
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

            e.Graphics.DrawRectangle(dashedPen, New Rectangle(3, 1, 19, 19))
        End Using
    End Sub

    Protected Overrides Sub OnRenderItemImage(e As ToolStripItemImageRenderEventArgs)
        e.Graphics.DrawImage(e.Item.Image, e.ImageRectangle)
    End Sub

    Private Sub InvalidateCheckedItems(items As ToolStripItemCollection)
        For Each item As ToolStripMenuItem In items.OfType(Of ToolStripMenuItem)
            ' Only invalidate checked + visible ones
            If item.Checked AndAlso item.Visible Then
                item.Invalidate(New Rectangle(3, 1, 20, 20))
            End If

            ' Recurse visible submenus
            If item.HasDropDownItems AndAlso item.DropDown?.Visible Then
                InvalidateCheckedItems(item.DropDownItems)
            End If
        Next
    End Sub
    Public Shared insertItemAbove As ToolStripItem = Nothing
    Public Shared insertItemBelow As ToolStripItem = Nothing

#If DEBUG Then
    Private Shared seperatorPenAbove As New Pen(Color.Red, 3)
    Private Shared seperatorPenBelow As New Pen(Color.Green, 3)

    Protected Overrides Sub OnRenderMenuItemBackground(e As ToolStripItemRenderEventArgs)
        Dim sender = e.Item

        If FrmMain.draggeditem Is sender Then
            dBug.log($"OnRenderMenuItemBackground selecting {sender}")
            sender.Select()
        End If

        MyBase.OnRenderMenuItemBackground(e)
        If insertItemAbove Is sender AndAlso FrmMain.draggeditem IsNot sender Then
            e.Graphics.DrawLine(seperatorPenAbove, New Point(0, sender.Bounds.Height), New Point(sender.Bounds.Width, sender.Bounds.Height))
        End If
        If insertItemBelow Is sender AndAlso FrmMain.draggeditem IsNot sender Then
            e.Graphics.DrawLine(seperatorPenBelow, New Point(0, 0), New Point(sender.Bounds.Width, 0))
        End If
    End Sub

#Else
 Private Shared seperatorPen As New Pen(Color.Black, 2)
    Protected Overrides Sub OnRenderMenuItemBackground(e As ToolStripItemRenderEventArgs)
        MyBase.OnRenderMenuItemBackground(e)
        Dim sender = e.Item
        If insertItemAbove Is sender AndAlso FrmMain.draggeditem IsNot sender Then
            e.Graphics.DrawLine(seperatorPen, New Point(0, sender.Bounds.Height), New Point(sender.Bounds.Width, sender.Bounds.Height))
        End If
        If insertItemBelow Is sender AndAlso FrmMain.draggeditem IsNot sender Then
            e.Graphics.DrawLine(seperatorPen, New Point(0, 0), New Point(0, sender.Bounds.Height))
        End If
    End Sub
#End If

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
