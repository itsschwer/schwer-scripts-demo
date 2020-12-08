using UnityEngine;

public class Player : Character {
    protected override void GetInput() {
        input.direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
