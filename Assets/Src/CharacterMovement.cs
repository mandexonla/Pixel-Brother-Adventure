using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    bool isFacingRight = true;
    public ParticleSystem smokeFX;
    BoxCollider2D playerCollider;

    private GameManager gameManager;


    [Header("Movement")]
    public float moveSpeed = 5f;
    private float horizontalMovement;

    [Header("Dashing")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.1f;
    bool isDashing;
    bool canDash = true;
    TrailRenderer trailRenderer;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public int maxJump = 2;
    int jumpRemaining;

    [Header("GroundCheck")]
    public Transform groundCheckPosion;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrouneded;
    bool isOnPlatform;


    [Header("Gravity")]
    public float baseGravity = 5f;
    public float maxSpeedFall = 18f;
    public float fallSpeedMultiplayer = 2f;



    [Header("WallCheck")]
    public Transform wallCheckPosion;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;


    [Header("Wallmovement")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding;

    //Wall Jumping
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.2f;
    float wallJumpTimer;
    public Vector2 wallJumpForce = new Vector2(5f, 10f);

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (gameManager.IsGameOver() || gameManager.IsGameWin()) return;

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);
        animator.SetBool("isWallSliding", isWallSliding);

        if (isDashing)
        {
            return;
        }

        GroundCheck();
        ProcessGravity();
        ProcessWallSlide();
        ProcessWallJump();

        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
            Flip();
        }
    }


    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (context.performed && isGrouneded && isOnPlatform && playerCollider.enabled)
        {
            //Coroutine dropping
            StartCoroutine(DisablePlayerCollider(0.25f));
        }
    }

    private IEnumerator DisablePlayerCollider(float disableTime)
    {
        playerCollider.enabled = false;
        yield return new WaitForSeconds(disableTime);
        playerCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = true;
        }
        else if (collision.gameObject.CompareTag("Cup"))
        {
            //estroy(collision.gameObject);
            gameManager.GameWin();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = false;
        }
    }

    private IEnumerator DashCoroutine()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true);
        canDash = false;
        isDashing = true;
        trailRenderer.emitting = true;

        float dashDirection = isFacingRight ? 1f : -1f;

        rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y);//dash movement
        yield return new WaitForSeconds(dashDuration);

        rb.velocity = new Vector2(0f, rb.velocity.y);// reset horizontal veclocity

        isDashing = false;
        trailRenderer.emitting = false;
        Physics2D.IgnoreLayerCollision(7, 8, false);

        yield return new WaitForSeconds(dashDirection);
        canDash = true;

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpRemaining > 0)
        {
            if (context.performed)
            {
                //Hold jump button = higher jump
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpRemaining--;
                JumpFX();
            }
            else if (context.canceled)
            {
                //Ligh tap jump button = lower jump
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpRemaining--;
                JumpFX();
            }
        }

        //Wall Jump
        if (context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpForce.x, wallJumpForce.y); //Jump away from wall
            wallJumpTimer = 0f;
            animator.SetTrigger("jump");

            //Force flip 
            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1;
                transform.localScale = ls;
            }

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f); // wall Jump = 0.5f -- Jump again = 0.6f
            JumpFX();
        }

    }

    private void JumpFX()
    {
        animator.SetTrigger("jump");
        smokeFX.Play();
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPosion.position, groundCheckSize, 0, groundLayer))
        {
            jumpRemaining = maxJump;

            isGrouneded = true;
        }
        else
        {
            isGrouneded = false;
        }
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPosion.position, wallCheckSize, 0, wallLayer);
    }

    private void ProcessGravity()
    {
        //falling gravity
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplayer; //Fall increasingly faster
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxSpeedFall)); //max fall speed
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void ProcessWallSlide()
    {
        //Not grounded & On a wall & movement !=0
        if (!isGrouneded & WallCheck() && horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));//caps fall rate
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }


    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;

            if (rb.velocity.y == 0)
            {
                smokeFX.Play();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPosion.position, groundCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPosion.position, wallCheckSize);
    }
}
