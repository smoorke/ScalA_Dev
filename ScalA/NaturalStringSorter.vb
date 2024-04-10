Imports System.Runtime.InteropServices

Public NotInheritable Class NaturalStringSorter : Implements IComparer(Of String)
    <DllImport("shlwapi.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function StrCmpLogicalW(s1 As String, s2 As String) As Integer : End Function
    Public Function Compare(s1 As String, s2 As String) As Integer Implements IComparer(Of String).Compare
        Return StrCmpLogicalW(s1, s2)
    End Function
End Class