using UnityEngine;

namespace Schwer.ItemSystem {
    using Schwer.Database;

    public class ItemDatabase : ScriptableDatabase<Item> {
        // Generated via ItemDatabaseUtility
        [SerializeField] private Item[] items;

        public override void Initialise(Item[] items) {
            this.items = FilterByID(items);
        }

        public Item GetItem(int id) => GetFromID(id, items);
    }
}
