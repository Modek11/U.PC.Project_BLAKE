using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BasicAttack : MonoBehaviour, IAttack
{
    [SerializeField] private float timeBetweenShooting;
    [SerializeField] private float spread;
    [SerializeField] private int shotsPerTap;
    [SerializeField] private int bulletsPerShot = 1;
    private int bulletsToShotInThisAttack;
    private Weapon usedWeapon;
    
    public void Attack(Weapon weapon)
    {
        bulletsToShotInThisAttack = shotsPerTap;
        usedWeapon = weapon;
        Shot();
    }

    private void Shot()
    {
        usedWeapon.isLastShotOver = false;
        usedWeapon.As.PlayOneShot(usedWeapon.As.clip);
        for (int i = 0; i < bulletsPerShot; i++)
        {
            if(usedWeapon.BulletsLeft == 0 ) break;
            //TODO: Add pooling
            Instantiate(usedWeapon.BulletPrefab, usedWeapon.BulletsSpawnPoint.position, usedWeapon.transform.rotation).GetComponent<IBullet>().SetupBullet(Random.Range(-spread, spread));
            usedWeapon.BulletsLeft--;
        }
        bulletsToShotInThisAttack--;
        usedWeapon.Invoke(nameof(usedWeapon.ResetShot), timeBetweenShooting);
        if (bulletsToShotInThisAttack > 0 && usedWeapon.BulletsLeft > 0)
        {
            Invoke(nameof(Shot), timeBetweenShooting);
        }
    }
}
