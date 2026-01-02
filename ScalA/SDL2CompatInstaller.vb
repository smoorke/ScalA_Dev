Imports System.IO
Imports System.Net.Http
Imports System.Web.Script.Serialization

Public Class SDL2CompatInstaller

    Private Const SDL2_COMPAT_RELEASE_LATEST_JSON As String = "https://api.github.com/repos/libsdl-org/sdl2-compat/releases/latest"
    Private Const SDL2_COMPAT_GITHUB_URL As String = "https://github.com/libsdl-org/sdl2-compat"

    Private _targetPath As String = String.Empty

    Public Property TargetPath As String
        Get
            Return _targetPath
        End Get
        Set(value As String)
            _targetPath = value
            If Not String.IsNullOrEmpty(value) Then
                txtPath.Text = value
            End If
        End Set
    End Property

    Private Sub SDL2CompatInstaller_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Try to auto-detect path from current client
        If FrmMain.AltPP IsNot Nothing AndAlso FrmMain.AltPP.Id <> 0 Then
            Try
                Dim clientPath As String = FrmMain.AltPP.FinalPath
                If Not String.IsNullOrEmpty(clientPath) Then
                    Dim dir As String = IO.Path.GetDirectoryName(clientPath)

                    txtPath.Text = dir

                End If
            Catch ex As Exception
                dBug.Print($"SDL2CompatInstaller: Failed to detect client path: {ex.Message}")
            End Try
        End If

        ' If no path detected and TargetPath was set, use that
        If String.IsNullOrEmpty(txtPath.Text) AndAlso Not String.IsNullOrEmpty(_targetPath) Then
            txtPath.Text = _targetPath
        End If

        UpdateLabelAndButtonState()
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim fpd As New FolderPicker With {
                .Title = "Select the game directory where SDL3.dll should be installed",
                .Multiselect = False
        }

        If Not String.IsNullOrEmpty(txtPath.Text) AndAlso Directory.Exists(txtPath.Text) Then
            fpd.InputPath = txtPath.Text
        ElseIf Directory.Exists("C:\Program Files (x86)\Astonia Resurgence\client\bin") Then
            fpd.InputPath = "C:\Program Files (x86)\Astonia Resurgence\client\bin"
            'elseif directory.exists steaminstallpath then
            ' fpd.inputpath = steaminstallpath

        End If

        If fpd.ShowDialog(Me) = True Then
            txtPath.Text = fpd.ResultPath
        End If
    End Sub

    Private Sub txtPath_TextChanged(sender As Object, e As EventArgs) Handles txtPath.TextChanged
        UpdateLabelAndButtonState()
    End Sub

    Private Sub UpdateLabelAndButtonState()
        lblStatus.Text = ""
        btnInstall.Enabled = False
        If String.IsNullOrEmpty(txtPath.Text) Then Exit Sub
        If Not Directory.Exists(txtPath.Text) Then
            lblStatus.Text = "Invalid Path"
        Else
            If Not File.Exists(IO.Path.Combine(txtPath.Text, "moac.exe")) Then
                lblStatus.Text = "Not an Astonia game directory"
            Else
                If Not File.Exists(IO.Path.Combine(txtPath.Text, "SDL2.dll")) Then
                    lblStatus.Text = "Not an Astonia SDL game client"
                Else
                    btnInstall.Enabled = True
                    Dim info As FileVersionInfo = FileVersionInfo.GetVersionInfo(IO.Path.Combine(txtPath.Text, "SDL2.dll"))
                    If Not File.Exists(IO.Path.Combine(txtPath.Text, "SDL3.dll")) Then
                        btnInstall.Text = "Install/Repair"
                    ElseIf info.ProductName = "Simple DirectMedia Layer 2.0 wrapper" Then
                        Dim version = info.FileVersion.Replace(",", ".").ToCharArray.Where(Function(c) Not Char.IsWhiteSpace(c)).ToArray()
                        lblStatus.Text = $"SDL2 compat {New String(version, 0, Array.LastIndexOf(version, "."c))} already installed"
                        btnInstall.Text = "Update/Repair"
                    Else
                        lblStatus.Text = "Unknown Version Installed"
                        btnInstall.Text = "Install/Repair"
                    End If
                End If
            End If
        End If
    End Sub

    Private Async Sub btnInstall_Click(sender As Object, e As EventArgs) Handles btnInstall.Click
        Dim targetDir As String = txtPath.Text.Trim()

        If Not Directory.Exists(targetDir) Then
            CustomMessageBox.Show(Me, "The specified directory does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim sdl2DllPath As String = IO.Path.Combine(targetDir, "SDL2.dll")
        Dim sdl3DllPath As String = IO.Path.Combine(targetDir, "SDL3.dll")

        ' Check if SDL2.dll or SDL3.dll already exist
        Dim existingFiles As New List(Of String)
        If File.Exists(sdl2DllPath) Then existingFiles.Add("SDL2.dll")
        If File.Exists(sdl3DllPath) Then existingFiles.Add("SDL3.dll")

        If existingFiles.Count > 0 Then
            ' Create backups
            For Each dllName In existingFiles
                Dim dllPath = IO.Path.Combine(targetDir, dllName)
                Dim backupPath As String = dllPath & ".backup"
                Dim backupNum As Integer = 1
                While File.Exists(backupPath)
                    backupPath = dllPath & $".backup{backupNum}"
                    backupNum += 1
                End While

                Try
                    File.Copy(dllPath, backupPath, False)
                    dBug.Print($"SDL2CompatInstaller: Backed up existing {dllName} to {backupPath}")
                Catch ex As Exception
                    Dim result = CustomMessageBox.Show(Me,
                            $"Failed to create backup of existing {dllName}:{vbCrLf}{ex.Message}{vbCrLf}{vbCrLf}Continue anyway?",
                            "Backup Failed", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    If result <> DialogResult.Yes Then Return
                End Try
            Next
        Else
            Dim result = CustomMessageBox.Show(Me,
                    $"{String.Join(" and ", existingFiles)} already exist in this directory. Overwrite?",
                    "Confirm Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result <> DialogResult.Yes Then Return
        End If

        ' Disable controls during download
        btnInstall.Enabled = False
        btnBrowse.Enabled = False
        txtPath.Enabled = False
        progressBar.Visible = True
        progressBar.Style = ProgressBarStyle.Marquee
        lblStatus.Text = "Downloading SDL2-compat..."

        Try
            Dim tempZipPath As String = IO.Path.Combine(IO.Path.GetTempPath(), "sdl2-compat.zip")
            Dim tempExtractPath As String = IO.Path.Combine(IO.Path.GetTempPath(), "sdl2-compat-extract")

            ' Download the zip file
            Using client As New HttpClient()
                client.Timeout = TimeSpan.FromMinutes(1)
                client.DefaultRequestHeaders.Add("User-Agent", "ScalA.exe/SDL2-Compat Installer for Astonia 3") ' GitHub API requires User-Agent

                Using jsonResponse = Await client.GetAsync(SDL2_COMPAT_RELEASE_LATEST_JSON, HttpCompletionOption.ResponseHeadersRead)
                    jsonResponse.EnsureSuccessStatusCode()

                    'parse JSON file, get download URL
                    Dim jsonText As String = Await jsonResponse.Content.ReadAsStringAsync()
                    Dim serializer As New JavaScriptSerializer()
                    Dim assets As Object() = serializer.DeserializeObject(jsonText)("assets")

                    Dim assets0 As Dictionary(Of String, Object) = assets(0)

                    Dim zipUrl As String = assets0.Where(Function(kv) kv.Key = "browser_download_url").FirstOrDefault.Value

                    Debug.Print(zipUrl)

                    Using response = Await client.GetAsync(zipUrl, HttpCompletionOption.ResponseHeadersRead)
                        Using fs As New IO.FileStream(tempZipPath, IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.None)
                            Await response.Content.CopyToAsync(fs)
                        End Using
                    End Using
                End Using
            End Using

            lblStatus.Text = "Extracting..."

            ' Clean up extract directory if it exists
            If Directory.Exists(tempExtractPath) Then
                Directory.Delete(tempExtractPath, True)
            End If

            ' Extract the zip
            System.IO.Compression.ZipFile.ExtractToDirectory(tempZipPath, tempExtractPath)

            ' Find SDL2.dll and SDL3.dll in the extracted files
            Dim extractedSdl2 As String = Directory.GetFiles(tempExtractPath, "SDL2.dll", SearchOption.AllDirectories).FirstOrDefault()
            Dim extractedSdl3 As String = Directory.GetFiles(tempExtractPath, "SDL3.dll", SearchOption.AllDirectories).FirstOrDefault()

            If String.IsNullOrEmpty(extractedSdl2) Then
                Throw New FileNotFoundException("SDL2.dll not found in the downloaded archive.")
            End If
            If String.IsNullOrEmpty(extractedSdl3) Then
                Throw New FileNotFoundException("SDL3.dll not found in the downloaded archive.")
            End If

            lblStatus.Text = "Installing..."

            ' Copy SDL2.dll and SDL3.dll to target directory. Todo: write SDL_compat_Installer.exe, If path is in ProgramFiles it needs elevation. it will also need to terminate and restart all clients that are installed there

            File.Copy(extractedSdl2, sdl2DllPath, True)
            File.Copy(extractedSdl3, sdl3DllPath, True)

            ' Clean up temp files
            Try
                File.Delete(tempZipPath)
                Directory.Delete(tempExtractPath, True)
            Catch
                ' Ignore cleanup errors
            End Try

            progressBar.Visible = False
            lblStatus.Text = "Installation complete!"

            CustomMessageBox.Show(Me,
                $"SDL2-compat has been installed successfully!{vbCrLf}{vbCrLf}" &
                $"Installed to:{vbCrLf}{sdl2DllPath}{vbCrLf}{sdl3DllPath}{vbCrLf}{vbCrLf}",
                "Installation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK

        Catch ex As Exception
            progressBar.Visible = False
            lblStatus.Text = "Installation failed."

            dBug.Print($"SDL2CompatInstaller: Installation failed: {ex.Message}")
            CustomMessageBox.Show(Me,
                $"Installation failed:{vbCrLf}{vbCrLf}{ex.Message}",
                "Installation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            btnInstall.Enabled = False
            btnBrowse.Enabled = True
            txtPath.Enabled = True
        End Try
        Await Task.Delay(500)
        UpdateLabelAndButtonState()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub linkGitHub_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkGitHub.LinkClicked
        Try
            Process.Start(New ProcessStartInfo(SDL2_COMPAT_GITHUB_URL) With {.UseShellExecute = True})
        Catch ex As Exception
            dBug.Print($"SDL2CompatInstaller: Failed to open GitHub link: {ex.Message}")
        End Try
    End Sub

End Class
