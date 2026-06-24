using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))]
public class PlayerInputController : MonoBehaviour
{

    public GameInput input;
    Character character;
    LayerMask groundLayer;
    Vector3 lookTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        input = new GameInput();
        character = GetComponent<Character>();
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();

        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        character.HandleMovement(moveDirection);

        character.HandleRotation(lookTarget);
    }

    private void Update()
    {
        SetLookRotation();
        
    }

    void SetLookRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer ))
        {
            lookTarget = hit.point;
        }
    }
}
