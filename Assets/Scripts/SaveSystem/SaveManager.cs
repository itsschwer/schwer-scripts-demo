using Schwer.ItemSystem;
using UnityEngine;

public class SaveManager : MonoBehaviour {
    private static SaveManager _Instance;
    public static SaveManager Instance => _Instance;

    private string fileNameAndExtension = "save.showcase";
    private string path => Application.persistentDataPath + "/" + fileNameAndExtension;

    [SerializeField] private ItemDatabase itemDatabase = default;
    [SerializeField] private InventorySO _inventory = default;
    private Inventory inventory => _inventory.value;

    private void Awake() {
        if (_Instance != null && _Instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _Instance = this;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F9)) {
            Debug.Log("Saved data to " + path);
            SaveReadWriter.WriteSaveDataFile(new SaveData(inventory), path);
        }
        else if (Input.GetKeyDown(KeyCode.F10)) {
            var sd = SaveReadWriter.ReadSaveDataFile(path);
            if (sd != null) {
                Debug.Log("Loaded data from " + path);
                LoadSaveData(sd);
            }
        }
        else if (Input.GetKeyDown(KeyCode.F11)) {
            Debug.Log("Loaded new save data");
            LoadSaveData(new SaveData());
        }
    }

    private void LoadSaveData(SaveData sd) => sd.Load(out _inventory.value, itemDatabase);
}
