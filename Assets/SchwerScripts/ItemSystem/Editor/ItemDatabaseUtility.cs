using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SchwerEditor.ItemSystem {
    using Schwer.ItemSystem;
    
    [CustomEditor(typeof(ItemDatabase))]
    public class ItemDatabaseInspector : Editor {
        public override void OnInspectorGUI() {
            if (GUILayout.Button("Regenerate ItemDatabase")) {
                ItemDatabaseUtility.GenerateItemDatabase();
            }
            GUILayout.Space(5);

            var listProperty = new SerializedObject((ItemDatabase)target).FindProperty("items");
            if (listProperty.propertyType == SerializedPropertyType.Generic) {
                // Use Copy() to avoid unwanted iterating.
                var listCount = listProperty.Copy().arraySize;
                GUILayout.Label("Items (" + listCount + ")");

                foreach (SerializedProperty itemProperty in listProperty) {
                    if (itemProperty.propertyType == SerializedPropertyType.ObjectReference) {
                        using (new EditorGUI.DisabledScope(true)) {
                            EditorGUILayout.PropertyField(itemProperty, GUIContent.none);
                        }
                    }
                }
            }
        }
    }

    public static class ItemDatabaseUtility {
        [MenuItem("Item System/Generate ItemDatabase")]
        public static void GenerateItemDatabase() {
            var itemDB = GetItemDatabase();
            if (itemDB == null) { return; }

            itemDB.Initialise(GetAllItemAssets());
            EditorUtility.SetDirty(itemDB);

            AssetsUtility.SaveRefreshAndFocus();
            Selection.activeObject = itemDB;
        }

        private static ItemDatabase GetItemDatabase() {
            var databases = AssetsUtility.FindAllInstances<ItemDatabase>();

            ItemDatabase itemDB = null;
            if (databases.Length < 1) {
                Debug.Log("Creating a new ItemDatabase since none exist.");
                itemDB = ScriptableObjectUtility.CreateAsset<ItemDatabase>();
            }
            else if (databases.Length > 1) {
                Debug.LogError("Multiple ItemDatabases exist. Please delete the extra(s) and try again.");
            }
            else {
                itemDB = databases[0];
            }

            return itemDB;
        }

        private static List<Item> GetAllItemAssets() {
            var result = new List<Item>();

            var instances = AssetsUtility.FindAllInstances<Item>();
            var gatheredIDs = new List<int>();
            for (int i = 0; i < instances.Length; i++) {
                if (gatheredIDs.Contains(instances[i].id)) {
                    var sharedID = result[gatheredIDs.IndexOf(instances[i].id)].name;
                    Debug.LogWarning("'" + instances[i].name + "' was excluded from the ItemDatabase because it shares its ID (" + instances[i].id + ") with '" + sharedID + "'.");
                }
                else {
                    result.Add(instances[i]);
                    gatheredIDs.Add(instances[i].id);
                }
            }

            result = result.OrderBy(i => i.id).ToList();
            return result;
        }
    }
}
