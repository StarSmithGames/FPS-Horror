using System;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public sealed class InputActionWrap
    {
        public InputAction Input { get; }
        
        private readonly Action _onStarted;
        private readonly Action _onEnded;
        
        public InputActionWrap( InputAction input, Action onStartHold = null, Action onEndHold = null )
        {
            Input = input ?? throw new ArgumentNullException( nameof(input) );
            _onStarted = onStartHold;
            _onEnded = onEndHold;
        }

        public void Enable()
        {
            Input.performed += InputPerformedHandler;
            Input.canceled += InputCanceledHandler;
        }

        public void Disable()
        {
            Input.performed -= InputPerformedHandler;
            Input.canceled -= InputCanceledHandler;
        }

        private void InputPerformedHandler( InputAction.CallbackContext context )
        {
            _onStarted?.Invoke();
        }
        
        private void InputCanceledHandler( InputAction.CallbackContext context )
        {
            _onEnded?.Invoke();
        }
    }
}