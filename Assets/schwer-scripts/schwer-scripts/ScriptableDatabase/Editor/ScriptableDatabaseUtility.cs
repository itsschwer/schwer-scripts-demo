using UnityEditor;
using UnityEngine;

namespace SchwerEditor.Database {
    using Schwer.Database;

    /// <summary>
    /// An editor-only class for generating <c cref="ScriptableDatabase">ScriptableDatabase</c>s.
    /// </summary>
    public static class ScriptableDatabaseUtility {
        // [MenuItem("Generate ScriptableDatabase", false, -2), MenuItem("Assets/Create/ScriptableDatabase", false, -11)]
        /// <summary>
        /// Generates or regenerates a <c cref="ScriptableDatabase">ScriptableDatabase</c> asset.
        /// </summary>
        /// <remarks>
        /// Use with the <c>MenuItem</c> attribute or other editor scripts.
        /// </remarks>
        public static void GenerateDatabase<TDatabase, TElement>() 
            where TDatabase : ScriptableDatabase<TElement>
            where TElement : ScriptableObject {

            var db = GetDatabase<TDatabase, TElement>();
            if (db == null) return;

            db.Initialise(AssetsUtility.FindAllAssets<TElement>());

            EditorUtility.SetDirty(db);
            AssetsUtility.SaveRefreshAndFocus();
            Selection.activeObject = db;
        }

        /// <summary>
        /// Attempts to find an instance of a <c cref="ScriptableDatabase">ScriptableDatabase</c> from the Assets folder, creating one if none exist.
        /// </summary>
        /// <remarks>
        /// Returns <c>null</c> if multiple instances exist, logging a warning in the console.
        /// </remarks>
        private static TDatabase GetDatabase<TDatabase, TElement>() 
            where TDatabase : ScriptableDatabase<TElement>
            where TElement : ScriptableObject {

            var databases = AssetsUtility.FindAllAssets<TDatabase>();

            if (databases.Length < 1) {
                Debug.Log($"Creating a new {typeof(TDatabase).Name} since none exist.");
                return ScriptableObjectUtility.CreateAsset<TDatabase>();
            }
            else if (databases.Length > 1) {
                Debug.LogError($"Multiple `{typeof(TDatabase).Name}` exist. Please delete the extra(s) and try again.");
                return null;
            }
            else {
                return databases[0];
            }
        }
    }
}
