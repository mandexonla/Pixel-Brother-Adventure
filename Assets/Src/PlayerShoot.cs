using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefabs;
    public float bulletSpeed = 50f;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (gameManager != null && gameManager.IsGameWin() || gameManager.IsGameOver()) return;

        if (Input.GetMouseButtonDown(0))//left click
        {
            Shoot();
            SoundEffectManager.Play("BulletShoot");
        }
    }
    void Shoot()
    {
        //get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //direction from player to mouse position
        Vector3 shootDirection = (mousePosition - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefabs, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootDirection.x, shootDirection.y) * bulletSpeed;
        Destroy(bullet, 2f);
    }

}
