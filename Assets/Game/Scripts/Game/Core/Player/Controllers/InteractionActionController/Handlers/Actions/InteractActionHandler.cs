using Game.Core.Entity;
using Game.Managers.InputManager;
using System;

namespace Game.Core.Player
{
    public sealed class InteractActionHandler : QuickActionHandler
    {
        private InteractableObject _interactable;

        private readonly PlayerController _playerController;
        
        public InteractActionHandler(
            PlayerController playerController,
            InputKeyActionsSettings inputKeyActionsSettings
            ) : base( inputKeyActionsSettings.InteractAction )
        {
            _playerController = playerController ?? throw new ArgumentNullException( nameof(playerController) );
        }
        
        public void Set( InteractableObject target )
        {
            _interactable = target;
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