Imports System.Runtime.InteropServices
Imports System.Runtime.CompilerServices
Imports System.ComponentModel

Public Enum PROCESSINFOCLASS
    ProcessBasicInformation = 0
    ProcessWow64Information = 26
End Enum

<Flags>
Public Enum PEB_OFFSET
    CurrentDirectory
    CommandLine
End Enum

Public Class Is64BitChecker
    <DllImport("kernel32.dll", SetLastError:=True, CallingConvention:=CallingConvention.Winapi)>
    Private Shared Function IsWow64Process(<[In]> ByVal hProcess As IntPtr, <Out> ByRef wow64Process As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean : End Function

    Public Shared Function GetProcessIsWow64(ByVal hProcess As IntPtr) As Boolean
        If (Environment.OSVersion.Version.Major = 5 AndAlso Environment.OSVersion.Version.Minor >= 1) OrElse Environment.OSVersion.Version.Major >= 6 Then
            Dim retVal As Boolean

            If Not IsWow64Process(hProcess, retVal) Then
                Return False
            End If

            Return retVal
        Else
            Return False
        End If
    End Function

    Public Shared Function InternalCheckIsWow64() As Boolean
        If (Environment.OSVersion.Version.Major = 5 AndAlso Environment.OSVersion.Version.Minor >= 1) OrElse Environment.OSVersion.Version.Major >= 6 Then

            Using p As Process = Process.GetCurrentProcess()
                Dim retVal As Boolean

                If Not IsWow64Process(p.Handle, retVal) Then
                    Return False
                End If

                Return retVal
            End Using
        Else
            Return False
        End If
    End Function
End Class

Module ProcessUtilities
    Public ReadOnly Is64BitProcess As Boolean = IntPtr.Size > 4
    Public ReadOnly Is64BitOperatingSystem As Boolean = Is64BitProcess OrElse Is64BitChecker.InternalCheckIsWow64()

    Function GetCurrentDirectory(ByVal processId As Integer) As String
        Return GetProcessParametersString(processId, PEB_OFFSET.CurrentDirectory)
    End Function

    <Extension()>
    Function GetCurrentDirectory(ByVal process As Process) As String
        If process Is Nothing Then Throw New ArgumentNullException("process")
        Return GetCurrentDirectory(process.Id)
    End Function

    Private Function GetProcessParametersString(ByVal processId As Integer, ByVal type As PEB_OFFSET) As String
        Dim handle As IntPtr = OpenProcess(PROCESS_QUERY_INFORMATION Or PROCESS_VM_READ, False, processId)
        If handle = IntPtr.Zero Then Throw New Win32Exception(Marshal.GetLastWin32Error())
        Dim IsWow64Process As Boolean = Is64BitChecker.InternalCheckIsWow64()
        Dim IsTargetWow64Process As Boolean = Is64BitChecker.GetProcessIsWow64(handle)
        Dim IsTarget64BitProcess As Boolean = Is64BitOperatingSystem AndAlso Not IsTargetWow64Process
        Dim offset As Long = 0
        Dim processParametersOffset As Long = If(IsTarget64BitProcess, &H20, &H10)

        Select Case type
            Case PEB_OFFSET.CurrentDirectory
                offset = If(IsTarget64BitProcess, &H38, &H24)
            Case Else
                Return Nothing
        End Select

        Try
            Dim pebAddress As Long = 0

            If IsTargetWow64Process Then
                Dim peb32 As IntPtr = New IntPtr()
                Dim hr As Integer = NtQueryInformationProcess(handle, CInt(PROCESSINFOCLASS.ProcessWow64Information), peb32, IntPtr.Size, IntPtr.Zero)
                If hr <> 0 Then Throw New Win32Exception(hr)
                pebAddress = peb32.ToInt64()
                Dim pp As IntPtr = New IntPtr()
                If Not ReadProcessMemory(handle, New IntPtr(pebAddress + processParametersOffset), pp, New IntPtr(Marshal.SizeOf(pp)), IntPtr.Zero) Then Throw New Win32Exception(Marshal.GetLastWin32Error())
                Dim us As UNICODE_STRING_32 = New UNICODE_STRING_32()
                If Not ReadProcessMemory(handle, New IntPtr(pp.ToInt64() + offset), us, New IntPtr(Marshal.SizeOf(us)), IntPtr.Zero) Then Throw New Win32Exception(Marshal.GetLastWin32Error())
                If (us.Buffer = 0) OrElse (us.Length = 0) Then Return Nothing
                Dim s As String = New String(vbNullChar, us.Length / 2)
                If Not ReadProcessMemory(handle, New IntPtr(us.Buffer), s, New IntPtr(us.Length), IntPtr.Zero) Then Throw New Win32Exception(Marshal.GetLastWin32Error())
                Return s
            ElseIf IsWow64Process Then
                Dim pbi As PROCESS_BASIC_INFORMATION_WOW64 = New PROCESS_BASIC_INFORMATION_WOW64()
                Dim hr As Integer = NtWow64QueryInformationProcess64(handle, CInt(PROCESSINFOCLASS.ProcessBasicInformation), pbi, Marshal.SizeOf(pbi), IntPtr.Zero)
                If hr <> 0 Then Throw New Win32Exception(hr)
                pebAddress = pbi.PebBaseAddress
                Dim pp As Long = 0
                hr = NtWow64ReadVirtualMemory64(handle, pebAddress + processParametersOffset, pp, Marshal.SizeOf(pp), IntPtr.Zero)
                If hr <> 0 Then Throw New Win32Exception(hr)
                Dim us As UNICODE_STRING_WOW64 = New UNICODE_STRING_WOW64()
                hr = NtWow64ReadVirtualMemory64(handle, pp + offset, us, Marshal.SizeOf(us), IntPtr.Zero)
                If hr <> 0 Then Throw New Win32Exception(hr)
                If (us.Buffer = 0) OrElse (us.Length = 0) Then Return Nothing
                Dim s As String = New String(vbNullChar, us.Length / 2)
                hr = NtWow64ReadVirtualMemory64(handle, us.Buffer, s, us.Length, IntPtr.Zero)
                If hr <> 0 Then Throw New Win32Exception(hr)
                Return s
            Else
                Dim pbi As PROCESS_BASIC_INFORMATION = New PROCESS_BASIC_INFORMATION()
                Dim hr As Integer = NtQueryInformationProcess(handle, CInt(PROCESSINFOCLASS.ProcessBasicInformation), pbi, Marshal.SizeOf(pbi), IntPtr.Zero)
                If hr <> 0 Then Throw New Win32Exception(hr)
                pebAddress = pbi.PebBaseAddress.ToInt64()
                Dim pp As IntPtr = New IntPtr()
                If Not ReadProcessMemory(handle, New IntPtr(pebAddress + processParametersOffset), pp, New IntPtr(Marshal.SizeOf(pp)), IntPtr.Zero) Then Throw New Win32Exception(Marshal.GetLastWin32Error())
                Dim us As UNICODE_STRING = New UNICODE_STRING()
                If Not ReadProcessMemory(handle, New IntPtr(CLng(pp) + offset), us, New IntPtr(Marshal.SizeOf(us)), IntPtr.Zero) Then Throw New Win32Exception(Marshal.GetLastWin32Error())
                If (us.Buffer = IntPtr.Zero) OrElse (us.Length = 0) Then Return Nothing
                Dim s As String = New String(vbNullChar, us.Length / 2)
                If Not ReadProcessMemory(handle, us.Buffer, s, New IntPtr(us.Length), IntPtr.Zero) Then Throw New Win32Exception(Marshal.GetLastWin32Error())
                Return s
            End If

        Finally
            CloseHandle(handle)
        End Try
    End Function

    Private Const PROCESS_QUERY_INFORMATION As Integer = &H400
    Private Const PROCESS_VM_READ As Integer = &H10

    <StructLayout(LayoutKind.Sequential)>
    Private Structure PROCESS_BASIC_INFORMATION
        Public Reserved1 As IntPtr
        Public PebBaseAddress As IntPtr
        Public Reserved2_0 As IntPtr
        Public Reserved2_1 As IntPtr
        Public UniqueProcessId As IntPtr
        Public Reserved3 As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure UNICODE_STRING
        Public Length As Short
        Public MaximumLength As Short
        Public Buffer As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure PROCESS_BASIC_INFORMATION_WOW64
        Public Reserved1 As Long
        Public PebBaseAddress As Long
        Public Reserved2_0 As Long
        Public Reserved2_1 As Long
        Public UniqueProcessId As Long
        Public Reserved3 As Long
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure UNICODE_STRING_WOW64
        Public Length As Short
        Public MaximumLength As Short
        Public Buffer As Long
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure UNICODE_STRING_32
        Public Length As Short
        Public MaximumLength As Short
        Public Buffer As Integer
    End Structure

    <DllImport("ntdll.dll")>
    Private Function NtQueryInformationProcess(ByVal ProcessHandle As IntPtr, ByVal ProcessInformationClass As Integer, ByRef ProcessInformation As PROCESS_BASIC_INFORMATION, ByVal ProcessInformationLength As Integer, ByVal ReturnLength As IntPtr) As Integer : End Function
    Private Function NtQueryInformationProcess(ByVal ProcessHandle As IntPtr, ByVal ProcessInformationClass As Integer, ByRef ProcessInformation As IntPtr, ByVal ProcessInformationLength As Integer, ByVal ReturnLength As IntPtr) As Integer : End Function
    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Function ReadProcessMemory(ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByRef lpBuffer As IntPtr, ByVal dwSize As IntPtr, ByVal lpNumberOfBytesRead As IntPtr) As Boolean : End Function
    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Function ReadProcessMemory(ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByRef lpBuffer As UNICODE_STRING, ByVal dwSize As IntPtr, ByVal lpNumberOfBytesRead As IntPtr) As Boolean : End Function
    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Function ReadProcessMemory(ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByRef lpBuffer As UNICODE_STRING_32, ByVal dwSize As IntPtr, ByVal lpNumberOfBytesRead As IntPtr) As Boolean : End Function
    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Function ReadProcessMemory(ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr,
    <MarshalAs(UnmanagedType.LPWStr)> ByVal lpBuffer As String, ByVal dwSize As IntPtr, ByVal lpNumberOfBytesRead As IntPtr) As Boolean : End Function
    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Function OpenProcess(ByVal dwDesiredAccess As Integer, ByVal bInheritHandle As Boolean, ByVal dwProcessId As Integer) As IntPtr : End Function
    <DllImport("kernel32.dll")>
    Private Function CloseHandle(ByVal hObject As IntPtr) As Boolean : End Function
    <DllImport("ntdll.dll")>
    Private Function NtWow64QueryInformationProcess64(ByVal ProcessHandle As IntPtr, ByVal ProcessInformationClass As Integer, ByRef ProcessInformation As PROCESS_BASIC_INFORMATION_WOW64, ByVal ProcessInformationLength As Integer, ByVal ReturnLength As IntPtr) As Integer : End Function
    <DllImport("ntdll.dll")>
    Private Function NtWow64ReadVirtualMemory64(ByVal hProcess As IntPtr, ByVal lpBaseAddress As Long, ByRef lpBuffer As Long, ByVal dwSize As Long, ByVal lpNumberOfBytesRead As IntPtr) As Integer : End Function
    <DllImport("ntdll.dll")>
    Private Function NtWow64ReadVirtualMemory64(ByVal hProcess As IntPtr, ByVal lpBaseAddress As Long, ByRef lpBuffer As UNICODE_STRING_WOW64, ByVal dwSize As Long, ByVal lpNumberOfBytesRead As IntPtr) As Integer : End Function
    <DllImport("ntdll.dll")>
    Private Function NtWow64ReadVirtualMemory64(ByVal hProcess As IntPtr, ByVal lpBaseAddress As Long,
    <MarshalAs(UnmanagedType.LPWStr)> ByVal lpBuffer As String, ByVal dwSize As Long, ByVal lpNumberOfBytesRead As IntPtr) As Integer : End Function
End Module