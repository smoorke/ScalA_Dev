Imports System.IO

''' <summary>
''' Helper module for installing/uninstalling the SDL2 mouse wrapper DLL
''' </summary>
Module SDL2WrapperHelper

    ''' <summary>
    ''' Check if the SDL2 wrapper is installed in the specified directory
    ''' </summary>
    Public Function IsWrapperInstalled(gameDir As String) As Boolean
        If String.IsNullOrEmpty(gameDir) OrElse Not Directory.Exists(gameDir) Then Return False
        ' Wrapper is installed if SDL2_real.dll exists (the original was renamed)
        Return File.Exists(IO.Path.Combine(gameDir, "SDL2_real.dll"))
    End Function

    ''' <summary>
    ''' Check if SDL2.dll exists in the specified directory
    ''' </summary>
    Public Function HasSDL2(gameDir As String) As Boolean
        If String.IsNullOrEmpty(gameDir) OrElse Not Directory.Exists(gameDir) Then Return False
        Return File.Exists(IO.Path.Combine(gameDir, "SDL2.dll"))
    End Function

    ''' <summary>
    ''' Get the directory containing SDL2.dll for a process
    ''' </summary>
    Public Function GetSDL2Directory(process As AstoniaProcess) As String
        If process Is Nothing Then Return Nothing

        Try
            Dim exePath As String = process.FinalPath
            If String.IsNullOrEmpty(exePath) Then Return Nothing

            Dim exeDir As String = IO.Path.GetDirectoryName(exePath)

            ' Check exe directory first
            If File.Exists(IO.Path.Combine(exeDir, "SDL2.dll")) Then
                Return exeDir
            End If

            ' Check bin subdirectory (common for Astonia)
            Dim binDir As String = IO.Path.Combine(exeDir, "bin")
            If Directory.Exists(binDir) AndAlso File.Exists(IO.Path.Combine(binDir, "SDL2.dll")) Then
                Return binDir
            End If

            ' Check parent\bin (if exe is in client folder)
            Dim parentBin As String = IO.Path.Combine(IO.Path.GetDirectoryName(exeDir), "bin")
            If Directory.Exists(parentBin) AndAlso File.Exists(IO.Path.Combine(parentBin, "SDL2.dll")) Then
                Return parentBin
            End If

        Catch ex As Exception
            dBug.Print($"SDL2WrapperHelper: Error finding SDL2 directory: {ex.Message}")
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' Install the SDL2 wrapper to the specified directory
    ''' </summary>
    Public Function InstallWrapper(gameDir As String) As Boolean
        If String.IsNullOrEmpty(gameDir) OrElse Not Directory.Exists(gameDir) Then
            Return False
        End If

        Dim sdl2Path As String = IO.Path.Combine(gameDir, "SDL2.dll")
        Dim sdl2RealPath As String = IO.Path.Combine(gameDir, "SDL2_real.dll")

        Try
            ' Check if already installed
            If File.Exists(sdl2RealPath) Then
                dBug.Print("SDL2WrapperHelper: Wrapper already installed, updating...")
            Else
                ' Rename original SDL2.dll to SDL2_real.dll
                If Not File.Exists(sdl2Path) Then
                    dBug.Print("SDL2WrapperHelper: SDL2.dll not found")
                    Return False
                End If

                File.Move(sdl2Path, sdl2RealPath)
                dBug.Print($"SDL2WrapperHelper: Renamed SDL2.dll to SDL2_real.dll")
            End If

            ' Write embedded wrapper DLL
            Dim wrapperBytes As Byte() = My.Resources.SDL2Wrapper
            File.WriteAllBytes(sdl2Path, wrapperBytes)
            dBug.Print($"SDL2WrapperHelper: Installed wrapper ({wrapperBytes.Length} bytes)")

            Return True

        Catch ex As UnauthorizedAccessException
            dBug.Print($"SDL2WrapperHelper: Access denied - {ex.Message}")
            ' Try with elevation
            Return InstallWrapperElevated(gameDir)

        Catch ex As Exception
            dBug.Print($"SDL2WrapperHelper: Install failed - {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Install wrapper with admin elevation
    ''' </summary>
    Private Function InstallWrapperElevated(gameDir As String) As Boolean
        Try
            ' Write wrapper to temp file
            Dim tempWrapper As String = IO.Path.Combine(IO.Path.GetTempPath(), "SDL2Wrapper_temp.dll")
            File.WriteAllBytes(tempWrapper, My.Resources.SDL2Wrapper)

            ' Create a batch script to do the install
            Dim batchPath As String = IO.Path.Combine(IO.Path.GetTempPath(), "install_wrapper.bat")
            Dim sdl2Path As String = IO.Path.Combine(gameDir, "SDL2.dll")
            Dim sdl2RealPath As String = IO.Path.Combine(gameDir, "SDL2_real.dll")

            Dim batchContent As String = $"@echo off
if not exist ""{sdl2RealPath}"" (
    move ""{sdl2Path}"" ""{sdl2RealPath}""
)
copy /Y ""{tempWrapper}"" ""{sdl2Path}""
del ""{tempWrapper}""
del ""%~f0""
"
            File.WriteAllText(batchPath, batchContent)

            ' Run as admin
            Dim psi As New ProcessStartInfo With {
                .FileName = batchPath,
                .Verb = "runas",
                .UseShellExecute = True,
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden
            }

            Dim proc = Process.Start(psi)
            proc?.WaitForExit(10000)

            Return IsWrapperInstalled(gameDir)

        Catch ex As Exception
            dBug.Print($"SDL2WrapperHelper: Elevated install failed - {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Uninstall the SDL2 wrapper from the specified directory
    ''' </summary>
    Public Function UninstallWrapper(gameDir As String) As Boolean
        If String.IsNullOrEmpty(gameDir) OrElse Not Directory.Exists(gameDir) Then
            Return False
        End If

        Dim sdl2Path As String = IO.Path.Combine(gameDir, "SDL2.dll")
        Dim sdl2RealPath As String = IO.Path.Combine(gameDir, "SDL2_real.dll")

        Try
            If Not File.Exists(sdl2RealPath) Then
                dBug.Print("SDL2WrapperHelper: Wrapper not installed (SDL2_real.dll not found)")
                Return False
            End If

            ' Delete wrapper
            If File.Exists(sdl2Path) Then
                File.Delete(sdl2Path)
            End If

            ' Restore original
            File.Move(sdl2RealPath, sdl2Path)
            dBug.Print("SDL2WrapperHelper: Wrapper uninstalled, original restored")

            Return True

        Catch ex As UnauthorizedAccessException
            dBug.Print($"SDL2WrapperHelper: Access denied during uninstall - {ex.Message}")
            Return UninstallWrapperElevated(gameDir)

        Catch ex As Exception
            dBug.Print($"SDL2WrapperHelper: Uninstall failed - {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Uninstall wrapper with admin elevation
    ''' </summary>
    Private Function UninstallWrapperElevated(gameDir As String) As Boolean
        Try
            Dim batchPath As String = IO.Path.Combine(IO.Path.GetTempPath(), "uninstall_wrapper.bat")
            Dim sdl2Path As String = IO.Path.Combine(gameDir, "SDL2.dll")
            Dim sdl2RealPath As String = IO.Path.Combine(gameDir, "SDL2_real.dll")

            Dim batchContent As String = $"@echo off
del ""{sdl2Path}""
move ""{sdl2RealPath}"" ""{sdl2Path}""
del ""%~f0""
"
            File.WriteAllText(batchPath, batchContent)

            Dim psi As New ProcessStartInfo With {
                .FileName = batchPath,
                .Verb = "runas",
                .UseShellExecute = True,
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden
            }

            Dim proc = Process.Start(psi)
            proc?.WaitForExit(10000)

            Return Not IsWrapperInstalled(gameDir)

        Catch ex As Exception
            dBug.Print($"SDL2WrapperHelper: Elevated uninstall failed - {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Prompt user to install wrapper when attaching to an SDL game
    ''' </summary>
    Public Sub CheckAndPromptInstall(process As AstoniaProcess, owner As Form)
        If process Is Nothing OrElse Not process.isSDL Then Return

        Dim sdl2Dir As String = GetSDL2Directory(process)
        If String.IsNullOrEmpty(sdl2Dir) Then Return

        ' Already installed?
        If IsWrapperInstalled(sdl2Dir) Then
            dBug.Print($"SDL2WrapperHelper: Wrapper already installed in {sdl2Dir}")
            Return
        End If

        ' Has SDL2.dll?
        If Not HasSDL2(sdl2Dir) Then Return

        ' Prompt user
        Dim result = CustomMessageBox.Show(owner,
            "SDL2 mouse wrapper can improve zoom accuracy by fixing mouse position jitter." & vbCrLf & vbCrLf &
            "Install SDL2 wrapper for this game?" & vbCrLf & vbCrLf &
            $"Location: {sdl2Dir}",
            "Install SDL2 Wrapper",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            If InstallWrapper(sdl2Dir) Then
                CustomMessageBox.Show(owner,
                    "SDL2 wrapper installed successfully!" & vbCrLf & vbCrLf &
                    "Restart the game for changes to take effect.",
                    "Installation Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information)
            Else
                CustomMessageBox.Show(owner,
                    "Failed to install SDL2 wrapper." & vbCrLf &
                    "You may need to run ScalA as administrator.",
                    "Installation Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error)
            End If
        End If
    End Sub

End Module
