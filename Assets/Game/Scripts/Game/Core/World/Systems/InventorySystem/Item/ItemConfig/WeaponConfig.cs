using UnityEngine;

namespace Game.Core.World.InventorySystem
{
    [ CreateAssetMenu( fileName = "Item", menuName = "Game/Inventory/Item/Weapon" ) ]
    public sealed class WeaponConfig : ItemConfig
    {
        [ field: SerializeField ] public int MagSize { get; private set; } = 8;
        [ field: SerializeField ] public float Damage { get; private set; } = 25f;
        [ field: SerializeField ] public float FireRate { get; private set; } = 2f;
    }
}