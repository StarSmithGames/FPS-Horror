using PuzzlescapeGames.VVM;
using System;

namespace Game.Core.UI.MenuScreen
{
    public sealed class MenuScreenViewModel : ViewModel< UIMenuScreen >
    {
        private readonly UIRootGame _uiRootGame;
        
        public MenuScreenViewModel( UIRootGame uiRootGame )
        {
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            
            CreateView();
        }

        protected override UIMenuScreen GetView() => _uiRootGame.MenuScreen;
    }
}