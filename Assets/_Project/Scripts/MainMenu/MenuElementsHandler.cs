using System;
using UnityEngine;

public class MenuElementsHandler : MonoBehaviour
{
    public void ContinueGame()
    {
        Debug.LogWarning("Continue Button has no implementation!");
    }

    public void NewGameNormalDifficulty()
    {
        Debug.LogWarning("New Game Normal Difficulty has no implementation!");
    }
    
    public void NewGameHardDifficulty()
    {
        Debug.LogWarning("New Game Hard Difficulty has no implementation!");
    }
    
    
    public void QuitButton()
    {
        Application.Quit();
        
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
