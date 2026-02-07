using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core.World.LibrarySystem
{
    public sealed class Library
    {
        public IReadOnlyList< LibraryItem > Items => _items;
        private readonly List< LibraryItem > _items = new();

        public Library()
        {
            
        }

        public void Load()
        {
            
        }
        
        public void AddItem( LibraryItem item )
        {
            _items.Add( item );
        }

        public bool Contains( string UID ) => _items.FirstOrDefault( ( x ) => string.Equals( x.UID, UID, StringComparison.InvariantCultureIgnoreCase ) ) != null;
    }
}