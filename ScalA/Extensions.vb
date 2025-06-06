﻿Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Module Extensions
    ''' <summary>
    ''' Gets the value associated With the specified key.
    ''' </summary>
    ''' <typeparam name="TKey"></typeparam>
    ''' <typeparam name="TValue"></typeparam>
    ''' <param name="source"></param>
    ''' <param name="Key"></param>
    ''' <param name="defaultValue"></param>
    ''' <returns>If the key is nonexistant returns defaultValue.</returns>
    <Extension()>
    Public Function GetValueOrDefault(Of TKey, TValue)(source As Dictionary(Of TKey, TValue), Key As TKey, Optional defaultValue As TValue = CType(Nothing, TValue)) As TValue
        Dim found As TValue
        If source.TryGetValue(Key, found) Then Return found
        Return defaultValue
    End Function
    <Extension()>
    Public Function GetValueOrDefault(Of TKey, TValue)(source As Concurrent.ConcurrentDictionary(Of TKey, TValue), Key As TKey, Optional defaultValue As TValue = Nothing) As TValue
        Dim found As TValue
        If source.TryGetValue(Key, found) Then Return found
        Return defaultValue
    End Function

    <DllImport("user32.dll")>
    Private Function GetWindowLong(ByVal hwnd As IntPtr, ByVal nIndex As Integer) As UInteger
    End Function
    ''' <summary>
    ''' Checks if a processes mainwindow is topmost.
    ''' </summary>
    ''' <param name="PP"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function IsTopMost(PP As Process) As Boolean
        Const WS_EX_TOPMOST = 8L
        Return (GetWindowLong(PP.MainWindowHandle, GWL_EXSTYLE) And WS_EX_TOPMOST) = WS_EX_TOPMOST
    End Function


    ''' <summary>
    ''' Caps a string at a given length.
    ''' </summary>
    ''' <param name="str"></param>
    ''' <param name="length"></param>
    ''' <returns></returns>
    <Extension>
    Public Function Cap(str As String, length As Integer) As String
        Return Strings.Left(str, Math.Min(length, str.Length))
    End Function

    ''' <summary>
    ''' Returns the input string with the first character converted to uppercase, Or mutates any nulls passed into string.Empty
    ''' </summary>
    <Extension>
    Public Function FirstToUpper(s As String, Optional restLower As Boolean = False) As String
        If (String.IsNullOrEmpty(s)) Then Return String.Empty

        If restLower Then s = s.ToLower()
        Dim a As Char() = s.ToCharArray()
        a(0) = Char.ToUpper(a(0))
        Return New String(a)
    End Function

    ''' <summary>
    ''' Checks if a control contains a given point in screen coords
    ''' </summary>
    ''' <param name="ctrl"></param>
    ''' <param name="screenPt"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function Contains(ctrl As Control, screenPt As Point) As Boolean
        Dim box As ComboBox = TryCast(ctrl, ComboBox)
        If TypeOf ctrl Is Label Then Return CType(ctrl, Label).Contains(screenPt)
        If TypeOf ctrl IsNot Form Then screenPt = ctrl.FindForm?.PointToClient(screenPt)
        Return ctrl.Bounds.Contains(screenPt) OrElse
                (box IsNot Nothing AndAlso box.DroppedDown AndAlso
                New Rectangle(box.Left, box.Bottom, box.DropDownWidth, box.DropDownHeight).Contains(screenPt))
    End Function

    ' duplications due to hot path

    ''' <summary>
    ''' Checks lablel contains a given point in screen coords
    ''' </summary>
    ''' <param name="lbl"></param>
    ''' <param name="screenPt"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function Contains(lbl As Label, screenPt As Point) As Boolean
        Return lbl.RectangleToScreen(lbl.Bounds).Contains(screenPt)
    End Function

    <Extension()>
    Public Function Contains(but As Button, screenPt As Point) As Boolean
        Return but.Parent.RectangleToScreen(but.Bounds).Contains(screenPt)
    End Function

    ''' <summary>
    ''' Checks if a form contains a given point in screen coords
    ''' </summary>
    ''' <param name="frm"></param>
    ''' <param name="screenPt"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function Contains(frm As Form, screenPt As Point) As Boolean
        Return frm.Bounds.Contains(screenPt)
    End Function

    ''' <summary>
    ''' Checks if a combobox or it's dropdown contains a given point in screen coords
    ''' </summary>
    ''' <param name="box"></param>
    ''' <param name="screenPt"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function Contains(box As ComboBox, screenPt As Point) As Boolean
        screenPt = box.FindForm?.PointToClient(screenPt)
        Return box.Bounds.Contains(screenPt) OrElse
                (box.DroppedDown AndAlso
                New Rectangle(box.Left, box.Bottom, box.DropDownWidth, box.DropDownHeight).Contains(screenPt))
    End Function
    ''' <summary>
    ''' Checks if the dropdown of a combobox contains a given point in screen coords
    ''' </summary>
    ''' <param name="box"></param>
    ''' <param name="screenPt"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function DropDownContains(box As ComboBox, screenPt As Point) As Boolean
        screenPt = box.FindForm?.PointToClient(screenPt)
        Return (box.DroppedDown AndAlso
                New Rectangle(box.Left, box.Bottom, box.DropDownWidth, box.DropDownHeight).Contains(screenPt))
    End Function

    ''' <summary>
    ''' Checks if a panel contains a given point in screen coords
    ''' </summary>
    ''' <param name="pnl"></param>
    ''' <param name="screenPt"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function Contains(pnl As Panel, screenPt As Point) As Boolean
        screenPt = pnl.FindForm?.PointToClient(screenPt)
        Return pnl.Bounds.Contains(screenPt)
    End Function

    ''' <summary>
    ''' Checks if a picturebox contains a given point in screen coords
    ''' </summary>
    ''' <param name="pbox"></param>
    ''' <param name="screenPt"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function Contains(pbox As PictureBox, screenPt As Point) As Boolean
        screenPt = pbox.FindForm?.PointToClient(screenPt)
        Return pbox.Bounds.Contains(screenPt)
    End Function

    ''' <summary>
    ''' Maps an integer between a given from and to a given min max value.
    ''' </summary>
    ''' <param name="this"></param>
    ''' <param name="fromMin"></param>
    ''' <param name="fromMax"></param>
    ''' <param name="toMin"></param>
    ''' <param name="toMax"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function Map(this As Integer, fromMin As Integer, fromMax As Integer, toMin As Integer, toMax As Integer) As Integer
        If (fromMax - fromMin) = 0 Then Return 0
        Return toMin + ((this - fromMin) * (toMax - toMin) / (fromMax - fromMin))
    End Function



    <System.Runtime.CompilerServices.Extension()>
    Public Sub SetFlag(ByRef flags As SetWindowPosFlags, value As SetWindowPosFlags)
        flags = flags Or value
    End Sub

End Module