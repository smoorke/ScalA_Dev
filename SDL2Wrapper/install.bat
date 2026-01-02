@echo off
:: Run as Administrator
set GAMEDIR=C:\Program Files (x86)\Astonia Resurgence\client\bin
set WRAPPERDIR=C:\Users\zzies\Documents\Scala\SDL2Wrapper

echo Installing SDL2 Wrapper...

:: Check if already installed
if exist "%GAMEDIR%\SDL2_real.dll" (
    echo SDL2_real.dll already exists - wrapper may already be installed
    echo Updating wrapper DLL only...
    copy /Y "%WRAPPERDIR%\SDL2.dll" "%GAMEDIR%\SDL2.dll"
    goto done
)

:: Backup original
echo Renaming original SDL2.dll to SDL2_real.dll...
rename "%GAMEDIR%\SDL2.dll" SDL2_real.dll
if errorlevel 1 (
    echo FAILED - Run as Administrator!
    pause
    exit /b 1
)

:: Copy wrapper
echo Copying wrapper SDL2.dll...
copy /Y "%WRAPPERDIR%\SDL2.dll" "%GAMEDIR%\SDL2.dll"
if errorlevel 1 (
    echo FAILED to copy wrapper
    pause
    exit /b 1
)

:done
echo.
echo SUCCESS - Wrapper installed!
echo Original: %GAMEDIR%\SDL2_real.dll
echo Wrapper:  %GAMEDIR%\SDL2.dll
pause
