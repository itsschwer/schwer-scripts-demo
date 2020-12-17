using UnityEditor;
using UnityEngine;

namespace SchwerEditor.ItemSystem {
    using Schwer.ItemSystem;
    
    [CustomPropertyDrawer(typeof(Inventory))]
    public class InventoryDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => -4;
        // ^ Reference: https://forum.unity.com/threads/accumulating-empty-space-at-the-top-of-an-array-containing-custompropertydrawer-items.509133/
        // Otherwise there would be an excessive gap between the script field and where the drawer began.

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var keys = property.FindPropertyRelative("keys");
            var values = property.FindPropertyRelative("values");

            if (keys.arraySize != values.arraySize) {
                var warning = "The number of keys does not match the number of values!";
                var details = $"({keys.arraySize} keys; {values.arraySize} values)";
                EditorGUILayout.HelpBox(warning + "\n" + details, MessageType.Warning);
            }

            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, "Contents (" + keys.arraySize + ")", true);

            if (property.isExpanded) {
                EditorGUI.BeginDisabledGroup(true);
                for (int i = 0; i < keys.arraySize; i++) {
                    EditorGUILayout.BeginHorizontal();
                    var key = keys.GetArrayElementAtIndex(i);
                    var value = values.GetArrayElementAtIndex(i);
                    EditorGUILayout.PropertyField(key, GUIContent.none);
                    EditorGUILayout.PropertyField(value, GUIContent.none);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.EndDisabledGroup();
            }
        }
    }
}
