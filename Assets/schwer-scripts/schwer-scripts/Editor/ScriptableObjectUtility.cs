using System.IO;
using UnityEditor;
using UnityEngine;

namespace SchwerEditor {
    /// <summary>
    /// An editor-only class for working with <c>ScriptableObject</c> assets.
    /// </summary>
    public static class ScriptableObjectUtility {
        /// <summary>
        /// Creates a new <c>ScriptableObject</c> asset in a manner that mimics Unity's asset creation process.
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
            AssetsUtility.SaveRefreshAndFocus();
            return asset;
        }
    }
}
