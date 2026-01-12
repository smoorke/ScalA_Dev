# Build SDL2 Wrapper v3
$ErrorActionPreference = "Stop"

Push-Location $PSScriptRoot
try {
    # Setup VS environment
    $vsPath = "C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvarsall.bat"

    Write-Host "Building SDL2 Wrapper v3..." -ForegroundColor Cyan

    # Use cmd to call vcvarsall and cl in one command
    $buildCmd = @"
call "$vsPath" x86 >nul 2>&1
cl /nologo /O2 /W3 /LD SDL2Wrapper_v3.c /Fe:SDL2.dll /link user32.lib kernel32.lib
"@

    cmd /c $buildCmd

    if (Test-Path "SDL2.dll") {
        $dll = Get-Item "SDL2.dll"
        Write-Host "`nSUCCESS!" -ForegroundColor Green
        Write-Host "  SDL2.dll: $($dll.Length) bytes"
        Write-Host "  Built: $($dll.LastWriteTime)"
    } else {
        Write-Host "`nFAILED - SDL2.dll not created" -ForegroundColor Red
    }
}
finally {
    Pop-Location
}
