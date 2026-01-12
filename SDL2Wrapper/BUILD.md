# SDL2 Wrapper Build Guide

This wrapper DLL fixes mouse position jitter when using ScalA's zoom feature with SDL2 games.

## Prerequisites

- **Visual Studio 2022** (Community edition is fine)
  - Install "Desktop development with C++" workload
  - Ensure "MSVC v143" and "Windows 10/11 SDK" are selected

## Building the DLL

### Option 1: Using the Build Script (Recommended)

1. Open **Developer Command Prompt for VS 2022**
   - Start Menu → Visual Studio 2022 → Developer Command Prompt

2. Navigate to the SDL2Wrapper folder:
   ```
   cd C:\path\to\ScalA\SDL2Wrapper
   ```

3. Run the build script:
   ```
   build_simple.bat
   ```

4. If successful, you'll see `SDL2.dll` (about 123 KB) in the folder.

### Option 2: Manual Build

1. Open **Developer Command Prompt for VS 2022**

2. Run:
   ```
   cl /nologo /O2 /W3 /LD /D_CRT_SECURE_NO_WARNINGS SDL2Wrapper_v2.c /Fe:SDL2.dll /link user32.lib kernel32.lib
   ```

## Installation

### Automatic (Recommended)

1. Right-click `install.bat` → **Run as administrator**

This will:
- Rename the game's `SDL2.dll` to `SDL2_real.dll` (backup)
- Copy the wrapper as the new `SDL2.dll`

### Manual Installation

1. Go to your game's folder containing `SDL2.dll`:
   ```
   C:\Program Files (x86)\Astonia Resurgence\client\bin\
   ```

2. Rename the original:
   ```
   SDL2.dll → SDL2_real.dll
   ```

3. Copy the wrapper:
   ```
   Copy SDL2Wrapper\SDL2.dll → game's bin folder
   ```

## Uninstallation

Run `uninstall.bat` as administrator, or manually:
1. Delete the wrapper `SDL2.dll`
2. Rename `SDL2_real.dll` back to `SDL2.dll`

## How It Works

1. ScalA writes zoom viewport info to shared memory
2. The wrapper intercepts `SDL_GetMouseState` calls
3. Mouse coordinates are transformed from screen space to game client space
4. All other SDL2 functions are forwarded to the original DLL

## Troubleshooting

**Game won't start:**
- Ensure `SDL2_real.dll` exists (the original SDL2.dll renamed)
- Check the wrapper was built for 32-bit (use x86 Developer Command Prompt)

**Mouse still jittery:**
- Verify ScalA is running and attached to the game
- Check that the game uses SDL2 for mouse input

**Build errors:**
- Ensure you're using Developer Command Prompt (not regular cmd)
- Verify Visual Studio C++ tools are installed
