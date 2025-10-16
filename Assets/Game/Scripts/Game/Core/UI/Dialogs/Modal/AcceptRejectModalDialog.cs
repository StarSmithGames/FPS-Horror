using PuzzlescapeGames.VVM.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.UI.Dialogs
{
    public class AcceptRejectModalDialog : UIViewFade
    {
        public event Action OnAcceptButtonClicked;
        public event Action OnRejectButtonClicked;

        [ field: SerializeField ] public List< UIOption > Buttons { get; private set; } = new();
        
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