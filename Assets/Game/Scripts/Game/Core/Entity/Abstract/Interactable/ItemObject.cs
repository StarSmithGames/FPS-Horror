using Game.Core.World.InspectionSystem;
using UnityEngine;

namespace Game.Core.Entity
{
    public abstract class ItemObject : ObservableObject
    {
        [ field: SerializeField ] public string NameId { get; private set; }
        [ field: SerializeField ] public string TextId { get; private set; }
        [ field: SerializeField ] public InspectionSettings InspectionSettings { get; private set; }

        public void Interact()
        {
            Debug.LogError( "Interact" );
        }
        
        public Transform TransformInspection => transform;
    }
}