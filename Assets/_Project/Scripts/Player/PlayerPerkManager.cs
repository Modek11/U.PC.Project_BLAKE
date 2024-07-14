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
        if (perk == null) return;
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

    public List<PerkScriptableObject> GetPerkList()
    {
        return perkScripts;
    }

    private void Start()
    {
        foreach(var perk in perksToStartWith)
        {
            AddPerk(perk);
        }
    }
}
