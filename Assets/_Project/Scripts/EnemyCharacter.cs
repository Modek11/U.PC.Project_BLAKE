using System.Collections;
using System.Collections.Generic;
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
        weaponPickupObject.GetComponent<WeaponPickup>().SetWeaponDefinition(ai.GetWeaponRef().GetComponent<Weapon>().GetWeaponDefinition());
    }
}
