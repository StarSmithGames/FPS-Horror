using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core.Entity;
using PuzzlescapeGames.Extensions;
using PuzzlescapeGames.IoC;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Core.World.PointerSystem
{
    public sealed class InteractionPointer : ZenjectMonoPoolable
    {
        [ SerializeField ] private Canvas _canvas;
        [ SerializeField ] private CanvasGroup _canvasGroup;
        [ SerializeField ] private Image _center;

        public bool IsShowing { get; private set; }
        
        private Tween _tween;
        private Tween _centerLoopTween;
        
        private Transform _lookAtTarget;
        private CancellationTokenSource _cancellationLookAtSource;
        
        public void Show( InteractableObject target, Action callback = null )
        {
            if ( IsShowing ) return;
            IsShowing = true;
            
            transform.position = target.PointerStartPosition;

            _tween?.Kill( true );
            _tween = DOTween.Sequence()
                .Append( _canvasGroup.DOFade( 1f, 0.33f ) )
                .Join( transform.DOMove( target.PointerEndPosition, 0.33f) )
                .OnComplete( () => callback?.Invoke() );
            
            _centerLoopTween?.Kill();
            _centerLoopTween = _center.transform.DOScale( 0.65f, 0.99f )
                .From( 0.25f )
                .SetEase( Ease.InOutSine )
                .SetLoops( -1, LoopType.Yoyo );
        }
        
        public void Hide( Action callback = null )
        {
            if ( !IsShowing ) return;
            IsShowing = false;
            
            _tween?.Kill( true );
            _centerLoopTween?.Kill();
            
            _tween = DOTween.Sequence()
                .Append( _canvasGroup.DOFade( 0f, 0.33f ) )
                .OnComplete( () =>
                {
                    DespawnIt();
                    callback?.Invoke();
                } );
        }

        public void StartLookAt( Transform target )
        {
            _lookAtTarget = target;
            
            _cancellationLookAtSource = new();
            LookAt( _cancellationLookAtSource.Token ).Forget();
        }

        public void StopLookAt()
        {
            _cancellationLookAtSource?.Cancel();
            _cancellationLookAtSource?.Dispose();
            _cancellationLookAtSource = null;
        }
        
        private async UniTask LookAt( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                transform.LookAt( _lookAtTarget );
                
                await UniTask.Yield( PlayerLoopTiming.LastUpdate );
            }
        }
    }
}