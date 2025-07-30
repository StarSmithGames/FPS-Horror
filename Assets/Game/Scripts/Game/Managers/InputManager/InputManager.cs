using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public static class InputManager
    {
        public static GameplayInputs Inputs { get; private set; }

        public static float scrolling, MouseX, MouseY, ControllerX, ControllerY;

        private static CancellationTokenSource _cancellationTokenSource;
        
        public static void Initialize()
        {
            Inputs = new GameplayInputs();
            Inputs.Enable();

            ToggleGameControls( true );
            ToggleUIControls( false );

            _cancellationTokenSource = new();
            Tick( _cancellationTokenSource.Token ).Forget();
        }

        public static void Dispose()
        {
            Inputs?.Disable();
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
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

                await UniTask.Yield();
            }
        }

        private static void ToggleGameControls( bool enable )
        {
            if ( enable ) Inputs.Player.Enable();
            else Inputs.Player.Disable();
        }

        private static void ToggleUIControls( bool enable )
        {
            if ( enable ) Inputs.UI.Enable();
            else Inputs.UI.Disable();
        }

        public static float GatherRawMouseX(float currentSensX, float currentControllerSensX)
        {
            return (MouseX * currentSensX * Time.fixedDeltaTime + ControllerX * Time.deltaTime * currentControllerSensX); 
        }
        public static float GatherRawMouseY(int sensYInverted, int sensYInvertedController, float currentSensY, float currentControllerSensY)
        {
            return (MouseY * currentSensY * sensYInverted * Time.fixedDeltaTime + ControllerY * sensYInvertedController * Time.deltaTime * currentControllerSensY);
        }
    }
}