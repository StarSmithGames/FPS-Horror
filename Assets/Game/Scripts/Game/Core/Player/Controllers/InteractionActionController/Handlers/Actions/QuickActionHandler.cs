using Game.Managers.InputManager;
using System;

namespace Game.Core.Player
{
    public abstract class QuickActionHandler : ActionHandler
    {
        protected readonly InputActionVoidWrap _holder;
        protected readonly InputKeyAction _inputKeyAction;
        
        public QuickActionHandler( InputKeyAction inputAction )
        {
            _inputKeyAction = inputAction ?? throw new ArgumentNullException( nameof(inputAction) );
            
            _holder = new InputActionVoidWrap( _inputKeyAction.InputAction, Completed );
        }

        public override void Enable()
        {
            InputActionManager.AddInputActionWrap( _holder );
            
            IsEnable = true;
        }
        
        public override void Disable()
        {
            InputActionManager.RemoveInputActionWrap( _holder );
            
            IsEnable = false;
        }
    }
}