using System.Linq;
using System.Collections.Generic;
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
        private Item selectedItem;
        private Vector2 sidebarScroll;
        private Vector2 selectedItemScroll;

        [MenuItem("Item System/Open Item Editor")]
        public static void ShowWindow() => GetWindow<ItemEditor>("Item Editor");
        public static void ShowWindow(Item item) {
            var window = GetWindow<ItemEditor>("Item Editor");
            window.selectedItem = item;
        }

        private void OnGUI() {
            Repaint();

            //! Should probably only run this line if an Item asset was created or deleted.
            var items = ScriptableObjectUtility.GetAllInstances<Item>().OrderBy(i => i.id).ToArray();
            
            EditorGUILayout.BeginHorizontal();
            selectedItem = DrawItemsSidebar(items);
            DrawSelectedItem();
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Generate ItemDatabase")) {
                ItemDatabaseUtility.GenerateItemDatabase();
            }
        }

        private Item DrawItemsSidebar(Item[] items) {
            var maxWidth = 150;
            var scrollBarWidth = 30;
            
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(maxWidth + scrollBarWidth), GUILayout.ExpandHeight(true));
            sidebarScroll = EditorGUILayout.BeginScrollView(sidebarScroll, GUILayout.MinWidth(maxWidth + 1), GUILayout.MaxWidth(maxWidth + scrollBarWidth), GUILayout.ExpandWidth(true));   // Need to use maxWidth + 1 to avoid horizontal scroll bar

            var button = new GUIStyle(GUI.skin.button);
            button.alignment = TextAnchor.MiddleLeft;

            var regCol = GUI.backgroundColor;
            var selCol = new Color(0.239f, 0.501f, 0.874f);
            var selTxt = Color.white;
            var dupCol = new Color(0.866f, 0.258f, 0.250f);
            var dupTxt = dupCol;

            var ids = new HashSet<int>();
            foreach (var item in items) {
                var selected = (selectedItem == item);
                var dupeID = (!ids.Add(item.id));

                var label = new GUIStyle(GUI.skin.label);

                if (selected || dupeID) {
                    label.normal.textColor = selTxt;
                    if (selected) {
                        GUI.backgroundColor = selCol;
                        if (dupeID) {
                            label.normal.textColor = dupTxt;
                        }
                    }
                    else {
                        GUI.backgroundColor = dupCol;
                    }
                }

                EditorGUILayout.BeginHorizontal("box");
                GUI.backgroundColor = regCol;

                GUILayout.Label(item.id.ToString(), label, GUILayout.MinWidth(28), GUILayout.ExpandWidth(false));
                if (GUILayout.Button(item.name, button)) {
                    selectedItem = item;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            
            return selectedItem;
        }

        private void DrawSelectedItem() {
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            selectedItemScroll = EditorGUILayout.BeginScrollView(selectedItemScroll);

            if (selectedItem != null) {
                if (selectedItem is Item) {
                    DrawItemProperties((Item)selectedItem);
                }
            }
            else {
                EditorGUILayout.HelpBox("Select an item from the sidebar to begin editing.", MessageType.Info);
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DrawItemProperties(Item item) {
            DrawDisabledItemField(item);

            // Reference: https://forum.unity.com/threads/odd-serialization-behaviour-of-unityevent-inside-an-editor-window.505653/
            // ^ Not sure if there are any issues/quirks with using `DrawDefaultInspector`
            var editor = Editor.CreateEditor(item);
            editor.DrawDefaultInspector();
        }

        private void DrawDisabledItemField(Item item) {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField(item, typeof(Item), false);
            EditorGUI.EndDisabledGroup();
        }
    }
}
