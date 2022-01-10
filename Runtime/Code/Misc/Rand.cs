using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCommons {
    public static class Rand {
        private static readonly RNGProvider provider = new RNGProvider();
        private static readonly Stack<ulong> stateStack = new Stack<ulong>();
        private static uint iterations;
        private const float pi = 3.1415926535897932f;
        private const float twoPi = 6.2831853071795864f;

        public static int Seed {
            set {
                if (stateStack.Count == 0)
                    Debug.LogError("Modifying the initial rand seed. Call PushState() first. The initial rand seed should always be based on the startup time and set only once.");
                provider.Seed = (uint) value;
                iterations = 0U;
            }
        }

        /// <summary>
        /// Returns a random float from 0 to 1 (both inclusive)
        /// </summary>
        public static float Float => provider.GetFloat(iterations++);
        
        /// <summary>
        /// Returns a random integer
        /// </summary>
        public static int Int => provider.GetInt(iterations++);
        
        /// <summary>
        /// Returns a random long
        /// </summary>
        public static long Long => BitConverter.ToInt64(Bytes(8), 0);
        
        /// <summary>
        /// Returns a random bool
        /// </summary>
        public static bool Bool => Float < 0.5;
        
        /// <summary>
        /// Returns a random sign. (Either 1 or -1)
        /// </summary>
        public static int Sign => Bool ? 1 : -1;

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

        #region Lists

        /// <summary>
        /// Returns a random element from <paramref name="list"/>
        /// </summary>
        public static T ListItem<T>(IList<T> list) {
            return list[Range(0, list.Count)];
        }
        
        /// <summary>
        /// Returns a random element from <paramref name="readOnlyList"/>
        /// </summary>
        public static T ReadOnlyListItem<T>(IReadOnlyList<T> readOnlyList) {
            return readOnlyList[Range(0, readOnlyList.Count)];
        }

        #endregion

        #region General

        /// <summary>
        /// Returns a random integer from <paramref name="min"/> (inclusive) to <paramref name="max"/> (exclusive)
        /// </summary>
        /// <param name="min">Minimum range (inclusive)</param>
        /// <param name="max">Maximum range (exclusive)</param>
        /// <returns>A random integer from <paramref name="min"/> (inclusive) to <paramref name="max"/> (exclusive)</returns>
        public static int Range(int min, int max) {
            if (max <= min)
                return min;
            return min + Mathf.Abs(Int % (max - min));
        }
        /// <summary>
        /// Returns a random integer from 0 (inclusive) to <paramref name="max"/> (exclusive)
        /// </summary>
        /// <param name="max">Maximum range (exclusive)</param>
        /// <returns>A random integer from 0 (inclusive) to <paramref name="max"/> (exclusive)</returns>
        public static int Range(int max) {
            return Range(0, max);
        }

        /// <summary>
        /// Returns a random integer from <paramref name="min"/> (inclusive) to <paramref name="max"/> (inclusive)
        /// </summary>
        /// <param name="min">Minimum range (inclusive)</param>
        /// <param name="max">Maximum range (inclusive)</param>
        /// <returns>A random integer from <paramref name="min"/> (inclusive) to <paramref name="max"/> (inclusive)</returns>
        public static int RangeInclusive(int min, int max) {
            if (max <= min)
                return min;
            return Range(min, max + 1);
        }

        /// <summary>
        /// Returns a random float from <paramref name="min"/> (inclusive) to <paramref name="max"/> (inclusive)
        /// </summary>
        /// <param name="min">Minimum range (inclusive)</param>
        /// <param name="max">Maximum range (inclusive)</param>
        /// <returns>A random float from <paramref name="min"/> (inclusive) to <paramref name="max"/> (inclusive)</returns>
        public static float Range(float min, float max) {
            if (max <= (double) min)
                return min;
            return Float * (max - min) + min;
        }

        /// <summary>
        /// Returns a random float from 0 (inclusive) to <paramref name="max"/> (inclusive)
        /// </summary>
        /// <param name="max">Maximum range (inclusive)</param>
        /// <returns>A random float from 0 (inclusive) to <paramref name="max"/> (inclusive)</returns>
        public static float Range(float max) {
            return Range(0f, max);
        }

        /// <summary>
        /// Returns true if a random chance occurs, false otherwise
        /// </summary>
        /// <param name="chance">The chance (between 0 and 1)</param>
        /// <returns>true if a random chance occurs, false otherwise</returns>
        public static bool Chance(float chance) {
            if (chance <= 0.0)
                return false;
            if (chance >= 1.0)
                return true;
            return Float < (double) chance;
        }

        /// <summary>
        /// Returns an array of <paramref name="count"/> random bytes
        /// </summary>
        /// <param name="count">The number of bytes to generate</param>
        /// <returns>An array of <paramref name="count"/> random bytes</returns>
        public static byte[] Bytes(int count) {
            byte[] buffer = new byte[count];
            for (int i = 0; i < buffer.Length; i++) {
                buffer[i] = (byte) (Int % 256);
            }
            return buffer;
        }

        #endregion

        #region Geometric
        
        /// <summary>
        /// Returns a random unit vector
        /// </summary>
        public static Vector3 UnitVector3 => new Vector3(Gaussian(), Gaussian(), Gaussian()).normalized;

        /// <summary>
        /// Returns a random unit vector
        /// </summary>
        public static Vector2 UnitVector2 => new Vector2(Gaussian(), Gaussian()).normalized;

        /// <summary>
        /// Returns a random point inside the unit circle
        /// </summary>
        public static Vector2 InsideUnitCircle {
            get {
                Vector2 vector;
                do {
                    vector = new Vector2(Float - 0.5f, Float - 0.5f) * 2f;
                } while (vector.sqrMagnitude > 1.0f);

                return vector;
            }
        }

        /// <summary>
        /// Returns a random point inside the unit circle
        /// </summary>
        public static Vector3 InsideUnitCircleVec3 {
            get {
                Vector2 insideUnitCircle = InsideUnitCircle;
                return new Vector3(insideUnitCircle.x, 0.0f, insideUnitCircle.y);
            }
        }

        /// <summary>
        /// Returns a random point inside the unit sphere
        /// </summary>
        public static Vector3 InsideUnitSphere {
            get {
                Vector3 vector;
                do {
                    vector = new Vector3(Float - 0.5f, Float - 0.5f, Float - 0.5f) * 2f;
                } while (vector.sqrMagnitude > 1.0f);

                return vector;
            }
        }

        #endregion

        #region Seeded

        /// <summary>
        /// Returns a random integer in from <paramref name="min"/> (inclusive) to <paramref name="max"/> (exclusive) using <paramref name="seed"/> as a seed
        /// </summary>
        /// <param name="min">Minimum range (inclusive)</param>
        /// <param name="max">Maximum range (exclusive)</param>
        /// <param name="seed">Custom seed</param>
        /// <returns>A random integer in from <paramref name="min"/> (inclusive) to <paramref name="max"/> (exclusive) using <paramref name="seed"/> as a seed</returns>
        public static int RangeSeeded(int min, int max, int seed) {
            PushState(seed);
            int num = Range(min, max);
            PopState();
            return num;
        }

        /// <summary>
        /// Returns a random integer in from <paramref name="min"/> (inclusive) to <paramref name="max"/> (inclusive) using <paramref name="seed"/> as a seed
        /// </summary>
        /// <param name="min">Minimum range (inclusive)</param>
        /// <param name="max">Maximum range (inclusive)</param>
        /// <param name="seed">Custom seed</param>
        /// <returns>A random integer in from <paramref name="min"/> (inclusive) to <paramref name="max"/> (inclusive) using <paramref name="seed"/> as a seed</returns>
        public static int RangeInclusiveSeeded(int min, int max, int seed) {
            PushState(seed);
            int num = RangeInclusive(min, max);
            PopState();
            return num;
        }
        
        /// <summary>
        /// Returns a random float in from <paramref name="min"/> (inclusive) to <paramref name="max"/> (inclusive) using <paramref name="seed"/> as a seed
        /// </summary>
        /// <param name="min">Minimum range (inclusive)</param>
        /// <param name="max">Maximum range (inclusive)</param>
        /// <param name="seed">Custom seed</param>
        /// <returns>A random float in from <paramref name="min"/> (inclusive) to <paramref name="max"/> (inclusive) using <paramref name="seed"/> as a seed</returns>
        public static float RangeSeeded(float min, float max, int seed) {
            PushState(seed);
            float num = Range(min, max);
            PopState();
            return num;
        }

        /// <summary>
        /// Returns a random float from 0 to 1 (both inclusive) using <paramref name="seed"/> as a seed
        /// </summary>
        /// <param name="seed">Custom seed</param>
        /// <returns>A random float from 0 to 1 (both inclusive) using <paramref name="seed"/> as a seed</returns>
        public static float FloatSeeded(int seed) {
            PushState(seed);
            float num = Float;
            PopState();
            return num;
        }

        /// <summary>
        /// Returns true if a random chance occurs using <paramref name="seed"/> as a seed, false otherwise
        /// </summary>
        /// <param name="chance">The chance (between 0 and 1)</param>
        /// <param name="seed">Custom seed</param>
        /// <returns>true if a random chance occurs using <paramref name="seed"/> as a seed, false otherwise</returns>
        public static bool ChanceSeeded(float chance, int seed) {
            PushState(seed);
            bool flag = Chance(chance);
            PopState();
            return flag;
        }

        /// <summary>
        /// Returns an array of <paramref name="count"/> random bytes using <paramref name="seed"/> as a seed
        /// </summary>
        /// <param name="count">The number of bytes to generate</param>
        /// <param name="seed">Custom seed</param>
        /// <returns>An array of <paramref name="count"/> random bytes</returns>
        public static byte[] BytesSeeded(int count, int seed) {
            PushState(seed);
            byte[] bytes = Bytes(count);
            PopState();
            return bytes;
        }

        #endregion

        #region Element

        /// <summary>
        /// Returns either <paramref name="a"/> or <paramref name="b"/> randomly.
        /// </summary>
        public static T Element<T>(T a, T b) {
            if (Bool)
                return a;
            return b;
        }

        /// <summary>
        /// Returns either <paramref name="a"/>, <paramref name="b"/>, or <paramref name="c"/> randomly.
        /// </summary>
        public static T Element<T>(T a, T b, T c) {
            float num = Float;
            if (num < 0.333330005407333)
                return a;
            if (num < 0.666660010814667)
                return b;
            return c;
        }

        /// <summary>
        /// Returns either <paramref name="a"/>, <paramref name="b"/>, <paramref name="c"/>, or <paramref name="d"/> randomly.
        /// </summary>
        public static T Element<T>(T a, T b, T c, T d) {
            float num = Float;
            if (num < 0.25)
                return a;
            if (num < 0.5)
                return b;
            if (num < 0.75)
                return c;
            return d;
        }

        /// <summary>
        /// Returns either <paramref name="a"/>, <paramref name="b"/>, <paramref name="c"/>, <paramref name="d"/>, or <paramref name="e"/> randomly.
        /// </summary>
        public static T Element<T>(T a, T b, T c, T d, T e) {
            float num = Float;
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

        /// <summary>
        /// Returns either <paramref name="a"/>, <paramref name="b"/>, <paramref name="c"/>, <paramref name="d"/>, <paramref name="e"/>, or <paramref name="f"/> randomly.
        /// </summary>
        public static T Element<T>(T a, T b, T c, T d, T e, T f) {
            float num = Float;
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

        /// <summary>
        /// Returns a random element from <paramref name="items"/>
        /// </summary>
        public static T Element<T>(params T[] items) {
            return ListItem(items);
        }

        #endregion

        #region Vector Ranges

        /// <summary>
        /// Returns a random float from <paramref name="range"/>.x to <paramref name="range"/>.y
        /// </summary>
        public static float Range(Vector2 range) {
            return Range(range.x, range.y);
        }

        /// <summary>
        /// Returns a random integer from <paramref name="range"/>.x (inclusive) to <paramref name="range"/>.y (exclusive)
        /// </summary>
        public static int Range(Vector2Int range) {
            return Range(range.x, range.y);
        }

        /// <summary>
        /// Returns a random integer from <paramref name="range"/>.x (inclusive) to <paramref name="range"/>.y (inclusive)
        /// </summary>
        public static int RangeInclusive(Vector2Int range) {
            return RangeInclusive(range.x, range.y);
        }

        #endregion

        #region Utilities

        public static float Gaussian(float centerX = 0.0f, float widthFactor = 1f) {
            return Mathf.Sqrt(-2f * Mathf.Log(Float)) * Mathf.Sin(twoPi * Float) * widthFactor + centerX;
        }

        public static float GaussianAsymmetric(float centerX = 0.0f, float lowerWidthFactor = 1f, float upperWidthFactor = 1f) {
            float num = Mathf.Sqrt(-2f * Mathf.Log(Float)) * Mathf.Sin(twoPi * Float);
            if (num <= 0.0)
                return num * lowerWidthFactor + centerX;
            return num * upperWidthFactor + centerX;
        }

        public static void PushState() {
            stateStack.Push(StateCompressed);
        }

        /// <summary>
        /// Replaces the current seed with <paramref name="replacementSeed"/>. Use <see cref="PopState"/> to undo this operation and retrieve the original state
        /// </summary>
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
        
        #region Miscellaneous

        /// <summary>
        /// Generates a random color using an HSV range and alpha range.
        /// </summary>
        /// <param name="hueMin">Minimum hue [range 0..1]</param>
        /// <param name="hueMax">Maximum hue [range 0..1]</param>
        /// <param name="saturationMin">Minimum saturation [range 0..1]</param>
        /// <param name="saturationMax">Maximum saturation [range 0..1]</param>
        /// <param name="valueMin">Minimum value [range 0..1]</param>
        /// <param name="valueMax">Maximum value [range 0..1]</param>
        /// <param name="alphaMin">Minimum alpha [range 0..1]</param>
        /// <param name="alphaMax">Maximum alpha [range 0..1]</param>
        /// <returns>A random color with HSV and alpha values in input range</returns>
        /// <footer>This is the same implementation as <see cref="UnityEngine.Random.ColorHSV()">UnityEngine.Random.ColorHSV()</see></footer>
        public static Color ColorHSV(
            float hueMin = 0.0f, float hueMax = 1.0f, float saturationMin = 0.0f, float saturationMax = 1.0f,
            float valueMin = 0.0f, float valueMax = 1.0f, float alphaMin = 1.0f, float alphaMax = 1.0f
        ) {
            Color color = Color.HSVToRGB(
                Range(hueMin.Clamped01(), hueMax.Clamped01()),
                Range(saturationMin.Clamped01(), saturationMax.Clamped01()),
                Range(valueMin.Clamped01(), valueMax.Clamped01())
            );
            color.a = Range(alphaMin.Clamped01(), alphaMax.Clamped01());
            return color;
        }

        #endregion
        
        private class RNGProvider {
            public uint Seed = (uint) DateTime.Now.GetHashCode();

            public int GetInt(uint iterations) {
                return (int) GetHash((int) iterations);
            }

            public float GetFloat(uint iterations) {
                return (float) ((GetInt(iterations) - (double) int.MinValue) / uint.MaxValue);
            }

            private uint GetHash(int buffer) {
                uint num1 = Rotate(Seed + 374761393U + 4U + (uint) (buffer * -1028477379), 17) * 668265263U;
                uint num2 = (num1 ^ (num1 >> 15)) * 2246822519U;
                uint num3 = (num2 ^ (num2 >> 13)) * 3266489917U;
                return num3 ^ (num3 >> 16);
            }

            private static uint Rotate(uint value, int count) {
                return (value << count) | (value >> (32 - count));
            }
        }

    }
}