using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.World.InteractionSystem;
using Game.Managers.InputManager;
using PuzzlescapeGames.Localization;
using StarSmithGames.Localization;
using System;

namespace Game.Core.Player
{
    public sealed class PickUpActionHandler : QuickActionHandler
    {
        private ItemObject _item;

        private readonly PlayerInventoryController _playerInventoryController;
        
        public PickUpActionHandler(
            PlayerInventoryController playerInventoryController,
            InputKeyActionsSettings inputKeyActionsSettings,
            ILocalizationSystem localizationSystem
            ) : base( inputKeyActionsSettings.InteractAction )
        {
            _playerInventoryController = playerInventoryController ?? throw new ArgumentNullException( nameof(playerInventoryController) );
            
            if ( ContextMenuOperation == null )
            {
                ContextMenuOperation = new();
            }
            ContextMenuOperation.Key = inputKeyActionsSettings.InteractAction.GetDisplayKey();
            ContextMenuOperation.Name = localizationSystem.Translate( LocalizationIds.UI_CONTROL_TAKE );
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

            _playerInventoryController.PickUpItem( _item );
            
            base.Completed();
        }
    }
}