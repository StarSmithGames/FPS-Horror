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
        
        private readonly PlayerStates _states;
        private readonly PlayerObject _view;
        private readonly PlayerInventoryController _playerInventoryController;
        private readonly UIRootGame _uiRootGame;

        public PlayerInspectionController(
            PlayerStates states,
            PlayerObject view,
            PlayerInventoryController playerInventoryController,
            UIRootGame uiRootGame
            )
        {
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _playerInventoryController = playerInventoryController ?? throw new ArgumentNullException( nameof(playerInventoryController) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
        }

        public void InspectItem( ItemObject item )
        {
            _states.IsBlocked = true;

            _item = item;
            
            _inspectDialogViewModel = _uiRootGame.DialogAggregator.GetOrCreateIfNotExist< InspectDialogViewModel >();
            _inspectDialogViewModel.Set( _item, _view.CameraFPS );
            _inspectDialogViewModel.OnActionButtonClicked += ItemTakenHandler;
            _inspectDialogViewModel.OnCancelButtonClicked += InspectCompletedHandler;
            _inspectDialogViewModel.ShowView();
            
            OnInspectStarted?.Invoke();
        }

        private void ItemTakenHandler()
        {
            _inspectDialogViewModel.OnActionButtonClicked -= ItemTakenHandler;
            _inspectDialogViewModel = null;
            
            _playerInventoryController.PickUpItem( _item );
            
            _states.IsBlocked = false;
            
            OnInspectEnded?.Invoke();
        }

        private void InspectCompletedHandler()
        {
            _inspectDialogViewModel.OnCancelButtonClicked -= InspectCompletedHandler;
            _inspectDialogViewModel = null;
            
            _states.IsBlocked = false;
            
            OnInspectEnded?.Invoke();
        }
    }
}