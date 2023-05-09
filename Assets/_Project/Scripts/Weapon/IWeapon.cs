using UnityEngine;

public interface IWeapon
{
    public bool PrimaryAttack();

    public GameObject GetGameObject();

    public float GetWeaponRange();

    public float GetWeaponFireRate();
}
