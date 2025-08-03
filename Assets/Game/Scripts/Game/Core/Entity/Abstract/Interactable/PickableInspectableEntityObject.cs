using Game.Core.World.InspectionSystem;
using Game.Core.World.InteractionSystem;
using UnityEngine;

namespace Game.Core.Entity
{
    public abstract class PickableInspectableEntityObject : InteractableEntityObject, IPickable, IInspectable
    {
        [ field: SerializeField ] public InspectionSettings InspectionSettings { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public string NameId { get; private set; }
        [ field: SerializeField ] public string TextId { get; private set; }

        public Transform TransformInspection => transform;
    }
}