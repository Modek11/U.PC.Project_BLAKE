using UnityEngine;

public interface IWeapon
{
    public bool PrimaryAttack();
    public void SetAmmo(int newAmmo);

    public GameObject GetWeapon();
    public void SetWeaponDefinition(WeaponDefinition weaponDefinition);
    public WeaponDefinition GetWeaponDefinition();

    public float GetWeaponRange();
    public float GetWeaponFireRate();
}
