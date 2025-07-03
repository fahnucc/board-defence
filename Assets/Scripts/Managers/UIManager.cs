using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    private LevelData levelData;
    private InventoryManager inventoryManager;

    [SerializeField] private GameObject placementPopup;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject defenceButtonPrefab;

    [SerializeField] private GameObject gameOverPopup;
    [SerializeField] private TextMeshProUGUI gameOverTitle;
    private Cell selectedCell;
    public bool IsPlacementPopupOpen => placementPopup.activeSelf;

    public void Initialize(LevelData data, InventoryManager inventory) { this.levelData = data; this.inventoryManager = inventory; }

    void Start()
    {
        gameOverPopup.SetActive(false);
    }

    public void ShowGameOverPopup(bool hasWon)
    {
        HidePlacementPopup();
        
        if (hasWon)
        {
            gameOverTitle.text = "You Win";
        }
        else
        {
            gameOverTitle.text = "Game Over";
        }

        gameOverPopup.SetActive(true);

    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene"); 
    }

    public void ShowPlacementPopup(Cell targetCell)
    {
        if (gameOverPopup.activeSelf) return;
        
        selectedCell?.UpdateVisuals();

        foreach (Transform child in buttonContainer) { Destroy(child.gameObject); }

        foreach (var itemInfo in inventoryManager.itemCounts)
        {
            GameObject button = Instantiate(defenceButtonPrefab, buttonContainer);
            BuildDefenceItemButton buttonUI = button.GetComponent<BuildDefenceItemButton>();

            buttonUI.Initialize(itemInfo.Key, itemInfo.Value, this);
        }

        selectedCell = targetCell;
        selectedCell.UpdateVisuals(true);
        placementPopup.SetActive(true);
    }

    public void OnPlacementButtonClicked(DefenceItemData selectedData)
    {
        if (selectedCell == null) return;
    
        if (inventoryManager.HasItem(selectedData))
        {
            inventoryManager.UseItem(selectedData);
            selectedCell.PlaceItem(selectedData.prefab, selectedData);
        }

        HidePlacementPopup();
    }

    public void HidePlacementPopup()
    {
        selectedCell?.UpdateVisuals();
        selectedCell = null;
        placementPopup.SetActive(false);
    }
}
