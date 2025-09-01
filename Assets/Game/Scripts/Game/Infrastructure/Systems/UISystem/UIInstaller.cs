using PuzzlescapeGames.Services.UIBlockingService;
using PuzzlescapeGames.Services.UITransitionService;
using UnityEngine;
using Zenject;

namespace Game.UISystem
{
    [ CreateAssetMenu( fileName = "UIInstaller", menuName = "Installers/UIInstaller") ] 
    public sealed class UIInstaller : ScriptableObjectInstaller< UIInstaller >
    {
        [ SerializeField ] private UISettings _uiSettings;
        [ SerializeField ] private UIBlockerSettings _uiBlockerSettings;
        [ SerializeField ] private UITransitionSettings _uiTransitionSettings;

        public override void InstallBindings()
        {
            Container.BindInstance( _uiSettings );

            Container.BindInstance( _uiBlockerSettings );
            Container.Bind< UIBlockingService >().AsSingle();
            
            Container.BindInstance( _uiTransitionSettings );
            Container.Bind< UITransitionService >().AsSingle();
        }
    }
}