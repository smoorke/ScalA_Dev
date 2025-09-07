Imports System.Drawing.Drawing2D

Public NotInheritable Class CustomToolStripRenderer : Inherits ToolStripProfessionalRenderer

    Public Sub New()
        MyBase.New(New CustomColorTable())
    End Sub

    Protected Overrides Sub OnRenderItemCheck(e As ToolStripItemImageRenderEventArgs)
        Using dashedPen As New Pen(If(clipBoardInfo.Action = ClipboardAction.Move, If(e.Item.Selected, Color.Red, Color.Pink), If(My.Settings.DarkMode, If(e.Item.Selected, Color.DarkBlue, Color.LightBlue), Color.Black)))
            dashedPen.DashStyle = Drawing2D.DashStyle.DashDot
            e.Graphics.DrawRectangle(dashedPen, New Rectangle(3, 1, 19, 19))
        End Using
    End Sub

    Protected Overrides Sub OnRenderItemImage(e As ToolStripItemImageRenderEventArgs)
        e.Graphics.DrawImage(e.Item.Image, e.ImageRectangle)
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
