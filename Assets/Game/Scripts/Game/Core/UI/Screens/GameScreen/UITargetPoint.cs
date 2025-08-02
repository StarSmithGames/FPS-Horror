using DG.Tweening;
using PuzzlescapeGames.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI.GameScreen
{
    public sealed class UITargetPoint : MonoBehaviour
    {
        [ SerializeField ] private Image _targetPoint;
        
        private bool _isTargetPointShowing;
        private Tween _targetPointTween;

        public void Disable()
        {
            _targetPoint.SetAlpha( 0 );
        }
        
        public void EnableTargetPoint( bool trigger ) 
        {
            if ( trigger )
            {
                ShowTargetPoint();
            }
            else
            {
                HideTargetPoint();
            }
        }

        private void ShowTargetPoint()
        {
            if ( _isTargetPointShowing ) return;
            _isTargetPointShowing = true;
            
            _targetPointTween?.Kill();
            _targetPointTween = _targetPoint.DOFade( 1f, 0.48f ).SetEase( Ease.OutQuad );
        }

        private void HideTargetPoint()
        {
            if ( !_isTargetPointShowing ) return;
            _isTargetPointShowing = false;
            
            _targetPointTween?.Kill();
            _targetPointTween = _targetPoint.DOFade( 0f, 0.24f ).SetEase( Ease.OutQuad );
        }
    }
}