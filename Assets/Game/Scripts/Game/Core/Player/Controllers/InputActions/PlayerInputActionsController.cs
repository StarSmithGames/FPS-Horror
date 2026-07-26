using Game.Managers.InputManager;
using Game.Systems.StorageSystem;
using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerInputActionsController
    {
        private InputActionVoidWrap _inputActionSprint;
        private InputActionVoidWrap _inputActionCrouch;
        private InputActionVoidWrap _inputActionJump;
        private InputActionVoidWrap _inputActionLighter;
        
        private readonly PlayerStates _states;
        private readonly PlayerCrouchController _crouchController;
        private readonly PlayerJumpController _jumpController;
        private readonly PlayerEquipmentController _equipmentController;
        private readonly DataHolder _dataHolder;
        
        public PlayerInputActionsController(
            PlayerStates states,
            PlayerCrouchController crouchController,
            PlayerJumpController jumpController,
            PlayerEquipmentController equipmentController,
            DataHolder dataHolder
            )
        {
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _crouchController = crouchController ?? throw new ArgumentNullException( nameof(crouchController) );
            _jumpController = jumpController ?? throw new ArgumentNullException( nameof(jumpController) );
            _equipmentController = equipmentController ?? throw new ArgumentNullException( nameof(equipmentController) );

            _dataHolder = dataHolder ?? throw new ArgumentNullException( nameof(dataHolder) );
        }

        public void Initialize()
        {
            _inputActionSprint = new( InputManager.Inputs.Player.Sprint, SpringStartHandler, SpringStopHandler );
            _inputActionCrouch = new( InputManager.Inputs.Player.Crouch, CrouchStartHandler, CrouchStopHandler );
            _inputActionJump = new( InputManager.Inputs.Player.Jump, JumpClickedHandler );
            _inputActionLighter = new( InputManager.Inputs.System.Lighter, LighterClickedHandler );
        }
        
        public void Enable()
        {
            EnablePlayer();
        }

        public void Disable()
        {
            DisablePlayer();
        }

        public void EnablePlayer()
        {
            _inputActionSprint.Enable();
            _inputActionCrouch.Enable();
            _inputActionJump.Enable();
            _inputActionLighter.Enable();
        }

        public void DisablePlayer()
        {
            _inputActionSprint.Disable();
            _inputActionCrouch.Disable();
            _inputActionJump.Disable();
            _inputActionLighter.Disable();
        }

        #region Player
        private void SpringStartHandler()
        {
            if ( _dataHolder.GeneralStorageData.Gameplay.Value.IsSprintToggle )
            {
                _states.IsSprinting = !_states.IsSprinting;
            }
            else
            {
                _states.IsSprinting = true;
            }
        }
        
        private void SpringStopHandler()
        {
            if ( _dataHolder.GeneralStorageData.Gameplay.Value.IsSprintToggle )
            {
                
            }
            else
            {
                _states.IsSprinting = false;
            }
        }

        private void CrouchStartHandler()
        {
            if ( _dataHolder.GeneralStorageData.Gameplay.Value.IsCrouchToggle )
            {
                if ( _states.IsCrouching )
                {
                    _crouchController.StopCrouch();
                }
                else
                {
                    _crouchController.StartCrouch();
                }
            }
            else
            {
                if ( _states.IsBlocked ) return;

                _crouchController.StartCrouch();
            }
        }
        
        private void CrouchStopHandler()
        {
            if ( _dataHolder.GeneralStorageData.Gameplay.Value.IsCrouchToggle )
            {
                
            }
            else
            {
                _crouchController.StopCrouch();
            }
        }

        private void JumpClickedHandler()
        {
            if ( _states.IsBlocked ) return;
            
            _jumpController.Jump( InputManager.Inputs.Player.Movement.ReadValue< Vector2 >() );
        }
        #endregion
        
        private void LighterClickedHandler()
        {
            if ( _states.IsBlocked ) return;
            
            _equipmentController.SelectLighter();
        }
    }
}