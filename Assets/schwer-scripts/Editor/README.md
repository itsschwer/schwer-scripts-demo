# schwer-scripts: editor
[![Root](https://img.shields.io/badge/Root-schwer--scripts-0366D6.svg)](/../../) [![Donate](https://img.shields.io/badge/Donate-PayPal-brightgreen.svg)](https://www.paypal.com/donate?hosted_button_id=NYFKAS24D4MJS)

A collection of various editor scripts.

These should always be placed in a folder named `Editor`, as that is a [special folder name](https://docs.unity3d.com/Manual/SpecialFolders.html) that Unity uses to determine which scripts should be stripped from builds.

## Contents
* [`PrefabMenu`](#PrefabMenu) (menu items to speed up prefab workflow)
* [`AssetsUtility`](#AssetsUtility) (work with assets via code)
* [`ScriptableObjectUtility`](#ScriptableObjectUtility) (work with Scriptable Object assets via code)
* [`EditorWindowUtility`](#EditorWindowUtility) [not yet documented]

# `PrefabMenu`
Editor script to improve prefab workflow by adding `Instantiate Prefab` as a menu item to the `GameObject` / right-click in Hierarchy menu. Requires light modification for each prefab.
## Guide <img align="right" width="223" height="402" alt="screenshot of prefab menu in editor" src="https://github.com/itsschwer/schwer-scripts/blob/master/screen-captures/prefab_menu.png?raw=true"></img>
1. Add a `public static` function (with `MenuCommand` as a parameter) that calls `InstantiatePrefab`, passing in the `MenuCommand` and the path to the asset.
2. Add the attribute `[MenuItem("GameObject/Instantiate Prefab/<prefabName (arbitrary)>, false, 0)]` above the function.
#### Example
```cs
[MenuItem("GameObject/Instantiate Prefab/Player", false, 0)]
public static void InstantiatePlayerPrefab(MenuCommand command) {
    InstantiatePrefab(command, "Assets/Prefabs/Player.prefab");
    // ^ You could also use a class-level string to store the path
    //   and pass that as an argument instead.
}
```

# `AssetsUtility`
An editor-only class containing wrapper functions for working with assets.
## Methods
### `FindAllInstances<T>`
Returns an array of all assets of a specified type in the project.
#### Example
```cs
var allScriptableObjects = AssetsUtility.FindAllAssets<ScriptableObject>();
```
### `FindFirstAsset<T>`
Returns the first asset found of a specified type in the project.

The function will automatically append ` typeof(T)` if the `filter` argument does not contain the `t:` keyword.

Refer to the official Unity documentation on [`AssetDatabase.FindAssets`](https://docs.unity3d.com/ScriptReference/AssetDatabase.FindAssets.html) for more information on the `filter` argument.

# `ScriptableObjectUtility`
An editor-only class for working with Scriptable Object assets through code.
## Methods
### `CreateAsset<T>`
Creates a Scriptable Object of type `T` in a process similar to `[CreateAssetMenu]`. 
#### Example usage *(from [ScriptableDatabaseUtility.cs](/schwer-scripts/ScriptableDatabase/Editor/ScriptableDatabaseUtility.cs))*
```cs
private static TDatabase GetDatabase<TDatabase, TElement>() 
    where TDatabase : ScriptableDatabase<TElement>
    where TElement : ScriptableObject {

    var databases = AssetsUtility.FindAllAssets<TDatabase>();

    if (databases.Length < 1) {
        Debug.Log($"Creating a new {typeof(TDatabase).Name} since none exist.");
        return ScriptableObjectUtility.CreateAsset<TDatabase>();
        // ^ Creating a new ScriptableObject asset if none exist in the project!
    }
    else if (databases.Length > 1) {
        Debug.LogError($"Multiple `{typeof(TDatabase).Name}` exist. Please delete the extra(s) and try again.");
        return null;
    }
    else {
        return databases[0];
    }
}
```

# `EditorWindowUtility`
Not yet documented.
