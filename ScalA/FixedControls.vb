
Public Class FixedControls

End Class
Public Class FixedCheckbox : Inherits CheckBox
    Sub New()
        Me.SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer, True)
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        'MyBase.OnPaint(pevent)
        Dim g As Graphics = e.Graphics
        g.Clear(Me.BackColor)

        '
#If DEBUG Then
        Dim x As VisualStyles.CheckBoxState

        x = VisualStyles.CheckBoxState.UncheckedNormal '1
        x = VisualStyles.CheckBoxState.UncheckedHot '2
        x = VisualStyles.CheckBoxState.UncheckedPressed '3
        x = VisualStyles.CheckBoxState.UncheckedDisabled '4

        x = VisualStyles.CheckBoxState.CheckedNormal '5
        x = VisualStyles.CheckBoxState.CheckedHot '6
        x = VisualStyles.CheckBoxState.CheckedPressed '7
        x = VisualStyles.CheckBoxState.CheckedDisabled '8

        x = VisualStyles.CheckBoxState.MixedNormal '9
        x = VisualStyles.CheckBoxState.MixedHot '10
        x = VisualStyles.CheckBoxState.MixedPressed '11
        x = VisualStyles.CheckBoxState.MixedDisabled '12
#End If

        Dim state = VisualStyles.CheckBoxState.UncheckedNormal
        If Me.MouseIsOver Then state = VisualStyles.CheckBoxState.UncheckedHot
        If Me.IsPressed Then state = VisualStyles.CheckBoxState.UncheckedPressed
        If Not Me.Enabled Then state = VisualStyles.CheckBoxState.UncheckedDisabled

        state += Me.CheckState * 4

        Dim glyphSize As Size = CheckBoxRenderer.GetGlyphSize(g, state)
        Dim glyphTop As Integer = (Me.Height - glyphSize.Height) \ 2
        Dim glyphRect As New Rectangle(2, glyphTop, glyphSize.Width, glyphSize.Height)


        CheckBoxRenderer.DrawCheckBox(g, New Point(Me.Width - glyphSize.Width, Me.Height / 2 - glyphSize.Height / 2), state)

        ' Draw text
        Dim textRect As New Rectangle(e.ClipRectangle.Left + 6, 0, e.ClipRectangle.Width - 8, Me.Height)
        TextRenderer.DrawText(g, Me.Text, Me.Font, textRect, Me.ForeColor, TextFormatFlags.VerticalCenter Or TextFormatFlags.Left)
    End Sub
    Private MouseIsOver As Boolean

    Private MouseIsDown As Boolean
    Private SpaceIsDown As Boolean

    Private IsPressed As Boolean

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MouseIsOver = True
        Me.Invalidate()
        'MyBase.OnMouseEnter(e)
    End Sub
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        If Me.ClientRectangle.Contains(e.Location) Then
            Me.IsPressed = MouseIsDown OrElse SpaceIsDown
        Else
            Me.IsPressed = SpaceIsDown
        End If
        'MyBase.OnMouseMove(e)
        Me.Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        Debug.Print($"MouseLeave {MouseIsOver} {MouseIsDown}")
        MouseIsOver = MouseIsDown
        IsPressed = MouseIsDown OrElse SpaceIsDown
        'MyBase.OnMouseLeave(e)
        Me.Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        Debug.Print($"MouseDown")
        If e.Button = MouseButtons.Left Then
            IsPressed = True
            MouseIsDown = True
        End If

        MyBase.OnMouseDown(e)
        Me.Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        Debug.Print($"MouseUp")
        If e.Button = MouseButtons.Left Then
            IsPressed = False
            MouseIsDown = False
        End If
        MyBase.OnMouseUp(e)
        Me.Invalidate()
    End Sub

    Protected Overrides Sub OnKeyDown(kevent As KeyEventArgs)
        If kevent.KeyCode = Keys.Space Then
            IsPressed = True
            'MouseIsDown = False
            SpaceIsDown = True
        End If
        Me.Invalidate()
        MyBase.OnKeyDown(kevent)
    End Sub

    Protected Overrides Sub OnKeyUp(kevent As KeyEventArgs)
        Debug.Print($"keyup {kevent.KeyCode}")
        If kevent.KeyCode = Keys.Space Then
            IsPressed = False
            MouseIsDown = False
            SpaceIsDown = False
        End If
        Me.Invalidate()
        MyBase.OnKeyUp(kevent)
    End Sub

End Class

Public Class FixedTextbox : Inherits TextBox
    Protected Overrides Sub ScaleControl(factor As SizeF, specified As BoundsSpecified)
        'MyBase.ScaleControl(factor, specified)
    End Sub
End Class