using Game.Core.Entity;
using System;
using UnityEngine;
using Zenject;

namespace Game.Core.World.InventorySystem
{
    public sealed class ItemFactory
    {
        private readonly DiContainer _diContainer;
        
        public ItemFactory( DiContainer diContainer )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
        }
        
        public ItemController Create( ItemObject prefab, Transform parent = null )
        {
            var view = _diContainer.InstantiatePrefab( prefab, parent ).GetComponent< ItemObject >();
            var controller = (ItemController)_diContainer.Instantiate( view.ControllerType, new[] { view } );
            view.SetController( controller );
            return controller;
        }
    }
}