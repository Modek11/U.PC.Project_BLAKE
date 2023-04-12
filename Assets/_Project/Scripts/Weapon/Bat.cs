using UnityEngine;

public class Bat : MonoBehaviour, IWeapon
{
    public void PrimaryAttack()
    {
        Debug.Log(name + "Primary attack");
    }

    public void Reload()
    {
        
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
