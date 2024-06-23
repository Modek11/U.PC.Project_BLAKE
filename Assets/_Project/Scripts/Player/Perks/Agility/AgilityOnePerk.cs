using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgilityOnePerk", menuName = "Abilities/Perks/Agility One")]
public class AgilityOnePerk : PerkScriptableObject
{
    [SerializeField]
    private PlayerMovement playerMovement;
    public float speedBoost = 5;
    public override void OnPickup(PlayerPerkManager player)
    {
        base.OnPickup(player);
        playerMovement = perkManager.GetComponent<PlayerMovement>();
        playerMovement?.AddAdditionalSpeed(speedBoost);
    }

    public override void OnRemove()
    {
        playerMovement?.RemoveAdditionalSpeed(speedBoost);
    }
}
