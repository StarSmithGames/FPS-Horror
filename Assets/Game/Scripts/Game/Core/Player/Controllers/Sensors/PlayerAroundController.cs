using Cysharp.Threading.Tasks;
using Game.Core.Entity;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerAroundController
    {
        public event Action< ObservableObject > OnCurrentObservableChanged;
   
        public ObservableObject CurrentObservable
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
        private ObservableObject _currentObservable;
        
        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        private readonly PlayerStates _states;
        private readonly PlayerPointsController _playerPointsController;
        private readonly PlayerFOVController _playerFOVController;
        
        public PlayerAroundController(
            PlayerObject view,
            PlayerConfig config,
            PlayerStates states,
            PlayerPointsController playerPointsController,
            PlayerFOVController playerFOVController
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _playerPointsController = playerPointsController ?? throw new ArgumentNullException( nameof(playerPointsController) );
            _playerFOVController = playerFOVController ?? throw new ArgumentNullException( nameof(playerFOVController) );
        }
        
        public void Initialize()
        {
            Loop().Forget();
        }

        private async UniTask Loop( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                if ( _states.IsBlocked )
                {
                    CurrentObservable = null;
                    _playerPointsController.Clear();
                }
                else
                {
                    Vector3 position = _view.CameraFPS.transform.position;
                    Collider[] colliders = Physics.OverlapSphere( position, _config.InteractionsSettings.MaxDistance, _config.CameraVisionSettings.InteractLayers );
                    var allTargets = GetInteractables( colliders );
                    CurrentObservable = FindNearestObservable( position, allTargets );
                    
                    _playerPointsController.PointsAround( allTargets, _view.transform, _view.CameraFPS.transform );
                }
                
                await UniTask.WaitForSeconds( 0.14f, cancellationToken: cancellationToken );
            }
        }

        private List< InteractableObject > GetInteractables( Collider[] colliders )
        {
            List< InteractableObject > result = new();

            foreach ( var collider in colliders )
            {
                var interactable = collider.GetComponentInParent< InteractableObject >();
                if ( interactable == null) continue;
                if ( !_playerFOVController.IsInFOV( interactable.GetInteractPointerPosition() ) ) continue;
                
                result.Add( interactable );
            }

            return result;
        }
        
        private ObservableObject FindNearestObservable( Vector3 from, List< InteractableObject > targets )
        {
            if ( targets.Count > 0 )
            {
                ObservableObject nearest = null;
                float minSqrDistance = _config.InteractionsSettings.KeyDistanceSquared;

                foreach ( var target in targets )
                {
                    Vector3 closestPoint = target.GetInteractPointerPosition();
                    float sqrDist = ( closestPoint - from ).sqrMagnitude;

                    if ( sqrDist < minSqrDistance )
                    {
                        minSqrDistance = sqrDist;
                        nearest = target;
                    }
                }

                return nearest;
            }

            return null;
        }
    }
}