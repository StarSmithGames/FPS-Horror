using Cysharp.Threading.Tasks;
using Game.Core.World.IndicatorManager;
using Game.Core.World.WorldManager;
using System;
using System.Threading;

namespace Game.Core.Player
{
    public sealed class PlayerIndicatorController
    {
        private readonly IndicatorManager _indicatorManager;
        private readonly WorldManager _worldManager;
        
        public PlayerIndicatorController(
            IndicatorManager indicatorManager,
            WorldManager worldManager
            )
        {
            _indicatorManager = indicatorManager ?? throw new System.ArgumentNullException( nameof(indicatorManager) );
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
                        continue;
                    
                }
                
                await UniTask.Yield();
            }
        }
    }
}