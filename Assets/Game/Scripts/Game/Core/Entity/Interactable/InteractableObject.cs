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

        public virtual Vector3 GetInteractPointerPosition()
        {
            
            Vector3 position = Vector3.zero;
            InteractableSettings.Points.ForEach( ( point ) => position += point.GetPointerPosition( transform ) );
            return position / InteractableSettings.Points.Count;
        }

        public virtual Vector3 GetInteractPointerPosition( Transform from )
        {
            return InteractableSettings.Points.FirstOrDefault()?.GetPointerPosition( transform ) ?? transform.position;
        }

        public virtual Vector3 GetLastPointerPosition()
        {
            return InteractableSettings.Points.LastOrDefault()?.GetPointerPosition( transform ) ?? transform.position;
        }
        
#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            if ( !IsCollidersEnabledInEditor ) return;
            
            Gizmos.color = Color.blue;
            foreach ( var point in InteractableSettings.Points )
            {
                Gizmos.DrawSphere( point.GetPointerPosition( transform ), 0.01f );
            }
        }
#endif
    }
}