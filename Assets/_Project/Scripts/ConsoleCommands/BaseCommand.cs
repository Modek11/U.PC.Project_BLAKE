using SickDev.DevConsole;
using UnityEngine;

namespace _Project.Scripts.ConsoleCommands
{
    public class BaseCommand : MonoBehaviour
    {
        private bool isInitialized = false;
        
        private void OnEnable()
        {
            DevConsole.singleton.onOpenStateChanged += OnOpenStateChanged;
            
            if (!isInitialized)
            {
                Initialize();
            }
            else
            {
                Debug.LogWarning($"Trying to initialize commands too many times!");
            }
        }

        private void OnDisable()
        {
            DevConsole.singleton.onOpenStateChanged -= OnOpenStateChanged;
        }

        private void OnOpenStateChanged(bool isOpen)
        {
            if (isOpen)
            {
                ReferenceManager.PlayerInputController.DisableInputSystem();
            }
            else
            {
                ReferenceManager.PlayerInputController.EnableInputSystem();
            }
        }

        protected virtual void Initialize()
        {
            isInitialized = true;
        }
    }
}
