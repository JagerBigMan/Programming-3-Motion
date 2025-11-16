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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(rb == null )
        {
            Debug.LogWarning("rigidbody is missing");           
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector2 playerInput = new Vector2(horizontalInput, 0f);

        MovementUpdate(playerInput);

        if (grounded != lastGrounded)
        {
            if (!grounded)
            {
                Debug.Log("Player is not grounded");
            }
            lastGrounded = grounded;
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
