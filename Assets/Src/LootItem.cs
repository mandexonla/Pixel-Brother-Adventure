using UnityEngine;

[System.Serializable]
public class LootItem
{
    public GameObject itemPrefabs;
    [Range(0, 100)] public float dropChance;
}
