using System.Collections.Generic;

namespace Schwer.ItemSystem {
    public class Inventory {
        public Dictionary<Item, int> contents = new Dictionary<Item, int>();

        /// <summary>
        /// Changes the count of an item by `amount`. Accepts positive and negative values.
        /// Removes an item from the inventory if there is less than one of it.
        /// </summary>
        public void ChangeItemCount(Item item, int amount) {
            if (contents.ContainsKey(item)) {
                contents[item] += amount;

                if (contents[item] <= 0) {
                    contents.Remove(item);
                }
            }
            else if (amount > 0){
                contents[item] = amount;
            }
        }

        /// <summary>
        /// Directly sets the count of an item.
        /// </summary>
        public void SetItem(Item item, int count) => contents[item] = count;

        /// <summary>
        /// Removes an item from the inventory.
        /// <para>
        /// Returns `true` if the inventory has the item and successfully removes it. Otherwise returns `false`.
        /// </para>
        /// </summary>
        public bool RemoveItem(Item item) => contents.Remove(item);

        /// <summary>
        /// Checks if the inventory has a specified amount of an item.
        /// </summary>
        public bool CheckItem(Item item, int count) {
            if (contents.ContainsKey(item)) {
                return contents[item] >= count;
            }
            return false;
        }

        /// <summary>
        /// Returns this `Inventory` as a `SerializableInventory` for saving.
        /// <para>
        /// Depends on item ids so item ids should not be changed after build release.
        /// </para>
        /// </summary>
        public SerializableInventory Serialize() {
            var result = new SerializableInventory();
            foreach (var entry in contents) {
                if (entry.Value > 0) {
                    result.contents[entry.Key.id] = entry.Value;
                }
            }
            return result;
        }
    }

    [System.Serializable]
    public class SerializableInventory {
        /// <summary>
        /// Represents an inventory in the form &lt;item id, item count&gt;.
        /// </summary>
        public Dictionary<int, int> contents { get; private set; } = new Dictionary<int, int>();

        /// <summary>
        /// Returns a deserialized `Inventory` using an `ItemDatabase`.
        /// <para>
        /// Depends on item ids so item ids should not be changed after build release.
        /// </para>
        /// </summary>
        public Inventory Deserialize(ItemDatabase itemDatabase) {
            var result = new Inventory();
            foreach (var entry in contents) {
                var itemSO = itemDatabase.GetItem(entry.Key);
                if (itemSO != null) {
                    result.contents[itemSO] = entry.Value;
                }
            }
            return result;
        }
    }
}
