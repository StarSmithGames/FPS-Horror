using System;

namespace Game.Systems.InventorySystem
{
    public sealed class ItemModel
    {
        public event Action< ItemModel > OnChanged;
        
        public string UID;
        public ItemConfig Config;

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
        
        public ItemState State { get; }

        public ItemModel( int quantity = 1 )
        {
            _quantity = quantity;
            State = new ItemState();
            State.OnChanged += NotifyChanged;
        }

        public void NotifyChanged()
        {
            OnChanged?.Invoke( this );
        }
    }
}