using Game.Core.World.EntityManager;
using Game.Core.UI;
using Game.Managers.GameManager;
using Game.Managers.PauseManager;
using Game.SceneSystem;
using Game.Systems.StorageSystem;
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
            
            StorageSystemInstaller.Install( Container );
            SceneSystemInstaller.Install( Container );
            PauseManagerInstaller.Install( Container );
            
            Container.Bind< UIRootGame >().FromComponentInNewPrefab( _uiRootGamePrefab ).AsSingle().NonLazy();
            Container.Bind< GameManager >().AsSingle().Lazy();
            Container.Bind< EntityManager >().AsSingle().Lazy();
            Container.BindInterfacesAndSelfTo< GameBoostrap >().AsSingle().NonLazy();
        }
    }
}