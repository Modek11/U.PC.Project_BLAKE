using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
public class PausedGameController : MonoBehaviour
{
    [SerializeField] private GameObject pausedGameCanvas;
    
    private PlayerInputController _playerInputController;
    private bool _gamePaused = false;
    

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        _playerInputController.escapeButtonEvent += EscapeButtonHandler;
    }

    public void EscapeButtonHandler()
    {
        _gamePaused = !_gamePaused;
        
        if (_gamePaused)
        {
            pausedGameCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pausedGameCanvas.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
