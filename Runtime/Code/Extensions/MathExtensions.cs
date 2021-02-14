using System;
using UnityEngine;

namespace UnityCommons {
    public static partial class Extensions {
        #region General Mapping

        /// <summary>
        /// Maps <paramref name="value"/> from the range [<paramref name="fromSource"/>, <paramref name="toSource"/>] to the range [<paramref name="fromTarget"/>, <paramref name="toTarget"/>].
        /// </summary>
        /// <param name="value">Value to map</param>
        /// <param name="fromSource">Minimum source value</param>
        /// <param name="toSource">Maximum source value</param>
        /// <param name="fromTarget">Minimum target value</param>
        /// <param name="toTarget">Maximum target value</param>
        /// <returns><paramref name="value"/> mapped from range [<paramref name="fromSource"/>, <paramref name="toSource"/>] to range [<paramref name="fromTarget"/>, <paramref name="toTarget"/>]</returns>
        public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget) {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        /// <summary>
        /// Maps <paramref name="value"/> from the range [<paramref name="fromSource"/>, <paramref name="toSource"/>] to the range [<paramref name="fromTarget"/>, <paramref name="toTarget"/>].
        /// </summary>
        /// <param name="value">Value to map</param>
        /// <param name="fromSource">Minimum source value</param>
        /// <param name="toSource">Maximum source value</param>
        /// <param name="fromTarget">Minimum target value</param>
        /// <param name="toTarget">Maximum target value</param>
        /// <returns><paramref name="value"/> mapped from range [<paramref name="fromSource"/>, <paramref name="toSource"/>] to range [<paramref name="fromTarget"/>, <paramref name="toTarget"/>]</returns>
        public static Vector2 Map(this Vector2 value, float fromSource, float toSource, float fromTarget, float toTarget) {
            return new Vector2(value.x.Map(fromSource, toSource, fromTarget, toTarget),
                               value.y.Map(fromSource, toSource, fromTarget, toTarget));
        }

        /// <summary>
        /// Maps <paramref name="value"/> from the range [<paramref name="fromSource"/>, <paramref name="toSource"/>] to the range [<paramref name="fromTarget"/>, <paramref name="toTarget"/>].
        /// </summary>
        /// <param name="value">Value to map</param>
        /// <param name="fromSource">Minimum source value</param>
        /// <param name="toSource">Maximum source value</param>
        /// <param name="fromTarget">Minimum target value</param>
        /// <param name="toTarget">Maximum target value</param>
        /// <returns><paramref name="value"/> mapped from range [<paramref name="fromSource"/>, <paramref name="toSource"/>] to range [<paramref name="fromTarget"/>, <paramref name="toTarget"/>]</returns>
        public static Vector3 Map(this Vector3 value, float fromSource, float toSource, float fromTarget, float toTarget) {
            return new Vector3(value.x.Map(fromSource, toSource, fromTarget, toTarget),
                               value.y.Map(fromSource, toSource, fromTarget, toTarget),
                               value.z.Map(fromSource, toSource, fromTarget, toTarget));
        }

        #endregion

        #region Extensions

        /// <summary>
        /// Returns <paramref name="value" /> clamped to be in range [0, 1]
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <returns><paramref name="value" /> clamped to the range [0, 1]</returns>
        public static float Clamped01(this float value) => Clamped(value, 0.0f, 1.0f);
        
        /// <summary>
        /// Returns <paramref name="value" /> clamped to be in range [0, 1]
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <returns><paramref name="value" /> clamped to the range [0, 1]</returns>
        public static double Clamped01(this double value) => Clamped(value, 0.0d, 1.0d);

        /// <summary>
        /// Returns <paramref name="value" /> clamped to be in range [0, 1]
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <returns><paramref name="value" /> clamped to the range [0, 1]</returns>
        public static decimal Clamped01(this decimal value) => Clamped(value, 0.0m, 1.0m);
        
        /// <summary>
        /// Returns <paramref name="value" /> clamped to be in range [0, 1]
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <returns><paramref name="value" /> clamped to the range [0, 1]</returns>
        public static int Clamped01(this int value) => value <= 0 ? 0 : 1;
        
        #endregion

        #region Generic Extensions

        /// <summary>
        /// Returns <paramref name="value" /> clamped to be in range [<paramref name="min" />, <paramref name="max" />]
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <typeparam name="T">Type that implements IComparable</typeparam>
        /// <returns><paramref name="value" /> clamped to the range [<paramref name="min" />, <paramref name="max" />]</returns>
        public static T Clamped<T>(this T value, T min, T max) where T : System.IComparable<T> => value.CompareTo(min) < 0 ? min : value.CompareTo(max) > 0 ? max : value;

        /// <summary>
        /// Returns <code>true</code> if <paramref name="value"/> is between <paramref name="a"/> (inclusive) and <paramref name="b"/> (inclusive), otherwise <code>false</code>.
        /// </summary>
        /// <param name="value">The value to compare</param>
        /// <param name="a">The minimum value to compare against (inclusive)</param>
        /// <param name="b">The maximum value to compare against (inclusive)</param>
        /// <returns><code>true</code> if <paramref name="value"/> is between <paramref name="a"/> (inclusive) and <paramref name="b"/> (inclusive), otherwise <code>false</code></returns>
        public static bool Between<T>(this T value, T a, T b) where T : IComparable<T> => value.CompareTo(a) >= 0 && value.CompareTo(b) <= 0;

        #endregion
    }
}