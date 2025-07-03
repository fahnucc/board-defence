using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private LevelData level1Data;
    [SerializeField] private LevelData level2Data;
    [SerializeField] private LevelData level3Data;

    private const string GameSceneName = "SampleScene";

    public void SelectLevel(LevelData levelData)
    {
        if (levelData == null) return;
        LevelManager.SelectedLevel = levelData;
        SceneManager.LoadScene(GameSceneName);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}