using Game.Core.Entity;
using System;
using UnityEngine;
using Zenject;

namespace Game.Systems.InventorySystem
{
    public sealed class ItemFactory
    {
        private readonly DiContainer _diContainer;
        
        public ItemFactory( DiContainer diContainer )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
        }
        
        public T Create< T >( ItemObject prefab, Transform parent = null )
            where T : ItemController
        {
            var view = _diContainer.InstantiatePrefab( prefab, parent ).GetComponent< ItemObject >();
            var controller = _diContainer.Instantiate< T >( new[] { view } );
            view.SetController( controller );
            return controller;
        }
    }
}