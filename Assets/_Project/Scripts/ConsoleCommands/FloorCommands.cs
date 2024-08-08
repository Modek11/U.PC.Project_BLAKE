using _Project.Scripts.GlobalHandlers;
using SickDev.CommandSystem;
using SickDev.DevConsole;

namespace _Project.Scripts.ConsoleCommands
{
    public class FloorCommands : BaseCommand
    {
        private readonly string NAME = "Floor";
        protected override void Initialize()
        {
            DevConsole.singleton.AddCommand(command = new ActionCommand(ResetFloor) { className = NAME });
            commandsHolder.Add(command);

            DevConsole.singleton.AddCommand(command = new ActionCommand(NextLevel) { className = NAME });
            commandsHolder.Add(command);
            
            base.Initialize();
        }

        private void ResetFloor()
        {
            ReferenceManager.SceneHandler.StartNewGame();
        }

        private void NextLevel()
        {
            ReferenceManager.LevelHandler.GoToNextLevel();
        }
    }
}
