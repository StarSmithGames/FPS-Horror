using DG.Tweening;
using Game.Managers.InputManager;
using UnityEngine;

namespace Game.Core.Player.Animations
{
    [ System.Serializable ]
    public sealed class AimAnimation
    {
        [ SerializeField ] private IdleAnimator _idleAnimator;
        
        private InputActionVoidWrap _inputActionMenu;

        private Sequence _tween;
        
        private RightHandModel _model;
        
        public void Initialize( RightHandModel model )
        {
            _model = model;
         
            _idleAnimator.Initialize( _model.Center );
            _inputActionMenu = new( InputManager.Inputs.Player.Aim, AimStartedHandler, AimStoppedHandler );
        }

        public void Enable()
        {
            _inputActionMenu.Enable();
        }

        public void Disable()
        {
            _inputActionMenu.Disable();
        }
        
        private void AimStartedHandler()
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

        private void AimStoppedHandler()
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