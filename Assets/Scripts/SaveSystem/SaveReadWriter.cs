using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveReadWriter {
    public static SaveData ReadSaveDataFile(string filePath) {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(filePath, FileMode.Open)) {
            try {
                return formatter.Deserialize(stream) as SaveData;
            }
            catch (System.Runtime.Serialization.SerializationException e) {
                Debug.Log("File at: " + filePath + " is incompatible. " + e);
            }
        }
        return null;
    }

    public static void WriteSaveDataFile(SaveData saveData, string filePath) {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(filePath, FileMode.Create)) {
            formatter.Serialize(stream, saveData);
        }

        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            WebGLHelper.PushToDownload(filePath, SaveManager.fileNameAndExtension);
        }
    }
}
