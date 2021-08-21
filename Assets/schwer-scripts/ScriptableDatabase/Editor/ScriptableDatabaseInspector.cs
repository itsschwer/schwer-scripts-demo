using UnityEditor;
using UnityEngine;

namespace SchwerEditor.Database {
    using Schwer.Database;

    // [CustomEditor(typeof(TDatabase))]
    /// <summary>
    /// A custom Inspector for displaying the properties of a <c cref="ScriptableDatabase">ScriptableDatabase</c> safely (prevents reassigning elements via the Inspector).
    /// </summary>
    public class ScriptableDatabaseInspector<TDatabase, TElement> : Editor
        where TDatabase : ScriptableDatabase<TElement>
        where TElement : ScriptableObject {

        public override void OnInspectorGUI() {
            if (GUILayout.Button($"Regenerate {typeof(TDatabase).Name}")) {
                ScriptableDatabaseUtility.GenerateDatabase<TDatabase, TElement>();
            }
            GUILayout.Space(5);

            var obj = new SerializedObject((TDatabase)target);
            var arrayProperty = obj.GetIterator();

            // `arrayProperty`: `Base`(?) to `Script`
            arrayProperty.NextVisible(true);
            var scriptProperty = arrayProperty.Copy();
            // `arrayProperty`: `Script` to array – relies on the first serializable property being an array (or list)
            arrayProperty.NextVisible(true);

            using (new EditorGUI.DisabledScope(true)) {
                EditorGUILayout.PropertyField(scriptProperty);
            }

            // Draw database array
            if (arrayProperty.isArray && arrayProperty.propertyType != SerializedPropertyType.String) {
                var size = arrayProperty.arraySize;
                var name = arrayProperty.displayName;
                GUILayout.Label($"{name} ({size})");

                foreach (SerializedProperty elementProperty in arrayProperty) {
                    if (elementProperty.propertyType == SerializedPropertyType.ObjectReference) {
                        using (new EditorGUI.DisabledScope(true)) {
                            EditorGUILayout.PropertyField(elementProperty, GUIContent.none);
                        }
                    }
                }
            }
            else {
                EditorGUILayout.HelpBox($"Expected first serializable property in `{typeof(TDatabase).Name}` to be an array.", MessageType.Error);
                return;
            }

            GUILayout.Space(5);
            DrawPropertiesExcluding(obj, new string[] {arrayProperty.name, scriptProperty.name});
        }
    }
}
