using System;

namespace Game.Core.World.InventorySystem
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