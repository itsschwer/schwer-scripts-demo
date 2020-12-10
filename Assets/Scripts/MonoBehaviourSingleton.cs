using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour {
    protected static T _Instance;
    public static T Instance => _Instance;

    protected virtual void Awake() {
        if (_Instance != null && _Instance != this) {
            Debug.Log("Instance (" + _Instance + ") already set!");
            Destroy(this.gameObject);
        }
        else {
            _Instance = this as T;
        }
    }
}
