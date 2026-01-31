using UnityEngine;

public class CamereFollow : MonoBehaviour
{
    [SerializeField] private Transform _tagetplayer;
    Vector3 _velocity = Vector3.zero;
    [Range(0f, 1f)]
    [SerializeField] private float _soomthtime;
    [SerializeField] private Vector3 _posoffset;
    [SerializeField] private Vector2 _Xlimit;
    [SerializeField] private Vector2 _Ylimit;
    private void Awake()
    {
        _tagetplayer = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void LateUpdate()
    {
        Vector3 _tagetposition = _tagetplayer.position + _posoffset;
        _tagetposition = new Vector3(Mathf.Clamp(_tagetposition.x, _Xlimit.x, _Xlimit.y), Mathf.Clamp(_tagetposition.y, _Ylimit.x, _Ylimit.y), -10);
        transform.position = Vector3.SmoothDamp(transform.position, _tagetposition, ref _velocity, _soomthtime);
        // velocity damp flow player 
    }
}
