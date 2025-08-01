using UnityEngine;
using Zenject;

namespace Game.Core.Player
{
    public sealed class PlayerInstaller : MonoInstaller
    {
        [ SerializeField ] private PlayerConfig _config;
        [ SerializeField ] private PlayerObject _view;
        
        public override void InstallBindings()
        {
            Container.BindInstance( _config );
            Container.BindInstance( _view );
            Container.Bind< PlayerLookController >().AsSingle();
            Container.Bind< PlayerMovementController >().AsSingle();
            Container.Bind< PlayerJumpController >().AsSingle();
            Container.Bind< CameraFOVController >().AsSingle();

            Container.Bind< PlayerFacade >().AsSingle();
            Container.Bind< PlayerStates >().AsSingle();
            Container.Bind< PlayerBrain >().AsSingle();
            Container.Bind< PlayerController >().AsSingle().NonLazy();
        }
    }
}