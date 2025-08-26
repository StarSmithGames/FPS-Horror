using System;
using System.Collections.Generic;

namespace Game.Systems.InventorySystem
{
    public sealed class Inventory
    {
        public List< Item > Items { get; private set; } = new();

        public void AddItem( ItemConfig config ) => AddItem( config.UID );
        public void AddItem( string uid )
        {
            var item = GetItem( uid );
            if ( item == null )
            {
                Items.Add( new()
                {
                    UID = uid,
                    Count = 1
                } );
            }
            else
            {
                item.Count++;
            }
        }

        public void RemoveItem( ItemConfig config ) => RemoveItem( config.UID );
        public void RemoveItem( string uid )
        {
            var item = GetItem( uid );
            if ( item != null )
            {
                Items.Remove( item );
            }
        }

        public bool ContainsItem( ItemConfig config ) => ContainsItem( config.UID );
        public bool ContainsItem( string uid ) => GetItem( uid ) != null;

        public Item GetItem( ItemConfig config ) => GetItem( config.UID );
        public Item GetItem( string uid )
        {
            return Items.Find( ( x ) => string.Equals( x.UID, uid, StringComparison.InvariantCultureIgnoreCase ) );
        }
    }
}