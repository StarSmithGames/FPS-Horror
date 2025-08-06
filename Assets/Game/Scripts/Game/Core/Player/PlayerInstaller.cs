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
            BindLocomotion();
            Container.Bind< CameraFOVController >().AsSingle();
            Container.Bind< CameraVisionController >().AsSingle();

            Container.Bind< PickableHandler >().AsSingle();
            Container.Bind< InspectableHandler >().AsSingle();
            Container.Bind< OpenableHandler >().AsSingle();
            Container.Bind< PullableHandler >().AsSingle();
            Container.Bind< PlayerTargetingController >().AsSingle();
            Container.Bind< PlayerSoundController >().AsSingle();
            
            Container.Bind< PlayerFacade >().AsSingle();
            Container.Bind< PlayerStates >().AsSingle();
            Container.Bind< PlayerBrain >().AsSingle();
            Container.Bind< PlayerController >().AsSingle().NonLazy();
        }

        private void BindLocomotion()
        {
            Container.Bind< PlayerLookController >().AsSingle();
            Container.Bind< PlayerMovementController >().AsSingle();
            Container.Bind< PlayerJumpController >().AsSingle();
            Container.Bind< PlayerCrouchController >().AsSingle();
        }
    }
}