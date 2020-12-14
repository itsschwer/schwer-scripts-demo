using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace SchwerEditor.ItemSystem {
    using Schwer.ItemSystem;
    
    public class ItemAssetHandler {
        // Allows double-clicking on an Item asset to open the Item Editor window.
        [OnOpenAsset()]
        public static bool OpenEditor(int instanceID, int line) {
            var obj = EditorUtility.InstanceIDToObject(instanceID) as Item;
            if (obj != null) {
                ItemEditor.ShowWindow(obj);
                return true;
            }
            return false;
        }
    }

    [CustomEditor(typeof(Item))]
    public class ItemInspector : Editor {
        // Places a button at the top of the Inspector for an Item that opens the Item Editor window.
        public override void OnInspectorGUI() {
            if (GUILayout.Button("Open Item Editor")) {
                ItemEditor.ShowWindow((Item)target);
            }
            GUILayout.Space(5);
            base.OnInspectorGUI();
        }
    }

    public class ItemEditor : EditorWindow {
        [MenuItem("Item System/Open Item Editor")]
        public static void ShowWindow() => GetWindow<ItemEditor>("Item Editor");
        public static void ShowWindow(Item item) {
            var window = GetWindow<ItemEditor>("Item Editor");
            window.selectedItem = item;
        }

        private Item selectedItem;
        private void OnGUI() {
            Repaint();

            //! Should probably only run this line if an Item asset was created or deleted.
            var items = ScriptableObjectUtility.GetAllInstances<Item>().OrderBy(i => i.id).ToArray();
            
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
            selectedItem = DrawItemsSidebar(items);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            if (selectedItem != null) {
                if (selectedItem is Item) {
                    DrawItemProperties((Item)selectedItem);
                }
            }
            else {
                EditorGUILayout.LabelField("Select an item from the list.");
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Generate ItemDatabase")) {
                ItemDatabaseUtility.GenerateItemDatabase();
            }
        }

        private void DrawItemProperties(Item item) {
            var itemObj = new SerializedObject(item);

            EditorGUILayout.PropertyField(itemObj.FindProperty("_id"));
            EditorGUILayout.PropertyField(itemObj.FindProperty("_name"));
            EditorGUILayout.PropertyField(itemObj.FindProperty("_description"));
            EditorGUILayout.PropertyField(itemObj.FindProperty("_sprite"));
            EditorGUILayout.PropertyField(itemObj.FindProperty("_stackable"));

            itemObj.ApplyModifiedProperties();
        }

        private Item DrawItemsSidebar(Item[] items) {
            foreach (var item in items) {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(item.id.ToString(), GUILayout.MaxWidth(28));
                if (GUILayout.Button(item.name)) {
                    selectedItem = item;
                }
                EditorGUILayout.EndHorizontal();
            }
            return selectedItem;
        }
    }
}
