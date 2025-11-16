using Cysharp.Threading.Tasks;
using Game.Core.World.InteractionSystem;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class CameraVisionController
    {
        public event Action< IObservable > OnCurrentObservableChanged;
        public event Action< bool > OnObservablesChanged;
   
        public IObservable CurrentObservable
        {
            get => _currentObservable;
            set
            {
                if ( _currentObservable != value )
                {
                    _currentObservable?.EndObserve();
                    _currentObservable = value;
                    _currentObservable?.StartObserve();
                    
                    OnCurrentObservableChanged?.Invoke( _currentObservable );
                }
                else
                {
                    _currentObservable?.Observe();
                }
            }
        }
        private IObservable _currentObservable;

        private Transform _head;
        private CancellationTokenSource _cancellationTokenSource;
        private Vector3 _lastHitPoint;
        
        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        private readonly PlayerStates _states;
        
        public CameraVisionController(
            PlayerObject view,
            PlayerConfig config,
            PlayerStates states
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );
        }
        
        public void Initialize()
        {
            _head = _view.CameraFPS.transform;
            
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
                if ( _states.IsBlocked )
                {
                    CurrentObservable = null;
                    OnObservablesChanged?.Invoke( false );
                }
                else
                {
                    Vision();
                }

                await UniTask.Yield();
            }
        }

        private void Vision()
        {
            RaycastHit hit;
            Ray ray = new Ray( _head.position, _head.forward );

            // Каст для всего окружения (то, что может блокировать обзор: стены, двери, и т.п.)
            if ( Physics.Raycast( ray, out hit, _config.CameraVisionSettings.MaxRayDistance, _config.CameraVisionSettings.DefaultLayers ) )
            {
                RaycastHit interactHit;

                // Каст для интерактивных объектов, НО НЕ ДАЛЬШЕ ПРЕПЯТСТВИЯ
                if ( Physics.Raycast( ray, out interactHit, Mathf.Min( _config.CameraVisionSettings.RayDistance, hit.distance ), _config.CameraVisionSettings.InteractLayers ) )
                {
                    CurrentObservable = interactHit.transform.GetComponentInParent< IObservable >();
                }
                else
                {
                    CurrentObservable = null;
                }

                _lastHitPoint = hit.point;

                Collider[] collidersIntersects = Physics.OverlapSphere( _lastHitPoint, _config.CameraVisionSettings.SphereRadius, _config.CameraVisionSettings.InteractLayers );
                OnObservablesChanged?.Invoke( CurrentObservable != null || collidersIntersects.Length > 0 );
            }
            else
            {
                CurrentObservable = null;
                OnObservablesChanged?.Invoke( false );
            }

            // Debug.DrawLine(_head.position, _head.position + (_head.forward * _config.CameraVisionSettings.RayDistance), Color.blue);
        }
    }
}