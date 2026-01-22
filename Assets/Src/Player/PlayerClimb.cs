using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClimb : MonoBehaviour
{
    [SerializeField] private float speedClimb = 4f;
    [SerializeField] private Rigidbody2D rb;

    private bool isLadder;
    private int verticalInput;
    private float defaultGravity;
    private bool isClimbing = false;
    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
    }

    public void OnClimb(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isClimbing = true;
        }
        else if (context.canceled)
        {
            isClimbing = false;
        }
        verticalInput = isClimbing ? 1 : 0;

    }

    private void FixedUpdate()
    {
        bool isClimbing_Fixupdate = isLadder && Mathf.Abs(verticalInput) > 0.01f;

        if (isClimbing_Fixupdate)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * speedClimb);
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isLadder = true;
            verticalInput = isClimbing ? 1 : 0;
            Debug.Log("Enter Ladder");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isLadder = false;
            verticalInput = 0;
            rb.gravityScale = defaultGravity;
            Debug.Log("Exit Ladder");
        }
    }
}
