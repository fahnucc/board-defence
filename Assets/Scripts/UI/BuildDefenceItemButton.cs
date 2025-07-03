using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildDefenceItemButton : MonoBehaviour
{
    public Button button;
    public Image iconImage;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI intervalText;
    public TextMeshProUGUI directionText;
    private DefenceItemData itemData;

    public void Initialize(DefenceItemData data, int count, UIManager uiManager)
    {
        this.itemData = data;

        if (data.image != null)
        {
            iconImage.sprite = data.image;
        }

        countText.text = "x" + count;
        button.interactable = count > 0;

        damageText.text = "Damage: " + data.damage.ToString();
        rangeText.text = "Range: " + data.range.ToString();
        intervalText.text = "Interval: " + data.interval.ToString();
        directionText.text = "Direction: " + ((AttackDirection)data.direction).ToString();
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => uiManager.OnPlacementButtonClicked(itemData));
    }
}