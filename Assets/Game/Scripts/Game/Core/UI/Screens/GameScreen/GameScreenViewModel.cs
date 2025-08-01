using Game.Core.Entity;
using Game.Core.World.InteractionSystem;
using PuzzlescapeGames.VVM;
using System;

namespace Game.Core.UI
{
    public sealed class GameScreenViewModel : ViewModel< UIGameScreen >
    {
        private bool _isShowingInformer;

        private IObservable _currentObservable;
        
        private readonly UIRootGame _uiRootGame;
        
        public GameScreenViewModel( UIRootGame uiRootGame )
        {
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            
            CreateView();
            ModelView.TargetPoint.Disable();
            ModelView.TargetInformer.Enable( false );
        }
        
        public void SetObservable( IObservable observable )
        {
            if ( _currentObservable == observable ) return;
            _currentObservable = observable;

            ModelView.TargetInformer.Button1.SetFillAmount( 0f );
            ModelView.TargetInformer.Button1.gameObject.SetActive( false );
            ModelView.TargetInformer.Button2.SetFillAmount( 0f );
            ModelView.TargetInformer.Button2.gameObject.SetActive( false );
            
            if ( _currentObservable == null )
            {
                if ( _isShowingInformer )
                {
                    _isShowingInformer = false;
                    ModelView.TargetInformer.Hide();
                }
            }
            else
            {
                if ( _currentObservable is PickableEntityObject pickable )
                {
                    ModelView.TargetInformer.Name.text = "Pickable";
                    ModelView.TargetInformer.Button1.gameObject.SetActive( true );
                    ModelView.TargetInformer.Button2.gameObject.SetActive( false );

                    if ( !_isShowingInformer )
                    {
                        _isShowingInformer = true;
                        ModelView.TargetInformer.Show();
                    }
                }   
            }
        }
        
        protected override UIGameScreen GetView() => _uiRootGame.GameScreen;
    }
}