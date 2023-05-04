using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class BasicBullet : MonoBehaviour, IBullet
{
    [SerializeField] private float bulletSpeed;
    [Tooltip("Time after bullet will be destroyed")]
    [SerializeField] private float destroyTime;
    [Tooltip("How many enemies bullet should penetrate 0 = destroy at first kill")]
    [SerializeField] private int penetrateAmount;
    private Vector3 moveDirection;
    private Rigidbody rb;

    public GameObject _instigator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    //TODO: Add magic function handler
    private void Update()
    {
        rb.velocity = moveDirection + transform.forward * bulletSpeed;
    }

    /// <summary>
    /// Use it on instantiate to declare base stats which are weapon related
    /// </summary>
    /// <param name="xSpread">Spread range (it declares range of (-xSpread, xSpread))</param>
    public void SetupBullet(float xSpread, GameObject instigator, float range)
    {
        destroyTime = range / bulletSpeed;
        //TODO: Instead of changing spawn pos, change rotation
        moveDirection = new Vector3(Random.Range(-xSpread, xSpread), 0, 0);
        _instigator = instigator;
        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null && collision.gameObject != _instigator)
        {
            //TODO: Add IDamagable interface on enemies, keeping damage on bullets (even if enemies are one shot one kill) will help us in future if we will be adding destroyable elements, which will require different strength
            damageable.TakeDamage(_instigator, 1/*damage*/);

            if (penetrateAmount > 0)
            {
                penetrateAmount--;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
}
