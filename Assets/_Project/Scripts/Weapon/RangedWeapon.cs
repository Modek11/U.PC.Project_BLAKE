using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Weapon
{
    [RequireComponent(typeof(AudioSource))]
    public class RangedWeapon : Weapon
    {
        [SerializeField]
        private Transform bulletsSpawnPoint;
        [SerializeField]
        private bool infinityAmmo = false;

        private float fireDelayTime;
        private float spread;
        private float spreadMinMagnitude;
        private int projectilesPerShot;
        private int magazineSize;
        
        private RangedWeaponDefinition rangedWeaponDefinition;
        private Rigidbody rb;
        private float lastFireTime;
        
        public int BulletsLeft { get; set; }
        public float Range { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            if (WeaponDefinition is not RangedWeaponDefinition definition)
            {
                Debug.LogError("Wrong WeaponDefinition is attached to the weapon!");
                return;
            }
            
            rangedWeaponDefinition = definition;
            fireDelayTime = rangedWeaponDefinition.FireDelayTime;
            spread = rangedWeaponDefinition.Spread;
            spreadMinMagnitude = rangedWeaponDefinition.SpreadMinMagnitude;
            projectilesPerShot = rangedWeaponDefinition.ProjectilesPerShot;
            magazineSize = rangedWeaponDefinition.MagazineSize;
            Range = rangedWeaponDefinition.Range;
            BulletsLeft = magazineSize;
        }

        public override void PrimaryAttack()
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(audioSource.clip);
        
            Shot();

            lastFireTime = Time.time;

            weaponsManager?.BroadcastOnPrimaryAttack();
        }

        public override void OnOwnerChanged()
        {
            base.OnOwnerChanged();

            rb = Owner.GetComponent<Rigidbody>();
        }

        public override bool CanPrimaryAttack()
        {
            if (Time.time - lastFireTime < fireDelayTime) return false;

            if(BulletsLeft <= 0)
            {
                StartCoroutine("UnequipSelf");
                return false;
            }

            return true;
        }

        private IEnumerator UnequipSelf()
        {
            yield return new WaitForEndOfFrame();

            if (Owner.TryGetComponent(out WeaponsManager weaponsManager))
            {
                weaponsManager.Unequip(weaponsManager.ActiveWeaponIndex);
                weaponsManager.SetActiveIndex(weaponsManager.ActiveWeaponIndex - 1);
            }
        }

        private bool Shot()
        {
            float spreadTmp = spread / 10f;
            //float chosenSpread = rb.velocity.magnitude <= spreadMinMagnitude ? 0 : Random.Range(projectilesPerShot > 1 ? -spread : 0f, spread);
            int f = -(projectilesPerShot / 2);
            if (f == 0)
            {
                f = 1;
                spreadTmp = spreadTmp * Random.value >= 0.5f ? 1f : -1f;
            }

            for (int i = 0; i < projectilesPerShot; i++)
            {
                if (BulletsLeft == 0) return false;
                //TODO: Add pooling
                var bulletPrefab = rangedWeaponDefinition.BasicBullet.gameObject;
                var bullet = Instantiate(bulletPrefab, bulletsSpawnPoint.position, transform.rotation);

                //Choose spread depending on player's controls
                //float chosenSpread = rb.velocity.magnitude <= spreadMinMagnitude ? 0 : Random.Range(-spread, spread);

                //if (projectilesPerShot <= 1)
                //{
                bullet.GetComponent<IBullet>().SetupBullet(f++ * spreadTmp/*chosenSpread*/, transform.parent.gameObject, Range);
                //}
                //else
                //{
                //    if (i % 2 == 0)
                //    {
                //        bullet.GetComponent<IBullet>().SetupBullet(chosenSpread/*Random.Range(-spread, 0)*/, transform.parent.gameObject, Range);
                //    }
                //    else
                //    {
                //        bullet.GetComponent<IBullet>().SetupBullet(chosenSpread/*Random.Range(0, spread)*/, transform.parent.gameObject, Range);
                //    }
                //}
            }

            if (!infinityAmmo) BulletsLeft--;

            return true;
        }

        public override WeaponInstanceInfo GenerateWeaponInstanceInfo(bool randomize)
        {
            if(randomize)
            {
                return new RangedWeaponInstanceInfo
                {
                    bulletsLeft = Random.Range(magazineSize / 2, magazineSize + 1)
                };
            }

            return new RangedWeaponInstanceInfo
            {
                bulletsLeft = BulletsLeft
            };
        }

        public override void LoadWeaponInstanceInfo(WeaponInstanceInfo weaponInstanceInfo)
        {
            RangedWeaponInstanceInfo rangedWeaponInstanceInfo = weaponInstanceInfo as RangedWeaponInstanceInfo;
            if (rangedWeaponInstanceInfo != null)
            {
                BulletsLeft = rangedWeaponInstanceInfo.bulletsLeft;
            }
        }

#if UNITY_EDITOR
        public void SetInfiniteAmmo(bool setInfinite)
        {
            infinityAmmo = setInfinite;
        }
#endif
    
    }
}
