using _Project.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugControls : MonoBehaviour, PlayerInputSystem.IDebugActions
{
    private PlayerInputSystem inputSystem;

    private void Awake()
    {
        inputSystem = new PlayerInputSystem();
        inputSystem.Debug.SetCallbacks(this);
    }

    [SerializeField]
    private GameObject PerkMenu;
    public void OnPerkMenu(InputAction.CallbackContext context)
    {
        if(PerkMenu != null)
        {
            PerkMenu.SetActive(!PerkMenu.activeInHierarchy);
        }
    }

    public void EnableInputSystem()
    {
        inputSystem.Enable();
    }

    public void DisableInputSystem()
    {
        inputSystem.Disable();
    }

    private void OnEnable()
    {
        EnableInputSystem();
    }

    private void OnDisable()
    {
        DisableInputSystem();
    }
}
