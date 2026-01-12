using Game.Core.Entity;
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
        private readonly ILocalizationSystem _localizationSystem;
        
        public PickUpActionHandler(
            PlayerInventoryController playerInventoryController,
            InputKeyActionsSettings inputKeyActionsSettings,
            ILocalizationSystem localizationSystem
            ) : base( inputKeyActionsSettings.InteractAction )
        {
            _playerInventoryController = playerInventoryController ?? throw new ArgumentNullException( nameof(playerInventoryController) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }
        
        public override void Initialize( IObservable target )
        {
            base.Initialize( target );
            
            _item = (ItemObject)target;
            
            ContextMenuOperation.Key = _inputKeyAction.GetDisplayKey();
            ContextMenuOperation.Name = _localizationSystem.Translate( LocalizationIds.UI_CONTROL_TAKE );
        }

        public override void Dispose()
        {
            _item = null;
            base.Dispose();
        }
        
        protected override void Completed()
        {
            if ( !IsEnable ) return;

            _playerInventoryController.PickUpItem( _item );
            
            base.Completed();
        }
    }
}