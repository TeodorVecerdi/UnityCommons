using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace UnityCommons.Editor.Hierarchy {
    [InitializeOnLoad]
    internal static class HierarchyDrawer {
        static HierarchyDrawer() {
            Initialize();
        }

        private static bool initialized = false;

        private static List<Type> singletonTypes;
        private static readonly Type singletonTypeDefinition = typeof(MonoSingleton<>);
        private static readonly Dictionary<int, GameObject> instanceIdDictionary = new Dictionary<int, GameObject>();
        private static readonly Dictionary<int, DuplicateObject> duplicateObjects = new Dictionary<int, DuplicateObject>();

        private static int firstDrawnItem = -1;
        private static bool drawParents;
        private static readonly HashSet<int> drawnItems = new HashSet<int>();
        private static readonly HashSet<int> oldDrawnItems = new HashSet<int>();

        private static Texture warningTexture;
        private static Texture2D lightGrayTexture;
        private static GUIStyle warningBackgroundStyle;
        private static GUIStyle buttonBackgroundStyle;

        private static void Initialize() {
            if (initialized) {
                EditorApplication.hierarchyWindowItemOnGUI -= Draw;
                EditorApplication.hierarchyChanged -= RetrieveData;
            }

            initialized = true;
            EditorApplication.hierarchyWindowItemOnGUI += Draw;
            EditorApplication.hierarchyChanged += RetrieveData;

            warningTexture = Resources.Load<Texture>("warning");
            
            var lightGray = new Color(0.63f, 0.63f, 0.63f);
            lightGrayTexture = new Texture2D(2,2);
            lightGrayTexture.SetPixels(new [] {lightGray, lightGray, lightGray, lightGray});
            lightGrayTexture.Apply();
            
            RetrieveData();
        }

        private static void Draw(int instanceId, Rect selectionRect) {
            if (firstDrawnItem == -1) {
                firstDrawnItem = instanceId;
            } else if (firstDrawnItem == instanceId) {
                oldDrawnItems.Clear();
                oldDrawnItems.UnionWith(drawnItems);
                ClearDrawnItems();
                drawParents = true;
            }

            if (duplicateObjects != null && duplicateObjects.ContainsKey(instanceId)) {
                drawnItems.Add(instanceId);
                DrawDuplicate(instanceId, selectionRect);
            } else if (ContainsDuplicateSingleton(instanceId, out var singletonType, out var childInstanceId) && !oldDrawnItems.Contains(childInstanceId) && drawParents) {
                buttonBackgroundStyle ??= new GUIStyle(EditorStyles.toolbar)
                    {fixedHeight = 0, normal = {background = Texture2D.grayTexture}, onHover = {background = lightGrayTexture}, hover = {background = lightGrayTexture}, active = {background = Texture2D.whiteTexture}};

                var oldBackgroundColor = GUI.backgroundColor;
                GUI.backgroundColor = new Color(1, 1, 1, 0.25f);
                if (GUI.Button(new Rect(selectionRect.xMax - 4, selectionRect.yMin, 16, 16), "", buttonBackgroundStyle)) {
                    Selection.activeInstanceID = childInstanceId;
                    EditorGUIUtility.PingObject(childInstanceId);
                }
                GUI.backgroundColor = oldBackgroundColor;
                GUI.Box(new Rect(selectionRect.xMax - 2, selectionRect.yMin + 2, 12, 12),
                               new GUIContent(warningTexture,
                                              $"Found duplicate instance of {singletonType.Name} in one of this GameObject's children. Duplicates will automatically be deleted when playing!"),
                               GUIStyle.none);
            }
        }

        private static void DrawDuplicate(int instanceId, Rect selectionRect) {
            var obj = duplicateObjects[instanceId];
            if (obj.GameObject == null || !obj.GameObject.activeInHierarchy) return;

            warningBackgroundStyle ??= new GUIStyle(EditorStyles.toolbar) {fixedHeight = 0, normal = {background = Texture2D.whiteTexture}};

            var depth = obj.Depth;
            var totalRect = new Rect(selectionRect.xMin - depth * 16 + 2 * depth, selectionRect.yMin, selectionRect.width + depth * 16 - 2 * depth + 16, 16);

            var oldBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(1, 0, 0, 0.125f);
            GUI.Box(totalRect, string.Empty, warningBackgroundStyle);
            GUI.backgroundColor = oldBackgroundColor;

            GUI.Box(new Rect(totalRect.xMax - 16 - 2, totalRect.yMin + 2, 12, 12),
                    new GUIContent(warningTexture, $"Found multiple instances of {obj.SingletonType} in the scene. Duplicates will automatically be deleted when playing!"),
                    GUIStyle.none);
        }

        private static void RetrieveData() {
            if (Application.isPlaying) return;
            ClearDrawnItems();
            drawParents = false;
            duplicateObjects.Clear();
            instanceIdDictionary.Clear();

            if (singletonTypes == null) OnScriptsReloaded();
            if (singletonTypes == null) throw new Exception();

            foreach (var gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject))) {
                instanceIdDictionary[gameObject.GetInstanceID()] = gameObject as GameObject;
            }

            foreach (var type in singletonTypes) {
                var tempObj = Resources.FindObjectsOfTypeAll(type);
                var objects = tempObj.Cast<Component>().Select(component => component.gameObject);
                var isDuplicate = tempObj.Length > 1;
                foreach (var gameObject in objects) {
                    var instanceId = gameObject.GetInstanceID();

                    if (!isDuplicate) {
                        duplicateObjects.Remove(instanceId);
                        continue;
                    }

                    duplicateObjects[instanceId] = new DuplicateObject(gameObject, type);
                }
            }
        }

        private static bool ContainsDuplicateSingleton(int instanceId, out Type singletonType, out int childInstanceId) {
            singletonType = null;
            childInstanceId = -1;
            if (!instanceIdDictionary.ContainsKey(instanceId)) {
                return false;
            }

            var gameObject = instanceIdDictionary[instanceId];
            if (gameObject == null) return false;

            var transform = gameObject.transform;
            // if (transform.parent != null) return false;

            var (singleton, instId) = FindDuplicateSingleton(transform);
            childInstanceId = instId;
            singletonType = singleton;
            return singleton != null;
        }

        private static (Type, int) FindDuplicateSingleton(Transform transform) {
            foreach (var type in singletonTypes) {
                var comp = transform.GetComponentInChildren(type);
                if (comp == null) continue;

                var instId = comp.gameObject.GetInstanceID();
                if (duplicateObjects.ContainsKey(instId)) {
                    return (type, instId);
                }
            }

            return (null, -1);
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {
            var allTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies() from type in assembly.GetTypes() select type).ToList();

            // Load types that inherit from MonoSingleton
            singletonTypes = new List<Type>(
                from type in allTypes
                where type.BaseType is {IsGenericType: true} && type.BaseType.GetGenericTypeDefinition() == singletonTypeDefinition
                select type
            );

            // Load types that inherit from a type inheriting from MonoSingleton
            singletonTypes.AddRange(
                from singletonType in singletonTypes
                where !singletonType.IsSealed
                from type in allTypes
                where type.IsSubclassOf(singletonType)
                select type
            );

            // Remove types that are not a valid MonoBehaviour type
            singletonTypes.RemoveAll(type => type.IsAbstract || type.IsGenericType);
        }

        private static void ClearDrawnItems() {
            drawnItems.Clear();
            firstDrawnItem = -1;
        }

        private class DuplicateObject {
            public readonly GameObject GameObject;
            public readonly Type SingletonType;
            public readonly int Depth;

            public DuplicateObject(GameObject gameObject, Type singletonType) {
                GameObject = gameObject;
                SingletonType = singletonType;
                Depth = GetObjectDepth(gameObject.transform);
            }

            private static int GetObjectDepth(Transform transform) {
                var depth = 1;
                var parent = transform.transform;
                while (parent != null) {
                    depth++;
                    parent = parent.parent;
                }

                return depth;
            }
        }
    }
}