using TMPro;
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

        public static TextMeshPro CreateWorldText(string text, Transform parent = null, Vector3 position = default, Quaternion? rotation = null, int fontSize = 32, Color? color = null,
                                                  HorizontalAlignmentOptions horizontalAlignment = HorizontalAlignmentOptions.Left,
                                                  VerticalAlignmentOptions verticalAlignment = VerticalAlignmentOptions.Top, int sortingOrder = 1000) {
            return CreateWorldText(text, parent, position, rotation ?? Quaternion.identity, fontSize, color ?? Color.white, horizontalAlignment, verticalAlignment, sortingOrder);
        }


        public static TextMeshPro CreateWorldText(string text, Transform parent, Vector3 position, Quaternion rotation, int fontSize, Color color, HorizontalAlignmentOptions horizontalAlignment, VerticalAlignmentOptions verticalAlignment, int sortingOrder) {
            var textMesh = new GameObject("WorldText", typeof(TextMeshPro)).GetComponent<TextMeshPro>();
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