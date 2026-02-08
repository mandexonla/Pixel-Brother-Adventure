using UnityEngine;

public class FiretrapController : MonoBehaviour
{
    [SerializeField] float _maxTime;
    [SerializeField] float _startDelay;

    BoxCollider2D _collider;
    Animator _anim;
    float _currentTime;
    private bool _isFiring = false;
    public bool IsFiring => _isFiring;
    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad < _startDelay) return;
        _currentTime += Time.deltaTime;
        if (_currentTime > _maxTime)
        {
            _anim.SetBool("IsFire", true);
            _anim.SetBool("PreFire", false);
            _collider.enabled = true;
            _isFiring = true;
            _currentTime = 0f;
        }
        else if (_currentTime > _maxTime / 1.18)
        {
            _anim.SetBool("PreFire", true);
        }
        else if (_currentTime > _maxTime / 3)
        {
            _anim.SetBool("IsFire", false);
            _collider.enabled = false;
            _isFiring = false;
        }
    }

}
