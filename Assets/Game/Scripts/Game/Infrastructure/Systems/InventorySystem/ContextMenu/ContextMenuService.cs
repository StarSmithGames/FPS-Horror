using System;
using System.Collections.Generic;

namespace Game.Systems.InventorySystem.ContextMenu
{
    public sealed class ContextMenuService
    {
        private readonly ContextMenuItems _settings;
        
        public ContextMenuService( ContextMenuItems settings )
        {
            _settings = settings ?? throw new ArgumentNullException( nameof(settings) );
        }

        public List< ContextMenuItem > GetWeaponMenu( bool isEquipped )
        {
            return new()
            {
                ( isEquipped ? _settings.Unequip : _settings.Equip ),
                _settings.Examine,
                _settings.Discard,
                _settings.Shortcut
            };
        }
        
        public List< ContextMenuItem > GetItemMenu( bool isCanDiscard )
        {
            return new()
            {
                _settings.Examine,
                _settings.Discard//disable
            };
        }
    }
}