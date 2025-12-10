using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.UI.InspectDialog;
using Game.Core.UI.PauseScreen;
using Game.Core.UI.ResourcesScreen;
using Game.Managers.InputManager;
using Game.Systems.StorageSystem;
using PuzzlescapeGames.VVM;
using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerInputActionsController
    {
        private InputActionVoidWrap _inputActionMenu;
        private InputActionVoidWrap _inputActionSprint;
        private InputActionVoidWrap _inputActionCrouch;
        private InputActionVoidWrap _inputActionJump;
        private InputActionVoidWrap _inputActionLighter;
        private InputActionVoidWrap _inputActionInventory;
        
        private InspectDialogViewModel _inspectDialogViewModel;

        private readonly PlayerObject _view;
        private readonly PlayerStates _states;
        private readonly PlayerCrouchController _crouchController;
        private readonly PlayerJumpController _jumpController;
        private readonly PlayerInventoryController _inventoryController;
        private readonly UIRootGame _uiRootGame;
        private readonly DataHolder _dataHolder;
        
        public PlayerInputActionsController(
            PlayerObject view,
            PlayerStates states,
            PlayerCrouchController crouchController,
            PlayerJumpController jumpController,
            PlayerInventoryController inventoryController,
            UIRootGame uiRootGame,
            DataHolder dataHolder
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _crouchController = crouchController ?? throw new ArgumentNullException( nameof(crouchController) );
            _jumpController = jumpController ?? throw new ArgumentNullException( nameof(jumpController) );
            _inventoryController = inventoryController ?? throw new ArgumentNullException( nameof(inventoryController) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _dataHolder = dataHolder ?? throw new ArgumentNullException( nameof(dataHolder) );
        }

        public void Initialize()
        {
            _inputActionMenu = new( InputManager.Inputs.System.Menu, MenuClickedHandler );
            _inputActionSprint = new( InputManager.Inputs.Player.Sprint, SpringStartHandler, SpringStopHandler );
            _inputActionCrouch = new( InputManager.Inputs.Player.Crouch, CrouchStartHandler, CrouchStopHandler );
            _inputActionJump = new( InputManager.Inputs.Player.Jump, JumpClickedHandler );
            _inputActionLighter = new( InputManager.Inputs.System.Lighter, LighterClickedHandler );
            _inputActionInventory = new( InputManager.Inputs.System.Inventory, InventoryClickedHandler );

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
            _inputActionInventory.Enable();
        }

        public void DisablePlayer()
        {
            _inputActionSprint.Disable();
            _inputActionCrouch.Disable();
            _inputActionJump.Disable();
            _inputActionLighter.Disable();
            _inputActionInventory.Disable();
        }

        #region Inspection
        public void InspectItem( ItemObject item )
        {
            _states.IsBlocked = true;

            _inspectDialogViewModel = _uiRootGame.DialogAggregator.GetOrCreateIfNotExist< InspectDialogViewModel >();
            _inspectDialogViewModel.Set( item, _view.CameraFPS );
            _inspectDialogViewModel.OnCancelButtonClicked += InspectCompletedHandler;
            _inspectDialogViewModel.ShowView();
        }

        private void InspectCompletedHandler()
        {
            _inspectDialogViewModel.OnCancelButtonClicked -= InspectCompletedHandler;
            _inspectDialogViewModel = null;
            
            _states.IsBlocked = false;
        }
        #endregion

        private void MenuClickedHandler()
        {
            if ( _inspectDialogViewModel != null ) return;
            
            _inputActionMenu.Disable();
            
            var screen = _uiRootGame.ScreenAggregator.GetOrCreateIfNotExist< PauseScreenViewModel >();
            screen.OnShowingChanged += ScreenShowingChangedHandler;
            screen.ShowView();
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
            
            _inventoryController.SelectLighter();
        }
        
        private void InventoryClickedHandler()
        {
            if ( _inspectDialogViewModel != null ) return;
            
            _inputActionMenu.Disable();
            
            var screen = _uiRootGame.ScreenAggregator.GetOrCreateIfNotExist< ResourcesScreenViewModel >();
            screen.OnShowingChanged += ScreenShowingChangedHandler;
            screen.ShowView();
        }

        private void ScreenShowingChangedHandler( IViewModel viewModel )
        {
            if ( viewModel.IsShowing ) return;
            viewModel.OnShowingChanged -= ScreenShowingChangedHandler;
            
            _inputActionMenu.Enable();
        }
    }
}