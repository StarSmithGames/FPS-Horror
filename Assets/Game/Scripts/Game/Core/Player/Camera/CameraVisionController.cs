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
        
        public CameraVisionController(
            PlayerObject view,
            PlayerConfig config
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
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
                RaycastHit hit;
                Ray ray = new Ray( _head.position, _head.forward );
                if ( Physics.Raycast( ray, out hit, _config.CameraVisionSettings.MaxRayDistance, _config.CameraVisionSettings.InteractLayers ) )
                {
                    _lastHitPoint = hit.point;

                    Collider[] collidersIntersects = Physics.OverlapSphere( _lastHitPoint, _config.CameraVisionSettings.SphereRadius, _config.CameraVisionSettings.InteractLayers );
                    // for ( int i = 0; i < collidersIntersects.Length; i++ )
                    // {
                    //     if ( collidersIntersects[ i ] != null )
                    //     {
                    //         Debug.DrawLine( _lastHitPoint, collidersIntersects[ i ].transform.position );
                    //     }
                    // }
                    
                    //каст для интерактивных объектов
                    if ( Physics.Raycast( ray, out hit, _config.CameraVisionSettings.RayDistance, _config.CameraVisionSettings.InteractLayers ) )
                    {
                        CurrentObservable = hit.transform.GetComponentInParent< IObservable >();
                    }
                    else
                    {
                        CurrentObservable = null;
                    }
                    OnObservablesChanged?.Invoke( collidersIntersects.Length > 0 );
                }
                else
                {
                    CurrentObservable = null;
                    OnObservablesChanged?.Invoke( false );
                }

                // Debug.DrawLine( _head.position, _head.position + ( _head.forward * _config.CameraVisionSettings.RayDistance ), Color.blue );

                await UniTask.Yield();
            }
        }
    }
}