using Game.Core.Entity;
using Game.Systems.InventorySystem;
using System;

namespace Game.Core.Player
{
    public sealed class PlayerInventoryController
    {
        private LighterController _lighterController;

        private bool _isLighterShowing;
        
        private readonly ItemDatabase _itemDatabase;
        private readonly ItemFactory _itemFactory;
        
        public PlayerInventoryController(
            ItemDatabase itemDatabase,
            ItemFactory itemFactory
            )
        {
            _itemDatabase = itemDatabase ?? throw new ArgumentNullException( nameof(itemDatabase) );
            _itemFactory = itemFactory ?? throw new ArgumentNullException( nameof(itemFactory) );
        }
        
        public void SelectLighter()
        {
            if ( _lighterController == null )
            {
                _lighterController = _itemFactory.Create< LighterController >( _itemDatabase.LighterConfig.Prefab );
            }

            if ( _isLighterShowing )
            {
                _lighterController.Hide();
            }
            else
            {
                _lighterController.Show();
            }
            _isLighterShowing = !_isLighterShowing;
        }
    }
}