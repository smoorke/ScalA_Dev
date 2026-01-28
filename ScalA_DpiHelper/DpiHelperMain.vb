Imports System.IO
Imports System.IO.Pipes
Imports System.Runtime.InteropServices
Imports System.Threading

Module Main
    ' Single-instance mutex
    Public SingleInstanceMutex As Mutex

    Sub Main()
        ' Attempt to acquire single instance
        Dim createdNew As Boolean = False
        SingleInstanceMutex = New Mutex(True, Constants.MutexName, createdNew)
        If Not createdNew Then
            Console.WriteLine("Another instance is already running.")
            Return
        End If

        Debug.Print("Helper started.")

        ' Start pipe server thread
        Dim pipeThread As New Thread(AddressOf PipeServerThread)
        pipeThread.IsBackground = True
        pipeThread.Start()

        ' Main loop: poll for running main app windows
        While True
            Thread.Sleep(3210)
            If Not AnyScalAWindowRunning() Then
                Console.WriteLine("No ScalA windows found. Exiting helper.")
                Exit While
            End If
        End While

        ' Cleanup
        Try
            SingleInstanceMutex.ReleaseMutex()
        Catch
        End Try
        Debug.Print("Helper terminated.")
    End Sub

    ' Simple pipe server placeholder
    Public Sub PipeServerThread()
        While True
            Try
                Using pipe As New NamedPipeServerStream(PipeName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous)
                    Debug.Print("Pipe server waiting for client...")
                    pipe.WaitForConnection()
                    Debug.Print("Client connected.")

                    ' TODO: implement message protocol
                    Dim reader As New StreamReader(pipe)
                    Dim writer As New StreamWriter(pipe) With {.AutoFlush = True}

                    Dim line As String = reader.ReadLine()
                    Debug.Print("Received: " & line)
                    writer.WriteLine("ACK:" & line)
                End Using
            Catch ex As Exception
                Debug.Print("Pipe server error: " & ex.Message)
            End Try
        End While
    End Sub

    ' Poll for running main app windows
    Public Function AnyScalAWindowRunning() As Boolean
        Dim found As Boolean = False
        EnumWindows(Function(h As IntPtr, l As IntPtr) As Boolean
                        If IsWindow(h) AndAlso GetModuleBuildName(h) = "Scala" Then
                            found = True
                            Return False
                        End If
                        Return True
                    End Function, IntPtr.Zero)
        Return found
    End Function

    Public Function GetModuleBuildName(hWnd As IntPtr) As String
        ' TODO: implement actual cross-process module name retrieval
        Return ""
    End Function

End Module
Public Module Constants
    Public Const PipeName As String = "ScalA_DpiHelperPipe"
    Public Const MutexName As String = "ScalA_DpiHelperSingleInstanceMutex"
End Module

Public Module NativeMethods
    Public Delegate Function EnumWindowsProc(hWnd As IntPtr, lParam As IntPtr) As Boolean

    <DllImport("user32.dll")>
    Public Function EnumWindows(lpEnumFunc As EnumWindowsProc, lParam As IntPtr) As Boolean : End Function

    <DllImport("user32.dll")>
    Public Function IsWindow(hWnd As IntPtr) As Boolean : End Function
End Module

Public Module Extensions
    'todo: read actual name from ScalA project 
    Private ReadOnly OrigScalAfname As String = "ScalA.exe" 'scalaProc.MainModule.FileVersionInfo.OriginalFilename

    '''' <summary>
    '''' Is process ScalA?
    '''' </summary>
    '''' <param name="p"></param>
    '''' <returns></returns>
    <System.Runtime.CompilerServices.Extension()>
    Public Function IsScalA(hWnd As IntPtr) As Boolean
        Dim pid As Integer
        getwindowthreadid(hWnd, pid)
        Try
            Dim p As Process = Process.GetProcessById(pid)
            Return p.MainModule.FileVersionInfo.OriginalFilename = OrigScalAfname
        Catch ex As System.ComponentModel.Win32Exception
            Dim processPath As String = ""

            Dim processHandle As IntPtr = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, False, pid)
            Try
                If Not processHandle = IntPtr.Zero Then
                    Dim buffer As New System.Text.StringBuilder(1024)
                    If QueryFullProcessImageName(processHandle, 0, buffer, buffer.Capacity) Then
                        processPath = buffer.ToString()
                    End If
                End If
            Finally
                CloseHandle(processHandle)
            End Try

            If String.IsNullOrEmpty(processPath) Then Return False

            Return FileVersionInfo.GetVersionInfo(processPath).OriginalFilename = OrigScalAfname

        Catch ex As Exception
            Return False
        End Try
    End Function
End Module
