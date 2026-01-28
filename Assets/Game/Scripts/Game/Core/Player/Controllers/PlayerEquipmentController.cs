using Game.Core.Entity;
using Game.Core.Entity.Weapon;
using Game.Core.World.EquipmentSystem;
using Game.Core.World.InventorySystem;
using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerEquipmentController
    {
        public event Action OnEquipChanged;
        
        public Equipment Equipment { get; } = new();
        
        private LighterController _lighterController;
        private WeaponController _weaponController;
        
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

        public bool IsEquipped( ItemModel item ) => Equipment.EquippedItem != null && Equipment.EquippedItem == item;

        public void Equip( ItemModel item )
        {
            if ( IsEquipped( item ) )
            {
                Equipment.EquippedItem = null;
                _playerAvatar.HandRight.DoRemoveItem();
                _weaponController = null;
                
                OnEquipChanged?.Invoke();
                return;
            }
            Equipment.EquippedItem = item;

            _weaponController = (WeaponController)_itemFactory.Create( item.Config.Prefab );
            _weaponController.Initialize();

            _playerAvatar.HandRight.DoAddItem( _weaponController.View );
            
            OnEquipChanged?.Invoke();
        }
        
        public void SelectLighter()
        {
            if ( _lighterController == null )
            {
                _lighterController = (LighterController)_itemFactory.Create( _itemDatabase.LighterConfig.Prefab, _playerAvatar.HandRight.Model.Root );
                _lighterController.View.transform.localPosition = Vector3.zero;
                _lighterController.View.transform.localRotation = Quaternion.identity;
                _lighterController.Initialize();
            }

            if ( _lighterController.IsOpened && _lighterController.IsHasFlame )
            {
                _lighterController.Hide();
            }
            else
            {
                _lighterController.Show();
            }
        }
    }
}