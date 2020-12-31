using UnityEngine;

namespace UnityCommons {
	public static class MathExtensions {
#region General Mapping

		public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget) {
			return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
		}

		public static Vector2 Map(this Vector2 value, float fromSource, float toSource, float fromTarget, float toTarget) {
			return new Vector2(value.x.Map(fromSource, toSource, fromTarget, toTarget),
			                   value.y.Map(fromSource, toSource, fromTarget, toTarget));
		}

		public static Vector2 Map(this Vector2 value, Vector2 fromSource, Vector2 toSource, Vector2 fromTarget, Vector2 toTarget) {
			return new Vector2(value.x.Map(fromSource.x, toSource.x, fromTarget.x, toTarget.x),
			                   value.y.Map(fromSource.y, toSource.y, fromTarget.y, toTarget.y));
		}

		public static Vector3 Map(this Vector3 value, float fromSource, float toSource, float fromTarget, float toTarget) {
			return new Vector3(value.x.Map(fromSource, toSource, fromTarget, toTarget),
			                   value.y.Map(fromSource, toSource, fromTarget, toTarget),
			                   value.z.Map(fromSource, toSource, fromTarget, toTarget));
		}

		public static Vector3 Map(this Vector3 value, Vector3 fromSource, Vector3 toSource, Vector3 fromTarget, Vector3 toTarget) {
			return new Vector3(value.x.Map(fromSource.x, toSource.x, fromTarget.x, toTarget.x),
			                   value.y.Map(fromSource.y, toSource.y, fromTarget.y, toTarget.y),
			                   value.z.Map(fromSource.z, toSource.z, fromTarget.z, toTarget.z));
		}

#endregion

#region Float Extensions

		public static float Clamped(this float value, float min, float max) => value < min ? min : value > max ? max : value;
		public static float Clamped01(this float value) => Clamped(value, 0f, 1f);

#endregion
	}
}