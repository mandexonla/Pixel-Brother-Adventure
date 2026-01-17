using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    public float chaseSpeed = 2f;
    public float jumpForce = 2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrouded;
    private bool shouldJump;

    public int damage = 1;
    public int maxHealth = 3;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color ogcolor;

    //LootTable 
    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        ogcolor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        //IsGrounded?
        isGrouded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        //player direction
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        //player above detection
        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 5f, 1 << player.gameObject.layer);

        if (isGrouded)
        {
            //chase player
            rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);

            //Jump if there's gap  ahead && no ground infront
            // else if there's player above and platform above

            //If ground
            RaycastHit2D groundInfront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);
            //If gap
            RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 2f, groundLayer);
            //If platform above
            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, groundLayer);

            if (!groundInfront.collider && !gapAhead.collider)
            {
                shouldJump = true;
            }
            else if (isPlayerAbove && platformAbove.collider)
            {
                shouldJump = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isGrouded && shouldJump)
        {
            shouldJump = false;
            Vector2 direction = (player.position - transform.position).normalized;

            Vector2 jumpDirection = direction * jumpForce;
            rb.AddForce(new Vector2(jumpDirection.x, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashWhite());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashWhite()
    {
        ogcolor = spriteRenderer.color;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = ogcolor;
    }

    private void Die()
    {
        //go around lootable
        //spawn item 
        foreach (LootItem lootitem in lootTable)
        {
            if (Random.Range(0f, 100f) <= lootitem.dropChance)
            {
                Instantiate(lootitem.itemPrefabs);
            }
            break;
        }
        Destroy(gameObject);
    }

    void InstantiateLoot(GameObject loot)
    {
        if (loot)
        {
            GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);

            droppedLoot.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
