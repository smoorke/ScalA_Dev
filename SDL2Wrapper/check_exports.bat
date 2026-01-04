@echo off
call "C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvarsall.bat" x86 >nul 2>&1
dumpbin /exports C:\Users\zzies\Documents\Scala\SDL2Wrapper\SDL2.dll
