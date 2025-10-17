using Game.Systems.StorageSystem;
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

        private float YawOffset => 0;
        private float PitchOffset => 0;

        private ControlsData _controlsData;
        
        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        private readonly PlayerStates _states;
        
        public PlayerLookController(
            PlayerObject view,
            PlayerConfig config,
            PlayerStates states,
            
            DataHolder dataHolder
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );

            _controlsData = dataHolder.GeneralStorageData.Controls.Value;
        }

        public void Look()
        {
            int sensYInverted = _config.LookSettings.InvertYSensitivity ? -1 : 1;
            int sensXInvertedController = !_controlsData.IsControllerInvertXToggle ? 1 : -1;
            int sensYInvertedController = _controlsData.IsControllerInvertYToggle ? 1 : -1;
            float sensitivityMultiplier = 1;

            float rawMouseX = InputManager.MouseX * _controlsData.MouseXSensitivity + InputManager.ControllerX * _controlsData.ControllerXSensitivity * sensXInvertedController;
            float rawMouseY = InputManager.MouseY * _controlsData.MouseYSensitivity * sensYInverted + InputManager.ControllerY * _controlsData.ControllerYSensitivity * sensYInvertedController;
            float mouseX = rawMouseX * sensitivityMultiplier;
            float mouseY = rawMouseY * sensitivityMultiplier;

            _cameraYaw += mouseX * Time.fixedDeltaTime;
            _cameraPitch -= mouseY * Time.fixedDeltaTime;
            _cameraPitch = Mathf.Clamp( _cameraPitch, -_config.LookSettings.MaxCameraAngle, _config.LookSettings.MaxCameraAngle );

            CalculateCameraRoll();

            const float smoothSpeed = 100f;
            Quaternion targetHeadRot = Quaternion.Euler( _cameraPitch, _cameraYaw, _cameraRoll );
            Quaternion targetRootRot = Quaternion.Euler( 0, _cameraYaw, 0 );

            Head.localRotation = Quaternion.Lerp( Head.localRotation, targetHeadRot, Time.deltaTime * smoothSpeed );
            Root.rotation = Quaternion.Lerp( Root.rotation, targetRootRot, Time.deltaTime * smoothSpeed );
        }

        // public void Look()
        // {
        //     int sensYInverted = _config.LookSettings.InvertYSensitivity ? -1 : 1;
        //     int sensYInvertedController = _config.LookSettings.InvertYControllerSensitivity ? 1 : -1;
        //     float sensitivityMultiplier = 1; //(weaponController.IsAiming) ? aimingSensitivityMultiplier : 1;
        //
        //     // Grab the Inputs from the user.
        //     float rawMouseX = InputManager.MouseX * _controlsData.MouseXSensitivity + InputManager.ControllerX * _controlsData.ControllerXSensitivity;
        //     float rawMouseY =  InputManager.MouseY * _controlsData.MouseYSensitivity * sensYInverted + InputManager.ControllerY * _controlsData.ControllerYSensitivity * sensYInvertedController;
        //     float mouseX = rawMouseX * sensitivityMultiplier;
        //     float mouseY = rawMouseY * sensitivityMultiplier;
        //
        //     _cameraYaw = Head.localRotation.eulerAngles.y + mouseX * Time.fixedDeltaTime + YawOffset * Time.fixedDeltaTime;//YawOffset = weaponRecoil.RecoilYawOffset
        //     _cameraPitch -= mouseY * Time.fixedDeltaTime - PitchOffset * Time.fixedDeltaTime;//PitchOffset = weaponRecoil.RecoilPitchOffset 
        //     
        //     // Make sure we dont over- or under-rotate.
        //     // The reason why the value is 89.7 instead of 90 is to prevent errors with the wallrun
        //     _cameraPitch = Mathf.Clamp( _cameraPitch, -_config.LookSettings.MaxCameraAngle, _config.LookSettings.MaxCameraAngle );
        //
        //     CalculateCameraRoll();
        //     SetRotation( _cameraPitch, _cameraYaw, _cameraRoll );
        //
        //     // HandleAimAssist();
        // }

        public void SetRotation( float x, float y, float z )
        {
            Head.localRotation = Quaternion.Euler( x, y, z );
            Root.rotation = Quaternion.Euler( 0, y, 0 );
        }

        public void SetRotation( Vector3 rotation )
        {
            Head.localRotation = Quaternion.Euler( rotation.x, rotation.y, rotation.z );
            Root.rotation = Quaternion.Euler( 0, rotation.y, 0 );
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