Imports System.ComponentModel

Public NotInheritable Class FixedScaleCheckbox : Inherits CheckBox

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overrides Property ForeColor As Color
        Get
            Return _ForeColor
        End Get
        Set(value As Color)
            If _ForeColor <> value Then
                _ForeColor = value
                ResetBuffer()
                Invalidate()
            End If
        End Set
    End Property

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads Property BackColor As Color

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads Property FlatStyle As FlatStyle

    Private _buffer As Bitmap
    Private _ForeColor As Color = Color.GreenYellow
    Private _MouseOver As Boolean = False
    Private _MouseDown As Boolean = False

    Public Sub New()
        MyBase.New()

        SetStyle(ControlStyles.OptimizedDoubleBuffer Or
             ControlStyles.ResizeRedraw Or
             ControlStyles.UserPaint, True)

        Me.ClientSize = New Size(Me.ClientSize.Width, 17)
    End Sub

    Public Sub ResetBuffer()
        If _buffer IsNot Nothing Then
            _buffer.Dispose()
            _buffer = Nothing
        End If
    End Sub

    Private _font As Font
    Public Overrides Property Font() As Font
        Get
            Return If(_font, MyBase.Font)
        End Get
        Set(ByVal value As Font)
            _font = value
        End Set
    End Property

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then ResetBuffer()
        MyBase.Dispose(disposing)
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        ResetBuffer()
        Invalidate()
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnCheckedChanged(e As EventArgs)
        MyBase.OnCheckedChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnCheckStateChanged(e As EventArgs)
        MyBase.OnCheckStateChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        MyBase.OnEnabledChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnGotFocus(e As EventArgs)
        MyBase.OnGotFocus(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnLostFocus(e As EventArgs)
        MyBase.OnLostFocus(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        _MouseOver = True
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        _MouseOver = False
        _MouseDown = False
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)

        If e.Button = MouseButtons.Left Then
            _MouseDown = True
            Invalidate()
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        _MouseDown = False
        Invalidate()
    End Sub

    Protected Overrides Sub OnInvalidated(e As InvalidateEventArgs)
        MyBase.OnInvalidated(e)
        PaintCheckBox()
    End Sub

    Private Sub PaintCheckBox()

        If ClientSize.Width <= 0 OrElse ClientSize.Height <= 0 Then Exit Sub

        If _buffer Is Nothing OrElse _buffer.Width <> ClientSize.Width Then
            ResetBuffer()
            _buffer = New Bitmap(ClientSize.Width, 17)
        End If

        Using g As Graphics = Graphics.FromImage(_buffer)

            g.Clear(FrmMain.pnlTitleBar.BackColor)

            Dim rect As New Rectangle(0, 0, ClientSize.Width, 17)

            'Debug.Print($"CheckRect {rect}")

            ' Checkbox dimensions
            Dim boxSize As Integer = 14
            Dim boxPadding As Integer = 4

            Dim boxX As Integer
            Dim boxY As Integer

            Dim sx As Single = 96.0F / g.DpiX
            Dim sy As Single = 96.0F / g.DpiY

            Select Case CheckAlign
                Case ContentAlignment.TopRight, ContentAlignment.MiddleRight, ContentAlignment.BottomRight

                    boxX = rect.Right - boxSize / sx - boxPadding

                Case ContentAlignment.TopCenter, ContentAlignment.MiddleCenter, ContentAlignment.BottomCenter

                    boxX = (rect.Width - boxSize) \ 2

                Case Else

                    boxX = boxPadding
            End Select

            Select Case CheckAlign
                Case ContentAlignment.TopLeft, ContentAlignment.TopCenter, ContentAlignment.TopRight

                    boxY = boxPadding

                Case ContentAlignment.BottomLeft, ContentAlignment.BottomCenter, ContentAlignment.BottomRight

                    boxY = rect.Bottom - boxSize - boxPadding

                Case Else

                    boxY = (rect.Height - boxSize) \ 2
            End Select

            Dim boxRect As New Rectangle(boxX, boxY, boxSize, boxSize)

            ' Determine visual state
            Dim state As VisualStyles.CheckBoxState

            If Not Enabled Then

                Select Case CheckState
                    Case CheckState.Checked
                        state = VisualStyles.CheckBoxState.CheckedDisabled
                    Case CheckState.Indeterminate
                        state = VisualStyles.CheckBoxState.MixedDisabled
                    Case Else
                        state = VisualStyles.CheckBoxState.UncheckedDisabled
                End Select

            ElseIf _MouseDown Then

                Select Case CheckState
                    Case CheckState.Checked
                        state = VisualStyles.CheckBoxState.CheckedPressed
                    Case CheckState.Indeterminate
                        state = VisualStyles.CheckBoxState.MixedPressed
                    Case Else
                        state = VisualStyles.CheckBoxState.UncheckedPressed
                End Select

            ElseIf _MouseOver Then

                Select Case CheckState
                    Case CheckState.Checked
                        state = VisualStyles.CheckBoxState.CheckedHot
                    Case CheckState.Indeterminate
                        state = VisualStyles.CheckBoxState.MixedHot
                    Case Else
                        state = VisualStyles.CheckBoxState.UncheckedHot
                End Select

            Else

                Select Case CheckState
                    Case CheckState.Checked
                        state = VisualStyles.CheckBoxState.CheckedNormal
                    Case CheckState.Indeterminate
                        state = VisualStyles.CheckBoxState.MixedNormal
                    Case Else
                        state = VisualStyles.CheckBoxState.UncheckedNormal
                End Select

            End If



            Dim glyphSize As Size = CheckBoxRenderer.GetGlyphSize(g, state)

            Using glyph As New Bitmap(glyphSize.Width, glyphSize.Height)

                Using gg As Graphics = Graphics.FromImage(glyph)
                    gg.Clear(FrmMain.pnlTitleBar.BackColor)
                    CheckBoxRenderer.DrawCheckBox(gg, Point.Empty, state)
                End Using

                Dim oldState = g.Save()

                g.ScaleTransform(sx, sy)

                Dim dest As New Point(boxX / sx, 1)

                'dBug.Print($"glyphdest {dest} s:{sx}/{sy}", 1)

                g.DrawImageUnscaled(glyph, dest)

                g.Restore(oldState)

            End Using

            ' Text alignment
            Dim stringAlignment As StringAlignment = StringAlignment.Near
            Dim lineAlignment As StringAlignment = StringAlignment.Center

            Select Case TextAlign

                Case ContentAlignment.TopLeft, ContentAlignment.MiddleLeft, ContentAlignment.BottomLeft

                    stringAlignment = StringAlignment.Near

                Case ContentAlignment.TopCenter, ContentAlignment.MiddleCenter, ContentAlignment.BottomCenter

                    stringAlignment = StringAlignment.Center

                Case ContentAlignment.TopRight, ContentAlignment.MiddleRight, ContentAlignment.BottomRight

                    stringAlignment = StringAlignment.Far

            End Select

            Select Case TextAlign

                Case ContentAlignment.TopLeft, ContentAlignment.TopCenter, ContentAlignment.TopRight

                    lineAlignment = StringAlignment.Near

                Case ContentAlignment.BottomLeft, ContentAlignment.BottomCenter, ContentAlignment.BottomRight

                    lineAlignment = StringAlignment.Far

            End Select

            Dim textRect As Rectangle = rect

            If CheckAlign = ContentAlignment.MiddleLeft OrElse CheckAlign = ContentAlignment.TopLeft OrElse CheckAlign = ContentAlignment.BottomLeft Then

                textRect.X = boxRect.Right + boxPadding
                textRect.Width = rect.Right - textRect.X

            ElseIf CheckAlign = ContentAlignment.MiddleRight OrElse CheckAlign = ContentAlignment.TopRight OrElse CheckAlign = ContentAlignment.BottomRight Then

                textRect.Width = boxRect.Left - boxPadding

            End If

            Using b As New SolidBrush(_ForeColor)

                Using sf As New StringFormat With {.Alignment = stringAlignment, .LineAlignment = lineAlignment, .FormatFlags = StringFormatFlags.NoWrap, .Trimming = StringTrimming.None}

                    If RightToLeft = RightToLeft.Yes Then
                        sf.FormatFlags = sf.FormatFlags Or StringFormatFlags.DirectionRightToLeft
                    End If

                    g.DrawString(Text, Font, b, textRect, sf)

                End Using

            End Using

            ' Focus rectangle
            If Focused AndAlso ShowFocusCues Then

                Dim focusRect As Rectangle = rect
                focusRect.Inflate(-1, -1)

                ControlPaint.DrawFocusRectangle(g, focusRect, _ForeColor, Color.Black)

            End If

        End Using

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        If _buffer Is Nothing Then PaintCheckBox()

        If _buffer IsNot Nothing Then
            e.Graphics.Clear(FrmMain.pnlTitleBar.BackColor)
            e.Graphics.DrawImageUnscaled(_buffer, Point.Empty)
        End If

    End Sub



End Class
