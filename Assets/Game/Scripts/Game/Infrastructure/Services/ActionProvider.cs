using Cysharp.Threading.Tasks;
using Game.Core.Player;
using Game.Core.UI;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Servies
{
    public sealed class ActionProvider
    {
        private Action _action;
        private Func< bool > _breaker;
        private Action< float > _progress;
        private Action< bool > _callback;
        private CancellationTokenSource _cancellationTokenSource;

        public void ProvideAction(
            Action action,
            float duration = 0.33f,
            Func< bool > breaker = null,
            Action< float > progress = null,
            Action< bool > callback = null )
        {
            _action = action;
            _breaker = breaker;
            _progress = progress;
            _callback = callback;
            
            _cancellationTokenSource = new();
            Tick( duration, _cancellationTokenSource.Token ).Forget();
        }
        
        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private async UniTask Tick( float duration = 0.33f, CancellationToken cancellationToken = default )
        {
            float t = 0f;
            while ( t < duration )
            {
                cancellationToken.ThrowIfCancellationRequested();
                if ( _breaker.Invoke() )
                {
                    Stop();
                    _callback?.Invoke( false );
                    return;
                }
                
                t += Time.deltaTime;

                _progress?.Invoke( t / duration );
                await UniTask.Yield();
            }
            
            _progress?.Invoke( 1f );
            await UniTask.Yield();

            _action?.Invoke();
            _callback?.Invoke( true );
        }
    }
}