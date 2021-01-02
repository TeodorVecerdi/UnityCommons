using System;
using UnityEngine;

namespace UnityCommons {
	public static class MathExtensions {
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
		public static Vector2 Map(this Vector2 value, Vector2 fromSource, Vector2 toSource, Vector2 fromTarget, Vector2 toTarget) {
			return new Vector2(value.x.Map(fromSource.x, toSource.x, fromTarget.x, toTarget.x),
			                   value.y.Map(fromSource.y, toSource.y, fromTarget.y, toTarget.y));
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

		/// <summary>
		/// Maps <paramref name="value"/> from the range [<paramref name="fromSource"/>, <paramref name="toSource"/>] to the range [<paramref name="fromTarget"/>, <paramref name="toTarget"/>].
		/// </summary>
		/// <param name="value">Value to map</param>
		/// <param name="fromSource">Minimum source value</param>
		/// <param name="toSource">Maximum source value</param>
		/// <param name="fromTarget">Minimum target value</param>
		/// <param name="toTarget">Maximum target value</param>
		/// <returns><paramref name="value"/> mapped from range [<paramref name="fromSource"/>, <paramref name="toSource"/>] to range [<paramref name="fromTarget"/>, <paramref name="toTarget"/>]</returns>
		public static Vector3 Map(this Vector3 value, Vector3 fromSource, Vector3 toSource, Vector3 fromTarget, Vector3 toTarget) {
			return new Vector3(value.x.Map(fromSource.x, toSource.x, fromTarget.x, toTarget.x),
			                   value.y.Map(fromSource.y, toSource.y, fromTarget.y, toTarget.y),
			                   value.z.Map(fromSource.z, toSource.z, fromTarget.z, toTarget.z));
		}

#endregion

#region Float Extensions

		/// <summary>
		/// Returns <paramref name="value" /> clamped to be in range [<paramref name="min" />, <paramref name="max" />]
		/// </summary>
		/// <param name="value">The value to clamp</param>
		/// <param name="min">The minimum value</param>
		/// <param name="max">The maximum value</param>
		/// <returns><paramref name="value" /> clamped to the range [<paramref name="min" />, <paramref name="max" />]</returns>
		public static float Clamped(this float value, float min, float max) => value < min ? min : value > max ? max : value;

		/// <summary>
		/// Returns <paramref name="value" /> clamped to be in range [0, 1]
		/// </summary>
		/// <param name="value">The value to clamp</param>
		/// <returns><paramref name="value" /> clamped to the range [0, 1]</returns>
		public static float Clamped01(this float value) => Clamped(value, 0f, 1f);

		/// <summary>
		/// Returns <code>true</code> if <paramref name="value"/> is between <paramref name="a"/> (inclusive) and <paramref name="b"/> (inclusive), otherwise <code>false</code>.
		/// </summary>
		/// <param name="value">The value to compare</param>
		/// <param name="a">The minimum value to compare against (inclusive)</param>
		/// <param name="b">The maximum value to compare against (inclusive)</param>
		/// <returns><code>true</code> if <paramref name="value"/> is between <paramref name="a"/> (inclusive) and <paramref name="b"/> (inclusive), otherwise <code>false</code></returns>
		public static bool Between(this float value, float a, float b) => value >= a && value <= b;

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
		public static T Clamped<T>(this T value, T min, T max) where T:System.IComparable<T> => value.CompareTo(min) < 0  ? min : value.CompareTo(max) > 0 ? max : value;

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