Public NotInheritable Class CustomStringSorter
	Implements IComparer(Of String)

	Private Shared ReadOnly StringComparer As Comparer(Of String) = Comparer(Of String).Default

	Public Function Compare(s1 As String, s2 As String) As Integer Implements IComparer(Of String).Compare
		'equal = 0, s1 > s2 = 1, s1 < s2 = -1

		If s1 Is Nothing AndAlso s2 Is Nothing Then Return 0
		If s1 Is Nothing Then Return -1
		If s2 Is Nothing Then Return 1

		If s1.Equals(String.Empty) AndAlso s2.Equals(String.Empty) Then Return 0
		If s1.Equals(String.Empty) Then Return -1
		If s2.Equals(String.Empty) Then Return 1

		Dim num1 As Integer = Val(s1)
		Dim num2 As Integer = Val(s2)

		If s1.TrimStart("0"c).StartsWith(num1) AndAlso s2.TrimStart("0"c).StartsWith(num2) Then
			If num1 = num2 Then Return StringComparer.Compare(s1.TrimStart({"0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c, "-"c}), s2.TrimStart({"0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c, "-"c}))
			If num1 > num2 Then Return 1
			If num1 < num2 Then Return -1
		End If

		If s1.StartsWith(num1) AndAlso Not s2.StartsWith(num2) Then Return -1
		If s2.StartsWith(num2) AndAlso Not s1.StartsWith(num1) Then Return 1

		Return StringComparer.Compare(s1, s2)

	End Function
End Class
