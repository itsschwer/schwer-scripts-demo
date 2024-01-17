# schwer-scripts
[![Source on GitHub](https://img.shields.io/badge/Source-0D1117?logo=github)](https://github.com/itsschwer/schwer-scripts)
[![Donate with PayPal](https://img.shields.io/badge/Donate-FFD140?logo=paypal)](https://www.paypal.com/donate?hosted_button_id=NYFKAS24D4MJS)

Open-source code for Unity.

A demonstration of how this repository could be used, including a playable demo, is available at [**schwer-scripts-demo**](https://github.com/itsschwer/schwer-scripts-demo).

## About
This repository contains some helpful scripts and  skeleton implementations of various features. Feel free to use this as a reference or a base for your own project(s).

I'm just a hobbyist, so if you have any improvements or suggestions, let me know!

## Contents
- [Common scripts](/schwer-scripts/Common)
    - `BinaryIO` (wrapper for reading and writing binary files)
    - `MonoBehaviourSingleton` & `DDOLSingleton`
- [Editor scripts](/schwer-scripts/Editor)
    - `PrefabMenu` (menu items to speed up prefab workflow)
    - `AssetsUtility` (work with assets via code)
    - `ScriptableObjectUtility` (work with Scriptable Object assets via code)
- [Item System](/schwer-scripts/ItemSystem) (serializable!)
- [Scriptable Database](/schwer-scripts/ScriptableDatabase)
- [WebGL Data Importing/Exporting](/schwer-scripts/WebGLSaveHelper)

## Updating
There currently isn't an automatic process to update a copy of **schwer-scripts** that has been imported into a Unity project, but here are the steps I've been taking to update manually:
1. Open the project that needs an updated version of **schwer-scripts** in Unity.
2. Open the Assets folder in a file explorer.
3. Make sure Unity is not focused (but still open).
    - This gives Unity the best chance to regenerate the `.meta` files so that references aren't broken.
4. Delete the `schwer-scripts` folder from the Assets folder (via the file explorer).
5. Copy the new `schwer-scripts` folder to where the old `schwer-scripts` was.
6. Refocus Unity and wait for it to finish importing.
7. Use your version control tool to restore any modifications you might have made to `schwer-scripts` files.
