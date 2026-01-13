using Game.Core.World.InteractionSystem;
using UnityEngine;

namespace Game.Core.Entity
{
    public abstract class InteractableObject : ObservableObject
    {
        [ SerializeField ] private Vector3 _pointerOffset = Vector3.zero;
        
        public Vector3 PointerPosition => transform.position + _pointerOffset;
        
        public virtual void Interact( IInteractor interactor )
        {
            
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere( PointerPosition, 0.01f );
        }
    }
}