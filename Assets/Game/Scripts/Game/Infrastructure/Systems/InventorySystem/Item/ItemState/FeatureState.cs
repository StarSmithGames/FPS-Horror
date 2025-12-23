using System;

namespace Game.Systems.InventorySystem
{
    public abstract class FeatureState
    {
        public event Action OnChanged;

        public void NotifyChanged()
        {
            OnChanged?.Invoke();
        }
    }
}