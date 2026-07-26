namespace Game.Core.World.InventorySystem.Commands
{
    public sealed class DropItemCommand : InventoryItemCommand
    {
        public DropItemCommand(
            ItemCommander itemCommander,
            InventoryItem inventoryItem
            ) : base( itemCommander, inventoryItem )
        {
        }

        public override void Execute() => _itemCommander.Drop( InventoryItem );
    }
}