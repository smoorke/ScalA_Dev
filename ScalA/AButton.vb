Public Class AButton
    Inherits Button
	Public Sub New(ByVal text As String, ByVal left As Integer, ByVal top As Integer, ByVal width As Integer, ByVal height As Integer)
		MyBase.New()

		Me.Text = text
		Me.Location = New Point(left, top)
		Me.Size = New Size(width, height)

		SetStyle(ControlStyles.AllPaintingInWmPaint Or
				 ControlStyles.Selectable Or
				 ControlStyles.DoubleBuffer Or
				 ControlStyles.ResizeRedraw Or
				 ControlStyles.SupportsTransparentBackColor Or
				 ControlStyles.UserPaint, True)
		Size = New Size(200, 150)
		Me.ImageAlign = ContentAlignment.TopRight
		Me.BackgroundImageLayout = ImageLayout.None
		Me.Padding = New Padding(0)
		Me.Margin = New Padding(0)
		Me.TextAlign = ContentAlignment.TopCenter
		Me.Font = New Font("Microsoft Sans Serif", 8.25)
		Me.Visible = False
	End Sub

	Private _passthrough As Rectangle
	Public ReadOnly Property ThumbRectangle() As Rectangle
		Get
			Return _passthrough
		End Get
	End Property
	Public ReadOnly Property ThumbRECT() As Rectangle
		Get ' New Rectangle(pnlStartup.Left + but.Left + 3, pnlStartup.Top + but.Top + 21, but.Right - 2, pnlStartup.Top + but.Bottom - 3)
			Return New Rectangle(Me.Parent.Left + Me.Left + 3, Me.Parent.Top + Me.Top + 21, Me.Right - 2, Me.Parent.Top + Me.Bottom - 3)
		End Get
	End Property
	Public ReadOnly Property ThumbContains(screenPt As Point) As Boolean
		Get
			Dim clientpt As Point = Me.FindForm.PointToClient(screenPt)
			If New Rectangle(ThumbRECT.X, ThumbRECT.Y, ThumbRECT.Width - ThumbRECT.X, ThumbRECT.Height - ThumbRECT.Y).Contains(clientpt) Then
				'Debug.Print($"contains = True")
				Return True
			Else
				Return False
			End If
		End Get
	End Property

	Protected Overrides Sub OnResize(e As EventArgs)
		MyBase.OnResize(e)
		_passthrough = New Rectangle(3, 21, Me.Width - 6, Me.Height - 24)
	End Sub


	Public Shared ActiveOverview As Boolean = True
	Protected Overrides Sub OnPaint(e As PaintEventArgs)
		MyBase.OnPaint(e)
		If ActiveOverview AndAlso My.Settings.gameOnOverview AndAlso Me.Text <> "" Then
			Using b As SolidBrush = New SolidBrush(Color.Magenta)
				e.Graphics.FillRectangle(b, Me.ThumbRectangle)
			End Using
		End If
	End Sub
End Class
