using Game.Core.World.InventorySystem.Commands;
using System;
using System.Collections.Generic;

namespace Game.Core.World.InventorySystem.ContextMenu
{
    public sealed class ContextMenuService
    {
        private readonly ContextMenuItems _settings;
        private readonly ItemCommander _itemCommander;
        
        public ContextMenuService(
            ContextMenuItems settings,
            ItemCommander itemCommander
            )
        {
            _settings = settings ?? throw new ArgumentNullException( nameof(settings) );
            _itemCommander = itemCommander ?? throw new ArgumentNullException( nameof(itemCommander) );
        }

        public List< MenuItemCommand > GetWeaponMenu( InventoryItem inventoryItem, bool isEquipped )
        {
            return new()
            {
                new( isEquipped ? _settings.Unequip : _settings.Equip, new EquipUnequipItemCommand( _itemCommander, inventoryItem ) ),
                new( _settings.Examine, new ExamineItemCommand( _itemCommander, inventoryItem ) ),
                new( _settings.Discard, new DropItemCommand( _itemCommander, inventoryItem) )
                // _settings.Shortcut
            };
        }
        
        public List< MenuItemCommand > GetItemMenu( InventoryItem inventoryItem )
        {
            return new()
            {
                new( _settings.Examine, new ExamineItemCommand( _itemCommander, inventoryItem ) ),
                // _settings.Discard//disable
            };
        }
    }
}