using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Controller))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
public abstract class Character : MonoBehaviour
{

    //EVENTS
    public event Action OnDeath;            
    public event Action OnResurrection;

    //Health
    public bool isAlive { get; protected set; }

    [SerializeField] float maxHealth;
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
    protected CharacterController CharController;
    public bool isMoving { get; private set; }
    [SerializeField] float defaultSpeed = 5f;
    [SerializeField] float m_moveSpeed;
    protected float moveSpeed
    {
        get => m_moveSpeed;
        set
        {
            m_moveSpeed = Mathf.Max(value, 0); // value min 0

            if (m_moveSpeed > 0) isMoving = true;
            else isMoving = false;
        }
    }
    

    //References
    protected CapsuleCollider capsuleCollider;
    protected Animator animator;
    protected AudioSource audioSource;
    [SerializeField] AudioClip moveClip;
    [SerializeField] AudioClip dieClip;
    [SerializeField] AudioClip getDmgClip;
    [SerializeField] AudioClip healClip;


    //METHODS
    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        CharController = GetComponent<CharacterController>();

        moveSpeed = defaultSpeed;
        audioSource.clip = moveClip;
        currentHealth = maxHealth;
        isAlive = true;

        Init();
    }

    protected abstract void Init();

    // Update is called once per frame
    void Update()
    {
       
    }

    public virtual void TakeDamage(float amount)
    {
        if (!isAlive) return;

        this.currentHealth -= amount;
        audioSource.PlayOneShot(getDmgClip);
    }

    public virtual void Heal(float amount)
    {
        if (!isAlive) return;

        this.currentHealth += amount;
        audioSource.PlayOneShot(healClip);
    }

    public void ModifySpeed(float multiplier)
    {
        moveSpeed *= multiplier;
    }
    public void ResetSpeed(float multiplier)
    {
        moveSpeed = defaultSpeed;
    }

    protected virtual void Die()
    {
        if (!isAlive) return;
        isAlive = false;
        audioSource.PlayOneShot(dieClip);
        OnDeath?.Invoke();
    }

    public virtual void HandleMovement(Vector3 direction)
    {
        if (direction.magnitude > 0.1f)
        {

            if (isAlive)
            {
                isMoving = true;
                CharController.Move(direction * m_moveSpeed * Time.deltaTime);
                if (!audioSource.isPlaying) audioSource.Play();
                animator.SetBool("isMoving", isMoving);
                
            }
            
        }
        else
        {
            isMoving = false;
            animator.SetBool("isMoving", isMoving);
            if (audioSource.isPlaying) audioSource.Stop();
            
        }
        animator.SetFloat("speed", direction.magnitude * m_moveSpeed);

    }

    public virtual void HandleRotation(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = targetRotation;
        }
    }

    public abstract void Attack();
    public abstract void Interact();
    public abstract void Melee();

    public abstract void Reload();

    public abstract void ChangeWeapon();

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
