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

    [SerializeField] private KeyCode import = KeyCode.Alpha4;
    [SerializeField] private KeyCode export = KeyCode.Alpha5;

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

        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            if (Input.GetKeyDown(import)) {
                WebGLSaveHelper.Import(extension, this.gameObject, ImportBase64String);
            }
            else if (Input.GetKeyDown(export)) {
                WebGLSaveHelper.Download(System.IO.File.ReadAllBytes(path), fileNameAndExtension);
            }
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
