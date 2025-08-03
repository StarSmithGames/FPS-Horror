using Game.Core.World.InspectionSystem;
using UnityEngine;

namespace Game.Core.Entity
{
    public abstract class InspectableEntityObject : InteractableEntityObject, IInspectable
    {
        [ field: SerializeField ] public InspectionSettings InspectionSettings { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public string NameId { get; private set; }
        [ field: SerializeField ] public string TextId { get; private set; }

        public Transform TransformInspection => transform;
    }
}