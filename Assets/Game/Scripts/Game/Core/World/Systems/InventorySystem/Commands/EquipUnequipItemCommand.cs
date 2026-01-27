namespace Game.Core.World.InventorySystem.Commands
{
    public sealed class EquipUnequipItemCommand : InventoryItemCommand
    {
        public EquipUnequipItemCommand(
            ItemCommander itemCommander,
            ItemModel item
            ) : base( itemCommander, item )
        {
        }

        public override void Execute() => _itemCommander.EquipUnequip( _item );
    }
}