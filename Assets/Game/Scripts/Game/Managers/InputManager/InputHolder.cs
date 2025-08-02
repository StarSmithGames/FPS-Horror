using System;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public sealed class InputHolder
    {
        private bool _isHold;
        
        private readonly InputAction _input;
        private readonly Action _onStartHold;
        private readonly Action _onHold;
        private readonly Action _onEndHold;
        
        public InputHolder( InputAction input, Action onStartHold = null, Action onHold = null, Action onEndHold = null )
        {
            _input = input ?? throw new ArgumentNullException( nameof(input) );
            _onStartHold = onStartHold;
            _onHold = onHold;
            _onEndHold = onEndHold;
        }

        public void Initialize()
        {
            _input.performed += InputPerformedHandler;
            _input.canceled += InputCanceledHandler;
            _input.Enable();
        }

        public void Dispose()
        {
            _input.performed -= InputPerformedHandler;
            _input.canceled -= InputCanceledHandler;
            _input.Disable();
        }

        public void Tick()
        {
            if ( _isHold )
            {
                _onHold?.Invoke();
            }
        }

        private void InputPerformedHandler( InputAction.CallbackContext context )
        {
            _onStartHold?.Invoke();
            _isHold = true;
        }
        
        private void InputCanceledHandler( InputAction.CallbackContext context )
        {
            _isHold = false;
            _onEndHold?.Invoke();
        }
    }
}