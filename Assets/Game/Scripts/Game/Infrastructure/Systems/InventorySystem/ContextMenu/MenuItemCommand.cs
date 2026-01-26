namespace Game.Systems.InventorySystem.ContextMenu
{
    public sealed class MenuItemCommand
    {
        public ContextMenuItem Item;
        public ICommand Command;
        
        public MenuItemCommand( ContextMenuItem item, ICommand command )
        {
            Item = item;
            Command = command;
        }
    }
}