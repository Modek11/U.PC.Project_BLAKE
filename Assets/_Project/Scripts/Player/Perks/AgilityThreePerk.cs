using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgilityThreePerk", menuName = "Abilities/Perks/Agility Three")]

public class AgilityThreePerk : PerkScriptableObject
{
    [SerializeField]
    private PlayerMovement playerMovement;
    public int dashesToAdd = 1;
    public override void OnPickup(PlayerPerkManager player)
    {
        base.OnPickup(player);
        playerMovement = perkManager.GetComponent<PlayerMovement>();
        playerMovement?.AddDashes(dashesToAdd);
    }

    public override void OnRemove()
    {
        playerMovement?.RemoveDashes(dashesToAdd);
    }
}
