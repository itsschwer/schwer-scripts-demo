using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserInterfaceController : MonoBehaviour {
    [SerializeField] private Canvas userInterface = default;
    private float timeScale;

    private void Update() {
        if (Input.GetButtonDown("Tab")) {
            if (userInterface.gameObject.activeSelf) {
                userInterface.gameObject.SetActive(false);
                Time.timeScale = timeScale;
                
            }
            else {
                userInterface.gameObject.SetActive(true);
                timeScale = Time.timeScale;
                Time.timeScale = 0;
                // Need to call `OnSelect` on current selectable to make it highlighted
                EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>()?.OnSelect(null);
            }
        }
    }
}
