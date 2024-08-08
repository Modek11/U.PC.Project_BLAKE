using _Project.Scripts.Interfaces;
using _Project.Scripts.Weapon.Definition;
using _Project.Scripts.Weapon.Statistics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

namespace _Project.Scripts.Weapon
{
    public class MeleeWeapon : Weapon
    {
        private PlayableDirector playableDirector;
        private Transform characterTransform;

        private MeleeWeaponDefinition meleeWeaponDefinition;
        private MeleeWeaponStatistics baseWeaponStats;
        private MeleeWeaponStatistics weaponUpgrades;
        private MeleeWeaponStatistics currentWeaponStats;
        
        private Collider[] raycastCollidersFound;
        private int maxSpreadRangePerSide;
        private float lastAttackTime;

        private void OnDisable()
        {
            transform.localRotation = quaternion.identity;
            playableDirector.Stop();
        }

        protected override void Awake()
        {
            base.Awake();
            SetupWeaponDefinition();

            playableDirector = GetComponent<PlayableDirector>();
            characterTransform = transform.GetComponentInParent<BlakeCharacter>().transform;
        }

        public override bool CanPrimaryAttack()
        {
            return Time.time - lastAttackTime > currentWeaponStats.AttackDelayTime;
        }

        public override void PrimaryAttack()
        {
            playableDirector.Stop();
            playableDirector.Play();
            audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSource.Play();
            
            lastAttackTime = Time.time;

            MakeRaycast();
        }

        private void MakeRaycast()
        {
            var characterForwardDir = characterTransform.forward;
            characterForwardDir.y = 0;

            var collidersFoundNumber = Physics.OverlapSphereNonAlloc(characterTransform.position,
                currentWeaponStats.SphereCastRadius, raycastCollidersFound, currentWeaponStats.EnemyLayerMask);
            for (var i = 0; i < collidersFoundNumber; i++)
            {
                var colliderFound = raycastCollidersFound[i];
                if (colliderFound is null)
                {
                    break;
                }
                
                var targetDir = colliderFound.transform.position - characterTransform.position;
                targetDir.y = 0;
                    
                var angle = Vector3.Angle(characterForwardDir, targetDir);

                if (angle > maxSpreadRangePerSide)
                {
                    continue;
                }
                
                var damageable = colliderFound.transform.GetComponentInParent<IDamageable>();
                damageable?.TryTakeDamage(transform.parent.gameObject, 1);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying || !drawDebugGizmos)
            {
                return;
            }
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(characterTransform.position, currentWeaponStats.SphereCastRadius);
        }
#endif
        
        private void SetupWeaponDefinition()
        {
            if (WeaponDefinition is not MeleeWeaponDefinition definition)
            {
                Debug.LogError("Wrong WeaponDefinition is attached to the weapon!");
                return;
            }
            
            meleeWeaponDefinition = definition;

            baseWeaponStats = meleeWeaponDefinition.GetWeaponStatistics();
            
            raycastCollidersFound = new Collider[meleeWeaponDefinition.MaxNumberOfEnemies];
            RestoreMeleeWeaponStatistics();
        }
        
        public override void CalculateWeaponStatsWithUpgrades(WeaponDefinition weaponDefinition, IWeaponStatistics weaponStatistics)
        {
            if (meleeWeaponDefinition is not null)
            {
                if(weaponDefinition != meleeWeaponDefinition)
                {
                    return;
                }
            }
            else if (weaponDefinition != (RangedWeaponDefinition)WeaponDefinition)
            {
                return;
            }

            var statistics = (MeleeWeaponStatistics)weaponStatistics;
            weaponUpgrades = statistics;

            RestoreMeleeWeaponStatistics();
        }

        public void ApplyMeleeWeaponStatistics(MeleeWeaponStatistics meleeWeaponStatistics)
        {
            currentWeaponStats = meleeWeaponStatistics;

            ResetSpread();
        }

        public void RestoreMeleeWeaponStatistics()
        {
            currentWeaponStats = baseWeaponStats + weaponUpgrades;
            
            ResetSpread();
        }
        
        private void ResetSpread()
        {
            maxSpreadRangePerSide = baseWeaponStats.MaxSpreadRange / 2;
        }

        public override void LoadWeaponInstanceInfo(WeaponInstanceInfo weaponInstanceInfo)
        {
        
        }
    }
}
