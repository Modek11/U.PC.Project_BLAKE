using UnityEngine;

public class MenuElementsHandler : MonoBehaviour
{
    public void ContinueGame()
    {
        Debug.LogWarning("Continue Button has no implementation!");
    }

    public void NewGameNormalDifficulty()
    {
        ReferenceManager.SceneHandler.StartNewGame();
        ReferenceManager.SceneHandler.isNormalDifficulty = true;
        //Debug.LogWarning("New Game Normal Difficulty has no implementation!");
    }
    
    public void NewGameHardDifficulty()
    {
        ReferenceManager.SceneHandler.StartNewGame();
        ReferenceManager.SceneHandler.isNormalDifficulty = false;
        //Debug.LogWarning("New Game Hard Difficulty has no implementation!");
    }
    
    
    public void QuitButton()
    {
        ReferenceManager.SceneHandler.QuitGame();
    }
    
}
