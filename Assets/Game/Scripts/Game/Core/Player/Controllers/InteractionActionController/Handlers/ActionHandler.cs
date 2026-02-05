using System;

namespace Game.Core.Player
{
    public abstract class ActionHandler
    {
        public event Action< ActionHandler > OnCompleted;
        public event Action OnStarted;
        public event Action< float > OnProgressChanged;
        public event Action OnFinished;
        
        public bool IsEnable { get; protected set; }

        public virtual void Dispose()
        {
            Disable();
        }

        public abstract void Enable();
        public abstract void Disable();

        protected virtual void InteractStarted()
        {
            OnStarted?.Invoke();
        }
        
        protected virtual void InteractProgress( float value )
        {
            OnProgressChanged?.Invoke( value );
        }
        
        protected virtual void InteractFinished( bool result )
        {
            OnFinished?.Invoke();
        }
        
        protected virtual void Completed()
        {
            OnCompleted?.Invoke( this );
        }
    }
}