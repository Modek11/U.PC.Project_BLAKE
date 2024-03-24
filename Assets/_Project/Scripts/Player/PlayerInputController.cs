using System;
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
        if (ReferenceManager.PlayerInputController != null && ReferenceManager.PlayerInputController != this)
        {
            Destroy(gameObject);
        }
        else
        {
            ReferenceManager.PlayerInputController = this;
            DontDestroyOnLoad(ReferenceManager.PlayerInputController);
        }
    }

    void SetUpControls()
    {
        inputSystem.Enable();

        //Shooting
    }

    public event Action<Vector2> movementEvent;
    public event Action<Vector2> mousePositionEvent;

    public event Action onShootStartEvent;
    public event Action shootEvent;
    public event Action onShootCancelEvent;
    public event Action<int> changeWeaponEvent;
    public event Action interactEvent;
    public event Action onMapPressEvent; 
    public event Action onMapReleaseEvent;
    public event Action dashEvent;
    public event Action escapeButtonEvent;

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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            interactEvent?.Invoke();
        }
    }

    public void OnMap(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onMapPressEvent?.Invoke();
        }
        else if (context.canceled)
        {
            onMapReleaseEvent?.Invoke();
        }
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        mousePositionEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            dashEvent?.Invoke();
        }
    }

    public void OnEscapeButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            escapeButtonEvent?.Invoke();
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
