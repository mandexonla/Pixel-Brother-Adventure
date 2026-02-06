using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField] public TextMeshProUGUI _countitemMelon;
    [SerializeField] public TextMeshProUGUI _countitemBanana;
    [SerializeField] public TextMeshProUGUI _countitemPineapple;

    public int _countMelon = 0;
    public int _countBanana = 0;
    public int _countPineapple = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Melon"))
        {
            _countMelon++;
            _countitemMelon.text = "X" + _countMelon;
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Banana"))
        {
            _countBanana++;
            _countitemBanana.text = "X" + _countBanana;
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Pineapple"))
        {
            _countPineapple++;
            _countitemPineapple.text = "X" + _countPineapple;
            Destroy(collision.gameObject);
        }

    }
}