using Game.Core.Entity;
using System;

namespace Game.Core.World.InventorySystem
{
    public sealed class InventoryItem
    {
        public event Action< InventoryItem > OnChanged;

        public string UID => Config.UID;

        public ItemObject View { get; private set; }
        public ItemConfig Config { get; }
        public ItemState State { get; }
        
        public int Quantity
        {
            get => _quantity;
            set
            {
                if ( Quantity != value )
                {
                    _quantity = value;
                    NotifyChanged();
                }
            }
        }
        private int _quantity; //сколько таких предметов лежит в этом слоте

        public InventoryItem( ItemConfig config, int quantity = 1 )
        {
            Config = config;
            State = new ItemState();

            _quantity = quantity;
            
            State.OnChanged += NotifyChanged;
        }

        public void SetView( ItemObject view )
        {
            View = view;
        }
        
        public void NotifyChanged()
        {
            OnChanged?.Invoke( this );
        }
    }
}