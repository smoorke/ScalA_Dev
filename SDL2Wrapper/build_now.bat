@echo off
call "C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvarsall.bat" x86 >nul 2>&1
cd /d C:\Users\zzies\Documents\Scala\SDL2Wrapper
echo Building SDL2 Wrapper...
cl /nologo /O2 /W3 /LD SDL2Wrapper_v3.c /Fe:SDL2.dll /link user32.lib kernel32.lib
if exist SDL2.dll (
    echo.
    echo SUCCESS - SDL2.dll built:
    dir /b SDL2.dll SDL2.lib SDL2.exp 2>nul
) else (
    echo.
    echo FAILED - SDL2.dll not created
)
