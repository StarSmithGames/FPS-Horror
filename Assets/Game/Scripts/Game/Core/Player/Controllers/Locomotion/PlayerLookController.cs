using System;
using UnityEngine;
using InputManager = Game.Managers.InputManager.InputManager;

namespace Game.Core.Player
{
    public sealed class PlayerLookController
    {
        private Transform Root => _view.transform;
        private Transform Head => _view.CameraFPS.transform;
        
        private float _cameraPitch;
        private float _cameraYaw;
        private float _cameraRoll;

        private float _currentSensX = 4f;
        private float _currentSensY = 4f;
        private float _currentControllerSensX = 35f;
        private float _currentControllerSensY = 35f;

        private float YawOffset => 0;
        private float PitchOffset => 0;
        
        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        private readonly PlayerStates _states;
        
        public PlayerLookController(
            PlayerObject view,
            PlayerConfig config,
            PlayerStates states
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );
        }

        public void Look()
        {
            int sensYInverted = _config.LookSettings.InvertYSensitivity ? -1 : 1;
            int sensYInvertedController = _config.LookSettings.InvertYControllerSensitivity ? 1 : -1;
            float sensitivityMultiplier = 1; //(weaponController.IsAiming) ? aimingSensitivityMultiplier : 1;

            // Grab the Inputs from the user.
            float rawMouseX = InputManager.GatherRawMouseX( _currentSensX, _currentControllerSensX );
            float rawMouseY = InputManager.GatherRawMouseY( sensYInverted, sensYInvertedController, _currentSensY, _currentControllerSensY );
            float mouseX = rawMouseX * sensitivityMultiplier;
            float mouseY = rawMouseY * sensitivityMultiplier;

            _cameraYaw = Head.localRotation.eulerAngles.y + mouseX + YawOffset * Time.deltaTime;//YawOffset = weaponRecoil.RecoilYawOffset
            _cameraPitch -= mouseY - PitchOffset * Time.deltaTime;//PitchOffset = weaponRecoil.RecoilPitchOffset 
            
            // Make sure we dont over- or under-rotate.
            // The reason why the value is 89.7 instead of 90 is to prevent errors with the wallrun
            _cameraPitch = Mathf.Clamp( _cameraPitch, -_config.LookSettings.MaxCameraAngle, _config.LookSettings.MaxCameraAngle );

            CalculateCameraRoll();
            
            // Handle camera rotation
            Head.localRotation = Quaternion.Euler( _cameraPitch, _cameraYaw, _cameraRoll );
            Root.rotation = Quaternion.Euler( 0, _cameraYaw, 0 );

            // HandleAimAssist();
        }

        // public void VerticalLook()
        // {
        //     if (!allowVerticalLookWhileClimbing || PauseMenu.isPaused) return;
        //
        //     int sensYInverted = invertYSensitivty ? -1 : 1;
        //     int sensYInvertedController = invertYControllerSensitivty ? -1 : 1;
        //     float sensitivityMultiplier = (weaponController.IsAiming) ? aimingSensitivityMultiplier : 1;
        //
        //     // Grab the Inputs from the user. Since we will only move the camera vertically, we only need Mouse Y
        //     float rawMouseY = InputManager.GatherRawMouseY(sensYInverted, sensYInvertedController, currentSensY, currentControllerSensY);
        //     float mouseY = rawMouseY * sensitivityMultiplier;
        //
        //     // Make sure we dont over- or under-rotate.
        //     // The reason why the value is 89.7 instead of 90 is to prevent errors with the wallrun
        //     cameraPitch -= mouseY;
        //     cameraPitch = Mathf.Clamp(cameraPitch, -maxCameraAngle, maxCameraAngle);
        //
        //     // Handle camera rotation
        //     playerCam.transform.localRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0);
        // }

        private void CalculateCameraRoll()
        {
            // if ( _states.IsCrouching &&
            //      _view.Rigidbody.velocity.magnitude >= _config.MovementSettings.WalkSpeed &&
            //      _config.SlidingSettings.AllowSliding && !_states.IsJumping )
            // {
            //     _cameraRoll = Mathf.Lerp( _cameraRoll, _config.LookSettings.SlidingCameraTiltAmount, Time.deltaTime * _config.LookSettings.CameraTiltTransitionSpeed );
            // }
            // else
            {
                _cameraRoll = Mathf.Lerp( _cameraRoll, 0, Time.deltaTime * _config.LookSettings.CameraTiltTransitionSpeed );
            }
        }
    }
}