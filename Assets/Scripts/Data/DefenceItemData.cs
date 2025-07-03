using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public enum AttackDirection { Forward, All }

[CreateAssetMenu(fileName = "DefenceItemData", menuName = "Data/DefenceItem")]
public class DefenceItemData : ScriptableObject
{
    // damage: 3, range: 4 blocks, interval: 3s, direction: forward
    public float damage;
    public int range;
    public float interval;
    public AttackDirection direction;
    public GameObject prefab;
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;
    public Sprite image;
}
