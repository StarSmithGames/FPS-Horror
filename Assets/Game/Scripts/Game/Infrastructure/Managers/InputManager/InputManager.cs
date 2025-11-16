using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public static class InputManager
    {
        public static GameplayInputs Inputs { get; private set; }

        public static float scrolling, MouseX, MouseY, ControllerX, ControllerY;

        private static InputDevice _lastGamepad;
        
        private static CancellationTokenSource _cancellationTokenSource;
        
        public static void Initialize()
        {
            _cancellationTokenSource = new();
            GamepadDetector.Initialize( _cancellationTokenSource.Token );
            
            Inputs = new GameplayInputs();
            Inputs.Enable();
            
            ToggleGameControls( true );
            ToggleUIControls( false );

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
            }
        }

        private static async UniTask Tick( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                if ( Mouse.current != null )
                {
                    MouseX = Mouse.current.delta.x.ReadValue();
                    MouseY = Mouse.current.delta.y.ReadValue();
                }

                if ( Gamepad.current != null )
                {
                    ControllerX = Gamepad.current.rightStick.x.ReadValue();
                    ControllerY = -Gamepad.current.rightStick.y.ReadValue();
                }

                await UniTask.Yield();
            }
        }
    }
}