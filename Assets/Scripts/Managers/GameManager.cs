using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private LevelData levelData;
    public GridManager gridManager;
    public SpawnManager spawnManager;
    public InventoryManager inventoryManager;
    public UIManager uiManager;
    public InputManager inputManager;
    public ObjectPool objectPool;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        Time.timeScale = 1f;

        if (LevelManager.SelectedLevel != null)
        {
            levelData = LevelManager.SelectedLevel;
        }

        objectPool.Initialize(levelData);
        gridManager.Initialize(levelData);
        spawnManager.Initialize(levelData, gridManager.GetSpawnPositions(), gridManager.GetCellSize());
        inventoryManager.Initialize(levelData);
        uiManager.Initialize(levelData, inventoryManager);
        inputManager.Initialize(uiManager);
    }

    public void GameOver(bool hasWon = false)
    {
        if (isGameOver) return;

        isGameOver = true;

        Time.timeScale = 0f;

        uiManager.ShowGameOverPopup(hasWon);
    }
}