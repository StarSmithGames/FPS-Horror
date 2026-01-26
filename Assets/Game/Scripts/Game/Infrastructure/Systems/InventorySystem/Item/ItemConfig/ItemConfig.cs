using Game.Core.Entity;
using UnityEngine;

namespace Game.Systems.InventorySystem
{
    [ CreateAssetMenu( fileName = "Item", menuName = "Game/Inventory/Item" ) ]
    public class ItemConfig : ScriptableObject
    {
        [ field: SerializeField ] public string UID { get; private set; }
        [ field: SerializeField ] public string NameId { get; private set; }
        [ field: TextArea ]
        [ field: SerializeField ] public string DescriptionId { get; private set; }
        [ field: SerializeField ] public Sprite Icon { get; private set; }
        [ field: SerializeField ] public int MaxStack { get; private set; } = 99;
        [ field: SerializeField ] public float Weight { get; private set; } = 0;
        [ field: Space ]
        [ field: SerializeField ] public ItemObject Prefab { get; private set; }
        
        public bool IsStackable => MaxStack > 1;
    }
}