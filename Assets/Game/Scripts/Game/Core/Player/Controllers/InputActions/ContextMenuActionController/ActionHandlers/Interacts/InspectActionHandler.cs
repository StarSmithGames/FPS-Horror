using Game.Core.Entity;
using Game.Core.World.InteractionSystem;
using Game.Managers.InputManager;
using PuzzlescapeGames.Localization;
using StarSmithGames.Localization;
using System;

namespace Game.Core.Player
{
    public sealed class InspectActionHandler : QuickActionHandler
    {
        private ItemObject _item;

        private readonly PlayerController _playerController;
        private readonly ILocalizationSystem _localizationSystem;
        
        public InspectActionHandler(
            PlayerController playerController,
            InputKeyActionsSettings inputKeyActionsSettings,
            ILocalizationSystem localizationSystem
            ) : base( inputKeyActionsSettings.InteractAction )
        {
            _playerController = playerController ?? throw new ArgumentNullException( nameof(playerController) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }

        public override void Initialize( IObservable target )
        {
            base.Initialize( target );
            
            _item = (ItemObject)target;
            
            ContextMenuOperation.Key = _inputKeyAction.GetDisplayKey();
            ContextMenuOperation.Name = _localizationSystem.Translate( LocalizationIds.UI_CONTROL_INSPECT );
        }
        
        public override void Dispose()
        {
            _item = null;

            base.Dispose();
        }

        protected override void Completed()
        {
            if ( !IsEnable ) return;
            
            _playerController.ServiceLocator.GetAs< PlayerInspectionController >().InspectItem( _item );
            
            base.Completed();
        }
    }
}