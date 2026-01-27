using System;
using System.Collections.Generic;

namespace Game.Core.World.InventorySystem
{
    public sealed class ItemState
    {
        public event Action OnChanged;
        
        private readonly Dictionary< string, FeatureState > _features = new();

        public T GetOrAdd< T >( string key )
            where T : FeatureState, new()
        {
            if ( !_features.TryGetValue( key, out var result ) )
            {
                result = new T();
                result.OnChanged += NotifyChanged;
                _features[ key ] = result;
                OnChanged?.Invoke();
            }

            return (T)result;
        }

        public bool TryGet< T >( string key, out T feature )
            where T : FeatureState
        {
            if ( _features.TryGetValue( key, out var result ) && result is T t )
            {
                feature = t;
                return true;
            }

            feature = null;
            return false;
        }
        
        public void NotifyChanged()
        {
            OnChanged?.Invoke();
        }
    }
}