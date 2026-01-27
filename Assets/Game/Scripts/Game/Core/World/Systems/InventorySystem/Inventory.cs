using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.World.InventorySystem
{
    public sealed class Inventory
    {
        public List< ItemModel > Items { get; } = new();

        public void AddItem( ItemConfig config )
        {
            var item = GetItem( config.UID );
            if ( item == null )
            {
                Items.Add( new( config, 1 ) );
            }
            else
            {
                item.Quantity++;
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

        public ItemModel GetItem( ItemConfig config ) => GetItem( config.UID );
        public ItemModel GetItem( string uid )
        {
            return Items.Find( ( x ) => string.Equals( x.UID, uid, StringComparison.InvariantCultureIgnoreCase ) );
        }
    }
}