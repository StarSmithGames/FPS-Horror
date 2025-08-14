using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public static class InputManager
    {
        public static event Action OnJump;
        
        public static GameplayInputs Inputs { get; private set; }

        public static float scrolling, MouseX, MouseY, ControllerX, ControllerY;

        private static CancellationTokenSource _cancellationTokenSource;
        private static List< InputActionHolder > _inputHolders = new();
        
        public static void Initialize()
        {
            Inputs = new GameplayInputs();
            Inputs.Enable();

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
            while ( !cancellationToken.IsCancellationRequested )
            {
                if (Mouse.current != null)
                {
                    MouseX = Mouse.current.delta.x.ReadValue();
                    MouseY = Mouse.current.delta.y.ReadValue();

                }

                if (Gamepad.current != null)
                {
                    ControllerX = Gamepad.current.rightStick.x.ReadValue();
                    ControllerY = -Gamepad.current.rightStick.y.ReadValue();
                }

                for ( int i = 0; i < _inputHolders.Count; i++ )
                {
                    _inputHolders[ i ].Tick();
                }

                await UniTask.Yield();
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

        public static float GatherRawMouseX( float currentSensX, float currentControllerSensX ) => ( MouseX * currentSensX * Time.fixedDeltaTime + ControllerX * Time.deltaTime * currentControllerSensX );
        public static float GatherRawMouseY( int sensYInverted, int sensYInvertedController, float currentSensY, float currentControllerSensY ) => ( MouseY * currentSensY * sensYInverted * Time.fixedDeltaTime + ControllerY * sensYInvertedController * Time.deltaTime * currentControllerSensY );

        private static void JumStartedHandler( InputAction.CallbackContext callbackContext )
        {
            OnJump?.Invoke();
        }
    }
}