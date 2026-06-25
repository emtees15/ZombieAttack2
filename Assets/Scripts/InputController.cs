using UnityEngine;
using UnityEngine.InputSystem;


public class InputController : Controller
{

    public GameInput input;

    protected override void Init()
    {
        input = new GameInput();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    protected override Vector3 SetMoveDirection()
    {
        Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();

        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        return moveDirection;
    }

    protected override Vector3 SetLookDirection()
    {
        Vector3 lookTarget = new Vector3(0, 0, 0);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            lookTarget = hit.point;
        }

        Vector3 lookDirection = lookTarget - transform.position;
        lookDirection.y = 0;

        return lookDirection;
    }

    protected override void HandleActions()
    {
        if (input.Player.Attack.triggered) character.Attack();
        if (input.Player.Interact.triggered) character.Interact();
        if (input.Player.ChangeWeapon.triggered) character.ChangeWeapon();
        if (input.Player.Melee.triggered) character.Melee();
        if (input.Player.Reload.triggered) character.Reload();
    }
}
