// NatafNative.h - Header for the Nataf native sound & effects library
// This C++ library provides low-level audio and visual effects for the Nataf game.
// It can be compiled as a shared library (.dll / .so) and consumed via P/Invoke from C#.

#pragma once

#ifdef _WIN32
    #define NATAF_API __declspec(dllexport)
#else
    #define NATAF_API __attribute__((visibility("default")))
#endif

#include <cstdint>

extern "C" {

    // ============================================================
    //  Sound Effects - Desert themed audio feedback
    // ============================================================

    /// Play a desert wind ambient sound effect (frequency sweep)
    NATAF_API void nataf_play_wind(uint32_t duration_ms);

    /// Play a sword clash / attack sound
    NATAF_API void nataf_play_attack(void);

    /// Play a treasure pickup chime
    NATAF_API void nataf_play_pickup(void);

    /// Play a portal activation sweep
    NATAF_API void nataf_play_portal(void);

    /// Play a dramatic boss encounter fanfare
    NATAF_API void nataf_play_boss_fanfare(void);

    /// Play a death/game-over melody
    NATAF_API void nataf_play_death(void);

    /// Play a victory celebration melody
    NATAF_API void nataf_play_victory(void);

    /// Play a custom tone (frequency in Hz, duration in ms)
    NATAF_API void nataf_play_tone(uint32_t frequency, uint32_t duration_ms);

    // ============================================================
    //  Procedural Generation Helpers
    // ============================================================

    /// Seed the native random number generator
    NATAF_API void nataf_seed_rng(uint32_t seed);

    /// Generate a random integer in [min, max] range
    NATAF_API int32_t nataf_random_range(int32_t min, int32_t max);

    /// Generate perlin-style noise value for procedural terrain
    /// Returns a float in [-1.0, 1.0]
    NATAF_API float nataf_noise2d(float x, float y);

    // ============================================================
    //  String Utility (for high-perf rendering support)
    // ============================================================

    /// Fill a buffer with a repeated character pattern (for fast sand rendering)
    /// Returns number of characters written
    NATAF_API int32_t nataf_fill_pattern(char* buffer, int32_t buffer_size,
                                          char pattern_char, int32_t repeat_count);

    /// Compute a simple hash of a string (for level checksums)
    NATAF_API uint32_t nataf_string_hash(const char* str);
}
