using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public EnemyData enemyType;
    public int count;
}

[System.Serializable]
public class DefenceItemInventory
{
    public DefenceItemData defenceItemType;
    public int count;
}


[CreateAssetMenu(fileName = "LevelData", menuName = "Data/Level")]
public class LevelData : ScriptableObject
{
    public string levelName = "New Level";
    public float spawnDelay = 1.5f;
    public EnemySpawnInfo[] enemyWaves;
    public DefenceItemInventory[] defenceItemInventory;
    public int gridWidth = 4;
    public int gridHeight = 8;
    public int placeableMinRow = 0;
    public int placeableMaxRow = 3;
}
