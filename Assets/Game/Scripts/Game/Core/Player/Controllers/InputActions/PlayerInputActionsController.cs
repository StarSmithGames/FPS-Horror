using Game.Core.UI;
using Game.Core.UI.PauseScreen;
using Game.Managers.InputManager;
using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerInputActionsController
    {
        private InputActionHolder _inputActionMenu;
        private InputActionHolder _inputActionSprint;
        private InputActionHolder _inputActionCrouch;
        private InputActionHolder _inputActionJump;
        private InputActionHolder _inputActionLighter;

        private readonly PlayerStates _states;
        private readonly PlayerCrouchController _crouchController;
        private readonly PlayerJumpController _jumpController;
        private readonly PlayerInventoryController _inventoryController;
        private readonly UIRootGame _uiRootGame;
        
        public PlayerInputActionsController(
            PlayerStates states,
            PlayerCrouchController crouchController,
            PlayerJumpController jumpController,
            PlayerInventoryController inventoryController,
            UIRootGame uiRootGame
            )
        {
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _crouchController = crouchController ?? throw new ArgumentNullException( nameof(crouchController) );
            _jumpController = jumpController ?? throw new ArgumentNullException( nameof(jumpController) );
            _inventoryController = inventoryController ?? throw new ArgumentNullException( nameof(inventoryController) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
        }

        public void Initialize()
        {
            _inputActionMenu = new( InputManager.Inputs.System.Menu, MenuClickedHandler );
            _inputActionSprint = new( InputManager.Inputs.Player.Sprint, SpringStartHandler, SpringStopHandler );
            _inputActionCrouch = new( InputManager.Inputs.Player.Crouch, CrouchStartHandler, CrouchStopHandler );
            _inputActionJump = new( InputManager.Inputs.Player.Jump, JumpClickedHandler );
            _inputActionLighter = new( InputManager.Inputs.System.Lighter, InputLighterCompletedHandler );
        }
        
        public void Enable()
        {
            _inputActionMenu.Enable();
            EnablePlayer();
        }

        public void Disable()
        {
            _inputActionMenu.Disable();
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

        private void MenuClickedHandler()
        {
            var screen = _uiRootGame.ScreenAggregator.GetOrCreateIfNotExist< PauseScreenViewModel >();
            if ( screen.IsShowing )
            {
                return;
            }
            screen.ShowView();
        }

        #region Player
        private void SpringStartHandler()
        {
            _states.IsSprinting = true;
        }
        
        private void SpringStopHandler()
        {
            _states.IsSprinting = false;
        }

        private void CrouchStartHandler()
        {
            if ( _states.IsBlocked ) return;
            
            _crouchController.StartCrouch();
        }
        
        private void CrouchStopHandler()
        {
            _crouchController.StopCrouch();
        }

        private void JumpClickedHandler()
        {
            if ( _states.IsBlocked ) return;
            
            _jumpController.Jump( InputManager.Inputs.Player.Movement.ReadValue< Vector2 >() );
        }
        #endregion
        
        private void InputLighterCompletedHandler()
        {
            if ( _states.IsBlocked ) return;
            
            _inventoryController.SelectLighter();
        }
    }
}