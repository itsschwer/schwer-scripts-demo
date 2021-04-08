# schwer-scripts: item system
[![Root](https://img.shields.io/badge/Root-schwer--scripts-0366D6.svg)](/../../) [![Donate](https://img.shields.io/badge/Donate-PayPal-brightgreen.svg)](https://www.paypal.com/donate?hosted_button_id=NYFKAS24D4MJS)

A skeleton implementation of items and (serializable!) inventories using Scriptable Objects, as well as several supporting editor tools.

## Features
* Items and inventories in Unity using Scriptable Objects
* Inventories that support saving and loading (i.e. serializable)
* An Item Database that is simple to generate and regenerate
* An Item Editor (custom editor window)

## Limitations
* Inventories are implemented using a `Dictionary`, which Unity does not serialize. This means that:
    * `Inventory` assets cannot be edited via the Inspector.
        * *A solution is being worked on. For now, the example `InventoryInspector` provides a crude workaround.*
    * ~~Inventory assets will lose their values on domain reload (e.g. entering Play mode)~~.
        * ***`Inventory` now implements `ISerializationCallbackReciever` to address this. This is a recent change, so please alert me if there are any issues pertaining to this.***
* By default, inventories do not support fixed inventory sizes, item weighting systems, or the like (though developers may try to implement such functionalities).
    * For reference, as may be evident from the demo prefab, this system was modelled after the one in *Breath of the Wild*.
        * *(Different inventory sections could be handled with front-end sorting code, e.g. through use of an `ItemType` enum).*

## Notes
* This implementation only provides the bare bones of an item system.
* It is expected that developers will extend the `Item` class in order for items to be usable in a game (e.g. consumables).
* The contents of the provided Demo folder is intended only for developers to familiarise themselves with the system.
    * Developers should implement their own system for displaying inventory to the UI, items that can be placed in the world, etc.
* As `Inventory` assets are Scriptable Objects, there must be a reference to each `Inventory` asset at all times during runtime in order for their values to persist, or Unity will unload them the next time it unloads unused resources.

## Getting started
Download this repository and add the `schwer-scripts` folder to your Unity project.
Familiarise yourself with the system by dragging the prefab `Inventory` (`schwer-scripts/ItemSystem/Demo/UI/Prefabs/Inventory`) into a new Scene and following along with this section.

You may need to create an Event System (`Right-click in the Hierarchy > UI/Event System`) in order to interact with the demo UI. Also note that the UI was designed for a 16:9 game window.

### Items
Create `Item` assets in a folder of your choice via `Create/Item System/Item`.
Edit them via the Inspector, or the Item Editor.

### Item Editor
Can be opened in three ways:
* From the toolbar via `Item System/Open Item Editor`
* From the Inspector of an `Item` asset
* From double-clicking an `Item` asset

This window is not necessary, but it does provide a helpful overview of the `Item` assets in your project.

### Item Database
This system relies on an `ItemDatabase` for the saving and loading of inventories. A `ItemDatabase` can be created:
* From the toolbar via `Item System/Generate Item Database`
* From the `Create` context menu (`Create/Item System/Item Database`).
* From the Item Editor (button at the bottom labelled `Generate Item Database`)

This will create an `ItemDatabase` in the currently selected folder if none exist.
Otherwise, it will update (regenerate) an existing `ItemDatabase`.
To regenerate an `ItemDatabase` after editing items, use either of the above methods for creating an `ItemDatabase` or via the Inspector of the `ItemDatabase` asset.

### Inventory
Create an `Inventory` asset in a folder of your choice via `Create/Item System/Inventory`.
Assign this asset to the `InventoryManager` script attached to the instance of the `Inventory` prefab in the Scene.

### Play Mode
Enter Play mode and select your `Inventory` asset to experiment with the demo controls (make sure to assign an `Item` to the custom Inspector!).

## Usage guide
Non-editor scripts are in the namespace `Schwer.ItemSystem`, so remember to add that as a `using` when working with the Item System.

### Item Database
Generate or regenerate by:
* Menu item `Item System/Generate Item Database`
* Menu item `Create/Item System/Item Database`
* Button in the Item Editor
* Inspector of an existing `ItemDatabase` asset

#### Generation process
This process will first check for all `ItemDatabase` assets in your project. If none exist, a new one will be created in the selected folder/folder of selected item with the name `ItemDatabase`. If one already exists it will be regenerated. If multiple exist, an error will be logged to the Console. Make sure that your game object(s) reference a single `ItemDatabase` and delete the extra assets before trying again.

Stores `Item`s in a List, ordered by id in ascending order. The generation process is done alphabetically (by filename). Any `Item`s with an id matching that of an `Item` that has already been added will be omitted from the `ItemDatabase` and a warning will be logged to the Console.

#### Usage
The `ItemDatabase` should only be referenced in the scene where inventories are deserialized, as that is its only intended purpose (otherwise all `Item`s referenced by the `ItemDatabase` will be loaded and kept in memory, which is likely unnecessary).

#### Why use an Item Database?
Rather than trying to serialize each `Item` in an `Inventory`, an `Inventory` is first serialized into a form (`SerializableInventory`) that stores the `Item`'s id instead of the `Item` itself. This is done for several reasons:
* It is unlikely that the entire `Item` object needs to be written to a save file — it's already in the game!
    * `Item`s themselves may not be serializable at all, depending on the fields they contain.
    * Prevents "cheating in" non-existent items by modifying save data.
* Serializing nested Scriptable Objects is prone to error.

Loading a `SerializableInventory` requires an `ItemDatabase` which is used to match ids in the `SerializableInventory` with the corresponding `Item` in the `ItemDatabase`. Because of this, ***item ids should not change once a build has been released***, since that will cause ids to be matched to the wrong `Item`.

### Item Editor
Accessible via:
* Menu item `Item System/Open Item Editor`
* Inspector of an `Item` asset
* Double-clicking on an `Item` asset

Serves as an additional way to edit existing `Item` assets. `Item`s are ordered in the sidebar by their id for convenience. Additionally, this window will highlight any `Item`s that have matching ids.

### Item
This implementation provides a bare-bones script intended to be a base from which developers will extend. It contains fields for: `id`, `name`, `description`, `sprite`, and `stackable`. How these are used is left up to the developer.

### Inventory
The inventory Scriptable Object (`InventorySO`) can be thought of as a container for an `Inventory`. An `Inventory` can be used in a manner similar to a  `Dictionary<Item, int>`, where `int` represents the number of an `Item` held in the `Inventory`.
##### Code examples:
```csharp
[SerializeField] private InventorySO _inventory = default;
public Inventory inventory => _inventory.value;

// e.g. Picking up an item.
public void ObtainItem(Item item, int amount) {
    inventory[item] += amount;
}

// Use up <amount> of <item>, e.g. for crafting.
public bool UseItem(Item item, int amount) {
    if (inventory[item] >= amount) {
        inventory[item] -= amount;
        return true;
    }
    else {
        return false;
    }
}

// e.g. Resetting a shop's inventory of an item.
public void SetItem(Item item, int amount) {
    inventory[item] = amount;
}
    
// e.g. Removing illegal items.
public void RemoveItem(Item item) {
    inventory.Remove(item);
}
```

## Saving and Loading
Saving and loading has successfully been done using a `BinaryFormatter` approach *(refer to [`BinaryIO`](/schwer-scripts/Common/NonMono/BinaryIO.cs))*. Ensure that the object you serialize uses `SerializableInventory` and not `Inventory`.
##### Example code:
```csharp
using Schwer.ItemSystem;

[System.Serializable]
public class SaveData {
    private SerializableInventory inventory;

    // Construct new save data.
    public SaveData() {
        inventory = new SerializableInventory();
    }

    // Construct save data from an Inventory.
    public SaveData(Inventory inventory) {
        this.inventory = inventory.Serialize();
    }

    // Load save data 
    public void Load(out Inventory inventory, ItemDatabase itemDB) {
        inventory = this.inventory.Deserialize(itemDB);
    }
}
```
