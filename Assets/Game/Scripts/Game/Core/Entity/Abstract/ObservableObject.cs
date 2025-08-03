using Game.Core.World.InteractionSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Entity
{
    public abstract class ObservableObject : EntityObject, IObservable
    {
        [ SerializeField ] private List< Collider > _colliders = new();
        
        public virtual void StartObserve() {}

        public virtual void Observe() {}

        public virtual void EndObserve() {}
        
        
        public void EnableCollider( bool trigger )
        {
            for ( int i = 0; i < _colliders.Count; i++ )
            {
                _colliders[ i ].enabled = trigger;
            }
        }
    }
}