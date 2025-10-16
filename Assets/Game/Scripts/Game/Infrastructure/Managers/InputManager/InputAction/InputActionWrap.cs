using System;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public abstract class InputActionWrap
    {
        public InputAction Input { get; }

        public InputActionWrap( InputAction input )
        {
            Input = input ?? throw new ArgumentNullException( nameof(input) );
        }

        public void Enable()
        {
            Input.started += InputStartedHandler;
            Input.performed += InputPerformedHandler;
            Input.canceled += InputCanceledHandler;
        }

        public void Disable()
        {
            Input.started -= InputStartedHandler;
            Input.performed -= InputPerformedHandler;
            Input.canceled -= InputCanceledHandler;
        }

        protected abstract void OnStarted();
        protected abstract void OnPerformed();
        protected abstract void OnCanceled();

        private void InputStartedHandler( InputAction.CallbackContext context )
        {
            OnStarted();
        }
        
        private void InputPerformedHandler( InputAction.CallbackContext context )
        {
            OnPerformed();
        }

        private void InputCanceledHandler( InputAction.CallbackContext context )
        {
            OnCanceled();
        }
    }
}