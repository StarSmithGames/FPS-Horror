using Game.Core.World.InventorySystem;

namespace Game.Core.World.EquipmentSystem
{
    public sealed class Equipment
    {
        public InventoryItem EquippedItem { get; set; }
        
        public InventoryItem TopShortcut { get; set; }
        public InventoryItem BottomShortcut { get; set; }
        public InventoryItem LeftShortcut { get; set; }
        public InventoryItem RightShortcut { get; set; }
    }
}