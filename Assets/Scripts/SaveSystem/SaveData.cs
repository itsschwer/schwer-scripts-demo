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
