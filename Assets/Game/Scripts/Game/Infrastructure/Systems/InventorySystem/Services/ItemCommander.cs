using UnityEngine;

namespace Game.Systems.InventorySystem
{
    public sealed class ItemCommander
    {
        public ItemCommander()
        {
            
        }

        public void Use( ItemModel item )
        {
            Debug.LogError( "Use" );
        }

        public void EquipUnequip( ItemModel item )
        {
            Debug.LogError( "EquipUnequip" );
        }

        public void Examine( ItemModel item )
        {
            Debug.LogError( "Examine" );
        }
        
        public void Drop( ItemModel item )
        {
            Debug.LogError( "Drop" );
        }
    }
}