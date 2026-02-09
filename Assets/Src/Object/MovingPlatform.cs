using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;

    private Vector3 nextPosition;

    void Start()
    {
        nextPosition = pointB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

        if (transform.position == nextPosition)
        {
            nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (this.gameObject.activeInHierarchy)
            {
                StartCoroutine(SettingPanelDelayed(collision.transform, transform));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (this.gameObject.activeInHierarchy)
            {
                StartCoroutine(SettingPanelDelayed(collision.transform, null));
            }
        }
    }

    private IEnumerator SettingPanelDelayed(Transform child, Transform parent)
    {
        yield return null;
        if (child != null)
        {
            child.parent = parent;
        }
    }
}