/*
 * SDL2 Wrapper DLL for ScalA
 * Intercepts mouse position calls and transforms coordinates based on zoom state
 *
 * Build: cl /LD SDL2Wrapper.c /Fe:SDL2.dll /link user32.lib kernel32.lib
 */

#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <stdint.h>

/* SDL2 typedefs */
typedef uint32_t Uint32;
typedef int32_t Sint32;

/* Shared memory structure - must match ScalA's definition */
#pragma pack(push, 1)
typedef struct {
    DWORD scalaPid;           /* ScalA process ID */
    DWORD gameWindowHandle;   /* Game window handle being managed */
    int   viewportX;          /* pbZoom screen X position */
    int   viewportY;          /* pbZoom screen Y position */
    int   viewportWidth;      /* pbZoom width (what user sees) */
    int   viewportHeight;     /* pbZoom height (what user sees) */
    int   gameClientWidth;    /* Original game client width (rcC.Width) */
    int   gameClientHeight;   /* Original game client height (rcC.Height) */
    int   enabled;            /* 1 if transform should be applied */
} ScalAZoomState;
#pragma pack(pop)

/* Function pointer types for real SDL2 functions */
typedef Uint32 (*SDL_GetMouseState_t)(int* x, int* y);
typedef Uint32 (*SDL_GetGlobalMouseState_t)(int* x, int* y);
typedef Uint32 (*SDL_GetRelativeMouseState_t)(int* x, int* y);

/* Handles */
static HMODULE hRealSDL2 = NULL;
static HANDLE hMapFile = NULL;
static ScalAZoomState* pZoomState = NULL;

/* Real function pointers */
static SDL_GetMouseState_t Real_SDL_GetMouseState = NULL;
static SDL_GetGlobalMouseState_t Real_SDL_GetGlobalMouseState = NULL;
static SDL_GetRelativeMouseState_t Real_SDL_GetRelativeMouseState = NULL;

/* Helper: Check if ScalA process is still running */
static BOOL IsScalARunning(void) {
    if (!pZoomState || pZoomState->scalaPid == 0) return FALSE;

    HANDLE hProcess = OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION, FALSE, pZoomState->scalaPid);
    if (hProcess == NULL) return FALSE;

    DWORD exitCode;
    BOOL running = GetExitCodeProcess(hProcess, &exitCode) && (exitCode == STILL_ACTIVE);
    CloseHandle(hProcess);
    return running;
}

/* Helper: Check if transform should be applied */
static BOOL ShouldTransform(void) {
    if (!pZoomState) return FALSE;
    if (!pZoomState->enabled) return FALSE;
    if (!IsScalARunning()) return FALSE;
    if (pZoomState->viewportWidth <= 0 || pZoomState->viewportHeight <= 0) return FALSE;
    if (pZoomState->gameClientWidth <= 0 || pZoomState->gameClientHeight <= 0) return FALSE;
    return TRUE;
}

/* Helper: Transform screen coordinates to game coordinates */
static void TransformMouseCoords(int screenX, int screenY, int* gameX, int* gameY) {
    if (!ShouldTransform()) {
        *gameX = screenX;
        *gameY = screenY;
        return;
    }

    /* Get mouse position relative to viewport */
    int relX = screenX - pZoomState->viewportX;
    int relY = screenY - pZoomState->viewportY;

    /* Check if mouse is within viewport bounds */
    if (relX < 0 || relX >= pZoomState->viewportWidth ||
        relY < 0 || relY >= pZoomState->viewportHeight) {
        /* Mouse is outside viewport - clamp to edges */
        if (relX < 0) relX = 0;
        if (relX >= pZoomState->viewportWidth) relX = pZoomState->viewportWidth - 1;
        if (relY < 0) relY = 0;
        if (relY >= pZoomState->viewportHeight) relY = pZoomState->viewportHeight - 1;
    }

    /* Map from viewport coords to game client coords */
    /* gameCoord = relCoord * gameSize / viewportSize */
    *gameX = (relX * pZoomState->gameClientWidth) / pZoomState->viewportWidth;
    *gameY = (relY * pZoomState->gameClientHeight) / pZoomState->viewportHeight;
}

/* Open shared memory for zoom state */
static void OpenZoomStateMapping(DWORD gamePid) {
    char mapName[64];
    snprintf(mapName, sizeof(mapName), "ScalA_ZoomState_%lu", (unsigned long)gamePid);

    hMapFile = OpenFileMappingA(FILE_MAP_READ, FALSE, mapName);
    if (hMapFile != NULL) {
        pZoomState = (ScalAZoomState*)MapViewOfFile(hMapFile, FILE_MAP_READ, 0, 0, sizeof(ScalAZoomState));
    }
}

/* Close shared memory */
static void CloseZoomStateMapping(void) {
    if (pZoomState) {
        UnmapViewOfFile(pZoomState);
        pZoomState = NULL;
    }
    if (hMapFile) {
        CloseHandle(hMapFile);
        hMapFile = NULL;
    }
}

/* Load the real SDL2 DLL and get function pointers */
static BOOL LoadRealSDL2(void) {
    if (hRealSDL2) return TRUE;

    /* Load SDL2_real.dll (renamed original) */
    hRealSDL2 = LoadLibraryA("SDL2_real.dll");
    if (!hRealSDL2) {
        /* Fallback: try SDL2_compat.dll */
        hRealSDL2 = LoadLibraryA("SDL2_compat.dll");
    }
    if (!hRealSDL2) return FALSE;

    /* Get mouse function pointers */
    Real_SDL_GetMouseState = (SDL_GetMouseState_t)GetProcAddress(hRealSDL2, "SDL_GetMouseState");
    Real_SDL_GetGlobalMouseState = (SDL_GetGlobalMouseState_t)GetProcAddress(hRealSDL2, "SDL_GetGlobalMouseState");
    Real_SDL_GetRelativeMouseState = (SDL_GetRelativeMouseState_t)GetProcAddress(hRealSDL2, "SDL_GetRelativeMouseState");

    return TRUE;
}

/* ============================================================================
 * EXPORTED FUNCTIONS - Mouse position interception
 * ============================================================================ */

__declspec(dllexport) Uint32 SDL_GetMouseState(int* x, int* y) {
    if (!Real_SDL_GetMouseState) {
        if (x) *x = 0;
        if (y) *y = 0;
        return 0;
    }

    int rawX, rawY;
    Uint32 buttons = Real_SDL_GetMouseState(&rawX, &rawY);

    if (ShouldTransform()) {
        /* Get global mouse position for transform */
        POINT pt;
        GetCursorPos(&pt);
        TransformMouseCoords(pt.x, pt.y, &rawX, &rawY);
    }

    if (x) *x = rawX;
    if (y) *y = rawY;
    return buttons;
}

__declspec(dllexport) Uint32 SDL_GetGlobalMouseState(int* x, int* y) {
    if (!Real_SDL_GetGlobalMouseState) {
        if (x) *x = 0;
        if (y) *y = 0;
        return 0;
    }

    int rawX, rawY;
    Uint32 buttons = Real_SDL_GetGlobalMouseState(&rawX, &rawY);

    if (ShouldTransform()) {
        TransformMouseCoords(rawX, rawY, &rawX, &rawY);
    }

    if (x) *x = rawX;
    if (y) *y = rawY;
    return buttons;
}

__declspec(dllexport) Uint32 SDL_GetRelativeMouseState(int* x, int* y) {
    /* Relative mouse state doesn't need transformation - it's delta movement */
    if (!Real_SDL_GetRelativeMouseState) {
        if (x) *x = 0;
        if (y) *y = 0;
        return 0;
    }
    return Real_SDL_GetRelativeMouseState(x, y);
}

/* ============================================================================
 * DLL Entry Point
 * ============================================================================ */

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved) {
    switch (fdwReason) {
        case DLL_PROCESS_ATTACH:
            DisableThreadLibraryCalls(hinstDLL);
            LoadRealSDL2();
            OpenZoomStateMapping(GetCurrentProcessId());
            break;

        case DLL_PROCESS_DETACH:
            CloseZoomStateMapping();
            if (hRealSDL2) {
                FreeLibrary(hRealSDL2);
                hRealSDL2 = NULL;
            }
            break;
    }
    return TRUE;
}
