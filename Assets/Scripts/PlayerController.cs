using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection
    {
        left, right
    }

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    private FacingDirection facingDirection = FacingDirection.left;     //Sets the facing direction to left as default. 

    private bool grounded = false;      //grounded variable
    private bool lastGrounded = false;

    [Header("Jump Settings")]
    public float jumpHeight = 3f;
    public float jumpDuration = 0.5f;

    private bool isJumping = false;
    private float jumpTime = 0f;
    private float jumpInitialVelocity = 0f;
    private float jumpGravity = 0f;
    private float originalGravityScale = 1f;

    [Header("Fall Settings")]
    public float terminalSpeed = -20;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(rb == null )
        {
            Debug.LogWarning("rigidbody is missing");           
        }
        else
        {
            originalGravityScale = rb.gravityScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector2 playerInput = new Vector2(horizontalInput, 0f);

        MovementUpdate(playerInput);

        if (Input.GetKeyDown(KeyCode.Space) && grounded && !isJumping && rb != null)
        {
            StartJump();
        }

        if (grounded != lastGrounded)
        {
            if (!grounded)
            {
                Debug.Log("Player is not grounded");
            }
            lastGrounded = grounded;
        }
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        if (isJumping )
        {
            jumpTime += Time.fixedDeltaTime;

            Vector2 vel = rb.linearVelocity;
            vel.y += jumpGravity * Time.fixedDeltaTime;
            rb.linearVelocity = vel;

            if (jumpTime >= jumpDuration)
            {
                EndJump();
            }
        }
        if (rb.linearVelocity.y < terminalSpeed)
        {
            Vector2 v = rb.linearVelocity;
            v.y = terminalSpeed;   
            rb.linearVelocity = v;
        }
    }
    private void MovementUpdate(Vector2 playerInput)
    {
        if (rb == null) return;

        float targetHorizontalVelocity = playerInput.x * moveSpeed;

        rb.linearVelocity = new Vector2 (targetHorizontalVelocity, rb.linearVelocity.y);        //linear velocity is what the system suggested me to use, otherwise it won't accept velocity by itself.

        if (playerInput.x > 0)
        {
            facingDirection = FacingDirection.right;
        }
        else if (playerInput.x < 0)
        {
            facingDirection = FacingDirection.left;
        }
    }

    private void StartJump()
    {
        isJumping = true;
        grounded = false;
        jumpTime = 0f;

        rb.gravityScale = 0f;       //Disable gravity

        jumpInitialVelocity = (2f * jumpHeight) / jumpDuration;                 
        jumpGravity = (-2f * jumpHeight) / (jumpDuration * jumpDuration);

        Vector2 vel = rb.linearVelocity;
        vel.y = jumpInitialVelocity;
        rb.linearVelocity = vel;
    }

    private void EndJump()
    {
        isJumping = false;
        rb.gravityScale = originalGravityScale;
    }

    public bool IsWalking()
    {
        return Mathf.Abs(rb.linearVelocity.x) > 0.01f;
    }
    public bool IsGrounded()
    {
        return grounded;
    }

    public FacingDirection GetFacingDirection()
    {
        return facingDirection;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            grounded = true;

            if (isJumping)
            {
                EndJump();
            }
        }    
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}
