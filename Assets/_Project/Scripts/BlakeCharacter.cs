using UnityEngine;

public class BlakeCharacter : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 1;
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

    public void Die()
    {
        animator.SetBool("IsAlive", false);

        onDeath?.Invoke();

        Invoke("DestroySelf", 5f);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(GameObject instigator, int damage)
    {
        if (health < 1) { return; }

        Debug.Log(instigator.name + " took " + damage + " damage to " + name);
        Health -= damage;
    }
}
