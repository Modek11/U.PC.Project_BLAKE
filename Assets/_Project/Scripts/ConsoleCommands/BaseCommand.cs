using System.Collections.Generic;
using _Project.Scripts.GlobalHandlers;
using SickDev.CommandSystem;
using SickDev.DevConsole;
using UnityEngine;

namespace _Project.Scripts.ConsoleCommands
{
    public class BaseCommand : MonoBehaviour
    {
        private bool isInitialized = false;
        
        protected List<Command> commandsHolder = new List<Command>();
        protected Command command;
        
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
            DevConsole.singleton.RemoveCommands(commandsHolder.ToArray());
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
            DontDestroyOnLoad(this);
        }
    }
}
