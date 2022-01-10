using TMPro;
using UnityEngine;

namespace UnityCommons {
    public static partial class Utils {
        /// <summary>
        /// Returns the world position of the mouse in screen position
        /// using <value>Camera.main</value> as the active camera, with z = 0.
        /// </summary>
        public static Vector3 MouseWorld_NoZ() {
            Vector3 world = MouseWorld();
            world.z = 0;
            return world;
        }

        /// <summary>
        /// Returns the world position of the mouse in screen position
        /// using <value>Camera.main</value> as the active camera, with y = 0.
        /// </summary>
        public static Vector3 MouseWorld_NoY() {
            Vector3 world = MouseWorld();
            world.y = 0;
            return world;
        }

        /// <summary>
        /// Returns the world position of the mouse in screen position
        /// using <value>Camera.main</value> as the active camera.
        /// </summary>
        public static Vector3 MouseWorld() {
            return ScreenToWorld(Input.mousePosition, Camera.main);
        }

        /// <summary>
        /// Returns the world position of the mouse in screen position
        /// using <paramref name="camera"/> as the active camera.
        /// </summary>
        public static Vector3 MouseWorld(Camera camera) {
            return ScreenToWorld(Input.mousePosition, camera);
        }

        /// <summary>
        /// Returns the world position of the mouse in screen position using <paramref name="camera"/> as the
        /// active camera, and <paramref name="layerMask"/> as the layer mask.
        /// </summary>
        public static Vector3 MouseWorld(Camera camera, int layerMask) {
            return ScreenToWorld(Input.mousePosition, camera, layerMask);
        }

        /// <summary>
        /// Returns the world position of <paramref name="screenPoint"/>
        /// using <value>Camera.main</value> as the active camera.
        /// </summary>
        public static Vector3 ScreenToWorld(Vector3 screenPoint) {
            return ScreenToWorld(screenPoint, Camera.main);
        }

        /// <summary>
        /// Returns the world position of <paramref name="screenPoint"/>
        /// using <paramref name="camera"/> as the active camera.
        /// </summary>
        public static Vector3 ScreenToWorld(Vector3 screenPoint, Camera camera) {
            Ray ray = camera.ScreenPointToRay(screenPoint);
            return Physics.Raycast(ray, out RaycastHit info, 10000f) ? info.point : Vector3.zero;
        }

        /// <summary>
        /// Returns the world position of <paramref name="screenPoint"/> using <paramref name="camera"/>
        /// as the active camera, and <paramref name="layerMask"/> as a layer mask.
        /// </summary>
        public static Vector3 ScreenToWorld(Vector3 screenPoint, Camera camera, int layerMask) {
            Ray ray = camera.ScreenPointToRay(screenPoint);
            return Physics.Raycast(ray, out RaycastHit info, 10000f, layerMask) ? info.point : Vector3.zero;
        }

        /// <summary>
        /// Returns the world position in 2D of <value>Input.mousePosition</value> with y = 0,
        /// using <value>Camera.main</value> as the active camera.
        /// </summary>
        public static Vector3 MouseWorld2D_NoY() {
            Vector3 position = MouseWorld2D();
            position.y = 0;
            return position;
        }

        /// <summary>
        /// Returns the world position in 2D of <value>Input.mousePosition</value> with z = 0,
        /// using <value>Camera.main</value> as the active camera.
        /// </summary>
        public static Vector3 MouseWorld2D_NoZ() {
            Vector3 position = MouseWorld2D();
            position.z = 0;
            return position;
        }

        /// <summary>
        /// Returns the world position in 2D of <value>Input.mousePosition</value>
        /// using <value>Camera.main</value> as the active camera.
        /// </summary>
        public static Vector3 MouseWorld2D() {
            return ScreenToWorld2D(Input.mousePosition, Camera.main);
        }

        /// <summary>
        /// Returns the world position in 2D of <value>Input.mousePosition</value>
        /// using <paramref name="camera"/> as the active camera.
        /// </summary>
        public static Vector3 MouseWorld2D(Camera camera) {
            return ScreenToWorld2D(Input.mousePosition, camera);
        }

        /// <summary>
        /// Returns the world position in 2D of <paramref name="screenPoint"/>
        /// using <value>Camera.main</value> as the active camera.
        /// </summary>
        public static Vector3 ScreenToWorld2D(Vector3 screenPoint) {
            return ScreenToWorld2D(screenPoint, Camera.main);
        }

        /// <summary>
        /// Returns the world position in 2D of <paramref name="screenPoint"/>
        /// using <paramref name="camera"/> as the active camera.
        /// </summary>
        public static Vector3 ScreenToWorld2D(Vector3 screenPoint, Camera camera) {
            return camera.ScreenToWorldPoint(screenPoint);
        }

        /// <summary>
        /// Creates a 3D text (TextMeshPro) object using the specified parameters.
        /// </summary>
        /// <returns>The 3D text instance</returns>
        public static TextMeshPro CreateWorldText(string text, Transform parent = null, Vector3 position = default, Quaternion? rotation = null, int fontSize = 32,
                                                  Color? color = null,
                                                  HorizontalAlignmentOptions horizontalAlignment = HorizontalAlignmentOptions.Left,
                                                  VerticalAlignmentOptions verticalAlignment = VerticalAlignmentOptions.Top, int sortingOrder = 1000) {
            return CreateWorldText(text, parent, position, rotation ?? Quaternion.identity, fontSize, color ?? Color.white, horizontalAlignment, verticalAlignment, sortingOrder);
        }

        /// <summary>
        /// Creates a 3D text (TextMeshPro) object using the specified parameters.
        /// </summary>
        /// <returns>The 3D text instance</returns>
        public static TextMeshPro CreateWorldText(string text, Transform parent, Vector3 position, Quaternion rotation, int fontSize, Color color,
                                                  HorizontalAlignmentOptions horizontalAlignment, VerticalAlignmentOptions verticalAlignment, int sortingOrder) {
            TextMeshPro textMesh = new GameObject("WorldText", typeof(TextMeshPro)).GetComponent<TextMeshPro>();
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.horizontalAlignment = horizontalAlignment;
            textMesh.verticalAlignment = verticalAlignment;
            textMesh.sortingOrder = sortingOrder;

            textMesh.transform.SetParent(parent, false);
            textMesh.transform.localPosition = position;
            textMesh.transform.localRotation = rotation;

            return textMesh;
        }
    }
}