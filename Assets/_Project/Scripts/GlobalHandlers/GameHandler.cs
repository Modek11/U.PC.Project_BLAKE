using _Project.Scripts.Patterns;
using UnityEngine;

namespace _Project.Scripts.GlobalHandlers
{
    public class GameHandler : Singleton<GameHandler>
    {
        private const string YOU_LOSE = "YouLose_Canvas";
        private const string PAUSE_GAME = "PauseGame_Canvas";
        private const string WEAPON_UPGRADE = "WeaponUpgrade_Canvas";
        private const string CONTROLS_POPUP = "ControlsPopup_Canvas";
        
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

        public GameObject PlayerLose()
        {
            return OpenPlayerUICanvas(YOU_LOSE);
        }
        
        public GameObject OpenPauseGameCanvas()
        {
            return OpenPlayerUICanvas(PAUSE_GAME);
        }
        
        public GameObject OpenWeaponUpgradesCanvas()
        {
            return OpenPlayerUICanvas(WEAPON_UPGRADE);
        }

        public GameObject ShowPlayerControlsPopup()
        {
            return OpenPlayerUICanvas(CONTROLS_POPUP, false);
        }

        private GameObject OpenPlayerUICanvas(string canvasName, bool pauseGame = true)
        {
            GameObject openedCanvas = null;
            for (var i = 0; i < pausedGameCanvas.transform.childCount; i++)
            {
                var child = pausedGameCanvas.transform.GetChild(i).gameObject;
                var canvasFound = child.name == canvasName;
                
                if(canvasFound)
                {
                    openedCanvas = child;
                }
                
                child.SetActive(canvasFound);
            }

            pausedGameCanvas.SetActive(true);

            IsGamePaused = pauseGame;
            return openedCanvas;
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
