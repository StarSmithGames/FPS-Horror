using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    [ CreateAssetMenu( fileName = "InputKeyAction", menuName = "Game/InputKeyAction" ) ]
    public sealed class InputKeyAction : ScriptableObject
    {
        [ SerializeField ] private InputActionAsset _inputActionAsset;
        [ SerializeField ] private string _actionMapName;
        [ SerializeField ] private string _actionName;

        public InputAction InputAction
        {
            get
            {
                if ( _cachedAction == null && _inputActionAsset != null )
                {
                    _cachedAction = _inputActionAsset.FindActionMap( _actionMapName ).FindAction( _actionName );
                }
                return _cachedAction;
            }
        }
        [ NonSerialized ] private InputAction _cachedAction;

        public string GetDisplayKey()
        {
            for ( int i = 0; i < InputAction.bindings.Count; i++ )
            {
                var binding = InputAction.bindings[i];

                if (binding.path.StartsWith("<Keyboard>"))
                {
                    string path = binding.effectivePath;
                    var keyName = path.Substring( path.LastIndexOf( '/' ) + 1 ).ToUpper();

                    return keyName.Length > 1 ? string.Concat( keyName.Take( 3 ) ) : keyName;
                }
            }
        
            return string.Empty;
        }
    }
}