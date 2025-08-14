using Game.Core.UI;
using Game.Managers.InputManager;
using System;

namespace Game.Core.Player
{
    public abstract class QuickActionHandler : ContextMenuActionHandler
    {
        protected readonly InputKeyAction _inputKeyAction;
        protected readonly InputActionHolder _holder;
        
        public QuickActionHandler( InputKeyAction inputAction )
        {
            _inputKeyAction = inputAction ?? throw new ArgumentNullException( nameof(inputAction) );
            _holder = new( inputAction.InputAction, Completed );
        }
        
        public override void Enable( UIInfoButton ui )
        {
            _holder.Enable();
            
            IsEnable = true;
        }
        
        public override void Disable()
        {
            _holder.Disable();

            IsEnable = false;
        }
    }
}