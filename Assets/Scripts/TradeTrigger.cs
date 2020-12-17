using UnityEngine;

public class TradeTrigger : MonoBehaviour {
    private Player player;
    private bool activationSent;

    private void Update() {
        if (Time.timeScale > 0) {
            if (player != null && player.inputInteract && !activationSent) {
                UserInterfaceController.RequestTradeInterface();
                activationSent = true;
            }
            else {
                activationSent = false;
            }
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
