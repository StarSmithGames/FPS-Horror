using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.UI.InspectDialog;
using Game.Core.World.Systems.InteractionSystem;
using Game.Managers.InputManager;
using PuzzlescapeGames.Localization;
using System;
using System.Collections.Generic;

namespace Game.Core.Player
{
    public sealed class InspectableHandler : InteractableHandler
    {
        private List< ContextMenuOperation > _contextMenuOperations;

        private ItemObject _item;
        private InputHolder _inputInspect;
        private InspectDialogViewModel _inspectDialogViewModel;

        private readonly PlayerObject _view;
        private readonly PlayerStates _states;
        private readonly UIRootGame _uiRootGame;
        private readonly InteractionsSettings _interactionsSettings;
        private readonly ILocalizationSystem _localizationSystem;
        
        public InspectableHandler(
            PlayerObject view,
            PlayerStates states,
            UIRootGame uiRootGame,
            InteractionsSettings interactionsSettings,
            ILocalizationSystem localizationSystem
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _interactionsSettings = interactionsSettings ?? throw new ArgumentNullException( nameof(interactionsSettings) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }

        public void Initialize()
        {
            _inputInspect = new( _interactionsSettings.InspectAction.InputAction, InspectionCompletedHandler );

            _contextMenuOperations = new()
            {
                new()
                {
                    Key = _interactionsSettings.InspectAction.GetDisplayKey(),
                    Name = _localizationSystem.Translate( _interactionsSettings.InspectAction.NameId )
                }
            };
        }

        public void Enable( ItemObject item )
        {
            _item = item ?? throw new ArgumentNullException( nameof(item) );
            _inputInspect.Enable();
        }

        public void Disable()
        {
            _inputInspect.Disable();
            _item = null;
        }
        
        private void InspectionCompletedHandler()
        {
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

            Completed();
        }

        public override List< ContextMenuOperation > GetContextMenuOptions() => _contextMenuOperations;
    }
}