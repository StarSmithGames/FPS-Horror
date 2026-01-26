using System;

namespace Game.Systems.InventorySystem.Commands
{
    public abstract class InventoryItemCommand : ICommand
    {
        protected readonly ItemCommander _itemCommander;
        protected readonly ItemModel _item;
        
        public InventoryItemCommand( ItemCommander itemCommander, ItemModel item )
        {
            _itemCommander = itemCommander ?? throw new ArgumentNullException( nameof(itemCommander) );
            _item = item ?? throw new ArgumentNullException( nameof(item) );
        }

        public virtual bool CanExecute() => true;

        public abstract void Execute();

        public void Undo() => throw new System.NotImplementedException();
    }
}