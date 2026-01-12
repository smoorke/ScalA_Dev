@echo off
REM SDL2 Wrapper Build Script
REM Requires Visual Studio Build Tools or Visual Studio with C++ support

setlocal

REM Try to find Visual Studio build tools
set VCVARSALL=
for %%v in (2022 2019 2017) do (
    if exist "C:\Program Files\Microsoft Visual Studio\%%v\Community\VC\Auxiliary\Build\vcvarsall.bat" (
        set VCVARSALL=C:\Program Files\Microsoft Visual Studio\%%v\Community\VC\Auxiliary\Build\vcvarsall.bat
        goto :found
    )
    if exist "C:\Program Files\Microsoft Visual Studio\%%v\Professional\VC\Auxiliary\Build\vcvarsall.bat" (
        set VCVARSALL=C:\Program Files\Microsoft Visual Studio\%%v\Professional\VC\Auxiliary\Build\vcvarsall.bat
        goto :found
    )
    if exist "C:\Program Files\Microsoft Visual Studio\%%v\Enterprise\VC\Auxiliary\Build\vcvarsall.bat" (
        set VCVARSALL=C:\Program Files\Microsoft Visual Studio\%%v\Enterprise\VC\Auxiliary\Build\vcvarsall.bat
        goto :found
    )
    if exist "C:\Program Files\Microsoft Visual Studio\%%v\BuildTools\VC\Auxiliary\Build\vcvarsall.bat" (
        set VCVARSALL=C:\Program Files\Microsoft Visual Studio\%%v\BuildTools\VC\Auxiliary\Build\vcvarsall.bat
        goto :found
    )
    if exist "C:\Program Files (x86)\Microsoft Visual Studio\%%v\Community\VC\Auxiliary\Build\vcvarsall.bat" (
        set VCVARSALL=C:\Program Files (x86)\Microsoft Visual Studio\%%v\Community\VC\Auxiliary\Build\vcvarsall.bat
        goto :found
    )
    if exist "C:\Program Files (x86)\Microsoft Visual Studio\%%v\BuildTools\VC\Auxiliary\Build\vcvarsall.bat" (
        set VCVARSALL=C:\Program Files (x86)\Microsoft Visual Studio\%%v\BuildTools\VC\Auxiliary\Build\vcvarsall.bat
        goto :found
    )
)

echo ERROR: Visual Studio Build Tools not found!
echo Please install Visual Studio or Build Tools with C++ support.
echo Download from: https://visualstudio.microsoft.com/downloads/
exit /b 1

:found
echo Found Visual Studio at: %VCVARSALL%

REM Initialize build environment for x86 (32-bit) - Astonia is likely 32-bit
call "%VCVARSALL%" x86
if errorlevel 1 (
    echo ERROR: Failed to initialize build environment
    exit /b 1
)

echo.
echo Building SDL2 Wrapper DLL (32-bit)...
echo.

REM Compile and link (using simplified v2)
cl /nologo /O2 /W3 /LD ^
   SDL2Wrapper_v2.c ^
   /Fe:SDL2.dll ^
   /link /DEF:SDL2Wrapper.def ^
   user32.lib kernel32.lib

if errorlevel 1 (
    echo.
    echo BUILD FAILED!
    exit /b 1
)

echo.
echo BUILD SUCCESSFUL!
echo.
echo Output files:
echo   SDL2.dll     - The wrapper DLL (copy to game directory)
echo.
echo Installation:
echo   1. Go to your Astonia game directory (where moac.exe is)
echo   2. Rename the existing SDL2.dll to SDL2_real.dll
echo   3. Copy the new SDL2.dll (this wrapper) to the game directory
echo   4. Make sure SDL3.dll is present (from SDL2-compat)
echo.

endlocal
