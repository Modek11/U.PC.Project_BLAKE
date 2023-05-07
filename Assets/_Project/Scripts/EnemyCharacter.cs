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
    protected override void DestroySelf()
    {
        base.DestroySelf();
        GameObject weaponPickupObject = Instantiate(weaponPickup, transform.position, Quaternion.identity);

        WeaponDefinition randomWeapon = WeaponsMagazine.GetRandomWeapon();
        WeaponPickup weaponPickupScript = weaponPickupObject.GetComponent<WeaponPickup>();
        weaponPickupScript.SetWeaponDefinition(randomWeapon != null ? randomWeapon : ai.GetWeaponRef().GetComponent<Weapon>().GetWeaponDefinition());
        weaponPickupScript.ammo = WeaponsMagazine.GetRandomWeaponAmmo(randomWeapon);
    }
}