using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkScriptableObject : ScriptableObject
{
    public string perkName;
    protected PlayerPerkManager perkManager;
    public virtual void OnPickup(PlayerPerkManager player)
    {
        this.perkManager = player;
    }

    public virtual void Activate()
    {

    }

    public virtual void OnRemove()
    {

    }
}
