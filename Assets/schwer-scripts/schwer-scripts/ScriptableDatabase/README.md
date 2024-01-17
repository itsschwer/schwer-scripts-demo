# schwer-scripts: scriptable database
[![root](https://img.shields.io/badge/docs--root-cornflowerblue.svg)](/../../)
[![Donate with PayPal](https://img.shields.io/badge/Donate-FFD140?logo=paypal)](https://www.paypal.com/donate?hosted_button_id=NYFKAS24D4MJS)

A basic implementation of a database of Scriptable Objects. Useful for creating collections for types of Scriptable Objects, for example, [Items](/schwer-scripts/ItemSystem).

By design, there should only exist up to one instance of a Scriptable Database of a given type at a time.

<center><img alt="screenshot of the inspector for a scriptable database" src="https://github.com/itsschwer/schwer-scripts/blob/master/screen-captures/database_inspector.png?raw=true"></img></center>

# Guide
0. Create a `ScriptableObject` class for the type of object to create a database for.
    - For example:
        ```cs
        public class Item : ScriptableObject {}
        ```
1. Create a new class that inherits from `ScriptableDatabase`.
    - For example:
        ```cs
        public class ItemDatabase : ScriptableDatabase<Item> {}
        ```
2. Add an array as the first serializable field in the database class.
    - For example:
        ```cs
        [SerializeField] private Item[] items;
        ```
3. Implement `Initialise`
    - For example:
        ```cs
        public override void Initialise(Item[] items) => this.items = items;
        ```
4. Create a new class that inherits from `ScriptableDatabaseInspector`.
    - This will need to be placed in a folder named `Editor`.
    - The type parameters should match that of your database and database element.
    - Don't forget to add the `CustomEditor` attribute.
    - For example:
        ```cs
        [CustomEditor(typeof(ItemDatabase))]
        public class ItemDatabaseInspector : ScriptableDatabaseInspector<ItemDatabase, Item> {}
        ```
5. Add a static method that calls `ScriptableDatabaseUtility.GenerateDatabase`
    - The type parameters should match that of your database and database element
    - For example:
        ```cs
        private static void GenerateDatabase() => ScriptableDatabaseUtility.GenerateDatabase<ItemDatabase, Item>();
        ```
6. Add the `MenuItem` attribute above the static method so that it can be run in the editor as a menu command.
    - For example:
        ```cs
        [MenuItem("Databases/Generate Item Database")]
        private static void GenerateDatabase() //…
        ```

# Documentation

## `ScriptableDatabase`
An abstract base class to inherit from to create a ScriptableObject that serves as a database for a specified type of ScriptableObject.

The first serializable property should be an array of the type of ScriptableObject the database should hold.

### Methods
#### `Initialise`
An abstract method that should be overriden in derived classes.

Intended to be used only in the editor through `ScriptableDatabaseUtility.GenerateDatabase` to initialise the array.
#### `FilterByID`
Filters out elements with duplicate ids (keeping only the first element) then sorts elements by id, in ascending order, returning the results as a new array.
#### `GetFromID`
Loops through elements and tries to find one with a matching id.

## `IID`
An interface that should be inherited from to ensure that a class has an id. Required in order to be compatible with the id-based methods in `ScriptableDatabase`.

## `ScriptableDatabaseUtility`
An editor-only class for generating Scriptable Databases.

### Methods
#### `GenerateDatabase`
Generates or regenerates a ScriptableDatabase asset.

Intended to be used in editor scripts and/or Unity's `MenuItem` attribute.
##### Example *(from [ItemDatabaseInspector.cs](/schwer-scripts/ItemSystem/Editor/ItemDatabaseInspector.cs))*
```cs
public class ItemDatabaseInspector : ScriptableDatabaseInspector<ItemDatabase, Item> {
    [MenuItem("Item System/Generate ItemDatabase", false, -2), MenuItem("Assets/Create/Item System/ItemDatabase", false, -11)]
    public static void GenerateDatabase() => ScriptableDatabaseUtility.GenerateDatabase<ItemDatabase, Item>();
}
```
