/*
 * SDL2 Proxy DLL for ScalA - Mouse Position Mapper (v3)
 *
 * Uses runtime GetProcAddress instead of linker forwarding.
 * Falls back gracefully if SDL2_real.dll is missing.
 */

#define WIN32_LEAN_AND_MEAN
#define _CRT_SECURE_NO_WARNINGS
#include <windows.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdarg.h>

typedef uint32_t Uint32;
typedef uint8_t Uint8;
typedef int16_t Sint16;
typedef int64_t Sint64;
typedef uint64_t Uint64;
typedef uint16_t Uint16;

/* Maximum ScalA instances per game - must match ZoomStateIPC.vb */
#define MAX_SCALA_INSTANCES 8

#pragma pack(push, 1)

/* Header (64 bytes) - reserved space for future expansion */
typedef struct {
    int version;        /* Structure version (currently 1) */
    int count;          /* Number of entries in use */
    int reserved[14];   /* Reserved for future use */
} ScalAZoomHeader;

/* Single entry for one ScalA instance (64 bytes) - reserved space for expansion */
typedef struct {
    int scalaPID;       /* PID of ScalA instance (0 = unused) */
    int viewportX;
    int viewportY;
    int viewportW;
    int viewportH;
    int clientW;
    int clientH;
    int enabled;
    int reserved[8];    /* Reserved for future use */
} ScalAZoomEntry;

/* Shared memory layout: header + array of entries */
typedef struct {
    ScalAZoomHeader header;
    ScalAZoomEntry entries[MAX_SCALA_INSTANCES];
} ScalAZoomState;

#pragma pack(pop)

static HMODULE g_hReal = NULL;
static HANDLE g_hMapFile = NULL;
static ScalAZoomState* g_pState = NULL;
static char g_szError[256] = {0};

/* ========== DLL Loading with Fallback ========== */

static BOOL LoadRealSDL(void) {
    /* Try in order: SDL2_real.dll, SDL2_backup.dll, SDL2_original.dll */
    const char* names[] = { "SDL2_real.dll", "SDL2_backup.dll", "SDL2_original.dll" };

    for (int i = 0; i < 3; i++) {
        g_hReal = LoadLibraryA(names[i]);
        if (g_hReal) return TRUE;
    }

    sprintf(g_szError,
        "SDL2 Wrapper Error: Could not find the original SDL2 library.\n\n"
        "Looked for:\n"
        "  - SDL2_real.dll\n"
        "  - SDL2_backup.dll\n"
        "  - SDL2_original.dll\n\n"
        "To fix: Rename your original SDL2.dll to SDL2_real.dll,\n"
        "or reinstall the game to restore SDL2.dll");
    return FALSE;
}

static void ConnectSharedMem(void) {
    if (g_hMapFile) return;
    char name[64];
    sprintf(name, "ScalA_ZoomState_%lu", GetCurrentProcessId());
    g_hMapFile = OpenFileMappingA(FILE_MAP_READ, FALSE, name);
    if (g_hMapFile)
        g_pState = (ScalAZoomState*)MapViewOfFile(g_hMapFile, FILE_MAP_READ, 0, 0, sizeof(ScalAZoomState));
}

/* ========== Mouse Position Mapping ========== */

/* Find the active entry whose viewport contains the mouse cursor */
static ScalAZoomEntry* FindActiveEntry(POINT* pt) {
    if (!g_pState) return NULL;

    int count = g_pState->header.count;
    if (count <= 0 || count > MAX_SCALA_INSTANCES) count = MAX_SCALA_INSTANCES;

    /* Walk array to find an enabled entry whose viewport contains the mouse */
    for (int i = 0; i < count; i++) {
        ScalAZoomEntry* e = &g_pState->entries[i];

        if (e->scalaPID == 0 || !e->enabled) continue;
        if (e->viewportW <= 0 || e->viewportH <= 0) continue;
        if (e->clientW <= 0 || e->clientH <= 0) continue;

        /* Check if mouse is inside this viewport */
        if (pt->x >= e->viewportX && pt->x < e->viewportX + e->viewportW &&
            pt->y >= e->viewportY && pt->y < e->viewportY + e->viewportH) {
            return e;
        }
    }

    return NULL;
}

static void MapMousePos(int* x, int* y) {
    if (!g_pState) ConnectSharedMem();
    if (!g_pState) return;

    POINT pt;
    GetCursorPos(&pt);

    ScalAZoomEntry* entry = FindActiveEntry(&pt);
    if (!entry) return;

    int relX = pt.x - entry->viewportX;
    int relY = pt.y - entry->viewportY;

    *x = (relX * entry->clientW) / entry->viewportW;
    *y = (relY * entry->clientH) / entry->viewportH;
}

/* ========== Function Pointer Cache ========== */

static void* GetSDLProc(const char* name) {
    if (!g_hReal) return NULL;
    return GetProcAddress(g_hReal, name);
}

#define GETPROC(name) static void* p = NULL; if (!p) p = GetSDLProc("SDL_" #name); if (!p) return

/* ========== Intercepted Mouse Functions ========== */

__declspec(dllexport) Uint32 SDL_GetMouseState(int* x, int* y) {
    typedef Uint32 (*FN)(int*, int*);
    static FN fn = NULL;
    if (!fn) fn = (FN)GetSDLProc("SDL_GetMouseState");
    if (!fn) return 0;

    int mx = 0, my = 0;
    Uint32 buttons = fn(&mx, &my);
    MapMousePos(&mx, &my);
    if (x) *x = mx;
    if (y) *y = my;
    return buttons;
}

__declspec(dllexport) Uint32 SDL_GetGlobalMouseState(int* x, int* y) {
    typedef Uint32 (*FN)(int*, int*);
    static FN fn = NULL;
    if (!fn) fn = (FN)GetSDLProc("SDL_GetGlobalMouseState");
    if (!fn) return 0;

    int mx = 0, my = 0;
    Uint32 buttons = fn(&mx, &my);
    MapMousePos(&mx, &my);
    if (x) *x = mx;
    if (y) *y = my;
    return buttons;
}

__declspec(dllexport) Uint32 SDL_GetRelativeMouseState(int* x, int* y) {
    typedef Uint32 (*FN)(int*, int*);
    static FN fn = NULL;
    if (!fn) fn = (FN)GetSDLProc("SDL_GetRelativeMouseState");
    if (!fn) return 0;
    return fn(x, y);
}

/* ========== Forwarding Macros ========== */

#define FWD_V0(name) \
    __declspec(dllexport) void SDL_##name(void) { \
        typedef void (*FN)(void); \
        static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_" #name); \
        if (fn) fn(); \
    }

#define FWD_I0(name) \
    __declspec(dllexport) int SDL_##name(void) { \
        typedef int (*FN)(void); \
        static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_" #name); \
        return fn ? fn() : 0; \
    }

#define FWD_U0(name) \
    __declspec(dllexport) Uint32 SDL_##name(void) { \
        typedef Uint32 (*FN)(void); \
        static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_" #name); \
        return fn ? fn() : 0; \
    }

#define FWD_U64_0(name) \
    __declspec(dllexport) Uint64 SDL_##name(void) { \
        typedef Uint64 (*FN)(void); \
        static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_" #name); \
        return fn ? fn() : 0; \
    }

#define FWD_P0(name) \
    __declspec(dllexport) void* SDL_##name(void) { \
        typedef void* (*FN)(void); \
        static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_" #name); \
        return fn ? fn() : NULL; \
    }

#define FWD_S0(name) \
    __declspec(dllexport) const char* SDL_##name(void) { \
        typedef const char* (*FN)(void); \
        static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_" #name); \
        return fn ? fn() : ""; \
    }

/* ========== Core Functions ========== */

__declspec(dllexport) int SDL_Init(Uint32 flags) {
    typedef int (*FN)(Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_Init");
    return fn ? fn(flags) : -1;
}

FWD_V0(Quit)

__declspec(dllexport) int SDL_InitSubSystem(Uint32 flags) {
    typedef int (*FN)(Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_InitSubSystem");
    return fn ? fn(flags) : -1;
}

__declspec(dllexport) void SDL_QuitSubSystem(Uint32 flags) {
    typedef void (*FN)(Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_QuitSubSystem");
    if (fn) fn(flags);
}

__declspec(dllexport) Uint32 SDL_WasInit(Uint32 flags) {
    typedef Uint32 (*FN)(Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_WasInit");
    return fn ? fn(flags) : 0;
}

__declspec(dllexport) const char* SDL_GetError(void) {
    if (g_szError[0]) return g_szError;
    typedef const char* (*FN)(void);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetError");
    return fn ? fn() : "SDL not loaded";
}

FWD_V0(ClearError)

/* ========== Window Functions ========== */

__declspec(dllexport) void* SDL_CreateWindow(const char* t, int x, int y, int w, int h, Uint32 f) {
    typedef void* (*FN)(const char*, int, int, int, int, Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_CreateWindow");
    return fn ? fn(t, x, y, w, h, f) : NULL;
}

__declspec(dllexport) void SDL_DestroyWindow(void* w) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_DestroyWindow");
    if (fn) fn(w);
}

__declspec(dllexport) void* SDL_GetWindowSurface(void* w) {
    typedef void* (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetWindowSurface");
    return fn ? fn(w) : NULL;
}

__declspec(dllexport) int SDL_UpdateWindowSurface(void* w) {
    typedef int (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_UpdateWindowSurface");
    return fn ? fn(w) : -1;
}

__declspec(dllexport) void SDL_GetWindowSize(void* w, int* pw, int* ph) {
    typedef void (*FN)(void*, int*, int*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetWindowSize");
    if (fn) fn(w, pw, ph);
}

__declspec(dllexport) void SDL_SetWindowSize(void* w, int width, int height) {
    typedef void (*FN)(void*, int, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetWindowSize");
    if (fn) fn(w, width, height);
}

__declspec(dllexport) void SDL_GetWindowPosition(void* w, int* x, int* y) {
    typedef void (*FN)(void*, int*, int*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetWindowPosition");
    if (fn) fn(w, x, y);
}

__declspec(dllexport) void SDL_SetWindowPosition(void* w, int x, int y) {
    typedef void (*FN)(void*, int, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetWindowPosition");
    if (fn) fn(w, x, y);
}

__declspec(dllexport) const char* SDL_GetWindowTitle(void* w) {
    typedef const char* (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetWindowTitle");
    return fn ? fn(w) : "";
}

__declspec(dllexport) void SDL_SetWindowTitle(void* w, const char* t) {
    typedef void (*FN)(void*, const char*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetWindowTitle");
    if (fn) fn(w, t);
}

#define WINDOW_VOID(name) \
    __declspec(dllexport) void SDL_##name(void* w) { \
        typedef void (*FN)(void*); \
        static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_" #name); \
        if (fn) fn(w); \
    }

WINDOW_VOID(ShowWindow)
WINDOW_VOID(HideWindow)
WINDOW_VOID(RaiseWindow)
WINDOW_VOID(MaximizeWindow)
WINDOW_VOID(MinimizeWindow)
WINDOW_VOID(RestoreWindow)

__declspec(dllexport) Uint32 SDL_GetWindowFlags(void* w) {
    typedef Uint32 (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetWindowFlags");
    return fn ? fn(w) : 0;
}

__declspec(dllexport) Uint32 SDL_GetWindowID(void* w) {
    typedef Uint32 (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetWindowID");
    return fn ? fn(w) : 0;
}

__declspec(dllexport) void* SDL_GetWindowFromID(Uint32 id) {
    typedef void* (*FN)(Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetWindowFromID");
    return fn ? fn(id) : NULL;
}

__declspec(dllexport) int SDL_SetWindowFullscreen(void* w, Uint32 f) {
    typedef int (*FN)(void*, Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetWindowFullscreen");
    return fn ? fn(w, f) : -1;
}

__declspec(dllexport) void SDL_SetWindowGrab(void* w, int g) {
    typedef void (*FN)(void*, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetWindowGrab");
    if (fn) fn(w, g);
}

__declspec(dllexport) int SDL_GetWindowGrab(void* w) {
    typedef int (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetWindowGrab");
    return fn ? fn(w) : 0;
}

__declspec(dllexport) void SDL_SetWindowIcon(void* w, void* i) {
    typedef void (*FN)(void*, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetWindowIcon");
    if (fn) fn(w, i);
}

__declspec(dllexport) void SDL_SetWindowBordered(void* w, int b) {
    typedef void (*FN)(void*, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetWindowBordered");
    if (fn) fn(w, b);
}

/* ========== Event Functions ========== */

__declspec(dllexport) int SDL_PollEvent(void* e) {
    typedef int (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_PollEvent");
    return fn ? fn(e) : 0;
}

__declspec(dllexport) int SDL_WaitEvent(void* e) {
    typedef int (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_WaitEvent");
    return fn ? fn(e) : 0;
}

FWD_V0(PumpEvents)

__declspec(dllexport) int SDL_PushEvent(void* e) {
    typedef int (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_PushEvent");
    return fn ? fn(e) : -1;
}

__declspec(dllexport) int SDL_PeepEvents(void* e, int n, int a, Uint32 min, Uint32 max) {
    typedef int (*FN)(void*, int, int, Uint32, Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_PeepEvents");
    return fn ? fn(e, n, a, min, max) : -1;
}

__declspec(dllexport) Uint8 SDL_EventState(Uint32 t, int s) {
    typedef Uint8 (*FN)(Uint32, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_EventState");
    return fn ? fn(t, s) : 0;
}

__declspec(dllexport) void SDL_FlushEvent(Uint32 t) {
    typedef void (*FN)(Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_FlushEvent");
    if (fn) fn(t);
}

__declspec(dllexport) void SDL_FlushEvents(Uint32 min, Uint32 max) {
    typedef void (*FN)(Uint32, Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_FlushEvents");
    if (fn) fn(min, max);
}

__declspec(dllexport) Uint32 SDL_RegisterEvents(int n) {
    typedef Uint32 (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_RegisterEvents");
    return fn ? fn(n) : (Uint32)-1;
}

/* ========== Renderer Functions ========== */

__declspec(dllexport) void* SDL_CreateRenderer(void* w, int i, Uint32 f) {
    typedef void* (*FN)(void*, int, Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_CreateRenderer");
    return fn ? fn(w, i, f) : NULL;
}

__declspec(dllexport) void SDL_DestroyRenderer(void* r) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_DestroyRenderer");
    if (fn) fn(r);
}

__declspec(dllexport) int SDL_RenderClear(void* r) {
    typedef int (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_RenderClear");
    return fn ? fn(r) : -1;
}

__declspec(dllexport) int SDL_RenderCopy(void* r, void* t, const void* s, const void* d) {
    typedef int (*FN)(void*, void*, const void*, const void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_RenderCopy");
    return fn ? fn(r, t, s, d) : -1;
}

__declspec(dllexport) void SDL_RenderPresent(void* r) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_RenderPresent");
    if (fn) fn(r);
}

__declspec(dllexport) int SDL_SetRenderDrawColor(void* r, Uint8 red, Uint8 g, Uint8 b, Uint8 a) {
    typedef int (*FN)(void*, Uint8, Uint8, Uint8, Uint8);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetRenderDrawColor");
    return fn ? fn(r, red, g, b, a) : -1;
}

__declspec(dllexport) int SDL_GetRenderDrawColor(void* r, Uint8* red, Uint8* g, Uint8* b, Uint8* a) {
    typedef int (*FN)(void*, Uint8*, Uint8*, Uint8*, Uint8*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetRenderDrawColor");
    return fn ? fn(r, red, g, b, a) : -1;
}

__declspec(dllexport) int SDL_RenderFillRect(void* r, const void* rect) {
    typedef int (*FN)(void*, const void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_RenderFillRect");
    return fn ? fn(r, rect) : -1;
}

__declspec(dllexport) int SDL_RenderDrawRect(void* r, const void* rect) {
    typedef int (*FN)(void*, const void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_RenderDrawRect");
    return fn ? fn(r, rect) : -1;
}

__declspec(dllexport) int SDL_RenderDrawLine(void* r, int x1, int y1, int x2, int y2) {
    typedef int (*FN)(void*, int, int, int, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_RenderDrawLine");
    return fn ? fn(r, x1, y1, x2, y2) : -1;
}

__declspec(dllexport) int SDL_SetRenderTarget(void* r, void* t) {
    typedef int (*FN)(void*, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetRenderTarget");
    return fn ? fn(r, t) : -1;
}

__declspec(dllexport) void* SDL_GetRenderer(void* w) {
    typedef void* (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetRenderer");
    return fn ? fn(w) : NULL;
}

__declspec(dllexport) int SDL_GetRendererInfo(void* r, void* i) {
    typedef int (*FN)(void*, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetRendererInfo");
    return fn ? fn(r, i) : -1;
}

/* ========== Texture Functions ========== */

__declspec(dllexport) void* SDL_CreateTexture(void* r, Uint32 f, int a, int w, int h) {
    typedef void* (*FN)(void*, Uint32, int, int, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_CreateTexture");
    return fn ? fn(r, f, a, w, h) : NULL;
}

__declspec(dllexport) void* SDL_CreateTextureFromSurface(void* r, void* s) {
    typedef void* (*FN)(void*, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_CreateTextureFromSurface");
    return fn ? fn(r, s) : NULL;
}

__declspec(dllexport) void SDL_DestroyTexture(void* t) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_DestroyTexture");
    if (fn) fn(t);
}

__declspec(dllexport) int SDL_UpdateTexture(void* t, const void* r, const void* p, int pitch) {
    typedef int (*FN)(void*, const void*, const void*, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_UpdateTexture");
    return fn ? fn(t, r, p, pitch) : -1;
}

__declspec(dllexport) int SDL_LockTexture(void* t, const void* r, void** p, int* pitch) {
    typedef int (*FN)(void*, const void*, void**, int*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_LockTexture");
    return fn ? fn(t, r, p, pitch) : -1;
}

__declspec(dllexport) void SDL_UnlockTexture(void* t) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_UnlockTexture");
    if (fn) fn(t);
}

__declspec(dllexport) int SDL_QueryTexture(void* t, Uint32* f, int* a, int* w, int* h) {
    typedef int (*FN)(void*, Uint32*, int*, int*, int*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_QueryTexture");
    return fn ? fn(t, f, a, w, h) : -1;
}

/* ========== Timer Functions ========== */

FWD_U0(GetTicks)

__declspec(dllexport) void SDL_Delay(Uint32 ms) {
    typedef void (*FN)(Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_Delay");
    if (fn) fn(ms); else Sleep(ms);
}

FWD_U64_0(GetPerformanceCounter)
FWD_U64_0(GetPerformanceFrequency)

__declspec(dllexport) int SDL_AddTimer(Uint32 i, void* cb, void* p) {
    typedef int (*FN)(Uint32, void*, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_AddTimer");
    return fn ? fn(i, cb, p) : 0;
}

__declspec(dllexport) int SDL_RemoveTimer(int id) {
    typedef int (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_RemoveTimer");
    return fn ? fn(id) : 0;
}

/* ========== Keyboard Functions ========== */

__declspec(dllexport) const Uint8* SDL_GetKeyboardState(int* n) {
    typedef const Uint8* (*FN)(int*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetKeyboardState");
    return fn ? fn(n) : NULL;
}

FWD_I0(GetModState)

__declspec(dllexport) void SDL_SetModState(int m) {
    typedef void (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetModState");
    if (fn) fn(m);
}

__declspec(dllexport) int SDL_GetKeyFromScancode(int s) {
    typedef int (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetKeyFromScancode");
    return fn ? fn(s) : 0;
}

__declspec(dllexport) int SDL_GetScancodeFromKey(int k) {
    typedef int (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetScancodeFromKey");
    return fn ? fn(k) : 0;
}

__declspec(dllexport) const char* SDL_GetKeyName(int k) {
    typedef const char* (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetKeyName");
    return fn ? fn(k) : "";
}

FWD_P0(GetKeyboardFocus)

/* ========== Cursor Functions ========== */

__declspec(dllexport) int SDL_ShowCursor(int t) {
    typedef int (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_ShowCursor");
    return fn ? fn(t) : 0;
}

__declspec(dllexport) void SDL_SetCursor(void* c) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetCursor");
    if (fn) fn(c);
}

FWD_P0(GetCursor)

__declspec(dllexport) void* SDL_CreateSystemCursor(int id) {
    typedef void* (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_CreateSystemCursor");
    return fn ? fn(id) : NULL;
}

__declspec(dllexport) void SDL_FreeCursor(void* c) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_FreeCursor");
    if (fn) fn(c);
}

__declspec(dllexport) void SDL_WarpMouseInWindow(void* w, int x, int y) {
    typedef void (*FN)(void*, int, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_WarpMouseInWindow");
    if (fn) fn(w, x, y);
}

__declspec(dllexport) int SDL_SetRelativeMouseMode(int e) {
    typedef int (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetRelativeMouseMode");
    return fn ? fn(e) : -1;
}

FWD_I0(GetRelativeMouseMode)
FWD_P0(GetMouseFocus)

__declspec(dllexport) int SDL_CaptureMouse(int e) {
    typedef int (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_CaptureMouse");
    return fn ? fn(e) : -1;
}

/* ========== Surface Functions ========== */

__declspec(dllexport) void* SDL_CreateRGBSurface(Uint32 f, int w, int h, int d, Uint32 rm, Uint32 gm, Uint32 bm, Uint32 am) {
    typedef void* (*FN)(Uint32, int, int, int, Uint32, Uint32, Uint32, Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_CreateRGBSurface");
    return fn ? fn(f, w, h, d, rm, gm, bm, am) : NULL;
}

__declspec(dllexport) void* SDL_CreateRGBSurfaceFrom(void* p, int w, int h, int d, int pitch, Uint32 rm, Uint32 gm, Uint32 bm, Uint32 am) {
    typedef void* (*FN)(void*, int, int, int, int, Uint32, Uint32, Uint32, Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_CreateRGBSurfaceFrom");
    return fn ? fn(p, w, h, d, pitch, rm, gm, bm, am) : NULL;
}

__declspec(dllexport) void* SDL_CreateRGBSurfaceWithFormat(Uint32 f, int w, int h, int d, Uint32 fmt) {
    typedef void* (*FN)(Uint32, int, int, int, Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_CreateRGBSurfaceWithFormat");
    return fn ? fn(f, w, h, d, fmt) : NULL;
}

__declspec(dllexport) void SDL_FreeSurface(void* s) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_FreeSurface");
    if (fn) fn(s);
}

__declspec(dllexport) int SDL_LockSurface(void* s) {
    typedef int (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_LockSurface");
    return fn ? fn(s) : -1;
}

__declspec(dllexport) void SDL_UnlockSurface(void* s) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_UnlockSurface");
    if (fn) fn(s);
}

/* SDL_BlitSurface is a macro for SDL_UpperBlit */
__declspec(dllexport) int SDL_UpperBlit(void* src, const void* sr, void* dst, void* dr) {
    typedef int (*FN)(void*, const void*, void*, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_UpperBlit");
    return fn ? fn(src, sr, dst, dr) : -1;
}

__declspec(dllexport) int SDL_BlitSurface(void* src, const void* sr, void* dst, void* dr) {
    typedef int (*FN)(void*, const void*, void*, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_UpperBlit");
    return fn ? fn(src, sr, dst, dr) : -1;
}

__declspec(dllexport) int SDL_UpperBlitScaled(void* src, const void* sr, void* dst, void* dr) {
    typedef int (*FN)(void*, const void*, void*, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_UpperBlitScaled");
    return fn ? fn(src, sr, dst, dr) : -1;
}

__declspec(dllexport) int SDL_BlitScaled(void* src, const void* sr, void* dst, void* dr) {
    typedef int (*FN)(void*, const void*, void*, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_UpperBlitScaled");
    return fn ? fn(src, sr, dst, dr) : -1;
}

__declspec(dllexport) int SDL_FillRect(void* s, const void* r, Uint32 c) {
    typedef int (*FN)(void*, const void*, Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_FillRect");
    return fn ? fn(s, r, c) : -1;
}

__declspec(dllexport) int SDL_SetColorKey(void* s, int f, Uint32 k) {
    typedef int (*FN)(void*, int, Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetColorKey");
    return fn ? fn(s, f, k) : -1;
}

__declspec(dllexport) int SDL_SetClipRect(void* s, const void* r) {
    typedef int (*FN)(void*, const void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetClipRect");
    return fn ? fn(s, r) : 0;
}

__declspec(dllexport) Uint32 SDL_MapRGB(const void* f, Uint8 r, Uint8 g, Uint8 b) {
    typedef Uint32 (*FN)(const void*, Uint8, Uint8, Uint8);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_MapRGB");
    return fn ? fn(f, r, g, b) : 0;
}

__declspec(dllexport) Uint32 SDL_MapRGBA(const void* f, Uint8 r, Uint8 g, Uint8 b, Uint8 a) {
    typedef Uint32 (*FN)(const void*, Uint8, Uint8, Uint8, Uint8);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_MapRGBA");
    return fn ? fn(f, r, g, b, a) : 0;
}

__declspec(dllexport) void SDL_GetRGB(Uint32 p, const void* f, Uint8* r, Uint8* g, Uint8* b) {
    typedef void (*FN)(Uint32, const void*, Uint8*, Uint8*, Uint8*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetRGB");
    if (fn) fn(p, f, r, g, b);
}

__declspec(dllexport) void SDL_GetRGBA(Uint32 p, const void* f, Uint8* r, Uint8* g, Uint8* b, Uint8* a) {
    typedef void (*FN)(Uint32, const void*, Uint8*, Uint8*, Uint8*, Uint8*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetRGBA");
    if (fn) fn(p, f, r, g, b, a);
}

__declspec(dllexport) void* SDL_ConvertSurface(void* s, const void* f, Uint32 flags) {
    typedef void* (*FN)(void*, const void*, Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_ConvertSurface");
    return fn ? fn(s, f, flags) : NULL;
}

__declspec(dllexport) void* SDL_ConvertSurfaceFormat(void* s, Uint32 f, Uint32 flags) {
    typedef void* (*FN)(void*, Uint32, Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_ConvertSurfaceFormat");
    return fn ? fn(s, f, flags) : NULL;
}

/* ========== Pixel Format Functions ========== */

__declspec(dllexport) void* SDL_AllocFormat(Uint32 f) {
    typedef void* (*FN)(Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_AllocFormat");
    return fn ? fn(f) : NULL;
}

__declspec(dllexport) void SDL_FreeFormat(void* f) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_FreeFormat");
    if (fn) fn(f);
}

__declspec(dllexport) int SDL_SetPaletteColors(void* p, const void* c, int f, int n) {
    typedef int (*FN)(void*, const void*, int, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetPaletteColors");
    return fn ? fn(p, c, f, n) : -1;
}

__declspec(dllexport) void* SDL_AllocPalette(int n) {
    typedef void* (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_AllocPalette");
    return fn ? fn(n) : NULL;
}

__declspec(dllexport) void SDL_FreePalette(void* p) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_FreePalette");
    if (fn) fn(p);
}

/* ========== RWops Functions ========== */

__declspec(dllexport) void* SDL_LoadBMP_RW(void* s, int f) {
    typedef void* (*FN)(void*, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_LoadBMP_RW");
    return fn ? fn(s, f) : NULL;
}

__declspec(dllexport) int SDL_SaveBMP_RW(void* s, void* d, int f) {
    typedef int (*FN)(void*, void*, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SaveBMP_RW");
    return fn ? fn(s, d, f) : -1;
}

__declspec(dllexport) void* SDL_RWFromFile(const char* f, const char* m) {
    typedef void* (*FN)(const char*, const char*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_RWFromFile");
    return fn ? fn(f, m) : NULL;
}

__declspec(dllexport) void* SDL_RWFromMem(void* m, int s) {
    typedef void* (*FN)(void*, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_RWFromMem");
    return fn ? fn(m, s) : NULL;
}

__declspec(dllexport) int SDL_RWclose(void* c) {
    typedef int (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_RWclose");
    return fn ? fn(c) : -1;
}

__declspec(dllexport) size_t SDL_RWread(void* c, void* p, size_t s, size_t n) {
    typedef size_t (*FN)(void*, void*, size_t, size_t);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_RWread");
    return fn ? fn(c, p, s, n) : 0;
}

__declspec(dllexport) size_t SDL_RWwrite(void* c, const void* p, size_t s, size_t n) {
    typedef size_t (*FN)(void*, const void*, size_t, size_t);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_RWwrite");
    return fn ? fn(c, p, s, n) : 0;
}

__declspec(dllexport) Sint64 SDL_RWseek(void* c, Sint64 o, int w) {
    typedef Sint64 (*FN)(void*, Sint64, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_RWseek");
    return fn ? fn(c, o, w) : -1;
}

__declspec(dllexport) Sint64 SDL_RWtell(void* c) {
    typedef Sint64 (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_RWtell");
    return fn ? fn(c) : -1;
}

/* ========== Display Functions ========== */

__declspec(dllexport) int SDL_GetDisplayMode(int d, int m, void* mode) {
    typedef int (*FN)(int, int, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetDisplayMode");
    return fn ? fn(d, m, mode) : -1;
}

__declspec(dllexport) int SDL_GetCurrentDisplayMode(int d, void* mode) {
    typedef int (*FN)(int, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetCurrentDisplayMode");
    return fn ? fn(d, mode) : -1;
}

__declspec(dllexport) int SDL_GetDesktopDisplayMode(int d, void* mode) {
    typedef int (*FN)(int, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetDesktopDisplayMode");
    return fn ? fn(d, mode) : -1;
}

FWD_I0(GetNumVideoDisplays)

__declspec(dllexport) int SDL_GetNumDisplayModes(int d) {
    typedef int (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetNumDisplayModes");
    return fn ? fn(d) : 0;
}

__declspec(dllexport) const char* SDL_GetDisplayName(int d) {
    typedef const char* (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetDisplayName");
    return fn ? fn(d) : "";
}

__declspec(dllexport) int SDL_GetDisplayBounds(int d, void* r) {
    typedef int (*FN)(int, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetDisplayBounds");
    return fn ? fn(d, r) : -1;
}

/* ========== Version/Platform Functions ========== */

__declspec(dllexport) void SDL_GetVersion(void* v) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetVersion");
    if (fn) fn(v);
}

FWD_S0(GetRevision)
FWD_S0(GetPlatform)

__declspec(dllexport) char* SDL_GetBasePath(void) {
    typedef char* (*FN)(void);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetBasePath");
    return fn ? fn() : NULL;
}

__declspec(dllexport) char* SDL_GetPrefPath(const char* o, const char* a) {
    typedef char* (*FN)(const char*, const char*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetPrefPath");
    return fn ? fn(o, a) : NULL;
}

/* ========== Audio Functions ========== */

__declspec(dllexport) int SDL_OpenAudio(void* d, void* o) {
    typedef int (*FN)(void*, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_OpenAudio");
    return fn ? fn(d, o) : -1;
}

FWD_V0(CloseAudio)

__declspec(dllexport) void SDL_PauseAudio(int p) {
    typedef void (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_PauseAudio");
    if (fn) fn(p);
}

__declspec(dllexport) void SDL_MixAudio(Uint8* d, const Uint8* s, Uint32 l, int v) {
    typedef void (*FN)(Uint8*, const Uint8*, Uint32, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_MixAudio");
    if (fn) fn(d, s, l, v);
}

FWD_V0(LockAudio)
FWD_V0(UnlockAudio)

__declspec(dllexport) void* SDL_LoadWAV_RW(void* s, int f, void* sp, Uint8** ab, Uint32* al) {
    typedef void* (*FN)(void*, int, void*, Uint8**, Uint32*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_LoadWAV_RW");
    return fn ? fn(s, f, sp, ab, al) : NULL;
}

__declspec(dllexport) void SDL_FreeWAV(Uint8* b) {
    typedef void (*FN)(Uint8*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_FreeWAV");
    if (fn) fn(b);
}

__declspec(dllexport) Uint32 SDL_OpenAudioDevice(const char* d, int c, const void* des, void* obt, int ch) {
    typedef Uint32 (*FN)(const char*, int, const void*, void*, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_OpenAudioDevice");
    return fn ? fn(d, c, des, obt, ch) : 0;
}

__declspec(dllexport) void SDL_CloseAudioDevice(Uint32 d) {
    typedef void (*FN)(Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_CloseAudioDevice");
    if (fn) fn(d);
}

__declspec(dllexport) void SDL_PauseAudioDevice(Uint32 d, int p) {
    typedef void (*FN)(Uint32, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_PauseAudioDevice");
    if (fn) fn(d, p);
}

__declspec(dllexport) int SDL_QueueAudio(Uint32 d, const void* data, Uint32 l) {
    typedef int (*FN)(Uint32, const void*, Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_QueueAudio");
    return fn ? fn(d, data, l) : -1;
}

__declspec(dllexport) Uint32 SDL_GetQueuedAudioSize(Uint32 d) {
    typedef Uint32 (*FN)(Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetQueuedAudioSize");
    return fn ? fn(d) : 0;
}

__declspec(dllexport) void SDL_ClearQueuedAudio(Uint32 d) {
    typedef void (*FN)(Uint32);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_ClearQueuedAudio");
    if (fn) fn(d);
}

__declspec(dllexport) int SDL_GetNumAudioDevices(int c) {
    typedef int (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetNumAudioDevices");
    return fn ? fn(c) : 0;
}

__declspec(dllexport) const char* SDL_GetAudioDeviceName(int i, int c) {
    typedef const char* (*FN)(int, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetAudioDeviceName");
    return fn ? fn(i, c) : NULL;
}

__declspec(dllexport) void SDL_MixAudioFormat(Uint8* d, const Uint8* s, Uint16 f, Uint32 l, int v) {
    typedef void (*FN)(Uint8*, const Uint8*, Uint16, Uint32, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_MixAudioFormat");
    if (fn) fn(d, s, f, l, v);
}

/* ========== Log Functions (variadic) ========== */

__declspec(dllexport) void SDL_Log(const char* fmt, ...) {
    typedef void (*FN)(int, int, const char*, va_list);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_LogMessageV");
    if (!fn) return;
    va_list ap; va_start(ap, fmt);
    fn(0, 3, fmt, ap);  /* SDL_LOG_CATEGORY_APPLICATION, SDL_LOG_PRIORITY_INFO */
    va_end(ap);
}

__declspec(dllexport) void SDL_LogError(int cat, const char* fmt, ...) {
    typedef void (*FN)(int, int, const char*, va_list);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_LogMessageV");
    if (!fn) return;
    va_list ap; va_start(ap, fmt);
    fn(cat, 5, fmt, ap);
    va_end(ap);
}

__declspec(dllexport) void SDL_LogWarn(int cat, const char* fmt, ...) {
    typedef void (*FN)(int, int, const char*, va_list);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_LogMessageV");
    if (!fn) return;
    va_list ap; va_start(ap, fmt);
    fn(cat, 4, fmt, ap);
    va_end(ap);
}

__declspec(dllexport) void SDL_LogInfo(int cat, const char* fmt, ...) {
    typedef void (*FN)(int, int, const char*, va_list);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_LogMessageV");
    if (!fn) return;
    va_list ap; va_start(ap, fmt);
    fn(cat, 3, fmt, ap);
    va_end(ap);
}

__declspec(dllexport) void SDL_LogDebug(int cat, const char* fmt, ...) {
    typedef void (*FN)(int, int, const char*, va_list);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_LogMessageV");
    if (!fn) return;
    va_list ap; va_start(ap, fmt);
    fn(cat, 2, fmt, ap);
    va_end(ap);
}

__declspec(dllexport) void SDL_LogVerbose(int cat, const char* fmt, ...) {
    typedef void (*FN)(int, int, const char*, va_list);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_LogMessageV");
    if (!fn) return;
    va_list ap; va_start(ap, fmt);
    fn(cat, 1, fmt, ap);
    va_end(ap);
}

__declspec(dllexport) void SDL_LogMessage(int cat, int pri, const char* fmt, ...) {
    typedef void (*FN)(int, int, const char*, va_list);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_LogMessageV");
    if (!fn) return;
    va_list ap; va_start(ap, fmt);
    fn(cat, pri, fmt, ap);
    va_end(ap);
}

__declspec(dllexport) void SDL_LogSetOutputFunction(void* cb, void* ud) {
    typedef void (*FN)(void*, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_LogSetOutputFunction");
    if (fn) fn(cb, ud);
}

__declspec(dllexport) void SDL_LogSetAllPriority(int p) {
    typedef void (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_LogSetAllPriority");
    if (fn) fn(p);
}

__declspec(dllexport) void SDL_LogSetPriority(int cat, int p) {
    typedef void (*FN)(int, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_LogSetPriority");
    if (fn) fn(cat, p);
}

/* ========== Memory Functions ========== */

__declspec(dllexport) void* SDL_malloc(size_t s) {
    typedef void* (*FN)(size_t);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_malloc");
    return fn ? fn(s) : malloc(s);
}

__declspec(dllexport) void SDL_free(void* m) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_free");
    if (fn) fn(m); else free(m);
}

__declspec(dllexport) void* SDL_realloc(void* m, size_t s) {
    typedef void* (*FN)(void*, size_t);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_realloc");
    return fn ? fn(m, s) : realloc(m, s);
}

__declspec(dllexport) void* SDL_memset(void* d, int c, size_t l) {
    typedef void* (*FN)(void*, int, size_t);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_memset");
    return fn ? fn(d, c, l) : memset(d, c, l);
}

__declspec(dllexport) void* SDL_memcpy(void* d, const void* s, size_t l) {
    typedef void* (*FN)(void*, const void*, size_t);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_memcpy");
    return fn ? fn(d, s, l) : memcpy(d, s, l);
}

__declspec(dllexport) void* SDL_memmove(void* d, const void* s, size_t l) {
    typedef void* (*FN)(void*, const void*, size_t);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_memmove");
    return fn ? fn(d, s, l) : memmove(d, s, l);
}

__declspec(dllexport) size_t SDL_strlen(const char* s) {
    typedef size_t (*FN)(const char*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_strlen");
    return fn ? fn(s) : strlen(s);
}

__declspec(dllexport) int SDL_strcmp(const char* a, const char* b) {
    typedef int (*FN)(const char*, const char*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_strcmp");
    return fn ? fn(a, b) : strcmp(a, b);
}

__declspec(dllexport) int SDL_strncmp(const char* a, const char* b, size_t n) {
    typedef int (*FN)(const char*, const char*, size_t);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_strncmp");
    return fn ? fn(a, b, n) : strncmp(a, b, n);
}

__declspec(dllexport) int SDL_strcasecmp(const char* a, const char* b) {
    typedef int (*FN)(const char*, const char*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_strcasecmp");
    return fn ? fn(a, b) : _stricmp(a, b);
}

__declspec(dllexport) int SDL_strncasecmp(const char* a, const char* b, size_t n) {
    typedef int (*FN)(const char*, const char*, size_t);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_strncasecmp");
    return fn ? fn(a, b, n) : _strnicmp(a, b, n);
}

__declspec(dllexport) char* SDL_strdup(const char* s) {
    typedef char* (*FN)(const char*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_strdup");
    return fn ? fn(s) : _strdup(s);
}

__declspec(dllexport) int SDL_snprintf(char* t, size_t m, const char* f, ...) {
    va_list ap; va_start(ap, f);
    int r = vsnprintf(t, m, f, ap);
    va_end(ap);
    return r;
}

/* ========== Hint Functions ========== */

__declspec(dllexport) int SDL_SetHint(const char* n, const char* v) {
    typedef int (*FN)(const char*, const char*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetHint");
    return fn ? fn(n, v) : 0;
}

__declspec(dllexport) const char* SDL_GetHint(const char* n) {
    typedef const char* (*FN)(const char*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetHint");
    return fn ? fn(n) : NULL;
}

/* ========== Clipboard Functions ========== */

__declspec(dllexport) int SDL_SetClipboardText(const char* t) {
    typedef int (*FN)(const char*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetClipboardText");
    return fn ? fn(t) : -1;
}

__declspec(dllexport) char* SDL_GetClipboardText(void) {
    typedef char* (*FN)(void);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GetClipboardText");
    return fn ? fn() : NULL;
}

FWD_I0(HasClipboardText)

/* ========== GL Functions ========== */

__declspec(dllexport) int SDL_GL_SetAttribute(int a, int v) {
    typedef int (*FN)(int, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GL_SetAttribute");
    return fn ? fn(a, v) : -1;
}

__declspec(dllexport) int SDL_GL_GetAttribute(int a, int* v) {
    typedef int (*FN)(int, int*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GL_GetAttribute");
    return fn ? fn(a, v) : -1;
}

__declspec(dllexport) void* SDL_GL_CreateContext(void* w) {
    typedef void* (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GL_CreateContext");
    return fn ? fn(w) : NULL;
}

__declspec(dllexport) void SDL_GL_DeleteContext(void* c) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GL_DeleteContext");
    if (fn) fn(c);
}

__declspec(dllexport) int SDL_GL_MakeCurrent(void* w, void* c) {
    typedef int (*FN)(void*, void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GL_MakeCurrent");
    return fn ? fn(w, c) : -1;
}

__declspec(dllexport) void SDL_GL_SwapWindow(void* w) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GL_SwapWindow");
    if (fn) fn(w);
}

__declspec(dllexport) int SDL_GL_SetSwapInterval(int i) {
    typedef int (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GL_SetSwapInterval");
    return fn ? fn(i) : -1;
}

FWD_I0(GL_GetSwapInterval)

__declspec(dllexport) void* SDL_GL_GetProcAddress(const char* p) {
    typedef void* (*FN)(const char*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GL_GetProcAddress");
    return fn ? fn(p) : NULL;
}

__declspec(dllexport) int SDL_GL_LoadLibrary(const char* p) {
    typedef int (*FN)(const char*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GL_LoadLibrary");
    return fn ? fn(p) : -1;
}

FWD_V0(GL_UnloadLibrary)

__declspec(dllexport) int SDL_GL_ExtensionSupported(const char* e) {
    typedef int (*FN)(const char*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GL_ExtensionSupported");
    return fn ? fn(e) : 0;
}

/* ========== Joystick Functions ========== */

FWD_I0(NumJoysticks)

__declspec(dllexport) void* SDL_JoystickOpen(int i) {
    typedef void* (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_JoystickOpen");
    return fn ? fn(i) : NULL;
}

__declspec(dllexport) void SDL_JoystickClose(void* j) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_JoystickClose");
    if (fn) fn(j);
}

__declspec(dllexport) const char* SDL_JoystickName(void* j) {
    typedef const char* (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_JoystickName");
    return fn ? fn(j) : "";
}

__declspec(dllexport) Sint16 SDL_JoystickGetAxis(void* j, int a) {
    typedef Sint16 (*FN)(void*, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_JoystickGetAxis");
    return fn ? fn(j, a) : 0;
}

__declspec(dllexport) Uint8 SDL_JoystickGetButton(void* j, int b) {
    typedef Uint8 (*FN)(void*, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_JoystickGetButton");
    return fn ? fn(j, b) : 0;
}

__declspec(dllexport) int SDL_JoystickNumAxes(void* j) {
    typedef int (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_JoystickNumAxes");
    return fn ? fn(j) : 0;
}

__declspec(dllexport) int SDL_JoystickNumButtons(void* j) {
    typedef int (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_JoystickNumButtons");
    return fn ? fn(j) : 0;
}

FWD_V0(JoystickUpdate)

__declspec(dllexport) int SDL_JoystickEventState(int s) {
    typedef int (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_JoystickEventState");
    return fn ? fn(s) : 0;
}

/* ========== Game Controller Functions ========== */

__declspec(dllexport) int SDL_IsGameController(int i) {
    typedef int (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_IsGameController");
    return fn ? fn(i) : 0;
}

__declspec(dllexport) void* SDL_GameControllerOpen(int i) {
    typedef void* (*FN)(int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GameControllerOpen");
    return fn ? fn(i) : NULL;
}

__declspec(dllexport) void SDL_GameControllerClose(void* c) {
    typedef void (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GameControllerClose");
    if (fn) fn(c);
}

__declspec(dllexport) const char* SDL_GameControllerName(void* c) {
    typedef const char* (*FN)(void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GameControllerName");
    return fn ? fn(c) : "";
}

__declspec(dllexport) Sint16 SDL_GameControllerGetAxis(void* c, int a) {
    typedef Sint16 (*FN)(void*, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GameControllerGetAxis");
    return fn ? fn(c, a) : 0;
}

__declspec(dllexport) Uint8 SDL_GameControllerGetButton(void* c, int b) {
    typedef Uint8 (*FN)(void*, int);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_GameControllerGetButton");
    return fn ? fn(c, b) : 0;
}

FWD_V0(GameControllerUpdate)

/* ========== Text Input Functions ========== */

FWD_V0(StartTextInput)
FWD_V0(StopTextInput)

__declspec(dllexport) void SDL_SetTextInputRect(const void* r) {
    typedef void (*FN)(const void*);
    static FN fn = NULL; if (!fn) fn = (FN)GetSDLProc("SDL_SetTextInputRect");
    if (fn) fn(r);
}

/* ========== DLL Entry Point ========== */

BOOL WINAPI DllMain(HINSTANCE hInst, DWORD reason, LPVOID reserved) {
    (void)hInst; (void)reserved;

    if (reason == DLL_PROCESS_ATTACH) {
        DisableThreadLibraryCalls(hInst);

        if (!LoadRealSDL()) {
            /* Show error but still allow DLL to load - SDL_GetError will explain */
            MessageBoxA(NULL, g_szError, "SDL2 Wrapper", MB_OK | MB_ICONWARNING);
        }

        ConnectSharedMem();

    } else if (reason == DLL_PROCESS_DETACH) {
        if (g_pState) { UnmapViewOfFile(g_pState); g_pState = NULL; }
        if (g_hMapFile) { CloseHandle(g_hMapFile); g_hMapFile = NULL; }
        if (g_hReal) { FreeLibrary(g_hReal); g_hReal = NULL; }
    }

    return TRUE;
}
