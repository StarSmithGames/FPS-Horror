using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player.Animations
{
    [ Serializable ]
    public sealed class IdleAnimator
    {
        [ Serializable ]
        public class Settings
        {
            [ Header( "Position amplitude (local)" ) ]
            public float posX;
            public float posY;
            public float posZ;

            [ Header( "Rotation amplitude (degrees, local euler)" ) ]
            public float rotPitch; // X
            public float rotYaw; // Y
            public float rotRoll; // Z

            [ Header( "Step duration (seconds)" ) ]
            public float minDur;
            public float maxDur;

            [ Header( "Extra random pause after each step (seconds)" ) ]
            public float minPause;
            public float maxPause;

            [ Header( "Use SetUpdate(true) to ignore Time.timeScale" ) ]
            public bool unscaledTime;
        }

        [ SerializeField ] private Settings _settings;

        private CancellationTokenSource _cts;
        private Tween _tween;

        private float _strength = 1f;

        private Vector3 _baseLocalPos;
        private Quaternion _baseLocalRot;

        private Transform _target;
        
        public void Initialize( Transform target )
        {
            _target = target;

            _baseLocalPos = target.localPosition;
            _baseLocalRot = target.localRotation;
        }

        public void SetStrength( float strength01 )
        {
            _strength = Mathf.Clamp01( strength01 );
        }

        public void Start( bool resetToBase = false )
        {
            Stop( resetToBase: false );

            if ( resetToBase )
                ResetToBase();

            _cts = new CancellationTokenSource();
            Loop( _cts.Token ).Forget();
        }

        public void Stop( bool resetToBase = true )
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            _tween?.Kill();
            _tween = null;

            if ( resetToBase )
                ResetToBase();
        }

        public void ResetToBase()
        {
            if ( _target == null ) return;
            _target.localPosition = _baseLocalPos;
            _target.localRotation = _baseLocalRot;
        }

        public void RebindBasePose()
        {
            // Если ты хочешь считать текущую позу "базовой" (например после анимации появления)
            _baseLocalPos = _target.localPosition;
            _baseLocalRot = _target.localRotation;
        }

        public void Dispose()
        {
            Stop( resetToBase: false );
        }

        private async UniTaskVoid Loop( CancellationToken ct )
        {
            while ( !ct.IsCancellationRequested )
            {
                float dur = UnityEngine.Random.Range( _settings.minDur, _settings.maxDur );

                Vector3 offsetPos = new Vector3( UnityEngine.Random.Range( -_settings.posX, _settings.posX ), UnityEngine.Random.Range( -_settings.posY, _settings.posY ), UnityEngine.Random.Range( -_settings.posZ, _settings.posZ ) ) * _strength;

                Vector3 offsetRot = new Vector3( UnityEngine.Random.Range( -_settings.rotPitch, _settings.rotPitch ), UnityEngine.Random.Range( -_settings.rotYaw, _settings.rotYaw ), UnityEngine.Random.Range( -_settings.rotRoll, _settings.rotRoll ) ) * _strength;

                _tween?.Kill();

                Vector3 targetPos = _baseLocalPos + offsetPos;

                // Базовую ротацию + маленькое смещение (через кватернион)
                Quaternion targetRot = _baseLocalRot * Quaternion.Euler( offsetRot );

                _tween = DOTween.Sequence().Append( _target.DOLocalMove( targetPos, dur ).SetEase( Ease.InOutSine ) ).Join( _target.DOLocalRotateQuaternion( targetRot, dur ).SetEase( Ease.InOutSine ) );

                if ( _settings.unscaledTime )
                    _tween.SetUpdate( true );

                float pause = UnityEngine.Random.Range( _settings.minPause, _settings.maxPause );

                await UniTask.Delay( TimeSpan.FromSeconds( dur + pause ), cancellationToken: ct );
            }
        }
    }
}