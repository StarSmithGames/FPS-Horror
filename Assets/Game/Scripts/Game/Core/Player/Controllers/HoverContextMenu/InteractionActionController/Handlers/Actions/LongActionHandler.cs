using Game.Core.UI;
using Game.Core.UI.GameScreen;
using Game.Managers.InputManager;
using System;

namespace Game.Core.Player
{
    public abstract class LongActionHandler : ActionHandler
    {
        protected GameScreenViewModel _gameScreenViewModel;
        
        protected readonly InputActionProvider _provider;
        protected readonly UIRootGame _uiRootGame;

        public LongActionHandler(
            UIRootGame uiRootGame,
            InputKeyAction inputKeyAction,
            float duration = 0.33f
            )
        {
            _provider = new( inputKeyAction.InputAction, Completed, duration, onStartHold: InteractStarted, progress: InteractProgress, callback: InteractFinished );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
        }

        public override void Enable()
        {
            _gameScreenViewModel = _uiRootGame.ScreenAggregator.GetOrCreateIfNotExist< GameScreenViewModel >();
            
            _provider.Enable();
            
            IsEnable = true;
        }

        public override void Disable()
        {
            _provider.Disable();

            _gameScreenViewModel = null;
            
            IsEnable = false;
        }

        protected virtual void InteractStarted() {}
        
        protected virtual void InteractProgress( float value )
        {
            _gameScreenViewModel.ModelView.TargetHolder.SetProgress( value );
        }

        protected virtual void InteractFinished( bool result )
        {
            if ( !IsEnable ) return;
            
            _gameScreenViewModel.ModelView.TargetHolder.SetProgress( 0 );
        }
    }
}