using UnityEngine;

namespace Schwer {
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour {
        public static T Instance { get; protected set; }

        protected virtual void Awake() {
            if (Instance != null && Instance != this) {
                // Debug.Log("Instance (" + Instance + ") already set!");
                Destroy(this.gameObject);
            }
            else {
                Instance = this as T;
            }
        }
    }
}
