using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public static class InputActionManager
    {
        private static Dictionary< InputAction, List< InputActionWrap > > _inputActions = new();
        
        public static InputActionWrap CreateInputActionWrap( InputAction inputAction, Action onStartHold = null, Action onEndHold = null )
        {
            var wrap = new InputActionWrap( inputAction, onStartHold, onEndHold );
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
            _inputActions[ wrap.Input ].Remove( wrap );
            if ( _inputActions[ wrap.Input ].Count == 0 )
            {
                wrap.Input.Disable();
                _inputActions.Remove( wrap.Input );
            }
        }
    }
}