/*
 * SDL2 Proxy DLL for ScalA - Mouse Position Mapper
 *
 * Intercepts mouse functions, forwards everything else to SDL2_real.dll
 * Uses GetProcAddress at runtime for all forwarded functions.
 */

#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <stdint.h>
#include <stdio.h>

typedef uint32_t Uint32;

/* Shared memory structure - 28 bytes */
#pragma pack(push, 1)
typedef struct {
    int viewportX;      /* pbZoom screen X */
    int viewportY;      /* pbZoom screen Y */
    int viewportW;      /* pbZoom width */
    int viewportH;      /* pbZoom height */
    int clientW;        /* game client width */
    int clientH;        /* game client height */
    int enabled;        /* 1 = transform active */
} ScalAZoomState;
#pragma pack(pop)

static HMODULE g_hRealSDL = NULL;
static HANDLE g_hMapFile = NULL;
static ScalAZoomState* g_pState = NULL;

/* Function pointer types for mouse functions */
typedef Uint32 (*PFN_SDL_GetMouseState)(int*, int*);
typedef Uint32 (*PFN_SDL_GetGlobalMouseState)(int*, int*);
typedef Uint32 (*PFN_SDL_GetRelativeMouseState)(int*, int*);

static PFN_SDL_GetMouseState g_Real_GetMouseState = NULL;
static PFN_SDL_GetGlobalMouseState g_Real_GetGlobalMouseState = NULL;
static PFN_SDL_GetRelativeMouseState g_Real_GetRelativeMouseState = NULL;

/* Map mouse position from viewport to game client coords */
static void MapMousePos(int* x, int* y) {
    if (!g_pState || !g_pState->enabled) return;
    if (g_pState->viewportW <= 0 || g_pState->viewportH <= 0) return;
    if (g_pState->clientW <= 0 || g_pState->clientH <= 0) return;

    POINT pt;
    GetCursorPos(&pt);

    int relX = pt.x - g_pState->viewportX;
    int relY = pt.y - g_pState->viewportY;

    if (relX >= 0 && relX < g_pState->viewportW &&
        relY >= 0 && relY < g_pState->viewportH) {
        *x = (relX * g_pState->clientW) / g_pState->viewportW;
        *y = (relY * g_pState->clientH) / g_pState->viewportH;
    }
}

static void ConnectSharedMem(void) {
    if (g_hMapFile) return;
    char name[64];
    sprintf(name, "ScalA_ZoomState_%lu", GetCurrentProcessId());
    g_hMapFile = OpenFileMappingA(FILE_MAP_READ, FALSE, name);
    if (g_hMapFile)
        g_pState = (ScalAZoomState*)MapViewOfFile(g_hMapFile, FILE_MAP_READ, 0, 0, sizeof(ScalAZoomState));
}

static void LoadRealSDL(void) {
    if (g_hRealSDL) return;
    g_hRealSDL = LoadLibraryA("SDL2_real.dll");
    if (!g_hRealSDL) return;
    g_Real_GetMouseState = (PFN_SDL_GetMouseState)GetProcAddress(g_hRealSDL, "SDL_GetMouseState");
    g_Real_GetGlobalMouseState = (PFN_SDL_GetGlobalMouseState)GetProcAddress(g_hRealSDL, "SDL_GetGlobalMouseState");
    g_Real_GetRelativeMouseState = (PFN_SDL_GetRelativeMouseState)GetProcAddress(g_hRealSDL, "SDL_GetRelativeMouseState");
}

/* === Our intercepted mouse functions === */

__declspec(dllexport) Uint32 SDL_GetMouseState(int* x, int* y) {
    if (!g_Real_GetMouseState) return 0;
    int mx = 0, my = 0;
    Uint32 buttons = g_Real_GetMouseState(&mx, &my);
    MapMousePos(&mx, &my);
    if (x) *x = mx;
    if (y) *y = my;
    return buttons;
}

__declspec(dllexport) Uint32 SDL_GetGlobalMouseState(int* x, int* y) {
    if (!g_Real_GetGlobalMouseState) return 0;
    int mx = 0, my = 0;
    Uint32 buttons = g_Real_GetGlobalMouseState(&mx, &my);
    MapMousePos(&mx, &my);
    if (x) *x = mx;
    if (y) *y = my;
    return buttons;
}

__declspec(dllexport) Uint32 SDL_GetRelativeMouseState(int* x, int* y) {
    if (!g_Real_GetRelativeMouseState) return 0;
    return g_Real_GetRelativeMouseState(x, y);
}

/* === Macro for forwarded functions === */
#define FORWARD_TO_REAL(name) \
    __declspec(dllexport) void* name(void) { \
        static void* proc = NULL; \
        if (!proc && g_hRealSDL) proc = GetProcAddress(g_hRealSDL, #name); \
        return proc ? ((void*(*)(void))proc)() : NULL; \
    }

/* This approach won't work for variadic or complex functions.
   Instead, use linker export forwarding. */

BOOL WINAPI DllMain(HINSTANCE hInst, DWORD reason, LPVOID reserved) {
    (void)hInst; (void)reserved;
    if (reason == DLL_PROCESS_ATTACH) {
        DisableThreadLibraryCalls(hInst);
        LoadRealSDL();
        ConnectSharedMem();
    } else if (reason == DLL_PROCESS_DETACH) {
        if (g_pState) { UnmapViewOfFile(g_pState); g_pState = NULL; }
        if (g_hMapFile) { CloseHandle(g_hMapFile); g_hMapFile = NULL; }
        if (g_hRealSDL) { FreeLibrary(g_hRealSDL); g_hRealSDL = NULL; }
    }
    return TRUE;
}

/*
 * For forwarding, we need linker-level export forwarding.
 * Generate with: #pragma comment(linker, "/export:SDL_Init=SDL2_real.SDL_Init")
 */
#pragma comment(linker, "/export:SDL_Init=SDL2_real.SDL_Init")
#pragma comment(linker, "/export:SDL_Quit=SDL2_real.SDL_Quit")
#pragma comment(linker, "/export:SDL_InitSubSystem=SDL2_real.SDL_InitSubSystem")
#pragma comment(linker, "/export:SDL_QuitSubSystem=SDL2_real.SDL_QuitSubSystem")
#pragma comment(linker, "/export:SDL_WasInit=SDL2_real.SDL_WasInit")
#pragma comment(linker, "/export:SDL_GetError=SDL2_real.SDL_GetError")
#pragma comment(linker, "/export:SDL_SetError=SDL2_real.SDL_SetError")
#pragma comment(linker, "/export:SDL_ClearError=SDL2_real.SDL_ClearError")
#pragma comment(linker, "/export:SDL_CreateWindow=SDL2_real.SDL_CreateWindow")
#pragma comment(linker, "/export:SDL_DestroyWindow=SDL2_real.SDL_DestroyWindow")
#pragma comment(linker, "/export:SDL_GetWindowSurface=SDL2_real.SDL_GetWindowSurface")
#pragma comment(linker, "/export:SDL_UpdateWindowSurface=SDL2_real.SDL_UpdateWindowSurface")
#pragma comment(linker, "/export:SDL_PollEvent=SDL2_real.SDL_PollEvent")
#pragma comment(linker, "/export:SDL_WaitEvent=SDL2_real.SDL_WaitEvent")
#pragma comment(linker, "/export:SDL_PumpEvents=SDL2_real.SDL_PumpEvents")
#pragma comment(linker, "/export:SDL_PushEvent=SDL2_real.SDL_PushEvent")
#pragma comment(linker, "/export:SDL_PeepEvents=SDL2_real.SDL_PeepEvents")
#pragma comment(linker, "/export:SDL_EventState=SDL2_real.SDL_EventState")
#pragma comment(linker, "/export:SDL_FlushEvent=SDL2_real.SDL_FlushEvent")
#pragma comment(linker, "/export:SDL_FlushEvents=SDL2_real.SDL_FlushEvents")
#pragma comment(linker, "/export:SDL_CreateRenderer=SDL2_real.SDL_CreateRenderer")
#pragma comment(linker, "/export:SDL_DestroyRenderer=SDL2_real.SDL_DestroyRenderer")
#pragma comment(linker, "/export:SDL_RenderClear=SDL2_real.SDL_RenderClear")
#pragma comment(linker, "/export:SDL_RenderCopy=SDL2_real.SDL_RenderCopy")
#pragma comment(linker, "/export:SDL_RenderPresent=SDL2_real.SDL_RenderPresent")
#pragma comment(linker, "/export:SDL_CreateTexture=SDL2_real.SDL_CreateTexture")
#pragma comment(linker, "/export:SDL_CreateTextureFromSurface=SDL2_real.SDL_CreateTextureFromSurface")
#pragma comment(linker, "/export:SDL_DestroyTexture=SDL2_real.SDL_DestroyTexture")
#pragma comment(linker, "/export:SDL_UpdateTexture=SDL2_real.SDL_UpdateTexture")
#pragma comment(linker, "/export:SDL_LockTexture=SDL2_real.SDL_LockTexture")
#pragma comment(linker, "/export:SDL_UnlockTexture=SDL2_real.SDL_UnlockTexture")
#pragma comment(linker, "/export:SDL_SetRenderDrawColor=SDL2_real.SDL_SetRenderDrawColor")
#pragma comment(linker, "/export:SDL_GetRenderDrawColor=SDL2_real.SDL_GetRenderDrawColor")
#pragma comment(linker, "/export:SDL_RenderFillRect=SDL2_real.SDL_RenderFillRect")
#pragma comment(linker, "/export:SDL_RenderDrawRect=SDL2_real.SDL_RenderDrawRect")
#pragma comment(linker, "/export:SDL_RenderDrawLine=SDL2_real.SDL_RenderDrawLine")
#pragma comment(linker, "/export:SDL_SetRenderTarget=SDL2_real.SDL_SetRenderTarget")
#pragma comment(linker, "/export:SDL_GetRenderer=SDL2_real.SDL_GetRenderer")
#pragma comment(linker, "/export:SDL_GetRendererInfo=SDL2_real.SDL_GetRendererInfo")
#pragma comment(linker, "/export:SDL_QueryTexture=SDL2_real.SDL_QueryTexture")
#pragma comment(linker, "/export:SDL_GetTicks=SDL2_real.SDL_GetTicks")
#pragma comment(linker, "/export:SDL_Delay=SDL2_real.SDL_Delay")
#pragma comment(linker, "/export:SDL_GetKeyboardState=SDL2_real.SDL_GetKeyboardState")
#pragma comment(linker, "/export:SDL_GetModState=SDL2_real.SDL_GetModState")
#pragma comment(linker, "/export:SDL_SetModState=SDL2_real.SDL_SetModState")
#pragma comment(linker, "/export:SDL_GetKeyFromScancode=SDL2_real.SDL_GetKeyFromScancode")
#pragma comment(linker, "/export:SDL_GetScancodeFromKey=SDL2_real.SDL_GetScancodeFromKey")
#pragma comment(linker, "/export:SDL_GetKeyName=SDL2_real.SDL_GetKeyName")
#pragma comment(linker, "/export:SDL_ShowCursor=SDL2_real.SDL_ShowCursor")
#pragma comment(linker, "/export:SDL_SetCursor=SDL2_real.SDL_SetCursor")
#pragma comment(linker, "/export:SDL_GetCursor=SDL2_real.SDL_GetCursor")
#pragma comment(linker, "/export:SDL_CreateSystemCursor=SDL2_real.SDL_CreateSystemCursor")
#pragma comment(linker, "/export:SDL_FreeCursor=SDL2_real.SDL_FreeCursor")
#pragma comment(linker, "/export:SDL_WarpMouseInWindow=SDL2_real.SDL_WarpMouseInWindow")
#pragma comment(linker, "/export:SDL_SetRelativeMouseMode=SDL2_real.SDL_SetRelativeMouseMode")
#pragma comment(linker, "/export:SDL_GetRelativeMouseMode=SDL2_real.SDL_GetRelativeMouseMode")
#pragma comment(linker, "/export:SDL_GetMouseFocus=SDL2_real.SDL_GetMouseFocus")
#pragma comment(linker, "/export:SDL_CaptureMouse=SDL2_real.SDL_CaptureMouse")
#pragma comment(linker, "/export:SDL_CreateRGBSurface=SDL2_real.SDL_CreateRGBSurface")
#pragma comment(linker, "/export:SDL_CreateRGBSurfaceFrom=SDL2_real.SDL_CreateRGBSurfaceFrom")
#pragma comment(linker, "/export:SDL_CreateRGBSurfaceWithFormat=SDL2_real.SDL_CreateRGBSurfaceWithFormat")
#pragma comment(linker, "/export:SDL_FreeSurface=SDL2_real.SDL_FreeSurface")
#pragma comment(linker, "/export:SDL_LockSurface=SDL2_real.SDL_LockSurface")
#pragma comment(linker, "/export:SDL_UnlockSurface=SDL2_real.SDL_UnlockSurface")
#pragma comment(linker, "/export:SDL_BlitSurface=SDL2_real.SDL_BlitSurface")
#pragma comment(linker, "/export:SDL_BlitScaled=SDL2_real.SDL_BlitScaled")
#pragma comment(linker, "/export:SDL_FillRect=SDL2_real.SDL_FillRect")
#pragma comment(linker, "/export:SDL_SetColorKey=SDL2_real.SDL_SetColorKey")
#pragma comment(linker, "/export:SDL_SetClipRect=SDL2_real.SDL_SetClipRect")
#pragma comment(linker, "/export:SDL_MapRGB=SDL2_real.SDL_MapRGB")
#pragma comment(linker, "/export:SDL_MapRGBA=SDL2_real.SDL_MapRGBA")
#pragma comment(linker, "/export:SDL_GetRGB=SDL2_real.SDL_GetRGB")
#pragma comment(linker, "/export:SDL_GetRGBA=SDL2_real.SDL_GetRGBA")
#pragma comment(linker, "/export:SDL_ConvertSurface=SDL2_real.SDL_ConvertSurface")
#pragma comment(linker, "/export:SDL_ConvertSurfaceFormat=SDL2_real.SDL_ConvertSurfaceFormat")
#pragma comment(linker, "/export:SDL_AllocFormat=SDL2_real.SDL_AllocFormat")
#pragma comment(linker, "/export:SDL_FreeFormat=SDL2_real.SDL_FreeFormat")
#pragma comment(linker, "/export:SDL_SetPaletteColors=SDL2_real.SDL_SetPaletteColors")
#pragma comment(linker, "/export:SDL_AllocPalette=SDL2_real.SDL_AllocPalette")
#pragma comment(linker, "/export:SDL_FreePalette=SDL2_real.SDL_FreePalette")
#pragma comment(linker, "/export:SDL_LoadBMP_RW=SDL2_real.SDL_LoadBMP_RW")
#pragma comment(linker, "/export:SDL_SaveBMP_RW=SDL2_real.SDL_SaveBMP_RW")
#pragma comment(linker, "/export:SDL_RWFromFile=SDL2_real.SDL_RWFromFile")
#pragma comment(linker, "/export:SDL_RWFromMem=SDL2_real.SDL_RWFromMem")
#pragma comment(linker, "/export:SDL_RWclose=SDL2_real.SDL_RWclose")
#pragma comment(linker, "/export:SDL_RWread=SDL2_real.SDL_RWread")
#pragma comment(linker, "/export:SDL_RWwrite=SDL2_real.SDL_RWwrite")
#pragma comment(linker, "/export:SDL_RWseek=SDL2_real.SDL_RWseek")
#pragma comment(linker, "/export:SDL_RWtell=SDL2_real.SDL_RWtell")
#pragma comment(linker, "/export:SDL_GetWindowSize=SDL2_real.SDL_GetWindowSize")
#pragma comment(linker, "/export:SDL_SetWindowSize=SDL2_real.SDL_SetWindowSize")
#pragma comment(linker, "/export:SDL_GetWindowPosition=SDL2_real.SDL_GetWindowPosition")
#pragma comment(linker, "/export:SDL_SetWindowPosition=SDL2_real.SDL_SetWindowPosition")
#pragma comment(linker, "/export:SDL_GetWindowTitle=SDL2_real.SDL_GetWindowTitle")
#pragma comment(linker, "/export:SDL_SetWindowTitle=SDL2_real.SDL_SetWindowTitle")
#pragma comment(linker, "/export:SDL_ShowWindow=SDL2_real.SDL_ShowWindow")
#pragma comment(linker, "/export:SDL_HideWindow=SDL2_real.SDL_HideWindow")
#pragma comment(linker, "/export:SDL_RaiseWindow=SDL2_real.SDL_RaiseWindow")
#pragma comment(linker, "/export:SDL_MaximizeWindow=SDL2_real.SDL_MaximizeWindow")
#pragma comment(linker, "/export:SDL_MinimizeWindow=SDL2_real.SDL_MinimizeWindow")
#pragma comment(linker, "/export:SDL_RestoreWindow=SDL2_real.SDL_RestoreWindow")
#pragma comment(linker, "/export:SDL_GetWindowFlags=SDL2_real.SDL_GetWindowFlags")
#pragma comment(linker, "/export:SDL_GetWindowID=SDL2_real.SDL_GetWindowID")
#pragma comment(linker, "/export:SDL_GetWindowFromID=SDL2_real.SDL_GetWindowFromID")
#pragma comment(linker, "/export:SDL_SetWindowFullscreen=SDL2_real.SDL_SetWindowFullscreen")
#pragma comment(linker, "/export:SDL_SetWindowGrab=SDL2_real.SDL_SetWindowGrab")
#pragma comment(linker, "/export:SDL_GetWindowGrab=SDL2_real.SDL_GetWindowGrab")
#pragma comment(linker, "/export:SDL_SetWindowIcon=SDL2_real.SDL_SetWindowIcon")
#pragma comment(linker, "/export:SDL_SetWindowBordered=SDL2_real.SDL_SetWindowBordered")
#pragma comment(linker, "/export:SDL_GetDisplayMode=SDL2_real.SDL_GetDisplayMode")
#pragma comment(linker, "/export:SDL_GetCurrentDisplayMode=SDL2_real.SDL_GetCurrentDisplayMode")
#pragma comment(linker, "/export:SDL_GetDesktopDisplayMode=SDL2_real.SDL_GetDesktopDisplayMode")
#pragma comment(linker, "/export:SDL_GetNumVideoDisplays=SDL2_real.SDL_GetNumVideoDisplays")
#pragma comment(linker, "/export:SDL_GetNumDisplayModes=SDL2_real.SDL_GetNumDisplayModes")
#pragma comment(linker, "/export:SDL_GetDisplayName=SDL2_real.SDL_GetDisplayName")
#pragma comment(linker, "/export:SDL_GetDisplayBounds=SDL2_real.SDL_GetDisplayBounds")
#pragma comment(linker, "/export:SDL_GetVersion=SDL2_real.SDL_GetVersion")
#pragma comment(linker, "/export:SDL_GetRevision=SDL2_real.SDL_GetRevision")
#pragma comment(linker, "/export:SDL_GetPlatform=SDL2_real.SDL_GetPlatform")
#pragma comment(linker, "/export:SDL_GetBasePath=SDL2_real.SDL_GetBasePath")
#pragma comment(linker, "/export:SDL_GetPrefPath=SDL2_real.SDL_GetPrefPath")
#pragma comment(linker, "/export:SDL_OpenAudio=SDL2_real.SDL_OpenAudio")
#pragma comment(linker, "/export:SDL_CloseAudio=SDL2_real.SDL_CloseAudio")
#pragma comment(linker, "/export:SDL_PauseAudio=SDL2_real.SDL_PauseAudio")
#pragma comment(linker, "/export:SDL_MixAudio=SDL2_real.SDL_MixAudio")
#pragma comment(linker, "/export:SDL_LockAudio=SDL2_real.SDL_LockAudio")
#pragma comment(linker, "/export:SDL_UnlockAudio=SDL2_real.SDL_UnlockAudio")
#pragma comment(linker, "/export:SDL_LoadWAV_RW=SDL2_real.SDL_LoadWAV_RW")
#pragma comment(linker, "/export:SDL_FreeWAV=SDL2_real.SDL_FreeWAV")
#pragma comment(linker, "/export:SDL_OpenAudioDevice=SDL2_real.SDL_OpenAudioDevice")
#pragma comment(linker, "/export:SDL_CloseAudioDevice=SDL2_real.SDL_CloseAudioDevice")
#pragma comment(linker, "/export:SDL_PauseAudioDevice=SDL2_real.SDL_PauseAudioDevice")
#pragma comment(linker, "/export:SDL_QueueAudio=SDL2_real.SDL_QueueAudio")
#pragma comment(linker, "/export:SDL_GetQueuedAudioSize=SDL2_real.SDL_GetQueuedAudioSize")
#pragma comment(linker, "/export:SDL_ClearQueuedAudio=SDL2_real.SDL_ClearQueuedAudio")
#pragma comment(linker, "/export:SDL_GetNumAudioDevices=SDL2_real.SDL_GetNumAudioDevices")
#pragma comment(linker, "/export:SDL_GetAudioDeviceName=SDL2_real.SDL_GetAudioDeviceName")
#pragma comment(linker, "/export:SDL_MixAudioFormat=SDL2_real.SDL_MixAudioFormat")
#pragma comment(linker, "/export:SDL_Log=SDL2_real.SDL_Log")
#pragma comment(linker, "/export:SDL_LogError=SDL2_real.SDL_LogError")
#pragma comment(linker, "/export:SDL_LogWarn=SDL2_real.SDL_LogWarn")
#pragma comment(linker, "/export:SDL_LogInfo=SDL2_real.SDL_LogInfo")
#pragma comment(linker, "/export:SDL_LogDebug=SDL2_real.SDL_LogDebug")
#pragma comment(linker, "/export:SDL_LogVerbose=SDL2_real.SDL_LogVerbose")
#pragma comment(linker, "/export:SDL_LogMessage=SDL2_real.SDL_LogMessage")
#pragma comment(linker, "/export:SDL_LogSetOutputFunction=SDL2_real.SDL_LogSetOutputFunction")
#pragma comment(linker, "/export:SDL_LogSetAllPriority=SDL2_real.SDL_LogSetAllPriority")
#pragma comment(linker, "/export:SDL_LogSetPriority=SDL2_real.SDL_LogSetPriority")
#pragma comment(linker, "/export:SDL_malloc=SDL2_real.SDL_malloc")
#pragma comment(linker, "/export:SDL_free=SDL2_real.SDL_free")
#pragma comment(linker, "/export:SDL_realloc=SDL2_real.SDL_realloc")
#pragma comment(linker, "/export:SDL_memset=SDL2_real.SDL_memset")
#pragma comment(linker, "/export:SDL_memcpy=SDL2_real.SDL_memcpy")
#pragma comment(linker, "/export:SDL_memmove=SDL2_real.SDL_memmove")
#pragma comment(linker, "/export:SDL_strlen=SDL2_real.SDL_strlen")
#pragma comment(linker, "/export:SDL_strcmp=SDL2_real.SDL_strcmp")
#pragma comment(linker, "/export:SDL_strncmp=SDL2_real.SDL_strncmp")
#pragma comment(linker, "/export:SDL_strcasecmp=SDL2_real.SDL_strcasecmp")
#pragma comment(linker, "/export:SDL_strncasecmp=SDL2_real.SDL_strncasecmp")
#pragma comment(linker, "/export:SDL_strdup=SDL2_real.SDL_strdup")
#pragma comment(linker, "/export:SDL_snprintf=SDL2_real.SDL_snprintf")
#pragma comment(linker, "/export:SDL_SetHint=SDL2_real.SDL_SetHint")
#pragma comment(linker, "/export:SDL_GetHint=SDL2_real.SDL_GetHint")
#pragma comment(linker, "/export:SDL_SetClipboardText=SDL2_real.SDL_SetClipboardText")
#pragma comment(linker, "/export:SDL_GetClipboardText=SDL2_real.SDL_GetClipboardText")
#pragma comment(linker, "/export:SDL_HasClipboardText=SDL2_real.SDL_HasClipboardText")
#pragma comment(linker, "/export:SDL_AddTimer=SDL2_real.SDL_AddTimer")
#pragma comment(linker, "/export:SDL_RemoveTimer=SDL2_real.SDL_RemoveTimer")
#pragma comment(linker, "/export:SDL_GetPerformanceCounter=SDL2_real.SDL_GetPerformanceCounter")
#pragma comment(linker, "/export:SDL_GetPerformanceFrequency=SDL2_real.SDL_GetPerformanceFrequency")
#pragma comment(linker, "/export:SDL_GL_SetAttribute=SDL2_real.SDL_GL_SetAttribute")
#pragma comment(linker, "/export:SDL_GL_GetAttribute=SDL2_real.SDL_GL_GetAttribute")
#pragma comment(linker, "/export:SDL_GL_CreateContext=SDL2_real.SDL_GL_CreateContext")
#pragma comment(linker, "/export:SDL_GL_DeleteContext=SDL2_real.SDL_GL_DeleteContext")
#pragma comment(linker, "/export:SDL_GL_MakeCurrent=SDL2_real.SDL_GL_MakeCurrent")
#pragma comment(linker, "/export:SDL_GL_SwapWindow=SDL2_real.SDL_GL_SwapWindow")
#pragma comment(linker, "/export:SDL_GL_SetSwapInterval=SDL2_real.SDL_GL_SetSwapInterval")
#pragma comment(linker, "/export:SDL_GL_GetSwapInterval=SDL2_real.SDL_GL_GetSwapInterval")
#pragma comment(linker, "/export:SDL_GL_GetProcAddress=SDL2_real.SDL_GL_GetProcAddress")
#pragma comment(linker, "/export:SDL_GL_LoadLibrary=SDL2_real.SDL_GL_LoadLibrary")
#pragma comment(linker, "/export:SDL_GL_UnloadLibrary=SDL2_real.SDL_GL_UnloadLibrary")
#pragma comment(linker, "/export:SDL_GL_ExtensionSupported=SDL2_real.SDL_GL_ExtensionSupported")
#pragma comment(linker, "/export:SDL_NumJoysticks=SDL2_real.SDL_NumJoysticks")
#pragma comment(linker, "/export:SDL_JoystickOpen=SDL2_real.SDL_JoystickOpen")
#pragma comment(linker, "/export:SDL_JoystickClose=SDL2_real.SDL_JoystickClose")
#pragma comment(linker, "/export:SDL_JoystickName=SDL2_real.SDL_JoystickName")
#pragma comment(linker, "/export:SDL_JoystickGetAxis=SDL2_real.SDL_JoystickGetAxis")
#pragma comment(linker, "/export:SDL_JoystickGetButton=SDL2_real.SDL_JoystickGetButton")
#pragma comment(linker, "/export:SDL_JoystickNumAxes=SDL2_real.SDL_JoystickNumAxes")
#pragma comment(linker, "/export:SDL_JoystickNumButtons=SDL2_real.SDL_JoystickNumButtons")
#pragma comment(linker, "/export:SDL_JoystickUpdate=SDL2_real.SDL_JoystickUpdate")
#pragma comment(linker, "/export:SDL_JoystickEventState=SDL2_real.SDL_JoystickEventState")
#pragma comment(linker, "/export:SDL_IsGameController=SDL2_real.SDL_IsGameController")
#pragma comment(linker, "/export:SDL_GameControllerOpen=SDL2_real.SDL_GameControllerOpen")
#pragma comment(linker, "/export:SDL_GameControllerClose=SDL2_real.SDL_GameControllerClose")
#pragma comment(linker, "/export:SDL_GameControllerName=SDL2_real.SDL_GameControllerName")
#pragma comment(linker, "/export:SDL_GameControllerGetAxis=SDL2_real.SDL_GameControllerGetAxis")
#pragma comment(linker, "/export:SDL_GameControllerGetButton=SDL2_real.SDL_GameControllerGetButton")
#pragma comment(linker, "/export:SDL_GameControllerUpdate=SDL2_real.SDL_GameControllerUpdate")
#pragma comment(linker, "/export:SDL_RegisterEvents=SDL2_real.SDL_RegisterEvents")
#pragma comment(linker, "/export:SDL_StartTextInput=SDL2_real.SDL_StartTextInput")
#pragma comment(linker, "/export:SDL_StopTextInput=SDL2_real.SDL_StopTextInput")
#pragma comment(linker, "/export:SDL_SetTextInputRect=SDL2_real.SDL_SetTextInputRect")
#pragma comment(linker, "/export:SDL_GetKeyboardFocus=SDL2_real.SDL_GetKeyboardFocus")
