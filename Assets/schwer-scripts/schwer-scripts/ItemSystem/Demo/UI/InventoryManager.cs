using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Schwer.ItemSystem.Demo {
    public class InventoryManager : MonoBehaviour {
        [SerializeField] private InventorySO _inventory = default;
        private Inventory inventory => _inventory.value;

        [Header("Item Display components")]
        [SerializeField] private Text nameDisplay = default;
        [SerializeField] private Text descriptionDisplay = default;

        [Header("Overflow message")]
        [SerializeField] private Text overflowLog = default;
        [SerializeField] private float fadeDuration = 0.5f;
        private Coroutine runningCoroutine;

        private List<ItemSlot> itemSlots = new List<ItemSlot>();

        private void OnEnable() {
            inventory.OnContentsChanged += UpdateSlots;

            var selected = EventSystem.current.currentSelectedGameObject;
            if (selected == null || !selected.transform.IsChildOf(this.transform)) {
                EventSystem.current.SetSelectedGameObject(itemSlots[0].gameObject);
            }

            Initialise();
        }

        private void OnDisable() => inventory.OnContentsChanged -= UpdateSlots;

        private void Awake() {
            GetComponentsInChildren<ItemSlot>(itemSlots);

            foreach (var slot in itemSlots) {
                slot.manager = this;
            }
        }

        private void Initialise() {
            overflowLog.canvasRenderer.SetAlpha(0);
            UpdateDisplay(null);
            if (inventory != null) {
                UpdateSlots();
            }
        }

        public void UpdateSlots() => UpdateSlots(null, 0);
        private void UpdateSlots(Item item, int count) {
            var items = GetInventoryList();
            for (int i = 0; i < itemSlots.Count; i++) {
                if (i < items.Count) {
                    itemSlots[i].SetItem(items[i].Item1, items[i].Item2);
                }
                else {
                    itemSlots[i].Clear();
                }
            }

            var current = EventSystem.current.currentSelectedGameObject?.GetComponent<ItemSlot>();
            if (current != null) {
                UpdateDisplay(current.item);
            }

            if (items.Count > itemSlots.Count) {
                overflowLog.canvasRenderer.SetAlpha(1);
            }
            else if (overflowLog.canvasRenderer.GetAlpha() > 0) {
                HideOverflowMessage();
            }
        }

        private List<(Item, int)> GetInventoryList() {
            var list = new List<(Item, int)>();

            foreach (var entry in inventory) {
                if (entry.Key.stackable) {
                    list.Add((entry.Key, entry.Value));
                }
                else {
                    for (int i = 0; i < entry.Value; i++) {
                        list.Add((entry.Key, 1));
                    }
                }
            }

            return list;
        }

        public void UpdateDisplay(Item item) {
            if (item != null) {
                nameDisplay.text = item.name;
                descriptionDisplay.text = item.description;
            }
            else {
                nameDisplay.text = "";
                descriptionDisplay.text = "";
            }
        }

        private void HideOverflowMessage() {
            if (runningCoroutine != null) {
                StopCoroutine(runningCoroutine);
                runningCoroutine = null;
            }
            runningCoroutine = StartCoroutine(FadeTextCo());
        }

        private IEnumerator FadeTextCo() {
            for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime) {
                overflowLog.canvasRenderer.SetAlpha(1 - (t / fadeDuration));
                yield return null;
            }
            runningCoroutine = null;
        }
    }
}
