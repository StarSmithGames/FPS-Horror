using Game.Core.World.InventorySystem;

namespace Game.Core.World.EquipmentSystem
{
    public sealed class Equipment
    {
        public ItemModel EquippedItem { get; set; }
        
        public ItemModel TopShortcut { get; set; }
        public ItemModel BottomShortcut { get; set; }
        public ItemModel LeftShortcut { get; set; }
        public ItemModel RightShortcut { get; set; }
    }
}