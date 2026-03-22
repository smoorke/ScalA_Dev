Imports System.ComponentModel

Public NotInheritable Class Dummy_QL
    'dummy class to prevent desiger from treating this as a component
End Class

''' <summary>
''' Custom ContextMenuStrip for the QuickLaunch menu with custom rendering
''' </summary>
NotInheritable Class QuickLaunch : Inherits ContextMenuStrip

    Public Sub New()
        Me.Renderer = New CustomToolStripRenderer()
    End Sub


    Protected Overrides Sub OnOpening(e As CancelEventArgs)
        MyBase.OnOpening(e)
    End Sub

    Protected Overrides Sub OnOpened(e As EventArgs)
        MyBase.OnOpened(e)
    End Sub

    Protected Overrides Sub OnDragOver(drgevent As DragEventArgs)
        MyBase.OnDragOver(drgevent)
    End Sub

End Class

''' <summary>
''' Custom ToolStripMenuItem for QuickLaunch with right-click tracking and click cancellation support
''' </summary>
Public NotInheritable Class QLMenuItem : Inherits ToolStripMenuItem

    ''' <summary>
    ''' When True, prevents the click event from firing (used during drag operations)
    ''' </summary>
    Public CancelClick As Boolean = False

    ''' <summary>
    ''' Tracks which menu item received the last right mouse button down event
    ''' </summary>
    Public Shared RMouseDownOn As QLMenuItem

    Public Sub New(text As String, Optional ico As Image = Nothing)
        MyBase.New(text, ico)
        'Me.DropDown?.Dispose()
        'Me.DropDown = New ThemedScrollableDropDownMenu() With {.Renderer = FrmMain.cmsQuickLaunch.Renderer, .Font = FrmMain.cmsQuickLaunch.Font, .ImageScalingSize = FrmMain.cmsQuickLaunch.ImageScalingSize}

    End Sub

    'Protected Overrides Function CreateDefaultDropDown() As ToolStripDropDown
    '    Return New ThemedScollableDropDown
    'End Function

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        'If e.Button = MouseButtons.Right Then
        '    CancelClick = True
        '    Debug.Print("OnMouseDown: click will be canceled")
        '    RMouseDownOn = Me
        '    Return
        'End If

        'If e.Button = MouseButtons.Middle Then

        'End If

        'MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnClick(e As EventArgs)
        'If CancelClick Then
        '    CancelClick = False
        '    Debug.Print("OnClick: canceled")
        '    Return
        'End If

        'Debug.Print("OnClick: executed")
        MyBase.OnClick(EventArgs.Empty)
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        If RMouseDownOn Is Me Then
        End If

        MyBase.OnMouseUp(e)
    End Sub


End Class
Public Class ThemedScrollableDropDownMenu : Inherits ToolStripDropDownMenu

    Public StickyLabels(1) As StickyButtonLabel

    Dim StickyLabels_initialized As Boolean = False

    Protected Overrides Sub OnOpened(e As EventArgs)
        MyBase.OnOpened(e)

        Dim Scaling As Single = 1.0!

        If Not StickyLabels_initialized Then

            Dim Labels = Me.Controls.OfType(Of Label).ToArray
            For i = 0 To 1
                StickyLabels(i) = New StickyButtonLabel(Labels(i), 0 = i)

                StickyLabels(i).BackColor = If(My.Settings.DarkMode, Color.DimGray, SystemColors.Control)
                If Scaling <> 1.0! Then
                    'todo: draw bitmap ourselves. scaling leads to jagged/blurry edges.
                    StickyLabels(i).Image = New Bitmap(StickyLabels(i).Image.Clone, New Size(StickyLabels(i).Image.Width * Scaling, StickyLabels(i).Image.Height * Scaling))
                End If
            Next

            StickyLabels_initialized = True

        End If
        ' Defer marking stable until after layout/paint
        Task.Run(Sub()
                     Threading.Thread.Sleep(100)
                     Me.BeginInvoke(Sub()
                                        For Each wrap In StickyLabels
                                            wrap.Stable = True
                                        Next
                                    End Sub)
                 End Sub)

    End Sub

    Private Sub ThemedScollableDropDown_MouseWheel(sender As ToolStripDropDown, e As MouseEventArgs) Handles Me.MouseWheel
        Const WM_LBUTTONDOWN As Integer = &H201
        Const WM_LBUTTONUP As Integer = &H202
        'Const WM_MOUSEMOVE = &H200
        'Dim StickyLabels As New List(Of Control)

        'EnumChildWindows(sender.Handle, Function(h, l)
        '                                    If IsWindowVisible(h) AndAlso GetWindowClass(h).Contains("STATIC") Then
        '                                        StickyLabels.Add(Control.FromHandle(h))
        '                                    End If
        '                                    Return True
        '                                End Function, IntPtr.Zero)
        ' Debug.Print($"buttons {StickyLabels.Count} {StickyLabels.FirstOrDefault()?.GetType}")

        'Dim zDelta As Short = (m.WParam.ToInt64 And &HFFFF0000UI) >> 16 'this needs overflow checks set to off

        'Debug.Print($"Mouse wheel scrolled {zDelta} on dropdown!")
        If StickyLabels.Count = 2 Then
            'Debug.Print($"{StickyLabels(0).Bounds.Y} {StickyLabels(1).Bounds.Y}")
            Dim currentShift As Integer = sender.Items(0).Bounds.Y
            If e.Delta > 0 Then
                SendMessage(StickyLabels(0).Handle, WM_LBUTTONDOWN, IntPtr.Zero, IntPtr.Zero)
                SendMessage(StickyLabels(0).Handle, WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero)
                Dim newshift = sender.Items(0).Bounds.Y
                If newshift = currentShift Then 'top reached
                    Dim itm As ToolStripItem = sender.Items(0)

                End If
            Else
                SendMessage(StickyLabels(1).Handle, WM_LBUTTONDOWN, IntPtr.Zero, IntPtr.Zero)
                SendMessage(StickyLabels(1).Handle, WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero)
                Dim newshift = sender.Items(0).Bounds.Y
                If newshift = currentShift Then 'bottom reached
                    Dim itm As ToolStripItem = sender.Items(sender.Items.Count - 1)

                End If
            End If
            'Threading.Thread.Sleep(1)
            'Threading.Thread.Sleep(1)
            'Cursor.Position += New Point(1, 0)
            'SendMessage(tsdd.Handle, WM_MOUSEMOVE, IntPtr.Zero, IntPtr.Zero)
        End If
        For Each item In sender.Items.Cast(Of ToolStripItem).OfType(Of ToolStripMenuItem).Where(Function(it) it.HasDropDown AndAlso it.DropDown.Visible)
            If item.Bounds.Y < 0 OrElse item.Bounds.Bottom >= sender.Bounds.Height Then
                item.DropDown.Close()
            End If
        Next
    End Sub



End Class
