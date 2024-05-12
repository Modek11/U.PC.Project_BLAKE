using SickDev.CommandSystem;
using SickDev.DevConsole;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.ConsoleCommands
{
    public class PerkCommands : BaseCommand
    {

        private readonly string NAME = "Perks";
        [SerializeField]
        private List<PerkScriptableObject> perkList = new();

        protected override void Initialize()
        {
            base.Initialize();

            DevConsole.singleton.AddCommand(command = new ActionCommand<string>(AddPerk) { className = NAME });
            commandsHolder.Add(command);

            DevConsole.singleton.AddCommand(command = new ActionCommand<string>(RemovePerk) { className = NAME });
            commandsHolder.Add(command);
        }

        private void AddPerk(string perkName)
        {
            foreach(var perk in perkList)
            {
                if(perk.perkName == perkName)
                {
                    ReferenceManager.BlakeHeroCharacter?.GetComponent<PlayerPerkManager>()?.AddPerk(perk);
                    return;
                }
            }
        }

        private void RemovePerk(string perkName)
        {
            ReferenceManager.BlakeHeroCharacter?.GetComponent<PlayerPerkManager>()?.RemovePerkByName(perkName);
        }
    }
}
