using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
public class PausedGameController : MonoBehaviour
{
    private PlayerInputController _playerInputController;
    private bool _gamePaused = false;
    

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        _playerInputController.escapeButtonEvent += PauseGame;
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
