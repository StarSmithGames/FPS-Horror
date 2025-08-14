using Cysharp.Threading.Tasks;
using Game.Managers.AudioManager;
using PuzzlescapeGames.Extensions;
using System;
using UnityEngine;
using Zenject;
using AudioSource = PuzzlescapeGames.Services.AudioService.AudioSource;
using Random = UnityEngine.Random;

namespace Game.Core.Entity
{
    public sealed class OpenableObject : OpenCloseDynamicObject
    {
        [ SerializeField ] private Transform _door;
        [ SerializeField ] private Transform _handle;
        [ SerializeField ] private OpenableSettings _settings;

        private Quaternion _handleRestRot;
        private Quaternion _handleTurnedRot;

        private AudioManager _audioManager;
        
        [ Inject ]
        private void Construct( AudioManager audioManager )
        {
            _audioManager = audioManager ?? throw new ArgumentNullException( nameof(audioManager) );
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            if ( _settings.IsLocked )
            {
                IsOpen = false;
                _door.localRotation = Quaternion.identity;
            }
            else
            {
                var angle = Mathf.Clamp( Mathf.Abs( GetAngle( _settings.DoorAxis, _door.localRotation.eulerAngles - Quaternion.identity.eulerAngles ) ), 0, _settings.DoorOpenAngle );
                IsOpen = angle > _settings.DoorOpenAngle / 2f && angle <= _settings.DoorOpenAngle;
            }

            if ( _handle != null )
            {
                _handleRestRot = _handle.localRotation;
                _handleTurnedRot = GetAngle( _settings.HandleAxis, -_settings.HandleAngle ) * _handleRestRot;
            }
        }

        public override void Open()
        {
            OpenDoorAsync().Forget();
        }

        public override void Close()
        {
            CloseDoorAsync().Forget();
        }

        private async UniTask OpenDoorAsync()
        {
            if ( IsOpen ) return;
            IsOpen = true;

            AudioSource sound = null;
            if ( _settings.OpenSounds.Count > 0 )
            {
                sound = _audioManager.PlaySound( _settings.OpenSounds.RandomItem(), volume: Random.Range( 0.5f, 1f ), pitch: Random.Range( 0.7f, 1.3f ) );
            }
            if ( _door.localRotation == Quaternion.identity && _handle != null )
            {
                await AnimateHandleAsync();
            }
            await AnimateDoorAsync( true );
        }

        private async UniTask CloseDoorAsync()
        {
            if ( !IsOpen ) return;
            IsOpen = false;
            
            await AnimateDoorAsync( false );
        }

        private async UniTask AnimateHandleAsync()
        {
            await LerpRotation( _handle, _handleRestRot, _handleTurnedRot, _settings.HandleDuration / 2f );
            await LerpRotation( _handle, _handleTurnedRot, _handleRestRot, _settings.HandleDuration / 2f );
        }

        private async UniTask AnimateDoorAsync( bool opening )
        {
            Quaternion from = opening ? _door.localRotation : GetAngle( _settings.DoorAxis, _settings.DoorOpenAngle );
            Quaternion to = opening ? GetAngle( _settings.DoorAxis, _settings.DoorOpenAngle ) :  Quaternion.identity;

            float angleDelta = Quaternion.Angle( from, to );
            float fraction = Mathf.Clamp01( angleDelta / _settings.DoorOpenAngle );

            await LerpRotation( _door, from, to, _settings.DoorDuration * fraction );
        }

        private async UniTask LerpRotation( Transform target, Quaternion from, Quaternion to, float duration )
        {
            float elapsed = 0f;
            while ( elapsed < duration )
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01( elapsed / duration );
                target.localRotation = Quaternion.Slerp( from, to, t );
                
                await UniTask.Yield();
            }

            target.localRotation = to;
        }
        
        private float GetAngle( Axis axis, Vector3 angle ) => axis switch
        {
            Axis.X => angle.x,
            Axis.Y => angle.y,
            Axis.Z => angle.z,
            _ => angle.y
        };
        
        private Quaternion GetAngle( Axis axis, float angle ) => axis switch
        {
            Axis.X => Quaternion.Euler( angle, 0, 0 ),
            Axis.Y => Quaternion.Euler( 0, angle, 0 ),
            Axis.Z => Quaternion.Euler( 0, 0, angle ),
            _ => Quaternion.Euler( 0, angle, 0 )
        };
    }
}