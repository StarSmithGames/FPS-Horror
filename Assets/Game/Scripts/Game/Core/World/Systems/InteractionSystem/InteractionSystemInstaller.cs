using UnityEngine;
using Zenject;

namespace Game.Core.World.Systems.InteractionSystem
{
    [ CreateAssetMenu( fileName = "InteractionSystemInstaller", menuName = "Installers/InteractionSystemInstaller" ) ]
    public sealed class InteractionSystemInstaller : ScriptableObjectInstaller< InteractionSystemInstaller >
    {
        [ SerializeField ] private InteractionsSettings _settings;

        public override void InstallBindings()
        {
            Container.BindInstance( _settings );
        }
    }
}