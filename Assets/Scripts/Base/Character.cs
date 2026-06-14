using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Character : MonoBehaviour
{

    //EVENTS
    public event Action OnDeath;            
    public event Action OnResurrection;

    //Health
    public bool isAlive { get; protected set; }

    [SerializeField] int maxHealth;
    float m_currentHealth;
    public float currentHealth
    {
        get => m_currentHealth;
        protected set
        {
            m_currentHealth = Mathf.Clamp(value, 0, maxHealth); // value between 0 - maxHealth

            if (m_currentHealth <= 0)
            {
                Die();
            }
        }
    }

    //Movement 
    public bool isMoving { get; private set; }
    float m_moveSpeed;
    protected float moveSpeed
    {
        get => m_moveSpeed;
        set
        {
            m_moveSpeed = Mathf.Max(value, 0);

            if (m_moveSpeed > 0) isMoving = true;
            else isMoving = false;
        }
    }
    

    //References
    protected CapsuleCollider capsuleCollider;


    //METHODS
    void Awake()
    {
        currentHealth = maxHealth;
        isAlive = true;
        capsuleCollider = GetComponent<CapsuleCollider>();
        Init();
    }

    protected virtual void Init() { }

    // Update is called once per frame
    void Update()
    {
        if (isAlive) HandleMovement();
        OnUpdate();
    }

    protected virtual void OnUpdate() { }

    public virtual void TakeDamage(float amount)
    {
        this.currentHealth -= amount;
    }

    public virtual void ModifySpeed(float multiplier)
    {
        moveSpeed *= multiplier;
    }
    protected virtual void Die()
    {
        if (!isAlive) return;
        isAlive = false;

        OnDeath?.Invoke();
    }

    protected abstract void HandleMovement();

    public virtual void burn(float duration, float dmgPerSec)
    {
        StartCoroutine(Burning(duration, dmgPerSec));
    }

    IEnumerator Burning(float duration, float dmgPerSec)
    {
        float elapsed = 0f;

        //take dmg every second for duration time
        while(elapsed < duration && isAlive)
        {
            TakeDamage(dmgPerSec);
            elapsed++;
            yield return new WaitForSeconds(1f);
        }
    }

    public virtual void Resurrect(float hpProcentage)
    {
        if (isAlive) return;

        isAlive = true;
        currentHealth = maxHealth * Mathf.Clamp01(hpProcentage); //value between 0 - 1

        OnResurrection?.Invoke();
    }
}
