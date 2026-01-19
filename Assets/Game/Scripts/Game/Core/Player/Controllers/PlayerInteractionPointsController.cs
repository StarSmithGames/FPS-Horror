using Cysharp.Threading.Tasks;
using Game.Core.Entity;
using Game.Core.World.PointerSystem;
using Game.Core.World.WorldManager;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerInteractionPointsController
    {
        private const float MAX_DISTANCE = 3f * 3f;
        private const float KEY_DISTANCE = 1.5f * 1.5f;
        
        
        private InteractionPointerDictionary _puzzlesDictionary = new();
        
        private readonly PointerSystem _pointerSystem;
        private readonly WorldManager _worldManager;
        private readonly PlayerObject _view;
        
        public PlayerInteractionPointsController(
            PointerSystem pointerSystem,
            WorldManager worldManager,
            PlayerObject view
            )
        {
            _pointerSystem = pointerSystem ?? throw new ArgumentNullException( nameof(pointerSystem) );
            _worldManager = worldManager ?? throw new ArgumentNullException( nameof(worldManager) );
            _view = view ?? throw new ArgumentNullException( nameof(view) );
        }

        public void Initialize()
        {
            Loop().Forget();
        }

        private async UniTask Loop( CancellationToken cancellationToken = default )
        {
            // List< InteractableObject >
            
            while ( !cancellationToken.IsCancellationRequested )
            {
                for ( int i = 0; i < _worldManager.Level.Items.Count; i++ )
                {
                    var item = _worldManager.Level.Items[ i ];
                    Pointing( item );
                }
                
                for ( int i = 0; i < _worldManager.Level.Puzzles.Count; i++ )
                {
                    var puzzle = _worldManager.Level.Puzzles[ i ];
                    Pointing( puzzle );
                }
                
                await UniTask.WaitForSeconds( 0.2f, cancellationToken: cancellationToken );
            }
        }

        private void Pointing( InteractableObject target )
        {
            if ( !target.IsCollidersEnabled )
            {
                _puzzlesDictionary.TryRemove( target );
                return;
            }

            Vector3 delta = target.PointerStartPosition - _view.transform.position;
            delta.y = 0f;
            float sqrMagnitude = delta.sqrMagnitude;
            if ( sqrMagnitude < MAX_DISTANCE )//if player is close enough
            {
                if ( _puzzlesDictionary.IsPointerShowing( target ) )
                {
                    var pointer = _puzzlesDictionary.Get( target );
                    if ( sqrMagnitude < KEY_DISTANCE )
                    {
                        pointer.ShowKey();
                    }
                    else
                    {
                        pointer.HideKey();
                    }
                    
                    return;
                }
                        
                //show pointer
                if ( !_puzzlesDictionary.Contains( target ) )
                {
                    var pointer = _pointerSystem.CreateIndicator();
                    _puzzlesDictionary.TryAdd( target, pointer );
                    
                    if ( sqrMagnitude < KEY_DISTANCE )
                    {
                        pointer.ShowKey();
                    }
                    else
                    {
                        pointer.HideKey();
                    }
                }
                _puzzlesDictionary.ShowPointer( target, _view.CameraFPS.transform );
            }
            else
            {
                if ( _puzzlesDictionary.IsPointerShowing( target ) )
                {
                    _puzzlesDictionary.TryRemove( target );
                }
            }
        }
    }
}