﻿Module DWMThumb
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
    Public Declare Function DwmQueryThumbnailSourceSize Lib "dwmapi.dll" (hThumbnail As IntPtr, ByRef pSize As Size) As Integer

End Module

Public NotInheritable Class DWMAPI
    'dummy class to prevent form being generated
End Class

Partial Public NotInheritable Class FrmMain
    Public thumb As IntPtr = IntPtr.Zero
    Public Function CreateThumb() As Integer

        If AltPP IsNot Nothing Then
            Dim oldThumb = thumb
            Dim ret = DwmRegisterThumbnail(ScalaHandle, AltPP.MainWindowHandle, thumb)
            DwmUnregisterThumbnail(oldThumb)
            Return ret
        End If
        Return -1
    End Function
    Private prevMode As Integer = 0
    Public Sub UpdateThumb(opacity As Byte)
        If AltPP?.Id = 0 Then Exit Sub

        Dim twp As DWM_THUMBNAIL_PROPERTIES
        twp.dwFlags = DwmThumbnailFlags.DWM_TNP_OPACITY Or
                      DwmThumbnailFlags.DWM_TNP_RECTDESTINATION Or
                      DwmThumbnailFlags.DWM_TNP_SOURCECLIENTAREAONLY Or
                      DwmThumbnailFlags.DWM_TNP_VISIBLE
        'DwmThumbnailFlags.DWM_TNP_SOURCECLIENTAREAONLY 
        twp.opacity = opacity
        twp.fVisible = True
        'twp.rcSource = New Rectangle(AltPP.ClientOffset.X, AltPP.ClientOffset.Y, rcC.Width + AltPP.ClientOffset.X, rcC.Height + AltPP.ClientOffset.Y)
        twp.rcDestination = New Rectangle(pbZoom.Left, pbZoom.Top, pbZoom.Right, pbZoom.Bottom)

        Dim mode = My.Settings.ScalingMode '0 auto, 1 blur, 2 pixel
        If My.Settings.ScalingMode = 0 Then
            Dim compsz As Size = pbZoom.Size
            Debug.Print($"UpdateThumb pbzoom {pbZoom.Size}")
            If (compsz.Width / AltPP.ClientRect.Width >= 2) AndAlso
               (compsz.Height / AltPP.ClientRect.Height >= 2) Then
                mode = 2
            Else
                mode = 1
            End If
        End If

        If Me.WindowsScaling <> 100 Then 'handle windows scaling
            Debug.Print("scaling not 100: pixel mode disabled")
            mode = 1
            twp.rcDestination = New Rectangle(pbZoom.Left, pbZoom.Top - 1, pbZoom.Right, pbZoom.Bottom)
        End If

        If mode = 1 Then
            twp.fSourceClientAreaOnly = True
        Else
            twp.fSourceClientAreaOnly = False
            twp.dwFlags = twp.dwFlags Or DwmThumbnailFlags.DWM_TNP_RECTSOURCE
            twp.rcSource = AltPP.rcSource(pbZoom.Size, mode)
        End If

        If prevMode <> mode Then
            Dim oldThumb = thumb
            DwmRegisterThumbnail(ScalaHandle, AltPP.MainWindowHandle, thumb)
            DwmUpdateThumbnailProperties(thumb, twp)
            DwmUnregisterThumbnail(oldThumb)
            prevMode = mode
        Else
            DwmUpdateThumbnailProperties(thumb, twp)
        End If
        Debug.Print($"Thumb {thumb}")
    End Sub


    Public Sub AnimateThumb(startRC As Rectangle, endRC As Rectangle, Optional time As Integer = 50)
        Task.Run(Sub()
                     Dim timer As Stopwatch = Stopwatch.StartNew

                     Dim twp As DWM_THUMBNAIL_PROPERTIES
                     twp.dwFlags = DwmThumbnailFlags.DWM_TNP_OPACITY Or
                                   DwmThumbnailFlags.DWM_TNP_RECTDESTINATION Or
                                   DwmThumbnailFlags.DWM_TNP_SOURCECLIENTAREAONLY Or
                                   DwmThumbnailFlags.DWM_TNP_VISIBLE

                     twp.fSourceClientAreaOnly = True

                     'twp.rcSource = AltPP.ClientRect

                     twp.fVisible = True
                     twp.opacity = If(chkDebug.Checked, 128, 255)
                     twp.rcDestination = startRC

                     ' New Rectangle(AltPP.ClientOffset.X, AltPP.ClientOffset.Y, rcC.Width + AltPP.ClientOffset.X, rcC.Height + AltPP.ClientOffset.Y)

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

                     UpdateThumb(If(chkDebug.Checked, 128, 255))

                 End Sub)
    End Sub

End Class
