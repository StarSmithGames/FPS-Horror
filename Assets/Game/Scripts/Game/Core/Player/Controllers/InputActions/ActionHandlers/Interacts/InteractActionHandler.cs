using Game.Core.Entity;
using Game.Core.World.InteractionSystem;
using Game.Managers.InputManager;
using PuzzlescapeGames.Localization;
using StarSmithGames.Localization;
using System;

namespace Game.Core.Player
{
    public sealed class InteractActionHandler : QuickActionHandler
    {
        private IInteractable _interactable;
        
        public InteractActionHandler(
            InputKeyActionsSettings inputKeyActionsSettings,
            ILocalizationSystem localizationSystem
            ) : base( inputKeyActionsSettings.InteractAction )
        {
            if ( ContextMenuOperation == null )
            {
                ContextMenuOperation = new();
            }
            ContextMenuOperation.Key = inputKeyActionsSettings.InteractAction.GetDisplayKey();
            ContextMenuOperation.Name = localizationSystem.Translate( LocalizationIds.UI_CONTROL_INTERACT );
        }
        
        public override void Initialize( IObservable target )
        {
            _interactable = (IInteractable)target;
        }

        public override void Disable()
        {
            _interactable = null;
            base.Disable();
        }
        
        protected override void Completed()
        {
            if ( !IsEnable ) return;

            _interactable.Interact();
            
            base.Completed();
        }
    }
}