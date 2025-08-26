using System;

namespace Game.Core.Entity
{
    public abstract class OpenCloseDynamicObject : DynamicObject
    {
        public event Action OnChanged;
        
        public bool IsOpen
        {
            get => _isOpen;
            protected set
            {
                if ( _isOpen != value )
                {
                    _isOpen = value;
                    Changed();
                }
            }
        }
        protected bool _isOpen;
        
        public abstract void Open();
        public abstract void Close();
        
        public void Toggle()
        {
            if ( IsOpen )
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        protected void Changed()
        {
            OnChanged?.Invoke();
        }
    }
}