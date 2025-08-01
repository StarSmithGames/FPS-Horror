using DG.Tweening;
using PuzzlescapeGames.Extensions;
using PuzzlescapeGames.VVM;
using System;

namespace Game.Core.UI
{
    public sealed class GameScreenViewModel : ViewModel< UIGameScreen >
    {
        private bool _isTargetPointShowing;
        private Tween _targetPointTween;
        
        private readonly UIRootGame _uiRootGame;
        
        public GameScreenViewModel( UIRootGame uiRootGame )
        {
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            
            CreateView();
            ModelView.TargetPoint.SetAlpha( 0f );
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
            _targetPointTween = ModelView.TargetPoint.DOFade( 1f, 0.48f ).SetEase( Ease.OutQuad );
        }

        private void HideTargetPoint()
        {
            if ( !_isTargetPointShowing ) return;
            _isTargetPointShowing = false;
            
            _targetPointTween?.Kill();
            _targetPointTween = ModelView.TargetPoint.DOFade( 0f, 0.24f ).SetEase( Ease.OutQuad );
        }
        
        protected override UIGameScreen GetView() => _uiRootGame.GameScreen;
    }
}