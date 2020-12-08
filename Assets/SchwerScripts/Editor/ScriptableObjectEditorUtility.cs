using System.IO;
using UnityEditor;
using UnityEngine;

namespace SchwerEditor {
    public static class ScriptableObjectEditorUtility {
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
        /// Creates a new Scriptable Object asset in a manner that mimics Unity's asset creation process.
        /// </summary>
        public static T CreateAsset<T>() where T : ScriptableObject {
            // From: https://wiki.unity3d.com/index.php/CreateScriptableObjectAsset
            var asset = ScriptableObject.CreateInstance<T>();

            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "") {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "") {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            path = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(T).Name + ".asset");
            
            AssetDatabase.CreateAsset(asset, path);
            SaveRefreshAndFocus();
            return asset;
        }

        /// <summary>
        /// Get all instances of a specified Scriptable Object from the Assets folder.
        /// </summary>
        public static T[] GetAllInstances<T>() where T: ScriptableObject {
            // From: https://answers.unity.com/questions/1425758/how-can-i-find-all-instances-of-a-scriptable-objec.html
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);

            T[] instances = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++) {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                instances[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return instances;
        }
    }
}
