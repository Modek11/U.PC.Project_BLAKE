using _Project.Scripts.PointsSystem;
using _Project.Scripts;
using _Project.Scripts.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SavageTwoPerk", menuName = "Abilities/Perks/Savage Two")]

public class SavageTwo : PerkScriptableObject
{
    private WeaponsManager weaponsManager;
    public override void OnPickup(PlayerPerkManager player)
    {
        base.OnPickup(player);
        weaponsManager = ReferenceManager.BlakeHeroCharacter.GetComponent<WeaponsManager>();
        weaponsManager.throwOnNoAmmo = true;
    }

    public override void OnRemove()
    {
        weaponsManager.throwOnNoAmmo = false;

    }
}
