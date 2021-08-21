using UnityEngine;

namespace Schwer {
    /// <summary>
    /// Inherit from this to make the derived class a singleton (not self-instantiating).
    /// </summary>
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour {
        /// <summary>
        /// The singleton instance of this <c>MonoBehaviour</c>.
        /// </summary>
        public static T Instance { get; protected set; }

        /// <summary>
        /// Initializes a <c>MonoBehaviourSingleton</c> by destroying it if an instance already exists or setting it as the instance if none exist.
        /// </summary>
        /// <remarks>
        /// If overriding, be sure to call <c>base.Awake()</c> to preserve this behaviour.
        /// </remarks>
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
