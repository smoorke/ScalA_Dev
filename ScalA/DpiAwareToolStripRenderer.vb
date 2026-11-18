Public Class DpiAwareToolStripRenderer : Inherits ToolStripProfessionalRenderer
    Public Sub New(colortable As ProfessionalColorTable)
        MyBase.New(colortable)
    End Sub

    Protected Overrides Sub OnRenderItemCheck(e As ToolStripItemImageRenderEventArgs)
        'render normal check to get correct border
        MyBase.OnRenderItemCheck(e)
        'calculate scale
        Dim scale As Single = e.Item.GetCurrentParent().ImageScalingSize.Width / 16.0!
        If scale = 1.0! Then Exit Sub

        Dim g As Graphics = e.Graphics
        Dim state = g.Save()

        Try
            Dim rect As Rectangle = e.ImageRectangle
            'Set the clip region FIRST while still in normal, unscaled coordinates
            g.SetClip(rect)
            'Translate to the check area and scale
            g.TranslateTransform(rect.X, rect.Y - scale)
            g.ScaleTransform(scale, scale)
            g.TranslateTransform(-rect.X, -rect.Y + scale)
            'Render using base method. borders will be clipped. but already drawn by first render
            MyBase.OnRenderItemCheck(e)
        Finally
            g.Restore(state)
        End Try
    End Sub
    Private Shared Function EvenInt(dbl As Single) As Integer
        Dim int As Integer = CInt(dbl)
        If int Mod 2 <> 0 Then int += 1
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
        Dim tri = GetTriangle(e.Item.GetCurrentParent().ImageScalingSize.Width / 16.0!)
        e.Graphics.DrawImageUnscaled(tri, New Rectangle(e.ArrowRectangle.Right - tri.Width - 3, (e.ArrowRectangle.Height + 1.5) \ 2 - tri.Height \ 2, tri.Width, tri.Height))
    End Sub

End Class
