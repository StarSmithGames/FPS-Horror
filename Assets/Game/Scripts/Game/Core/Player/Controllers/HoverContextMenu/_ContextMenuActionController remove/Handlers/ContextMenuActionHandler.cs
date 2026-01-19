using Game.Core.Entity;
using Game.Core.UI;

namespace Game.Core.Player
{
    public abstract class ContextMenuActionHandler : ActionHandler
    {
        public ContextMenuOperation ContextMenuOperation { get; protected set; } = new();

        public virtual void Initialize( ObservableObject target ) {}

        public virtual void Set( UIInfoButton ui ) {}
    }
}