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

    public override void Die(GameObject killer)
    {
        //animator.SetBool("IsAlive", false);
        explosionParticleInstantiated = Instantiate(explosionParticle, transform.position, Quaternion.identity);
        Invoke("DestroySelf", 2f);

        base.Die(killer);
    }

    protected override void DestroySelf()
    {
        if (ai.Weapon != null)
        {
            if (ai.Weapon.WeaponDefinition != null)
            {
                float drop = Random.Range(0f, 1f);
                if (drop > 0 && drop <= ai.Weapon.WeaponDefinition.DropRate)
                {
                    GameObject weaponPickupObject = Instantiate(weaponPickup, transform.position, Quaternion.identity);
                    SpawnedInRoom.AddSpawnedWeapon(weaponPickupObject);

                    WeaponPickup weaponPickupScript = weaponPickupObject.GetComponent<WeaponPickup>();
                    weaponPickupScript.WeaponDefinition = ai.Weapon.WeaponDefinition;
                    weaponPickupScript.WeaponInstanceInfo = ai.Weapon.GenerateWeaponInstanceInfo(true);
                }
            }
            else
            {
                Debug.LogError("WeaponDefinition is not valid. " + name);
            }
        }

        Destroy(explosionParticleInstantiated);
        base.DestroySelf();
    }
}