using Game.Core.World.InteractionSystem;

namespace Game.Core.Entity
{
    public abstract class InteractableEntityObject : ObservableEntityObject, IInteractable
    {
        public virtual void Interact() {}
    }
}