using UnityEditor;
using UnityEngine;

namespace SchwerEditor {
    /// <summary>
    /// Editor class containing wrapper functions for working with assets.
    /// </summary>
    public static class AssetsUtility {
        /// <summary>
        /// Wrapper for `AssetDatabase.SaveAssets`, `AssetDatabase.Refresh`, and `EditorUtility.FocusProjectWindow`.
        /// </summary>
        public static void SaveRefreshAndFocus() {
            //! Need clarification on what each line does.
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }
        
        /// <summary>
        /// Returns all assets of a specified type from the Assets folder.
        /// </summary>
        public static T[] FindAllAssets<T>() where T : Object {
            // From: https://answers.unity.com/questions/1425758/how-can-i-find-all-instances-of-a-scriptable-objec.html
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);

            T[] instances = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++) {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                instances[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return instances;
        }

        /// <summary>
        /// Returns the first asset of a specified type from the Assets folder.
        /// </summary>
        public static T FindFirstAsset<T>(string filter) where T : Object {
            if (!filter.Contains("t:")) {
                filter = $"{filter} t:{typeof(T)}";
            }

            var guids = AssetDatabase.FindAssets(filter);
            if (guids.Length <= 0) return default(T);

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
        }
    }
}
