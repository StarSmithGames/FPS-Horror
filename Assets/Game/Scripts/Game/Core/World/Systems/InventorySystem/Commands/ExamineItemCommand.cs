namespace Game.Core.World.InventorySystem.Commands
{
    public sealed class ExamineItemCommand : InventoryItemCommand
    {
        public ExamineItemCommand(
            ItemCommander itemCommander,
            InventoryItem inventoryItem
            ) : base( itemCommander, inventoryItem )
        {
        }

        public override void Execute() => _itemCommander.Examine( InventoryItem );
    }
}