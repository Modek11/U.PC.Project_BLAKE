using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCharacter : BlakeCharacter
{
    [SerializeField]
    private WeaponPickup weaponPickup;
    
    private AIController ai;
    private float destroySelfTime = 2f;
    private float dropWeaponTime = .3f;

    [HideInInspector] 
    public Room SpawnedInRoom;
    
    private void Awake()
    {
        ai = GetComponent<AIController>();
    }

    public override void Die(GameObject killer)
    {
        //animator.SetBool("IsAlive", false);
        explosionParticleInstantiated = Instantiate(explosionParticle, transform.position, Quaternion.identity);

        _ = DestroySelf();
        _ = DropWeapon();

        base.Die(killer);
    }

    private async UniTaskVoid DestroySelf()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(destroySelfTime));
        
        Destroy(explosionParticleInstantiated);
        Destroy(gameObject);
    }

    private async UniTaskVoid DropWeapon()
    {
        if (ai.Weapon.WeaponDefinition == null)
        {
            Debug.LogError("WeaponDefinition is not valid. " + name);
            return;
        }

        var weapon = ai.Weapon;
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