using System;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public class InputActionValueWrap< T > : InputActionWrap
        where T : struct
    {
        private readonly Action< T > _onStarted;
        private readonly Action< T > _onPerformed;
        private readonly Action< T > _onCanceled;
        
        public InputActionValueWrap( InputAction input, Action< T > onPerformed = null, Action< T > onStarted = null, Action< T > onCanceled = null ) : base( input )
        {
            _onStarted = onStarted;
            _onPerformed = onPerformed;
            _onCanceled = onCanceled;
        }

        protected override void OnStarted()
        {
            _onStarted?.Invoke( Input.ReadValue< T >() );
        }

        protected override void OnPerformed()
        {
            _onPerformed?.Invoke( Input.ReadValue< T >() );
        }

        protected override void OnCanceled()
        {
            _onCanceled?.Invoke( Input.ReadValue< T >() );
        }
    }
}