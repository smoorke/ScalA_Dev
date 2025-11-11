Imports System.ComponentModel
Public NotInheritable Class ThemedControls
    'dummy class
End Class
Public NotInheritable Class ThemedComboBox
    Inherits ComboBox

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads Property ForeColor As Color
        Get
            Return _ForeColor
        End Get
        Set(value As Color)
            If ForeColor <> value Then
                _ForeColor = value
                _buffer = Nothing
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
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads Property DropDownStyle As ComboBoxStyle

    Private _buffer As Bitmap
    Private _theme As Boolean = False
    Private _ForeColor As Color = Color.White

    Public WriteOnly Property DarkTheme() As Boolean
        Set(ByVal value As Boolean)
            If value = _theme Then Exit Property
            _theme = value
            _buffer = Nothing
            Invalidate()
        End Set
    End Property

    Public Sub New()
        MyBase.New()
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        DrawMode = DrawMode.OwnerDrawVariable
        'MyBase.FlatStyle = FlatStyle.Flat
        MyBase.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then _buffer = Nothing
        MyBase.Dispose(disposing)
    End Sub

    Protected Overrides Sub OnTabStopChanged(ByVal e As EventArgs)
        MyBase.OnTabStopChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnTabIndexChanged(ByVal e As EventArgs)
        MyBase.OnTabIndexChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As EventArgs)
        MyBase.OnGotFocus(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnLostFocus(ByVal e As EventArgs)
        MyBase.OnLostFocus(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnTextUpdate(ByVal e As EventArgs)
        MyBase.OnTextUpdate(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnSelectedValueChanged(ByVal e As EventArgs)
        MyBase.OnSelectedValueChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnInvalidated(ByVal e As InvalidateEventArgs)
        MyBase.OnInvalidated(e)
        PaintCombobox()
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        _buffer = Nothing
        Invalidate()
    End Sub

    Private Sub PaintCombobox()

        If _buffer Is Nothing Then _buffer = New Bitmap(ClientRectangle.Width, ClientRectangle.Height)

        Using g = Graphics.FromImage(_buffer)
            Dim rect = New Rectangle(0, 0, ClientSize.Width, ClientSize.Height)
            'Dim textColor = If(_theme, Colors.LightText, Color.Black)
            Dim textColor = _ForeColor
            Dim borderColor = If(_theme, Colors.GreySelection, Color.FromArgb(&HFFADADAD))
            Dim fillColor = If(_theme, Colors.LightBackground, Color.FromArgb(&HFFE1E1E1))
            If Focused AndAlso TabStop Then borderColor = If(_theme, Colors.BlueHighlight, Color.FromArgb(&HFF005499))

            Using b = New SolidBrush(fillColor)
                g.FillRectangle(b, rect)
            End Using

            Using p = New Pen(borderColor, 1)
                Dim modRect = New Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1)
                g.DrawRectangle(p, modRect)
            End Using

            Dim arrow = My.Resources.scrollbar_arrow_small_hot
            g.DrawImageUnscaled(arrow, rect.Right - arrow.Width - (Consts.Padding / 2), (rect.Height / 2) - (arrow.Height / 2))
            Dim text As String = If(SelectedItem IsNot Nothing, GetItemText(SelectedItem).TrimStart, "")

            Using b = New SolidBrush(textColor)
                Dim padding = 2
                Dim modRect = New Rectangle(rect.Left + padding, rect.Top + padding, rect.Width - arrow.Width - (Consts.Padding / 2) - (padding * 2), rect.Height - (padding * 2))
                Dim stringFormat = New StringFormat With {
                    .LineAlignment = StringAlignment.Center,
                    .Alignment = StringAlignment.Near,
                    .FormatFlags = StringFormatFlags.NoWrap,
                    .Trimming = StringTrimming.None
                }
                g.DrawString(text, Font, b, modRect, stringFormat)
            End Using
        End Using
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If _buffer Is Nothing Then PaintCombobox()
        Dim g = e.Graphics
        g.DrawImageUnscaled(_buffer, Point.Empty)
    End Sub

    Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)
        Dim g = e.Graphics
        Dim rect = e.Bounds
        DropDownHeight = rect.Height * Items.Count + 2
        If rect.X = 3 Then Exit Sub
        Dim textColor = If(_theme, Colors.LightText, Color.Black)
        Dim fillColor = If(_theme, Colors.LightBackground, Color.White)
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected OrElse
           (e.State And DrawItemState.Focus) = DrawItemState.Focus OrElse
           (e.State And DrawItemState.NoFocusRect) <> DrawItemState.NoFocusRect Then
            fillColor = If(_theme, Colors.BlueSelection, Color.FromArgb(&HFF0078D7))
            If Not _theme Then textColor = Color.White
        End If

        Using b = New SolidBrush(fillColor)
            g.FillRectangle(b, rect)
        End Using

        If e.Index >= 0 AndAlso e.Index < Items.Count Then
            Dim leftpad As Integer = 2
            Dim ap As AstoniaProcess = Nothing
            If TypeOf Items(e.Index) Is AstoniaProcess Then ap = DirectCast(Items(e.Index), AstoniaProcess)
            If ap IsNot Nothing AndAlso e.Index > 0 Then
                Using bmp = ap.GetIcon?.ToBitmap
                    If bmp IsNot Nothing Then
                        leftpad = rect.Height
                        g.DrawImage(bmp, rect.Left, rect.Top, rect.Height, rect.Height)
                    End If
                End Using
            End If
            Dim padding = 2
            Dim text = GetItemText(Items(e.Index))
            Using b = New SolidBrush(textColor)
                Dim modRect = New Rectangle(rect.Left + leftpad, rect.Top + padding, rect.Width - (padding * 2), rect.Height - (padding * 2))
                Dim stringFormat = New StringFormat With {
                .LineAlignment = StringAlignment.Center,
                .Alignment = StringAlignment.Near,
                .FormatFlags = StringFormatFlags.NoWrap,
                .Trimming = StringTrimming.None
            }
                g.DrawString(text, Font, b, modRect, stringFormat)
            End Using
        End If
    End Sub


    Overloads Function DropDownContains(screenPt As Point) As Boolean
        screenPt = Me.FindForm?.PointToClient(screenPt)
        Return (Me.DroppedDown AndAlso
                New Rectangle(Me.Left, Me.Bottom, Me.Width, Me.DropDownHeight).Contains(screenPt))
    End Function

End Class


<ToolboxBitmap(GetType(Button))>
<DefaultEvent("Click")>
Public NotInheritable Class ThemedStartButton
    Inherits Button

    Private _style As DarkButtonStyle = DarkButtonStyle.Normal
    Private _buttonState As DarkControlState = DarkControlState.Normal

    Private _spacePressed As Boolean
    Private ReadOnly _padding As Integer = Consts.Padding / 2
    Private _imagePadding As Integer = 5

    Private _theme As Boolean = False
    Public WriteOnly Property DarkTheme() As Boolean
        Set(ByVal value As Boolean)
            If value = _theme Then Exit Property
            _theme = value
            Invalidate()
        End Set
    End Property

    Public Overloads Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            Invalidate()
        End Set
    End Property

    Public Overloads Property Enabled As Boolean
        Get
            Return MyBase.Enabled
        End Get
        Set(ByVal value As Boolean)
            MyBase.Enabled = value
            Invalidate()
        End Set
    End Property

    <Category("Appearance")>
    <Description("Determines the style of the button.")>
    <DefaultValue(DarkButtonStyle.Normal)>
    Public Property ButtonStyle As DarkButtonStyle
        Get
            Return _style
        End Get
        Set(ByVal value As DarkButtonStyle)
            _style = value
            Invalidate()
        End Set
    End Property

    <Category("Appearance")>
    <Description("Determines the amount of padding between the image and text.")>
    <DefaultValue(5)>
    Public Property ImagePadding As Integer
        Get
            Return _imagePadding
        End Get
        Set(ByVal value As Integer)
            _imagePadding = value
            Invalidate()
        End Set
    End Property

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property AutoEllipsis As Boolean
        Get
            Return False
        End Get
    End Property

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property ButtonState As DarkControlState
        Get
            Return _buttonState
        End Get
    End Property

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property ImageAlign As ContentAlignment
        Get
            Return MyBase.ImageAlign
        End Get
    End Property

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property FlatAppearance As Boolean
        Get
            Return False
        End Get
    End Property

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property FlatStyle As FlatStyle
        Get
            Return MyBase.FlatStyle
        End Get
    End Property

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property TextAlign As ContentAlignment
        Get
            Return MyBase.TextAlign
        End Get
    End Property

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property UseCompatibleTextRendering As Boolean
        Get
            Return False
        End Get
    End Property

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property UseVisualStyleBackColor As Boolean
        Get
            Return False
        End Get
    End Property

    Public Sub New()
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        MyBase.UseVisualStyleBackColor = False
        MyBase.UseCompatibleTextRendering = False
        SetButtonState(DarkControlState.Normal)
        Padding = New Padding(_padding)
    End Sub

    Private Sub SetButtonState(ByVal buttonState As DarkControlState)
        If _buttonState <> buttonState Then
            _buttonState = buttonState
            'Invalidate()
        End If
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If _spacePressed Then Return

        If e.Button = MouseButtons.Left Then

            If ClientRectangle.Contains(e.Location) Then
                SetButtonState(DarkControlState.Pressed)
            Else
                SetButtonState(DarkControlState.Hover)
            End If
        Else
            SetButtonState(DarkControlState.Hover)
        End If
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If e.Button <> MouseButtons.Left Then Return
        MyBase.OnMouseDown(e)
        If Not ClientRectangle.Contains(e.Location) Then Return
        SetButtonState(DarkControlState.Pressed)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        If e.Button <> MouseButtons.Left Then Return
        MyBase.OnMouseUp(e)
        If _spacePressed Then Return
        SetButtonState(DarkControlState.Normal)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        If _spacePressed Then Return
        SetButtonState(DarkControlState.Normal)
    End Sub

    Protected Overrides Sub OnMouseCaptureChanged(ByVal e As EventArgs)
        MyBase.OnMouseCaptureChanged(e)
        If _spacePressed Then Return
        Dim location = Cursor.Position
        If Not ClientRectangle.Contains(location) Then SetButtonState(DarkControlState.Normal)
    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As EventArgs)
        MyBase.OnGotFocus(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnLostFocus(ByVal e As EventArgs)
        MyBase.OnLostFocus(e)
        _spacePressed = False
        Dim location = Cursor.Position

        If Not ClientRectangle.Contains(location) Then
            SetButtonState(DarkControlState.Normal)
        Else
            SetButtonState(DarkControlState.Hover)
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        If e.KeyCode = Keys.Space Then
            _spacePressed = True
            SetButtonState(DarkControlState.Pressed)
        End If
    End Sub

    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs)
        MyBase.OnKeyUp(e)

        If e.KeyCode = Keys.Space Then
            _spacePressed = False
            Dim location = Me.PointToClient(Cursor.Position)

            If Not ClientRectangle.Contains(location) Then
                SetButtonState(DarkControlState.Normal)
            Else
                SetButtonState(DarkControlState.Hover)
            End If
        End If
    End Sub


    Protected Overrides Sub OnForeColorChanged(e As EventArgs)
        MyBase.OnForeColorChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim g = e.Graphics
        Dim rect = New Rectangle(0, 0, ClientSize.Width, ClientSize.Height)
        Dim textColor = ForeColor
        Dim borderColor = If(_theme, Colors.GreySelection, Color.FromArgb(&HFFADADAD))
        Dim fillColor As Color = If(_theme, Colors.LightBackground, Color.FromArgb(&HFFE1E1E1))

        If Focused AndAlso TabStop Then borderColor = If(_theme, Colors.BlueHighlight, Color.FromArgb(&HFF0078D7))

        Select Case ButtonState
            Case DarkControlState.Hover
                fillColor = If(_theme, Colors.LighterBackground, Color.FromArgb(&HFFE5F1FB))
            Case DarkControlState.Pressed
                fillColor = If(_theme, Colors.DarkBackground, Color.FromArgb(&HFFCCE4F7))
        End Select


        Using b = New SolidBrush(fillColor)
            g.FillRectangle(b, rect)
        End Using

        If ButtonStyle = DarkButtonStyle.Normal Then

            Using p = New Pen(borderColor, 1)
                Dim modRect = New Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1)
                g.DrawRectangle(p, modRect)
            End Using
        End If
        If Image IsNot Nothing Then
            g.DrawImage(Image, New Rectangle(1, 2, Me.Width - 2, Me.Height - 3), Image.GetBounds(GraphicsUnit.Pixel), GraphicsUnit.Pixel)
        Else
            Using p = New Pen(textColor, 1)
                Dim modRect = New Rectangle(rect.Left + 4, rect.Top + 4, rect.Width - 9, rect.Height - 9)
                g.DrawRectangle(p, modRect)
                g.DrawLine(p, New Point(modRect.Left + modRect.Width / 2, modRect.Top), New Point(modRect.Left + modRect.Width / 2, modRect.Bottom))
                g.DrawLine(p, New Point(modRect.Left, modRect.Top + modRect.Height / 2), New Point(modRect.Right, modRect.Top + modRect.Height / 2))
            End Using
        End If
    End Sub
End Class

Public Enum DarkButtonStyle
    Normal
    Flat
End Enum
Public Enum DarkControlState
    Normal
    Hover
    Pressed
End Enum


Public NotInheritable Class Colors
    Public Shared ReadOnly Property GreyBackground As Color
        Get
            Return Color.FromArgb(60, 63, 65)
        End Get
    End Property

    Public Shared ReadOnly Property HeaderBackground As Color
        Get
            Return Color.FromArgb(57, 60, 62)
        End Get
    End Property

    Public Shared ReadOnly Property BlueBackground As Color
        Get
            Return Color.FromArgb(66, 77, 95)
        End Get
    End Property

    Public Shared ReadOnly Property DarkBlueBackground As Color
        Get
            Return Color.FromArgb(52, 57, 66)
        End Get
    End Property

    Public Shared ReadOnly Property DarkBackground As Color
        Get
            Return Color.FromArgb(43, 43, 43)
        End Get
    End Property

    Public Shared ReadOnly Property MediumBackground As Color
        Get
            Return Color.FromArgb(49, 51, 53)
        End Get
    End Property

    Public Shared ReadOnly Property LightBackground As Color
        Get
            Return Color.FromArgb(69, 73, 74)
        End Get
    End Property

    Public Shared ReadOnly Property LighterBackground As Color
        Get
            Return Color.FromArgb(95, 101, 102)
        End Get
    End Property

    Public Shared ReadOnly Property LightestBackground As Color
        Get
            Return Color.FromArgb(178, 178, 178)
        End Get
    End Property

    Public Shared ReadOnly Property LightBorder As Color
        Get
            Return Color.FromArgb(81, 81, 81)
        End Get
    End Property

    Public Shared ReadOnly Property DarkBorder As Color
        Get
            Return Color.FromArgb(51, 51, 51)
        End Get
    End Property

    Public Shared ReadOnly Property LightText As Color
        Get
            Return Color.FromArgb(220, 220, 220)
        End Get
    End Property

    Public Shared ReadOnly Property DisabledText As Color
        Get
            Return Color.FromArgb(153, 153, 153)
        End Get
    End Property

    Public Shared ReadOnly Property BlueHighlight As Color
        Get
            Return Color.FromArgb(104, 151, 187)
        End Get
    End Property

    Public Shared ReadOnly Property BlueSelection As Color
        Get
            Return Color.FromArgb(75, 110, 175)
        End Get
    End Property

    Public Shared ReadOnly Property GreyHighlight As Color
        Get
            Return Color.FromArgb(122, 128, 132)
        End Get
    End Property

    Public Shared ReadOnly Property GreySelection As Color
        Get
            Return Color.FromArgb(92, 92, 92)
        End Get
    End Property

    Public Shared ReadOnly Property DarkGreySelection As Color
        Get
            Return Color.FromArgb(82, 82, 82)
        End Get
    End Property

    Public Shared ReadOnly Property DarkBlueBorder As Color
        Get
            Return Color.FromArgb(51, 61, 78)
        End Get
    End Property

    Public Shared ReadOnly Property LightBlueBorder As Color
        Get
            Return Color.FromArgb(86, 97, 114)
        End Get
    End Property

    Public Shared ReadOnly Property ActiveControl As Color
        Get
            Return Color.FromArgb(159, 178, 196)
        End Get
    End Property
End Class
Public NotInheritable Class Consts
    Public Shared Padding As Integer = 10
    Public Shared ScrollBarSize As Integer = 15
    Public Shared ArrowButtonSize As Integer = 15
    Public Shared MinimumThumbSize As Integer = 11
    Public Shared CheckBoxSize As Integer = 12
    Public Shared RadioButtonSize As Integer = 12
    Public Const ToolWindowHeaderSize As Integer = 25
    Public Const DocumentTabAreaSize As Integer = 24
    Public Const ToolWindowTabAreaSize As Integer = 21
End Class
