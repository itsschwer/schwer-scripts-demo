using Schwer.ItemSystem;
using SchwerEditor;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Inventory))]
public class InventoryDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        // EditorGUI.BeginProperty(position, label, property);

        var keys = property.FindPropertyRelative("keys");
        var values = property.FindPropertyRelative("values");

        EditorGUILayout.BeginHorizontal();
        property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, "Contents (" + keys.arraySize + ")");
        EditorGUILayout.EndHorizontal();

        if (property.isExpanded) {
            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(true);
            for (int i = 0; i < keys.arraySize; i++) {
                EditorGUILayout.BeginHorizontal();
                var key = keys.GetArrayElementAtIndex(i);
                var value = values.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(key);
                EditorGUILayout.PropertyField(value);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel--;
        }
    }
}
