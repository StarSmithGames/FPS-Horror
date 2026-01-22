using System;
using UnityEngine;

namespace Game.Core.Entity
{
    public abstract class OpenCloseObject : InteractableObject
    {
        public event Action OnChanged;
        
        [ field: Space ]
        [ field: SerializeField ] public string NameId { get; private set; }
        
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