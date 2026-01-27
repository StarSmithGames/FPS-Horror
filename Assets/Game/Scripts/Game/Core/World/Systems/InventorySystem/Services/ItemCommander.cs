using Game.Core.Player;
using System;
using UnityEngine;

namespace Game.Core.World.InventorySystem
{
    public sealed class ItemCommander
    {
        private readonly PlayerControllersService _playerControllersService;
        
        public ItemCommander( PlayerControllersService playerControllersService )
        {
            _playerControllersService = playerControllersService ?? throw new ArgumentNullException( nameof(playerControllersService) );
        }

        public void Use( ItemModel item )
        {
            Debug.LogError( "Use" );
        }

        public void EquipUnequip( ItemModel item )
        {
            var equipmentController = _playerControllersService.GetAs< PlayerEquipmentController >();
            equipmentController.Equip( item );
        }

        public void Examine( ItemModel item )
        {
            Debug.LogError( "Examine" );
        }
        
        public void Drop( ItemModel item )
        {
            Debug.LogError( "Drop" );
        }
    }
}