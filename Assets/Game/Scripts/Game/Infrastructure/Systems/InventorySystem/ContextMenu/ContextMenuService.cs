using Game.Systems.InventorySystem.Commands;
using System;
using System.Collections.Generic;

namespace Game.Systems.InventorySystem.ContextMenu
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

        public List< MenuItemCommand > GetWeaponMenu( ItemModel item, bool isEquipped )
        {
            return new()
            {
                new( isEquipped ? _settings.Unequip : _settings.Equip, new EquipUnequipItemCommand( _itemCommander, item ) ),
                new( _settings.Examine, new ExamineItemCommand( _itemCommander, item ) ),
                new( _settings.Discard, new DropItemCommand( _itemCommander, item) )
                // _settings.Shortcut
            };
        }
        
        public List< MenuItemCommand > GetItemMenu( ItemModel item )
        {
            return new()
            {
                new( _settings.Examine, new ExamineItemCommand( _itemCommander, item ) ),
                // _settings.Discard//disable
            };
        }
    }
}