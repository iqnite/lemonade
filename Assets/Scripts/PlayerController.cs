using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public float JumpForce;
    public DrillController Drill;
    public Transform GroundCheck;
    public Vector2 BoxSize;
    public LayerMask GroundLayer;

    [Header("Buoyancy Settings")]
    [Tooltip("How strongly liquids pushes the player up.")]
    public float BuoyancyMultiplier;
    [Tooltip("How much liquids slows down the player's movement.")]
    public float WaterDrag;
    public LayerMask WaterLayer;

    Rigidbody2D rb;
    Collider2D col;
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
        col = GetComponent<Collider2D>();
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
        Collider2D[] overlappingWater = Physics2D.OverlapBoxAll(
            col.bounds.center,
            col.bounds.size,
            0f,
            WaterLayer
        );
        if (overlappingWater.Length > 0)
        {
            float highestWaterY = float.MinValue;
            foreach (Collider2D water in overlappingWater)
            {
                if (water.transform.position.y > highestWaterY)
                {
                    highestWaterY = water.transform.position.y;
                }
            }
            float playerBottomY = col.bounds.min.y;
            float submergeDepth = highestWaterY - playerBottomY;
            if (submergeDepth > 0)
            {
                float upwardForce = submergeDepth * BuoyancyMultiplier;
                rb.AddForce(Vector2.up * upwardForce, ForceMode2D.Force);
            }
            rb.linearDamping = WaterDrag;
        }
        else
        {
            rb.linearDamping = originalDrag;
        }

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
