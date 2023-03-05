using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private int _mainMenuSceneIndex = 0;
    private int _gameSceneIndex = 1;

    public void StartGameButton()
    {
        SceneManager.LoadScene(_gameSceneIndex);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(_mainMenuSceneIndex);
    }

    public void QuitButton()
    {
        Application.Quit();
        
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
