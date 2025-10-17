using System;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public sealed class InputActionVoidWrap : InputActionWrap
    {
        private Action _onStarted;
        private Action _onPerformed;
        private Action _onCanceled;
        
        public InputActionVoidWrap( InputAction input, Action onPerformed = null, Action onStarted = null, Action onCanceled = null ) : base( input )
        {
            _onStarted = onStarted;
            _onPerformed = onPerformed;
            _onCanceled = onCanceled;
        }
        
        public override void Dispose()
        {
            base.Dispose();

            _onStarted = null;
            _onPerformed = null;
            _onCanceled = null;
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