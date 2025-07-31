using System;
using UnityEngine;
using InputManager = Game.Managers.InputManager.InputManager;

namespace Game.Core.Player
{
    public sealed class PlayerLookController
    {
        private float _cameraPitch;
        private float _cameraYaw;
        private float _cameraRoll;

        private float _currentSensX = 4f;
        private float _currentSensY = 4f;
        private float _currentControllerSensX = 35f;
        private float _currentControllerSensY = 35f;
        
        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        
        public PlayerLookController(
            PlayerObject view,
            PlayerConfig config
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
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

            // Calculate new yaw rotation ( around the y axis )
            // _cameraYaw = playerCam.transform.localRotation.eulerAngles.y + mouseX + weaponRecoil.RecoilYawOffset * Time.deltaTime;
            _cameraYaw = _view.FirstPersonCamera.transform.localRotation.eulerAngles.y + mouseX + Time.deltaTime;
            //Rotate Camera Pitch ( around x axis )
            // _cameraPitch -= mouseY - weaponRecoil.RecoilPitchOffset * Time.deltaTime;
            _cameraPitch -= mouseY - Time.deltaTime;
            // Make sure we dont over- or under-rotate.
            // The reason why the value is 89.7 instead of 90 is to prevent errors with the wallrun
            _cameraPitch = Mathf.Clamp( _cameraPitch, -_config.LookSettings.MaxCameraAngle, _config.LookSettings.MaxCameraAngle );

            CalculateCameraRoll();

            // Handle camera rotation
            _view.FirstPersonCamera.transform.localRotation = Quaternion.Euler( _cameraPitch, _cameraYaw, _cameraRoll );
            _view.transform.rotation = Quaternion.Euler( 0, _cameraYaw, 0 );

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
            // if (wallRunning && canWallRun) cameraRoll = wallLeft ? Mathf.Lerp(cameraRoll, -wallrunCameraTiltAmount, Time.deltaTime * cameraTiltTransitionSpeed) : Mathf.Lerp(cameraRoll, wallrunCameraTiltAmount, Time.deltaTime * cameraTiltTransitionSpeed);
            // else if (isCrouching && rb.velocity.magnitude >= walkSpeed && allowSliding && !hasJumped) cameraRoll = Mathf.Lerp(cameraRoll, slidingCameraTiltAmount, Time.deltaTime * cameraTiltTransitionSpeed);
            // else cameraRoll = Mathf.Lerp(cameraRoll, 0, Time.deltaTime * cameraTiltTransitionSpeed);
        }
    }
}