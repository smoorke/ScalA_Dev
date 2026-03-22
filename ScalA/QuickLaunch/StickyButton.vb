
Public Class StickyButtonLabel
    Public Up As Boolean
    Public label As Label
    Public Tag As Object

    Private _OrgImage As Bitmap
    Private _DimImage As Bitmap

    Private _stable As Boolean = False
    Public Property Stable As Boolean
        Get
            Return _stable
        End Get
        Set(value As Boolean)
            _stable = value
            ' Trigger EnabledChanged check once stable
            OnEnabledChanged(label, EventArgs.Empty)
        End Set
    End Property

    Public ReadOnly Property Handle As IntPtr
        Get
            Return label.Handle
        End Get
    End Property


    ' -----------------------------
    ' Constructor
    ' -----------------------------

    Public Sub New(label As Label, Optional up As Boolean? = Nothing)
        Me.label = label
        Me._OrgImage = label.Image.Clone
        If Not My.Settings.DarkMode Then
            Me._DimImage = CType(label.Image.Clone, Bitmap).AsDimmed(1.4)
        End If
        If up IsNot Nothing Then
            Me.Up = up
        Else
            Dim upFound As Boolean = False
            For Each ctrl As Label In label.Parent?.Controls.OfType(Of Label)
                If ctrl IsNot Nothing Then
                    If ctrl Is label Then
                        Me.Up = Not upFound ' first label = up
                        Exit For
                    End If
                    upFound = True
                End If
            Next
        End If

#If DEBUG Then
        AddHandler Me.label.EnabledChanged, AddressOf OnEnabledChanged
#End If
    End Sub
    ' Cast a Label to StickyButtonLabel
    Public Shared Widening Operator CType(lab As Label) As StickyButtonLabel
        Return New StickyButtonLabel(lab)
    End Operator

    ' Optional: Cast StickyButtonLabel back to Label
    Public Shared Widening Operator CType(wrap As StickyButtonLabel) As Label
        Return wrap.label
    End Operator

#Region "EventHandler Signature Events"

    ' Lazy-forwarded EnabledChanged
    Private _enabledHandlers As EventHandler = Nothing
    Public Custom Event EnabledChanged As EventHandler
        AddHandler(value As EventHandler)
            If _enabledHandlers Is Nothing Then
                AddHandler label.EnabledChanged, AddressOf OnEnabledChanged
            End If
            _enabledHandlers = DirectCast([Delegate].Combine(_enabledHandlers, value), EventHandler)
        End AddHandler
        RemoveHandler(value As EventHandler)
            _enabledHandlers = DirectCast([Delegate].Remove(_enabledHandlers, value), EventHandler)
            If _enabledHandlers Is Nothing Then
                RemoveHandler label.EnabledChanged, AddressOf OnEnabledChanged
            End If
        End RemoveHandler
        RaiseEvent(sender As Object, e As EventArgs)
            _enabledHandlers?.Invoke(sender, e)
        End RaiseEvent
    End Event

    Private EnabledState? As Boolean = Nothing
    Private Sub OnEnabledChanged(sender As Object, e As EventArgs)
        If Stable AndAlso label.Visible AndAlso (EnabledState Is Nothing OrElse EnabledState <> label.Enabled) Then
            EnabledState = label.Enabled
            Debug.Print($"EnabledChanged {If(Up, "up", "dn")} {label.Enabled}")
            If Not My.Settings.DarkMode Then
                If label.Enabled Then
                    Me.Image = _OrgImage
                Else
                    Me.Image = _DimImage
                End If
            End If
            'RaiseEvent EnabledChanged(Me, e)
        End If
    End Sub

    ' Lazy-forwarded VisibleChanged
    Private _visibleHandlers As EventHandler = Nothing
    Public Custom Event VisibleChanged As EventHandler
        AddHandler(value As EventHandler)
            If _visibleHandlers Is Nothing Then
                AddHandler label.VisibleChanged, AddressOf OnVisibleChanged
            End If
            _visibleHandlers = DirectCast([Delegate].Combine(_visibleHandlers, value), EventHandler)
        End AddHandler
        RemoveHandler(value As EventHandler)
            _visibleHandlers = DirectCast([Delegate].Remove(_visibleHandlers, value), EventHandler)
            If _visibleHandlers Is Nothing Then
                RemoveHandler label.VisibleChanged, AddressOf OnVisibleChanged
            End If
        End RemoveHandler
        RaiseEvent(sender As Object, e As EventArgs)
            _visibleHandlers?.Invoke(sender, e)
        End RaiseEvent
    End Event

    Private Sub OnVisibleChanged(sender As Object, e As EventArgs)
        RaiseEvent VisibleChanged(Me, e)
    End Sub

#End Region

#Region "MouseEventHandler Signature Events"

    Private _mouseDownHandlers As MouseEventHandler = Nothing
    Public Custom Event MouseDown As MouseEventHandler
        AddHandler(value As MouseEventHandler)
            If _mouseDownHandlers Is Nothing Then
                AddHandler label.MouseDown, AddressOf OnMouseDown
            End If
            _mouseDownHandlers = DirectCast([Delegate].Combine(_mouseDownHandlers, value), MouseEventHandler)
        End AddHandler
        RemoveHandler(value As MouseEventHandler)
            _mouseDownHandlers = DirectCast([Delegate].Remove(_mouseDownHandlers, value), MouseEventHandler)
            If _mouseDownHandlers Is Nothing Then
                RemoveHandler label.MouseDown, AddressOf OnMouseDown
            End If
        End RemoveHandler
        RaiseEvent(sender As Object, e As MouseEventArgs)
            _mouseDownHandlers?.Invoke(sender, e)
        End RaiseEvent
    End Event

    Private Sub OnMouseDown(sender As Object, e As MouseEventArgs)
        RaiseEvent MouseDown(Me, e)
    End Sub

#End Region

#Region "Forwarded Properties"

    Public Property Enabled As Boolean
        Get
            Return label.Enabled
        End Get
        Set(value As Boolean)
            label.Enabled = value
        End Set
    End Property

    Public Property Image As Image
        Get
            Return label.Image
        End Get
        Set(value As Image)
            label.Image = value
        End Set
    End Property

    Public Property BackColor As Color
        Get
            Return label.BackColor
        End Get
        Set(value As Color)
            label.BackColor = value
        End Set
    End Property

    Public Property Visible As Boolean
        Get
            Return label.Visible
        End Get
        Set(value As Boolean)
            label.Visible = value
        End Set
    End Property

#End Region

End Class

Module ext
    <Runtime.CompilerServices.Extension()>
    Public Function AsDimmed(ByVal img As Image, ByVal factor As Single) As Image
        ' factor: 0 = no change, 1 = fully dimmed to middle gray
        Try
            Dim bmp = New Bitmap(img.Width, img.Height)
            Using gfx = Graphics.FromImage(bmp), attr As New Imaging.ImageAttributes()
                ' Create a matrix that moves colors toward 0.5 (middle gray)
                Dim scale As Single = 1.0F - factor
                Dim offset As Single = factor * 0.5F
                Dim cm As New Imaging.ColorMatrix(
                    New Single()() {
                        New Single() {scale, 0, 0, 0, 0},
                        New Single() {0, scale, 0, 0, 0},
                        New Single() {0, 0, scale, 0, 0},
                        New Single() {0, 0, 0, 1, 0},
                        New Single() {offset, offset, offset, 0, 1}
                    })
                attr.SetColorMatrix(cm)
                gfx.DrawImage(img, New Rectangle(0, 0, bmp.Width, bmp.Height),
                              0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attr)
            End Using
            Return bmp
        Catch ex As Exception
            Debug.Print($"Exception in AsDimmed: {ex.Message}")
            Return Nothing
        End Try
    End Function
End Module