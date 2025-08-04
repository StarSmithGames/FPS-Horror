using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Core.Entity
{
    public sealed class OpenableObject : DynamicObject
    {
        [ SerializeField ] private Transform _door;
        [ SerializeField ] private Transform _handle;
        [ SerializeField ] private OpenCloseSettings _openCloseSettings;

        public bool IsOpen { get; private set; }
        
        private Quaternion _handleRestRot;
        private Quaternion _handleTurnedRot;

        protected override void Awake()
        {
            base.Awake();
            
            if ( _openCloseSettings.IsLocked )
            {
                IsOpen = false;
                _door.localRotation = Quaternion.identity;
            }
            else
            {
                var angle = Mathf.Clamp( Mathf.Abs( _door.localRotation.eulerAngles.y - Quaternion.identity.eulerAngles.y ), 0, _openCloseSettings.DoorOpenAngle );
                IsOpen = angle > _openCloseSettings.DoorOpenAngle / 2f && angle <= _openCloseSettings.DoorOpenAngle;
            }

            _handleRestRot = _handle.localRotation;
            _handleTurnedRot = Quaternion.Euler( 0, 0, -_openCloseSettings.HandleAngle ) * _handleRestRot;
        }

        public void Open()
        {
            OpenDoorAsync().Forget();
        }

        public void Close()
        {
            CloseDoorAsync().Forget();
        }
        
        public async UniTask Toggle()
        {
            if ( IsOpen )
                await CloseDoorAsync();
            else
                await OpenDoorAsync();
        }

        private async UniTask OpenDoorAsync()
        {
            if ( IsOpen ) return;
            IsOpen = true;

            if ( _door.localRotation == Quaternion.identity )
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
            await LerpRotation( _handle, _handleRestRot, _handleTurnedRot, _openCloseSettings.HandleDuration / 2f );
            await LerpRotation( _handle, _handleTurnedRot, _handleRestRot, _openCloseSettings.HandleDuration / 2f );
        }

        private async UniTask AnimateDoorAsync( bool opening )
        {
            Quaternion from = opening ? _door.localRotation : Quaternion.Euler( 0, _openCloseSettings.DoorOpenAngle, 0 );
            Quaternion to = opening ? Quaternion.Euler( 0, _openCloseSettings.DoorOpenAngle, 0 ) :  Quaternion.identity;

            float angleDelta = Quaternion.Angle( from, to );
            float fraction = Mathf.Clamp01( angleDelta / _openCloseSettings.DoorOpenAngle );

            await LerpRotation( _door, from, to, _openCloseSettings.DoorDuration * fraction );
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
    }
}