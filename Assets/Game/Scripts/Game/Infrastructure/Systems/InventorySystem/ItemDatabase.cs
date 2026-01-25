using Game.Core.UI.ContextMenu;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.InventorySystem
{
    [ CreateAssetMenu( fileName = "ItemDatabase", menuName = "Game/Inventory/ItemDatabase" ) ]
    public sealed class ItemDatabase : ScriptableObject
    {
        [ field: SerializeField ] public ContextMenuScheme ContextMenuScheme { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public List< ItemConfig > AllItems { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public ItemConfig LighterConfig { get; private set; }

        public const string FUSE = "fuse";
    }
}