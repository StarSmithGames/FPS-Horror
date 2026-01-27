using Game.Core.Entity;
using Game.Core.World.InventorySystem;

namespace Game.Core.Player
{
    public sealed class PlayerInventoryController
    {
        public Inventory Inventory { get; } = new();

        public void PickUpItem( ItemObject item )
        {
            Inventory.AddItem( item.Config );
            item.gameObject.SetActive( false );
        }
    }
}