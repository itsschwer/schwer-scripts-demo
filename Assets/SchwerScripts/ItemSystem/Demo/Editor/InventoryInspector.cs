using UnityEditor;
using UnityEngine;

namespace SchwerEditor.ItemSystem {
    using Schwer.ItemSystem;
    using Schwer.ItemSystem.Demo;

    [CustomEditor(typeof(InventorySO))]
    public class InventoryDemo : Editor {
        private InventoryManager manager;

        private Item item;
        private int amount = 1;

        public override void OnInspectorGUI() {
            var inventory = (InventorySO)target;

            GUILayout.Space(5);
            DrawDemoControls(inventory);
        }

        private void DrawDemoControls(InventorySO inventory) {
            EditorGUILayout.LabelField("Demo Controls", EditorStyles.boldLabel);
            manager = (InventoryManager)EditorGUILayout.ObjectField(manager, typeof(InventoryManager), true);

            if (GUILayout.Button("Clear Inventory")) {
                inventory.value = new Inventory();
                Debug.Log("Cleared '" + inventory.name + "'.");
                manager?.UpdateSlots();
            }

            DrawItemControls(inventory);
        }

        private void DrawItemControls(InventorySO inventory) {
            EditorGUILayout.BeginVertical("box");

            item = (Item)EditorGUILayout.ObjectField("Item", item, typeof(Item), false);
            amount = EditorGUILayout.IntField("Amount", amount);

            if (GUILayout.Button("Check")) {
                if (item != null) {
                    if (inventory.value.CheckItem(item, amount)) {
                        Debug.Log("'" + inventory.name + "' has " + amount + " or more of '" + item.name +"'.");
                    }
                    else {
                        Debug.Log("'" + inventory.name + "' has less than " + amount + " of '" + item.name +"'.");
                    }
                }
            }
            if (GUILayout.Button("Set")) {
                if (item != null) {
                    inventory.value.SetItem(item, amount);
                    Debug.Log("Set '" + inventory.name + "' '" + item.name + "' count to " + amount + ".");
                    manager?.UpdateSlots();
                }
            }
            if (GUILayout.Button("Add")) {
                if (item != null) {
                    inventory.value.ChangeItemCount(item, amount);
                    Debug.Log("Added " + amount + "x '" + item.name + "' to '" + inventory.name + "'.");
                    manager?.UpdateSlots();
                }
            }
            if (GUILayout.Button("Remove")) {
                if (item != null) {
                    inventory.value.ChangeItemCount(item, -amount);
                    Debug.Log("Removed " + amount + "x '" + item.name + "' from '" + inventory.name + "'.");
                    manager?.UpdateSlots();
                }
            }
            if (GUILayout.Button("Clear")) {
                if (item != null) {
                    if (inventory.value.RemoveItem(item)) {
                        Debug.Log("Cleared '" + item.name + "' from '" + inventory.name + "'.");
                        manager?.UpdateSlots();
                    }
                    else {
                        Debug.Log("'" + inventory.name + "' does not have any of '" + item.name + "'.");
                    }
                }
            }

            EditorGUILayout.EndVertical();
        }
    }   
}
