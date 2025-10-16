using System;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public sealed class InputActionVoidWrap : InputActionWrap
    {
        private readonly Action _onStarted;
        private readonly Action _onPerformed;
        private readonly Action _onCanceled;
        
        public InputActionVoidWrap( InputAction input, Action onPerformed = null, Action onStarted = null, Action onCanceled = null ) : base( input )
        {
            _onStarted = onStarted;
            _onPerformed = onPerformed;
            _onCanceled = onCanceled;
        }

        protected override void OnStarted()
        {
            _onStarted?.Invoke();
        }

        protected override void OnPerformed()
        {
            _onPerformed?.Invoke();
        }

        protected override void OnCanceled()
        {
            _onCanceled?.Invoke();
        }
    }
}