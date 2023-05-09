using UnityEngine;

public class BlakeCharacter : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 1;
    [SerializeField] private bool isPlayer;
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

    [SerializeField] private Animator animator;

    public delegate void OnDeath();
    public event OnDeath onDeath;

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        animator.SetBool("IsAlive", false);

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        onDeath?.Invoke();

        if (!isPlayer) Invoke("DestroySelf", 5f);
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

    public virtual bool CanTakeDamage(GameObject instigator)
    {
        BlakeCharacter other = instigator.GetComponent<BlakeCharacter>();
        if(other != null)
        {
            return isPlayer != other.isPlayer;
        }

        return true;
    }
}
