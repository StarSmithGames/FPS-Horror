using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public static class InputManager
    {
        public static event Action OnControllerChanged;
        
        public static event Action OnJump;
        
        public static GameplayInputs Inputs { get; private set; }

        public static bool IsMouse => Mouse.current != null;
        public static bool IsController => Gamepad.current != null && Gamepad.current.enabled;
        
        public static float scrolling, MouseX, MouseY, ControllerX, ControllerY;

        private static CancellationTokenSource _cancellationTokenSource;
        private static List< InputActionHolder > _inputHolders = new();
        
        public static void Initialize()
        {
            Inputs = new GameplayInputs();
            Inputs.Enable();

            InputSystem.onDeviceChange += DeviceChangedHandler;
            
            Inputs.Player.Jump.started += JumStartedHandler;
            
            ToggleGameControls( true );
            ToggleUIControls( false );

            _cancellationTokenSource = new();
            Tick( _cancellationTokenSource.Token ).Forget();
            
            void ToggleGameControls( bool enable )
            {
                if ( enable ) Inputs.Player.Enable();
                else Inputs.Player.Disable();
            }

            void ToggleUIControls( bool enable )
            {
                if ( enable ) Inputs.UI.Enable();
                else Inputs.UI.Disable();
            }
        }

        public static void Dispose()
        {
            InputSystem.onDeviceChange -= DeviceChangedHandler;
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            
            if ( Inputs != null )
            {
                Inputs.Disable();
                Inputs.Player.Jump.started -= JumStartedHandler;
            }
            OnJump = null;

            for ( int i = _inputHolders.Count - 1; i >= 0; i-- )
            {
                RemoveInputHolder( _inputHolders[ i ] );
            }
            _inputHolders.Clear();
        }

        private static async UniTask Tick( CancellationToken cancellationToken = default )
        {
            bool isController = IsController;
            
            while ( !cancellationToken.IsCancellationRequested )
            {
                if ( IsMouse )
                {
                    MouseX = Mouse.current.delta.x.ReadValue();
                    MouseY = Mouse.current.delta.y.ReadValue();
                }

                if ( IsController )
                {
                    ControllerX = Gamepad.current.rightStick.x.ReadValue();
                    ControllerY = -Gamepad.current.rightStick.y.ReadValue();
                }

                for ( int i = 0; i < _inputHolders.Count; i++ )
                {
                    _inputHolders[ i ].Tick();
                }

                await UniTask.Yield();

                if ( isController != IsController )
                {
                    isController = IsController;
                    OnControllerChanged?.Invoke();
                }
            }
        }

        public static void AddInputHolders( List< InputActionHolder > inputHolders )
        {
            for ( int i = 0; i < inputHolders.Count; i++ )
            {
                AddInputHolder( inputHolders[ i ] );
            }
        }
        
        public static void AddInputHolder( InputActionHolder inputActionHolder )
        {
            inputActionHolder.Enable();
            _inputHolders.Add( inputActionHolder );
        }

        public static void RemoveInputHolder( InputActionHolder inputActionHolder )
        {
            _inputHolders.Remove( inputActionHolder );
            inputActionHolder.Disable();
        }

        public static float GatherRawMouseX( float currentSensX, float currentControllerSensX ) => ( MouseX * currentSensX * Time.fixedDeltaTime + ControllerX * Time.fixedDeltaTime * currentControllerSensX );
        public static float GatherRawMouseY( int sensYInverted, int sensYInvertedController, float currentSensY, float currentControllerSensY ) => ( MouseY * currentSensY * sensYInverted * Time.fixedDeltaTime + ControllerY * sensYInvertedController * currentControllerSensY * Time.fixedDeltaTime );

        private static void JumStartedHandler( InputAction.CallbackContext callbackContext )
        {
            OnJump?.Invoke();
        }

        private static void DeviceChangedHandler( InputDevice device, InputDeviceChange change )
        {
            if ( device is Gamepad )
            {
                switch ( change )
                {
                    case InputDeviceChange.Reconnected:
                    case InputDeviceChange.Added:
                    case InputDeviceChange.Enabled:
                    {
                        OnControllerChanged?.Invoke();
                        break;
                    }
                    case InputDeviceChange.Disconnected:
                    case InputDeviceChange.Removed:
                    case InputDeviceChange.Disabled:
                    {
                        OnControllerChanged?.Invoke();
                        break;
                    }
                    default:
                    {
                        OnControllerChanged?.Invoke();
                        break;
                    }
                }
            }
        }
    }
}