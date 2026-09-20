using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public float JumpForce;
    public DrillController Drill;
    public Transform GroundCheck;
    public Vector2 BoxSize;
    public LayerMask GroundLayer;


    Rigidbody2D rb;
    InputAction moveAction;
    InputAction jumpAction;
    SliceController currentSlice;

    bool isGrounded;
    bool isJumpRequested;
    bool isDrilling;
    float originalDrag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        originalDrag = rb.linearDamping;
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
        if (isDrilling && currentSlice != null)
        {
            if (rb.linearVelocity.y < -currentSlice.PlayerMaxDrillSpeed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -currentSlice.PlayerMaxDrillSpeed);
            }
        }
    }

    public void StartDrilling(SliceController slice)
    {
        isDrilling = true;
        currentSlice = slice;
        rb.linearDamping = slice.PlayerDrag;
    }

    public void StopDrilling()
    {
        isDrilling = false;
        currentSlice = null;
        rb.linearDamping = originalDrag;
    }

    void OnDrawGizmosSelected()
    {
        if (GroundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(GroundCheck.position, BoxSize);
    }
}
