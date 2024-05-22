using _Project.Scripts.Interfaces;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

namespace _Project.Scripts.Weapon
{
    public class MeleeWeapon : Weapon
    {
        private MeleeWeaponDefinition meleeWeaponDefinition;
        private float spereCastRadius;
        private float maxDistance;
        private LayerMask layerMask;
        private PlayableDirector playableDirector;

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
            Invoke("MakeRaycast", 0.27f); // XD  
        }

        private void MakeRaycast()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, spereCastRadius, transform.forward, maxDistance, layerMask);
            foreach (RaycastHit hit in hits)
            {
                Debug.Log(hit.transform.gameObject.name);
                IDamageable damageable = hit.transform.GetComponentInParent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TryTakeDamage(transform.parent.gameObject, 1);
                }
            }
        }

        private void SetupWeaponDefinition()
        {
            if (WeaponDefinition is not MeleeWeaponDefinition definition)
            {
                Debug.LogError("Wrong WeaponDefinition is attached to the weapon!");
                return;
            }
            
            meleeWeaponDefinition = definition;
            spereCastRadius = meleeWeaponDefinition.SpereCastRadius;
            maxDistance = meleeWeaponDefinition.MaxDistance;
            layerMask = meleeWeaponDefinition.LayerMask;
        }

        public override void LoadWeaponInstanceInfo(WeaponInstanceInfo weaponInstanceInfo)
        {
        
        }
    }
}
