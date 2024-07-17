using _Project.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePerkButton : MonoBehaviour
{
    public PerkScriptableObject perk;

    private bool activated = false;

    public void GiveOrRemove()
    {
        var player = ReferenceManager.BlakeHeroCharacter?.GetComponent<PlayerPerkManager>();
        if (player == null) return;

        if(!activated)
        {
            player.AddPerk(perk);
        } else
        {
            player.RemovePerk(perk);
        }

        activated = !activated;
    }
}
