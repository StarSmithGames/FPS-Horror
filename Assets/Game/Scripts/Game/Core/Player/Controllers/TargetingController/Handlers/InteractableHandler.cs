using System.Collections.Generic;

namespace Game.Core.Player
{
    public abstract class InteractableHandler
    {
        public abstract List< ContextMenuOperation > GetContextMenuOptions();
    }
}