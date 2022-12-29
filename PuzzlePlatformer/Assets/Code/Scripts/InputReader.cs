using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Scripts
{
    public class InputReader : MonoBehaviour, Controls.IPlayerActions
    {
        public Vector3 MovementValue { get; private set; }

        public event Action OnJump;
        public event Action OnHold;
        public event Action OnDrop;
        public event Action OnHit;
        public event Action OnPick;
        

        private Controls _controls;

        private void Start()
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
            _controls.Player.Enable();
        }

        private void OnDestroy()
        {
            _controls.Player.Disable();
        }

        public void OnJumping(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            OnJump?.Invoke();
        }

        public void OnMoving(InputAction.CallbackContext context)
        {
            MovementValue = context.ReadValue<Vector2>();
        }

        public void OnLooking(InputAction.CallbackContext context)
        {
        }

        public void OnHolding(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                OnDrop?.Invoke();
            }
            
            if (!context.performed) return;
            
            OnHold?.Invoke();
        }

        public void OnPicking(InputAction.CallbackContext context)
        {
            OnPick?.Invoke();
        }

        public void OnHitting(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            OnHit?.Invoke();
        }
    }
}