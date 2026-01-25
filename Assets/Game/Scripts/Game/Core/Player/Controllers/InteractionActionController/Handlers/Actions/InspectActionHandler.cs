using Game.Core.Entity;
using Game.Managers.InputManager;
using System;

namespace Game.Core.Player
{
    public sealed class InspectActionHandler : QuickActionHandler
    {
        private ItemObject _item;

        private readonly PlayerController _playerController;
        
        public InspectActionHandler(
            PlayerController playerController,
            InputKeyActionsSettings inputKeyActionsSettings
            ) : base( inputKeyActionsSettings.InteractAction )
        {
            _playerController = playerController ?? throw new ArgumentNullException( nameof(playerController) );
        }
        
        public void Set( ItemObject target )
        {
            _item = target;
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