namespace Game.Systems.InventorySystem.Commands
{
    public sealed class ExamineItemCommand : InventoryItemCommand
    {
        public ExamineItemCommand(
            ItemCommander itemCommander,
            ItemModel item
            ) : base( itemCommander, item )
        {
        }

        public override void Execute() => _itemCommander.Examine( _item );
    }
}