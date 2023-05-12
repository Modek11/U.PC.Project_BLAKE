using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

public class Bat : MonoBehaviour, IWeapon
{
    [SerializeField] private float spereCastRadius;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float attackSpeed;

    private PlayableDirector playableDirector;

    private void OnDisable()
    {
        transform.localRotation = quaternion.identity;
        playableDirector.Stop();
    }

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    public bool PrimaryAttack()
    {
        if (playableDirector.state == PlayState.Playing) return false;

        playableDirector.Play();
        Invoke("MakeRaycast", 0.27f); // XD  
        return true;
    }

    private void MakeRaycast()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, spereCastRadius, transform.forward, maxDistance, layerMask);
        foreach (RaycastHit hit in hits)
        {
            Debug.Log(hit.transform.gameObject.name);
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(transform.parent.gameObject, 1);
            }
        }
    }

    public void Reload()
    {
        
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public float GetWeaponRange()
    {
        return maxDistance;
    }

    public float GetWeaponFireRate()
    {
        return attackSpeed;
    }

    public void SetAmmo(int newAmmo)
    {

    }
}
