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

        private float sphereCastRadius;
        private int maxSpreadRange;
        private LayerMask layerMask;
        private int maxNumberOfEnemies;
        
        private Collider[] raycastCollidersFound;
        private int maxSpreadRangePerSide;

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
            return playableDirector.state != PlayState.Playing;
        }

        public override void PrimaryAttack()
        {
            playableDirector.Play();
            audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSource.Play();

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
            sphereCastRadius = meleeWeaponDefinition.SpereCastRadius;
            maxSpreadRange = meleeWeaponDefinition.MaxSpreadRange;
            maxSpreadRangePerSide = maxSpreadRange / 2;
            layerMask = meleeWeaponDefinition.LayerMask;
            maxNumberOfEnemies = meleeWeaponDefinition.MaxNumberOfEnemies;
            raycastCollidersFound = new Collider[maxNumberOfEnemies];
        }

        public override void LoadWeaponInstanceInfo(WeaponInstanceInfo weaponInstanceInfo)
        {
        
        }
    }
}
