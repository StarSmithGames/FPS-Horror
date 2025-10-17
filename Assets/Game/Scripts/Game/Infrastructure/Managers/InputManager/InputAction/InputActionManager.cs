using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public static class InputActionManager
    {
        private static Dictionary< InputAction, List< InputActionWrap > > _inputActions = new();
        
        public static InputActionVoidWrap CreateInputActionWrap( InputAction inputAction, Action onPerformed = null, Action onStarted = null, Action onCanceled = null )
        {
            var wrap = new InputActionVoidWrap( inputAction, onPerformed, onStarted, onCanceled );
            wrap.Input.Enable();
            if ( _inputActions.TryGetValue( inputAction, out var list ) )
            {
                list.Add( wrap );
                return wrap;
            }
            _inputActions.Add( inputAction, new(){ wrap } );
            return wrap;
        }
        
        public static InputActionValueWrap< T > CreateInputActionWrap< T >( InputAction inputAction, Action< T > onPerformed = null, Action< T > onStarted = null, Action< T > onCanceled = null )
            where T : struct
        {
            var wrap = new InputActionValueWrap< T >( inputAction, onPerformed, onStarted, onCanceled );
            wrap.Input.Enable();
            if ( _inputActions.TryGetValue( inputAction, out var list ) )
            {
                list.Add( wrap );
                return wrap;
            }
            _inputActions.Add( inputAction, new(){ wrap } );
            return wrap;
        }

        public static void RemoveInputActionWrap( InputActionWrap wrap )
        {
            wrap.Dispose();
            _inputActions[ wrap.Input ].Remove( wrap );
            if ( _inputActions[ wrap.Input ].Count == 0 )
            {
                wrap.Input.Disable();
                _inputActions.Remove( wrap.Input );
            }
        }
    }
}