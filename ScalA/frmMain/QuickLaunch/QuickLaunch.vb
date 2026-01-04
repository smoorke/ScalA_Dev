Imports System.ComponentModel

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
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        If e.Button = MouseButtons.Right Then
            CancelClick = True
            Debug.Print("OnMouseDown: click will be canceled")
            RMouseDownOn = Me
            Return
        End If

        If e.Button = MouseButtons.Middle Then

        End If

        'MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnClick(e As EventArgs)
        If CancelClick Then
            CancelClick = False
            Debug.Print("OnClick: canceled")
            Return
        End If

        Debug.Print("OnClick: executed")
        'MyBase.OnClick(EventArgs.Empty)
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        If RMouseDownOn Is Me Then
            'MyBase.OnMouseUp(e)
        End If

    End Sub

End Class

Partial NotInheritable Class frmMain

End Class