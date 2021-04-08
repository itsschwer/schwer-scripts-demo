using UnityEngine;

namespace Schwer {
    public class DDOLSingleton<T> : MonoBehaviourSingleton<T> where T : MonoBehaviour {
        protected override void Awake() {
            if (Instance != null && Instance != this) {
                // Debug.Log("Instance (" + Instance + ") already set!");
                Destroy(this.gameObject);
            }
            else {
                Instance = this as T;

                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
}
