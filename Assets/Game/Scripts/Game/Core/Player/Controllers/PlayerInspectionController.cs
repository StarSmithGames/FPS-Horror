using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.UI.InspectDialog;
using Game.Core.UI.ResourcesScreen;
using Game.Core.World.InventorySystem;
using Game.Managers.CursorManager;
using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerInspectionController
    {
        public event Action OnInspectStarted;
        public event Action OnInspectEnded;
        
        public bool IsInspecting => _inspectDialogViewModel != null;

        private bool _isFromWorld;
        private ItemObject _item;
        private InspectDialogViewModel _inspectDialogViewModel;
        
        private readonly PlayerStates _states;
        private readonly PlayerObject _view;
        private readonly PlayerInventoryController _playerInventoryController;
        private readonly PlayerLibraryController _playerLibraryController;
        private readonly ItemFactory _itemFactory;
        private readonly UIRootGame _uiRootGame;

        public PlayerInspectionController(
            PlayerStates states,
            PlayerObject view,
            PlayerInventoryController playerInventoryController,
            PlayerLibraryController playerLibraryController,
            ItemFactory itemFactory,
            UIRootGame uiRootGame
            )
        {
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _playerInventoryController = playerInventoryController ?? throw new ArgumentNullException( nameof(playerInventoryController) );
            _playerLibraryController = playerLibraryController ?? throw new ArgumentNullException( nameof(playerLibraryController) );
            _itemFactory = itemFactory ?? throw new ArgumentNullException( nameof(itemFactory) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
        }

        public void InspectItemFromContextMenu( InventoryItem inventoryItem )
        {
            _isFromWorld = false;
            
            var screen = _uiRootGame.ScreenAggregator.GetAs< ResourcesScreenViewModel >();
            screen.ModelView.gameObject.SetActive( false );
            
            if ( inventoryItem.View != null )
            {
                inventoryItem.View.EnableView( true );
                InspectItem( inventoryItem.View );
            }
            else
            {
                var controller = _itemFactory.Create( inventoryItem.Config.Prefab );
                inventoryItem.View.EnableView( true );
                inventoryItem.SetView( controller.View );
                InspectItem( controller.View );
            }
        }

        public void InspectItemFromWorld( ItemObject item )
        {
            _isFromWorld = true;
         
            _states.IsBlocked = true;
            CursorManager.Enable();
            
            InspectItem( item );
        }
        
        private void InspectItem( ItemObject item )
        {
            _item = item;
            
            _inspectDialogViewModel = _uiRootGame.DialogAggregator.GetOrCreateIfNotExist< InspectDialogViewModel >();
            _inspectDialogViewModel.Set( _item, _view.CameraFPS, _isFromWorld );
            _inspectDialogViewModel.OnActionButtonClicked += ItemTakenHandler;
            _inspectDialogViewModel.OnCancelButtonClicked += InspectCompletedHandler;
            _inspectDialogViewModel.ShowView();
            
            _playerLibraryController.Add( _item );
            
            OnInspectStarted?.Invoke();
        }

        //Only World items
        private void ItemTakenHandler()
        {
            if ( _inspectDialogViewModel != null )
            {
                _inspectDialogViewModel.OnActionButtonClicked -= ItemTakenHandler;
            }
            _inspectDialogViewModel = null;
            
            _playerInventoryController.PickUpItem( _item );
            
            CursorManager.Disable();
            _states.IsBlocked = false;
            
            OnInspectEnded?.Invoke();
        }

        //UI and World items
        private void InspectCompletedHandler()
        {
            _inspectDialogViewModel.OnCancelButtonClicked -= InspectCompletedHandler;
            _inspectDialogViewModel = null;

            if ( _isFromWorld )
            {
                CursorManager.Disable();
                _states.IsBlocked = false;
            }
            else
            {
                var screen = _uiRootGame.ScreenAggregator.GetAs< ResourcesScreenViewModel >();
                screen.ModelView.gameObject.SetActive( true );
            }
            
            OnInspectEnded?.Invoke();
        }
    }
}