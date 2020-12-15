﻿using System.Linq;
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
                EditorGUILayout.HelpBox("Select an item from the sidebar to begin editing", MessageType.Info);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Generate ItemDatabase")) {
                ItemDatabaseUtility.GenerateItemDatabase();
            }
        }

        private void DrawItemProperties(Item item) {
            DrawDisabledItemField(item);

            var itemObj = new SerializedObject(item);

            EditorGUILayout.PropertyField(itemObj.FindProperty("_id"));
            EditorGUILayout.PropertyField(itemObj.FindProperty("_name"));
            EditorGUILayout.PropertyField(itemObj.FindProperty("_description"));
            EditorGUILayout.PropertyField(itemObj.FindProperty("_sprite"));
            EditorGUILayout.PropertyField(itemObj.FindProperty("_stackable"));

            itemObj.ApplyModifiedProperties();
        }

        private void DrawDisabledItemField(Item item) {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField(item, typeof(Item), false);
            EditorGUI.EndDisabledGroup();
        }

        private Item DrawItemsSidebar(Item[] items) {
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
            return selectedItem;
        }
    }
}
