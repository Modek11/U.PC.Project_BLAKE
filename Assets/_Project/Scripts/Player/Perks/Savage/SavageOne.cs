using _Project.Scripts;
using _Project.Scripts.PointsSystem;
using _Project.Scripts.Weapon;
using SickDev.DevConsole.Example;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.GlobalHandlers;
using UnityEngine;

[CreateAssetMenu(fileName = "SavageOnePerk", menuName = "Abilities/Perks/Savage One")]

public class SavageOne : PerkScriptableObject
{
    private WeaponsManager weaponsManager;
    public int multiplierPercent = 50;
    public override void OnPickup(PlayerPerkManager player)
    {
        base.OnPickup(player);
        weaponsManager = ReferenceManager.BlakeHeroCharacter.GetComponent<WeaponsManager>();
        EnemyDeathMediator.Instance.PlayerCurrencyController.onAddPoints += MultiplyPoints;
    }

    public override void OnRemove()
    {
        EnemyDeathMediator.Instance.PlayerCurrencyController.onAddPoints -= MultiplyPoints;

    }

    private void MultiplyPoints(float addedPoints)
    {
        if (weaponsManager == null) return;

        if (weaponsManager.ActiveWeaponIndex != 0) return;

        EnemyDeathMediator.Instance.PlayerCurrencyController.AddPoints(addedPoints * ((float)multiplierPercent / 100f));
    }
}
