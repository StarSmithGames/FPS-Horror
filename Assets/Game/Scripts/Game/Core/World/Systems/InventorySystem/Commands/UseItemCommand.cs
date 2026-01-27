namespace Game.Core.World.InventorySystem.Commands
{
    public sealed class UseItemCommand : InventoryItemCommand
    {
        public UseItemCommand(
            ItemCommander itemCommander,
            ItemModel item
            ) : base( itemCommander, item )
        {
            
        }

        public override void Execute() => _itemCommander.Use( _item );
    }
}