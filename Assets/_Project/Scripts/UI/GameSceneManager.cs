using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GaneSceneManager : MonoBehaviour
{
    public static GaneSceneManager Instance { get; private set; }
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this);
            return;
        }
        
        Instance = this; 
        DontDestroyOnLoad(this);

        SceneManager.LoadSceneAsync(0,LoadSceneMode.Additive);

    }

    public void LoadScene(int sceneIndex)
    {
        var loadingScene = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        while (!loadingScene.isDone)
        {
            float progress = Mathf.Clamp01(loadingScene.progress / 0.9f);
            
        }
    }
}
