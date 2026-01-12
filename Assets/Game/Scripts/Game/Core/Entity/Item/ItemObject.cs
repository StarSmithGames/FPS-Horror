using Game.Core.World.InspectionSystem;
using Game.Systems.InventorySystem;
using System;
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
        [ field: SerializeField ] public GameObject Root { get; private set; }
        [ field: SerializeField ] public InspectionSettings InspectionSettings { get; private set; }

        public virtual Type ControllerType { get; }
        public ItemController Controller { get; private set; }

        private int _oldLayer;
        
        public void SetController( ItemController controller )
        {
            Controller = controller;
        }

        public void SetLayer( string layer )
        {
            _oldLayer = Root.layer;
            SetLayerRecursively( Root, LayerMask.NameToLayer( layer ) );
        }
        
        public void ResetLayer()
        {
            SetLayerRecursively( Root, _oldLayer );
        }

        private void SetLayerRecursively( GameObject obj, int layer )
        {
            obj.layer = layer;
            foreach ( Transform child in obj.transform )
            {
                SetLayerRecursively( child.gameObject, layer );
            }
        }

        public Transform TransformInspection => transform;
    }
}