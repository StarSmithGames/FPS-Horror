using Cysharp.Threading.Tasks;
using Game.Core.Entity;
using Game.Core.World.PointerSystem;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerInteractablesController
    {
        public event Action< InteractableObject > OnCurrentInteractableChanged;
        public event Action< bool > OnObservablesChanged;
        
        public InteractableObject CurrentObservable
        {
            get => _currentObservable;
            set
            {
                if ( _currentObservable != value )
                {
                    _currentObservable?.EndObserve();
                    _currentObservable = value;
                    _currentObservable?.StartObserve();
                    
                    OnCurrentInteractableChanged?.Invoke( _currentObservable );
                }
                else
                {
                    _currentObservable?.Observe();
                }
            }
        }
        private InteractableObject _currentObservable;
        
        private CancellationTokenSource _cancellationTokenSource;
        private InteractionPointerDictionary _dictionary = new();
        
        private readonly PlayerConfig _config;
        private readonly PlayerStates _states;
        private readonly PlayerObject _view;
        private readonly PlayerHeadFunctions _headFunctions;
        private readonly InteractionPointerFactory _interactionPointerFactory;

        public PlayerInteractablesController(
            PlayerConfig config,
            PlayerStates states,
            PlayerObject view,
            PlayerHeadFunctions headFunctions,
            InteractionPointerFactory interactionPointerFactory
            )
        {
            _config = config ?? throw new ArgumentNullException( nameof(config) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _headFunctions = headFunctions ?? throw new ArgumentNullException( nameof(headFunctions) );
            _interactionPointerFactory = interactionPointerFactory ?? throw new ArgumentNullException( nameof(interactionPointerFactory) );
        }
        
        public void Initialize()
        {
            _cancellationTokenSource = new();
            Tick( _cancellationTokenSource.Token ).Forget();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
        
        public InteractionPointer GetPointer( InteractableObject interactable )
        {
            if ( interactable == null ) return null;
            return _dictionary.TryGet( interactable );
        }

        private async UniTask Tick( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                if ( _states.IsBlocked )
                {
                    _dictionary.Clear();
                    
                    CurrentObservable = null;
                    OnObservablesChanged?.Invoke( false );
                }
                else
                {
                    var targetsAround = _headFunctions.GetTargetsAround();
                    var current = _headFunctions.FindBestKeyInteractable( targetsAround );
                    
                    PointsAround( targetsAround, CurrentObservable );
                    
                    CurrentObservable = current;
                    
                    OnObservablesChanged?.Invoke( _headFunctions.CastInFrontRay( out RaycastHit _ ) );
                }

                await UniTask.WaitForSeconds( 0.14f, cancellationToken: cancellationToken );
            }
        }

        private void PointsAround( List< InteractableObject > allTargets, InteractableObject keyTarget )
        {
            _dictionary.TryRemoveSubtractions( allTargets );

            var cam = _view.CameraFPS.transform;

            for ( int i = 0; i < allTargets.Count; i++ )
            {
                var t = allTargets[ i ];
                if ( t == null || !t.IsCollidersEnabled )
                {
                    _dictionary.TryRemove( t );
                    continue;
                }

                Pointing( t, t == keyTarget, cam );
            }
        }

        private void Pointing( InteractableObject target, bool isKey, Transform camera )
        {
            Vector3 delta = target.transform.position - camera.position;
            delta.y = 0f;

            float sqrDist = delta.sqrMagnitude;
            if ( sqrDist >= _config.InteractionsSettings.MaxDistanceSquared )
            {
                _dictionary.TryRemove( target );
                return;
            }

            CreateAndShowPointer();

            var p = _dictionary.TryGet( target );
            bool shouldBeKey = isKey && sqrDist < _config.InteractionsSettings.KeyDistanceSquared;

            if ( shouldBeKey ) p.HidePointShowKey();
            else p.ShowPointHideKey();
            
            void CreateAndShowPointer()
            {
                if ( !_dictionary.IsShowing( target ) )
                {
                    var pointer = _interactionPointerFactory.Create();
                    _dictionary.TryAdd( target, pointer );
                    pointer.StartLookAt( camera, target );
                    pointer.Show();
                }
            }
        }
    }
}