using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgilityTwoPerk", menuName = "Abilities/Perks/Agility Two")]
public class AgilityTwoPerk : PerkScriptableObject
{
    [SerializeField]
    private PlayerMovement playerMovement;
    public float dashReduction = 1;
    public override void OnPickup(PlayerPerkManager player)
    {
        base.OnPickup(player);
        playerMovement = perkManager.GetComponent<PlayerMovement>();
        playerMovement?.AddDashCooldownReduction(dashReduction);
    }

    public override void OnRemove()
    {
        playerMovement?.RemoveDashCooldownReduction(dashReduction);
    }
}
