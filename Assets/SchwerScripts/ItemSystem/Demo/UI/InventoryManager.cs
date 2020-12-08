using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Schwer.ItemSystem.Demo
{
    public class InventoryManager : MonoBehaviour {
        [SerializeField] private InventorySO _inventory = default;
        private Inventory inventory => _inventory.value;

        private List<ItemSlot> itemSlots = new List<ItemSlot>();

        private void OnEnable() => inventory.OnContentsChanged += UpdateSlots;
        private void OnDisable() => inventory.OnContentsChanged -= UpdateSlots;

        private void Awake() {
            GetComponentsInChildren<ItemSlot>(itemSlots);

            if (inventory != null) {
                UpdateSlots();
            }
        }

        private void UpdateSlots() => UpdateSlots(null, 0);
        private void UpdateSlots(Item item, int count) {
            for (int i = 0; i < itemSlots.Count; i++) {
                if (i < inventory.Count) {
                    var entry = inventory.ElementAt(i);
                    itemSlots[i].SetItem(entry.Key, entry.Value);
                }
                else {
                    itemSlots[i].Clear();
                }
            }
        }
    }
}
