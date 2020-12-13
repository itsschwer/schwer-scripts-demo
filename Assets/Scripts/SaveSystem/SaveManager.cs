using Schwer.ItemSystem;
using UnityEngine;

public class SaveManager : DDOLSingleton<SaveManager> {
    public const string fileNameAndExtension = "save.showcase";
    public static string path => Application.persistentDataPath + "/" + fileNameAndExtension;

    [SerializeField] private ItemDatabase itemDatabase = default;
    [SerializeField] private InventorySO _inventory = default;
    private Inventory inventory => _inventory.value;

    private void Start() => LoadSaveData(SaveReadWriter.ReadSaveDataFile(path));

    private void Update() {
        if (Input.GetButtonDown("Save")) {
            Debug.Log("Saved data to " + path);
            DebugCanvas.Instance.Display("Saving");
            SaveReadWriter.WriteSaveDataFile(new SaveData(inventory), path);
        }
        else if (Input.GetButtonDown("Load")) {
            var sd = SaveReadWriter.ReadSaveDataFile(path);
            if (sd != null) {
                Debug.Log("Loaded data from " + path);
                DebugCanvas.Instance.Display("Loading");
                LoadSaveData(sd);
            }
        }
        else if (Input.GetButtonDown("New")) {
            Debug.Log("Loaded new save data");
            DebugCanvas.Instance?.Display("New save loaded");
            LoadSaveData(new SaveData());
        }
    }

    private void LoadSaveData(SaveData sd) {
        sd?.Load(out _inventory.value, itemDatabase);
        FindObjectOfType<Schwer.ItemSystem.Demo.InventoryManager>()?.UpdateSlots();
    }

    public void ImportBase64String(string base64) {
        var sd = WebGLHelper.SaveDataFromBase64String(base64);
        if (sd != null) {
            LoadSaveData(sd);
        }
    }
}
