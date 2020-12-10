using Schwer.ItemSystem;
using UnityEngine;

public class Player : Character {
    [SerializeField] private InventorySO _inventory = default;
    public Inventory inventory => _inventory.value;

    protected override void GetInput() {
        if (Time.timeScale <= 0) return;
        input.direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
