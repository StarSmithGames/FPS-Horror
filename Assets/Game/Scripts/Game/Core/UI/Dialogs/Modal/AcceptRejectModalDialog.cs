using PuzzlescapeGames.VVM.UI;
using System;

namespace Game.Core.UI.Dialogs
{
    public class AcceptRejectModalDialog : UIViewFade
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