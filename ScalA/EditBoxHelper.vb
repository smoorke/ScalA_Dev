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

        Dim tt As ToolTip

        Dim _selStart As Integer
        Dim _selEnd As Integer
        Dim _text As String

        <DllImport("user32.dll")>
        Private Function GetCaretPos(ByRef lpPoint As Point) As Boolean
        End Function
        <DllImport("user32.dll")>
        Private Function SetCaretPos(ByRef lpPoint As Point) As Boolean
        End Function

        Sub StoreEditState(hEdit As IntPtr)
            If hEdit = IntPtr.Zero Then Exit Sub
            Dim illegalchars As Char() = IO.Path.GetInvalidFileNameChars()
            If Not GetEditText(hEdit).Any(Function(c) illegalchars.Contains(c)) Then
                _text = String.Concat(GetEditText(hEdit).Where(Function(c) Not illegalchars.Contains(c)))
                GetSelection(hEdit, _selStart, _selEnd)
            End If
            Debug.Print($"editboxstate: ""{_text}"" {_selStart} {_selEnd}")
        End Sub


        Public Sub CleanInputBox()
            Dim fgw = GetForegroundWindow()
            Dim pid As Integer
            GetWindowThreadProcessId(fgw, pid)
            If pid = scalaPID AndAlso
                   GetWindowLong(fgw, GWL_HWNDPARENT) = ScalaHandle AndAlso
                   GetWindowText(fgw).StartsWith("Rename ") AndAlso
                   GetWindowClass(fgw) = ScalaClass Then ' GetWindowClass(ScalaHandle) Then
                Dim hEdit = FindWindowEx(fgw, IntPtr.Zero, Nothing, Nothing)
                Debug.Print($"{GetWindowClass(hEdit)}")
                If GetWindowClass(hEdit).Contains("EDIT") Then
                    Dim illegalChars = IO.Path.GetInvalidFileNameChars()
                    Dim text = GetEditText(hEdit)
                    If text.Any(Function(c) illegalChars.Contains(c)) Then

                        ' Remove illegal characters from the typed text
                        Dim sanitized = _text 'String.Concat(text.Where(Function(c) Not illegalChars.Contains(c)))

                        ' Calculate new caret/selection relative to stored state
                        'Dim deltaBeforeCaret = _text.Substring(0, _selStart).Count(Function(c) illegalChars.Contains(c))
                        Dim newSelStart = Math.Max(0, _selStart) ' - deltaBeforeCaret)
                        Dim newSelEnd = newSelStart + _selEnd - _selStart

                        ReplaceAllText(hEdit, sanitized, newSelStart, newSelEnd)

                        Beep()

                        ' Show tooltip
                        If tt Is Nothing Then tt = New ToolTip() With {
                                                    .IsBalloon = True,
                                                    .InitialDelay = 0,
                                                    .ReshowDelay = 0}

                        ' Get the screen position of the edit box
                        Dim rect As New RECT
                        GetWindowRect(hEdit, rect)
                        Dim pos As New Point(rect.left + 7, rect.top - 64)

                        Dim owner As Control = Control.FromHandle(hEdit)
                        pos = owner.PointToClient(pos)
                        Debug.Print($"ttpos: {pos} ""{owner}""")

                        Dim handle As IntPtr = IntPtr.Zero

                        EnumThreadWindows(ScalaThreadId, Function(hWnd, lParam)
                                                             If Not IsWindowVisible(hWnd) Then Return True
                                                             Dim sb As New System.Text.StringBuilder(256)
                                                             GetClassName(hWnd, sb, sb.Capacity)
                                                             If GetWindowClass(hWnd).Contains("tooltips_class32") Then
                                                                 handle = hWnd
                                                                 Return False ' stop enumeration
                                                             End If
                                                             Return True ' continue
                                                         End Function, IntPtr.Zero)

                        If handle = IntPtr.Zero Then
                            Debug.Print("showing tt")
                            tt.Show($"A file name can't contain any of the following characters:{vbCrLf}{vbTab} \ / : * ? "" < > |", owner, pos.X, pos.Y, 10000)
                        End If

                    End If
                End If
            End If
        End Sub

        Public Sub ReplaceAllText(hEdit As IntPtr, text As String, selStart As Integer, selEnd As Integer)
            ' Select all text
            SetSelection(hEdit, 0, GetEditText(hEdit).Length)
            ' Replace selection
            ReplaceSelection(hEdit, text)
            ' Restore caret/selection
            SetSelection(hEdit, selStart, selEnd)
        End Sub

    End Module
End Namespace