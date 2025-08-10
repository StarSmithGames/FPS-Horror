using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.World.InteractionSystem;
using Game.Managers.InputManager;
using PuzzlescapeGames.Localization;

namespace Game.Core.Player
{
    public class PickUpActionHandler : ContextMenuActionHandler
    {
        private InputActionProvider _provider;
        private ItemObject _item;
        private UIInfoButton _ui;
        
        public PickUpActionHandler(
            InputKeyActionsSettings inputKeyActionsSettings,
            ILocalizationSystem localizationSystem
            )
        {
            _provider = new( inputKeyActionsSettings.InteractAction.InputAction, Completed, 0.33f, progress: InteractProgress, callback: InteractFinished );

            if ( ContextMenuOperation == null )
            {
                ContextMenuOperation = new();
            }
            ContextMenuOperation.Key = inputKeyActionsSettings.InteractAction.GetDisplayKey();
            ContextMenuOperation.Name = localizationSystem.Translate( inputKeyActionsSettings.InteractAction.NameId );
        }
        
        public override void Initialize( IObservable target )
        {
            _item = (ItemObject)target;
        }

        public override void Enable( UIInfoButton ui )
        {
            _ui = ui;
            _provider.Enable();
            
            IsEnable = true;
        }

        public override void Disable()
        {
            _provider.Disable();
            _ui = null;
            _item = null;

            IsEnable = false;
        }
        
        private void InteractProgress( float value )
        {
            _ui.SetFillAmount( value );
        }

        private void InteractFinished( bool result )
        {
            if ( !IsEnable ) return;
            
            _ui.SetFillAmount( 0 );
        }

        protected override void Completed()
        {
            if ( !IsEnable ) return;
            
            _item.Interact();
            
            base.Completed();
        }
    }
}