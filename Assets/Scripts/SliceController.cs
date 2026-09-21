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
    public GameObject JuicePrefab;
    public int JuiceAmount;

    SpriteRenderer spriteRenderer;
    private int juiceToSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        juiceToSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (juiceToSpawn > 0)
        {
            Instantiate(JuicePrefab, transform.position + Vector3.down * Random.Range(0.5f, 1.5f), Quaternion.identity);
            juiceToSpawn--;
        }
    }

    public void Drill()
    {
        Color color = spriteRenderer.color;
        color.a -= DrillStep;
        juiceToSpawn += JuiceAmount;
        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
        spriteRenderer.color = color;
    }
}
