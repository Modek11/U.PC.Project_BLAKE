using UnityEngine;

public class BasicAttack : MonoBehaviour, IAttack
{
    [Tooltip("Declares how often player can shoot")]
    [SerializeField] private float timeBetweenShooting;
    [Tooltip("Declares range of bullet spawn")]
    [SerializeField] private float spread;
    [Tooltip("Declares how many times it should shot after one button press")]
    [SerializeField] private int shotsPerTap;
    [Tooltip("Declares how many bullets should be instantiated in one shot")]
    [SerializeField] private int bulletsPerShot = 1;
    private int bulletsToShotInThisAttack;
    private Weapon usedWeapon;
    
    public void Attack(Weapon weapon)
    {
        bulletsToShotInThisAttack = shotsPerTap;
        usedWeapon = weapon;
        Shot();
    }

    public float ReturnFireRate()
    {
        return timeBetweenShooting;
    }

    private void Shot()
    {
        usedWeapon.isLastShotOver = false;
        usedWeapon.As.PlayOneShot(usedWeapon.As.clip);
        for (int i = 0; i < bulletsPerShot; i++)
        {
            if(usedWeapon.BulletsLeft == 0 ) break;
            //TODO: Add pooling
            var bullet = Instantiate(usedWeapon.BulletPrefab, usedWeapon.BulletsSpawnPoint.position, usedWeapon.transform.rotation);
            bullet.GetComponent<IBullet>().SetupBullet(Random.Range(-spread, spread), usedWeapon.transform.parent.gameObject, usedWeapon.Range);
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
