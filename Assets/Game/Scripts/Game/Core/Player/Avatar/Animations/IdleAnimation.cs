using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player.Animations
{
    [ System.Serializable ]
    public sealed class IdleAnimation
    {
        [ Header( "Idle" ) ]
        [ SerializeField, Range( 0f, 0.05f ) ] private float _posAmplitudeY = 0.010f; // дыхание по Y
        [ SerializeField, Range( 0f, 0.05f ) ] private float _posAmplitudeX = 0.004f; // небольшой боковой ход
        [ SerializeField, Range( 0f, 0.05f ) ] private float _posAmplitudeZ = 0.002f; // очень слабая глубина (аккуратно!)
        [ SerializeField, Range( 0.1f, 5f ) ] private float _breathSpeed = 0.85f; // скорость дыхания (медленно)

        [ Header( "Idle Rotation" ) ]
        [ SerializeField, Range( 0f, 5f ) ] private float _rotX = 0.8f;
        [ SerializeField, Range( 0f, 5f ) ] private float _rotY = 0.6f;
        [ SerializeField, Range( 0f, 5f ) ] private float _rotZ = 1.2f;
        [ SerializeField, Range( 0.1f, 5f ) ] private float _rotSpeed = 0.75f;

        [ Header( "Idle Drift" ) ]
        [ SerializeField, Range( 0f, 0.03f ) ] private float _driftPos = 0.0035f; // медленный дрейф позиции
        [ SerializeField, Range( 0f, 2f ) ] private float _driftRot = 0.35f; // медленный дрейф углов
        [ SerializeField, Range( 0.05f, 1f ) ] private float _driftSpeed = 0.12f; // очень медленно

        [ Header( "Smoothing" ) ]
        [ SerializeField, Range( 1f, 30f ) ] private float _posSmooth = 14f;
        [ SerializeField, Range( 1f, 30f ) ] private float _rotSmooth = 14f;
        
        private bool _idleEnabled;
        
        private CancellationTokenSource _cancellationTokenSource;
        private Vector3 _smoothedPosOffset;
        private Quaternion _smoothedRotOffset = Quaternion.identity;
        private float _t;

        private RightHandModel _model;
        
        public void Initialize( RightHandModel model )
        {
            _model = model;
        }

        public void Reset()
        {
            _t = 0f;
            _smoothedPosOffset = Vector3.zero;
            _smoothedRotOffset = Quaternion.identity;
        }

        public void PlayIdle()
        {
            if ( _idleEnabled ) return;

            _idleEnabled = true;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();

            IdleLoop( _cancellationTokenSource.Token ).Forget();
        }

        public void StopIdle()
        {
            _idleEnabled = false;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private async UniTaskVoid IdleLoop( CancellationToken token )
        {
            // чтобы не было скачка при старте
            _t = 0f;
            _smoothedPosOffset = Vector3.zero;
            _smoothedRotOffset = Quaternion.identity;

            while ( !token.IsCancellationRequested )
            {
                float dt = Time.deltaTime;
                _t += dt;

                // --- BREATH (две частоты, чтобы не было "робота") ---
                float b1 = Mathf.Sin( _t * _breathSpeed );
                float b2 = Mathf.Sin( _t * ( _breathSpeed * 0.5f ) + 1.7f ); // фаза + другая частота

                // Позиция (очень мягко)
                Vector3 targetPosOffset = new Vector3( ( b1 * 0.65f + b2 * 0.35f ) * _posAmplitudeX, ( b1 * 0.75f + b2 * 0.25f ) * _posAmplitudeY, ( b1 * 0.60f + b2 * 0.40f ) * _posAmplitudeZ );

                // --- DRIFT (очень медленный Perlin, без "дребезга") ---
                float pnX = Mathf.PerlinNoise( _t * _driftSpeed, 11.3f ) - 0.5f;
                float pnY = Mathf.PerlinNoise( 22.7f, _t * _driftSpeed ) - 0.5f;

                targetPosOffset += new Vector3( pnX, pnY, 0f ) * _driftPos;

                // Поворот: дыхание + дрейф
                float r1 = Mathf.Sin( _t * _rotSpeed );
                float r2 = Mathf.Sin( _t * ( _rotSpeed * 0.6f ) + 2.1f );

                Vector3 targetEuler = new Vector3( ( r1 * 0.70f + r2 * 0.30f ) * _rotX, ( r1 * 0.60f + r2 * 0.40f ) * _rotY, ( r1 * 0.65f + r2 * 0.35f ) * _rotZ );

                // дрейф угла
                float pnR = ( Mathf.PerlinNoise( _t * _driftSpeed, 99.1f ) - 0.5f ) * 2f;
                targetEuler += new Vector3( pnR, -pnR * 0.6f, pnR * 0.8f ) * _driftRot;

                Quaternion targetRotOffset = Quaternion.Euler( targetEuler );

                // --- SMOOTH ---
                float posLerp = 1f - Mathf.Exp( -_posSmooth * dt );
                float rotLerp = 1f - Mathf.Exp( -_rotSmooth * dt );

                _smoothedPosOffset = Vector3.Lerp( _smoothedPosOffset, targetPosOffset, posLerp );
                _smoothedRotOffset = Quaternion.Slerp( _smoothedRotOffset, targetRotOffset, rotLerp );

                _model.Root.localPosition = _model.BasePos + _smoothedPosOffset;
                _model.Root.localRotation = _model.BaseRot * _smoothedRotOffset;

                await UniTask.Yield( PlayerLoopTiming.Update, token );
            }
        }
    }
}