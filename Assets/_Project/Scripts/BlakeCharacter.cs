using Unity.Mathematics;
using UnityEngine; 

public abstract class BlakeCharacter : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected int health = 1;

    public int Health
    {
        get => health;
        set
        {
            health = value;
            if (health < 1 && !isDead)
            {
                Die();
            }
        }
    }

    [SerializeField] 
    protected GameObject explosionParticle;

    [SerializeField] 
    protected Animator animator;

    protected GameObject explosionParticleInstantiated;
    protected int defaultHealth;
    protected float onDamageTakenCounter;
    protected bool isDead = false;
    protected Vector3 respawnPos;

    public delegate void OnDeath();
    public event OnDeath onDeath;
    public delegate void OnRespawn();
    public event OnRespawn onRespawn;

    private void Update()
    {
        if (onDamageTakenCounter > 0)
        {
            onDamageTakenCounter -= Time.deltaTime;
        }
    }

    public virtual void Die()
    {
        isDead = true;
        
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //GetComponent<CapsuleCollider>().enabled = false;
        onDeath?.Invoke();
    }

    protected virtual void DestroySelf()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(GameObject instigator, int damage)
    {
        if (onDamageTakenCounter > 0) return;
        if (health < 1) { return; }

        Debug.Log(instigator.name + " took " + damage + " damage to " + name);
        Health -= damage;
        onDamageTakenCounter = .5f;
    }

    public virtual bool CanTakeDamage(GameObject instigator)
    {
        BlakeCharacter other = instigator.GetComponent<BlakeCharacter>();
        if(other != null)
        {
            return GetType() != other.GetType();
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

    protected void Respawn()
    {
        onRespawn?.Invoke();
        isDead = false;
        //animator.SetBool("IsAlive", true); //only for DD we should only animate enemies
        Destroy(explosionParticleInstantiated);
        transform.position = respawnPos;
        gameObject.SetActive(true);
        //GetComponent<Rigidbody>().constraints -= RigidbodyConstraints.FreezePositionX;
        //GetComponent<Rigidbody>().constraints -= RigidbodyConstraints.FreezePositionZ;
        GetComponent<CapsuleCollider>().enabled = true;
        health = defaultHealth;
    }
}
