using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI
{
    public sealed class UIOptionToggle : UIOption
    {
        public event Action< bool > OnToggleClicked;
        
        [ SerializeField ] private Image _back;
        [ field: SerializeField ] public Toggle Toggle { get; private set; }
        
        public void SetBackColor( Color color )
        {
            _back.color = color;
        }
        
        public void OnToggleClick( bool result )
        {
            OnToggleClicked?.Invoke( result );
        }

        public void OnButtonClick()
        {
            Toggle.isOn = !Toggle.isOn;
        }
    }
}