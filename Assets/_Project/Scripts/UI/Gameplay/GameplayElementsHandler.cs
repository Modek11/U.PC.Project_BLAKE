using UnityEngine;

public class GameplayElementsHandler : MonoBehaviour
{
    public void LeaveToMainMenu()
    {
        SceneHandler.Instance.LoadMainMenu();
    }

    public void PlayAgain()
    {
        SceneHandler.Instance.StartNewGame();
    }
    
    
}
