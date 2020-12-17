using UnityEngine;

public class TradeTrigger : MonoBehaviour {
    private Animator animator;

    private Player player;
    private bool activationSent;

    private void Awake() => animator = GetComponent<Animator>();

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
            animator.SetBool("closed", false);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        var player = other.GetComponent<Player>();
        if (player != null) {
            this.player = null;
            animator.SetBool("closed", true);
        }
    }
}
