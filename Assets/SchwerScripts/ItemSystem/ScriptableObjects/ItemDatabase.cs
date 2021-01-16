using System.Collections.Generic;
using UnityEngine;

namespace Schwer.ItemSystem {
    public class ItemDatabase : ScriptableObject {
        // Generated via ItemDatabaseUtility
        [field:SerializeField] private List<Item> items;
        public void Initialise(List<Item> items) {
            this.items = items;
        }

        public Item GetItem(int itemID) {
            Item result = null;
            foreach (var item in items) {
                if (item == null) {
                    Debug.LogWarning("The database contains a null entry. Update the ItemDatabase to fix (refer to the readme for help).");
                    continue;
                }

                if (item.id == itemID) {
                    result = item;
                }
            }
            if (result == null) { Debug.LogWarning("Item with ID '" + itemID + "' was not found in the database."); }
            return result;
        }
    }
}
