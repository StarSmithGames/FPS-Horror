using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Managers.InputManager
{
    public sealed class InputActionProvider
    {
        public bool IsInProcess { get; private set; }
        
        private CancellationTokenSource _cancellationTokenSource;
        
        private readonly InputAction _input;
        private readonly Action _action;
        private readonly float _duration;
        private readonly Action _onStartHold;
        private readonly Action _onEndHold;
        private readonly Func< bool > _breaker = null;
        private readonly Action< float > _progress = null;
        private readonly Action< bool > _callback = null;
        
        public InputActionProvider(
            InputAction input,
            Action action,
            float duration = 0.24f,
            Action onStartHold = null,
            Action onEndHold = null,
            Func< bool > breaker = null,
            Action< float > progress = null,
            Action< bool > callback = null
            )
        {
            _input = input ?? throw new ArgumentNullException( nameof(input) );
            _action = action ?? throw new ArgumentNullException( nameof(action) );
            _duration = duration;
            _onStartHold = onStartHold;
            _onEndHold = onEndHold;
            _breaker = breaker;
            _progress = progress;
            _callback = callback;
        }

        public void Enable()
        {
            _input.performed += InputPerformedHandler;
            _input.canceled += InputCanceledHandler;
            _input.Enable();
        }

        public void Disable()
        {
            _input.performed -= InputPerformedHandler;
            _input.canceled -= InputCanceledHandler;
            _input.Disable();

            Stop();
        }
        
        private async UniTask Tick( CancellationToken cancellationToken = default )
        {
            IsInProcess = true;
            
            float t = 0f;
            while ( t < _duration )
            {
                cancellationToken.ThrowIfCancellationRequested();
                if ( _breaker?.Invoke() ?? false )
                {
                    Stop();
                    return;
                }
                
                t += Time.deltaTime;

                _progress?.Invoke( t / _duration );
                await UniTask.Yield();
            }
            
            _progress?.Invoke( 1f );
            await UniTask.Yield();

            IsInProcess = false;
            
            _action?.Invoke();
            _callback?.Invoke( true );
        }
        
        private void Stop()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            
            if ( IsInProcess )
            {
                _callback?.Invoke( false );
            }
            IsInProcess = false;
        }
        
        private void InputPerformedHandler( InputAction.CallbackContext context )
        {
            _onStartHold?.Invoke();
            _cancellationTokenSource = new();
            Tick( _cancellationTokenSource.Token ).Forget();
        }
        
        private void InputCanceledHandler( InputAction.CallbackContext context )
        {
            Stop();
            _onEndHold?.Invoke();
        }
    }
}