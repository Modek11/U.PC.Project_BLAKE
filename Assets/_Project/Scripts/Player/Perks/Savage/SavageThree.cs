using _Project.Scripts.Weapon;
using _Project.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SavageThreePerk", menuName = "Abilities/Perks/Savage Three")]

public class SavageThree : PerkScriptableObject
{
    private WeaponsManager weaponsManager;
    public override void OnPickup(PlayerPerkManager player)
    {
        base.OnPickup(player);
        weaponsManager = ReferenceManager.BlakeHeroCharacter.GetComponent<WeaponsManager>();
        weaponsManager.AddThirdWeaponSlot();
    }

    public override void OnRemove()
    {
        weaponsManager.RemoveThirdWeaponSlot();

    }
}
