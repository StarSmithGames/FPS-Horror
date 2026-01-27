using Game.Core.World.WorldManager;
using System;

namespace Game.Core
{
    public sealed class PlayerControllersService
    {
        private readonly WorldManager _worldManager;

        public PlayerControllersService( WorldManager worldManager )
        {
            _worldManager = worldManager ?? throw new ArgumentNullException( nameof(worldManager) );
        }

        public T GetAs< T >() => _worldManager.Player.Controller.ServiceLocator.GetAs< T >();
    }
}