using Game.Core.World.InteractionSystem;
using System.Linq;
using UnityEngine;

namespace Game.Core.Entity
{
    public abstract class InteractableObject : ObservableObject
    {
        [ field: SerializeField ] public InteractableSettings InteractableSettings { get; private set; }
        
        public virtual void Interact( IInteractor interactor )
        {
            
        }

        public Vector3 GetInteractPointerPosition()
        {
            return InteractableSettings.Points.FirstOrDefault()?.GetPointerPosition( transform ) ?? transform.position;
        }
        
        public Vector3 GetLastPointerPosition()
        {
            return InteractableSettings.Points.LastOrDefault()?.GetPointerPosition( transform ) ?? transform.position;
        }
        
        private void OnDrawGizmos()
        {
            if ( !IsCollidersEnabled ) return;
            
            Gizmos.color = Color.blue;
            foreach ( var point in InteractableSettings.Points )
            {
                Gizmos.DrawSphere( point.GetPointerPosition( transform ), 0.01f );
            }
        }
    }
}