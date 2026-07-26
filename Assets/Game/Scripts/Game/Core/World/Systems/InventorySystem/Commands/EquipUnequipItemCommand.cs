namespace Game.Core.World.InventorySystem.Commands
{
    public sealed class EquipUnequipItemCommand : InventoryItemCommand
    {
        public EquipUnequipItemCommand(
            ItemCommander itemCommander,
            InventoryItem inventoryItem
            ) : base( itemCommander, inventoryItem )
        {
        }

        public override void Execute() => _itemCommander.EquipUnequip( InventoryItem );
    }
}