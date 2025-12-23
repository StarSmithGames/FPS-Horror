using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.InventorySystem
{
    public sealed class Inventory
    {
        public List< ItemModel > Items { get; private set; } = new();

        public void AddItem( ItemConfig config )
        {
            Debug.LogError( ( config == null ));
            var item = GetItem( config.UID );
            if ( item == null )
            {
                Items.Add( new( 1 )
                {
                    UID = config.UID,
                    Config = config,
                } );
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