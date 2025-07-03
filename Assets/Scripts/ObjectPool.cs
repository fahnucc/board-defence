using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    private Dictionary<string, List<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize(LevelData levelData)
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();
        Dictionary<string, GameObject> prefabsToPool = new Dictionary<string, GameObject>();
        Dictionary<string, int> poolSizes = new Dictionary<string, int>();

        foreach (var wave in levelData.enemyWaves)
        {
            if (wave.enemyType.prefab == null) continue;

            string tag = wave.enemyType.prefab.name;
            if (!prefabsToPool.ContainsKey(tag))
            {
                prefabsToPool.Add(tag, wave.enemyType.prefab);
                poolSizes.Add(tag, 0);
            }
            poolSizes[tag] += wave.count;
        }

        foreach (var item in levelData.defenceItemInventory)
        {
            if (item.defenceItemType.projectilePrefab != null)
            {
                string tag = item.defenceItemType.projectilePrefab.name;
                if (!prefabsToPool.ContainsKey(tag))
                {
                    prefabsToPool.Add(tag, item.defenceItemType.projectilePrefab);
                    poolSizes.Add(tag, 0);
                }
                poolSizes[tag] += 5;
            }
        }
        

        foreach (var pair in prefabsToPool)
        {
            string tag = pair.Key;
            GameObject prefab = pair.Value;
            int size = poolSizes[tag];

            List<GameObject> objectPool = new List<GameObject>();
            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                objectPool.Add(obj);
            }
            poolDictionary.Add(tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag)) return null;

        List<GameObject> pool = poolDictionary[tag];
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                return obj;
            }
        }
        
        GameObject newObj = Instantiate(pool[0]);
        pool.Add(newObj);
        newObj.SetActive(true);
        newObj.transform.position = position;
        newObj.transform.rotation = rotation;
        return newObj;
    }
}