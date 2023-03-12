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
    
    public void Attack(Weapon weapon)
    {
        bulletsToShotInThisAttack = bulletsPerTap;
        usedWeapon = weapon;
        Shot();
    }

    private void Shot()
    {
        usedWeapon.isLastShotOver = false;
        float xSpread = Random.Range(-spread, spread);
        //TODO: Add pooling
        Instantiate(usedWeapon.BulletPrefab, usedWeapon.BulletsSpawnPoint.position, usedWeapon.transform.rotation).GetComponent<IBullet>().SetupBullet(xSpread);
        usedWeapon.BulletsLeft--;
        bulletsToShotInThisAttack--;
        usedWeapon.Invoke(nameof(usedWeapon.ResetShot), timeBetweenShooting);
        if (bulletsToShotInThisAttack > 0 && usedWeapon.BulletsLeft > 0)
        {
            Invoke(nameof(Shot), timeBetweenShooting);
        }
    }
}
