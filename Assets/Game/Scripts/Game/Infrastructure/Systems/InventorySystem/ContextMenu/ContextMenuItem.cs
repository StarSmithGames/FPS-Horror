using UnityEngine;

namespace Game.Systems.InventorySystem.ContextMenu
{
    [ CreateAssetMenu( fileName = "Item", menuName = "Game/ContextMenuItem" ) ]
    public sealed class ContextMenuItem : ScriptableObject
    {
        [ field: SerializeField ] public string UID { get; private set; }
        [ field: SerializeField ] public string NameId { get; private set; }
        [ field: SerializeField ] public Sprite Icon { get; private set; }
        [ field: SerializeField ] public Vector2 IconSize { get; private set; } = new( 16, 16 );
    }
}