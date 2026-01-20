using System;
using UnityEngine;

public class Fruit : MonoBehaviour, Item
{
    public static event Action<int> OnFruitCollect;
    public int worth = 5;
    public void Collect()
    {
        OnFruitCollect?.Invoke(worth);
        SoundEffectManager.Play("Fruit");
        Destroy(gameObject);
    }
}
