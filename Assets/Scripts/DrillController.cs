using UnityEngine;
using UnityEngine.InputSystem;

public class DrillController : MonoBehaviour
{
    public PlayerController player;

    InputAction jumpAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out SliceController slice))
        {
            player.StartDrilling(slice);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out SliceController slice))
        {
            player.StopDrilling();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out SliceController sliceController) && jumpAction.IsPressed())
        {
            sliceController.Drill();
        }
    }
}
