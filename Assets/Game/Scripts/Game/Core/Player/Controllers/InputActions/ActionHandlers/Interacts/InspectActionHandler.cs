using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.UI.InspectDialog;
using Game.Core.World.InteractionSystem;
using Game.Managers.InputManager;
using PuzzlescapeGames.Localization;
using StarSmithGames.Localization;
using System;
using System.Collections.Generic;

namespace Game.Core.Player
{
    public sealed class InspectActionHandler : QuickActionHandler
    {
        private List< ContextMenuOperation > _contextMenuOperations;

        private ItemObject _item;
        private InspectDialogViewModel _inspectDialogViewModel;

        private readonly PlayerObject _view;
        private readonly PlayerStates _states;
        private readonly UIRootGame _uiRootGame;
        
        public InspectActionHandler(
            PlayerObject view,
            PlayerStates states,
            UIRootGame uiRootGame,
            InputKeyActionsSettings inputKeyActionsSettings,
            ILocalizationSystem localizationSystem
            ) : base( inputKeyActionsSettings.InspectAction )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            
            if ( ContextMenuOperation == null )
            {
                ContextMenuOperation = new();
            }
            ContextMenuOperation.Key = inputKeyActionsSettings.InspectAction.GetDisplayKey();
            ContextMenuOperation.Name = localizationSystem.Translate( LocalizationIds.UI_CONTROL_INSPECT );
        }

        public override void Initialize( IObservable target )
        {
            _item = (ItemObject)target;
        }
        
        public override void Disable()
        {
            _item = null;

            base.Disable();
        }

        protected override void Completed()
        {
            if ( !IsEnable ) return;
            
            base.Completed();
            
            _states.IsBlocked = true;

            _inspectDialogViewModel = _uiRootGame.DialogAggregator.GetOrCreateIfNotExist< InspectDialogViewModel >();
            _inspectDialogViewModel.Set( _item, _view.CameraFPS );
            _inspectDialogViewModel.OnCancelButtonClicked += InspectCompletedHandler;
            _inspectDialogViewModel.ShowView();
        }

        private void InspectCompletedHandler()
        {
            _inspectDialogViewModel.OnCancelButtonClicked -= InspectCompletedHandler;
            _inspectDialogViewModel = null;
            
            _states.IsBlocked = false;
        }
    }
}