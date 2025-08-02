using Game.Core.World.InteractionSystem;
using UnityEngine;

namespace Game.Core.Entity
{
    public abstract class PickableEntityObject : EntityObject, IPickable, IObservable
    {
        public void Interact()
        {
            Debug.LogError( "Interact" );
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