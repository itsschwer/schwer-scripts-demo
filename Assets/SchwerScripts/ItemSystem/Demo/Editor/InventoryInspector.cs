using UnityEditor;
using UnityEngine;

namespace SchwerEditor.ItemSystem {
    using Schwer.ItemSystem;

    [CustomEditor(typeof(InventorySO))]
    public class InventoryDemo : Editor {
        private static Item item;
        private static int amount = 1;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            GUILayout.Space(5);
            
            var inventory = (InventorySO)target;
            DrawDemoControls(inventory);
        }

        private void DrawDemoControls(InventorySO inventory) {
            EditorGUILayout.LabelField("Demo Controls", EditorStyles.boldLabel);

            if (GUILayout.Button("Clear Inventory")) {
                inventory.value.Clear();
                Debug.Log("Cleared '" + inventory.name + "'.");
            }

            DrawItemControls(inventory);
        }

        private void DrawItemControls(InventorySO inventory) {
            EditorGUILayout.BeginVertical("box");

            item = (Item)EditorGUILayout.ObjectField("Item", item, typeof(Item), false);
            amount = EditorGUILayout.IntField("Amount", amount);

            var invName = "'" + inventory.name + "'";
            var itemName = "'" + ((item != null) ? item.name : "(Item)") + "'";

            EditorGUI.BeginDisabledGroup(item == null);
            if (GUILayout.Button("Check for " + itemName)) {
                Debug.Log(invName + " has " + inventory.value[item] + "x of " + itemName + ".");
            }
            if (GUILayout.Button("Check " + itemName + " >= " + amount)) {
                if (inventory.value[item] >= amount) {
                    Debug.Log(invName + " has " + amount + " or more of " + itemName + ".");
                }
                else {
                    Debug.Log("" + invName + " has less than " + amount + " of " + itemName + ".");
                }
            }
            if (GUILayout.Button("Set " + itemName + " to " + amount + "x")) {
                inventory.value[item] = amount;
                Debug.Log("Set " + invName + " " + itemName + " to " + amount + "x.");
            }
            if (GUILayout.Button("Add " + amount + "x " + itemName)) {
                inventory.value[item] += amount;
                Debug.Log("Added " + amount + "x " + itemName + " to " + invName + ".");
            }
            if (GUILayout.Button("Subtract " + amount + "x " + itemName)) {
                inventory.value[item] -= amount;
                Debug.Log("Removed " + amount + "x " + itemName + " from " + invName + ".");
            }
            if (GUILayout.Button("Remove all of " + itemName)) {
                if (inventory.value.Remove(item)) {
                    Debug.Log("Removed all of " + itemName + " from " + invName + ".");
                }
                else {
                    Debug.Log(invName + " does not have any of " + itemName + " to remove.");
                }
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();
        }
    }   
}
