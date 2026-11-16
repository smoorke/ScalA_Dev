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
        'MyBase.SetBoundsCore(MyBase.Left, MyBase.Top, MyBase.Width, 17, BoundsSpecified.Height)
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.SupportsTransparentBackColor, True)
    End Sub

    Protected Overrides Sub SetBoundsCore(x As Integer, y As Integer, width As Integer, height As Integer, specified As BoundsSpecified)
        Dim w As Integer = width
        If CheckAlign = ContentAlignment.MiddleLeft Then
            'w = TextRenderer.MeasureText(Text, Font, New Size(Integer.MaxValue, Integer.MaxValue), TextFormatFlags.NoPrefix Or TextFormatFlags.SingleLine).Width + 19
            'If Text.Length > 4 Then w += 13
        End If
        MyBase.SetBoundsCore(x, y, w, If(Me.Dock = DockStyle.None, 17 * (Me.Text.Count(Function(c) vbCrLf.Contains(c)) \ 2 + 1), height), specified Or BoundsSpecified.Height Or BoundsSpecified.Width)
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

        Dim rect As New Rectangle(0, 0, ClientSize.Width, ClientSize.Height)

        ' Checkbox dimensions
        Dim boxSize As Integer = 13
        Dim boxPadding As Integer = 1

        Dim sx As Single = 96.0F / g.DpiX
        Dim sy As Single = 96.0F / g.DpiY

        Dim boxX As Integer
        Dim boxY As Integer

        Select Case CheckAlign
            Case ContentAlignment.TopRight, ContentAlignment.MiddleRight, ContentAlignment.BottomRight
                boxX = rect.Right - boxSize - boxPadding
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
        If glyphSize.Width <> boxSize OrElse glyphSize.Height <> boxSize Then
            'Dim bgColor As Color = GetEffectiveBackColor()
            Dim scaledGlyph As Bitmap = GetScaledGlyph(g, state, glyphSize, boxSize, bgColor)
            g.DrawImageUnscaled(scaledGlyph, boxRect.Location)
        Else
            CheckBoxRenderer.DrawCheckBox(g, boxRect.Location, state)
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

        ' Map ContentAlignment to TextFormatFlags
        Dim flags As TextFormatFlags = TextFormatFlags.NoPrefix 'Or TextFormatFlags.SingleLine 'Or TextFormatFlags.NoPadding

        Select Case TextAlign
            Case ContentAlignment.TopLeft, ContentAlignment.MiddleLeft, ContentAlignment.BottomLeft
                flags = flags Or TextFormatFlags.Left
            Case ContentAlignment.TopCenter, ContentAlignment.MiddleCenter, ContentAlignment.BottomCenter
                flags = flags Or TextFormatFlags.HorizontalCenter
            Case ContentAlignment.TopRight, ContentAlignment.MiddleRight, ContentAlignment.BottomRight
                flags = flags Or TextFormatFlags.Right
        End Select

        Select Case TextAlign
            Case ContentAlignment.TopLeft, ContentAlignment.TopCenter, ContentAlignment.TopRight
                flags = flags Or TextFormatFlags.Top
            Case ContentAlignment.MiddleLeft, ContentAlignment.MiddleCenter, ContentAlignment.MiddleRight
                flags = flags Or TextFormatFlags.VerticalCenter
            Case ContentAlignment.BottomLeft, ContentAlignment.BottomCenter, ContentAlignment.BottomRight
                flags = flags Or TextFormatFlags.Bottom
        End Select

        If RightToLeft = RightToLeft.Yes Then
            flags = flags Or TextFormatFlags.RightToLeft
        End If

        ' Draw Unicode text with native Windows font linking & shaping
        Dim textColor As Color = If(Me.Enabled, Me.ForeColor, SystemColors.GrayText)
        TextRenderer.DrawText(g, Text, Font, textRect, textColor, flags)

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

    Private Shared ReadOnly _glyphCache As New Dictionary(Of String, Bitmap)()
    Private Shared _themeHooked As Boolean = False

    Private Shared Sub EnsureThemeHook()
        If Not _themeHooked Then
            _themeHooked = True
            AddHandler Microsoft.Win32.SystemEvents.UserPreferenceChanged, AddressOf OnUserPreferenceChanged
        End If
    End Sub

    Private Shared Sub OnUserPreferenceChanged(sender As Object, e As Microsoft.Win32.UserPreferenceChangedEventArgs)
        ' Clear cached glyphs if Windows theme, colors, or DPI change
        For Each bmp As Bitmap In _glyphCache.Values
            bmp.Dispose()
        Next
        _glyphCache.Clear()
    End Sub

    Private Function GetScaledGlyph(g As Graphics, state As VisualStyles.CheckBoxState, glyphSize As Size, targetSize As Integer, bgColor As Color) As Bitmap
        EnsureThemeHook()

        Dim key As String = $"{CInt(state)}_{bgColor.ToArgb()}_{glyphSize.Width}x{glyphSize.Height}_{targetSize}"
        Dim cached As Bitmap = Nothing

        If _glyphCache.TryGetValue(key, cached) Then
            Return cached
        End If

        ' Render native high-DPI glyph onto a GDI-compatible buffer
        Dim glyphRect As New Rectangle(0, 0, glyphSize.Width, glyphSize.Height)
        Dim finalBmp As New Bitmap(targetSize, targetSize, Imaging.PixelFormat.Format24bppRgb)

        Using glyphBuf As BufferedGraphics = BufferedGraphicsManager.Current.Allocate(g, glyphRect)
            glyphBuf.Graphics.Clear(bgColor)
            CheckBoxRenderer.DrawCheckBox(glyphBuf.Graphics, Point.Empty, state)

            Using rawBmp As New Bitmap(glyphRect.Width, glyphRect.Height, Imaging.PixelFormat.Format24bppRgb)
                Using gRaw As Graphics = Graphics.FromImage(rawBmp)
                    glyphBuf.Render(gRaw)
                End Using

                ' Pre-scale once into the exact 13x13 target bitmap
                Using gFinal As Graphics = Graphics.FromImage(finalBmp)
                    gFinal.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    Using ia As New Imaging.ImageAttributes()
                        ia.SetWrapMode(Drawing2D.WrapMode.TileFlipXY)
                        gFinal.DrawImage(rawBmp, New Rectangle(0, 0, targetSize, targetSize), 0, 0, rawBmp.Width, rawBmp.Height, GraphicsUnit.Pixel, ia)
                    End Using
                End Using
            End Using
        End Using

        _glyphCache(key) = finalBmp
        Return finalBmp
    End Function



End Class
