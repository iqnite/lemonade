using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SliceController : MonoBehaviour
{
    [Tooltip("Higher drill step means more of the slice is removed each frame.")]
    public float DrillStep;
    [Tooltip("Higher drag means slower movement through the slice.")]
    public float PlayerDrag;
    [Tooltip("Caps maximum drilling speed.")]
    public float PlayerMaxDrillSpeed;

    SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Drill()
    {
        Color color = spriteRenderer.color;
        color.a -= DrillStep;
        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
        spriteRenderer.color = color;
    }
}
