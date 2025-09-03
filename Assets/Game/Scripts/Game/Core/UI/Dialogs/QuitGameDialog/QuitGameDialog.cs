using PuzzlescapeGames.VVM.UI;
using System;

namespace Game.Core.UI.QuitGameDialog
{
    public sealed class QuitGameDialog : UIViewFade
    {
        public event Action OnAcceptButtonClicked;
        public event Action OnRejectButtonClicked;
        
        public void OnAcceptButtonClick()
        {
            OnAcceptButtonClicked?.Invoke();
        }
        
        public void OnRejectButtonClick()
        {
            OnRejectButtonClicked?.Invoke();
        }
    }
}