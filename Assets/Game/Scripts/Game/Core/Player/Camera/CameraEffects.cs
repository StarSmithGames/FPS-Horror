using Game.Managers.InputManager;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class CameraEffects : MonoBehaviour
    {
        [ Header( "TILT" ) ]
        [ SerializeField ] private float _tiltSpeed;
        [ SerializeField ] private float _tiltAmount;
        
        private void Update()
        {
            // UpdateTilt();
        }

        private void UpdateTilt()
        {
            // if (player.CurrentSpeed == 0) return;

            Quaternion rot = CalculateTilt();
            transform.localRotation = Quaternion.Lerp(transform.localRotation, rot, Time.deltaTime * _tiltSpeed);
        }

        private Quaternion CalculateTilt()
        {
            var moveInput = InputManager.Inputs.Player.Movement.ReadValue< Vector2 >();

            Vector3 vector = new Vector3( moveInput.y, 0, -moveInput.x ).normalized * _tiltAmount;

            return Quaternion.Euler( vector );
        }
    }
}