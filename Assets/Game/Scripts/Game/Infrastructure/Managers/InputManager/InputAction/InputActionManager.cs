using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public static class InputActionManager
    {
        private static Dictionary< InputAction, List< InputActionWrap > > _inputActions = new();
        
        public static InputActionVoidWrap CreateInputActionWrap( InputAction inputAction, Action onPerformed = null, Action onCanceled = null )
        {
            var wrap = new InputActionVoidWrap( inputAction, onPerformed, onCanceled );
            AddInputActionWrap( wrap );
            return wrap;
        }
        
        public static InputActionValueWrap< T > CreateInputActionWrap< T >( InputAction inputAction, Action< T > onPerformed = null, Action< T > onCanceled = null )
            where T : struct
        {
            var wrap = new InputActionValueWrap< T >( inputAction, onPerformed, onCanceled );
            AddInputActionWrap( wrap );
            return wrap;
        }

        public static void AddInputActionWrap( InputActionWrap wrap )
        {
            wrap.Enable();
            wrap.Input.Enable();
            if ( _inputActions.TryGetValue( wrap.Input, out var list ) )
            {
                if ( list.Contains( wrap ) ) return;
                list.Add( wrap );
                return;
            }
            _inputActions.Add( wrap.Input, new(){ wrap } );
        }
        
        public static void RemoveInputActionWrap( InputActionWrap wrap )
        {
            if ( !_inputActions.ContainsKey( wrap.Input ) ) return;
            
            _inputActions[ wrap.Input ].Remove( wrap );
            if ( _inputActions[ wrap.Input ].Count == 0 )
            {
                wrap.Disable();
                wrap.Input.Disable();
                _inputActions.Remove( wrap.Input );
            }
        }
    }
}