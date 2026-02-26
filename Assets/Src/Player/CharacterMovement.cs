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

    [Header("Movement")]
    public float moveSpeed = 5f;
    private float horizontalMovement;

    [Header("Dashing")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.1f;
    bool isDashing;
    bool canDash = true;

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
    bool isOnTrap;

    [Header("Trap")]
    [Range(0f, 1f)]
    public float trapSpeedMultiplier = 0.4f; // Speed ​​while standing on the trap


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

    //running
    bool isRunning => Mathf.Abs(rb.linearVelocity.x) > 0.1f;

    //interact
    private GameObject _currentInteractableObject;


    private void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver() || GameManager.Instance.IsGameWin()) return;

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
            float currentSpeed = isOnTrap ? moveSpeed * trapSpeedMultiplier : moveSpeed;
            rb.linearVelocity = new Vector2(horizontalMovement * currentSpeed, rb.linearVelocity.y);
            Flip();
        }
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isWallSliding", isWallSliding);
        animator.SetBool("isRunning", isRunning);
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

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (_currentInteractableObject == null) return;

        IInteractable interactable =
            _currentInteractableObject.GetComponent<IInteractable>();

        if (interactable != null)
        {
            interactable.Interact();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("InteractableObject"))
        {
            _currentInteractableObject = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        _currentInteractableObject = null;
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
            GameManager.Instance.GameWin();
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

        float dashDirection = isFacingRight ? 1f : -1f;

        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y);//dash movement
        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);// reset horizontal veclocity

        isDashing = false;
        Physics2D.IgnoreLayerCollision(7, 8, false);

        yield return new WaitForSeconds(dashDirection);
        canDash = true;

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpRemaining > 1)
        {
            animator.SetBool("jump", isGrouneded);
            if (context.performed)
            {
                //Hold jump button = higher jump
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                animator.SetBool("doubleJump", !isGrouneded);
                jumpRemaining--;
                ;
                SoundEffectManager.Play("PlayerJump");
                SoundEffectManager.Play("PlayerDoubleJump");
            }
            else if (context.canceled)
            {
                //Ligh tap jump button = lower jump
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);

                SoundEffectManager.Play("PlayerJump");
                animator.SetTrigger("jump");
                SoundEffectManager.Play("PlayerDoubleJump");
            }
        }


        //Wall Jump
        if (context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpForce.x, wallJumpForce.y); //Jump away from wall
            wallJumpTimer = 0f;

            //Force flip 
            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1;
                transform.localScale = ls;
            }

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f); // wall Jump = 0.5f -- Jump again = 0.6f
        }

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

        //When standing on a trap: act as if you are standing on the ground
        //reset jumpRemaining every frame to jump multiple times
        if (isOnTrap)
        {
            jumpRemaining = maxJump;
            isGrouneded = true;
        }
    }

    public void SetOnTrap(bool value)
    {
        isOnTrap = value;
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPosion.position, wallCheckSize, 0, wallLayer);
    }

    private void ProcessGravity()
    {
        //falling gravity
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplayer; //Fall increasingly faster
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxSpeedFall)); //max fall speed
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
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));//caps fall rate
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
