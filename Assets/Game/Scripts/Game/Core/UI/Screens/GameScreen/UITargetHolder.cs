using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI.GameScreen
{
    public sealed class UITargetHolder : MonoBehaviour
    {
        [ SerializeField ] private CanvasGroup _canvasGroup;
        [ SerializeField ] private Image _bar;
        
        private bool _isShowing;
        private Tween _tween;
        
        public void Disable()
        {
            _canvasGroup.alpha = 0;
        }

        public void Enable( bool trigger )
        {
            if ( trigger )
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
        
        public void Show()
        {
            if ( _isShowing ) return;
            _isShowing = true;
            
            _tween?.Kill();
            _tween = DOTween.To( () => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1, 0.24f );
        }

        public void Hide()
        {
            if ( !_isShowing ) return;
            _isShowing = false;
            
            _tween?.Kill();
            _tween = DOTween.To( () => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0, 0.24f );
        }

        public void SetProgress( float progress ) => _bar.fillAmount = progress;
    }
}