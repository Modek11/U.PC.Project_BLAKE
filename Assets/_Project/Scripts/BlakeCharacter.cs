using System;
using System.Collections;
using UnityEngine;

public abstract class BlakeCharacter : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected int health = 1;

    [SerializeField] 
    protected float timeBetweenDamages = .5f;

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

#if UNITY_EDITOR
    [SerializeField]
    private bool godMode;
#endif

    protected GameObject explosionParticleInstantiated;
    protected int defaultHealth;
    protected bool recentlyDamaged = false;
    protected bool isDead = false;
    protected Vector3 respawnPos;

    public delegate void OnDeath();
    public event OnDeath onDeath;
    public delegate void OnRespawn();
    public event OnRespawn onRespawn;
    public event Action OnDamageTaken;

    public virtual void Die()
    {
        isDead = true;
        onDeath?.Invoke();
    }

    protected virtual void DestroySelf()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(GameObject instigator, int damage)
    {
#if UNITY_EDITOR
        if (godMode) return;
#endif
        if (recentlyDamaged) return;
        if (health < 1) { return; }

        Debug.Log(instigator.name + " took " + damage + " damage to " + name);
        Health -= damage;
        
        if (health > 0)
        {
            StartCoroutine(StopTakingDamageForPeriod(timeBetweenDamages));
        }
        
        OnDamageTaken?.Invoke();
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

    private IEnumerator StopTakingDamageForPeriod(float period)
    {
        recentlyDamaged = true;
        yield return new WaitForSeconds(period);
        recentlyDamaged = false;
    }

    protected void Respawn()
    {
        isDead = false;
        Destroy(explosionParticleInstantiated);
        transform.position = respawnPos;
        gameObject.SetActive(true);
        GetComponent<CapsuleCollider>().enabled = true;
        health = defaultHealth;
        onRespawn?.Invoke();
    }
}
