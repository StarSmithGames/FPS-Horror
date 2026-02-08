using Game.Core.Entity;
using Game.Core.World.InventorySystem;
using Game.Core.World.LibrarySystem;
using PuzzlescapeGames.Localization;
using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerLibraryController
    {
        public event Action OnChanged;
        
        public Library Library { get; } = new();

        private readonly ItemDatabase _itemDatabase;
        private readonly ILocalizationSystem _localizationSystem;
        
        public PlayerLibraryController(
            ItemDatabase itemDatabase,
            ILocalizationSystem localizationSystem
            )
        {
            _itemDatabase = itemDatabase ?? throw new ArgumentNullException( nameof(itemDatabase) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }

        public void Add( ItemObject item )
        {
            if ( Library.Contains( item.Config.UID ) )
                return;
            
            Library.AddItem( new()
            {
                UID = item.Config.UID,
                Type = 0
            } );
            
            OnChanged?.Invoke();
        }

        public string GetItemName( LibraryItem item )
        {
            if ( item.Type == 0 )
            {
                var config = _itemDatabase.GetItem( item.UID );
                return _localizationSystem.Translate( config.NameId );
            }

            return string.Empty;
        }
        
        public string GetItemDescription( LibraryItem item )
        {
            if ( item.Type == 0 )
            {
                var config = _itemDatabase.GetItem( item.UID );
                return _localizationSystem.Translate( config.DescriptionId );
            }

            return string.Empty;
        }
    }
}