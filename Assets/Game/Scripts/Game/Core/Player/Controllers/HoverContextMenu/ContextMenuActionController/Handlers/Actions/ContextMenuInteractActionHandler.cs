using Game.Core.Entity;
using Game.Core.World.InteractionSystem;
using Game.Managers.InputManager;
using PuzzlescapeGames.Localization;
using StarSmithGames.Localization;
using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class ContextMenuInteractActionHandler : ContextMenuQuickActionHandler
    {
        private InteractableObject _interactable;

        private readonly PlayerController _playerController;
        private readonly ILocalizationSystem _localizationSystem;
        
        public ContextMenuInteractActionHandler(
            PlayerController playerController,
            InputKeyActionsSettings inputKeyActionsSettings,
            ILocalizationSystem localizationSystem
            ) : base( inputKeyActionsSettings.InteractAction )
        {
            _playerController = playerController ?? throw new ArgumentNullException( nameof(playerController) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }
        
        public override void Initialize( ObservableObject target )
        {
            base.Initialize( target );
            
            _interactable = (InteractableObject)target;
            
            ContextMenuOperation.Key = _inputKeyAction.GetDisplayKey();
            ContextMenuOperation.Name = _localizationSystem.Translate( LocalizationIds.UI_CONTROL_INTERACT );
        }

        public override void Dispose()
        {
            _interactable = null;
            
            base.Dispose();
        }
        
        protected override void Completed()
        {
            if ( !IsEnable ) return;

            _interactable.Interact( _playerController );
            
            base.Completed();
        }
    }
}