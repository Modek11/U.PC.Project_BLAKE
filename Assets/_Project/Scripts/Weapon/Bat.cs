using UnityEngine;

public class Bat : MonoBehaviour, IWeapon
{
    [SerializeField] private float spereCastRadius = 1.0f;
    [SerializeField] private float maxDistance = 1.0f;
    [SerializeField] private LayerMask layerMask;

    public void PrimaryAttack()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, spereCastRadius, transform.forward, maxDistance, layerMask);//, maxDistance, layerMask))
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

}
