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
        
        public override void Select()
        {
            base.Select();
            
            _back.color = new( 1, 1, 1, 0.3f );
        }

        public override void Deselect()
        {
            base.Deselect();
            
            _back.color = new( 1, 1, 1, 0.0f );
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