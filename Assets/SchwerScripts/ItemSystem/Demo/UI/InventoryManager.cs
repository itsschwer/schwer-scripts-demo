using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Schwer.ItemSystem.Demo
{
    public class InventoryManager : MonoBehaviour {
        [SerializeField] private InventorySO inventory = default;
        private List<ItemSlot> itemSlots = new List<ItemSlot>();

        private void Awake() {
            GetComponentsInChildren<ItemSlot>(itemSlots);
            UpdateSlots();
        }

        public void UpdateSlots() {
            for (int i = 0; i < itemSlots.Count; i++) {
                if (i < inventory.value.contents.Count) {
                    var entry = inventory.value.contents.ElementAt(i);
                    itemSlots[i].SetItem(entry.Key, entry.Value);
                }
                else {
                    itemSlots[i].Clear();
                }
            }
        }
    }
}
