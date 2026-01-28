using DG.Tweening;
using Game.Core.Entity;
using Game.Core.Player.Animations;
using PuzzlescapeGames.Extensions;
using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerRightHandObject : MonoBehaviour
    {
        [ SerializeField ] private Transform _root;
        [ SerializeField ] private Transform _center;
        
        [ SerializeField ] private IdleAnimation _idleAnimation;
        [ SerializeField ] private AimAnimation _aimAnimation;
        
        [ Header( "Show / Hide" ) ]
        [ SerializeField ] private Vector3 _hiddenOffset = new( 0f, -0.20f, 0.10f );
        [ SerializeField, Range( 0.05f, 1f ) ] private float _showDuration = 0.18f;
        [ SerializeField, Range( 0.05f, 1f ) ] private float _hideDuration = 0.16f;
        [ SerializeField ] private Ease _showEase = Ease.OutCubic;
        [ SerializeField ] private Ease _hideEase = Ease.InCubic;

        public RightHandModel Model { get; private set; }
        
        private Sequence _showHideTween;

        public void Start()
        {
            Model = new();
            Model.Root = _root;
            Model.Center = _center;
            Model.BasePos = _root.localPosition;
            Model.BaseRot = _root.localRotation;
            _idleAnimation.Initialize( Model );
            _aimAnimation.Initialize( Model );
        }

        public void ClearAndReset()
        {
            Model.Root.DestroyChildren();
            _idleAnimation.Reset();
            
            Model.Root.localPosition = Model.BasePos;
            Model.Root.localRotation = Model.BaseRot;
        }

        private void OnItemShowed()
        {
            _idleAnimation.PlayIdle();
            
            _aimAnimation.Enable();
        }

        private void OnItemHided()
        {
            ClearAndReset();
            
            _aimAnimation.Disable();
        }

        #region Show Hide
        public void DoAddItem( ItemObject item )
        {
            _idleAnimation.StopIdle();

            if ( Model.CurrentItem != null || Model.CurrentItem == item )
            {
                Hide( () =>
                {
                    OnItemHided();

                    if ( Model.CurrentItem != null )
                    {
                        Model.CurrentItem = item;
                        Model.CurrentItem.transform.SetParent( Model.Root );
                        Model.CurrentItem.transform.localPosition = Vector3.zero;
                        Model.CurrentItem.transform.localRotation = Quaternion.identity;

                        Show( 0.33f, OnItemShowed );
                    }
                    else
                    {
                        Model.CurrentItem = null;
                    }
                } );
                return;
            }

            Model.CurrentItem = item;
            Model.CurrentItem.transform.SetParent( Model.Root );
            Model.CurrentItem.transform.localPosition = Vector3.zero;
            Model.CurrentItem.transform.localRotation = Quaternion.identity;

            Show( callback: OnItemShowed );
        }

        public void DoRemoveItem()
        {
            if ( Model.CurrentItem == null ) return;

            Hide( OnItemHided );
        }

        private void Show( float delay = 0, Action callback = null )
        {
            Model.Root.localPosition = Model.BasePos + _hiddenOffset;
            Model.Root.localRotation = Model.BaseRot;

            var startScale = Model.Root.localScale;
            Model.Root.localScale = startScale * 0.96f;

            _showHideTween?.Kill();
            _showHideTween = DOTween.Sequence();
            if ( delay > 0 )
            {
                _showHideTween.AppendInterval( delay );
            }

            _showHideTween
                .Append( Model.Root.DOLocalMove( Model.BasePos, _showDuration ).SetEase( _showEase ) )
                .Join( Model.Root.DOScale( startScale, _showDuration ).SetEase( _showEase ) )
                .OnComplete( () => callback?.Invoke() );
        }

        private void Hide( Action callback = null )
        {
            var targetPos = Model.BasePos + _hiddenOffset;
            var startScale = Model.Root.localScale;

            _showHideTween?.Kill();
            _showHideTween = DOTween.Sequence()
                .Append( Model.Root.DOLocalMove( targetPos, _hideDuration ).SetEase( _hideEase ) )
                .Join( Model.Root.DOScale( startScale * 0.96f, _hideDuration ).SetEase( _hideEase ) )
                .OnComplete( () => callback?.Invoke() );
        }
        #endregion
    }
}