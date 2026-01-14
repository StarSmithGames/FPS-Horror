using System;

namespace Game.Systems.InventorySystem
{
    public sealed class ItemModel
    {
        public event Action< ItemModel > OnChanged;

        public string UID => Config.UID;

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

        public ItemModel( ItemConfig config, int quantity = 1 )
        {
            Config = config;
            State = new ItemState();

            _quantity = quantity;
            
            State.OnChanged += NotifyChanged;
        }

        public void NotifyChanged()
        {
            OnChanged?.Invoke( this );
        }
    }
}