using UnityEngine;
using UnityEngine.UI;

namespace Schwer.ItemSystem.Demo
{
    public class ItemSlot : MonoBehaviour {
        public Item item { get; private set; }
        [Header("Components")]
        [SerializeField] private Image slot = default;
        [SerializeField] private Image sprite = default;
        [SerializeField] private Text count = default;

        public void SetItem(Item item, int itemCount) {
            this.item = item;
            if (item != null) {
                sprite.sprite = item.sprite;
                sprite.enabled = true;
                count.text = "x" + itemCount;
            }
            else {
                Clear();
            }
        }

        public void Clear() {
            item = null;
            sprite.enabled = false;
            sprite.sprite = null;
            count.text = "";
        }
    }
}
