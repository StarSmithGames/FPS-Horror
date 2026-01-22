using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Entity
{
    [ System.Serializable ]
    public sealed class InteractableSettings
    {
        [ field: SerializeField ] public bool UseAnimation { get; private set; }
        [ field: SerializeField ] public List< Point > Points { get; private set; } = new();
        
        [ System.Serializable ]
        public sealed class Point
        {
            [ field: SerializeField ] public Transform CustomInteractablePoint { get; private set; }
            [ field: SerializeField ] public Vector3 Offset { get; private set; } = Vector3.zero;
            
            public Vector3 GetPointerPosition( Transform from ) => CustomInteractablePoint == null ? from.position + Offset : CustomInteractablePoint.position + Offset;
        }
    }
}