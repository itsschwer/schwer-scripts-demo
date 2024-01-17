using UnityEditor;

namespace SchwerEditor.ItemSystem {
    using Schwer.ItemSystem;
    using SchwerEditor.Database;

    [CustomEditor(typeof(ItemDatabase))]
    public class ItemDatabaseInspector : ScriptableDatabaseInspector<ItemDatabase, Item> {
        [MenuItem("Item System/Generate ItemDatabase", false, -2), MenuItem("Assets/Create/Item System/ItemDatabase", false, -11)]
        public static void GenerateDatabase() => ScriptableDatabaseUtility.GenerateDatabase<ItemDatabase, Item>();
    }
}
