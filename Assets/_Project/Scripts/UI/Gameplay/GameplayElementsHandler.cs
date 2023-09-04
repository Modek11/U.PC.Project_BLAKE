using UnityEngine;

public class GameplayElementsHandler : MonoBehaviour
{
    public void LeaveToMainMenu()
    {
        ReferenceManager.SceneHandler.LoadMainMenu();
    }

    public void PlayAgain()
    {
        ReferenceManager.SceneHandler.StartNewGame();
    }

    public void ClosePausedGameCanvas()
    {
        GameHandler.Instance.ClosePausedGameCanvas();
    }
}
