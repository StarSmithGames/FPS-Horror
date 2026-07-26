using DG.Tweening;
using Game.Managers.InputManager;
using UnityEngine;

namespace Game.Core.Player.Animations
{
    [ System.Serializable ]
    public sealed class AimAnimation
    {
        [ SerializeField ] private IdleAnimator _idleAnimator;

        private Sequence _tween;
        
        private RightHandModel _model;
        
        public void Initialize( RightHandModel model )
        {
            _model = model;
         
            _idleAnimator.Initialize( _model.Center );
        }
        
        public void StartAim()
        {
            _model.CurrentItem.transform.SetParent( _model.Center );
            _tween?.Kill();
            _tween = DOTween.Sequence();
            _tween.Append( _model.CurrentItem.transform.DOLocalMove( Vector3.zero, 0.33f ) );
            _tween.Append( _model.CurrentItem.transform.DOLocalRotate( Vector3.zero, 0.33f ) );
            _tween.OnComplete( () =>
            {
                _idleAnimator.Start();
            } );
        }

        public void StopAim()
        {
            _idleAnimator.Stop();
            
            _model.CurrentItem.transform.SetParent( _model.Root );
            _tween?.Kill();
            _tween.Append( _model.CurrentItem.transform.DOLocalMove( Vector3.zero, 0.33f ) );
            _tween.Append( _model.CurrentItem.transform.DOLocalRotate( Vector3.zero, 0.33f ) );
            _tween.OnComplete( () =>
            {
                
            } );
        }
    }
}