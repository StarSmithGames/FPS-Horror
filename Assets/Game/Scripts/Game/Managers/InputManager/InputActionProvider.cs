using Game.Servies;
using System;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public sealed class InputActionProvider
    {
        private ActionProvider _actionProvider = new();
        
        private readonly InputAction _input;
        private readonly Action _action;
        private readonly float _duration;
        private readonly Action _onStartHold;
        private readonly Action _onEndHold;
        private readonly Func< bool > _breaker = null;
        private readonly Action< float > _progress = null;
        private readonly Action< bool > _callback = null;
        
        public InputActionProvider(
            InputAction input,
            Action action,
            float duration = 0.24f,
            Action onStartHold = null,
            Action onEndHold = null,
            Func< bool > breaker = null,
            Action< float > progress = null,
            Action< bool > callback = null
            )
        {
            _input = input ?? throw new ArgumentNullException( nameof(input) );
            _action = action ?? throw new ArgumentNullException( nameof(action) );
            _duration = duration;
            _onStartHold = onStartHold;
            _onEndHold = onEndHold;
            _breaker = breaker;
            _progress = progress;
            _callback = callback;
        }

        public void Enable()
        {
            _input.performed += InputPerformedHandler;
            _input.canceled += InputCanceledHandler;
            _input.Enable();
        }

        public void Disable()
        {
            _input.performed -= InputPerformedHandler;
            _input.canceled -= InputCanceledHandler;
            _input.Disable();
            
            _actionProvider.Stop();
        }
        
        private void InputPerformedHandler( InputAction.CallbackContext context )
        {
            _onStartHold?.Invoke();
            _actionProvider.ProvideAction( _action, _duration, _breaker, _progress, _callback );
        }
        
        private void InputCanceledHandler( InputAction.CallbackContext context )
        {
            _actionProvider.Stop();
            _onEndHold?.Invoke();
        }
    }
}