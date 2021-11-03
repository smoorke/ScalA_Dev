Module DWMThumb
    <Flags()> Public Enum DwmThumbnailFlags As UInteger
        DWM_TNP_RECTDESTINATION = &H1
        DWM_TNP_RECTSOURCE = &H2
        DWM_TNP_OPACITY = &H4
        DWM_TNP_VISIBLE = &H8
        DWM_TNP_SOURCECLIENTAREAONLY = &H10
    End Enum

    <System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)>
    Public Structure DWM_THUMBNAIL_PROPERTIES
        Public dwFlags As DwmThumbnailFlags
        Public rcDestination As Rectangle
        Public rcSource As Rectangle
        Public opacity As Byte
        Public fVisible As Boolean
        Public fSourceClientAreaOnly As Boolean
    End Structure

    Public Declare Function DwmRegisterThumbnail Lib "dwmapi.dll" (ByVal Dest As IntPtr, ByVal Src As IntPtr, ByRef Thumb As IntPtr) As Integer
    Public Declare Function DwmUpdateThumbnailProperties Lib "dwmapi.dll" (ByVal hThumbnail As IntPtr, ByRef props As DWM_THUMBNAIL_PROPERTIES) As Integer
    Public Declare Function DwmUnregisterThumbnail Lib "dwmapi.dll" (ByVal Thumb As IntPtr) As Integer

End Module

Public Class DWMAPI
    'dummy class to prevent form being generated
End Class

Partial Public Class FrmMain
    Dim thumb As IntPtr = IntPtr.Zero
    Private Sub CreateThumb()

        If AltPP IsNot Nothing Then
            DwmUnregisterThumbnail(thumb)
            DwmRegisterThumbnail(Me.Handle, AltPP.MainWindowHandle, thumb)
        End If

    End Sub

    Public Sub UpdateThumb(opacity As Byte)
        Dim twp As DWM_THUMBNAIL_PROPERTIES
        twp.dwFlags = DwmThumbnailFlags.DWM_TNP_OPACITY + DwmThumbnailFlags.DWM_TNP_RECTDESTINATION + DwmThumbnailFlags.DWM_TNP_SOURCECLIENTAREAONLY + DwmThumbnailFlags.DWM_TNP_VISIBLE
        twp.opacity = opacity
        twp.fVisible = True
        twp.rcDestination = New Rectangle(pbZoom.Left, pbZoom.Top, pbZoom.Right, pbZoom.Bottom)
        twp.fSourceClientAreaOnly = True

        DwmUpdateThumbnailProperties(thumb, twp)
    End Sub


    Public Sub AnimateThumb(startRC As Rectangle, endRC As Rectangle, Optional time As Integer = 50)
        Dim timer As New Stopwatch
        timer.Start()

        Dim twp As DWM_THUMBNAIL_PROPERTIES
        twp.dwFlags = DwmThumbnailFlags.DWM_TNP_OPACITY + DwmThumbnailFlags.DWM_TNP_RECTDESTINATION + DwmThumbnailFlags.DWM_TNP_SOURCECLIENTAREAONLY + DwmThumbnailFlags.DWM_TNP_VISIBLE
        twp.fVisible = True
        twp.opacity = 255
        twp.rcDestination = startRC
        twp.fSourceClientAreaOnly = True

        'DwmUpdateThumbnailProperties(thumb, twp)

        While timer.ElapsedMilliseconds < time
            Dim percent = timer.ElapsedMilliseconds / time
            ' Debug.Print($"{percent * 100}%")
            twp.rcDestination = New Rectangle(startRC.X + (endRC.X - startRC.X) * percent,
                                              startRC.Y + (endRC.Y - startRC.Y) * percent,
                                              startRC.Width + (endRC.Width - startRC.Width) * percent,
                                              startRC.Height + (endRC.Height - startRC.Height) * percent)
            DwmUpdateThumbnailProperties(thumb, twp)
        End While

        timer.Stop()

        twp.rcDestination = endRC
        DwmUpdateThumbnailProperties(thumb, twp)

    End Sub

End Class
