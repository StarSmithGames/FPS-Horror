using System;
using TMPro;
using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class UIOptionLeftRight : UIOption
    {
        public event Action OnLeftButtonClicked;
        public event Action OnRightButtonClicked;

        [ SerializeField ] private TextMeshProUGUI _text;
        [ SerializeField ] private GameObject _buttonLeft;
        [ SerializeField ] private GameObject _buttonRight;
        
        public void EnableButtons( bool trigger )
        {
            _buttonLeft.SetActive( trigger );
            _buttonRight.SetActive( trigger );
        }

        public void SetText( string text )
        {
            _text.text = text;
        }

        public void OnLeftButtonClick()
        {
            OnLeftButtonClicked?.Invoke();
        }
        
        public void OnRightButtonClick()
        {
            OnRightButtonClicked?.Invoke();
        }
    }
}