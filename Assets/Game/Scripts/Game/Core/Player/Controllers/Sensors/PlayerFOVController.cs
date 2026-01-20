using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerFOVController
    {
        private CancellationTokenSource _cancellationTokenSource;
        private float _targetFOV;
        private float _lerpSpeed;
        
        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        
        public PlayerFOVController(
            PlayerObject view,
            PlayerConfig config
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
        }

        public void Initialize()
        {
            _lerpSpeed = _config.CameraFOVSettings.FadeFOVAmount; 
            _targetFOV = _config.CameraFOVSettings.NormalFOV;
            _view.CameraFPS.fieldOfView = _targetFOV;
            
            _cancellationTokenSource = new();
            Tick( _cancellationTokenSource.Token ).Forget();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private async UniTask Tick( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                _view.CameraFPS.fieldOfView = Mathf.Lerp( _view.CameraFPS.fieldOfView, _targetFOV, _lerpSpeed * Time.deltaTime );
                
                await UniTask.Yield();
            }
        }

        public void SetFOV( float fov )
        {
            _targetFOV = fov;
        }

        public bool IsInFOV( Vector3 worldPoint )
        {
            Vector3 dirToTarget = ( worldPoint - _view.CameraFPS.transform.position ).normalized;

            float dot = Vector3.Dot( _view.CameraFPS.transform.forward, dirToTarget );
            float minDot = Mathf.Cos( _view.CameraFPS.fieldOfView * 0.5f * Mathf.Deg2Rad );

            return dot >= minDot;
        }
    }
}