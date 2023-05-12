using UnityEngine; 

public class BlakeCharacter : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 1;
    [SerializeField] private bool isPlayer;
    protected bool isDead = false;
    protected Vector3 respawnPos;
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
    public delegate void OnRespawn();
    public event OnRespawn onRespawn;

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        animator.SetBool("IsAlive", false);

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<CapsuleCollider>().enabled = false;
        onDeath?.Invoke();

        if(isPlayer)
            Invoke("Respawn", 5f);
        else
            Invoke("DestroySelf", 2f);
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

    public void SetRespawnPosition(Vector3 position)
    {
        respawnPos = position;
    }
    public Vector3 GetRespawnPosition()
    {
        return respawnPos;
    }

    private void Respawn()
    {
        onRespawn?.Invoke();
        isDead = false;
        animator.SetBool("IsAlive", true);
        transform.position = respawnPos;
        GetComponent<Rigidbody>().constraints -= RigidbodyConstraints.FreezePositionX;
        GetComponent<Rigidbody>().constraints -= RigidbodyConstraints.FreezePositionZ;
        GetComponent<CapsuleCollider>().enabled = true;
        health = 1;
    }
}
