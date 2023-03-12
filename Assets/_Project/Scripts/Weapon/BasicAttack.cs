using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour, IAttack
{
    [SerializeField] private float timeBetweenShooting;
    [SerializeField] private int bulletsShot;
    [SerializeField] private int bulletsPerTap;
    public void Shot(Glock glock)
    {
        bulletsShot = bulletsPerTap;
        Attack(glock);
    }

    private void Attack(Glock glock)
    {
        glock.readyToShoot = false;
        Instantiate(glock.bulletPrefab, glock.attackPoint.position, glock.transform.rotation);
        glock.bulletsLeft--;
        bulletsShot--;
        glock.Invoke(nameof(glock.ResetShot), timeBetweenShooting);
        if (bulletsShot > 0 && glock.bulletsLeft > 0)
        {
            Invoke(nameof(Attack), timeBetweenShooting);
        }
    }
}
