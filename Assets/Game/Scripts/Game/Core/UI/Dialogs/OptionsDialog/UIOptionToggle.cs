using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class UIOptionToggle : UIOption
    {
        public event Action< bool > OnToggleClicked;
        
        [ field: SerializeField ] public Toggle Toggle { get; private set; }
        
        public void OnToggleClick( bool result )
        {
            OnToggleClicked?.Invoke( result );
        }
    }
}