using _Project.Scripts;
using _Project.Scripts.PointsSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PermanenceOnePerk", menuName = "Abilities/Perks/Permanence One")]

public class PermamenceOne : PerkScriptableObject
{
    public float percentageRecovered;
    public override void OnPickup(PlayerPerkManager player)
    {
        base.OnPickup(player);
        EnemyDeathMediator.Instance.PlayerCurrencyController.AddDeathModifierPercentage(-10f);
    }

    public override void OnRemove()
    {
        EnemyDeathMediator.Instance.PlayerCurrencyController.AddDeathModifierPercentage(10f);

    }
}
