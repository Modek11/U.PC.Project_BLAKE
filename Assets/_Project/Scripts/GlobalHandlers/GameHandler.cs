using _Project.Scripts.Patterns;
using UnityEngine;

namespace _Project.Scripts.GlobalHandlers
{
    public class GameHandler : Singleton<GameHandler>
    {
        [SerializeField]
        private GameObject pausedGameCanvas;

        private bool isGamePaused = false;
        
        public bool IsGamePaused
        {
            get => isGamePaused;

            private set
            {
                isGamePaused = value;
                Time.timeScale = isGamePaused ? 0f : 1f;
            }
        }

        private void Start()
        {
            ShowPlayerControlsPopup();
        }

        public void PlayerWin()
        {
            Debug.LogError("PlayerWin logic is missing");
        }

        public void PlayerLose()
        {
            OpenPlayerUICanvas("YouLose_Canvas");
        }
        
        public void OpenPauseGameCanvas()
        {
            OpenPlayerUICanvas("PauseGame_Canvas");
        }
        
        public void OpenWeaponUpgradesCanvas()
        {
            OpenPlayerUICanvas("WeaponUpgrade_Canvas");
        }

        public void ShowPlayerControlsPopup()
        {
            OpenPlayerUICanvas("ControlsPopup_Canvas", false);
        }

        private void OpenPlayerUICanvas(string canvasName, bool pauseGame = true)
        {
            for(int i = 0; i < pausedGameCanvas.transform.childCount; i++)
            {
                var child = pausedGameCanvas.transform.GetChild(i).gameObject;
                child.SetActive(child.name == canvasName);
            }
            pausedGameCanvas.SetActive(true);

            IsGamePaused = pauseGame;
        }
    
        public void CloseAllCanvasAndUnpause()
        {
            pausedGameCanvas.SetActive(false);
            for(int i = 0; i < pausedGameCanvas.transform.childCount; i++)
            {
                var child = pausedGameCanvas.transform.GetChild(i).gameObject;
                child.SetActive(false);
            }

            IsGamePaused = false;
        }

        private void OnDestroy()
        {
            IsGamePaused = false;
        }
    }
}
