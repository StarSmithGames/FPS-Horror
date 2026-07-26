using System;

namespace Game.Core.World.InventorySystem.Commands
{
    public abstract class InventoryItemCommand : ICommand
    {
        protected readonly ItemCommander _itemCommander;
        protected readonly InventoryItem InventoryItem;
        
        public InventoryItemCommand( ItemCommander itemCommander, InventoryItem inventoryItem )
        {
            _itemCommander = itemCommander ?? throw new ArgumentNullException( nameof(itemCommander) );
            InventoryItem = inventoryItem ?? throw new ArgumentNullException( nameof(inventoryItem) );
        }

        public virtual bool CanExecute() => true;

        public abstract void Execute();

        public void Undo() => throw new System.NotImplementedException();
    }
}