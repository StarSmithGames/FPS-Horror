using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Core.World.InventorySystem
{
    [ CreateAssetMenu( fileName = "ItemDatabase", menuName = "Game/Inventory/ItemDatabase" ) ]
    public sealed class ItemDatabase : ScriptableObject
    {
        [ field: SerializeField ] public List< ItemConfig > AllItems { get; private set; }
        [ field: SerializeField ] public List< NoteConfig > AllNotes { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public ItemConfig LighterConfig { get; private set; }

        public const string FUSE = "fuse";
        
        public ItemConfig GetItem( string uid )
        {
            var item = AllItems.FirstOrDefault( ( x ) => string.Equals( x.UID, uid, StringComparison.InvariantCultureIgnoreCase ) );
            if( item != null ) return item;
            var note = AllNotes.FirstOrDefault( ( x ) => string.Equals( x.UID, uid, StringComparison.InvariantCultureIgnoreCase ) );
            if( note != null ) return note;

            return null;
        }
    }
}