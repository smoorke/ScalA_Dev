Imports System.Runtime.InteropServices
Imports System.Text

Namespace EditBoxHelper
    Public Module EditBoxHelper

        '--- Win32 messages
        Public Const WM_GETTEXT As Integer = &HD
        Public Const WM_GETTEXTLENGTH As Integer = &HE
        Public Const WM_SETTEXT As Integer = &HC
        Public Const EM_GETSEL As Integer = &HB0
        Public Const EM_SETSEL As Integer = &HB1
        Public Const EM_REPLACESEL As Integer = &HC2

        '--- Win32 API
        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Public Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
        End Function
        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Public Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As String) As IntPtr
        End Function
        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Public Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As StringBuilder) As IntPtr
        End Function
        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Public Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As Integer) As IntPtr
        End Function

        Public Function GetEditText(hEdit As IntPtr) As String
            Dim len As Integer = SendMessage(hEdit, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero).ToInt32()
            Dim sb As New StringBuilder(len + 2)
            SendMessage(hEdit, WM_GETTEXT, CType(sb.Capacity, IntPtr), sb)
            Return sb.ToString()
        End Function

        Public Sub GetSelection(hEdit As IntPtr, ByRef start As Integer, ByRef [end] As Integer)
            Dim result As IntPtr = SendMessage(hEdit, EM_GETSEL, IntPtr.Zero, IntPtr.Zero)
            start = result.ToInt32() And &HFFFF
            [end] = (result.ToInt32() >> 16) And &HFFFF
        End Sub

        Public Sub SetSelection(hEdit As IntPtr, start As Integer, [end] As Integer)
            SendMessage(hEdit, EM_SETSEL, CType(start, IntPtr), CType([end], IntPtr))
        End Sub

        Public Sub ReplaceSelection(hEdit As IntPtr, replacementText As String)
            SendMessage(hEdit, EM_REPLACESEL, CType(1, IntPtr), replacementText)
        End Sub

        '--- Main: delete previous word safely and keep undo buffer
        Public Sub DeletePreviousWord(hEdit As IntPtr)
            If hEdit = IntPtr.Zero Then Exit Sub

            Dim text As String = GetEditText(hEdit)
            Dim selStart As Integer = 0, selEnd As Integer = 0
            GetSelection(hEdit, selStart, selEnd)

            If selStart <> selEnd Then
                ' delete selection using EM_REPLACESEL so undo works
                ReplaceSelection(hEdit, "")
                Exit Sub
            End If

            If selStart = 0 Then Exit Sub

            Dim i As Integer = selStart - 1
            While i > 0 AndAlso Char.IsWhiteSpace(text(i))
                i -= 1
            End While
            While i > 0 AndAlso Not Char.IsWhiteSpace(text(i - 1))
                i -= 1
            End While

            ' Select from i to selStart
            SetSelection(hEdit, i, selStart)

            ' Replace selection with nothing (delete)
            ReplaceSelection(hEdit, "")
        End Sub

    End Module
End Namespace