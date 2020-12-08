using UnityEngine;

namespace Schwer.ItemSystem {
    [CreateAssetMenu(menuName = "Scriptable Object/Item System/Inventory")]
    public class InventorySO : ScriptableObject {
        public Inventory value = new Inventory();
    }
}
