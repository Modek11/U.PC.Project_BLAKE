using UnityEngine;

public class PausedGameController : MonoBehaviour
{
    private bool _gamePaused = false;
    

    private void Start()
    {
        ReferenceManager.PlayerInputController.escapeButtonEvent += PauseGame;
    }

    public void PauseGame()
    {
        _gamePaused = !_gamePaused;
        
        if (_gamePaused)
        {
            GameHandler.Instance.PlayerPause();
            Time.timeScale = 0f;
        }
        else
        {
            GameHandler.Instance.ClosePausedGameCanvas();
            Time.timeScale = 1f;
        }
    }
}
