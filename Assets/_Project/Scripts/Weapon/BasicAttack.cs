using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour, IAttack
{
    [SerializeField] private float timeBetweenShooting;
    [SerializeField] private float spread;
    [SerializeField] private int bulletsPerTap;
    private int bulletsToShotInThisAttack;
    private Weapon usedWeapon;
    
    public void Shot(Weapon weapon)
    {
        bulletsToShotInThisAttack = bulletsPerTap;
        usedWeapon = weapon;
        Attack();
    }

    private void Attack()
    {
        float xSpread = Random.Range(-spread, spread);
        usedWeapon.isLastShotOver = false;
        Instantiate(usedWeapon.BulletPrefab, usedWeapon.BulletsSpawnPoint.position, usedWeapon.transform.rotation);
        usedWeapon.BulletsLeft--;
        bulletsToShotInThisAttack--;
        usedWeapon.Invoke(nameof(usedWeapon.ResetShot), timeBetweenShooting);
        if (bulletsToShotInThisAttack > 0 && usedWeapon.BulletsLeft > 0)
        {
            Invoke(nameof(Attack), timeBetweenShooting);
        }
    }
}
