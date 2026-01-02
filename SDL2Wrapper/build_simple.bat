@echo off
call "C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvarsall.bat" x86 >nul 2>&1
cd /d C:\Users\zzies\Documents\Scala\SDL2Wrapper
echo Building SDL2 Wrapper (no forwarding, just 3 mouse exports)...
cl /nologo /O2 /W3 /LD /D_CRT_SECURE_NO_WARNINGS SDL2Wrapper_v2.c /Fe:SDL2.dll /link user32.lib kernel32.lib
if exist SDL2.dll (
    echo.
    echo SUCCESS:
    dir SDL2.dll
) else (
    echo FAILED
)
