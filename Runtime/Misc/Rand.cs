using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCommons {
    public static class Rand {
        private static readonly RNGProvider provider = new RNGProvider();
        private static readonly Stack<ulong> stateStack = new Stack<ulong>();
        private static uint iterations;
        public static int Sign = Bool ? 1 : -1;
        private const float pi = 3.1415926535897932384626433832795028841971693993751058209749445923f;
        private const float twoPi = 6.2831853071795864769252867665590057683943387987502116419498891846f;

        public static int Seed {
            set {
                if (stateStack.Count == 0)
                    Debug.LogError("Modifying the initial rand seed. Call PushState() first. The initial rand seed should always be based on the startup time and set only once.");
                provider.Seed = (uint) value;
                iterations = 0U;
            }
        }

        public static float Float => provider.GetFloat(iterations++);
        public static int Int => provider.GetInt(iterations++);
        public static bool Bool => Float < 0.5;

        private static ulong StateCompressed {
            get => provider.Seed | ((ulong) iterations << 32);
            set {
                provider.Seed = (uint) (value & uint.MaxValue);
                iterations = (uint) ((value >> 32) & uint.MaxValue);
            }
        }

        static Rand() {
            provider.Seed = (uint) DateTime.Now.GetHashCode();
        }

        #region General
        public static int Range(int min, int max) {
            if (max <= min)
                return min;
            return min + Mathf.Abs(Int % (max - min));
        }

        public static int Range(int max) {
            return Range(0, max);
        }

        public static int RangeInclusive(int min, int max) {
            if (max <= min)
                return min;
            return Range(min, max + 1);
        }

        public static float Range(float min, float max) {
            if (max <= (double) min)
                return min;
            return Float * (max - min) + min;
        }

        public static float Range(float max) {
            return Range(0f, max);
        }

        public static bool Chance(float chance) {
            if (chance <= 0.0)
                return false;
            if (chance >= 1.0)
                return true;
            return Float < (double) chance;
        }
        #endregion

        #region Seeded
        public static bool ChanceSeeded(float chance, int seed) {
            PushState(seed);
            var flag = Chance(chance);
            PopState();
            return flag;
        }

        public static float FloatSeeded(int seed) {
            PushState(seed);
            var num = Float;
            PopState();
            return num;
        }

        public static float RangeSeeded(float min, float max, int seed) {
            PushState(seed);
            var num = Range(min, max);
            PopState();
            return num;
        }

        public static int RangeSeeded(int min, int max, int seed) {
            PushState(seed);
            var num = Range(min, max);
            PopState();
            return num;
        }

        public static int RangeInclusiveSeeded(int min, int max, int seed) {
            PushState(seed);
            var num = RangeInclusive(min, max);
            PopState();
            return num;
        }
        #endregion

        #region Element
        public static T Element<T>(T a, T b) {
            if (Bool)
                return a;
            return b;
        }

        public static T Element<T>(T a, T b, T c) {
            var num = Float;
            if (num < 0.333330005407333)
                return a;
            if (num < 0.666660010814667)
                return b;
            return c;
        }

        public static T Element<T>(T a, T b, T c, T d) {
            var num = Float;
            if (num < 0.25)
                return a;
            if (num < 0.5)
                return b;
            if (num < 0.75)
                return c;
            return d;
        }

        public static T Element<T>(T a, T b, T c, T d, T e) {
            var num = Float;
            if (num < 0.2f)
                return a;
            if (num < 0.4f)
                return b;
            if (num < 0.6f)
                return c;
            if (num < 0.8f)
                return d;
            return e;
        }

        public static T Element<T>(T a, T b, T c, T d, T e, T f) {
            var num = Float;
            if (num < 0.166659995913506)
                return a;
            if (num < 0.333330005407333)
                return b;
            if (num < 0.5)
                return c;
            if (num < 0.666660010814667)
                return d;
            if (num < 0.833329975605011)
                return e;
            return f;
        }
        public static T Element<T>(params T[] items) {
            return ListItem(items);
        }
        #endregion

        #region Lists
        public static T ListItem<T>(IList<T> list) {
            return list[Range(0, list.Count)];
        }
        #endregion

        #region Vector Ranges
        public static float Range(Vector2 range) {
            return Range(range.x, range.y);
        }
        public static int Range(Vector2Int range) {
            return Range(range.x, range.y);
        }
        public static int RangeInclusive(Vector2Int range) {
            return RangeInclusive(range.x, range.y);
        }
        #endregion
        
        #region Utilities
        public static float Gaussian(float centerX = 0.0f, float widthFactor = 1f) {
            return Mathf.Sqrt(-2f * Mathf.Log(Float)) * Mathf.Sin(twoPi * Float) * widthFactor + centerX;
        }

        public static float GaussianAsymmetric(float centerX = 0.0f, float lowerWidthFactor = 1f, float upperWidthFactor = 1f) {
            var num = Mathf.Sqrt(-2f * Mathf.Log(Float)) * Mathf.Sin(twoPi * Float);
            if (num <= 0.0)
                return num * lowerWidthFactor + centerX;
            return num * upperWidthFactor + centerX;
        }

        public static void PushState() {
            stateStack.Push(StateCompressed);
        }

        public static void PushState(int replacementSeed) {
            PushState();
            Seed = replacementSeed;
        }

        public static void PopState() {
            StateCompressed = stateStack.Pop();
        }

        public static void EnsureStateStackEmpty() {
            if (stateStack.Count <= 0)
                return;
            Debug.LogWarning("Random state stack is not empty. There were more calls to PushState than PopState. Fixing.");
            while (stateStack.Any())
                PopState();
        }
        #endregion
    }

    internal class RNGProvider {
        public uint Seed = (uint) DateTime.Now.GetHashCode();

        public int GetInt(uint iterations) {
            return (int) GetHash((int) iterations);
        }

        public float GetFloat(uint iterations) {
            return (float) ((GetInt(iterations) - (double) int.MinValue) / uint.MaxValue);
        }

        private uint GetHash(int buffer) {
            var num1 = Rotate(Seed + 374761393U + 4U + (uint) (buffer * -1028477379), 17) * 668265263U;
            var num2 = (num1 ^ (num1 >> 15)) * 2246822519U;
            var num3 = (num2 ^ (num2 >> 13)) * 3266489917U;
            return num3 ^ (num3 >> 16);
        }

        private static uint Rotate(uint value, int count) {
            return (value << count) | (value >> (32 - count));
        }
    }
}