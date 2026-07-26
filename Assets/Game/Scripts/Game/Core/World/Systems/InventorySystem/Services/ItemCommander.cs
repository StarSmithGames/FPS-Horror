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

        public void Use( InventoryItem inventoryItem )
        {
            Debug.LogError( "Use" );
        }

        public void EquipUnequip( InventoryItem inventoryItem )
        {
            var equipmentController = _playerControllersService.GetAs< PlayerEquipmentController >();
            equipmentController.Equip( inventoryItem );
        }

        public void Examine( InventoryItem inventoryItem )
        {
            var inspectionController = _playerControllersService.GetAs< PlayerInspectionController >();
            inspectionController.InspectItemFromContextMenu( inventoryItem );
        }
        
        public void Drop( InventoryItem inventoryItem )
        {
            Debug.LogError( "Drop" );
        }
    }
}