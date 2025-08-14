using System;
using System.Collections.Generic;

namespace Game.Systems.InventorySystem
{
    public sealed class Inventory
    {
        public List< Item > Items { get; private set; } = new();

        public void AddItem( ItemConfig config )
        {
            var item = Items.Find( ( x ) => string.Equals( x.UID, config.UID, StringComparison.InvariantCultureIgnoreCase ) );
            if ( item == null )
            {
                Items.Add( new()
                {
                    UID = config.UID,
                    Count = 1
                } );
            }
            else
            {
                item.Count++;
            }
        }
    }
}