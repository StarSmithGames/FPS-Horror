using Game.Core.World.InteractionSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Entity
{
    public abstract class ObservableObject : EntityObject, IObservable
    {
        [ Header( "SETTINGS" ) ]
        [ SerializeField ] protected List< Collider > _colliders = new();
        
        public bool IsCollidersEnabled { get; protected set; }
        
        public IReadOnlyList< Collider > Colliders => _colliders;

        public virtual void StartObserve() {}

        public virtual void Observe() {}

        public virtual void EndObserve() {}
        
        
        public void EnableCollider( bool trigger )
        {
            for ( int i = 0; i < _colliders.Count; i++ )
            {
                _colliders[ i ].enabled = trigger;
            }

            IsCollidersEnabled = trigger;
        }
    }
}