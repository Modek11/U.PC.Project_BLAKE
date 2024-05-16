using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace _Project.Scripts.Weapon
{
    public struct RangedWeaponStatistics
    {
        public RangedWeaponStatistics(SpreadType spreadType, float spread, float spreadResetThreshold, int projectilesPerShot, float range, float fireDelayTime = 0)
        {
            SpreadType = spreadType;
            Spread = spread;
            SpreadResetThreshold = spreadResetThreshold;
            ProjectilesPerShot = projectilesPerShot;
            Range = range;
            FireDelayTime = fireDelayTime;
        }
        
        public SpreadType SpreadType;
        public float Spread;
        public float SpreadResetThreshold;
        public int ProjectilesPerShot;
        public float Range;
        public float FireDelayTime;
    }
    
    [RequireComponent(typeof(AudioSource))]
    public class RangedWeapon : Weapon
    {
        [SerializeField]
        private Transform bulletsSpawnPoint;
        [SerializeField]
        private bool infinityAmmo = false;

        private RangedWeaponStatistics savedRangedWeaponStatistics;
        private RangedWeaponDefinition rangedWeaponDefinition;
        private Rigidbody rb;
        private float fireDelayTime;
        private SpreadType spreadType;
        private float spread;
        private float spreadStep;
        private float spreadThreshold;
        private float spreadResetThreshold;
        private int projectilesPerShot;
        private int magazineSize;
        private float range;
        
        private float lastFireTime;
        private float firstFireTime;
        private float negativeSpreadThreshold;
        private float positiveSpreadThreshold;
        private float currentSpread;
        
        public float Range => range;
        public int BulletsLeft { get; set; }
        public SpreadType CurrentSpreadType { get; set; }

        protected override void Awake()
        {
            base.Awake();

            SetupWeaponDefinition();
        }

        public override void PrimaryAttack()
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(audioSource.clip);
        
            Shot();

            lastFireTime = Time.time;

            weaponsManager?.BroadcastOnPrimaryAttack();
        }

        protected override void OnOwnerChanged()
        {
            base.OnOwnerChanged();

            rb = Owner.GetComponent<Rigidbody>();
        }

        public override bool CanPrimaryAttack()
        {
            if(BulletsLeft <= 0)
            {
                StartCoroutine("UnequipSelf");
                return false;
            }
            
            if (Time.time - lastFireTime < fireDelayTime) return false;

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
            if (BulletsLeft == 0) return false;

            foreach (var bulletSpreadValue in GetCalculatedProjectilesAngles())
            {
                //TODO: Add pooling
                var bulletPrefab = rangedWeaponDefinition.BasicBullet.gameObject;
                var bullet = Instantiate(bulletPrefab, bulletsSpawnPoint.position, transform.rotation);
                
                bullet.GetComponent<IBullet>().SetupBullet(bulletSpreadValue, transform.parent.gameObject, range);
                Debug.Log(bulletSpreadValue + " ||| " + spreadType);
            }

            if (!infinityAmmo) BulletsLeft--;

            return true;
        }

        private List<float> GetCalculatedProjectilesAngles()
        {
            var projectilesAngles = new List<float>();
            
            if (spreadType == SpreadType.Undefined)
            {
                Debug.LogError($"SpreadType in {rangedWeaponDefinition.WeaponName} is Undefined!");
            }

            else if (spreadType == SpreadType.NoSpread)
            {
                projectilesAngles.Add(0f);
            }
            
            else if (spreadType == SpreadType.Static)
            {
                var projectileAngle = Random.Range(negativeSpreadThreshold, positiveSpreadThreshold);
                projectilesAngles.Add(projectileAngle);
            }

            else if (spreadType == SpreadType.StaticMultiShot)
            {
                var anglePerProjectile = spread / (projectilesPerShot - 1);

                for (var i = 0; i < projectilesPerShot; i++)
                {
                    var projectileAngle = negativeSpreadThreshold + anglePerProjectile * i;
                    projectilesAngles.Add(projectileAngle);
                }
            }

            else if (spreadType == SpreadType.NoSpreadThenStatic)
            {
                if (Time.time - lastFireTime < spreadResetThreshold)
                { 
                    var projectileAngle = Random.Range(negativeSpreadThreshold, positiveSpreadThreshold);
                    projectilesAngles.Add(projectileAngle);
                }
                else
                {
                    projectilesAngles.Add(0f);
                }
            }

            else if (spreadType == SpreadType.GraduallyIncrease)
            {
                if (Time.time - lastFireTime < fireDelayTime + 0.1f)
                {
                    ResetSpread();
                }
                
                projectilesAngles.Add(currentSpread);
                if (currentSpread < spreadThreshold)
                {
                    currentSpread += spreadStep;
                }
            }

            return projectilesAngles;
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

        private void SetupWeaponDefinition()
        {
            if (WeaponDefinition is not RangedWeaponDefinition definition)
            {
                Debug.LogError("Wrong WeaponDefinition is attached to the weapon!");
                return;
            }
            
            rangedWeaponDefinition = definition;
            fireDelayTime = rangedWeaponDefinition.FireDelayTime;
            spreadType = rangedWeaponDefinition.SpreadType;
            spread = rangedWeaponDefinition.Spread;
            spreadStep = rangedWeaponDefinition.SpreadStep;
            spreadThreshold = rangedWeaponDefinition.SpreadThreshold;
            spreadResetThreshold = rangedWeaponDefinition.SpreadResetThreshold;
            projectilesPerShot = rangedWeaponDefinition.ProjectilesPerShot;
            magazineSize = rangedWeaponDefinition.MagazineSize;
            range = rangedWeaponDefinition.Range;
            BulletsLeft = magazineSize;
            
            ResetSpread();
        }

        public RangedWeaponStatistics SaveAndGetRangedWeaponStatistics()
        {
            return savedRangedWeaponStatistics = new RangedWeaponStatistics(spreadType, spread, spreadResetThreshold, projectilesPerShot, range, fireDelayTime);
        }

        public void ApplyRangedWeaponStatistics(RangedWeaponStatistics rangedWeaponStatistics)
        {
            spreadType = rangedWeaponStatistics.SpreadType;
            spread = rangedWeaponStatistics.Spread;
            spreadResetThreshold = rangedWeaponStatistics.SpreadResetThreshold;
            projectilesPerShot = rangedWeaponStatistics.ProjectilesPerShot;
            range = rangedWeaponStatistics.Range;
            fireDelayTime = rangedWeaponStatistics.FireDelayTime;
            lastFireTime = Time.time;

            ResetSpread();
        }

        public void RestoreRangedWeaponStatistics()
        {
            ApplyRangedWeaponStatistics(savedRangedWeaponStatistics);
        }

        private void ResetSpread()
        {
            currentSpread = spread;
            negativeSpreadThreshold = -(spread / 2);
            positiveSpreadThreshold = spread / 2;
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
