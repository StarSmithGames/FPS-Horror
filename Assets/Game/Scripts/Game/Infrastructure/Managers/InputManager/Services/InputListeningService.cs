using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Game.Managers.InputManager
{
    public static class InputListeningService
    {
        /// <summary>
        /// Ждёт первое нажатие с любого устройства.
        /// </summary>
        public static async UniTask< InputControl > WaitForAnyInput()
        {
            InputSystem.FlushDisconnectedDevices();
            InputControl result = null;

            await UniTask.WaitUntil( () =>
            {
                foreach ( var device in InputSystem.devices )
                {
                    // Игнорируем "none"
                    if ( !device.added || !device.enabled )
                        continue;

                    foreach ( var control in device.allControls )
                    {
                        if ( IsPressed( control ) )
                        {
                            result = control;
                            return true;
                        }
                    }
                }

                return false;
            } );

            return result;
        }

        private static bool IsPressed( InputControl control )
        {
            switch ( control )
            {
                case ButtonControl button:
                    return button.wasPressedThisFrame;

                case AxisControl axis:
                    return Mathf.Abs( axis.ReadValue() ) > 0.5f;

                case Vector2Control stick:
                    return stick.ReadValue().sqrMagnitude > 0.7f;

                default:
                    return false;
            }
        }
    }
}