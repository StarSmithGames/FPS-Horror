using DG.Tweening;
using PuzzlescapeGames.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI.GameScreen
{
    public sealed class UITargetHand : MonoBehaviour
    {
        [ SerializeField ] private Image _targetHand;
        [ SerializeField ] private Sprite _hand1;
        [ SerializeField ] private Sprite _hand2;
        
        private bool _isTargetHandShowing;
        private Tween _targetHandTween;
        
        public void Disable()
        {
            _targetHand.SetAlpha( 0 );
        }

        public void SetHand( bool isGrab )
        {
            _targetHand.sprite = isGrab ? _hand1 : _hand2;
        }
        
        public void EnableTargetHand( bool trigger ) 
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

        private void Show()
        {
            if ( _isTargetHandShowing ) return;
            _isTargetHandShowing = true;
            
            _targetHandTween?.Kill();
            _targetHandTween = _targetHand.DOFade( 1f, 0.48f ).SetEase( Ease.OutQuad );
        }

        private void Hide()
        {
            if ( !_isTargetHandShowing ) return;
            _isTargetHandShowing = false;
            
            _targetHandTween?.Kill();
            _targetHandTween = _targetHand.DOFade( 0f, 0.24f ).SetEase( Ease.OutQuad );
        }
    }
}