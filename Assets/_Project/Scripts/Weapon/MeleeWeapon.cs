using _Project.Scripts.Weapon;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

public class MeleeWeapon : Weapon
{
    [SerializeField] 
    private float spereCastRadius;

    [SerializeField] 
    private float maxDistance;

    [SerializeField] 
    private LayerMask layerMask;

    private PlayableDirector playableDirector;

    private void OnDisable()
    {
        transform.localRotation = quaternion.identity;
        playableDirector.Stop();
    }

    protected override void Awake()
    {
        base.Awake();

        playableDirector = GetComponent<PlayableDirector>();
    }

    public override bool CanPrimaryAttack()
    {
        return playableDirector.state != PlayState.Playing;
    }

    public override void PrimaryAttack()
    {
        playableDirector.Play();
        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        audioSource.Play();
        Invoke("MakeRaycast", 0.27f); // XD  
    }

    private void MakeRaycast()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, spereCastRadius, transform.forward, maxDistance, layerMask);
        foreach (RaycastHit hit in hits)
        {
            Debug.Log(hit.transform.gameObject.name);
            IDamageable damageable = hit.transform.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(transform.parent.gameObject, 1);
            }
        }
    }

    public override void LoadWeaponInstanceInfo(WeaponInstanceInfo weaponInstanceInfo)
    {
        
    }
}
