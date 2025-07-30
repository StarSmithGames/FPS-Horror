using UnityEngine;
using Zenject;

namespace Game.Core
{
    public sealed class GameInstaller : MonoInstaller
    {
        [ SerializeField ] private GameplayConfig _gameplayConfig;
        
        public override void InstallBindings()
        {
            Container.BindInstance( _gameplayConfig );

            Container.BindInterfacesAndSelfTo< GameplayPipeline >().AsSingle().NonLazy();
        }
    }
}