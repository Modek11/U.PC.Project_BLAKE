using SickDev.CommandSystem;
using SickDev.DevConsole;
using UnityEngine;

namespace _Project.Scripts.ConsoleCommands
{
    public class PlayerCommands : BaseCommand
    {
        private readonly string NAME = "Player";

        private float shortDashValue = 0.3f;
        private float baseDashValue = float.MinValue;
        
        protected override void Initialize()
        {
            base.Initialize();
            
            DevConsole.singleton.AddCommand(new ActionCommand<bool>(GodMode) { className = NAME });
            DevConsole.singleton.AddCommand(new ActionCommand<bool>(ShortDash) { className = NAME });
        }

        private void GodMode(bool isEnabled)
        {
            ReferenceManager.BlakeHeroCharacter.SetGodMode(isEnabled);
        }
        
        private void ShortDash(bool isEnabled)
        {
            var playerMovement = ReferenceManager.BlakeHeroCharacter.GetComponent<PlayerMovement>();

            if (baseDashValue < 0)
            {
                baseDashValue = playerMovement.DashCooldown;
            }

            playerMovement.SetDashValue(isEnabled ? shortDashValue : baseDashValue);
        }
    }
}
