using Game.Core.UI;
using Game.Core.World.InteractionSystem;
using System;

namespace Game.Core.Player
{
    public abstract class ContextMenuActionHandler
    {
        public event Action< ContextMenuActionHandler > OnCompleted;
        
        public bool IsEnable { get; protected set; }
        
        public ContextMenuOperation ContextMenuOperation { get; protected set; }

        public virtual void Initialize( IObservable target ) {}
        
        public abstract void Enable( UIInfoButton ui );
        public abstract void Disable();
        
        protected virtual void Completed()
        {
            OnCompleted?.Invoke( this );
        }
    }
}