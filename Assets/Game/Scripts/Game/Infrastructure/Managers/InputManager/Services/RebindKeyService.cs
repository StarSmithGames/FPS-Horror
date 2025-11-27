using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public static class RebindKeyService
    {
        public static async UniTask Rebind( InputAction action, string compositePartName = null )
        {
            int index = FindBindingIndex( action, compositePartName );
            if ( index == -1 )
            {
                Debug.LogError( "[InputManager] Binding not found!" );
                return;
            }

            Debug.Log( "[InputManager] Waiting for input..." );

            InputControl newControl = await InputListeningService.WaitForAnyInput();
            if ( newControl == null )
            {
                Debug.Log( "[InputManager] Canceled" );
                return;
            }

            action.ApplyBindingOverride( index, newControl.path );

            Debug.Log( $"[InputManager] Rebind: {action.name} [{compositePartName}] → {newControl.path}" );
        }

        public static int FindBindingIndex( InputAction action, string partName )
        {
            if ( partName == null )
                return 0;

            for ( int i = 0; i < action.bindings.Count; i++ )
            {
                var b = action.bindings[ i ];
                if ( b.isPartOfComposite && b.name == partName )
                    return i;
            }

            return -1;
        }
        
        public static string FindBindingString( InputAction action, string partName )
        {
            if ( partName == null )
                return string.Empty;

            for ( int i = 0; i < action.bindings.Count; i++ )
            {
                var b = action.bindings[ i ];
                if ( b.isPartOfComposite && b.name == partName )
                {
                    return action.bindings[ i ].effectivePath;
                }
            }

            return string.Empty;
        }

        public static List< BindingInfo > GetBindingsForDevice( InputAction action, DeviceType deviceType )
        {
            var result = new List< BindingInfo >();

            string deviceName = deviceType.ToString();
            
            foreach ( var b in action.bindings )
            {
                if ( b.isComposite ) // композит — контейнер, пропускаем
                    continue;

                string path = b.effectivePath;
                if ( string.IsNullOrEmpty( path ) )
                    path = b.path;

                string device = InputControlPath.TryGetDeviceLayout( path );
                if ( device != deviceName )
                    continue;

                string key = InputControlPath.ToHumanReadableString( path, InputControlPath.HumanReadableStringOptions.OmitDevice );

                result.Add( new BindingInfo( device, path, key ) );
            }

            return result;
        }
    }
}