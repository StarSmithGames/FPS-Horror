using Cysharp.Threading.Tasks;
using Game.Core.Entity;
using Game.Core.World.PointerSystem;
using Game.Core.World.WorldManager;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerInteractionPointsController
    {
        private InteractionPointerDictionary _puzzlesDictionary = new();
        
        private readonly PointerSystem _pointerSystem;
        private readonly WorldManager _worldManager;
        
        public PlayerInteractionPointsController(
            PointerSystem pointerSystem,
            WorldManager worldManager
            )
        {
            _pointerSystem = pointerSystem ?? throw new ArgumentNullException( nameof(pointerSystem) );
            _worldManager = worldManager ?? throw new ArgumentNullException( nameof(worldManager) );
        }

        public void Initialize()
        {
            Loop().Forget();
        }

        private async UniTask Loop( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                for ( int i = 0; i < _worldManager.Level.Puzzles.Count; i++ )
                {
                    var puzzle = _worldManager.Level.Puzzles[ i ];
                    if ( !puzzle.IsCollidersEnabled )
                    {
                        _puzzlesDictionary.TryRemove( puzzle );
                        continue;
                    }
                    if ( _puzzlesDictionary.IsPointerShowing( puzzle ) )
                    {
                        continue;
                    }
                    if ( !_puzzlesDictionary.Contains( puzzle ) )
                    {
                        var pointer = _pointerSystem.CreateIndicator();
                        _puzzlesDictionary.TryAdd( puzzle, pointer );
                    }
                    _puzzlesDictionary.ShowPointer( puzzle );
                }
                
                Debug.LogError( "Tick" );
                
                await UniTask.WaitForSeconds( 0.2f, cancellationToken: cancellationToken );
            }
        }
    }
}