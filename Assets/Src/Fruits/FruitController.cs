using UnityEngine;

public class FruitController : MonoBehaviour, Item
{
    [SerializeField] Fruits _fruitType;
    //private Animator FruitAminator;
    bool isCollected;


    //private void Awake()
    //{
    //    FruitAminator = GetComponent<Animator>();
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCollected)
        {

            FruitManager.Instance.IncreaseFruitNumber(_fruitType);
            //FruitAminator.Play("Collected");
            isCollected = true;
            Destroy(this.gameObject, 0.5f);

        }
    }
    public void Collect()
    {
        SoundEffectManager.Play("Fruit");
        Destroy(gameObject);
    }
}
