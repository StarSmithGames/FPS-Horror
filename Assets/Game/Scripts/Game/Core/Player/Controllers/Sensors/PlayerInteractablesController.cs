using Cysharp.Threading.Tasks;
using Game.Core.Entity;
using Game.Core.World.PointerSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerInteractablesController
    {
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

        private async UniTask Tick( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                if ( _states.IsBlocked )
                {
                    _dictionary.Clear();
                    
                    // CurrentObservable = null;
                    // OnObservablesChanged?.Invoke( false );
                }
                else
                {
                    // bool isHasCollidersAround = _headFunctions.CastInFrontRay( out RaycastHit hit );
                    var targetsAround = _headFunctions.GetTargetsAround();
                    var targetNearestAround = _headFunctions.FindBestKeyInteractable( targetsAround );
                    
                    PointsAround( targetsAround, targetNearestAround );
                }

                await UniTask.WaitForSeconds( 0.14f, cancellationToken: cancellationToken );
            }
        }

        private void PointsAround( List< InteractableObject > allTargets, InteractableObject targetNearestAround )
        {
            _dictionary.TryRemoveSubtractions( allTargets );
            
            for ( int i = 0; i < allTargets.Count; i++ )
            {
                var target = allTargets[ i ];
                if( target == null ) continue;
                if ( !target.IsCollidersEnabled )
                {
                    _dictionary.TryRemove( target );
                    continue;
                }
                
                Pointing( target, target == targetNearestAround, _view.CameraFPS.transform );
            }
        }

        private void Pointing( InteractableObject target, bool isKey, Transform camera )
        {
            Vector3 delta = target.transform.position - camera.position;
            delta.y = 0f;
            float sqrMagnitude = delta.sqrMagnitude;
            if ( sqrMagnitude < _config.InteractionsSettings.MaxDistanceSquared )//if root is close enough
            {
                if ( _dictionary.IsShowing( target ) )
                {
                    var pointer = _dictionary.Get( target );
                    if ( sqrMagnitude < _config.InteractionsSettings.KeyDistanceSquared && isKey )
                    {
                        pointer.HidePointShowKey();
                    }
                    else
                    {
                        pointer.ShowPointHideKey();
                    }
                    
                    return;
                }

                CreateAndShowPointer();
            }
            else
            {
                if ( _dictionary.IsShowing( target ) )
                {
                    _dictionary.TryRemove( target );
                }
            }
            
            void CreateAndShowPointer()
            {
                if ( !_dictionary.Contains( target ) )
                {
                    var pointer = _interactionPointerFactory.Create();
                    _dictionary.TryAdd( target, pointer );
                    
                    pointer.StartLookAt( camera, target );
                    pointer.Show();
                    if ( sqrMagnitude < _config.InteractionsSettings.KeyDistanceSquared && isKey )
                    {
                        pointer.HidePointShowKey();
                    }
                    else
                    {
                        pointer.ShowPointHideKey();
                    }
                }
            }
        }
    }
}