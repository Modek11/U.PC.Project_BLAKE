using UnityEngine;

public class EnemyCharacter : BlakeCharacter
{
    public Room SpawnedInRoom;
    
    [SerializeField]
    private GameObject weaponPickup;

    private AIController ai;
    
    private void Awake()
    {
        ai = GetComponent<AIController>();
    }

    public override void Die()
    {
        //animator.SetBool("IsAlive", false);
        explosionParticleInstantiated = Instantiate(explosionParticle, transform.position, Quaternion.identity);
        Invoke("DestroySelf", 2f);

        base.Die();
    }

    protected override void DestroySelf()
    {
        if (ai.Weapon != null)
        {
            float drop = Random.Range(0f, 1f);
            if (drop <= ai.Weapon.WeaponDefinition.DropRate)
            {
                GameObject weaponPickupObject = Instantiate(weaponPickup, transform.position, Quaternion.identity);
                SpawnedInRoom.AddSpawnedWeapon(weaponPickupObject);

                WeaponPickup weaponPickupScript = weaponPickupObject.GetComponent<WeaponPickup>();
                weaponPickupScript.WeaponDefinition = ai.Weapon.WeaponDefinition;
                weaponPickupScript.WeaponInstanceInfo = ai.Weapon.GenerateWeaponInstanceInfo(true);
            }
        }

        Destroy(explosionParticleInstantiated);
        base.DestroySelf();
    }
}