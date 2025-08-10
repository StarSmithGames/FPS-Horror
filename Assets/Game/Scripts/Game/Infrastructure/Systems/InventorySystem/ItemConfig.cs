using Game.Core.Entity;
using UnityEngine;

namespace Game.Systems.InventorySystem
{
    [ CreateAssetMenu( fileName = "Item", menuName = "Game/Inventory/Item" ) ]
    public sealed class ItemConfig : ScriptableObject
    {
        [ field: SerializeField ] public ItemObject Prefab { get; private set; }
    }
}