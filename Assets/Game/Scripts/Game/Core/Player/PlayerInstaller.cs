using UnityEngine;
using Zenject;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Core.Player
{
    public sealed class PlayerInstaller : MonoInstaller
    {
        [ SerializeField ] private PlayerConfig _config;
        [ SerializeField ] private PlayerObject _view;
        [ SerializeField ] private PlayerAvatar _avatar;
        [ HideInInspector ]
        [ SerializeField ] private Vector3 _rotationOffset;
        
        public override void InstallBindings()
        {
            Container.BindInstance( _config );
            Container.BindInstance( _view );
            Container.BindInstance( _avatar );
            BindLocomotion();
            Container.Bind< CameraFOVController >().AsSingle().Lazy();
            Container.Bind< CameraVisionController >().AsSingle().Lazy();

            Container.Bind< InteractionActionController >().AsSingle().Lazy();
            Container.Bind< InteractionActionFactory >().AsSingle().Lazy();
            // Container.Bind< ContextMenuActionController >().AsSingle().Lazy();
            // Container.Bind< ContextMenuActionFactory >().AsSingle().Lazy();
            
            Container.Bind< PlayerHoveringController >().AsSingle().Lazy();
            Container.Bind< PlayerInteractionPointsController >().AsSingle().Lazy();
            
            Container.Bind< PlayerInputActionsController >().AsSingle().Lazy();
            Container.Bind< PlayerInventoryController >().AsSingle().Lazy();
            Container.Bind< PlayerInspectionController >().AsSingle().Lazy();
            
            Container.Bind< PlayerSoundController >().AsSingle().Lazy();
            
            Container.Bind< PlayerStates >().AsSingle().Lazy();
            Container.Bind< PlayerBrain >().AsSingle().Lazy();
            Container.Bind< PlayerController >().AsSingle().NonLazy();
        }

        private void BindLocomotion()
        {
            Container.Bind< PlayerLookController >().AsSingle().Lazy();
            Container.Bind< PlayerMoveController >().AsSingle().Lazy();
            Container.Bind< PlayerJumpController >().AsSingle().Lazy();
            Container.Bind< PlayerCrouchController >().AsSingle().Lazy();
        }

#if UNITY_EDITOR
        [ CustomEditor( typeof( PlayerInstaller ) ) ]
        public sealed class PlayerInstallerEditor : Editor
        {
            private PlayerInstaller _target;
            private Vector3 _lastRotation;
            
            private void OnEnable()
            {
                _target = (PlayerInstaller)target;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                EditorGUI.BeginChangeCheck();
                var rotation = EditorGUILayout.Vector3Field( "Rotation", _target._rotationOffset );

                if ( EditorGUI.EndChangeCheck() )
                {
                    Undo.RecordObject( _target, "Change Rotation Offset" );
                    _target._rotationOffset = rotation;
                    EditorUtility.SetDirty( _target );
                }

                if ( _target._view != null )
                {
                    if ( _target._view.CameraFPS != null )
                    {
                        _target._view.CameraFPS.transform.localRotation = Quaternion.Euler( rotation.x, rotation.y, rotation.z );
                    }

                    _target._view.transform.rotation = Quaternion.Euler( 0, rotation.y, 0 );
                }
            }
        }
#endif
    }
}