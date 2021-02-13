using UnityEngine;

namespace UnityCommons {
    public static partial class Utils {
        /// <summary>
        /// Returns the world position of <value>Input.mousePosition</value> with y = 0,
        /// using <value>Camera.main</value> as the active camera.
        /// </summary>
        public static Vector3 MouseWorldPositionNoY() {
            var position = MouseWorldPosition();
            position.y = 0;
            return position;
        }
        
        /// <summary>
        /// Returns the world position of <value>Input.mousePosition</value> with z = 0,
        /// using <value>Camera.main</value> as the active camera.
        /// </summary>
        public static Vector3 MouseWorldPositionNoZ() {
            var position = MouseWorldPosition();
            position.z = 0;
            return position;
        }
        
        /// <summary>
        /// Returns the world position of <value>Input.mousePosition</value>
        /// using <value>Camera.main</value> as the active camera.
        /// </summary>
        public static Vector3 MouseWorldPosition() {
            return MouseWorldPosition(Input.mousePosition, Camera.main);
        }
        
        /// <summary>
        /// Returns the world position of <paramref name="screenPoint"/>
        /// using <value>Camera.main</value> as the active camera.
        /// </summary>
        public static Vector3 MouseWorldPosition(Vector3 screenPoint) {
            return MouseWorldPosition(screenPoint, Camera.main);
        }

        /// <summary>
        /// Returns the world position of <paramref name="screenPoint"/>
        /// using <paramref name="camera"/> as the active camera.
        /// </summary>
        public static Vector3 MouseWorldPosition(Vector3 screenPoint, Camera camera) {
            return camera.ScreenToWorldPoint(screenPoint);
        }
    }
}