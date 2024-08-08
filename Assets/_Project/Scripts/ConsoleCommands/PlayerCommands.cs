using _Project.Scripts.GlobalHandlers;
using _Project.Scripts.PointsSystem;
using SickDev.CommandSystem;
using SickDev.DevConsole;

namespace _Project.Scripts.ConsoleCommands
{
    public class PlayerCommands : BaseCommand
    {
#if UNITY_EDITOR
        private readonly string NAME = "Player";

        private float shortDashValue = 0.3f;
        private float baseDashValue = float.MinValue;
        
        protected override void Initialize()
        {
            base.Initialize();
            
            DevConsole.singleton.AddCommand(command = new ActionCommand<bool>(GodMode) { className = NAME });
            commandsHolder.Add(command);
            
            DevConsole.singleton.AddCommand(command = new ActionCommand<bool>(TrueGodMode) { className = NAME });
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

        private void TrueGodMode(bool isEnabled)
        {
            foreach (var command1 in DevConsole.singleton.GetCommands())
            {
                if (command1.name is "Player.GodMode" or "Player.ShortDash" or "Weapons.InfiniteAmmo")
                {
                    command1.Execute(new object[] {isEnabled});
                }
                if (command1.name == "Weapons.SpawnGun" && isEnabled && command1.signature.parameters.Length == 1)
                {
                    command1.Execute(new object[] {"glock"});
                    command1.Execute(new object[] {"machine gun"});
                }
            }
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
#endif
    }
}
