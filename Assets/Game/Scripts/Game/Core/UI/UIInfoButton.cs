using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI
{
    public sealed class UIInfoButton : MonoBehaviour
    {
        public event Action OnButtonClicked;
        
        [ SerializeField ] private TMPro.TextMeshProUGUI _buttonName;
        [ SerializeField ] private Image _buttonProgress;
        [ SerializeField ] private TMPro.TextMeshProUGUI _buttonActionText;

        public void Set( string key, string name )
        {
            _buttonName.text = key;
            _buttonActionText.text = name;
        }

        public void SetFillAmount( float value )
        {
            _buttonProgress.fillAmount = value;
        }

        public void OnButtonClick()
        {
            OnButtonClicked?.Invoke();
        }
    }
}