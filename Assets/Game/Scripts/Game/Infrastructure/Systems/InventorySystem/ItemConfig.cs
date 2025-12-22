using Game.Core.Entity;
using UnityEngine;

namespace Game.Systems.InventorySystem
{
    [ CreateAssetMenu( fileName = "Item", menuName = "Game/Inventory/Item" ) ]
    public sealed class ItemConfig : ScriptableObject
    {
        [ field: SerializeField ] public string UID { get; private set; }
        [ field: SerializeField ] public Sprite Icon { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public ItemObject Prefab { get; private set; }
    }
}