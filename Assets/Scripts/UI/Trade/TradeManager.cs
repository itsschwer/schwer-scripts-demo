using System.Collections.Generic;
using System.Linq;
using Schwer.ItemSystem;
using Schwer.ItemSystem.Demo;
using UnityEngine;
using UnityEngine.EventSystems;

public class TradeManager : MonoBehaviour {
    [SerializeField] private InventorySO _inventory = default;
    private Inventory inventory => _inventory.value;

    [SerializeField] private Offers[] offers = default;

    private List<ItemSlot> itemSlots = new List<ItemSlot>();
    private List<OfferSlot> offerSlots = new List<OfferSlot>();

    private void OnEnable() {
        inventory.OnContentsChanged += UpdateSlots;
        if (inventory != null) {
            UpdateSlots();
        }

        var selected = EventSystem.current.currentSelectedGameObject;
        if (selected == null || !selected.transform.IsChildOf(this.transform)) {
            EventSystem.current.SetSelectedGameObject(offerSlots[0].gameObject);
        }
    }

    private void OnDisable() => inventory.OnContentsChanged -= UpdateSlots;

    private void Awake() {
        GetComponentsInChildren<ItemSlot>(itemSlots);
        for (int i = itemSlots.Count - 1; i > 0; i--) {
            if (itemSlots[i].GetComponentInParent<OfferSlot>()) {
                itemSlots.RemoveAt(i);
            }
        }

        GetComponentsInChildren<OfferSlot>(offerSlots);
        for (int i = 0; i < offerSlots.Count; i++) {
            offerSlots[i].manager = this;
            offerSlots[i].SetOfferItems(offers[i].offer);
        }
    }

    public void UpdateSlots() => UpdateSlots(null, 0);
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

    public void TryTrade(OfferSlot offerSlot) {
        if (offerSlot.CheckInventory(inventory)) {
            for (int i = 0; i < offerSlot.inputItems.Length; i++) {
                if (offerSlot.inputItems[i].item != null) {
                    inventory[offerSlot.inputItems[i].item] -= offerSlot.inputItems[i].count;
                }
            }

            for (int i = 0; i < offerSlot.outputItems.Length; i++) {
                if (offerSlot.outputItems[i].item != null) {
                    inventory[offerSlot.outputItems[i].item] += offerSlot.outputItems[i].count;
                }
            }
        }
    }

    [System.Serializable]
    public struct Offers {
        // Hard-coded example: expects 4 OfferItems
        public OfferSlot.OfferItem[] offer;
    }
}
