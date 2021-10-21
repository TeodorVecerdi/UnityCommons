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
        public static int Map(this int value, int fromSource, int toSource, int fromTarget, int toTarget) {
            return (int) (((float)value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget);
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
        public static UnityEngine.Vector2 Map(this UnityEngine.Vector2 value, float fromSource, float toSource, float fromTarget, float toTarget) {
            return new UnityEngine.Vector2(value.x.Map(fromSource, toSource, fromTarget, toTarget),
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
        public static UnityEngine.Vector3 Map(this UnityEngine.Vector3 value, float fromSource, float toSource, float fromTarget, float toTarget) {
            return new UnityEngine.Vector3(value.x.Map(fromSource, toSource, fromTarget, toTarget),
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

        /// <summary>
        /// Returns <paramref name="value"/> wrapped between the range <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        public static float Wrapped(this float value, float min, float max) {
            if (value < min)
                return max - (min - value) % (max - min);
            return min + (value - min) % (max - min);
        }
        
        /// <summary>
        /// Returns <paramref name="value"/> wrapped between the range <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        public static double Wrapped(this double value, double min, double max) {
            if (value < min)
                return max - (min - value) % (max - min);
            return min + (value - min) % (max - min);
        }
        
        /// <summary>
        /// Returns <paramref name="value"/> wrapped between the range <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        public static decimal Wrapped(this decimal value, decimal min, decimal max) {
            if (value < min)
                return max - (min - value) % (max - min);
            return min + (value - min) % (max - min);
        }
        
        /// <summary>
        /// Returns <paramref name="value"/> wrapped between the range <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        public static int Wrapped(this int value, int min, int max) {
            if (value < min)
                return max - (min - value) % (max - min);
            return min + (value - min) % (max - min);
        }

        /// <summary>
        /// Returns <paramref name="value"/> rounded to the nearest multiple of <paramref name="n"/>
        /// </summary>
        /// <param name="value">The value to round</param>
        /// <param name="n">The number to round to nearest multiple of</param>
        /// <returns><paramref name="value"/> rounded to the nearest multiple of <paramref name="n"/></returns>
        public static int RoundedTo(this int value, int n) {
            var remainder = value % n;
            if (System.Math.Abs(remainder) < n / 2.0) return value - remainder;
            if (value > 0) return value + (n - System.Math.Abs(remainder));
            return value - (n - System.Math.Abs(remainder));
        }

        /// <summary>
        /// Returns <paramref name="value"/> rounded to the nearest multiple of <paramref name="n"/>
        /// </summary>
        /// <param name="value">The value to round</param>
        /// <param name="n">The number to round to nearest multiple of</param>
        /// <returns><paramref name="value"/> rounded to the nearest multiple of <paramref name="n"/></returns>
        public static float RoundedTo(this float value, float n) => UnityEngine.Mathf.Round(value / n) * n;

        /// <summary>
        /// Returns <paramref name="value"/> rounded to the nearest multiple of <paramref name="n"/>
        /// </summary>
        /// <param name="value">The value to round</param>
        /// <param name="n">The number to round to nearest multiple of</param>
        /// <returns><paramref name="value"/> rounded to the nearest multiple of <paramref name="n"/></returns>
        public static double RoundedTo(this double value, double n) => System.Math.Round(value / n, System.MidpointRounding.AwayFromZero) * n;

        /// <summary>
        /// Returns <paramref name="value"/> rounded to the nearest multiple of <paramref name="n"/>
        /// </summary>
        /// <param name="value">The value to round</param>
        /// <param name="n">The number to round to nearest multiple of</param>
        /// <returns><paramref name="value"/> rounded to the nearest multiple of <paramref name="n"/></returns>
        public static decimal RoundedTo(this decimal value, decimal n) => System.Math.Round(value / n, System.MidpointRounding.AwayFromZero) * n;

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
        public static T Clamped<T>(this T value, T min, T max) where T : System.IComparable<T> {
            // swap is min is greater than max
            if (min.CompareTo(max) > 0) {
                (min, max) = (max, min);
            }
            return value.CompareTo(min) < 0 ? min : value.CompareTo(max) > 0 ? max : value;
        }

        /// <summary>
        /// Constrains <paramref name="value"/> to be at least <paramref name="min"/> 
        /// </summary>
        /// <param name="value">The value to constrain</param>
        /// <param name="min">The minimum value</param>
        /// <typeparam name="T">Type that implements IComparable</typeparam>
        /// <returns><paramref name="min"/> if <paramref name="value"/> is less than <paramref name="min"/>, and <paramref name="value"/> otherwise</returns>
        public static T Min<T>(this T value, T min) where T : System.IComparable<T> {
            return value.CompareTo(min) < 0 ? min : value;
        }
        
        /// <summary>
        /// Constrains <paramref name="value"/> to be at most <paramref name="max"/> 
        /// </summary>
        /// <param name="value">The value to constrain</param>
        /// <param name="max">The maximum value</param>
        /// <typeparam name="T">Type that implements IComparable</typeparam>
        /// <returns><paramref name="max"/> if <paramref name="value"/> is greater than <paramref name="max"/>, and <paramref name="value"/> otherwise</returns>
        public static T Max<T>(this T value, T max) where T : System.IComparable<T> {
            return value.CompareTo(max) > 0 ? max : value;
        }

        /// <summary>
        /// Returns <code>true</code> if <paramref name="value"/> is between <paramref name="a"/> (inclusive) and <paramref name="b"/> (inclusive), otherwise <code>false</code>.
        /// </summary>
        /// <param name="value">The value to compare</param>
        /// <param name="a">The minimum value to compare against (inclusive)</param>
        /// <param name="b">The maximum value to compare against (inclusive)</param>
        /// <typeparam name="T">Type that implements <see cref="System.IComparable{T}"/>IComparable</typeparam>
        /// <returns><c>true</c> if <paramref name="value"/> is between <paramref name="a"/> (inclusive) and <paramref name="b"/> (inclusive), otherwise <c>false</c></returns>
        public static bool Between<T>(this T value, T a, T b) where T : System.IComparable<T> => value.CompareTo(a) >= 0 && value.CompareTo(b) <= 0;

        #endregion
    }
}