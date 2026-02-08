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
            // Sửa thêm: Kiểm tra active để an toàn tuyệt đối
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
            // SỬA LỖI: Chỉ chạy lệnh gỡ cha nếu object này ĐANG HOẠT ĐỘNG
            if (this.gameObject.activeInHierarchy)
            {
                StartCoroutine(SettingPanelDelayed(collision.transform, null));
            }
            // Nếu activeInHierarchy == false (đang tắt), ta bỏ qua để tránh lỗi SetParent
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