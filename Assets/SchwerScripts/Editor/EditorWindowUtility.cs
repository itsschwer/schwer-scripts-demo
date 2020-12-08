using UnityEditor;
using UnityEngine;

namespace SchwerEditor {
    public static class EditorWindowUtility {
        // Adapted from: GameDevGuide's "Easy Editor Windows in Unity with Serialized Properties"
        // https://www.youtube.com/watch?v=c_3DXBrH-Is

        /// <summary>
        /// Draws properties as the default Inspector would.
        /// </summary>
        public static void DrawProperties(SerializedProperty property, bool drawChildren) {
            var lastPropertyPath = string.Empty;

            foreach (SerializedProperty p in property) {
                if (p.isArray && p.propertyType == SerializedPropertyType.Generic) {
                    EditorGUILayout.BeginHorizontal();
                    p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                    EditorGUILayout.EndHorizontal();

                    if (p.isExpanded) {
                        EditorGUI.indentLevel++;
                        DrawProperties(p, drawChildren);
                        EditorGUI.indentLevel--;
                    }
                }
                else {
                    // Prevent child properties from being drawn over each iteration.
                    if (!string.IsNullOrEmpty(lastPropertyPath) && p.propertyPath.Contains(lastPropertyPath)) {
                        continue;
                    }

                    lastPropertyPath = p.propertyPath;
                    EditorGUILayout.PropertyField(p, drawChildren);
                }
            }
        }

        /// <summary>
        /// Draws a sidebar populated with the elements of a specified property.
        /// <para>
        /// `selectedPropertyPath` should be a class level variable so that the selection persists.
        /// </para>
        /// </summary>
        public static SerializedProperty DrawSidebar(SerializedProperty property, SerializedObject obj, ref string selectedPropertyPath) {
            foreach (SerializedProperty p in property) {
                if (GUILayout.Button(GetObjectNameOrPropertyDisplay(p))) {
                    selectedPropertyPath = p.propertyPath;
                }
            }

            if (!string.IsNullOrEmpty(selectedPropertyPath)) {
                return obj.FindProperty(selectedPropertyPath);
            }
            return null;
        }

        /// <summary>
        /// Draws a sidebar populated with the specified objects.
        /// <para>
        /// `selectedObject` should be a class level variable so that the selection persists.
        /// </para>
        /// </summary>
        public static void DrawSidebar(Object[] objects, ref Object selectedObject) {
            foreach (var obj in objects) {
                if (GUILayout.Button(obj.name)) {
                    selectedObject = obj;
                }
            }
        }
        
        public static string GetObjectNameOrPropertyDisplay(SerializedProperty property) => (property.propertyType == SerializedPropertyType.ObjectReference) ? property.objectReferenceValue.name : property.displayName;
    }
}
