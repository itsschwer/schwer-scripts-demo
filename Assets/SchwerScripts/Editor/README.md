# schwer-scripts: editor
[![Root](https://img.shields.io/badge/Root-schwer--scripts-blue.svg)](https://github.com/itsschwer/schwer-scripts) [![Donate](https://img.shields.io/badge/Donate-PayPal-brightgreen.svg)](https://www.paypal.com/donate?hosted_button_id=NYFKAS24D4MJS)

A collection of various editor scripts.

These should always be placed in a folder named `Editor`, as that is a [special folder name](https://docs.unity3d.com/Manual/SpecialFolders.html) that Unity uses to determine which scripts should be stripped from builds.

## Contents
* [`PrefabMenu`](#PrefabMenu) (menu items to speed up prefab workflow)
* [`ScriptableObjectUtility`](#ScriptableObjectUtility) (work with Scriptable Object assets via code)
* [`EditorWindowUtility`](#EditorWindowUtility) [not yet documented]

# `PrefabMenu`
Editor script to improve prefab workflow by adding `Instantiate Prefab` as a menu item to the `GameObject` / right-click in Hierarchy menu. Requires light modification for each prefab.
## Guide <img align="right" width="223" height="402" alt="screenshot of prefab menu in editor" src="https://github.com/itsschwer/schwer-scripts/blob/master/screen-captures/prefab_menu.png?raw=true"></img>
1. Add a `public static` function (with `MenuCommand` as a parameter) that calls `InstantiatePrefab`, passing in the `MenuCommand` and the path to the asset.
2. Add the attribute `[MenuItem("GameObject/Instantiate Prefab/<prefabName (arbitrary)>, false, 0)]` above the function.
#### Example
```csharp
[MenuItem("GameObject/Instantiate Prefab/Player", false, 0)]
public static void InstantiatePlayerPrefab(MenuCommand command) {
    InstantiatePrefab(command, "Assets/Prefabs/Player.prefab");
    // ^ You could also use a class-level string to store the path
    //   and pass that as an argument instead.
}
```

# `ScriptableObjectUtility`
Editor script intended for working with Scriptable Object assets through code.
## Methods
### `CreateAsset<T>`
Creates a Scriptable Object of type `T` in a process similar to `[CreateAssetMenu]`. 
#### Example usage (from [ItemDatabaseUtility.cs](https://github.com/itsschwer/schwer-scripts/blob/master/SchwerScripts/ItemSystem/Editor/ItemDatabaseUtility.cs)):
```csharp
private static ItemDatabase GetItemDatabase() {
    var databases = ScriptableObjectUtility.GetAllInstances<ItemDatabase>();

    ItemDatabase itemDB = null;
    if (databases.Length < 1) {
        Debug.Log("Creating a new ItemDatabase since none exist.");
        itemDB = ScriptableObjectUtility.CreateAsset<ItemDatabase>();
        // ^ Creating an asset if none exist in the project!
    }
    else if (databases.Length > 1) {
        Debug.LogError("Multiple ItemDatabases exist. Please delete the extra(s) and try again.");
    }
    else {
        itemDB = databases[0];
    }

    return itemDB;
}
```
### `GetAllInstances<T>`
Returns an array of all Scriptable Object assets of type `T` in the project.
#### Example usage (from [ItemDatabaseUtility.cs](https://github.com/itsschwer/schwer-scripts/blob/master/SchwerScripts/ItemSystem/Editor/ItemDatabaseUtility.cs)):
```csharp
// Return a list of Item instances, omitting those with duplicate ids.
private static List<Item> GetAllItemAssets() {
    var result = new List<Item>();

    var instances = ScriptableObjectUtility.GetAllInstances<Item>();
    // ^ Get all instances of type Item for sorting!
    var gatheredIDs = new List<int>();
    for (int i = 0; i < instances.Length; i++) {
        if (gatheredIDs.Contains(instances[i].id)) {
            var sharedID = result[gatheredIDs.IndexOf(instances[i].id)].name;
            Debug.LogWarning("'" + instances[i].name + "' was excluded from the ItemDatabase because it shares its ID (" + instances[i].id + ") with '" + sharedID + "'.");
        }
        else {
            result.Add(instances[i]);
            gatheredIDs.Add(instances[i].id);
        }
    }

    result = result.OrderBy(i => i.id).ToList();
    return result;
}
```

# `EditorWindowUtility`
Not yet documented.
