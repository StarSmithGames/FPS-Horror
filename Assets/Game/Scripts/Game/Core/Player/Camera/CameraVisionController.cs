using Cysharp.Threading.Tasks;
using Game.Core.UI;
using Game.Core.World.InteractionSystem;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class CameraVisionController
    {
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
                }
                else
                {
                    _currentObservable?.Observe();
                }
            }
        }
        private IObservable _currentObservable;

        private Transform _head;
        private GameScreenViewModel _gameScreenViewModel;
        private CancellationTokenSource _cancellationTokenSource;

        private Vector3 _lastHitPoint;
        
        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        private readonly UIRootGame _uiRootGame;
        
        public CameraVisionController(
            PlayerObject view,
            PlayerConfig config,
            UIRootGame uiRootGame
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
        }
        
        public void Initialize()
        {
            _head = _view.FirstPersonCamera.transform;
            _gameScreenViewModel = _uiRootGame.ScreenAggregator.GetAs< GameScreenViewModel >();
            
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
                    for ( int i = 0; i < collidersIntersects.Length; i++ )
                    {
                        if ( collidersIntersects[ i ] != null )
                        {
                            Debug.DrawLine( _lastHitPoint, collidersIntersects[ i ].transform.position );
                        }
                    }
                    
                    _gameScreenViewModel.ModelView.TargetPoint.EnableTargetPoint( collidersIntersects.Length > 0 );

                    //каст для интерактивных объектов
                    if ( Physics.Raycast( ray, out hit, _config.CameraVisionSettings.RayDistance, _config.CameraVisionSettings.InteractLayers ) )
                    {
                        CurrentObservable = hit.transform.GetComponentInParent< IObservable >();
                    }
                    else
                    {
                        CurrentObservable = null;
                    }
                }
                else
                {
                    CurrentObservable = null;
                    _gameScreenViewModel.ModelView.TargetPoint.EnableTargetPoint( false );
                }
                _gameScreenViewModel.SetObservable( CurrentObservable );

                Debug.DrawLine( _head.position, _head.position + ( _head.forward * _config.CameraVisionSettings.RayDistance ), Color.blue );

                await UniTask.Yield();
            }
        }
    }
}