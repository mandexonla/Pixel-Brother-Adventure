using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item _item = collision.GetComponent<Item>();

        if (_item != null)
        {
            _item.Collect();
        }
    }
}
