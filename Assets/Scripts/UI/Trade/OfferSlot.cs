using System.Collections;
using Schwer.ItemSystem;
using Schwer.ItemSystem.Demo;
using UnityEngine;
using UnityEngine.UI;

public class OfferSlot : MonoBehaviour {
    [SerializeField] private ItemSlot[] inputSlots = default;
    [SerializeField] private ItemSlot[] outputSlots = default;

    public TradeManager manager { get; set; }

    public OfferItem[] inputItems { get; private set; } = new OfferItem[2];
    public OfferItem[] outputItems { get; private set; } = new OfferItem[2];

    private GraphicInfo[] graphics = new GraphicInfo[4];

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(() => manager.TryTrade(this));

        for (int i = 0; i < 4; i++) {
            if (i < 2) {
                graphics[i].graphic = inputSlots[i].GetComponent<Graphic>();
            }
            else {
                graphics[i].graphic = outputSlots[i - 2].GetComponent<Graphic>();
            }
        }
    }

    private void OnDisable() {
        for (int i = 0; i < graphics.Length; i++) {
            graphics[i].graphic.color = graphics[i].initialColor;
        }
    }

    /// <summary>
    /// Hard-coded example: expects 4 offerItems (2 input, 2 output)
    /// </summary>
    public void SetOfferItems(OfferItem[] offerItems) {
        for (int i = 0; i < 4; i++) {
            var offer = offerItems[i];
            if (i < 2) {
                inputItems[i] = offer;
                inputSlots[i].SetItem(offer.item, offer.count);
            }
            else {
                outputItems[i - 2] = offer;
                outputSlots[i - 2].SetItem(offer.item, offer.count);
            }
        }
    }

    public bool CheckInventory(Inventory inventory) {
        bool result = true;
        for (int i = 0; i < inputItems.Length; i++) {
            if (inputItems[i].item != null && inventory[inputItems[i].item] < inputItems[i].count) {
                if (graphics[i].coroutine != null) {
                    StopCoroutine(graphics[i].coroutine);
                }
                graphics[i].coroutine = StartCoroutine(FlashCo(graphics[i], 0.5f));
                result = false;
            }
        }
        return result;
    }

    private IEnumerator FlashCo(GraphicInfo graphic, float duration) {
        var targetColor = Color.red;
        targetColor.a = graphic.initialColor.a;

        graphic.graphic.color = targetColor;
        // graphic.graphic.CrossFadeColor(graphic.initialColor, duration, true, false);
        // yield return new WaitForSeconds(duration);
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime) {
            graphic.graphic.color = Color.Lerp(targetColor, graphic.initialColor, t / duration);
            yield return null;
        }
        graphic.graphic.color = graphic.initialColor;
        graphic.coroutine = null;
    }

    [System.Serializable]
    public struct OfferItem {
        public Item item;
        public int count;
    }

    private struct GraphicInfo {
        private Graphic _graphic;
        public Graphic graphic {
            get => _graphic;
            set {
                _graphic = value;
                initialColor = (_graphic != null) ? _graphic.color : Color.white;
            }
        }
        public Color initialColor { get; private set; }

        public Coroutine coroutine;
    }
}
