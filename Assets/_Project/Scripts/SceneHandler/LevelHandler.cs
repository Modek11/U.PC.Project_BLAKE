using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts;
using _Project.Scripts.SceneHandler;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SceneHandler))]
public class LevelHandler : MonoBehaviour
{
    [SerializeField]
    private int levelIndex = 0;
    [SerializeField]
    private LevelList levelNames;
    private SceneHandler sceneHandler;

    private void Awake()
    {

        
        
    }

    private void Start()
    {
        if (ReferenceManager.LevelHandler != null && ReferenceManager.LevelHandler != this)
        {
            Destroy(gameObject);
        }
        else
        {
            ReferenceManager.LevelHandler = this;
            DontDestroyOnLoad(ReferenceManager.LevelHandler);
        }

        sceneHandler = GetComponent<SceneHandler>();

        string currentSceneName = SceneManager.GetActiveScene().name;
        if (levelNames.levelNames.Contains(currentSceneName))
        {
            for (int i = 0; i < levelNames.levelNames.Length; i++)
            {
                if (levelNames.levelNames[i] == currentSceneName)
                {
                    levelIndex = i;
                    break;
                }
            }
        }
    }

    public void GoToNextLevel()
    {
        if (levelIndex == levelNames.levelNames.Length - 1)
        {
            EndRun();
            return;
        }

        levelIndex++;
        sceneHandler.LoadNewLevel(levelNames.levelNames[levelIndex]);
    }

    public void EndRun()
    {
        levelIndex = 0;
        Destroy(ReferenceManager.PlayerInputController.gameObject);
        sceneHandler.LoadMainMenu();
    }

    public void ResetValues()
    {
        levelIndex = 0;
    }
}
