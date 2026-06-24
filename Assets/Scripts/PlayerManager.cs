using UnityEngine;

public class PlayerManager : Character
{


    public override void attack()
    {
        
    }

    public override void HandleRotation(Vector3 lookTarget)
    {
        Vector3 direction = lookTarget - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = targetRotation;
        }
    }

    protected override void Init()
    {
       
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
