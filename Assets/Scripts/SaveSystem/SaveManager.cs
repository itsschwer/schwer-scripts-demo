using Schwer;
using Schwer.IO;
using Schwer.ItemSystem;
using Schwer.WebGL;
using UnityEngine;

public class SaveManager : DDOLSingleton<SaveManager> {
    public const string extension = ".showcase";
    public const string fileNameAndExtension = "save" + extension;
    public static string path => Application.persistentDataPath + "/" + fileNameAndExtension;

    [SerializeField] private ItemDatabase itemDatabase = default;
    [SerializeField] private InventorySO _inventory = default;
    private Inventory inventory => _inventory.value;

    private void Start() {
        if (Application.platform != RuntimePlatform.WebGLPlayer) {
            LoadSaveData(BinaryIO.ReadFile<SaveData>(path));
        }
    }

    private void Update() {
        if (Input.GetButtonDown("Save")) {
            Debug.Log("Saved data to " + path);
            DebugCanvas.Instance.Display("Saving");
            BinaryIO.WriteFile<SaveData>(new SaveData(inventory), path);
            if (Application.platform == RuntimePlatform.WebGLPlayer) WebGLSaveHelper.Download(path, SaveManager.fileNameAndExtension);
        }
        else if (Input.GetButtonDown("Load")) {
            var sd = BinaryIO.ReadFile<SaveData>(path);
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
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            WebGLSaveHelper.Import(extension, this.gameObject, ImportBase64String);
        }
    }

    private void LoadSaveData(SaveData sd) {
        sd?.Load(out _inventory.value, itemDatabase);
        // Just for demo, loading normally wouldn't be possible with inventory displayed
        // or should replace with 'real' solution (event, most likely)
        FindObjectOfType<Schwer.ItemSystem.Demo.InventoryManager>()?.UpdateSlots();
        FindObjectOfType<TradeManager>()?.UpdateSlots();
    }

    private void ImportBase64String(string base64) {
        var sd = WebGLSaveHelper.FromBase64String<SaveData>(base64);
        if (sd != null) {
            LoadSaveData(sd);
        }
    }
}
