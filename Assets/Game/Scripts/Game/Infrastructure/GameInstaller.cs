using Game.Core.UI;
using Game.SceneSystem;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class GameInstaller : MonoInstaller
    {
        [ SerializeField ] private GameConfig _gameConfig;
        [ SerializeField ] private UIRootGame _uiRootGamePrefab;
        
        public override void InstallBindings()
        {
            Container.BindInstance( _gameConfig );
            
            SceneSystemInstaller.Install( Container );
            
            Container.Bind< UIRootGame >().FromComponentInNewPrefab( _uiRootGamePrefab ).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo< GameBoostrap >().AsSingle().NonLazy();
        }
    }
}