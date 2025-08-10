using Game.Core.World.InspectionSystem;
using Game.Systems.InventorySystem;
using UnityEngine;

namespace Game.Core.Entity
{
    public abstract class ItemObject : ObservableObject
    {
        [ field: Header( "INFO" ) ]
        [ field: SerializeField ] public string NameId { get; private set; }
        [ field: SerializeField ] public string TextId { get; private set; }
        [ field: SerializeField ] public ItemConfig Config { get; private set; }
        [ field: Header( "INSPECT" ) ]
        [ field: SerializeField ] public InspectionSettings InspectionSettings { get; private set; }

        public ItemController Controller { get; private set; }

        public void SetController( ItemController controller )
        {
            Controller = controller;
        }
        
        public void Interact()
        {
            Debug.LogError( "Interact" );
        }
        
        public Transform TransformInspection => transform;
    }
}