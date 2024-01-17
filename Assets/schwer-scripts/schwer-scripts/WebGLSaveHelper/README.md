# schwer-scripts: WebGL save helper
[![root](https://img.shields.io/badge/docs--root-cornflowerblue.svg)](/../../)
[![Donate with PayPal](https://img.shields.io/badge/Donate-FFD140?logo=paypal)](https://www.paypal.com/donate?hosted_button_id=NYFKAS24D4MJS)

Helper files for importing and exporting data to and from WebGL builds.

***Note**: `Pointer_stringify` (in `.jslib`) should be replaced with `UTF8ToString` from Unity 2021.2 onwards!*

## Contents
- `WebGLSaveHelper.jslib` (handles downloading and selecting files)
- `WebGLSaveHelper.cs` (wrapper for calling JavaScript functions)

## Example usage
```cs
public class SaveManager : MonoBehaviourSingleton<SaveManager> {
    private static string filePath => $"{Application.persistentDataPath}/{fileName}{extension}";
    private const string fileName = "save";
    private const string fileExtension = ".dat";

    // `SaveData` would be a class marked with the `System.Serializable` attribute.
    public SaveData saveData;

    public void Save() {
        BinaryIO.WriteFile<SaveData>(saveData, filePath);

        // Automatically download new saves when they are made
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            WebGLSaveHelper.Download(File.ReadAllBytes(filePath), fileName + fileExtension);
        }
    }

    public void Load() => saveData = BinaryIO.ReadFile<SaveData>(filePath);

    // Start browser interaction for file select dialog
    public void Select() => WebGLSaveHelper.Import(extension, this.gameObject, Import);

    // Automatically called from .jslib using SendMessage (arguments provided by `WebGLSaveHelper.Import` — see above!)
    private void Import(string base64) {
        var data = WebGLSaveHelper.FromBase64String<SaveData>(base64);
        if (data != null) {
            saveData = data;
        }
    }
}
```
