@echo off
call "C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvarsall.bat" x86
cd /d C:\Users\zzies\Documents\Scala\SDL2Wrapper
cl /nologo /O2 /W3 /LD SDL2Wrapper_v2.c /Fe:SDL2.dll /link /DEF:SDL2Wrapper.def user32.lib kernel32.lib
if errorlevel 1 (
    echo BUILD FAILED
) else (
    echo BUILD SUCCESS
    dir SDL2.dll
)
