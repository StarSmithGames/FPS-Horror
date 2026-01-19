using UnityEngine;
using Zenject;

namespace Game.Core.World.PointerSystem
{
    [ CreateAssetMenu( fileName = "PointerSystemInstaller", menuName =  "Installers/PointerSystemInstaller" ) ]
    public sealed class PointerSystemInstaller : ScriptableObjectInstaller< PointerSystemInstaller >
    {
        [ SerializeField ] private InteractionPointer _interactionPointerPrefab;
        
        public override void InstallBindings()
        {
            Container.BindFactory< InteractionPointer, InteractionPointerFactory >()
                .FromMonoPoolableMemoryPool( ( x ) => x.WithInitialSize( 1 )
                    .FromComponentInNewPrefab( _interactionPointerPrefab ) ).Lazy();
        }
    }
}