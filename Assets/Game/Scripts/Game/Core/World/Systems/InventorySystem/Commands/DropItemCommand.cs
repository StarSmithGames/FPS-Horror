namespace Game.Core.World.InventorySystem.Commands
{
    public sealed class DropItemCommand : InventoryItemCommand
    {
        public DropItemCommand(
            ItemCommander itemCommander,
            ItemModel item
            ) : base( itemCommander, item )
        {
        }

        public override void Execute() => _itemCommander.Drop( _item );
    }
}