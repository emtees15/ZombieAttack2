using UnityEngine;

[RequireComponent(typeof(Character))]
public abstract class Controller : MonoBehaviour
{

    protected Character character;
    protected LayerMask groundLayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        character = GetComponent<Character>();
        groundLayer = LayerMask.GetMask("Ground");
        Init();

    }

    protected abstract void Init();
    // Update is called once per frame
    void FixedUpdate()
    {
        character.HandleMovement(SetMoveDirection());

        character.HandleRotation(SetLookDirection());

        
    }

    private void Update()
    {
        HandleActions();
    }

    protected abstract Vector3 SetMoveDirection();

    protected abstract Vector3 SetLookDirection();

    protected abstract void HandleActions();


}
