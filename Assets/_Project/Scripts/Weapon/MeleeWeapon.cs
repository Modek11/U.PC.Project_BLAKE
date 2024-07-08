using _Project.Scripts.Interfaces;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

namespace _Project.Scripts.Weapon
{
    public class MeleeWeapon : Weapon
    {
        private MeleeWeaponDefinition meleeWeaponDefinition;
        private PlayableDirector playableDirector;
        private Transform characterTransform;

        private MeleeWeaponStatistics savedMeleeWeaponStatistics;
        private float attackDelayTime;
        private float sphereCastRadius;
        private int maxSpreadRange;
        private LayerMask layerMask;
        private int maxNumberOfEnemies;
        
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
            return Time.time - lastAttackTime > attackDelayTime;
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
            
            var collidersFoundNumber = Physics.OverlapSphereNonAlloc(characterTransform.position, sphereCastRadius, raycastCollidersFound, layerMask);
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
            if (!Application.isPlaying)
            {
                return;
            }
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(characterTransform.position, sphereCastRadius);
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
            attackDelayTime = meleeWeaponDefinition.AttackDelayTime;
            sphereCastRadius = meleeWeaponDefinition.SpereCastRadius;
            maxSpreadRange = meleeWeaponDefinition.MaxSpreadRange;
            maxSpreadRangePerSide = maxSpreadRange / 2;
            layerMask = meleeWeaponDefinition.LayerMask;
            maxNumberOfEnemies = meleeWeaponDefinition.MaxNumberOfEnemies;
            raycastCollidersFound = new Collider[maxNumberOfEnemies];
        }
        
        public MeleeWeaponStatistics SaveAndGetMeleeWeaponStatistics()
        {
            return savedMeleeWeaponStatistics = new MeleeWeaponStatistics(attackDelayTime, sphereCastRadius, 
                maxSpreadRange, layerMask, maxNumberOfEnemies);
        }

        public void ApplyMeleeWeaponStatistics(MeleeWeaponStatistics meleeWeaponStatistics)
        {
            attackDelayTime = meleeWeaponStatistics.AttackDelayTime;
            sphereCastRadius = meleeWeaponStatistics.SphereCastRadius;
            maxSpreadRange = meleeWeaponStatistics.MaxSpreadRange;
            layerMask = meleeWeaponStatistics.LayerMask;
            maxNumberOfEnemies = meleeWeaponStatistics.MaxNumberOfEnemies;
        }

        public void RestoreMeleeWeaponStatistics()
        {
            ApplyMeleeWeaponStatistics(savedMeleeWeaponStatistics);
        }

        public override void LoadWeaponInstanceInfo(WeaponInstanceInfo weaponInstanceInfo)
        {
        
        }
    }
}
