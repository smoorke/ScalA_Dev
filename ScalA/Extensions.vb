Imports System.Runtime.CompilerServices
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
        Const GWL_EXSTYLE = -20
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
    ''' Checks if a control contains a given point in screen coords
    ''' </summary>
    ''' <param name="ctrl"></param>
    ''' <param name="screenPt"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function Contains(ctrl As Control, screenPt As Point) As Boolean
        If TypeOf ctrl IsNot Form Then screenPt = ctrl.FindForm.PointToClient(screenPt)
        Return ctrl.Bounds.Contains(screenPt)
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
        Return toMin + ((this - fromMin) * (toMax - toMin) / (fromMax - fromMin))
    End Function

End Module