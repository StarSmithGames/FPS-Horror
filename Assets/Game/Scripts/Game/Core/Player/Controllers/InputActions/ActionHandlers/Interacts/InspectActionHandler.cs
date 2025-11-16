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
        private ItemObject _item;

        private readonly PlayerController _playerController;
        
        public InspectActionHandler(
            PlayerController playerController,
            InputKeyActionsSettings inputKeyActionsSettings,
            ILocalizationSystem localizationSystem
            ) : base( inputKeyActionsSettings.InspectAction )
        {
            _playerController = playerController ?? throw new ArgumentNullException( nameof(playerController) );
            
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
            
            _playerController.ServiceLocator.GetAs< PlayerInputActionsController >().InspectItem( _item );
            
            base.Completed();
        }
    }
}