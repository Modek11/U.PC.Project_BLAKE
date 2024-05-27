using System;
using _Project.Scripts.Patterns;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Player
{
    public class PlayerInputController : Singleton<PlayerInputController>, PlayerInputSystem.IGameplayActions
    {
        private PlayerInputSystem inputSystem;

        protected override void Awake()
        {
            base.Awake();
            
            inputSystem = new PlayerInputSystem();
            inputSystem.Gameplay.SetCallbacks(this);
            SetUpControls();
            ReferenceManager.PlayerInputController = this;
        }

        void SetUpControls()
        {
            inputSystem.Enable();

            //Shooting
        }

        public event Action<Vector2> movementEvent;
        public event Action<Vector2> mousePositionEvent;

        public event Action onShootBasicStartEvent;
        public event Action shootBasicEvent;
        public event Action onShootBasicCancelEvent;
        public event Action onShootStrongStartEvent;
        public event Action shootStrongEvent;
        public event Action onShootStrongCancelEvent;
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

        public void OnShootBasic(InputAction.CallbackContext context)
        {
            if (context.started) onShootBasicStartEvent?.Invoke();
            if (context.canceled) onShootBasicCancelEvent?.Invoke();

            if(context.performed)
            {
                shootBasicEvent?.Invoke();
            }
        }
        
        public void OnShootStrong(InputAction.CallbackContext context)
        {
            if (context.started) onShootStrongStartEvent?.Invoke();
            if (context.canceled) onShootStrongCancelEvent?.Invoke();

            if(context.performed)
            {
                shootStrongEvent?.Invoke();
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
}
