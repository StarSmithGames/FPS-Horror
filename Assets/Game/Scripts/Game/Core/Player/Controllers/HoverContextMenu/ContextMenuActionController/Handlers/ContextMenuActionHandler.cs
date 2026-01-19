using Game.Core.Entity;
using Game.Core.UI;
using System;

namespace Game.Core.Player
{
    public abstract class ContextMenuActionHandler
    {
        public event Action< ContextMenuActionHandler > OnCompleted;
        
        public bool IsEnable { get; protected set; }

        public ContextMenuOperation ContextMenuOperation { get; protected set; } = new();

        public virtual void Initialize( ObservableObject target ) {}
        public virtual void Dispose()
        {
            Disable();
        }

        public abstract void Enable( UIInfoButton ui );
        public abstract void Disable();
        
        protected virtual void Completed()
        {
            OnCompleted?.Invoke( this );
        }
    }
}