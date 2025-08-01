using Game.Core.World.InteractionSystem;

namespace Game.Core.Entity
{
    public abstract class PickableEntityObject : EntityObject, IPickable, IObservable
    {
        public void Pickup()
        {
            
        }

        public virtual void StartObserve()
        {
        }

        public virtual void Observe()
        {
        }

        public virtual void EndObserve()
        {
        }
    }
}