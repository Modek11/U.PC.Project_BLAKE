using SickDev.CommandSystem;
using SickDev.DevConsole;

namespace _Project.Scripts.ConsoleCommands
{
    public class FloorCommands : BaseCommand
    {
        private readonly string NAME = "Floor";
        protected override void Initialize()
        {
            base.Initialize();
            
            DevConsole.singleton.AddCommand(command = new ActionCommand(ResetFloor) { className = NAME });
            commandsHolder.Add(command);
        }

        private void ResetFloor()
        {
            ReferenceManager.SceneHandler.StartNewGame();
        }
    }
}
