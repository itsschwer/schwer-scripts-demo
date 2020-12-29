using UnityEngine;

namespace Schwer.ItemSystem {
    [CreateAssetMenu(menuName = "Scriptable Object/Item System/Inventory")]
    public class InventorySO : ScriptableObject {
        public Inventory value = new Inventory();

#if UNITY_EDITOR
        private void OnEnable() => value.OnContentsChanged += MarkDirtyIfChanged;
        private void OnDisable() => value.OnContentsChanged -= MarkDirtyIfChanged;

        private void MarkDirtyIfChanged(Item item, int count) {
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
