using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public float JumpForce;
    public Transform GroundCheck;
    public Vector2 BoxSize;
    public LayerMask GroundLayer;

    Rigidbody2D rb;
    InputAction moveAction;
    InputAction jumpAction;

    bool isGrounded;
    bool isJumpRequested;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float moveDirection = moveInput.x;
        rb.linearVelocity = new Vector2(moveDirection * MoveSpeed, rb.linearVelocity.y);
        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            isJumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(GroundCheck.position, BoxSize, 0f, GroundLayer);
        if (isJumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
            isJumpRequested = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (GroundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(GroundCheck.position, BoxSize);
    }
}
