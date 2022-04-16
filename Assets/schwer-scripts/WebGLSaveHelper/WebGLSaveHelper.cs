using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Schwer.WebGL {
    /// <summary>
    /// Wrapper class for calling JavaScript Export/Import functions.
    /// </summary>
    public static class WebGLSaveHelper {
        [DllImport("__Internal")] private static extern void Export(string base64, string fileName);
        [DllImport("__Internal")] private static extern void Import(string extension, string receiverObject, string receiverMethod);

        /// <summary>
        /// Attempts to deserialize <c>base64</c> into an object of the specified type.
        /// </summary>
        public static T FromBase64String<T>(string base64) where T : class {
            // References:
            // https://stackoverflow.com/questions/17845032/net-mvc-deserialize-byte-array-from-json-uint8array
            // https://stackoverflow.com/questions/4736155/how-do-i-convert-struct-system-byte-byte-to-a-system-io-stream-object-in-c
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

        /// <summary>
        /// Request the browser download <c>data</c> to a file named <c>fileNameAndExtension</c>.
        /// </summary>
        public static void Download(byte[] data, string fileNameAndExtension) {
            // Reference: https://forum.unity.com/threads/access-specific-files-in-idbfs.452168/
            Export(Convert.ToBase64String(data), fileNameAndExtension);
        }

        /// <summary>
        /// Request the browser open a file selection dialog accepting files with the specified <c>extension</c>.
        /// </summary>
        /// <remarks>
        /// A selected file will be sent back as a base64 string using <see href="https://docs.unity3d.com/Manual/webgl-interactingwithbrowserscripting.html">SendMessage</see> to the specified receiver.
        /// </remarks>
        public static void Import(string extension, GameObject receiverObject, Action<string> receiverMethod) {
            Import(extension, receiverObject.name, receiverMethod.Method.Name);
        }
    }
}
