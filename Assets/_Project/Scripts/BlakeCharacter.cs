using Unity.Mathematics;
using UnityEngine; 

public class BlakeCharacter : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 1;
    [SerializeField] private bool isPlayer;
    [SerializeField] private GameObject explosionParticle;
    private GameObject explosionParticleInstantiated;
    private int defaultHealth;
    private float onDamageTakenCounter;
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

    private void Awake()
    {
        if (!isPlayer) return;
        defaultHealth = SceneHandler.Instance.isNormalDifficulty ? 3 : 1;
        health = defaultHealth;
    }

    private void Update()
    {
        if (onDamageTakenCounter > 0)
        {
            onDamageTakenCounter -= Time.deltaTime;
        }
    }

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        if(!isPlayer) //only for DD we should only animate enemies
            animator.SetBool("IsAlive", false);
        
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<CapsuleCollider>().enabled = false;
        onDeath?.Invoke();

        if (isPlayer)
        {
            explosionParticleInstantiated = Instantiate(explosionParticle,transform.position,quaternion.identity);//DD
            gameObject.SetActive(false); //DD
            Invoke("Respawn", 2f);
        }
        else
        {
            Invoke("DestroySelf", 2f);
        }
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
        //animator.SetBool("IsAlive", true); //only for DD we should only animate enemies
        Destroy(explosionParticleInstantiated);
        transform.position = respawnPos;
        gameObject.SetActive(true);
        GetComponent<Rigidbody>().constraints -= RigidbodyConstraints.FreezePositionX;
        GetComponent<Rigidbody>().constraints -= RigidbodyConstraints.FreezePositionZ;
        GetComponent<CapsuleCollider>().enabled = true;
        health = defaultHealth;
    }
}
