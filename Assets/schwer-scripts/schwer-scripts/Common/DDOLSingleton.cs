using UnityEngine;

namespace Schwer {
    /// <summary>
    /// Inherit from this to make the derived class a singleton that persists between scene loads (not self-instantiating).
    /// </summary>
    public class DDOLSingleton<T> : MonoBehaviourSingleton<T> where T : MonoBehaviour {
        /// <summary>
        /// Initializes a <c>DDOLSingleton</c> by destroying it if an instance already exists or setting it as the instance and marking it as <c>DontDestroyOnLoad</c> if none exist.
        /// </summary>
        /// <remarks>
        /// If overriding, be sure to call <c>base.Awake()</c> to preserve this behaviour.
        /// </remarks>
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
