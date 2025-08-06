using Game.Core.UI;
using PuzzlescapeGames.IoC;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class GameInstaller : MonoInstaller
    {
        [ SerializeField ] private GameplayConfig _gameplayConfig;
        [ SerializeField ] private UIRootGame _uiRootGame;
        
        public override void InstallBindings()
        {
            Container.BindInstance( _gameplayConfig );
            Container.BindInstance( _uiRootGame );

            Container.BindInterfacesAndSelfTo< GameplayPipeline >().AsSingle().NonLazy();
            
            DiManager.CurrentContainer = Container;
        }
    }
}