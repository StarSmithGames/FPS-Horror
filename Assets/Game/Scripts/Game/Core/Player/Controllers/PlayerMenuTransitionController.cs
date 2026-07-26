using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.UI.PauseScreen;
using Game.Core.UI.ResourcesScreen;
using Game.Core.World.InventorySystem;
using Game.Managers.CursorManager;
using Game.Managers.GameManager;
using Game.Managers.InputManager;
using Game.Managers.PauseManager;
using PuzzlescapeGames.VVM;
using System;

namespace Game.Core.Player
{
    public sealed class PlayerMenuTransitionController
    {
        private InputActionVoidWrap _inputActionMenu;
        private InputActionVoidWrap _inputActionInventory;
        
        private readonly PlayerStates _states;
        private readonly PlayerInspectionController _inspectionController;
        private readonly PauseManager _pauseManager;
        private readonly GameManager _gameManager;
        private readonly UIRootGame _uiRootGame;
        private readonly ItemFactory _itemFactory;

        public PlayerMenuTransitionController(
            PlayerStates states,
            PlayerInspectionController inspectionController,
            PauseManager pauseManager,
            GameManager gameManager,
            UIRootGame uiRootGame,
            ItemFactory itemFactory
            )
        {
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _inspectionController = inspectionController ?? throw new ArgumentNullException( nameof(inspectionController) );
            _pauseManager = pauseManager ?? throw new ArgumentNullException( nameof(pauseManager) );
            _gameManager = gameManager ?? throw new ArgumentNullException( nameof(gameManager) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _itemFactory = itemFactory ?? throw new ArgumentNullException( nameof(itemFactory) );
        }

        public void Initialize()
        {
            _inputActionMenu = new( InputManager.Inputs.System.Menu, MenuClickedHandler );
            _inputActionInventory = new( InputManager.Inputs.System.Inventory, InventoryClickedHandler );
        }
        
        public void Enable()
        {
            _inputActionMenu.Enable();
            _inputActionInventory.Enable();
        }

        public void Disable()
        {
            _inputActionMenu.Disable();
            _inputActionInventory.Disable();
        }

        private void SetStateToMenu()
        {
            _pauseManager.Pause();
            _gameManager.SetState( GameState.Menu );
            CursorManager.Enable();
            _states.IsBlocked = true;
        }

        private void SetStateToGame()
        {
            _pauseManager.UnPause();
            _gameManager.SetState( GameState.Game );
            CursorManager.Disable();
            _states.IsBlocked = false;
        }

        #region Inspection
        private ItemObject _cachedInspectItem;
        
        //From UI
        public void TransitToInspection( InventoryItem item )
        {
            var screen = _uiRootGame.ScreenAggregator.GetAs< ResourcesScreenViewModel >();
            screen.Hide( true );
            
            if ( item.View == null )
            {
                var controller = _itemFactory.Create( item.Config.Prefab );
                item.SetView( controller.View );
            }
            _cachedInspectItem = item.View;
            _cachedInspectItem.EnableView( true );
            
            _inspectionController.OnInspectEnded += InspectFromUIEndedHandler;
            _inspectionController.InspectItemFromUI( item.View );
        }
        
        //From World
        public void TransitToInspection( ItemObject item )
        {
            SetStateToMenu();
            _inputActionMenu.Disable();
            _inputActionInventory.Disable();
            
            _inspectionController.OnInspectEnded += InspectFromWorldEndedHandler;
            _inspectionController.InspectItemFromWorld( item );
        }

        private void InspectFromUIEndedHandler()
        {
            _inspectionController.OnInspectEnded -= InspectFromUIEndedHandler;
            
            var screen = _uiRootGame.ScreenAggregator.GetAs< ResourcesScreenViewModel >();
            screen.Hide( false );
            
            _cachedInspectItem.EnableView( false );
        }
        
        private void InspectFromWorldEndedHandler()
        {
            _inspectionController.OnInspectEnded -= InspectFromWorldEndedHandler;

            SetStateToGame();
            _inputActionMenu.Enable();
            _inputActionInventory.Enable();
        }
        #endregion
        
        private void MenuClickedHandler()
        {
            if ( _inspectionController.IsInspecting ) return;

            SetStateToMenu();
            _inputActionMenu.Disable();
            _inputActionInventory.Disable();
            
            var screen = _uiRootGame.ScreenAggregator.GetOrCreateIfNotExist< PauseScreenViewModel >();
            screen.OnShowingChanged += ScreenShowingChangedHandler;
            screen.ShowView();
        }
        
        private void InventoryClickedHandler()
        {
            if ( _inspectionController.IsInspecting ) return;
            
            SetStateToMenu();
            _inputActionMenu.Disable();
            _inputActionInventory.Disable();
            
            var screen = _uiRootGame.ScreenAggregator.GetOrCreateIfNotExist< ResourcesScreenViewModel >();
            screen.OnShowingChanged += ScreenShowingChangedHandler;
            screen.ShowView();
        }
        
        private void ScreenShowingChangedHandler( IViewModel viewModel )
        {
            if ( viewModel.IsShowing ) return;
            viewModel.OnShowingChanged -= ScreenShowingChangedHandler;
            
            SetStateToGame();
            _inputActionMenu.Enable();
            _inputActionInventory.Enable();
        }
    }
}