using Game.Core.Entity;
using Game.Core.Entity.Weapon;
using Game.Core.World.EquipmentSystem;
using Game.Core.World.InventorySystem;
using PuzzlescapeGames.Extensions;
using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerEquipmentController
    {
        public event Action OnEquipChanged;
        
        public Equipment Equipment { get; } = new();
        
        private LighterController _lighterController;
        
        private readonly ItemDatabase _itemDatabase;
        private readonly ItemFactory _itemFactory;
        private readonly PlayerAvatar _playerAvatar;
        
        public PlayerEquipmentController(
            ItemDatabase itemDatabase,
            ItemFactory itemFactory,
            PlayerAvatar playerAvatar
            )
        {
            _itemDatabase = itemDatabase ?? throw new ArgumentNullException( nameof(itemDatabase) );
            _itemFactory = itemFactory ?? throw new ArgumentNullException( nameof(itemFactory) );
            _playerAvatar = playerAvatar ?? throw new ArgumentNullException( nameof(playerAvatar) );
        }

        public bool IsEquipped( InventoryItem inventoryItem ) => Equipment.EquippedItem != null && Equipment.EquippedItem == inventoryItem;

        public void Equip( InventoryItem inventoryItem )
        {
            if ( IsEquipped( inventoryItem ) )
            {
                Equipment.EquippedItem = null;
                _playerAvatar.HandRight.DoRemoveItem();
                
                OnEquipChanged?.Invoke();
                return;
            }
            Equipment.EquippedItem = inventoryItem;
            if ( inventoryItem.View == null )
            {
                var controller = (WeaponController)_itemFactory.Create( inventoryItem.Config.Prefab );
                inventoryItem.SetView( controller.View );
            }
            _playerAvatar.HandRight.DoAddItem( inventoryItem.View );
            
            OnEquipChanged?.Invoke();
        }
        
        public void SelectLighter()
        {
            if ( _lighterController == null )
            {
                _lighterController = (LighterController)_itemFactory.Create( _itemDatabase.LighterConfig.Prefab );
                _lighterController.Initialize();
            }
            if ( _lighterController.IsInProcess ) return;
            
            if ( _lighterController.IsOpened )
            {
                _lighterController.Hide();
                _playerAvatar.HandRight.DoRemoveItem();
            }
            else
            {
                 _playerAvatar.HandRight.DoAddItem( _lighterController.View );
                _lighterController.Show();
            }
        }
    }
}