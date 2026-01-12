using UnityEngine;
using Zenject;

namespace Game.Core.World.IndicatorManager
{
    [ CreateAssetMenu( fileName = "IndicatorManagerInstaller", menuName =  "Installers/IndicatorManagerInstaller" ) ]
    public sealed class IndicatorManagerInstaller : ScriptableObjectInstaller< IndicatorManagerInstaller >
    {
        [ SerializeField ] private InteractionIndicator _interactionIndicatorPrefab;
        
        public override void InstallBindings()
        {
            Container
                .BindFactory< InteractionIndicator, InteractionIndicatorFactory >()
                .FromMonoPoolableMemoryPool( ( x ) => x.WithInitialSize( 1 ) ).Lazy();

            Container.Bind< IndicatorManager >().AsSingle().Lazy();
        }
    }
}