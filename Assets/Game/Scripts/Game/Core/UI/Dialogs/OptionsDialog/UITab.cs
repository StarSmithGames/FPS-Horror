using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class UITab : MonoBehaviour
    {
        public event Action< UITab > OnButtonClicked;

        [ SerializeField ] private Image _back;
        
        public void Select()
        {
            _back.gameObject.SetActive( true );
        }

        public void Deselect()
        {
            _back.gameObject.SetActive( false );
        }
        
        public void OnButtonClick()
        {
            OnButtonClicked?.Invoke( this );
        }
    }
}