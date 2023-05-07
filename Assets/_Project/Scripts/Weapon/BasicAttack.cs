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
    
    public bool Attack(Weapon weapon)
    {
        bulletsToShotInThisAttack = shotsPerTap;
        usedWeapon = weapon;
        usedWeapon.As.PlayOneShot(usedWeapon.As.clip);
        return Shot();
    }

    public float ReturnFireRate()
    {
        return timeBetweenShooting;
    }

    private bool Shot()
    {
        usedWeapon.isLastShotOver = false;
        for (int i = 0; i < bulletsPerShot; i++)
        {
            if(usedWeapon.BulletsLeft == 0 ) return false;
            //TODO: Add pooling
            var bullet = Instantiate(usedWeapon.BulletPrefab, usedWeapon.BulletsSpawnPoint.position, usedWeapon.transform.rotation);

            //makes spread goes both sides
            if (bulletsPerShot <= 1)
            {
                bullet.GetComponent<IBullet>().SetupBullet(Random.Range(-spread, spread), usedWeapon.transform.parent.gameObject, usedWeapon.Range);

            }
            else
            {
                if (i % 2 == 0)
                {
                    bullet.GetComponent<IBullet>().SetupBullet(Random.Range(-spread, 0), usedWeapon.transform.parent.gameObject, usedWeapon.Range);
                }
                else
                {
                    bullet.GetComponent<IBullet>().SetupBullet(Random.Range(0, spread), usedWeapon.transform.parent.gameObject, usedWeapon.Range);

                }
            }
            if (!usedWeapon.infinityAmmo) usedWeapon.BulletsLeft--;
        }
        //bulletsToShotInThisAttack--;
        usedWeapon.Invoke(nameof(usedWeapon.ResetShot), timeBetweenShooting);
        return true;
    }
}
