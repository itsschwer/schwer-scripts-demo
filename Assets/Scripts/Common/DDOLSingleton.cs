using UnityEngine;

public class DDOLSingleton<T> : MonoBehaviourSingleton<T> where T : MonoBehaviour {
    protected override void Awake() {
        if (_Instance != null && _Instance != this) {
            Debug.Log("Instance (" + _Instance + ") already set!");
            Destroy(this.gameObject);
        }
        else {
            _Instance = this as T;

            DontDestroyOnLoad(this.gameObject);
        }
    }
}
