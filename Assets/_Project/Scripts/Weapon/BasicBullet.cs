using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class BasicBullet : MonoBehaviour, IBullet
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float destroyTime;
    [SerializeField] private int penetrateAmount;
    private Vector3 moveDirection;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    
    //TODO: Add magic function handler
    private void Update()
    {
        rb.velocity = moveDirection + transform.forward * bulletSpeed;
    }

    public void SetupBullet(float xSpread)
    {
        moveDirection = new Vector3(Random.Range(-xSpread, xSpread), 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // collision.gameObject.GetComponent<IDamagable>().GetDamage(damage);
            Destroy(collision.gameObject);
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
