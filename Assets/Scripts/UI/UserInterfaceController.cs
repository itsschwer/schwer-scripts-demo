using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserInterfaceController : MonoBehaviour {
    private static event Action OnTradeInterfaceRequested;
    public static void RequestTradeInterface() => OnTradeInterfaceRequested?.Invoke();
    
    [SerializeField] private Canvas inventoryInterface = default;
    [SerializeField] private Canvas tradeInterface = default;

    private float timeScale;

    private void OnEnable() => OnTradeInterfaceRequested += EnableTradeInterface;
    private void OnDisable() => OnTradeInterfaceRequested -= EnableTradeInterface;

    private void Update() {
        if (Input.GetButtonDown("Tab")) {
            if (tradeInterface != null && tradeInterface.gameObject.activeSelf) {
                tradeInterface.gameObject.SetActive(false);
                Time.timeScale = timeScale;
            }
            else {
                if (inventoryInterface.gameObject.activeSelf) {
                    inventoryInterface.gameObject.SetActive(false);
                    Time.timeScale = timeScale;
                    
                }
                else {
                    PauseTime();
                    inventoryInterface.gameObject.SetActive(true);
                    // Need to call `OnSelect` on current selectable to make it highlighted
                    EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>()?.OnSelect(null);
                }
            }
        }
    }

    private void PauseTime() {
        timeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    private void EnableTradeInterface() {
        PauseTime();
        tradeInterface.gameObject.SetActive(true);
        // Need to call `OnSelect` on current selectable to make it highlighted
        EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>()?.OnSelect(null);
    }
}
