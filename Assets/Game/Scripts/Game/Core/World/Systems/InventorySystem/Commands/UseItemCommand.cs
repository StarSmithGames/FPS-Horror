namespace Game.Core.World.InventorySystem.Commands
{
    public sealed class UseItemCommand : InventoryItemCommand
    {
        public UseItemCommand(
            ItemCommander itemCommander,
            InventoryItem inventoryItem
            ) : base( itemCommander, inventoryItem )
        {
            
        }

        public override void Execute() => _itemCommander.Use( InventoryItem );
    }
}