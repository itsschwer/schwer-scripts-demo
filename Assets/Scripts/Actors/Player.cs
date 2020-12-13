using Schwer.ItemSystem;
using UnityEngine;

public class Player : Character {
    [SerializeField] private InventorySO _inventory = default;
    public Inventory inventory => _inventory.value;

    private void OnEnable() => SceneTransition.OnNewSceneOrient += Orient;
    private void OnDisable() => SceneTransition.OnNewSceneOrient -= Orient;

    private void Orient(Vector2 position, Vector2 direction) {
        transform.position = position;
        input.direction = direction;
    }

    protected override void GetInput() {
        if (Time.timeScale <= 0) return;
        input.direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
