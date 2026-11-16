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
    Public Overloads Property FlatStyle As FlatStyle

    Private _buffer As Bitmap
    Private _ForeColor As Color = Color.Black
    Private _MouseOver As Boolean = False
    Private _MouseDown As Boolean = False

    Public Sub New()
        MyBase.New()
        MyBase.SetBoundsCore(MyBase.Left, MyBase.Top, MyBase.Width, 17, BoundsSpecified.Height)
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.SupportsTransparentBackColor, True)
    End Sub

    Protected Overrides Sub SetBoundsCore(x As Integer, y As Integer, width As Integer, height As Integer, specified As BoundsSpecified)

        Dim w As Integer = width
        If CheckAlign = ContentAlignment.MiddleLeft Then
            w = TextRenderer.MeasureText(Text, Font, New Size(Integer.MaxValue, Integer.MaxValue), TextFormatFlags.NoPadding Or TextFormatFlags.SingleLine).Width + 19
            If Text.Length > 4 Then w += 13
        End If
        MyBase.SetBoundsCore(x, y, w, 17, specified Or BoundsSpecified.Height Or BoundsSpecified.Width)
    End Sub

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim params As CreateParams = MyBase.CreateParams

            'params.ExStyle = params.ExStyle Or WindowStylesEx.WS_EX_TRANSPARENT

            Return params
        End Get
    End Property

    Public Sub ResetBuffer()
        If _buffer IsNot Nothing Then
            _buffer.Dispose()
            _buffer = Nothing
        End If
    End Sub

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

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If ClientSize.Width <= 0 OrElse ClientSize.Height <= 0 Then Exit Sub

        Dim g As Graphics = e.Graphics

        Dim bgColor As Color = GetEffectiveBackColor()
        g.Clear(bgColor)

        Dim rect As New Rectangle(0, 0, ClientSize.Width, 17)

        ' Checkbox dimensions
        Dim boxSize As Integer = 14
        Dim boxPadding As Integer = 4

        Dim sx As Single = 96.0F / g.DpiX
        Dim sy As Single = 96.0F / g.DpiY

        Dim boxX As Integer
        Dim boxY As Integer

        Select Case CheckAlign
            Case ContentAlignment.TopRight, ContentAlignment.MiddleRight, ContentAlignment.BottomRight
                boxX = CInt(rect.Right - (boxSize / sx) - boxPadding)
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
                Case CheckState.Checked : state = VisualStyles.CheckBoxState.CheckedDisabled
                Case CheckState.Indeterminate : state = VisualStyles.CheckBoxState.MixedDisabled
                Case Else : state = VisualStyles.CheckBoxState.UncheckedDisabled
            End Select
        ElseIf _MouseDown Then
            Select Case CheckState
                Case CheckState.Checked : state = VisualStyles.CheckBoxState.CheckedPressed
                Case CheckState.Indeterminate : state = VisualStyles.CheckBoxState.MixedPressed
                Case Else : state = VisualStyles.CheckBoxState.UncheckedPressed
            End Select
        ElseIf _MouseOver Then
            Select Case CheckState
                Case CheckState.Checked : state = VisualStyles.CheckBoxState.CheckedHot
                Case CheckState.Indeterminate : state = VisualStyles.CheckBoxState.MixedHot
                Case Else : state = VisualStyles.CheckBoxState.UncheckedHot
            End Select
        Else
            Select Case CheckState
                Case CheckState.Checked : state = VisualStyles.CheckBoxState.CheckedNormal
                Case CheckState.Indeterminate : state = VisualStyles.CheckBoxState.MixedNormal
                Case Else : state = VisualStyles.CheckBoxState.UncheckedNormal
            End Select
        End If

        ' Draw checkbox glyph
        Dim glyphSize As Size = CheckBoxRenderer.GetGlyphSize(g, state)
        If glyphSize.Width > 13 Then
            ' High-DPI downscaling via GDI-compatible buffer to prevent dark edge fringes
            'Dim bgColor As Color = GetEffectiveBackColor()
            Dim glyphRect As New Rectangle(0, 0, glyphSize.Width + 2, glyphSize.Height + 2)

            Using glyphBuf As BufferedGraphics = BufferedGraphicsManager.Current.Allocate(g, glyphRect)
                glyphBuf.Graphics.Clear(bgColor)
                CheckBoxRenderer.DrawCheckBox(glyphBuf.Graphics, New Point(1, 1), state)

                Using tmpBmp As New Bitmap(glyphRect.Width, glyphRect.Height, Imaging.PixelFormat.Format24bppRgb)
                    Using gTmp As Graphics = Graphics.FromImage(tmpBmp)
                        glyphBuf.Render(gTmp)
                    End Using

                    Dim oldInterp = g.InterpolationMode
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

                    Dim destRect As New Rectangle(boxX, 1, CInt(glyphRect.Width * sx), CInt(glyphRect.Height * sy))
                    Using ia As New Imaging.ImageAttributes()
                        ia.SetWrapMode(Drawing2D.WrapMode.Clamp)
                        g.DrawImage(tmpBmp, destRect, 0, 0, tmpBmp.Width, tmpBmp.Height, GraphicsUnit.Pixel, ia)
                    End Using

                    g.InterpolationMode = oldInterp
                End Using
            End Using
        Else
            ' At 100% DPI (96 DPI), draw directly onto e.Graphics (already double-buffered by WinForms)
            CheckBoxRenderer.DrawCheckBox(g, New Point(boxX, 2), state)
        End If

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

        Dim textRect As New Rectangle(rect.X, rect.Y, rect.Width, rect.Height)

        If CheckAlign = ContentAlignment.MiddleLeft OrElse CheckAlign = ContentAlignment.TopLeft OrElse CheckAlign = ContentAlignment.BottomLeft Then
            textRect.X = boxRect.Right
            textRect.Width = rect.Right - textRect.X
        ElseIf CheckAlign = ContentAlignment.MiddleRight OrElse CheckAlign = ContentAlignment.TopRight OrElse CheckAlign = ContentAlignment.BottomRight Then
            textRect.Width = boxRect.Left - boxPadding
        End If

        Using b As New SolidBrush(If(Me.Enabled, Me.ForeColor, Color.Gray))
            Using sf As New StringFormat With {
            .Alignment = stringAlignment,
            .LineAlignment = lineAlignment,
            .FormatFlags = StringFormatFlags.NoWrap,
            .Trimming = StringTrimming.None
        }
                If RightToLeft = RightToLeft.Yes Then
                    sf.FormatFlags = sf.FormatFlags Or StringFormatFlags.DirectionRightToLeft
                End If

                g.DrawString(Text, Font, b, textRect, sf)
            End Using
        End Using

        ' Focus rectangle
        If Focused AndAlso ShowFocusCues Then
            Dim focusRect As New Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, 17)
            ControlPaint.DrawFocusRectangle(g, focusRect, Me.ForeColor, Color.Black)
        End If
    End Sub

    Private Function GetEffectiveBackColor() As Color
        Dim ctrl As Control = Me
        While ctrl IsNot Nothing
            If ctrl.BackColor.A = 255 AndAlso ctrl.BackColor <> Color.Transparent Then
                Return ctrl.BackColor
            End If
            ctrl = ctrl.Parent
        End While
        Return SystemColors.Control
    End Function

End Class
