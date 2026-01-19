using System;

namespace Game.Core.Player
{
    public abstract class ActionHandler
    {
        public event Action< ActionHandler > OnCompleted;
        
        public bool IsEnable { get; protected set; }

        public virtual void Dispose()
        {
            Disable();
        }

        public abstract void Enable();
        public abstract void Disable();
        
        protected virtual void Completed()
        {
            OnCompleted?.Invoke( this );
        }
    }
}