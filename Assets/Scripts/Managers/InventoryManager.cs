using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    private LevelData levelData;
    public Dictionary<DefenceItemData, int> itemCounts = new Dictionary<DefenceItemData, int>();

    public void Initialize(LevelData data)
    {
        this.levelData = data;

        itemCounts.Clear();
        foreach (var item in levelData.defenceItemInventory)
        {
            if (!itemCounts.ContainsKey(item.defenceItemType))
            {
                itemCounts.Add(item.defenceItemType, item.count);
            }
        }
    }

    public bool HasItem(DefenceItemData itemData)
    {
        return itemCounts.ContainsKey(itemData) && itemCounts[itemData] > 0;
    }

    public void UseItem(DefenceItemData itemData)
    {
        if (HasItem(itemData))
        {
            itemCounts[itemData]--;
        }
    }
}
