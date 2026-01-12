@echo off
:: Run as Administrator
set GAMEDIR=C:\Program Files (x86)\Astonia Resurgence\client\bin

echo Uninstalling SDL2 Wrapper...

if not exist "%GAMEDIR%\SDL2_real.dll" (
    echo SDL2_real.dll not found - wrapper not installed
    pause
    exit /b 1
)

:: Delete wrapper
del "%GAMEDIR%\SDL2.dll"

:: Restore original
rename "%GAMEDIR%\SDL2_real.dll" SDL2.dll
if errorlevel 1 (
    echo FAILED - Run as Administrator!
    pause
    exit /b 1
)

echo.
echo SUCCESS - Original SDL2.dll restored
pause
