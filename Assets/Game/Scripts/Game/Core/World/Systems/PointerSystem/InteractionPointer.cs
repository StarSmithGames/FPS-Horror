using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core.Entity;
using PuzzlescapeGames.Extensions;
using PuzzlescapeGames.IoC;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.World.PointerSystem
{
    public sealed class InteractionPointer : ZenjectMonoPoolable
    {
        [ SerializeField ] private Canvas _canvas;
        [ SerializeField ] private CanvasGroup _canvasGroup;
        [ SerializeField ] private Image _frame;
        [ SerializeField ] private Image _center;
        [ SerializeField ] private TextMeshProUGUI _key;

        public bool IsShowing { get; private set; }
        public bool IsPointShowing { get; private set; }
        public bool IsKeyShowing { get; private set; }
        
        private Tween _tween;
        private Tween _tweenMorph;
        private Tween _centerLoopTween;

        private InteractableObject _target;
        private Transform _lookAtTarget;
        private CancellationTokenSource _cancellationLookAtSource;

        public void Show()
        {
            if ( IsShowing ) return;
            IsShowing = true;
            
            transform.position = _target.PointerStartPosition;
            
            _tween?.Kill( true );
            _tween = DOTween.Sequence()
                .Append( _canvasGroup.DOFade( 1f, 0.33f ) )
                .Join( transform.DOMove( _target.PointerEndPosition, 0.33f) );
        }

        public void Hide( Action callback = null )
        {
            if ( !IsShowing ) return;
            IsShowing = false;
            
            _tween?.Kill( true );
            _tween = DOTween.Sequence()
                .Append( _canvasGroup.DOFade( 0f, 0.33f ) )
                .Join( transform.DOMove( _target.PointerEndPosition, 0.33f) )
                .OnComplete( () =>
                {
                    DespawnIt();
                    callback?.Invoke();
                } );
        }
        
        public void ShowPointHideKey()
        {
            if ( IsPointShowing && !IsKeyShowing ) return;
            IsPointShowing = true;
            IsKeyShowing = false;
            
            _tweenMorph?.Kill( true );
            _tweenMorph = DOTween.Sequence()
                .Append( _key.DOFade( 0f, 0.33f ) )
                .Join( DoPixelsPerUnitMultiplier( 1f ) );
            
            CenterIdle();
        }
        
        public void HidePointShowKey()
        {
            if ( !IsPointShowing && IsKeyShowing ) return;
            IsPointShowing = false;
            IsKeyShowing = true;
            
            _tweenMorph?.Kill( true );
            _centerLoopTween?.Kill();
            
            _tweenMorph = DOTween.Sequence()
                .Append( _center.transform.DOScale( 0, 0.33f ) )
                .Join( _key.DOFade( 1f, 0.33f ) )
                .Join( DoPixelsPerUnitMultiplier( 5f ) );
        }

        #region LookAt
        public void StartLookAt( Transform lookAt, InteractableObject target )
        {
            _lookAtTarget = lookAt;
            _target = target;
            
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
                // transform.LookAt( _lookAtTarget );

                var dir = _lookAtTarget.transform.position - transform.position;
                var rot = Quaternion.LookRotation( dir );
                transform.rotation = rot * Quaternion.Euler( 0f, 180f, 0f );
                
                await UniTask.Yield( PlayerLoopTiming.LastUpdate );
            }
        }
        #endregion

        private Tween DoPixelsPerUnitMultiplier( float target, float duration = 0.33f )
        {
            return DOTween.To( () => _frame.pixelsPerUnitMultiplier, ( x ) => _frame.pixelsPerUnitMultiplier = x, target, duration );
        }

        private void CenterIdle()
        {
            _centerLoopTween?.Kill();
            _centerLoopTween = _center.transform.DOScale( 0.65f, 0.99f )
                .From( 0.25f )
                .SetEase( Ease.InOutSine )
                .SetLoops( -1, LoopType.Yoyo );
        }
    }
}