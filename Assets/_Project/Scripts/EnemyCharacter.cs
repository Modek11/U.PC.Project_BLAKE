using UnityEngine;

public class EnemyCharacter : BlakeCharacter
{
    private EnemyAIManager ai;
    [SerializeField]
    private GameObject weaponPickup;

    private void Awake()
    {
        ai = GetComponent<EnemyAIManager>();
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
        base.DestroySelf();
        Destroy(explosionParticleInstantiated);

        WeaponDefinition weaponDef = ai.GetWeaponRef().GetComponent<Weapon>()?.GetWeaponDefinition();

        if (weaponDef != null)
        {
            float drop = Random.Range(0f, 1f);
            Debug.Log("Drop chance: " + drop + " | Treshold: " + weaponDef.dropRate);
            if (drop <= weaponDef.dropRate)
            {
                GameObject weaponPickupObject = Instantiate(weaponPickup, transform.position, Quaternion.identity);

                //WeaponDefinition randomWeapon = WeaponsMagazine.GetRandomWeapon();
                WeaponPickup weaponPickupScript = weaponPickupObject.GetComponent<WeaponPickup>();
                weaponPickupScript.SetWeaponDefinition(weaponDef);
                weaponPickupScript.ammo = WeaponsMagazine.GetRandomWeaponAmmo(weaponDef);
            }
        }
    }
}