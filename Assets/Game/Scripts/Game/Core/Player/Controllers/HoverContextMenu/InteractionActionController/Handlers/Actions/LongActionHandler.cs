using Game.Core.UI;
using Game.Managers.InputManager;
using System;

namespace Game.Core.Player
{
    public abstract class LongActionHandler : ActionHandler
    {
        protected readonly InputActionProvider _provider;
        protected readonly InputKeyAction _inputKeyAction;

        public LongActionHandler( InputKeyAction inputKeyAction, float duration = 0.33f )
        {
            _inputKeyAction = inputKeyAction ?? throw new ArgumentNullException( nameof(inputKeyAction) );
            _provider = new( inputKeyAction.InputAction, Completed, duration, progress: InteractProgress, callback: InteractFinished );
        }

        public override void Enable()
        {
            _provider.Enable();
            
            IsEnable = true;
        }

        public override void Disable()
        {
            _provider.Disable();
            // _ui = null;
            
            IsEnable = false;
        }
        
        private void InteractProgress( float value )
        {
            // _ui.SetFillAmount( value );
        }

        private void InteractFinished( bool result )
        {
            if ( !IsEnable ) return;
            
            // _ui.SetFillAmount( 0 );
        }
    }
}