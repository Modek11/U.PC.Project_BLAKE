using _Project.Scripts.PointsSystem;
using _Project.Scripts;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.GlobalHandlers;
using UnityEngine;

[CreateAssetMenu(fileName = "PermanenceTwoPerk", menuName = "Abilities/Perks/Permanence Two")]

public class PermanenceTwo : PerkScriptableObject
{
    private BlakeCharacter playerScript;
    public override void OnPickup(PlayerPerkManager player)
    {
        base.OnPickup(player);
        playerScript = ReferenceManager.BlakeHeroCharacter;
        ReferenceManager.LevelHandler.onNextLevel += AddRespawn;
    }

    public override void OnRemove()
    {
        ReferenceManager.LevelHandler.onNextLevel -= AddRespawn;
    }

    private void AddRespawn() {
        playerScript?.AddRespawnCounter();
    }
}
