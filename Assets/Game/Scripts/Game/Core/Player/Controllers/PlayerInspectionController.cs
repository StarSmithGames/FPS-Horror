using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.UI.InspectDialog;
using System;

namespace Game.Core.Player
{
    public sealed class PlayerInspectionController
    {
        public event Action OnInspectStarted;
        public event Action OnInspectEnded;
        
        public bool IsInspecting => _inspectDialogViewModel != null;

        private ItemObject _item;
        private InspectDialogViewModel _inspectDialogViewModel;
        
        private readonly PlayerObject _view;
        private readonly PlayerInventoryController _playerInventoryController;
        private readonly PlayerLibraryController _playerLibraryController;
        private readonly UIRootGame _uiRootGame;

        public PlayerInspectionController(
            PlayerObject view,
            PlayerInventoryController playerInventoryController,
            PlayerLibraryController playerLibraryController,
            UIRootGame uiRootGame
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _playerInventoryController = playerInventoryController ?? throw new ArgumentNullException( nameof(playerInventoryController) );
            _playerLibraryController = playerLibraryController ?? throw new ArgumentNullException( nameof(playerLibraryController) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
        }

        public void InspectItemFromWorld( ItemObject item )
        {
            InspectItem( item, true );
        }
        
        public void InspectItemFromUI( ItemObject item )
        {
            InspectItem( item, false );
        }
        
        private void InspectItem( ItemObject item, bool isFromWorld )
        {
            _item = item;
            
            _inspectDialogViewModel = _uiRootGame.DialogAggregator.GetOrCreateIfNotExist< InspectDialogViewModel >();
            _inspectDialogViewModel.Set( _item, _view.CameraFPS, isFromWorld );
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
            
            OnInspectEnded?.Invoke();
        }

        //UI and World items
        private void InspectCompletedHandler()
        {
            _inspectDialogViewModel.OnCancelButtonClicked -= InspectCompletedHandler;
            _inspectDialogViewModel = null;

            OnInspectEnded?.Invoke();
        }
    }
}