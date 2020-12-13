using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class WebGLHelper {
    // Reference: https://answers.unity.com/questions/1095407/saving-webgl.html
    // [DllImport("__Internal")] private static extern void SyncFiles();
    // ^ Needs to be called in order to save persistent data to browser's IndexedDB
    // Unsatisfactory on itch.io, since each upload will have a different path
    // Does not work by default with Chrome -incognito, haven't tested with other browsers

    [DllImport("__Internal")] private static extern void SetDownload(string base64, string fileName);
    [DllImport("__Internal")] public static extern void ImportEnabled(bool enabled);

    // References:
    // https://stackoverflow.com/questions/17845032/net-mvc-deserialize-byte-array-from-json-uint8array
    // https://stackoverflow.com/questions/4736155/how-do-i-convert-struct-system-byte-byte-to-a-system-io-stream-object-in-c
    public static SaveData SaveDataFromBase64String(string base64) {
        var formatter = new BinaryFormatter();
        using (var stream = new MemoryStream(Convert.FromBase64String(base64))) {
            try {
                return formatter.Deserialize(stream) as SaveData;
            }
            catch (System.Runtime.Serialization.SerializationException e) {
                Debug.Log("Deserialisation failed: " + e);
            }
        }
        return null;
    }

    // Reference: https://forum.unity.com/threads/access-specific-files-in-idbfs.452168/
    public static void PushToDownload(string filePath, string fileName) {
        var bytes = File.ReadAllBytes(filePath);
        SetDownload(Convert.ToBase64String(bytes), fileName);
    }
}
