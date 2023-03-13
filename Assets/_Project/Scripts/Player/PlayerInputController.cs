using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour, PlayerInputSystem.IGameplayActions
{
    private PlayerInputSystem inputSystem;
    private void Awake()
    {
        inputSystem = new PlayerInputSystem();
        inputSystem.Gameplay.SetCallbacks(this);
        SetUpControls();
    }

    void SetUpControls()
    {
        inputSystem.Enable();

        //Shooting
    }

    public event Action<Vector2> movementEvent;

    public event Action onShootStartEvent;
    public event Action shootEvent;
    public event Action onShootCancelEvent;
    public event Action reloadEvent;
    public event Action<int> changeWeaponEvent;

    public event Action interactEvent;

    public void OnMovement(InputAction.CallbackContext context)
    {
        movementEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnChangeWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            changeWeaponEvent?.Invoke((int)context.ReadValue<float>() - 1);
        }
    }

    public void OnShooting(InputAction.CallbackContext context)
    {
        if (context.started) onShootStartEvent?.Invoke();
        if (context.canceled) onShootCancelEvent?.Invoke();

        if(context.performed)
        {
            shootEvent?.Invoke();
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            reloadEvent?.Invoke();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            interactEvent?.Invoke();
        }
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }
}
