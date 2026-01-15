using Game.Core.World.InteractionSystem;
using UnityEngine;

namespace Game.Core.Entity
{
    public abstract class InteractableObject : ObservableObject
    {
        [ SerializeField ] private Vector3 _pointerStartOffset = Vector3.zero;
        [ SerializeField ] private Vector3 _pointerEndOffset = Vector3.zero;
        
        public Vector3 PointerStartPosition => transform.position + _pointerStartOffset;
        public Vector3 PointerEndPosition => transform.position + _pointerEndOffset;
        
        public virtual void Interact( IInteractor interactor )
        {
            
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere( PointerStartPosition, 0.01f );
            Gizmos.DrawSphere( PointerEndPosition, 0.01f );
        }
    }
}