using PuzzlescapeGames.VVM;
using System;

namespace Game.Core.UI
{
    public sealed class GameScreenViewModel : ViewModel< UIGameScreen >
    {
        private readonly UIRootGame _uiRootGame;
        
        public GameScreenViewModel( UIRootGame uiRootGame )
        {
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            
            CreateView();
        }
        
        protected override UIGameScreen GetView() => _uiRootGame.GameScreen;
    }
}