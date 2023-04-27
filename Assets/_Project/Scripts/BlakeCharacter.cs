using UnityEngine;

public class BlakeCharacter : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 1;
    protected bool isDead = false;
    public int Health 
    { 
        get
        {
            return health;
        }

        set
        {
            health = value;
            if (health < 1)
            {
                Die();
            }
        }
    }

    private Animator animator;

    public delegate void OnDeath();
    public event OnDeath onDeath;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        //animator.SetBool("IsAlive", false);

        onDeath?.Invoke();

        Invoke("DestroySelf", 0.15f);
    }

    protected virtual void DestroySelf()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(GameObject instigator, int damage)
    {
        if (health < 1) { return; }

        Debug.Log(instigator.name + " took " + damage + " damage to " + name);
        Health -= damage;
    }
}
