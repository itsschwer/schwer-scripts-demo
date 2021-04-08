using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class BinaryIO {
    public static T ReadFile<T>(string filePath) {
        var formatter = new BinaryFormatter();
        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
            try {
                return (T)(formatter.Deserialize(stream));
            }
            catch (System.Exception e) {
                Debug.Log("File at '" + filePath + "' is incompatible — " + e);
            }
        }
        return default(T);
    }

    public static void WriteFile<T>(T obj, string filePath) {
        var formatter = new BinaryFormatter();
        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None)) {
            formatter.Serialize(stream, obj);
        }

        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            WebGLHelper.PushToDownload(filePath, SaveManager.fileNameAndExtension);
        }
    }
}
