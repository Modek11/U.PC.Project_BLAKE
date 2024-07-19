using _Project.Scripts;
using _Project.Scripts.PointsSystem;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.GlobalHandlers;
using UnityEngine;

[CreateAssetMenu(fileName = "PermanenceThreePerk", menuName = "Abilities/Perks/Permanence Three")]

public class PermanenceThree : PerkScriptableObject
{
    private BlakeCharacter playerScript;
    private bool isActive = false;
    public float comboCounterForShield = 5f;

    public override void OnPickup(PlayerPerkManager player)
    {
        base.OnPickup(player);
        playerScript = ReferenceManager.BlakeHeroCharacter;
        EnemyDeathMediator.Instance.ComboController.comboShieldEvent += ActivateShield;
        EnemyDeathMediator.Instance.ComboController.OnComboTimerEnd += ResetTimer;
    }

    public override void OnRemove()
    {
        EnemyDeathMediator.Instance.ComboController.comboShieldEvent -= ActivateShield;
        EnemyDeathMediator.Instance.ComboController.OnComboTimerEnd -= ResetTimer;
    }

    private void ActivateShield(float comboCounter)
    {
        if (!isActive && comboCounter >= comboCounterForShield)
        {
            playerScript.ActivateShield();
            isActive = true;
        }
    }

    private void ResetTimer()
    {
        isActive = false;
    }
}
