using _Project.Scripts.Weapon;
using _Project.Scripts.GlobalHandlers;
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
