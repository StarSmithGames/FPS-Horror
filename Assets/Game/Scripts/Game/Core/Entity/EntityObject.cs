using System;
using UnityEngine;

namespace Game.Core.Entity
{
    public abstract class EntityObject : MonoBehaviour
    {
        public event Action< EntityObject > OnDisposed;

        protected virtual void Awake() {}

        public virtual void Dispose()
        {
            OnDisposed?.Invoke( this );
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}