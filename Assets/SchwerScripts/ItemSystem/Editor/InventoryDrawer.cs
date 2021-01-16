using UnityEditor;
using UnityEngine;

namespace SchwerEditor.ItemSystem {
    using Schwer.ItemSystem;

    [CustomPropertyDrawer(typeof(Inventory))]
    public class InventoryDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var keys = property.FindPropertyRelative("keys");
            var values = property.FindPropertyRelative("values");

            if (keys.arraySize != values.arraySize) {
                var warning = "The number of keys does not match the number of values!";
                var details = $"({keys.arraySize} keys; {values.arraySize} values)";
                EditorGUILayout.HelpBox(warning + "\n" + details, MessageType.Warning);
                //! ^ Need to convert from `EditorGUILayout` to `EditorGUI`
            }

            var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, "Contents (" + keys.arraySize + ")", true);

            if (property.isExpanded) {
                EditorGUI.BeginDisabledGroup(true);

                var kvpHeight = EditorGUIUtility.singleLineHeight;
                var kvpSpacing = EditorGUIUtility.standardVerticalSpacing;
                var halfWidth = position.width / 2;
                for (int i = 0; i < keys.arraySize; i++) {
                    var posY = position.y + ((i + 1) * (kvpHeight + kvpSpacing));
                    var keyRect = new Rect(position.x, posY, halfWidth, kvpHeight);
                    var valueRect = new Rect(position.x + halfWidth, posY, halfWidth, kvpHeight);
                    var key = keys.GetArrayElementAtIndex(i);
                    var value = values.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(keyRect, key, GUIContent.none);
                    EditorGUI.PropertyField(valueRect, value, GUIContent.none);
                    GUILayout.Space(kvpHeight + kvpSpacing);
                }
                GUILayout.Space(kvpSpacing);

                EditorGUI.EndDisabledGroup();
            }
        }
    }
}
