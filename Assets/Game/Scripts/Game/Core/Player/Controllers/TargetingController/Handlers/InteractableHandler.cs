using System;
using System.Collections.Generic;

namespace Game.Core.Player
{
    public abstract class InteractableHandler
    {
        public event Action< InteractableHandler > OnCompleted;
        
        public abstract List< ContextMenuOperation > GetContextMenuOptions();

        protected void Completed()
        {
            OnCompleted?.Invoke( this );
        }
    }
}