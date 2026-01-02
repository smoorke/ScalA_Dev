/*
 * SDL2 Wrapper DLL for ScalA - Simple Mouse Position Mapper
 *
 * Just lies to the game about mouse position:
 * - If mouse is in zoom rect: map position to client rect
 * - Otherwise: pass through unchanged
 *
 * Build: cl /LD SDL2Wrapper_v2.c /Fe:SDL2.dll /link /DEF:SDL2Wrapper.def user32.lib kernel32.lib
 */

#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <stdint.h>

typedef uint32_t Uint32;

/* Shared memory structure - must match ScalA's ZoomStateIPC */
#pragma pack(push, 1)
typedef struct {
    DWORD scalaPid;
    DWORD gameWindowHandle;
    int   zoomRectX;        /* pbZoom screen X */
    int   zoomRectY;        /* pbZoom screen Y */
    int   zoomRectWidth;    /* pbZoom width */
    int   zoomRectHeight;   /* pbZoom height */
    int   clientWidth;      /* game client width */
    int   clientHeight;     /* game client height */
    int   enabled;
} ScalAZoomState;
#pragma pack(pop)

/* Function pointers */
typedef Uint32 (*PFN_SDL_GetMouseState)(int*, int*);
typedef Uint32 (*PFN_SDL_GetGlobalMouseState)(int*, int*);
typedef Uint32 (*PFN_SDL_GetRelativeMouseState)(int*, int*);

static HMODULE g_hRealSDL = NULL;
static HANDLE g_hMapFile = NULL;
static ScalAZoomState* g_pState = NULL;

static PFN_SDL_GetMouseState g_Real_GetMouseState = NULL;
static PFN_SDL_GetGlobalMouseState g_Real_GetGlobalMouseState = NULL;
static PFN_SDL_GetRelativeMouseState g_Real_GetRelativeMouseState = NULL;

/* Check if ScalA is still running */
static int IsScalAAlive(void) {
    if (!g_pState || g_pState->scalaPid == 0) return 0;
    HANDLE h = OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION, FALSE, g_pState->scalaPid);
    if (!h) return 0;
    DWORD code;
    int alive = GetExitCodeProcess(h, &code) && code == STILL_ACTIVE;
    CloseHandle(h);
    return alive;
}

/* Map mouse position if in zoom rect */
static void MapMousePos(int* x, int* y) {
    if (!g_pState || !g_pState->enabled || !IsScalAAlive()) return;
    if (g_pState->zoomRectWidth <= 0 || g_pState->zoomRectHeight <= 0) return;
    if (g_pState->clientWidth <= 0 || g_pState->clientHeight <= 0) return;

    POINT pt;
    GetCursorPos(&pt);

    /* Check if mouse is in zoom rect */
    int relX = pt.x - g_pState->zoomRectX;
    int relY = pt.y - g_pState->zoomRectY;

    if (relX >= 0 && relX < g_pState->zoomRectWidth &&
        relY >= 0 && relY < g_pState->zoomRectHeight) {
        /* Mouse is in zoom rect - map to client coords */
        *x = (relX * g_pState->clientWidth) / g_pState->zoomRectWidth;
        *y = (relY * g_pState->clientHeight) / g_pState->zoomRectHeight;
    }
    /* else: leave x,y unchanged (pass through real values) */
}

/* Connect to shared memory */
static void ConnectSharedMem(void) {
    if (g_hMapFile) return;

    char name[64];
    snprintf(name, sizeof(name), "ScalA_ZoomState_%lu", GetCurrentProcessId());

    g_hMapFile = OpenFileMappingA(FILE_MAP_READ, FALSE, name);
    if (g_hMapFile) {
        g_pState = (ScalAZoomState*)MapViewOfFile(g_hMapFile, FILE_MAP_READ, 0, 0, sizeof(ScalAZoomState));
    }
}

/* Load real SDL2 */
static void LoadRealSDL(void) {
    if (g_hRealSDL) return;

    g_hRealSDL = LoadLibraryA("SDL2_real.dll");
    if (!g_hRealSDL) g_hRealSDL = LoadLibraryA("SDL2_compat.dll");
    if (!g_hRealSDL) return;

    g_Real_GetMouseState = (PFN_SDL_GetMouseState)GetProcAddress(g_hRealSDL, "SDL_GetMouseState");
    g_Real_GetGlobalMouseState = (PFN_SDL_GetGlobalMouseState)GetProcAddress(g_hRealSDL, "SDL_GetGlobalMouseState");
    g_Real_GetRelativeMouseState = (PFN_SDL_GetRelativeMouseState)GetProcAddress(g_hRealSDL, "SDL_GetRelativeMouseState");
}

/* === Exported functions === */

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
    /* Relative = delta movement, no mapping needed */
    if (!g_Real_GetRelativeMouseState) return 0;
    return g_Real_GetRelativeMouseState(x, y);
}

/* DLL entry point */
BOOL WINAPI DllMain(HINSTANCE hInst, DWORD reason, LPVOID reserved) {
    (void)hInst; (void)reserved;

    if (reason == DLL_PROCESS_ATTACH) {
        DisableThreadLibraryCalls(hInst);
        LoadRealSDL();
        ConnectSharedMem();
    }
    else if (reason == DLL_PROCESS_DETACH) {
        if (g_pState) { UnmapViewOfFile(g_pState); g_pState = NULL; }
        if (g_hMapFile) { CloseHandle(g_hMapFile); g_hMapFile = NULL; }
        if (g_hRealSDL) { FreeLibrary(g_hRealSDL); g_hRealSDL = NULL; }
    }
    return TRUE;
}
