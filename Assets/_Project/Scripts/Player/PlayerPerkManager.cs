using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPerkManager : MonoBehaviour
{
    [SerializeField]
    private List<PerkScriptableObject> perkScripts = new List<PerkScriptableObject>();
    [SerializeField]
    private List<PerkScriptableObject> perksToStartWith = new List<PerkScriptableObject>();

    public void AddPerk(PerkScriptableObject perk)
    {
        perkScripts.Add(perk);
        perk.OnPickup(this);
    }

    public void RemovePerk(PerkScriptableObject perk)
    {
        perkScripts.Remove(perk);
        perk.OnRemove();
    }

    public void RemovePerkByName(string name)
    {
        var perksToRemove = perkScripts.Where(x => x.perkName == name).ToList();

        foreach (var perk in perksToRemove)
        {
            perk.OnRemove();
            perkScripts.Remove(perk);
        }
    }

    private void Start()
    {
        foreach(var perk in perksToStartWith)
        {
            AddPerk(perk);
        }
    }
}
