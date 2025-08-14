using Game.Core.Entity;
using Game.Systems.InventorySystem;
using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerInventoryController
    {
        private LighterController _lighterController;

        private readonly ItemDatabase _itemDatabase;
        private readonly ItemFactory _itemFactory;
        private readonly PlayerAvatar _playerAvatar;
        
        public PlayerInventoryController(
            ItemDatabase itemDatabase,
            ItemFactory itemFactory,
            PlayerAvatar playerAvatar
            )
        {
            _itemDatabase = itemDatabase ?? throw new ArgumentNullException( nameof(itemDatabase) );
            _itemFactory = itemFactory ?? throw new ArgumentNullException( nameof(itemFactory) );
            _playerAvatar = playerAvatar ?? throw new ArgumentNullException( nameof(playerAvatar) );
        }

        public void PickUpItem( ItemObject item )
        {
            if ( item.Controller != null )
            {
                item.SetController( null );
            }
            GameObject.Destroy( item.gameObject );
        }
        
        public void SelectLighter()
        {
            if ( _lighterController == null )
            {
                _lighterController = _itemFactory.Create< LighterController >( _itemDatabase.LighterConfig.Prefab, _playerAvatar.HandRight );
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