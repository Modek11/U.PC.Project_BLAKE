using System;
using _Project.Scripts.Weapon;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class EnemyCharacter : BlakeCharacter
    {

        [SerializeField]
        private float speed = 6f;
        [SerializeField]
        private float additionalSpeed = 0;
        [SerializeField]
        private float patrolSpeed = 2.5f;

        [SerializeField]
        private WeaponPickup weaponPickup;

        private AIController ai;
        private float destroySelfTime = 2f;
        private float dropWeaponTime = .3f;

        [HideInInspector]
        public Room SpawnedInRoom;
        public bool savageThreeActivated = false;

        private void Awake()
        {
            ai = GetComponent<AIController>();
        }

        public float CalculateSpeed()
        {
            return speed + additionalSpeed;
        }

        public float GetPatrolSpeed()
        {
            return patrolSpeed;
        }

        public void AddAdditionalSpeed(float speed)
        {
            additionalSpeed += speed;
        }

        public void RemoveAdditionalSpeed(float speed)
        {
            additionalSpeed -= speed;
        }

        public override void Die(GameObject killer)
        {
            //animator.SetBool("IsAlive", false);
            explosionParticleInstantiated = Instantiate(explosionParticle, transform.position, Quaternion.identity);

            _ = DestroySelf();
            _ = DropWeapon(killer);

            base.Die(killer);
        }

        private async UniTaskVoid DestroySelf()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(destroySelfTime));

            Destroy(explosionParticleInstantiated);
            Destroy(gameObject);
        }

        private async UniTaskVoid DropWeapon(GameObject killer)
        {
            if (ai.Weapon.WeaponDefinition == null)
            {
                Debug.LogError("WeaponDefinition is not valid. " + name);
                return;
            }
            var weapon = ai.Weapon;

            if (savageThreeActivated)
            {
                WeaponsManager wm = killer.GetComponent<WeaponsManager>();
                if (wm != null)
                {
                    foreach (var w in wm.Weapons)
                    {
                        if (w == null) continue;
                        if (weapon.WeaponDefinition.WeaponName == w.WeaponDefinition.WeaponName)
                        {
                            return;
                        }
                    }
                }
            }
            Destroy(ai.Weapon.gameObject);

            await UniTask.Delay(TimeSpan.FromSeconds(dropWeaponTime));

            var drop = Random.Range(0f, 1f);
            if (drop <= weapon.WeaponDefinition.DropRate)
            {
                var weaponPickupInstantiated = Instantiate(weaponPickup, transform.position, Quaternion.identity);
                weaponPickupInstantiated.WeaponDefinition = weapon.WeaponDefinition;
                weaponPickupInstantiated.WeaponInstanceInfo = weapon.GenerateWeaponInstanceInfo(true);
                SpawnedInRoom.AddSpawnedWeapon(weaponPickupInstantiated.gameObject);
            }
        }
    }
}