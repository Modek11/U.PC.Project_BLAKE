using System;
using System.Linq;
using _Project.Scripts.Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.SceneHandler
{
    [RequireComponent(typeof(SceneHandler))]
    public class LevelHandler : Singleton<LevelHandler>
    public event Action onNextLevel;
    {
        [SerializeField]
        private int levelIndex = 0;
        [SerializeField]
        private LevelList levelNames;
        private SceneHandler sceneHandler;

        private void Start()
        {
            ReferenceManager.LevelHandler = this;

            sceneHandler = ReferenceManager.SceneHandler;

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

        onNextLevel?.Invoke();
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
}
