using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Schwer.WebGL {
    public static class WebGLSaveHelper {
        [DllImport("__Internal")] private static extern void Export(string base64, string fileName);
        [DllImport("__Internal")] private static extern void Import(string extension, string receiverObject, string receiverMethod);

        // References:
        // https://stackoverflow.com/questions/17845032/net-mvc-deserialize-byte-array-from-json-uint8array
        // https://stackoverflow.com/questions/4736155/how-do-i-convert-struct-system-byte-byte-to-a-system-io-stream-object-in-c
        public static T FromBase64String<T>(string base64) where T : class {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(Convert.FromBase64String(base64))) {
                try {
                    return formatter.Deserialize(stream) as T;
                }
                catch (System.Runtime.Serialization.SerializationException e) {
                    Debug.LogWarning(e);
                    return null;
                }
            }
        }

        // Reference: https://forum.unity.com/threads/access-specific-files-in-idbfs.452168/
        public static void Download(string filePath, string fileName) {
            Export(Convert.ToBase64String(File.ReadAllBytes(filePath)), fileName);
        }

        public static void Import(string extension, GameObject receiverObject, Action<string> receiverMethod) {
            Import(extension, receiverObject.name, receiverMethod.Method.Name);
        }
    }
}
