using Game.Core.World.InteractionSystem;

namespace Game.Core.Entity
{
    public abstract class PuzzleObject : ObservableObject, IInteractable
    {
        public virtual void Interact( IInteractor interactor )
        {
            
        }
    }
}