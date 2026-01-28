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

        public override Vector3 GetInteractPointerPosition( Transform from )
        {
            Vector3 nearest = transform.position;
            float minSqrDist = float.MaxValue;

            foreach ( var point in InteractableSettings.Points )
            {
                Vector3 pos = point.GetPointerPosition( transform );
                float sqrDist = ( pos - from.position ).sqrMagnitude;

                if ( sqrDist < minSqrDist )
                {
                    minSqrDist = sqrDist;
                    nearest = pos;
                }
            }

            return nearest;
        }

        protected void Changed()
        {
            OnChanged?.Invoke();
        }
    }
}