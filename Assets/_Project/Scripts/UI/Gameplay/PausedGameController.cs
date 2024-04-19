using _Project.Scripts;
using _Project.Scripts.Floor_Generation;
using _Project.Scripts.GameHandler;
using UnityEngine;

public class PausedGameController : MonoBehaviour
{

    [SerializeField]
    private FloorManager floorManager;

    private bool _gamePaused = false;
    
    private void Start()
    {
        floorManager.FloorGeneratorEnd += FloorManagerOnFloorGeneratorEnd;
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

    private void FloorManagerOnFloorGeneratorEnd(Transform playerTransform, Transform cameraFollowTransform)
    {
        ReferenceManager.PlayerInputController.escapeButtonEvent += PauseGame;
    }

}
