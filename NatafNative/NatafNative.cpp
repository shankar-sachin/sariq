// NatafNative.cpp - Implementation of the Nataf native sound & effects library
// Compile: cl /LD NatafNative.cpp /Fe:NatafNative.dll (Windows MSVC)
//          g++ -shared -fPIC -o libNatafNative.so NatafNative.cpp (Linux)
//          clang++ -shared -fPIC -o libNatafNative.dylib NatafNative.cpp (macOS)

#include "NatafNative.h"
#include <cstdlib>
#include <cstring>
#include <cmath>
#include <ctime>

#ifdef _WIN32
    #include <windows.h>
#endif

// ============================================================
//  Internal Helpers
// ============================================================

namespace {
    // Simple platform beep wrapper
    void platform_beep(uint32_t freq, uint32_t ms) {
#ifdef _WIN32
        Beep(freq, ms);
#else
        // On non-Windows, we can write BEL character as a fallback
        // In a real implementation, this would use ALSA/PulseAudio/CoreAudio
        (void)freq;
        (void)ms;
#endif
    }

    // Permutation table for noise
    static const int perm[512] = {
        151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,
        140,36,103,30,69,142,8,99,37,240,21,10,23,190,6,148,
        247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,
        57,177,33,88,237,149,56,87,174,20,125,136,171,168,68,175,
        74,165,71,134,139,48,27,166,77,146,158,231,83,111,229,122,
        60,211,133,230,220,105,92,41,55,46,245,40,244,102,143,54,
        65,25,63,161,1,216,80,73,209,76,132,187,208,89,18,169,
        200,196,135,130,116,188,159,86,164,100,109,198,173,186,3,64,
        52,217,226,250,124,123,5,202,38,147,118,126,255,82,85,212,
        207,206,59,227,47,16,58,17,182,189,28,42,223,183,170,213,
        119,248,152,2,44,154,163,70,221,153,101,155,167,43,172,9,
        129,22,39,253,19,98,108,110,79,113,224,232,178,185,112,104,
        218,246,97,228,251,34,242,193,238,210,144,12,191,179,162,241,
        81,51,145,235,249,14,239,107,49,192,214,31,181,199,106,157,
        184,84,204,176,115,121,50,45,127,4,150,254,138,236,205,93,
        222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
        // Repeat
        151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,
        140,36,103,30,69,142,8,99,37,240,21,10,23,190,6,148,
        247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,
        57,177,33,88,237,149,56,87,174,20,125,136,171,168,68,175,
        74,165,71,134,139,48,27,166,77,146,158,231,83,111,229,122,
        60,211,133,230,220,105,92,41,55,46,245,40,244,102,143,54,
        65,25,63,161,1,216,80,73,209,76,132,187,208,89,18,169,
        200,196,135,130,116,188,159,86,164,100,109,198,173,186,3,64,
        52,217,226,250,124,123,5,202,38,147,118,126,255,82,85,212,
        207,206,59,227,47,16,58,17,182,189,28,42,223,183,170,213,
        119,248,152,2,44,154,163,70,221,153,101,155,167,43,172,9,
        129,22,39,253,19,98,108,110,79,113,224,232,178,185,112,104,
        218,246,97,228,251,34,242,193,238,210,144,12,191,179,162,241,
        81,51,145,235,249,14,239,107,49,192,214,31,181,199,106,157,
        184,84,204,176,115,121,50,45,127,4,150,254,138,236,205,93,
        222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
    };

    float fade(float t) { return t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f); }
    float lerp(float t, float a, float b) { return a + t * (b - a); }

    float grad(int hash, float x, float y) {
        int h = hash & 3;
        float u = h < 2 ? x : y;
        float v = h < 2 ? y : x;
        return ((h & 1) ? -u : u) + ((h & 2) ? -2.0f * v : 2.0f * v);
    }
}

// ============================================================
//  Sound Effects Implementation
// ============================================================

extern "C" {

NATAF_API void nataf_play_wind(uint32_t duration_ms) {
    uint32_t step = duration_ms / 20;
    for (uint32_t i = 0; i < 20; i++) {
        uint32_t freq = 200 + (i % 5) * 50 + (rand() % 100);
        platform_beep(freq, step);
    }
}

NATAF_API void nataf_play_attack(void) {
    platform_beep(800, 40);
    platform_beep(600, 40);
    platform_beep(900, 30);
}

NATAF_API void nataf_play_pickup(void) {
    platform_beep(1200, 50);
    platform_beep(1500, 50);
    platform_beep(1800, 80);
}

NATAF_API void nataf_play_portal(void) {
    for (uint32_t f = 300; f <= 1500; f += 50) {
        platform_beep(f, 20);
    }
}

NATAF_API void nataf_play_boss_fanfare(void) {
    platform_beep(200, 200);
    platform_beep(250, 200);
    platform_beep(300, 200);
    platform_beep(200, 100);
    platform_beep(300, 100);
    platform_beep(400, 300);
}

NATAF_API void nataf_play_death(void) {
    platform_beep(500, 200);
    platform_beep(400, 200);
    platform_beep(300, 300);
    platform_beep(200, 500);
}

NATAF_API void nataf_play_victory(void) {
    // Triumphant melody
    uint32_t notes[] = {523, 659, 784, 1047, 784, 1047, 1318};
    uint32_t durations[] = {150, 150, 150, 200, 100, 100, 400};
    for (int i = 0; i < 7; i++) {
        platform_beep(notes[i], durations[i]);
    }
}

NATAF_API void nataf_play_tone(uint32_t frequency, uint32_t duration_ms) {
    platform_beep(frequency, duration_ms);
}

// ============================================================
//  Procedural Generation
// ============================================================

NATAF_API void nataf_seed_rng(uint32_t seed) {
    srand(seed);
}

NATAF_API int32_t nataf_random_range(int32_t min, int32_t max) {
    if (min >= max) return min;
    return min + (rand() % (max - min + 1));
}

NATAF_API float nataf_noise2d(float x, float y) {
    int xi = (int)floor(x) & 255;
    int yi = (int)floor(y) & 255;
    float xf = x - floor(x);
    float yf = y - floor(y);

    float u = fade(xf);
    float v = fade(yf);

    int aa = perm[perm[xi] + yi];
    int ab = perm[perm[xi] + yi + 1];
    int ba = perm[perm[xi + 1] + yi];
    int bb = perm[perm[xi + 1] + yi + 1];

    float result = lerp(v,
        lerp(u, grad(aa, xf, yf), grad(ba, xf - 1.0f, yf)),
        lerp(u, grad(ab, xf, yf - 1.0f), grad(bb, xf - 1.0f, yf - 1.0f))
    );

    return result;
}

// ============================================================
//  String Utility
// ============================================================

NATAF_API int32_t nataf_fill_pattern(char* buffer, int32_t buffer_size,
                                      char pattern_char, int32_t repeat_count) {
    if (!buffer || buffer_size <= 0 || repeat_count <= 0) return 0;

    int32_t count = (repeat_count < buffer_size - 1) ? repeat_count : (buffer_size - 1);
    memset(buffer, pattern_char, count);
    buffer[count] = '\0';
    return count;
}

NATAF_API uint32_t nataf_string_hash(const char* str) {
    if (!str) return 0;
    // DJB2 hash algorithm
    uint32_t hash = 5381;
    int c;
    while ((c = *str++)) {
        hash = ((hash << 5) + hash) + c;
    }
    return hash;
}

} // extern "C"
