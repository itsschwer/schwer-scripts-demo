# schwer-scripts: common
[![Root](https://img.shields.io/badge/Root-schwer--scripts-0366D6.svg)](/../../) [![Donate](https://img.shields.io/badge/Donate-PayPal-brightgreen.svg)](https://www.paypal.com/donate?hosted_button_id=NYFKAS24D4MJS)

A collection of scripts that are likely to be used in any project.

## Contents
* [`BinaryIO`](#BinaryIO) (wrapper for reading and writing binary files)
* [Singletons (`MonoBehaviourSingleton` & `DDOLSingleton`)](#Singletons)

# `BinaryIO`
Wrapper class containing generic functions for reading and writing binary files.

Does not contain any error checking.

#### Example usage:
```csharp
public class SaveManager : MonoBehaviourSingleton<SaveManager> {
    private static string filePath => Application.persistentDataPath + "/save.dat";

    // `SaveData` would be a class marked with the `System.Serializable` attribute.
    public SaveData saveData;

    public void Save() => BinaryIO.WriteFile<SaveData>(saveData, filePath);

    public void Load() => saveData = BinaryIO.ReadFile<SaveData>(filePath);
}
```

# Singletons
Singletons are a useful (but often abused) method of ensuring only one instance of a class exists at any time. Avoid using these where possible.

In a situation where there are multiple instances of a class deriving from these base classes, the longest living one will persist, while any newer ones will `Destroy` themselves (and the game object they are attached to) in `Awake`.

These classes are *not* self-instantiating (i.e. you must make sure an `Instance` exists in the scene before attempting to use it).
## Base Classes
* `MonoBehaviourSingleton`
* `DDOLSingleton`
#### Example usage:
```csharp
public class GameManager : MonoBehaviourSingleton<GameManager> {
    // ^ If your class needs to be marked as `DontDestroyOnLoad`, inherit from `DDOLSingleton` instead.
    public int score;
}
```
```csharp
public class Jewel : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        GameManager.Instance.score += 100;
        Destroy(this.gameObject);
    }
}
```
