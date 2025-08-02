using Game.Core.World.InteractionSystem;

namespace Game.Core.Entity
{
    public abstract class ObservableEntityObject : EntityObject, IObservable
    {
        public virtual void StartObserve() {}

        public virtual void Observe() {}

        public virtual void EndObserve() {}
    }
}