using _Project.Scripts.GlobalHandlers;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.MainMenu
{
    public class MenuNavigationButton : MonoBehaviour
    {
        [SerializeField] private ButtonOptionEnum buttonOptionEnum;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(ButtonClickedEvent);
        }

        private void ButtonClickedEvent()
        {
            switch (buttonOptionEnum)
            { 
                case ButtonOptionEnum.Undefined:
                    Undefined();
                    break;
                case ButtonOptionEnum.GameModeNormal:
                    GameModeNormal();
                    break;
                case ButtonOptionEnum.GameModeHard:
                    GameModeHard();
                    break;
                case ButtonOptionEnum.GameContinue:
                    GameContinue();
                    break;
                case ButtonOptionEnum.GameQuit:
                    GameQuit();
                    break;
            }
        }

        private void Undefined()
        {
            Debug.LogError("No button option chosen");
        }

        private void GameModeNormal()
        {
            ReferenceManager.SceneHandler.isNormalDifficulty = true;
            ReferenceManager.SceneHandler.StartNewGame();
        }

        private void GameModeHard()
        {
            ReferenceManager.SceneHandler.isNormalDifficulty = false;
            ReferenceManager.SceneHandler.StartNewGame();
        }

        private void GameContinue()
        {
            Debug.LogWarning("Continue Button has no implementation!");
        }

        private void GameQuit()
        {
            ReferenceManager.SceneHandler.QuitGame();
        }
    }
}
