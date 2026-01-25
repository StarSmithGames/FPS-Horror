using UnityEngine;

namespace Game.Core.UI.ContextMenu
{
    [ CreateAssetMenu( fileName = "Scheme", menuName = "Game/ContextMenuScheme" ) ]
    public sealed class ContextMenuScheme : ScriptableObject
    {
        [ field: SerializeField ] public ContextMenuItem Use { get; private set; }
        [ field: SerializeField ] public ContextMenuItem Equip { get; private set; }
        [ field: SerializeField ] public ContextMenuItem Unequip { get; private set; }
        [ field: SerializeField ] public ContextMenuItem Examine { get; private set; }
        [ field: SerializeField ] public ContextMenuItem Combain { get; private set; }
        [ field: SerializeField ] public ContextMenuItem Discard { get; private set; }
        [ field: SerializeField ] public ContextMenuItem Shortcut { get; private set; }
    }
}