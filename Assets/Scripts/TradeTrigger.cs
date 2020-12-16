using UnityEngine;

public class TradeTrigger : MonoBehaviour {
    private Player player;

    private void Update() {
        if (player != null && player.inputInteract) {
            UserInterfaceController.RequestTradeInterface();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var player = other.GetComponent<Player>();
        if (player != null) {
            this.player = player;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        var player = other.GetComponent<Player>();
        if (player != null) {
            this.player = null;
        }
    }
}
