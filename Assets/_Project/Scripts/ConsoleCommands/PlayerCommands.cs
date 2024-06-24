using _Project.Scripts.PointsSystem;
using SickDev.CommandSystem;
using SickDev.DevConsole;

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
            
            DevConsole.singleton.AddCommand(command = new ActionCommand<bool>(GodMode) { className = NAME });
            commandsHolder.Add(command);
            
            DevConsole.singleton.AddCommand(command = new ActionCommand<bool>(ShortDash) { className = NAME });
            commandsHolder.Add(command);

            DevConsole.singleton.AddCommand(command = new ActionCommand(MaxCombo) { className = NAME });
            commandsHolder.Add(command);
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

        private void MaxCombo()
        {
            EnemyDeathMediator.Instance.ComboController.SetMaxCombo();
            EnemyDeathMediator.Instance.RegisterEnemyDeath(1, EnemyTypeEnum.EnemyBatonMelee);
            EnemyDeathMediator.Instance.RegisterEnemyDeath(1, EnemyTypeEnum.EnemyBatonMelee);
        }
    }
}
