using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(DestroyMe());
    }

    void Update()
    {
        _rigidbody.velocity = transform.forward * bulletSpeed;
    }

    private IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}