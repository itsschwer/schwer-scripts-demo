using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Schwer.IO {
    /// <summary>
    /// Wrapper class containing generic functions for reading and writing binary files.
    /// </summary>
    public static class BinaryIO {
        /// <summary>
        /// Attempts to read the file at `filePath`, returning an object of the specified type.
        /// </summary>
        public static T ReadFile<T>(string filePath) {
            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                try {
                    return (T)(formatter.Deserialize(stream));
                }
                catch (System.Exception e) {
                    UnityEngine.Debug.Log("File at '" + filePath + "' is incompatible — " + e);
                }
            }
            return default(T);
        }

        /// <summary>
        /// Attempts to write `obj` to the file at `filePath`, replacing any existing file.
        /// </summary>
        public static void WriteFile<T>(T obj, string filePath) {
            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None)) {
                formatter.Serialize(stream, obj);
            }
        }
    }
}
