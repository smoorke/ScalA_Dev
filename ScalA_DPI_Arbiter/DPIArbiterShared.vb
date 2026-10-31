
Imports System.IO
Imports System.IO.Pipes
Imports System.Text
Imports System.Threading

Namespace DPIArbiter
    Module DPIArbiterShared

        Public PipeName As String = "ScalaDPIArbiterPipe"

        Public Enum ArbiterCommand As Integer
            ''' <summary>
            ''' response "Pong"
            ''' </summary>
            Ping = 1
            ''' <summary>
            ''' arg(0) hwnd as string
            ''' response: % as string
            ''' </summary>
            GetScale = 2
            ''' <summary>
            ''' signal the arbiter to shutdown
            ''' </summary>
            Shutdown = -1
        End Enum

        Public Enum ArbiterResponse As Integer
            Pong = 1
            Scale = 2
            ErrorResponse = -1
        End Enum

        Private Pipe As NamedPipeClientStream
        Private Reader As StreamReader
        Private Writer As StreamWriter

        Private ReadOnly PipeLock As New SemaphoreSlim(1, 1)

        Private Function SerializeCommand(command As ArbiterCommand, Optional args As String() = Nothing) As String

            Select Case command
                Case ArbiterCommand.Ping
                    Return "PING"

                Case ArbiterCommand.GetScale
                    If args Is Nothing OrElse args.Length = 0 OrElse String.IsNullOrWhiteSpace(args(0)) Then
                        Throw New ArgumentException("GETSCALE requires a Window Handle.", NameOf(args))
                    End If

                    Return $"GETSCALE|{args(0)}"

                Case ArbiterCommand.Shutdown
                    Return "SHUTDOWN"

                Case Else
                    Throw New ArgumentOutOfRangeException(NameOf(command))
            End Select
        End Function

        Private Sub DisconnectPipe()
            If Reader IsNot Nothing Then
                Reader.Dispose()
                Reader = Nothing
            End If

            If Writer IsNot Nothing Then
                Writer.Dispose()
                Writer = Nothing
            End If

            If Pipe IsNot Nothing Then
                Pipe.Dispose()
                Pipe = Nothing
            End If
        End Sub

        Private Async Function EnsureConnectedAsync(cancellationToken As CancellationToken) As Task

            If Pipe IsNot Nothing AndAlso Pipe.IsConnected Then Return

            DisconnectPipe()

            Dim newPipe As New NamedPipeClientStream(".", PipeName, PipeDirection.InOut, PipeOptions.Asynchronous)

            Try
                Await newPipe.ConnectAsync(cancellationToken).ConfigureAwait(False)

                Pipe = newPipe
                Reader = New StreamReader(Pipe, Encoding.UTF8, False, 1024, True)
                Writer = New StreamWriter(Pipe, Encoding.UTF8, 1024, True) With {.AutoFlush = True}

            Catch
                newPipe.Dispose()
                Throw
            End Try
        End Function

        Public Async Function SendArbiterCommandAsync(command As ArbiterCommand, Optional args As String() = Nothing, Optional timeoutMs As Integer = 250, Optional cancellationToken As CancellationToken = Nothing) As Task(Of String)

            If timeoutMs <= 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(timeoutMs))
            End If

            Dim commandText As String = SerializeCommand(command, args)

            Using timeoutCts As New CancellationTokenSource(timeoutMs),
              linkedCts As CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token)

                Await PipeLock.WaitAsync(linkedCts.Token).ConfigureAwait(False)

                Try
                    Await EnsureConnectedAsync(linkedCts.Token).ConfigureAwait(False)

                    Await Writer.WriteLineAsync(commandText).ConfigureAwait(False)

                    Dim responseTask As Task(Of String) = Reader.ReadLineAsync()
                    Dim cancelTask As Task = Task.Delay(Timeout.Infinite, linkedCts.Token)

                    Dim completed As Task = Await Task.WhenAny(responseTask, cancelTask).ConfigureAwait(False)

                    If completed IsNot responseTask Then
                        cancellationToken.ThrowIfCancellationRequested()
                        Throw New TimeoutException("Timed out waiting for the DPI Arbiter.")
                    End If

                    Dim response As String = Await responseTask.ConfigureAwait(False)

                    If response Is Nothing Then
                        Throw New IOException("The DPI Arbiter closed the pipe.")
                    End If

                    Return response

                Catch
                    ' A timed-out or failed request may leave the response stream
                    ' out of sync. Reset the connection before the next request.
                    DisconnectPipe()
                    Throw

                Finally
                    PipeLock.Release()
                End Try
            End Using
        End Function

        Public Sub CloseArbiterConnection()
            PipeLock.Wait()
            Try
                DisconnectPipe()
            Finally
                PipeLock.Release()
            End Try
        End Sub

    End Module
End Namespace