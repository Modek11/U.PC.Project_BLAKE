using UnityEngine;

public interface IWeapon
{
    public void PrimaryAttack();
    public void Reload();

    public GameObject GetGameObject();
}
